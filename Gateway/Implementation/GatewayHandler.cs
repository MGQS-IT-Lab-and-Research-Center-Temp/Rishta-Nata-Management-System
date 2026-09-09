using Application.Interfaces.Gateway;
using Domain.Entities;
using Infrastructure.Identity.Tokens;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using Infrastructure.Identity.Users;
using Gateway.Extensions;

namespace Gateway.Implementation;

public class GatewayHandler : IGatewayHandler
{
    private readonly HttpClient _client;
    private readonly string _apiUrl;

    public GatewayHandler(HttpClient client, IConfiguration config)
    {
        _client = client;
        _apiUrl = config["TajneedApiBaseUrl"] ?? throw new InvalidOperationException("TajneedApiBaseUrl is not configured");
    }

    public async Task<JamaatMember?> GetMemberByMemberNoAsync(string memberNo, CancellationToken cancellationToken = default)
    {
        var url = $"{_apiUrl}members/{memberNo}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        var response = await _client.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var member = DeserializeMember(content);

            // The Tajneed member payload does not (reliably) carry the chanda
            // number, so stamp it from the identifier we queried with.
            if (member is not null)
            {
                member.ChandaNo = memberNo;
            }

            return member;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new HttpRequestException($"Member API returned {(int)response.StatusCode} ({response.StatusCode}).");
    }

    public async Task<(MemberApiLoginResponse?, string? ErrorMessage)> GenerateToken(TokenRequest tokenRequest)
    {
        var url = $"{_apiUrl}token";

        var credentials = new TokenConstant
        {
            Username = tokenRequest.ChandaNo,
            Password = tokenRequest.Password
        };

        var jsonContent = new StringContent(
            JsonConvert.SerializeObject(credentials), 
            Encoding.UTF8, 
            "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = jsonContent
        };

        // POST /token flows through the same resilient client as the member
        // endpoint (AddStandardResilienceHandler), so the attempt timeout and
        // circuit breaker apply here too.
        var response = await _client.SendAsync(request, CancellationToken.None);

        var errorContent = await response.Content.ReadAsStringAsync();

        if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Unauthorized or HttpStatusCode.NotFound)
        {
            var errorData = JsonConvert.DeserializeObject<ApiErrorResponse>(errorContent);
            var errorMessage = errorData?.Message ?? $"Authentication failed: {(int)response.StatusCode} {response.StatusCode}";

            return (null, errorMessage);
        }

        // Server-side failures (5xx) indicate the Tajneed service itself is
        // unhealthy => surface as an HttpRequestException so the circuit breaker
        // counts them and AuthService maps them to "service temporarily
        // unreachable". Other non-success codes stay as specific error tuples.
        if ((int)response.StatusCode >= 500)
        {
            throw new HttpRequestException($"Token API returned {(int)response.StatusCode} ({response.StatusCode}).");
        }

        if (!response.IsSuccessStatusCode)
        {
            return (null, $"Token API returned {(int)response.StatusCode} ({response.StatusCode}). Response: {errorContent}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var successData = JsonConvert.DeserializeObject<MemberApiLoginResponse>(content);

        return (successData, null);
    }

    /// <summary>
    /// Parses a member payload that may be returned either as the raw member
    /// object or wrapped inside the API's { message, status, data } envelope.
    /// </summary>
    private static JamaatMember? DeserializeMember(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        var token = JToken.Parse(content);

        // Guard against an array body (the API returns a string array for
        // validation errors / not-found cases).
        if (token is JArray)
        {
            return null;
        }

        if (token is JObject obj &&
            obj.TryGetValue("data", StringComparison.OrdinalIgnoreCase, out var data) &&
            data.Type == JTokenType.Object)
        {
            token = data;
        }

        // The Tajneed member payload may send null for non-nullable value-type
        // fields (e.g. dateOfBirth for a member with no recorded DOB). With the
        // default settings Newtonsoft throws "Null object cannot be converted
        // to a value type". Ignoring nulls leaves such members at their default,
        // which downstream code already treats as "unknown" (DateOfBirth.Year > 1).
        var serializer = JsonSerializer.Create(
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        return token.ToObject<JamaatMember>(serializer);
    }
}