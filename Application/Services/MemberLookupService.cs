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
    private const int MaxGatewayAttempts = 3;
    private static readonly TimeSpan InitialRetryDelay = TimeSpan.FromMilliseconds(200);
    private static readonly TimeSpan AttemptTimeout = TimeSpan.FromSeconds(5);

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

        var member = await LookupGatewayWithBackoffAsync(no, cancellationToken);

        member ??= await _context.JamaatMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ChandaNo == no, cancellationToken);

        return member is null ? null : Map(member, no);
    }

    private async Task<JamaatMember?> LookupGatewayWithBackoffAsync(
        string no,
        CancellationToken cancellationToken)
    {
        for (var attempt = 1; attempt <= MaxGatewayAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                using var attemptCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                attemptCts.CancelAfter(AttemptTimeout);

                return await _gateway.GetMemberByChandaNoAsync(no, attemptCts.Token);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex) when (attempt < MaxGatewayAttempts)
            {
                var delay = TimeSpan.FromMilliseconds(
                    InitialRetryDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));

                _logger.LogWarning(
                    ex,
                    "Gateway member lookup failed for {ChandaNo} (attempt {Attempt}/{MaxAttempts}); retrying in {DelayMs}ms.",
                    no, attempt, MaxGatewayAttempts, delay.TotalMilliseconds);

                await Task.Delay(delay, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Gateway member lookup failed for {ChandaNo} after {MaxAttempts} attempts; falling back to local cache.",
                    no, MaxGatewayAttempts);

                return null;
            }
        }

        return null;
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
