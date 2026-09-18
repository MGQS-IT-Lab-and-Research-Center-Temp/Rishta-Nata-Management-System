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
/// Chain: bride's Jamaat President → [groom's Jamaat President when different
/// Jamaats] → National Rishtanata Secretary → Amir → imam sign-off (post-
/// ceremony) → Completed (locked). ApproveByAmirAsync no longer completes the
/// form; the imam's sign-off does. The agreed Nikah date comes from the couple.
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

    public async Task<StageAuthorizationResult> SubmitJamaatPresidentVerificationAsync(
        string membershipNo,
        Guid applicationFormId,
        JamaatPresidentVerificationSubmission submission,
        CancellationToken cancellationToken = default)
    {
        var (form, memberId, denied) = await AuthorizeAsync(
            membershipNo, applicationFormId,
            MarriageFormStage.AwaitingBrideJamaatPresident,
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
                CreatedBy = memberId
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
            form.JamaatPresidentVerification.ModifiedBy = memberId;
        }

        // Mirror onto the flat form columns (used by the read side).
        form.JamaatPresidentName = submission.Name;
        form.JamaatPresidentSignatureDate = submission.SignatureDate;

        // Same-Jamaat couples: this president signs for both partners and the
        // form advances straight to the secretary. Different Jamaats: forward to
        // the groom's president first.
        var nextStage = await PartnersShareJamaatAsync(form, cancellationToken)
            ? MarriageFormStage.AwaitingRishtanataSecretary
            : MarriageFormStage.AwaitingGroomJamaatPresident;

        return await AdvanceAsync(
            form, memberId, now, nextStage, "Jamaat president verification");
    }

    public async Task<StageAuthorizationResult> SubmitGroomJamaatPresidentVerificationAsync(
        string membershipNo,
        Guid applicationFormId,
        JamaatPresidentVerificationSubmission submission,
        CancellationToken cancellationToken = default)
    {
        var (form, memberId, denied) = await AuthorizeAsync(
            membershipNo, applicationFormId,
            MarriageFormStage.AwaitingGroomJamaatPresident,
            cancellationToken);
        if (form is null)
        {
            return denied;
        }

        var now = DateTime.UtcNow;

        if (form.GroomJamaatPresidentVerification is null)
        {
            var section = new GroomJamaatPresidentVerificationSection
            {
                MarriageApplicationFormId = form.Id,
                Name = submission.Name,
                Tel = submission.Tel,
                SignatureDate = submission.SignatureDate,
                CreatedAt = now,
                CreatedBy = memberId
            };

            _context.Add(section);
            form.GroomJamaatPresidentVerification = section;
        }
        else
        {
            form.GroomJamaatPresidentVerification.Name = submission.Name;
            form.GroomJamaatPresidentVerification.Tel = submission.Tel;
            form.GroomJamaatPresidentVerification.SignatureDate = submission.SignatureDate;
            form.GroomJamaatPresidentVerification.ModifiedAt = now;
            form.GroomJamaatPresidentVerification.ModifiedBy = memberId;
        }

        form.GroomJamaatPresidentName = submission.Name;
        form.GroomJamaatPresidentSignatureDate = submission.SignatureDate;

        return await AdvanceAsync(
            form, memberId, now,
            MarriageFormStage.AwaitingRishtanataSecretary,
            "groom Jamaat president verification");
    }

    public async Task<StageAuthorizationResult> SubmitRishtanataRecommendationAsync(
        string membershipNo,
        Guid applicationFormId,
        RishtanataRecommendationSubmission submission,
        CancellationToken cancellationToken = default)
    {
        var (form, memberId, denied) = await AuthorizeAsync(
            membershipNo, applicationFormId,
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
                OfficiatingImamMembershipNo = submission.OfficiatingImamMembershipNo,
                CreatedAt = now,
                CreatedBy = memberId
            };

            _context.Add(section);
            form.RishtanataRecommendation = section;
        }
        else
        {
            form.RishtanataRecommendation.WakeelName = submission.WakeelName;
            form.RishtanataRecommendation.WakeelDeclaration = submission.WakeelDeclaration;
            form.RishtanataRecommendation.SignatureDate = submission.SignatureDate;
            form.RishtanataRecommendation.OfficiatingImamMembershipNo = submission.OfficiatingImamMembershipNo;
            form.RishtanataRecommendation.ModifiedAt = now;
            form.RishtanataRecommendation.ModifiedBy = memberId;
        }

        form.OfficiatingImamMembershipNo = submission.OfficiatingImamMembershipNo;
        form.NationalRishtanataSecretaryName = submission.WakeelName;
        form.NationalRishtanataSecretarySignatureDate = submission.SignatureDate;

        // The agreed date changes only via partner-communication to the office.
        if (submission.ApprovedDateOfNikah.HasValue)
        {
            form.ApprovedDateOfNikah = submission.ApprovedDateOfNikah.Value;
        }

        return await AdvanceAsync(
            form, memberId, now,
            MarriageFormStage.AwaitingAmirApproval,
            "Rishtanata secretary recommendation");
    }

    public async Task<StageAuthorizationResult> ApproveByAmirAsync(
        string membershipNo,
        Guid applicationFormId,
        AmirApprovalSubmission submission,
        CancellationToken cancellationToken = default)
    {
        var (form, memberId, denied) = await AuthorizeAsync(
            membershipNo, applicationFormId,
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
                SignatureDate = submission.SignatureDate,
                CreatedAt = now,
                CreatedBy = memberId
            };

            _context.Add(section);
            form.AmirApproval = section;
        }
        else
        {
            form.AmirApproval.SignatureDate = submission.SignatureDate;
            form.AmirApproval.ModifiedAt = now;
            form.AmirApproval.ModifiedBy = memberId;
        }

        form.NationalAmirOrMissionarySignatureDate = submission.SignatureDate;

        // The Amir does not invent a Nikah date — it is the date the couple agreed
        // and set, changeable only via the Rishtanata office.
        form.ApprovedDateOfNikah ??= form.ProposedNikahDate;

        // NOT Completed: the ceremony then the imam sign-off still follow.
        return await AdvanceAsync(
            form, memberId, now,
            MarriageFormStage.AwaitingImamSignoff,
            "Amir approval");
    }

    public async Task<StageAuthorizationResult> SubmitImamSignoffAsync(
        string membershipNo,
        Guid applicationFormId,
        ImamSignoffSubmission submission,
        CancellationToken cancellationToken = default)
    {
        var (form, memberId, denied) = await AuthorizeAsync(
            membershipNo, applicationFormId,
            MarriageFormStage.AwaitingImamSignoff,
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
                CreatedBy = memberId
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
            form.ImamVerification.ModifiedBy = memberId;
        }

        // Mirror onto the flat columns so every read-side mapper surfaces the
        // imam's sign-off (previously these were never written — a real gap).
        form.OfficiatingImamName = submission.Name;
        form.OfficiatingImamAddressJamaat = submission.AddressJamaat;
        form.OfficiatingImamSignatureDate = submission.SignatureDate;

        return await AdvanceAsync(
            form, memberId, now,
            MarriageFormStage.Completed,
            "imam sign-off");
    }

    // =====================================================================
    // Shared pipeline
    // =====================================================================

    /// <summary>
    /// Re-checks authorization for the required stage, then loads the form.
    /// Returns a null form together with the denial result when the request
    /// must not proceed — no entity has been touched at that point.
    /// </summary>
    private async Task<(MarriageApplicationForm? Form, Guid MemberId, StageAuthorizationResult Denied)> AuthorizeAsync(
        string membershipNo,
        Guid applicationFormId,
        MarriageFormStage requiredStage,
        CancellationToken cancellationToken)
    {
        var auth = await _stageAuthorization.CanUserActAsync(
            membershipNo, applicationFormId, requiredStage, cancellationToken);

        if (!auth.IsAllowed)
        {
            _logger.LogInformation(
                "Workflow submission blocked before any write: MembershipNo={MembershipNo}, ApplicationFormId={ApplicationFormId}, RequiredStage={RequiredStage}, Reason={Reason}",
                membershipNo, applicationFormId, requiredStage, auth.Reason);

            return (null, Guid.Empty, auth);
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
            return (null, Guid.Empty, StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.FormNotFound,
                "No such application/form exists."));
        }

        var memberId = await _context.JamaatMembers
            .Where(m => m.ChandaNo == membershipNo)
            .Select(m => (Guid?)m.Id)
            .FirstOrDefaultAsync(cancellationToken) ?? Guid.Empty;

        return (form, memberId, StageAuthorizationResult.Allow());
    }

    /// <summary>
    /// True when the bride and groom belong to the same Jama'at (case-insensitive
    /// match on their member records). Controls whether the workflow passes through
    /// AwaitingGroomJamaatPresident (different Jamaats) or skips it (same Jamaat).
    /// </summary>
    private async Task<bool> PartnersShareJamaatAsync(
        MarriageApplicationForm form,
        CancellationToken cancellationToken)
    {
        var brideJamaat = await _context.JamaatMembers
            .Where(m => m.ChandaNo == form.BrideMembershipNo)
            .Select(m => m.JamaatName)
            .FirstOrDefaultAsync(cancellationToken);

        var groomJamaat = await _context.JamaatMembers
            .Where(m => m.ChandaNo == form.BridegroomMembershipNo)
            .Select(m => m.JamaatName)
            .FirstOrDefaultAsync(cancellationToken);

        return !string.IsNullOrWhiteSpace(brideJamaat)
            && !string.IsNullOrWhiteSpace(groomJamaat)
            && string.Equals(brideJamaat, groomJamaat, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Advances the stage, stamps audit fields, and saves.</summary>
    private async Task<StageAuthorizationResult> AdvanceAsync(
        MarriageApplicationForm form,
        Guid memberId,
        DateTime now,
        MarriageFormStage nextStage,
        string actionLabel)
    {
        form.FormStage = nextStage;

        // Keep the coarse review-chain ApplicationStage in sync with the
        // fine-grained FormStage so the revert flow (which authorizes against
        // ApplicationStage) can never deadlock/stall at ApplicantsReview.
        form.ApplicationStage = WorkflowStageMapping.ToApplicationStage(nextStage);

        form.ModifiedAt = now;
        form.ModifiedBy = memberId;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Workflow advanced: MemberId={MemberId}, ApplicationFormId={ApplicationFormId}, Action={Action}, NewStage={NewStage}",
            memberId, form.Id, actionLabel, nextStage);

        return StageAuthorizationResult.Allow();
    }
}
