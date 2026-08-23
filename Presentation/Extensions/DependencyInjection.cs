using Application.Interfaces.Auth;
using Application.Interfaces.Identity;
using Application.Services;
using Application.Interfaces;
using Domain.Interfaces;
using Gateway.Implementation;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Presentation.Data;
using Presentation.Services.Auth;

namespace Presentation.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMySQLServer<RishtanataDbContext>(
            configuration.GetConnectionString("DefaultConnection")!);

        services.AddScoped<IFormApplicationService, FormApplicationService>();
        services.AddScoped<IMarriageApplicationFormService, MarriageApplicationFormService>();
        services.AddScoped<IBridegroomService, BridegroomService>();
        services.AddScoped<IAqeeqahCertificateService, AqeeqahCertificateService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<IRishtanataSecretaryService, RishtanataSecretaryService>();
        services.AddScoped<ICookieAuthenticationService, CookieAuthenticationService>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IInvitationEmailService, InvitationEmailService>();
        services.AddScoped<IJamaatMemberService,JamaatMemberService>();
        services.AddScoped<IStageAuthorizationService, StageAuthorizationService>();
        services.AddHttpClient<IGatewayHandler, GatewayHandler>();
        services.AddHttpContextAccessor();

        return services;
    }
}