using Application.Interfaces.Gateway;
using Domain.Constants;
using Gateway.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Presentation.Services;

namespace Presentation.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        // Timeouts are governed by the standard resilience pipeline (attempt and
        // total request timeouts), so no client.Timeout is set here.
        services.AddHttpClient<IGatewayHandler, GatewayHandler>()
            .AddStandardResilienceHandler();
        services.AddScoped<IDashboardRedirector, DashboardRedirector>();
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

        // Coarse section-fill gates — the real gate is IStageAuthorizationService,
        // which re-checks role + stage per docs/stage-authorization-policy.md §5.
        options.AddPolicy("CanFillBrideSection", p => p.RequireAuthenticatedUser());
        options.AddPolicy("CanFillBridegroomSection", p => p.RequireAuthenticatedUser());
        options.AddPolicy("CanFillGuardianOrWakeelSection", p => p.RequireAuthenticatedUser());
        options.AddPolicy("CanFillWitnessesSection", p => p.RequireAuthenticatedUser());
        options.AddPolicy("CanFillImamVerificationSection", p => p.RequireAssertion(ctx =>
            ctx.User.Claims.Any(c =>
                c.Type == ClaimTypes.Role &&
                (c.Value.Contains("imam", StringComparison.OrdinalIgnoreCase) ||
                 c.Value.Contains("missionary", StringComparison.OrdinalIgnoreCase)))));
        options.AddPolicy("CanFillJamaatPresidentSection", p => p.RequireRole(RoleNames.JamaatPresident));
        options.AddPolicy("CanFillRishtanataSection", p => p.RequireRole(RoleNames.RishtanataSecretary));
        options.AddPolicy("CanFillAmirApprovalSection", p => p.RequireRole(RoleNames.Amir));
    });

        return services;
    }
}
