using Infrastructure.DTOs.Certificates;
using Infrastructure.DTOs.JamaatPresidentDashboardDto;
using Presentation.ViewModels;

namespace Presentation.Mapping;

public static class JamaatPresidentMapping
{
    public static JamaatPresidentDashboardViewModel ToViewModel(
        JamaatPresidentDashboardDto dto)
    {
        return new JamaatPresidentDashboardViewModel
        {
            PresidentName = dto.PresidentName,
            JamaatName = dto.JamaatName,
            CircuitName = dto.CircuitName,
            PendingNikahReviews = dto.PendingNikahReviews,
            ReviewedToday = dto.ReviewedToday,
            TotalNikahApplications = dto.TotalNikahApplications,
            PendingApplications = dto.PendingApplications
                .Select(ToViewModel)
                .ToList(),
            RecentActivities = dto.RecentActivities
                .Select(ToViewModel)
                .ToList()
        };
    }

    public static NikahApplicationViewModel ToViewModel(NikahApplicationDto dto)
    {
        return new NikahApplicationViewModel
        {
            Id = dto.Id,
            ReferenceNumber = dto.ReferenceNumber,
            GroomName = dto.GroomName,
            BrideName = dto.BrideName,
            JamaatName = dto.JamaatName,
            SubmittedDate = dto.SubmittedDate,
            Status = dto.Status
        };
    }

    public static RecentActivityViewModel ToViewModel(RecentActivityDto dto)
    {
        return new RecentActivityViewModel
        {
            ApplicationNumber = dto.ApplicationNumber,
            Description = dto.Description,
            Date = dto.Date
        };
    }

    public static JamaatPresidentReviewViewModel ToViewModel(
        JamaatPresidentReviewDto dto)
    {
        return new JamaatPresidentReviewViewModel
        {
            Id = dto.Id,
            ReferenceNumber = dto.ReferenceNumber,
            Status = dto.Status,
            SubmittedDate = dto.SubmittedDate,
            ProposedNikahDate = dto.ProposedNikahDate,
            Venue = dto.Venue,

            BrideMembershipNo = dto.BrideMembershipNo,
            BrideName = dto.BrideName,
            BrideDateOfBirth = dto.BrideDateOfBirth,
            BrideResidentOf = dto.BrideResidentOf,
            BrideGenotype = dto.BrideGenotype,
            BrideBloodGroup = dto.BrideBloodGroup,
            BrideMaritalStatus = dto.BrideMaritalStatus,
            BrideProposedDowerAmount = dto.BrideProposedDowerAmount,
            BrideDowerAmountReceivedInCash = dto.BrideDowerAmountReceivedInCash,
            BrideSignatureTel = dto.BrideSignatureTel,

            BridegroomMembershipNo = dto.BridegroomMembershipNo,
            BridegroomName = dto.BridegroomName,
            BridegroomDateOfBirth = dto.BridegroomDateOfBirth,
            BridegroomResidentOf = dto.BridegroomResidentOf,
            BridegroomGenotype = dto.BridegroomGenotype,
            BridegroomBloodGroup = dto.BridegroomBloodGroup,
            BridegroomDowerAmountPaidInCash = dto.BridegroomDowerAmountPaidInCash,
            BridegroomDowerAmountToBePaid = dto.BridegroomDowerAmountToBePaid,
            IsFirstNikah = dto.IsFirstNikah,
            IsSecondThirdOrFourthNikah = dto.IsSecondThirdOrFourthNikah,
            FormerWifeIsDead = dto.FormerWifeIsDead,
            HasDivorcedFormerWife = dto.HasDivorcedFormerWife,
            FormerWifeIsPresent = dto.FormerWifeIsPresent,
            FormerWifeObtainedKhula = dto.FormerWifeObtainedKhula,
            BridegroomSignatureTel = dto.BridegroomSignatureTel,

            BrideFatherName = dto.BrideFatherName,
            BridegroomFatherName = dto.BridegroomFatherName,

            GuardianName = dto.GuardianName,
            GuardianRelationToBride = dto.GuardianRelationToBride,
            GuardianAddress = dto.GuardianAddress,
            GuardianTel = dto.GuardianTel,
            GuardianSignatureDate = dto.GuardianSignatureDate,

            RepresentativeName = dto.RepresentativeName,
            RepresentativeAddress = dto.RepresentativeAddress,
            RepresentativeActingFor = dto.RepresentativeActingFor,
            RepresentativeSignatureDate = dto.RepresentativeSignatureDate,

            WitnessOneName = dto.WitnessOneName,
            WitnessOneAddress = dto.WitnessOneAddress,
            WitnessOneTel = dto.WitnessOneTel,
            WitnessOneSignatureDate = dto.WitnessOneSignatureDate,

            WitnessTwoName = dto.WitnessTwoName,
            WitnessTwoAddress = dto.WitnessTwoAddress,
            WitnessTwoTel = dto.WitnessTwoTel,
            WitnessTwoSignatureDate = dto.WitnessTwoSignatureDate,

            OfficiatingImamName = dto.OfficiatingImamName,
            OfficiatingImamAddressJamaat = dto.OfficiatingImamAddressJamaat,
            OfficiatingImamSignatureDate = dto.OfficiatingImamSignatureDate,

            JamaatPresidentName = dto.JamaatPresidentName,
            JamaatPresidentSignatureDate = dto.JamaatPresidentSignatureDate,

            NationalRishtanataSecretaryName = dto.NationalRishtanataSecretaryName,
            NationalRishtanataSecretarySignatureDate = dto.NationalRishtanataSecretarySignatureDate,

            ApprovedDateOfNikah = dto.ApprovedDateOfNikah,
            NationalAmirOrMissionarySignatureDate = dto.NationalAmirOrMissionarySignatureDate
        };
    }

    public static CertificateViewModel ToViewModel(CertificateDto dto)
    {
        return new CertificateViewModel
        {
            Id = dto.Id,
            SerialNumber = dto.SerialNumber,
            BrideName = dto.BrideName,
            BridegroomName = dto.BridegroomName,
            NikahDate = dto.NikahDate,
            IssueDate = dto.IssueDate,
            CertificateFilePath = dto.CertificateFilePath
        };
    }
}
