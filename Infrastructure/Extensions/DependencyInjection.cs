using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RishtanataDbContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!));
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IInvitationEmailService, InvitationEmailService>();
        services.AddScoped<IMarriageFormNotificationService, MarriageFormNotificationService>();

        return services;
    }
}
