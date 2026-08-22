using Domain.Entities;
using Infrastructure.DTOs.Bride;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;

namespace Infrastructure.Mapper;

public static class MarriageApplicationFormMapper
{
    // ============================================================
    // BRIDE ENTITY -> BRIDE DTO
    // ============================================================

    public static BrideDto ToBrideDto(this Bride entity)
    {
        return new BrideDto
        {
            Id = entity.Id,
            MarriageApplicationFormId =
                entity.MarriageApplicationFormId,

            MembershipNo = entity.MembershipNo,
            Name = entity.Name,
            DateOfBirth = entity.DateOfBirth,
            ResidentOf = entity.ResidentOf,
            Genotype = entity.Genotype,
            BloodGroup = entity.BloodGroup,
            MaritalStatus = entity.MaritalStatus,

            ProposedDowerAmount =
                entity.ProposedDowerAmount,

            DowerAmountReceivedInCash =
                entity.DowerAmountReceivedInCash,

            SignatureTel = entity.SignatureTel
        };
    }


    // ============================================================
    // BRIDE DTO -> BRIDE ENTITY
    // ============================================================

    public static Bride ToEntity(this BrideDto dto)
    {
        return new Bride
        {
            Id = dto.Id,
            MarriageApplicationFormId =
                dto.MarriageApplicationFormId,

            MembershipNo = dto.MembershipNo,
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            ResidentOf = dto.ResidentOf,
            Genotype = dto.Genotype,
            BloodGroup = dto.BloodGroup,
            MaritalStatus = dto.MaritalStatus,

            ProposedDowerAmount =
                dto.ProposedDowerAmount,

            DowerAmountReceivedInCash =
                dto.DowerAmountReceivedInCash,

            SignatureTel = dto.SignatureTel
        };
    }


    // ============================================================
    // PENDING APPROVAL DTO
    // ============================================================

    public static PendingApprovalDto ToPendingApprovalDto(
        this MarriageApplicationForm form)
    {
        return new PendingApprovalDto
        {
            Id = form.MarriageApplicationId,

            ApplicationNumber =
                form.ReferenceNumber,

            GroomName =
                form.BridegroomName,

            BrideName =
                form.Bride?.Name ?? string.Empty,

            PresidentName =
                form.JamaatPresidentName,

            SubmittedDate =
                form.CreatedAt,

            Status =
                form.MarriageApplication.Status.ToString()
        };
    }


    // ============================================================
    // REVIEW APPLICATION DTO
    // ============================================================

    public static ReviewApplicationDto ToReviewApplicationDto(
        this MarriageApplicationForm form)
    {
        return new ReviewApplicationDto
        {
            Id = form.MarriageApplicationId,

            ApplicationNumber =
                form.ReferenceNumber,

            GroomName =
                form.BridegroomName,

            BrideName =
                form.Bride?.Name ?? string.Empty,

            GroomPhone =
                form.BridegroomSignatureTel,

            BridePhone =
                form.Bride?.SignatureTel ?? string.Empty,

            PresidentName =
                form.JamaatPresidentName,

            SubmittedDate =
                form.CreatedAt,

            Status =
                form.MarriageApplication.Status.ToString()
        };
    }


    // ============================================================
    // MARRIED COUPLE DTO
    // ============================================================

    public static MarriedCoupleDto ToMarriedCoupleDto(
        this MarriageApplicationForm form)
    {
        return new MarriedCoupleDto
        {
            Id =
                form.MarriageApplicationId,

            CertificateNumber =
                form.MarriageApplication.CertificateId,

            HusbandName =
                form.BridegroomName,

            WifeName =
                form.Bride?.Name ?? string.Empty,

            MarriageDate =
                form.ApprovedDateOfNikah
                ?? DateTime.MinValue,

            Status =
                form.MarriageApplication.Status.ToString()
        };
    }
}