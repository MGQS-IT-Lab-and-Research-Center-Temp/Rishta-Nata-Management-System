using Application.Interfaces.Identity;
using Domain.Entities;
using Gateway.Extensions;
using Infrastructure.Identity.Tokens;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace Gateway.Implementation;

public class GatewayHandler : IGatewayHandler
{
    private readonly HttpClient _client;
    private readonly string _apiUrl;

    public GatewayHandler(HttpClient client, IConfiguration config)
    {
        _client = client;
        _apiUrl = config["Api"] ?? throw new InvalidOperationException("Api URL is not configured");
    }

    public async Task<string[]?> GetMemberRoleAsync(string chandaNo)
    {
        var url = $"{_apiUrl}{chandaNo}/userRoles";
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            url
            );

        var response = await _client.SendAsync(request);

        // The Tajneed API returns 204 No Content when the member has no roles
        // (and 404 when the member does not exist).
        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return Array.Empty<string>();
        }
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                return Array.Empty<string>();
            }

            var token = JToken.Parse(content);

            // Accept both a raw JSON array and an envelope with a "data" array.
            if (token is JObject obj &&
                obj.TryGetValue("data", StringComparison.OrdinalIgnoreCase, out var data))
            {
                token = data;
            }

            return token is JArray
                ? token.ToObject<string[]>()
                : Array.Empty<string>();
        }
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new HttpRequestException($"Member roles API returned" + $"{(int)response.StatusCode} ({response.StatusCode}).");
    }

    public async Task<JamaatMember?> GetMemberByChandaNoAsync(string chandaNo)
    {
        var url = $"{_apiUrl}members/{chandaNo}";

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            url);

        var response = await _client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var member = DeserializeMember(content);

            // The Tajneed member payload does not (reliably) carry the chanda
            // number, so stamp it from the identifier we queried with.
            if (member is not null)
            {
                member.ChandaNo = chandaNo;
            }

            return member;
        }
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new HttpRequestException($"Member API returned" + $"{(int)response.StatusCode} ({response.StatusCode}).");
    }
    public async Task<MemberApiLoginResponse?> GenerateToken(TokenRequest tokenRequest)
    {
        var url = $"{_apiUrl}token";

        // The Tajneed API expects the camelCase { userName, password } body.
        var jsonContent = new StringContent
            (JsonConvert.SerializeObject(new
            {
                userName = tokenRequest.ChandaNo,
                password = tokenRequest.Password
            }),
            Encoding.UTF8,
            "application/json");

        var request = new HttpRequestMessage
            (HttpMethod.Post,
            url)
        {
            Content = jsonContent
        };

        var response = await _client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MemberApiLoginResponse>(content);
        }

        // The Tajneed API reports invalid credentials as 400 Bad Request
        // ({"message":"Invalid Credential","status":false}), alongside the
        // conventional 401/404 — all mean "not authenticated".
        if (response.StatusCode == HttpStatusCode.BadRequest ||
            response.StatusCode == HttpStatusCode.Unauthorized ||
            response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        var errorContent = await response.Content.ReadAsStringAsync();

        throw new HttpRequestException(
            $"Token API returned {(int)response.StatusCode} ({response.StatusCode}). " +
            $"Response: {errorContent}");
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

        return token.ToObject<JamaatMember>();
    }
}