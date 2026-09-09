using Application.Interfaces;
using Application.Interfaces.Gateway;
using Domain.Entities;
using Infrastructure.DTOs.Members;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class MemberLookupService : IMemberLookupService
{
    private readonly IGatewayHandler _gateway;
    private readonly RishtanataDbContext _context;
    private readonly ILogger<MemberLookupService> _logger;

    public MemberLookupService(
        IGatewayHandler gateway,
        RishtanataDbContext context,
        ILogger<MemberLookupService> logger)
    {
        _gateway = gateway;
        _context = context;
        _logger = logger;
    }

    public async Task<MemberLookupDto?> LookupAsync(
        string chandaNo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(chandaNo))
            return null;

        var no = chandaNo.Trim();

        var member = await LookupGatewayAsync(no, cancellationToken);

        member ??= await _context.JamaatMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ChandaNo == no, cancellationToken);

        return member is null ? null : Map(member, no);
    }

    // Retry/backoff, the attempt and total-request timeouts, and the circuit
    // breaker all live on the shared IGatewayHandler HttpClient
    // (AddStandardResilienceHandler in Presentation/Extensions/DependencyInjection.cs),
    // so a single call here is enough; no hand-rolled retry loop is stacked on top.
    private async Task<JamaatMember?> LookupGatewayAsync(
        string no,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _gateway.GetMemberByMemberNoAsync(no, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Gateway member lookup failed for {ChandaNo}; falling back to local cache.",
                no);

            return null;
        }
    }

    private static MemberLookupDto Map(JamaatMember member, string chandaNo)
    {
        return new MemberLookupDto
        {
            ChandaNo = chandaNo,
            FirstName = member.FirstName,
            MiddleName = member.MiddleName ?? string.Empty,
            Surname = member.Surname,
            FullName = BuildFullName(member.FirstName, member.Surname),
            PhoneNo = member.PhoneNo ?? string.Empty,
            Address = member.Address ?? string.Empty,
            JamaatName = member.JamaatName,
            DateOfBirth = member.DateOfBirth.Year > 1 ? member.DateOfBirth : null
        };
    }

    private static string BuildFullName(string? firstName, string? surname) =>
        $"{firstName} {surname}".Trim();
}
