using Application.Interfaces;
using Infrastructure.DTOs.MarriageApplicationFormDetail;
using Infrastructure.Mapper;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services;

/// <summary>
/// Assembles MarriageApplicationFormDetailDto for display (Epic C3).
///
/// CanCurrentUserEdit is derived exclusively from IStageAuthorizationService —
/// the same authorization logic Epic B endpoints use — never a
/// re-implementation (policy §7.3). When the user is unauthenticated or the
/// form has not entered the staged workflow yet, the flag is false.
/// </summary>
public class MarriageApplicationFormDetailService : IMarriageApplicationFormDetailService
{
    private readonly RishtanataDbContext _context;
    private readonly IStageAuthorizationService _stageAuthorization;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MarriageApplicationFormDetailService(
        RishtanataDbContext context,
        IStageAuthorizationService stageAuthorization,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _stageAuthorization = stageAuthorization;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<MarriageApplicationFormDetailDto?> GetDetailAsync(
        Guid applicationFormId,
        CancellationToken cancellationToken = default)
    {
        // Controllers address forms either by the MarriageApplicationForm id
        // or by its owning FormApplication id; accept both.
        var form = await _context.MarriageApplicationForms
            .Include(f => f.MarriageApplication)
            .Include(f => f.Rejections)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                f => f.Id == applicationFormId ||
                     f.MarriageApplicationId == applicationFormId,
                cancellationToken);

        if (form is null)
        {
            return null;
        }

        var dto = MarriageApplicationFormDetailMapper.ToDetailDto(
            form, form.Rejections.ToList());

        dto.CanCurrentUserEdit = await ComputeCanCurrentUserEditAsync(form, cancellationToken);

        return dto;
    }

    /// <summary>
    /// "Can the current user act on this form right now?" — answered by the
    /// Epic B authorization service for the stage the form is currently at.
    /// </summary>
    private async Task<bool> ComputeCanCurrentUserEditAsync(
        Domain.Entities.MarriageApplicationForm form,
        CancellationToken cancellationToken)
    {
        if (!form.CurrentStage.HasValue)
        {
            // The form has not entered the staged workflow: nobody can act on
            // a section yet.
            return false;
        }

        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            return false;
        }

        var result = await _stageAuthorization.CanUserActAsync(
            userId.Value,
            form.Id,
            form.CurrentStage.Value,
            cancellationToken);

        return result.IsAllowed;
    }

    private Guid? GetCurrentUserId()
    {
        var value = _httpContextAccessor.HttpContext?.User
            ?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(value, out var id) ? id : null;
    }
}