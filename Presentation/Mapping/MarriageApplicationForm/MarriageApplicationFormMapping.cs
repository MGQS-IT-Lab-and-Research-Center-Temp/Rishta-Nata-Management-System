using Domain.Entities;
using Infrastructure.DTOs;
using Infrastructure.DTOs.MarriageApplicationForm;
using Presentation.ViewModel;

namespace Application.Mappings;

public static class MarriageApplicationFormMapping
{
    // MarriageApplicationForm DTO → Entity

    public static MarriageApplicationForm ToEntity(
        MarriageApplicationFormDto dto)
    {
        return new MarriageApplicationForm
        {
            Id = dto.Id,

            // Application
            MarriageApplicationId = dto.MarriageApplicationId,
            ReferenceNumber = dto.ReferenceNumber,
            ProposedNikahDate = dto.ProposedNikahDate,
            Venue = dto.Venue,

            // Bride
            BrideMembershipNo = dto.BrideMembershipNo,
            BrideName = dto.BrideName,
            BrideDateOfBirth = dto.BrideDateOfBirth,
            BrideResidentOf = dto.BrideResidentOf,
            BrideGenotype = dto.BrideGenotype,
            BrideBloodGroup = dto.BrideBloodGroup,
            BrideMaritalStatus = dto.BrideMaritalStatus,
            BrideProposedDowerAmount = dto.BrideProposedDowerAmount,
            BrideDowerAmountReceivedInCash =
                dto.BrideDowerAmountReceivedInCash,
            BrideSignatureTel = dto.BrideSignatureTel,

            // Bridegroom
            BridegroomMembershipNo = dto.BridegroomMembershipNo,
            BridegroomName = dto.BridegroomName,
            BridegroomDateOfBirth = dto.BridegroomDateOfBirth,
            BridegroomResidentOf = dto.BridegroomResidentOf,
            BridegroomGenotype = dto.BridegroomGenotype,
            BridegroomBloodGroup = dto.BridegroomBloodGroup,
            BridegroomDowerAmountPaidInCash =
                dto.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid =
                dto.BridegroomDowerAmountToBePaid,

            IsFirstNikah = dto.IsFirstNikah,
            IsSecondThirdOrFourthNikah =
                dto.IsSecondThirdOrFourthNikah,

            FormerWifeIsDead = dto.FormerWifeIsDead,
            HasDivorcedFormerWife = dto.HasDivorcedFormerWife,
            FormerWifeIsPresent = dto.FormerWifeIsPresent,
            FormerWifeObtainedKhula = dto.FormerWifeObtainedKhula,

            BridegroomSignatureTel = dto.BridegroomSignatureTel,

            // Parents
            BrideFatherName = dto.BrideFatherName,
            BridegroomFatherName = dto.BridegroomFatherName,

            // Guardian
            GuardianName = dto.GuardianName,
            GuardianRelationToBride = dto.GuardianRelationToBride,
            GuardianAddress = dto.GuardianAddress,
            GuardianTel = dto.GuardianTel,
            GuardianSignatureDate = dto.GuardianSignatureDate,

            // Representative
            RepresentativeName = dto.RepresentativeName,
            RepresentativeAddress = dto.RepresentativeAddress,
            RepresentativeActingFor = dto.RepresentativeActingFor,
            RepresentativeSignatureDate =
                dto.RepresentativeSignatureDate,

            // Verification & Approval
            OfficiatingImamName = dto.OfficiatingImamName,
            OfficiatingImamAddressJamaat =
                dto.OfficiatingImamAddressJamaat,
            OfficiatingImamSignatureDate =
                dto.OfficiatingImamSignatureDate,

            JamaatPresidentName = dto.JamaatPresidentName,
            JamaatPresidentSignatureDate =
                dto.JamaatPresidentSignatureDate,

            NationalRishtanataSecretaryName =
                dto.NationalRishtanataSecretaryName,
            NationalRishtanataSecretarySignatureDate =
                dto.NationalRishtanataSecretarySignatureDate,

            ApprovedDateOfNikah = dto.ApprovedDateOfNikah,

            NationalAmirOrMissionarySignatureDate =
                dto.NationalAmirOrMissionarySignatureDate
        };
    }
    // MarriageApplicationForm Entity → DTO

    public static MarriageApplicationFormDto ToDto(
        MarriageApplicationForm entity)
    {
        return new MarriageApplicationFormDto
        {
            Id = entity.Id,

            // Application
            MarriageApplicationId =
                entity.MarriageApplicationId,

            ReferenceNumber =
                entity.ReferenceNumber,

            ProposedNikahDate =
                entity.ProposedNikahDate,

            Venue =
                entity.Venue,

            // Bride
            BrideMembershipNo =
                entity.BrideMembershipNo,

            BrideName =
                entity.BrideName,

            BrideDateOfBirth =
                entity.BrideDateOfBirth,

            BrideResidentOf =
                entity.BrideResidentOf,

            BrideGenotype =
                entity.BrideGenotype,

            BrideBloodGroup =
                entity.BrideBloodGroup,

            BrideMaritalStatus =
                entity.BrideMaritalStatus,

            BrideProposedDowerAmount =
                entity.BrideProposedDowerAmount,

            BrideDowerAmountReceivedInCash =
                entity.BrideDowerAmountReceivedInCash,

            BrideSignatureTel =
                entity.BrideSignatureTel,

            // Bridegroom
            BridegroomMembershipNo =
                entity.BridegroomMembershipNo,

            BridegroomName =
                entity.BridegroomName,

            BridegroomDateOfBirth =
                entity.BridegroomDateOfBirth,

            BridegroomResidentOf =
                entity.BridegroomResidentOf,

            BridegroomGenotype =
                entity.BridegroomGenotype,

            BridegroomBloodGroup =
                entity.BridegroomBloodGroup,

            BridegroomDowerAmountPaidInCash =
                entity.BridegroomDowerAmountPaidInCash,

            BridegroomDowerAmountToBePaid =
                entity.BridegroomDowerAmountToBePaid,

            IsFirstNikah =
                entity.IsFirstNikah,

            IsSecondThirdOrFourthNikah =
                entity.IsSecondThirdOrFourthNikah,

            FormerWifeIsDead =
                entity.FormerWifeIsDead,

            HasDivorcedFormerWife =
                entity.HasDivorcedFormerWife,

            FormerWifeIsPresent =
                entity.FormerWifeIsPresent,

            FormerWifeObtainedKhula =
                entity.FormerWifeObtainedKhula,

            BridegroomSignatureTel =
                entity.BridegroomSignatureTel,

            // Parents
            BrideFatherName =
                entity.BrideFatherName,

            BridegroomFatherName =
                entity.BridegroomFatherName,

            // Guardian
            GuardianName =
                entity.GuardianName,

            GuardianRelationToBride =
                entity.GuardianRelationToBride,

            GuardianAddress =
                entity.GuardianAddress,

            GuardianTel =
                entity.GuardianTel,

            GuardianSignatureDate =
                entity.GuardianSignatureDate,

            // Representative
            RepresentativeName =
                entity.RepresentativeName,

            RepresentativeAddress =
                entity.RepresentativeAddress,

            RepresentativeActingFor =
                entity.RepresentativeActingFor,

            RepresentativeSignatureDate =
                entity.RepresentativeSignatureDate,

            // Verification & Approval
            OfficiatingImamName =
                entity.OfficiatingImamName,

            OfficiatingImamAddressJamaat =
                entity.OfficiatingImamAddressJamaat,

            OfficiatingImamSignatureDate =
                entity.OfficiatingImamSignatureDate,

            JamaatPresidentName =
                entity.JamaatPresidentName,

            JamaatPresidentSignatureDate =
                entity.JamaatPresidentSignatureDate,

            NationalRishtanataSecretaryName =
                entity.NationalRishtanataSecretaryName,

            NationalRishtanataSecretarySignatureDate =
                entity.NationalRishtanataSecretarySignatureDate,

            ApprovedDateOfNikah =
                entity.ApprovedDateOfNikah,

            NationalAmirOrMissionarySignatureDate =
                entity.NationalAmirOrMissionarySignatureDate
        };
    }
    // MarriageApplicationForm ViewModel → DTO

    public static MarriageApplicationFormDto ToDto(
        MarriageApplicationFormViewModel model)
    {
        return new MarriageApplicationFormDto
        {
            // Application
            MarriageApplicationId =
                model.MarriageApplicationId,

            ReferenceNumber =
                model.ReferenceNumber,

            ProposedNikahDate =
                model.ProposedNikahDate,

            Venue =
                model.Venue,

            // Bride
            BrideMembershipNo =
                model.BrideMembershipNo,

            BrideName =
                model.BrideName,

            BrideDateOfBirth =
                model.BrideDateOfBirth,

            BrideResidentOf =
                model.BrideResidentOf,

            BrideGenotype =
                model.BrideGenotype,

            BrideBloodGroup =
                model.BrideBloodGroup,

            BrideMaritalStatus =
                model.BrideMaritalStatus,

            BrideProposedDowerAmount =
                model.BrideProposedDowerAmount,

            BrideDowerAmountReceivedInCash =
                model.BrideDowerAmountReceivedInCash,

            BrideSignatureTel =
                model.BrideSignatureTel,

            // Bridegroom
            BridegroomMembershipNo =
                model.BridegroomMembershipNo,

            BridegroomName =
                model.BridegroomName,

            BridegroomDateOfBirth =
                model.BridegroomDateOfBirth,

            BridegroomResidentOf =
                model.BridegroomResidentOf,

            BridegroomGenotype =
                model.BridegroomGenotype,

            BridegroomBloodGroup =
                model.BridegroomBloodGroup,

            BridegroomDowerAmountPaidInCash =
                model.BridegroomDowerAmountPaidInCash,

            BridegroomDowerAmountToBePaid =
                model.BridegroomDowerAmountToBePaid,

            IsFirstNikah =
                model.IsFirstNikah,

            IsSecondThirdOrFourthNikah =
                model.IsSecondThirdOrFourthNikah,

            FormerWifeIsDead =
                model.FormerWifeIsDead,

            HasDivorcedFormerWife =
                model.HasDivorcedFormerWife,

            FormerWifeIsPresent =
                model.FormerWifeIsPresent,

            FormerWifeObtainedKhula =
                model.FormerWifeObtainedKhula,

            BridegroomSignatureTel =
                model.BridegroomSignatureTel,

            // Parents
            BrideFatherName =
                model.BrideFatherName,

            BridegroomFatherName =
                model.BridegroomFatherName,

            // Guardian
            GuardianName =
                model.GuardianName,

            GuardianRelationToBride =
                model.GuardianRelationToBride,

            GuardianAddress =
                model.GuardianAddress,

            GuardianTel =
                model.GuardianTel,

            GuardianSignatureDate =
                model.GuardianSignatureDate,

            // Representative
            RepresentativeName =
                model.RepresentativeName,

            RepresentativeAddress =
                model.RepresentativeAddress,

            RepresentativeActingFor =
                model.RepresentativeActingFor,

            RepresentativeSignatureDate =
                model.RepresentativeSignatureDate,

            // Witnesses
            WitnessIds = model.Witnesses
                .Select(w => w.Id)
                .ToList(),

            // Verification & Approval
            OfficiatingImamName =
                model.OfficiatingImamName,

            OfficiatingImamAddressJamaat =
                model.OfficiatingImamAddressJamaat,

            OfficiatingImamSignatureDate =
                model.OfficiatingImamSignatureDate,

            JamaatPresidentName =
                model.JamaatPresidentName,

            JamaatPresidentSignatureDate =
                model.JamaatPresidentSignatureDate,

            NationalRishtanataSecretaryName =
                model.NationalRishtanataSecretaryName,

            NationalRishtanataSecretarySignatureDate =
                model.NationalRishtanataSecretarySignatureDate,

            ApprovedDateOfNikah =
                model.ApprovedDateOfNikah,

            NationalAmirOrMissionarySignatureDate =
                model.NationalAmirOrMissionarySignatureDate
        };
    }
    // Witness DTO → Entity

    public static Witness ToWitnessEntity(
        WitnessDto dto)
    {
        return new Witness
        {
            Id = dto.Id,

            MarriageApplicationFormId =
                dto.MarriageApplicationFormId,

            FullName =
                dto.FullName,

            Email =
                dto.Email,

            PhoneNumber =
                dto.PhoneNumber,

            SignatureDate =
                dto.SignatureDate,

            Role =
                dto.Role,

            WitnessNumber =
                dto.WitnessNumber,

            InvitationToken =
                dto.InvitationToken,

            IsCompleted =
                dto.IsCompleted,

            CompletedAt =
                dto.CompletedAt
        };
    }
    // Witness Entity → DTO

    public static WitnessDto ToWitnessDto(
        Witness entity)
    {
        return new WitnessDto
        {
            Id = entity.Id,

            MarriageApplicationFormId =
                entity.MarriageApplicationFormId,

            FullName =
                entity.FullName,

            Email =
                entity.Email,

            PhoneNumber =
                entity.PhoneNumber,

            SignatureDate =
                entity.SignatureDate,

            Role =
                entity.Role,

            WitnessNumber =
                entity.WitnessNumber,

            InvitationToken =
                entity.InvitationToken,

            IsCompleted =
                entity.IsCompleted,

            CompletedAt =
                entity.CompletedAt
        };
    }

    // ============================================
    // Witness DTO → ViewModel
    // ============================================
    public static WitnessViewModel ToViewModel(WitnessDto dto)
    {
        return new WitnessViewModel
        {
            Id = dto.Id,

            MarriageApplicationFormId =
                dto.MarriageApplicationFormId,

            FullName =
                dto.FullName,

            Email =
                dto.Email,

            PhoneNumber =
                dto.PhoneNumber,

            SignatureDate =
                dto.SignatureDate ?? string.Empty,

            Role =
                dto.Role,

            WitnessNumber =
                dto.WitnessNumber,

            InvitationToken =
                dto.InvitationToken,

            IsCompleted =
                dto.IsCompleted,

            CompletedAt =
                dto.CompletedAt
        };
    }
}