using Application.Authorization;
using Application.Interfaces;
using Application.Workflow;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

/// Implements the verification/approval chain (backlog D3). Every method:
///   1. re-checks authorization through IStageAuthorizationService for the
///      stage it is responsible for — immediately before writing, never
///      trusting a controller-level check (policy §5, backlog DoD);
///   2. persists (creates or updates) its section row on the form;
///   3. advances FormStage to the next stage in the paper-form order.
///
/// ApproveByAmirAsync additionally sets ApprovedDateOfNikah and moves the
/// form to Completed, locking it against further edits.
public class MarriageFormWorkflowService : IMarriageFormWorkflowService
{
    private readonly RishtanataDbContext _context;
    private readonly IStageAuthorizationService _stageAuthorization;
    private readonly ILogger<MarriageFormWorkflowService> _logger;

    public MarriageFormWorkflowService(
        RishtanataDbContext context,
        IStageAuthorizationService stageAuthorization,
        ILogger<MarriageFormWorkflowService> logger)
    {
        _context = context;
        _stageAuthorization = stageAuthorization;
        _logger = logger;
    }

    public async Task<StageAuthorizationResult> SubmitImamVerificationAsync(
        Guid userId,
        Guid applicationFormId,
        ImamVerificationSubmission submission,
        CancellationToken cancellationToken = default)
    {
        var (form, denied) = await AuthorizeAsync(
            userId, applicationFormId,
            MarriageFormStage.AwaitingImamVerification,
            cancellationToken);
        if (form is null)
        {
            return denied;
        }

        var now = DateTime.UtcNow;

        if (form.ImamVerification is null)
        {
            var section = new ImamVerificationSection
            {
                MarriageApplicationFormId = form.Id,
                Name = submission.Name,
                AddressJamaat = submission.AddressJamaat,
                Tel = submission.Tel,
                SignatureDate = submission.SignatureDate,
                CreatedAt = now,
                CreatedBy = userId
            };

            // Track explicitly: nav-discovery on a tracked principal can
            // misclassify a new dependent with a pre-set key as Modified.
            _context.Add(section);
            form.ImamVerification = section;
        }
        else
        {
            form.ImamVerification.Name = submission.Name;
            form.ImamVerification.AddressJamaat = submission.AddressJamaat;
            form.ImamVerification.Tel = submission.Tel;
            form.ImamVerification.SignatureDate = submission.SignatureDate;
            form.ImamVerification.ModifiedAt = now;
            form.ImamVerification.ModifiedBy = userId;
        }

        return await AdvanceAsync(
            form, userId, now,
            MarriageFormStage.AwaitingJamaatPresident,
            "imam verification");
    }

    public async Task<StageAuthorizationResult> SubmitJamaatPresidentVerificationAsync(
        Guid userId,
        Guid applicationFormId,
        JamaatPresidentVerificationSubmission submission,
        CancellationToken cancellationToken = default)
    {
        var (form, denied) = await AuthorizeAsync(
            userId, applicationFormId,
            MarriageFormStage.AwaitingJamaatPresident,
            cancellationToken);
        if (form is null)
        {
            return denied;
        }

        var now = DateTime.UtcNow;

        if (form.JamaatPresidentVerification is null)
        {
            var section = new JamaatPresidentVerificationSection
            {
                MarriageApplicationFormId = form.Id,
                Name = submission.Name,
                Tel = submission.Tel,
                SignatureDate = submission.SignatureDate,
                CreatedAt = now,
                CreatedBy = userId
            };

            _context.Add(section);
            form.JamaatPresidentVerification = section;
        }
        else
        {
            form.JamaatPresidentVerification.Name = submission.Name;
            form.JamaatPresidentVerification.Tel = submission.Tel;
            form.JamaatPresidentVerification.SignatureDate = submission.SignatureDate;
            form.JamaatPresidentVerification.ModifiedAt = now;
            form.JamaatPresidentVerification.ModifiedBy = userId;
        }

        return await AdvanceAsync(
            form, userId, now,
            MarriageFormStage.AwaitingRishtanataSecretary,
            "Jamaat president verification");
    }

    public async Task<StageAuthorizationResult> SubmitRishtanataRecommendationAsync(
        Guid userId,
        Guid applicationFormId,
        RishtanataRecommendationSubmission submission,
        CancellationToken cancellationToken = default)
    {
        var (form, denied) = await AuthorizeAsync(
            userId, applicationFormId,
            MarriageFormStage.AwaitingRishtanataSecretary,
            cancellationToken);
        if (form is null)
        {
            return denied;
        }

        var now = DateTime.UtcNow;

        if (form.RishtanataRecommendation is null)
        {
            var section = new RishtanataRecommendationSection
            {
                MarriageApplicationFormId = form.Id,
                WakeelName = submission.WakeelName,
                WakeelDeclaration = submission.WakeelDeclaration,
                SignatureDate = submission.SignatureDate,
                CreatedAt = now,
                CreatedBy = userId
            };

            _context.Add(section);
            form.RishtanataRecommendation = section;
        }
        else
        {
            form.RishtanataRecommendation.WakeelName = submission.WakeelName;
            form.RishtanataRecommendation.WakeelDeclaration = submission.WakeelDeclaration;
            form.RishtanataRecommendation.SignatureDate = submission.SignatureDate;
            form.RishtanataRecommendation.ModifiedAt = now;
            form.RishtanataRecommendation.ModifiedBy = userId;
        }

        return await AdvanceAsync(
            form, userId, now,
            MarriageFormStage.AwaitingAmirApproval,
            "Rishtanata secretary recommendation");
    }

    public async Task<StageAuthorizationResult> ApproveByAmirAsync(
        Guid userId,
        Guid applicationFormId,
        AmirApprovalSubmission submission,
        CancellationToken cancellationToken = default)
    {
        var (form, denied) = await AuthorizeAsync(
            userId, applicationFormId,
            MarriageFormStage.AwaitingAmirApproval,
            cancellationToken);
        if (form is null)
        {
            return denied;
        }

        var now = DateTime.UtcNow;

        if (form.AmirApproval is null)
        {
            var section = new AmirApprovalSection
            {
                MarriageApplicationFormId = form.Id,
                ApprovedDateOfNikah = submission.ApprovedDateOfNikah,
                SignatureDate = submission.SignatureDate,
                CreatedAt = now,
                CreatedBy = userId
            };

            _context.Add(section);
            form.AmirApproval = section;
        }
        else
        {
            form.AmirApproval.ApprovedDateOfNikah = submission.ApprovedDateOfNikah;
            form.AmirApproval.SignatureDate = submission.SignatureDate;
            form.AmirApproval.ModifiedAt = now;
            form.AmirApproval.ModifiedBy = userId;
        }

        // Final approval: record the approved Nikah date on the form itself
        // and lock the workflow
        form.ApprovedDateOfNikah = submission.ApprovedDateOfNikah;

        return await AdvanceAsync(
            form, userId, now,
            MarriageFormStage.Completed,
            "Amir approval");
    }

    // =====================================================================
    // Shared pipeline
    // =====================================================================

    /// <summary>
    /// Re-checks authorization for the required stage, then loads the form.
    /// Returns a null form together with the denial result when the request
    /// must not proceed — no entity has been touched at that point.
    /// </summary>
    private async Task<(MarriageApplicationForm? Form, StageAuthorizationResult Denied)> AuthorizeAsync(
        Guid userId,
        Guid applicationFormId,
        MarriageFormStage requiredStage,
        CancellationToken cancellationToken)
    {
        var auth = await _stageAuthorization.CanUserActAsync(
            userId, applicationFormId, requiredStage, cancellationToken);

        if (!auth.IsAllowed)
        {
            _logger.LogInformation(
                "Workflow submission blocked before any write: UserId={UserId}, ApplicationFormId={ApplicationFormId}, RequiredStage={RequiredStage}, Reason={Reason}",
                userId, applicationFormId, requiredStage, auth.Reason);

            return (null, auth);
        }

        var form = await _context.MarriageApplicationForms
            .Include(f => f.MarriageApplication)
            .FirstOrDefaultAsync(
                f => f.Id == applicationFormId ||
                     f.MarriageApplicationId == applicationFormId,
                cancellationToken);

        if (form is null)
        {
            // Authorization already resolved the form; a miss here means it
            // vanished between checks. Deny without side effects.
            return (null, StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists."));
        }

        return (form, StageAuthorizationResult.Allow());
    }

    /// <summary>Advances the stage, stamps audit fields, and saves.</summary>
    private async Task<StageAuthorizationResult> AdvanceAsync(
        MarriageApplicationForm form,
        Guid userId,
        DateTime now,
        MarriageFormStage nextStage,
        string actionLabel)
    {
        form.FormStage = nextStage;
        form.ModifiedAt = now;
        form.ModifiedBy = userId;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Workflow advanced: UserId={UserId}, ApplicationFormId={ApplicationFormId}, Action={Action}, NewStage={NewStage}",
            userId, form.Id, actionLabel, nextStage);

        return StageAuthorizationResult.Allow();
    }
}
