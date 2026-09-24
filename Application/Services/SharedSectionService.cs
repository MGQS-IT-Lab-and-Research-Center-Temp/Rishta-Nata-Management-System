using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Application.Workflow;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DTOs.Members;
using Infrastructure.DTOs.SharedSection;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class SharedSectionService : ISharedSectionService
{
    private readonly RishtanataDbContext _context;
    private readonly IMemberLookupService _memberLookup;

    public SharedSectionService(RishtanataDbContext context, IMemberLookupService memberLookup)
    {
        _context = context;
        _memberLookup = memberLookup;
    }

    public async Task<SectionTokenStatus> ValidateTokenAsync(
        string token, CancellationToken cancellationToken = default)
    {
        var hash = HashToken(token);
        var tokenRow = await _context.SectionAccessTokens
            .AsNoTracking()
            .Include(x => x.MarriageApplicationForm)
            .FirstOrDefaultAsync(x => x.TokenHash == hash, cancellationToken);

        if (tokenRow is null)
        {
            return Invalid("No matching token.");
        }

        var form = tokenRow.MarriageApplicationForm;

        if (tokenRow.RevokedAt.HasValue)
        {
            if (tokenRow.SubmittedAt.HasValue)
                return new SectionTokenStatus
                {
                    IsValid = false,
                    IsSubmitted = true,
                    FormId = form.Id,
                    SectionType = tokenRow.SectionType,
                    ReferenceNumber = form.ReferenceNumber,
                    BrideName = form.BrideName,
                    BridegroomName = form.BridegroomName
                };

            return Invalid("This link has been revoked.");
        }

        if (form.FormStage != MarriageFormStage.AwaitingWitnesses)
            return Invalid("These signatures are no longer being collected.");

        return new SectionTokenStatus
        {
            IsValid = true,
            FormId = form.Id,
            SectionType = tokenRow.SectionType,
            ReferenceNumber = form.ReferenceNumber,
            BrideName = form.BrideName,
            BridegroomName = form.BridegroomName
        };
    }

    public async Task<SectionSubmitResult> SubmitSectionAsync(
        string token, SectionFillData data, CancellationToken cancellationToken = default)
    {
        var hash = HashToken(token);
        var tokenRow = await _context.SectionAccessTokens
            .Include(x => x.MarriageApplicationForm)
                .ThenInclude(f => f.GuardianOrWakeelSection)
            .Include(x => x.MarriageApplicationForm)
                .ThenInclude(f => f.GroomWakeelSection)
            .Include(x => x.MarriageApplicationForm)
                .ThenInclude(f => f.WitnessSignatures)
            .Include(x => x.MarriageApplicationForm)
                .ThenInclude(f => f.MarriageApplication)
            .FirstOrDefaultAsync(x => x.TokenHash == hash, cancellationToken);

        if (tokenRow is null || tokenRow.RevokedAt.HasValue ||
            tokenRow.MarriageApplicationForm.FormStage != MarriageFormStage.AwaitingWitnesses)
        {
            return new SectionSubmitResult { Success = false, Message = "This link is invalid or no longer active." };
        }

        var form = tokenRow.MarriageApplicationForm;

        // A representative may only sign when the guardian declared one. Checked
        // before any write so a rejected submission leaves no side effects.
        if (tokenRow.SectionType == SectionType.Representative &&
            form.GuardianOrWakeelSection?.AppointsRepresentative != true)
        {
            return new SectionSubmitResult { Success = false, Message = "This link is invalid or no longer active." };
        }

        // Member-prefill (override only blank manual fields).
        if (data.IsMember && !string.IsNullOrWhiteSpace(data.MemberMembershipNo))
        {
            var member = await _memberLookup.LookupAsync(data.MemberMembershipNo, cancellationToken);
            if (member is not null)
            {
                ApplyMemberPrefill(data, member);
            }
        }

        ApplySectionUpsert(form, tokenRow.SectionType, data);
        StampsSectionRowReferenceNumber(form, tokenRow.SectionType, form.ReferenceNumber);

        // The guardian withdrew (or never made) a representative appointment:
        // make sure no Representative link stays usable.
        if (tokenRow.SectionType == SectionType.Guardian && !data.AppointsRepresentative)
        {
            await ReleaseRepresentativeLinkAsync(form.Id, cancellationToken);
        }

        // Idempotent block advancement — a re-save while still open is allowed,
        // but once advanced it stays advanced (never double-advances).
        var advanced = false;
        if (form.FormStage == MarriageFormStage.AwaitingWitnesses &&
            IsBlockComplete(form))
        {
            form.FormStage = MarriageFormStage.AwaitingBrideJamaatPresident;

            // The signature block hands the form to the review chain. Keep the
            // coarse ApplicationStage in sync (the revert flow authorizes on it;
            // see docs/stage-authorization-policy.md §5) and flip the wrapping
            // application to "pending review" so the Jama'at President's
            // dashboard actually surfaces it.
            form.ApplicationStage = WorkflowStageMapping.ToApplicationStage(
                MarriageFormStage.AwaitingBrideJamaatPresident);

            if (form.MarriageApplication is not null)
            {
                form.MarriageApplication.Status = ApplicationStatus.ApplicationPending;
            }

            advanced = true;
        }

        // Auto-revoke: seal the token so the couple cannot re-use or manage this link.
        // The hash is retained so a re-visit identifies as "already submitted";
        // the raw token is cleared so the link itself stops working.
        tokenRow.SubmittedAt = DateTime.UtcNow;
        tokenRow.RevokedAt = DateTime.UtcNow;
        tokenRow.RawToken = string.Empty;
        tokenRow.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new SectionSubmitResult
        {
            Success = true,
            StageAdvanced = advanced,
            Message = advanced
                ? "All signatures are recorded; the application moves to the Jama'at President's review."
                : "Your section has been recorded."
        };
    }

    public async Task<IReadOnlyList<SectionLinkStatus>> GetSignatureLinksStatusAsync(
        Guid applicationFormId, CancellationToken cancellationToken = default)
    {
        var form = await _context.MarriageApplicationForms
            .AsNoTracking()
            .Include(x => x.GuardianOrWakeelSection)
            .Include(x => x.GroomWakeelSection)
            .Include(x => x.WitnessSignatures)
            .Include(x => x.SectionAccessTokens)
            .FirstOrDefaultAsync(x => x.Id == applicationFormId, cancellationToken)
            ?? new MarriageApplicationForm { Id = applicationFormId };

        var statuses = new List<SectionLinkStatus>
        {
            BuildStatus(SectionType.Guardian, form),
            BuildStatus(SectionType.WitnessOne, form),
            BuildStatus(SectionType.WitnessTwo, form)
        };

        if (form.GuardianOrWakeelSection?.AppointsRepresentative == true)
        {
            statuses.Add(BuildStatus(SectionType.Representative, form));
        }

        if (!form.CanAttendNikahInPerson)
        {
            statuses.Add(BuildStatus(SectionType.GroomWakeel, form));
        }

        return statuses;
    }

    public async Task<string> GenerateSectionTokenAsync(
        Guid applicationFormId, SectionType section, string createdByMembershipNo,
        bool allowSubmitted = false, CancellationToken cancellationToken = default)
    {
        await EnsureSectionApplicableAsync(applicationFormId, section, cancellationToken);

        var existing = await _context.SectionAccessTokens
            .FirstOrDefaultAsync(
                x => x.MarriageApplicationFormId == applicationFormId && x.SectionType == section,
                cancellationToken);

        if (existing?.SubmittedAt is not null && !allowSubmitted)
        {
            throw new InvalidOperationException(
                $"The section {section} was already submitted; only the Rishtanata Secretary may reopen its link.");
        }

        var raw = NewToken();

        if (existing is null)
        {
            _context.SectionAccessTokens.Add(new SectionAccessToken
            {
                MarriageApplicationFormId = applicationFormId,
                SectionType = section,
                TokenHash = HashToken(raw),
                RawToken = raw,
                CreatedByMembershipNo = createdByMembershipNo,
                RevokedAt = null,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            });
        }
        else if (existing.RevokedAt.HasValue)
        {
            existing.TokenHash = HashToken(raw);
            existing.RawToken = raw;
            existing.CreatedByMembershipNo = createdByMembershipNo;
            existing.SubmittedAt = null;
            existing.RevokedAt = null;
            existing.ModifiedAt = DateTime.UtcNow;
        }
        else
        {
            throw new InvalidOperationException(
                $"A link for section {section} already exists; use Regenerate instead.");
        }

        await _context.SaveChangesAsync(cancellationToken);
        return raw;
    }

    public async Task<string> RegenerateSectionTokenAsync(
        Guid applicationFormId, SectionType section, string createdByMembershipNo,
        bool allowSubmitted = false, CancellationToken cancellationToken = default)
    {
        await EnsureSectionApplicableAsync(applicationFormId, section, cancellationToken);

        var row = await _context.SectionAccessTokens
            .FirstOrDefaultAsync(
                x => x.MarriageApplicationFormId == applicationFormId && x.SectionType == section,
                cancellationToken)
            ?? throw new InvalidOperationException(
                $"No link exists for section {section}; generate one first.");

        if (row.SubmittedAt is not null && !allowSubmitted)
        {
            throw new InvalidOperationException(
                $"The section {section} was already submitted; only the Rishtanata Secretary may reopen its link.");
        }

        var raw = NewToken();
        row.TokenHash = HashToken(raw);
        row.RawToken = raw;
        row.CreatedByMembershipNo = createdByMembershipNo;
        row.SubmittedAt = null;
        row.RevokedAt = null;
        row.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return raw;
    }

    public async Task RevokeSectionTokenAsync(
        Guid applicationFormId, SectionType section,
        CancellationToken cancellationToken = default)
    {
        var row = await _context.SectionAccessTokens
            .FirstOrDefaultAsync(
                x => x.MarriageApplicationFormId == applicationFormId && x.SectionType == section,
                cancellationToken)
            ?? throw new InvalidOperationException(
                $"No link exists for section {section}.");

        if (row.RevokedAt.HasValue)
            return;

        row.RevokedAt = DateTime.UtcNow;
        row.RawToken = string.Empty;
        row.TokenHash = string.Empty;
        row.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }

    // ==================================================================

    private static void ApplyMemberPrefill(SectionFillData data, MemberLookupDto member)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
            data.Name = member.FullName;
        if (string.IsNullOrWhiteSpace(data.Address))
            data.Address = member.Address ?? string.Empty;
        if (string.IsNullOrWhiteSpace(data.Tel))
            data.Tel = member.PhoneNo ?? string.Empty;
    }

    private void ApplySectionUpsert(
        MarriageApplicationForm form, SectionType section, SectionFillData data)
    {
        switch (section)
        {
            case SectionType.Guardian:
                if (form.GuardianOrWakeelSection is null)
                {
                    form.GuardianOrWakeelSection = new GuardianOrWakeelSection
                    {
                        Name = data.Name,
                        Address = data.Address,
                        Tel = data.Tel,
                        RelationToBride = data.RelationToBride,
                        AppointsRepresentative = data.AppointsRepresentative,
                        Signature = data.Name,
                        Date = data.SignatureDate,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    };
                }
                else
                {
                    form.GuardianOrWakeelSection.Name = data.Name;
                    form.GuardianOrWakeelSection.Address = data.Address;
                    form.GuardianOrWakeelSection.Tel = data.Tel;
                    form.GuardianOrWakeelSection.RelationToBride = data.RelationToBride;
                    form.GuardianOrWakeelSection.AppointsRepresentative = data.AppointsRepresentative;
                    form.GuardianOrWakeelSection.Signature = data.Name;
                    form.GuardianOrWakeelSection.Date = data.SignatureDate;
                    form.GuardianOrWakeelSection.ModifiedAt = DateTime.UtcNow;
                }
                form.GuardianName = data.Name;
                form.GuardianRelationToBride = data.RelationToBride;
                form.GuardianAddress = data.Address;
                form.GuardianTel = data.Tel;
                form.GuardianSignatureDate = data.SignatureDate.ToString("yyyy-MM-dd");

                if (!data.AppointsRepresentative)
                {
                    ClearRepresentative(form);
                }
                break;

            case SectionType.GroomWakeel:
                if (form.GroomWakeelSection is null)
                {
                    form.GroomWakeelSection = new GroomWakeelSection
                    {
                        Name = data.Name,
                        FatherName = data.RelationToBride,
                        Tel = data.Tel,
                        Signature = data.Name,
                        Date = data.SignatureDate,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    };
                }
                else
                {
                    form.GroomWakeelSection.Name = data.Name;
                    form.GroomWakeelSection.FatherName = data.RelationToBride;
                    form.GroomWakeelSection.Tel = data.Tel;
                    form.GroomWakeelSection.Signature = data.Name;
                    form.GroomWakeelSection.Date = data.SignatureDate;
                    form.GroomWakeelSection.ModifiedAt = DateTime.UtcNow;
                }
                form.GroomWakeelName = data.Name;
                form.GroomWakeelFatherName = data.RelationToBride;
                form.GroomWakeelTel = data.Tel;
                form.GroomWakeelSignatureDate = data.SignatureDate.ToString("yyyy-MM-dd");
                break;

            case SectionType.Representative:
                // SubmitSectionAsync has already rejected this section unless the
                // guardian's row exists with AppointsRepresentative == true.
                var guardian = form.GuardianOrWakeelSection!;
                guardian.RepresentativeName = data.Name;
                guardian.RepresentativeAddress = data.Address;
                guardian.ActingFor = data.RelationToBride;
                guardian.RepresentativeSignature = data.Name;
                guardian.RepresentativeDate = data.SignatureDate;
                guardian.ModifiedAt = DateTime.UtcNow;

                form.RepresentativeName = data.Name;
                form.RepresentativeAddress = data.Address;
                form.RepresentativeActingFor = data.RelationToBride;
                form.RepresentativeSignatureDate = data.SignatureDate.ToString("yyyy-MM-dd");
                break;

            case SectionType.WitnessOne:
                UpsertWitness(form, 1, data);
                form.WitnessOneName = data.Name;
                form.WitnessOneAddress = data.Address;
                form.WitnessOneTel = data.Tel;
                form.WitnessOneMembershipNo = data.IsMember ? data.MemberMembershipNo ?? string.Empty : string.Empty;
                form.WitnessOneSignatureDate = data.SignatureDate.ToString("yyyy-MM-dd");
                break;

            case SectionType.WitnessTwo:
                UpsertWitness(form, 2, data);
                form.WitnessTwoName = data.Name;
                form.WitnessTwoAddress = data.Address;
                form.WitnessTwoTel = data.Tel;
                form.WitnessTwoMembershipNo = data.IsMember ? data.MemberMembershipNo ?? string.Empty : string.Empty;
                form.WitnessTwoSignatureDate = data.SignatureDate.ToString("yyyy-MM-dd");
                break;
        }
    }

    private void UpsertWitness(MarriageApplicationForm form, int witnessNumber, SectionFillData data)
    {
        var witness = form.WitnessSignatures.FirstOrDefault(w => w.WitnessNumber == witnessNumber);

        if (witness is null)
        {
            form.WitnessSignatures.Add(new WitnessSignatureSection
            {
                WitnessNumber = witnessNumber,
                WitnessContext = WitnessContext.NikahCeremony,
                Name = data.Name,
                Address = data.Address,
                Tel = data.Tel,
                Signature = data.Name,
                SignatureDate = data.SignatureDate,
                ReferenceNumber = form.ReferenceNumber,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            });
        }
        else
        {
            witness.Name = data.Name;
            witness.Address = data.Address;
            witness.Tel = data.Tel;
            witness.Signature = data.Name;
            witness.SignatureDate = data.SignatureDate;
            witness.ReferenceNumber = form.ReferenceNumber;
            witness.ModifiedAt = DateTime.UtcNow;
        }
    }

    private static void StampsSectionRowReferenceNumber(MarriageApplicationForm form, SectionType section, string referenceNumber)
    {
        switch (section)
        {
            case SectionType.Guardian when form.GuardianOrWakeelSection is not null:
                form.GuardianOrWakeelSection.ReferenceNumber = referenceNumber;
                break;
            case SectionType.GroomWakeel when form.GroomWakeelSection is not null:
                form.GroomWakeelSection.ReferenceNumber = referenceNumber;
                break;
            case SectionType.Representative when form.GuardianOrWakeelSection is not null:
                form.GuardianOrWakeelSection.ReferenceNumber = referenceNumber;
                break;
            case SectionType.WitnessOne:
                form.WitnessSignatures.FirstOrDefault(w => w.WitnessNumber == 1)!.ReferenceNumber = referenceNumber;
                break;
            case SectionType.WitnessTwo:
                form.WitnessSignatures.FirstOrDefault(w => w.WitnessNumber == 2)!.ReferenceNumber = referenceNumber;
                break;
        }
    }

    private static bool IsBlockComplete(MarriageApplicationForm form) =>
        form.GuardianOrWakeelSection is not null &&
        !string.IsNullOrWhiteSpace(form.GuardianOrWakeelSection.Name) &&
        (!form.GuardianOrWakeelSection.AppointsRepresentative ||
         !string.IsNullOrWhiteSpace(form.GuardianOrWakeelSection.RepresentativeName)) &&
        form.WitnessSignatures.Count(w => w.WitnessNumber is 1 or 2 && !string.IsNullOrWhiteSpace(w.Name)) >= 2 &&
        (form.CanAttendNikahInPerson ||
         (form.GroomWakeelSection is not null && !string.IsNullOrWhiteSpace(form.GroomWakeelSection.Name)));

    private static SectionLinkStatus BuildStatus(SectionType section, MarriageApplicationForm form)
    {
        string? filledByName = null;
        var complete = false;

        if (section == SectionType.Guardian)
        {
            filledByName = form.GuardianOrWakeelSection?.Name;
            complete = !string.IsNullOrWhiteSpace(filledByName);
        }
        else if (section == SectionType.GroomWakeel)
        {
            filledByName = form.GroomWakeelSection?.Name;
            complete = !string.IsNullOrWhiteSpace(filledByName);
        }
        else if (section == SectionType.Representative)
        {
            filledByName = form.GuardianOrWakeelSection?.RepresentativeName;
            complete = !string.IsNullOrWhiteSpace(filledByName);
        }
        else
        {
            var number = section == SectionType.WitnessOne ? 1 : 2;
            var witness = form.WitnessSignatures.FirstOrDefault(w => w.WitnessNumber == number);
            filledByName = witness?.Name;
            complete = !string.IsNullOrWhiteSpace(filledByName);
        }

        var token = form.SectionAccessTokens.FirstOrDefault(t =>
            t.SectionType == section);

        var submitted = token?.SubmittedAt.HasValue == true;

        return new SectionLinkStatus
        {
            Section = section,
            HasActiveToken = token is not null && !token.RevokedAt.HasValue,
            RawToken = token?.RawToken,
            Complete = complete,
            FilledByName = filledByName,
            Submitted = submitted
        };
    }

    /// <summary>
    /// Throws when a link for <paramref name="section"/> makes no sense for this
    /// form (e.g. a Representative link when the guardian appointed none).
    /// Controllers surface the message through TempData["Error"].
    /// </summary>
    private async Task EnsureSectionApplicableAsync(
        Guid applicationFormId, SectionType section, CancellationToken cancellationToken)
    {
        if (section == SectionType.Representative)
        {
            var appoints = await _context.MarriageApplicationForms
                .Where(f => f.Id == applicationFormId)
                .Select(f => f.GuardianOrWakeelSection != null && f.GuardianOrWakeelSection.AppointsRepresentative)
                .FirstOrDefaultAsync(cancellationToken);

            if (!appoints)
            {
                throw new InvalidOperationException(
                    "The guardian has not appointed a representative, so no representative link can be created.");
            }
        }
    }

    /// <summary>Removes a previously recorded representative signature (entity + flat mirrors).</summary>
    private static void ClearRepresentative(MarriageApplicationForm form)
    {
        if (form.GuardianOrWakeelSection is not null)
        {
            form.GuardianOrWakeelSection.RepresentativeName = string.Empty;
            form.GuardianOrWakeelSection.RepresentativeAddress = string.Empty;
            form.GuardianOrWakeelSection.ActingFor = null;
            form.GuardianOrWakeelSection.RepresentativeSignature = null;
            form.GuardianOrWakeelSection.RepresentativeDate = null;
        }

        form.RepresentativeName = string.Empty;
        form.RepresentativeAddress = string.Empty;
        form.RepresentativeActingFor = string.Empty;
        form.RepresentativeSignatureDate = string.Empty;
    }

    /// <summary>
    /// Kills any Representative link for the form and clears its "submitted"
    /// seal, so a later re-appointment can generate a fresh link. Tracked
    /// change only; the caller saves.
    /// </summary>
    private async Task ReleaseRepresentativeLinkAsync(Guid applicationFormId, CancellationToken cancellationToken)
    {
        var row = await _context.SectionAccessTokens
            .FirstOrDefaultAsync(
                x => x.MarriageApplicationFormId == applicationFormId && x.SectionType == SectionType.Representative,
                cancellationToken);

        if (row is null)
            return;

        row.RevokedAt ??= DateTime.UtcNow;
        row.SubmittedAt = null;
        row.RawToken = string.Empty;
        row.TokenHash = string.Empty;
        row.ModifiedAt = DateTime.UtcNow;
    }

    private static SectionTokenStatus Invalid(string reason) =>
        new() { IsValid = false, InvalidationReason = reason };

    private static string NewToken() =>
        Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}