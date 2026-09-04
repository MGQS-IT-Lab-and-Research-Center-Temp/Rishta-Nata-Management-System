using System;
using Application.Interfaces.Gateway;
using Domain.Constants;
using Gateway.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddHttpClient<IGatewayHandler, GatewayHandler>();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

    services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireRishtanataSecretary", p => p.RequireRole(RoleNames.RishtanataSecretary));
        options.AddPolicy("RequireJamaatSecretary", p => p.RequireRole(RoleNames.JamaatPresident));
        options.AddPolicy("RequireCircuitSecretary", p => p.RequireRole(RoleNames.CircuitPresident));
        options.AddPolicy("RequireAmir", p => p.RequireRole(RoleNames.Amir));
        options.AddPolicy("StageVerifier", p => p.RequireRole(RoleNames.RishtanataSecretary,RoleNames.JamaatPresident,RoleNames.CircuitPresident, RoleNames.Amir));
    });

        return services;
    }
}
