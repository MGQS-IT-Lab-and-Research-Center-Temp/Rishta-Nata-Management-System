using Application.Interfaces;
using Domain.Enums;
using Infrastructure.DTOs.Imam;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ImamSignoffService : IImamSignoffService
{
    private readonly RishtanataDbContext _context;

    public ImamSignoffService(RishtanataDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ImamPendingApplicationDto>> GetPendingAsync(
        string membershipNo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(membershipNo))
        {
            return Array.Empty<ImamPendingApplicationDto>();
        }

        return await _context.MarriageApplicationForms
            .Where(f => f.FormStage == MarriageFormStage.AwaitingImamSignoff)
            .Where(f => f.OfficiatingImamMembershipNo == membershipNo)
            .OrderBy(f => f.ModifiedAt)
            .Select(f => new ImamPendingApplicationDto
            {
                FormId = f.Id,
                MarriageApplicationId = f.MarriageApplicationId,
                ReferenceNumber = f.ReferenceNumber,
                BrideName = f.BrideName,
                BridegroomName = f.BridegroomName,
                ApprovedDateOfNikah = f.ApprovedDateOfNikah,
                Venue = f.Venue,
                SignatureDate = f.ImamVerification != null ? f.ImamVerification.SignatureDate : string.Empty
            })
            .ToListAsync(cancellationToken);
    }
}