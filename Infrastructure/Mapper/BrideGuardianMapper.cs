using Domain.Entities;
using Infrastructure.DTOs.BrideGuardian;

namespace Infrastructure.Mapper;

public static class BrideGuardianMapper
{
    public static BrideGuardianDto ToDto(BrideGuardian entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new BrideGuardianDto
        {
            BrideGuardianId = entity.BrideGuardianId,
            MarriageApplicationId = entity.MarriageApplicationId,
            ReferenceNumber = entity.ReferenceNumber,
            BrideIds = entity.Brides.Select(x => x.Id).ToList(),
            GuardianName = entity.GuardianName,
            GuardianRelationToBride = entity.GuardianRelationToBride,
            GuardianAddress = entity.GuardianAddress,
            GuardianTel = entity.GuardianTel,
            GuardianSignatureDate = entity.GuardianSignatureDate
        };
    }

    public static BrideGuardian ToEntity(BrideGuardianDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new BrideGuardian
        {
            BrideGuardianId = dto.BrideGuardianId,
            MarriageApplicationId = dto.MarriageApplicationId,
            ReferenceNumber = dto.ReferenceNumber,
            GuardianName = dto.GuardianName,
            GuardianRelationToBride = dto.GuardianRelationToBride,
            GuardianAddress = dto.GuardianAddress,
            GuardianTel = dto.GuardianTel,
            GuardianSignatureDate = dto.GuardianSignatureDate
        };
    }
}