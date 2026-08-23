using Application.Interfaces.Service;
using Domain.Entities;
using Infrastructure.DTOs.Bride;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BrideFormSectionService : IBrideFormSectionService
{
    private readonly RishtanataDbContext _context;

    public BrideFormSectionService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<BrideDto> CreateAsync(CreateBrideFormSectionDto dto)
    {
        var bride = new BrideFormSection
        {
            MarriageApplicationFormId = dto.MarriageApplicationFormId,
            MembershipNo = dto.MembershipNo,
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            ResidentOf = dto.ResidentOf,
            Genotype = dto.Genotype,
            BloodGroup = dto.BloodGroup,
            MaritalStatus = dto.MaritalStatus,
            ProposedDowerAmount = dto.ProposedDowerAmount,
            DowerAmountReceivedInCash = dto.DowerAmountReceivedInCash,
            SignatureTel = dto.SignatureTel
        };

        _context.BrideFormSections.Add(bride);
        await _context.SaveChangesAsync();

        return new BrideDto
        {
            Id = bride.Id,
            MarriageApplicationFormId = bride.MarriageApplicationFormId,
            MembershipNo = bride.MembershipNo,
            Name = bride.Name,
            DateOfBirth = bride.DateOfBirth,
            ResidentOf = bride.ResidentOf,
            Genotype = bride.Genotype,
            BloodGroup = bride.BloodGroup,
            MaritalStatus = bride.MaritalStatus,
            ProposedDowerAmount = bride.ProposedDowerAmount,
            DowerAmountReceivedInCash = bride.DowerAmountReceivedInCash,
            SignatureTel = bride.SignatureTel
        };
    }

    public async Task<BrideDto?> GetByIdAsync(Guid id)
    {
        var bride = await _context.BrideFormSections.FindAsync(id);

        if (bride == null)
            return null;

        return new BrideDto
        {
            Id = bride.Id,
            MarriageApplicationFormId = bride.MarriageApplicationFormId,
            MembershipNo = bride.MembershipNo,
            Name = bride.Name,
            DateOfBirth = bride.DateOfBirth,
            ResidentOf = bride.ResidentOf,
            Genotype = bride.Genotype,
            BloodGroup = bride.BloodGroup,
            MaritalStatus = bride.MaritalStatus,
            ProposedDowerAmount = bride.ProposedDowerAmount,
            DowerAmountReceivedInCash = bride.DowerAmountReceivedInCash,
            SignatureTel = bride.SignatureTel
        };
    }

    public async Task<BrideDto?> GetByMarriageApplicationFormIdAsync(Guid marriageApplicationFormId)
    {
        var bride = await _context.BrideFormSections
            .FirstOrDefaultAsync(b => b.MarriageApplicationFormId == marriageApplicationFormId);

        if (bride == null)
            return null;

        return await GetByIdAsync(bride.Id);
    }

    public async Task UpdateAsync(Guid id, UpdateBrideFormSectionDto dto)
    {
        var bride = await _context.BrideFormSections.FindAsync(id);

        if (bride == null)
            throw new Exception("Bride not found.");

        bride.MembershipNo = dto.MembershipNo;
        bride.Name = dto.Name;
        bride.DateOfBirth = dto.DateOfBirth;
        bride.ResidentOf = dto.ResidentOf;
        bride.Genotype = dto.Genotype;
        bride.BloodGroup = dto.BloodGroup;
        bride.MaritalStatus = dto.MaritalStatus;
        bride.ProposedDowerAmount = dto.ProposedDowerAmount;
        bride.DowerAmountReceivedInCash = dto.DowerAmountReceivedInCash;
        bride.SignatureTel = dto.SignatureTel;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var bride = await _context.BrideFormSections.FindAsync(id);

        if (bride == null)
            return;

        _context.BrideFormSections.Remove(bride);
        await _context.SaveChangesAsync();
    }
}



