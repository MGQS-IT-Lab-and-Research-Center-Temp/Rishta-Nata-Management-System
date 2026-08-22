using Application.Interfaces;
using Application.Interfaces.Identity;
using Application.Services;
using Domain.Interfaces;
using Gateway.Implementation;
using Infrastructure.Identity;         
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
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

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<RishtanataDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IFormApplicationService, FormApplicationService>();
        services.AddScoped<IAqeeqahCertificateService, AqeeqahCertificateService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<IRishtanataSecretaryService, RishtanataSecretaryService>();
        services.AddScoped<ICookieAuthenticationService, CookieAuthenticationService>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IInvitationEmailService, InvitationEmailService>();
        services.AddScoped<IRoleAssignmentService, RoleAssignmentService>();

        services.AddHttpClient<IGatewayHandler, GatewayHandler>();
        services.AddHttpContextAccessor();

        return services;
    }
}