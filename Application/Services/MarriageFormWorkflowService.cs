using System.Globalization;
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

        var invalid = ValidateLocalRishtanataSecretary(submission);
        if (invalid is not null)
        {
            return StageAuthorizationResult.Deny(StageAuthorizationDenyReason.WrongStage, invalid);
        }

        // Same-Jamaat couples: this president signs for both partners, so they
        // also attest the groom (Gap 7). Decided before any write.
        var sharesJamaat = await PartnersShareJamaatAsync(form, cancellationToken);

        var attestationError =
            ValidatePartnerAttestation(submission.Bride, "bride")
            ?? (sharesJamaat ? ValidatePartnerAttestation(submission.Groom, "bridegroom") : null)
            ?? (!submission.GuardianIsBonafide || !submission.BrideSignedFreely
                ? "Confirm that the guardian is bona fide and that the bride signed freely before signing."
                : null);
        if (attestationError is not null)
        {
            return StageAuthorizationResult.Deny(StageAuthorizationDenyReason.WrongStage, attestationError);
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
                LocalRishtanataSecretaryName = submission.LocalRishtanataSecretaryName.Trim(),
                LocalRishtanataSecretaryTel = submission.LocalRishtanataSecretaryTel.Trim(),
                LocalRishtanataSecretarySignatureDate = submission.LocalRishtanataSecretarySignatureDate.Trim(),
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
            form.JamaatPresidentVerification.LocalRishtanataSecretaryName = submission.LocalRishtanataSecretaryName.Trim();
            form.JamaatPresidentVerification.LocalRishtanataSecretaryTel = submission.LocalRishtanataSecretaryTel.Trim();
            form.JamaatPresidentVerification.LocalRishtanataSecretarySignatureDate = submission.LocalRishtanataSecretarySignatureDate.Trim();
            form.JamaatPresidentVerification.ModifiedAt = now;
            form.JamaatPresidentVerification.ModifiedBy = memberId;
        }

        // President's attestations (Gap 7); validated above. Years only apply to
        // converts. The groom's answers live here only on the same-Jamaat path;
        // otherwise they're cleared (e.g. after a revert) and the groom's
        // president records them.
        var verification = form.JamaatPresidentVerification!;
        (verification.BrideIsBornAhmadi, verification.BrideYearsAsAhmadi, verification.BrideMarriageReason) =
            ToStoredAttestation(submission.Bride!);

        if (sharesJamaat)
        {
            (verification.GroomIsBornAhmadi, verification.GroomYearsAsAhmadi, verification.GroomMarriageReason) =
                ToStoredAttestation(submission.Groom!);
        }
        else
        {
            verification.GroomIsBornAhmadi = null;
            verification.GroomYearsAsAhmadi = null;
            verification.GroomMarriageReason = string.Empty;
        }

        verification.GuardianIsBonafide = submission.GuardianIsBonafide;
        verification.BrideSignedFreely = submission.BrideSignedFreely;

        // Mirror onto the flat form columns (used by the read side).
        form.JamaatPresidentName = submission.Name;
        form.JamaatPresidentSignatureDate = submission.SignatureDate;
        form.BrideLocalRishtanataSecretaryName = submission.LocalRishtanataSecretaryName.Trim();
        form.BrideLocalRishtanataSecretarySignatureDate = submission.LocalRishtanataSecretarySignatureDate.Trim();

        // Same-Jamaat couples: this president signs for both partners and the
        // form advances straight to the secretary. Different Jamaats: forward to
        // the groom's president first.
        var nextStage = sharesJamaat
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

        var invalid = ValidateLocalRishtanataSecretary(submission);
        if (invalid is not null)
        {
            return StageAuthorizationResult.Deny(StageAuthorizationDenyReason.WrongStage, invalid);
        }

        // The groom's president attests the groom only (Gap 7).
        var attestationError = ValidatePartnerAttestation(submission.Groom, "bridegroom");
        if (attestationError is not null)
        {
            return StageAuthorizationResult.Deny(StageAuthorizationDenyReason.WrongStage, attestationError);
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
                LocalRishtanataSecretaryName = submission.LocalRishtanataSecretaryName.Trim(),
                LocalRishtanataSecretaryTel = submission.LocalRishtanataSecretaryTel.Trim(),
                LocalRishtanataSecretarySignatureDate = submission.LocalRishtanataSecretarySignatureDate.Trim(),
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
            form.GroomJamaatPresidentVerification.LocalRishtanataSecretaryName = submission.LocalRishtanataSecretaryName.Trim();
            form.GroomJamaatPresidentVerification.LocalRishtanataSecretaryTel = submission.LocalRishtanataSecretaryTel.Trim();
            form.GroomJamaatPresidentVerification.LocalRishtanataSecretarySignatureDate = submission.LocalRishtanataSecretarySignatureDate.Trim();
            form.GroomJamaatPresidentVerification.ModifiedAt = now;
            form.GroomJamaatPresidentVerification.ModifiedBy = memberId;
        }

        // President's attestation for the groom (Gap 7); validated above.
        var groomVerification = form.GroomJamaatPresidentVerification!;
        (groomVerification.GroomIsBornAhmadi, groomVerification.GroomYearsAsAhmadi, groomVerification.GroomMarriageReason) =
            ToStoredAttestation(submission.Groom!);

        form.GroomJamaatPresidentName = submission.Name;
        form.GroomJamaatPresidentSignatureDate = submission.SignatureDate;
        form.GroomLocalRishtanataSecretaryName = submission.LocalRishtanataSecretaryName.Trim();
        form.GroomLocalRishtanataSecretarySignatureDate = submission.LocalRishtanataSecretarySignatureDate.Trim();

        return await AdvanceAsync(
            form, memberId, now,
            MarriageFormStage.AwaitingRishtanataSecretary,
            "groom Jamaat president verification");
    }

    /// <summary>
    /// Signing requires the Local Rishtanata Secretary block (Gap 4). Returns a
    /// user-facing error, or null when valid. Lengths match the narrowest
    /// columns (GroomJamaatPresidentVerifications: 200 / 30 / 50).
    /// </summary>
    private static string? ValidateLocalRishtanataSecretary(JamaatPresidentVerificationSubmission submission)
    {
        var name = submission.LocalRishtanataSecretaryName?.Trim();
        var tel = submission.LocalRishtanataSecretaryTel?.Trim();
        var date = submission.LocalRishtanataSecretarySignatureDate?.Trim();

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(tel) || string.IsNullOrEmpty(date))
        {
            return "Enter the Local Rishtanata Secretary's name, telephone and signature date before signing.";
        }

        if (name.Length > 200 || tel.Length > 30)
        {
            return "The Local Rishtanata Secretary's name or telephone is too long.";
        }

        if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            return "The Local Rishtanata Secretary's signature date must be a valid date (yyyy-MM-dd).";
        }

        return null;
    }

    /// <summary>
    /// One partner's attestation (Gap 7). Returns a user-facing error, or null
    /// when valid. Limits match the columns (MarriageReason varchar(200)).
    /// </summary>
    private static string? ValidatePartnerAttestation(PartnerAttestationSubmission? attestation, string partner)
    {
        if (attestation?.IsBornAhmadi is null)
        {
            return $"Record whether the {partner} is a born Ahmadi before signing.";
        }

        if (attestation.IsBornAhmadi == false &&
            (attestation.YearsAsAhmadi is null || attestation.YearsAsAhmadi < 0 || attestation.YearsAsAhmadi > 120))
        {
            return $"Enter how many years (0–120) the {partner} has been an Ahmadi before signing.";
        }

        if ((attestation.MarriageReason?.Trim().Length ?? 0) > 200)
        {
            return $"The {partner}'s marriage reason must be 200 characters or fewer.";
        }

        return null;
    }

    /// <summary>
    /// The stored values of one validated partner attestation (Gap 7). Years
    /// only apply to converts; the reason is trimmed.
    /// </summary>
    private static (bool? IsBornAhmadi, int? YearsAsAhmadi, string MarriageReason) ToStoredAttestation(
        PartnerAttestationSubmission attestation) =>
        (attestation.IsBornAhmadi,
         attestation.IsBornAhmadi == true ? null : attestation.YearsAsAhmadi,
         attestation.MarriageReason?.Trim() ?? string.Empty);

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
                Name = submission.Name,
                Recommendation = submission.Recommendation,
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
            form.RishtanataRecommendation.Name = submission.Name;
            form.RishtanataRecommendation.Recommendation = submission.Recommendation;
            form.RishtanataRecommendation.SignatureDate = submission.SignatureDate;
            form.RishtanataRecommendation.OfficiatingImamMembershipNo = submission.OfficiatingImamMembershipNo;
            form.RishtanataRecommendation.ModifiedAt = now;
            form.RishtanataRecommendation.ModifiedBy = memberId;
        }

        form.OfficiatingImamMembershipNo = submission.OfficiatingImamMembershipNo;
        form.NationalRishtanataSecretaryName = submission.Name;
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

        // The ceremony witnesses (F1 §IX) sign through their shared links at this
        // stage; the imam cannot close the form before both have signed.
        var ceremonyWitnessNames = await _context.Set<WitnessSignatureSection>()
            .AsNoTracking()
            .Where(w => w.MarriageApplicationFormId == form.Id &&
                        w.WitnessContext == WitnessContext.NikahCeremony &&
                        (w.WitnessNumber == 1 || w.WitnessNumber == 2))
            .Select(w => w.Name)
            .ToListAsync(cancellationToken);

        if (ceremonyWitnessNames.Count(n => !string.IsNullOrWhiteSpace(n)) < 2)
        {
            return StageAuthorizationResult.Deny(
                StageAuthorizationDenyReason.WrongStage,
                "Both Nikah ceremony witnesses must sign before the imam can sign off. " +
                "The applicants can send the ceremony witness links from their Signature Links page.");
        }

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
