using Domain.Entities;
using Infrastructure.DTOs;

namespace Infrastructure.Mappings;

public static class WitnessFormMapper
{
    public static Witness ToEntity(WitnessDto dto)
    {
        return new Witness
        {
            Id = dto.Id,

            MarriageApplicationFormId =
                dto.MarriageApplicationFormId,

            FullName =
                dto.FullName ?? string.Empty,

            Email =
                dto.Email ?? string.Empty,

            PhoneNumber =
                dto.PhoneNumber ?? string.Empty,

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

    public static WitnessDto ToDto(Witness entity)
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
                entity.SignatureDate ?? string.Empty,

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
}