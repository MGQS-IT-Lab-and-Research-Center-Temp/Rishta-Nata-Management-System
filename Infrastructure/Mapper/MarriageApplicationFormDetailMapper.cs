using Domain.Entities;
using Infrastructure.DTOs.MarriageApplicationFormDetail;

namespace Infrastructure.Mapper;

/// <summary>
/// Maps a MarriageApplicationForm (with its owning FormApplication and
/// rejection history loaded) to the read-side detail DTO.
///
/// A section is emitted only when it has been submitted; otherwise the DTO
/// property stays null so the frontend can render "not completed yet".
/// Submission markers:
///   - applicant/party sections  → the person's name is present;
///   - verifier sections         → the office-holder's signature date is set
///                                 (Amir approval also accepts an approved
///                                 Nikah date);
///   - witnesses                 → included individually by name.
/// </summary>
public static class MarriageApplicationFormDetailMapper
{
    public static MarriageApplicationFormDetailDto ToDetailDto(
        MarriageApplicationForm form,
        IReadOnlyList<MarriageFormRejection> rejections)
    {
        var application = form.MarriageApplication;

        return new MarriageApplicationFormDetailDto
        {
            // ===== Shared fields =====
            FormId = form.Id,
            ApplicationId = form.MarriageApplicationId,
            ReferenceNumber = form.ReferenceNumber,
            ProposedNikahDate = form.ProposedNikahDate,
            Venue = form.Venue,
            Status = application?.Status.ToString() ?? string.Empty,
            AppliedAt = application?.AppliedAt ?? default,

            // ===== Workflow =====
            CurrentStage = form.ApplicationStage,

            // ===== Sections =====
            Bride = HasValue(form.BrideName)
                ? new BrideSectionDetailDto
                {
                    MembershipNo = form.BrideMembershipNo,
                    Name = form.BrideName,
                    DateOfBirth = form.BrideDateOfBirth,
                    ResidentOf = form.BrideResidentOf,
                    Genotype = form.BrideGenotype,
                    BloodGroup = form.BrideBloodGroup,
                    MaritalStatus = form.BrideMaritalStatus,
                    ProposedDowerAmount = form.BrideProposedDowerAmount,
                    DowerAmountReceivedInCash = form.BrideDowerAmountReceivedInCash,
                    SignatureTel = form.BrideSignatureTel
                }
                : null,

            Bridegroom = HasValue(form.BridegroomName)
                ? new BridegroomSectionDetailDto
                {
                    MembershipNo = form.BridegroomMembershipNo,
                    Name = form.BridegroomName,
                    DateOfBirth = form.BridegroomDateOfBirth,
                    ResidentOf = form.BridegroomResidentOf,
                    Genotype = form.BridegroomGenotype,
                    BloodGroup = form.BridegroomBloodGroup,
                    DowerAmountPaidInCash = form.BridegroomDowerAmountPaidInCash,
                    DowerAmountToBePaid = form.BridegroomDowerAmountToBePaid,
                    IsFirstNikah = form.IsFirstNikah,
                    IsSecondThirdOrFourthNikah = form.IsSecondThirdOrFourthNikah,
                    FormerWifeIsDead = form.FormerWifeIsDead,
                    HasDivorcedFormerWife = form.HasDivorcedFormerWife,
                    FormerWifeIsPresent = form.FormerWifeIsPresent,
                    FormerWifeObtainedKhula = form.FormerWifeObtainedKhula,
                    SignatureTel = form.BridegroomSignatureTel
                }
                : null,

            Guardian = HasValue(form.GuardianName)
                ? new GuardianSectionDetailDto
                {
                    Name = form.GuardianName,
                    RelationToBride = form.GuardianRelationToBride,
                    Address = form.GuardianAddress,
                    Tel = form.GuardianTel,
                    SignatureDate = form.GuardianSignatureDate
                }
                : null,

            Representative = HasValue(form.RepresentativeName)
                ? new RepresentativeSectionDetailDto
                {
                    Name = form.RepresentativeName,
                    Address = form.RepresentativeAddress,
                    ActingFor = form.RepresentativeActingFor,
                    SignatureDate = form.RepresentativeSignatureDate
                }
                : null,

            Witnesses = CollectWitnesses(form),

            OfficiatingImam = HasValue(form.OfficiatingImamSignatureDate)
                ? new OfficiatingImamSectionDetailDto
                {
                    Name = form.OfficiatingImamName,
                    AddressJamaat = form.OfficiatingImamAddressJamaat,
                    SignatureDate = form.OfficiatingImamSignatureDate
                }
                : null,

            JamaatPresident = HasValue(form.JamaatPresidentSignatureDate)
                ? new JamaatPresidentSectionDetailDto
                {
                    Name = form.JamaatPresidentName,
                    SignatureDate = form.JamaatPresidentSignatureDate
                }
                : null,

            NationalRishtanataSecretary = HasValue(form.NationalRishtanataSecretarySignatureDate)
                ? new RishtanataSecretarySectionDetailDto
                {
                    Name = form.NationalRishtanataSecretaryName,
                    SignatureDate = form.NationalRishtanataSecretarySignatureDate
                }
                : null,

            AmirApproval =
                HasValue(form.NationalAmirOrMissionarySignatureDate) ||
                form.ApprovedDateOfNikah.HasValue
                    ? new AmirApprovalSectionDetailDto
                    {
                        ApprovedDateOfNikah = form.ApprovedDateOfNikah,
                        NationalAmirOrMissionarySignatureDate =
                            form.NationalAmirOrMissionarySignatureDate
                    }
                    : null,

            // ===== Rejection history =====
            Rejections = rejections
                .OrderBy(r => r.CreatedAt)
                .Select(r => new RejectionHistoryItemDto
                {
                    Id = r.Id,
                    RejectedAtStage = r.RejectedAtStage,
                    RevertedToStage = r.RevertedToStage,
                    Reason = r.Reason,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy
                })
                .ToList()
        };
    }

    private static WitnessDetailDto[] CollectWitnesses(MarriageApplicationForm form)
    {
        var witnesses = new List<WitnessDetailDto>(2);

        if (HasValue(form.WitnessOneName))
        {
            witnesses.Add(new WitnessDetailDto
            {
                Position = 1,
                Name = form.WitnessOneName,
                Address = form.WitnessOneAddress,
                Tel = form.WitnessOneTel,
                SignatureDate = form.WitnessOneSignatureDate
            });
        }

        if (HasValue(form.WitnessTwoName))
        {
            witnesses.Add(new WitnessDetailDto
            {
                Position = 2,
                Name = form.WitnessTwoName,
                Address = form.WitnessTwoAddress,
                Tel = form.WitnessTwoTel,
                SignatureDate = form.WitnessTwoSignatureDate
            });
        }

        return witnesses.ToArray();
    }

    private static bool HasValue(string? value) => !string.IsNullOrWhiteSpace(value);
}