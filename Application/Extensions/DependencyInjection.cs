using Application.EventHandlers;
using Application.Interfaces;
using Application.Services;
using Domain.Abstractions;
using Domain.Events;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Services.Auth;

namespace Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICookieAuthenticationService, CookieAuthenticationService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFormApplicationService, FormApplicationService>();
        services.AddScoped<IMarriageApplicationFormService, MarriageApplicationFormService>();
        services.AddScoped<IMarriageApplicationFormDetailService, MarriageApplicationFormDetailService>();
        services.AddScoped<IBrideSectionService, BrideSectionService>();
        services.AddScoped<IBridegroomSectionService, BridegroomSectionService>();
        services.AddScoped<IBridegroomService, BridegroomService>();
        services.AddScoped<IAqeeqahCertificateService, AqeeqahCertificateService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<IRishtanataSecretaryService, RishtanataSecretaryService>();
        services.AddScoped<IJamaatPresidentService, JamaatPresidentService>();
        services.AddScoped<IBrideGuardianService, BrideGuardianService>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<IMarriageFormWorkflowService, MarriageFormWorkflowService>();
        services.AddScoped<IJamaatMemberService, JamaatMemberService>();
        services.AddScoped<IStageAuthorizationService, StageAuthorizationService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEventHandler<MarriageFormStageRevertedEvent>, MarriageFormStageRevertedEventHandler>();

        return services;
    }
}
