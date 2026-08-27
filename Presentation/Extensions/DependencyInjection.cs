using Application.Interfaces;
using Application.Interfaces.Identity;
using Application.Services;
using Domain.Interfaces;
using Gateway.Implementation;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Presentation.Constants.Roles;
using Presentation.Services.Auth;
using Domain.Abstractions;
using Domain.Events;
using Application.EventHandlers;

namespace Presentation.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMySQLServer<RishtanataDbContext>(
            configuration.GetConnectionString("DefaultConnection")!);

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<RishtanataDbContext>()
            .AddDefaultTokenProviders();

        // services.AddScoped<INotificationService, NotificationService>(); -- uncomment me...later!
        services.AddScoped<IFormApplicationService, FormApplicationService>();
        services.AddScoped<IMarriageApplicationFormService, MarriageApplicationFormService>();
        services.AddScoped<IStageAuthorizationService, StageAuthorizationService>();
        services.AddScoped<IMarriageApplicationFormDetailService, MarriageApplicationFormDetailService>();
        services.AddScoped<IBridegroomService, BridegroomService>();
        services.AddScoped<IAqeeqahCertificateService, AqeeqahCertificateService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<IRishtanataSecretaryService, RishtanataSecretaryService>();
        services.AddScoped<IJamaatPresidentService, JamaatPresidentService>();
        services.AddScoped<IBrideGuardianService, BrideGuardianService>();
        services.AddScoped<ICookieAuthenticationService, CookieAuthenticationService>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IInvitationEmailService, InvitationEmailService>();
        services.AddScoped<IMarriageFormNotificationService, MarriageFormNotificationService>();
        services.AddScoped<IRoleAssignmentService, RoleAssignmentService>();
        services.AddScoped<IJamaatMemberService, JamaatMemberService>();
        services.AddScoped<IStageAuthorizationService, StageAuthorizationService>();
        services.AddScoped<IMarriageApplicationFormDetailService, MarriageApplicationFormDetailService>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IMarriageFormNotificationService, MarriageFormNotificationService>();
        services.AddScoped<IEventHandler<MarriageFormStageRevertedEvent>, MarriageFormStageRevertedEventHandler>();
        services.AddScoped<IMarriageFormWorkflowService, MarriageFormWorkflowService>();
        services.AddHttpClient<IGatewayHandler, GatewayHandler>();
        services.AddHttpContextAccessor();
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
            options.AddPolicy("RequireJamaatSecretary", p => p.RequireRole(RoleNames.JamaatSecretary));
            options.AddPolicy("RequireCircuitSecretary", p => p.RequireRole(RoleNames.CircuitSecretary));
            options.AddPolicy("RequireAmir", p => p.RequireRole(RoleNames.Amir));
            options.AddPolicy("StageVerifier", p => p.RequireRole( RoleNames.RishtanataSecretary,RoleNames.JamaatSecretary,RoleNames.CircuitSecretary,
      RoleNames.Amir));

        });


        return services;
    }
}