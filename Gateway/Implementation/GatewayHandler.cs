using Application.Interfaces.Identity;
using Domain.Entities;
using Gateway.Extensions;
using Infrastructure.Identity.Tokens;
using Infrastructure.Identity.Users;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

    public async Task<string[]?> GetMemberRoleAsync(int chandaNo)
    {
        var url = $"{_apiUrl}{chandaNo}/userRoles";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        var response = await _client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return await response.ReadContentAs<string[]>();
        }
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new HttpRequestException($"Member roles API returned" + $"{(int)response.StatusCode} ({response.StatusCode}).");
    }

    public async Task<JamaatMember?> GetMemberByChandaNoAsync(int chandaNo)
    {
        var url = $"{_apiUrl}members/{chandaNo}";

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            url);

        var response = await _client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return await response.ReadContentAs<JamaatMember>();
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

        var credentials = new TokenConstant
        {
            Username = tokenRequest.ChandaNo,
            Password = tokenRequest.Password
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = jsonContent
        };
        
        var response = await _client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MemberApiLoginResponse>(content);
        }
        
        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new HttpRequestException(
            $"Token API returned " + $"{(int)response.StatusCode} ({response.StatusCode}).");
    }
}
