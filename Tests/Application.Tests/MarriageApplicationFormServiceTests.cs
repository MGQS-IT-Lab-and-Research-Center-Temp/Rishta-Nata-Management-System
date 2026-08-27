using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Application.Tests;

public sealed class MarriageApplicationFormServiceTests : IDisposable
{
    private readonly RishtanataDbContext _context;
    private readonly NotificationSpy _notifications = new();

    public MarriageApplicationFormServiceTests()
    {
        var options = new DbContextOptionsBuilder<RishtanataDbContext>()
            .UseInMemoryDatabase($"FormServiceTests_{Guid.NewGuid():N}")
            .Options;
        _context = new RishtanataDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task RevertStageAsync_AuthorizedVerifier_RecordsRevertAndNotifies()
    {
        var seed = await SeedAsync(ApplicationStage.NationalRishtanataSecretaryVerification);
        var result = await CreateSut().RevertStageAsync(
            seed.FormId,
            ApplicationStage.JamaatPresidentReview,
            "Please correct the recommendation.",
            seed.SecretaryId);

        var form = await _context.MarriageApplicationForms
            .Include(x => x.Rejections)
            .SingleAsync(x => x.Id == seed.FormId);

        Assert.Equal(RevertStageResult.Success, result);

        Assert.Equal(ApplicationStage.JamaatPresidentReview, form.ApplicationStage);
        var rejection = Assert.Single(form.Rejections);
        Assert.Equal(ApplicationStage.NationalRishtanataSecretaryVerification, rejection.RejectedAtStage);
        Assert.Equal(seed.SecretaryId, rejection.CreatedBy);
        Assert.Same(rejection, _notifications.Rejection);
    }

    [Fact]
    public async Task RevertStageAsync_UnauthorizedVerifier_DoesNotWrite()
    {
        var seed = await SeedAsync(ApplicationStage.NationalRishtanataSecretaryVerification);
        var result = await CreateSut().RevertStageAsync(
            seed.FormId,
            ApplicationStage.JamaatPresidentReview,
            "Please correct the recommendation.",
            seed.MemberId);

        Assert.Equal(RevertStageResult.Unauthorized, result);

        Assert.Empty(await _context.MarriageFormRejections.ToListAsync());
        Assert.Empty(_notifications.Calls);
    }

    private MarriageApplicationFormService CreateSut() => new(
        _context,
        NullLogger<MarriageApplicationFormService>.Instance,
        new StageAuthorizationService(_context, NullLogger<StageAuthorizationService>.Instance),
        _notifications);

    private async Task<SeedResult> SeedAsync(ApplicationStage stage)
    {
        var secretaryRole = new Role { Name = "National Rishtanata Secretary", HierarchyLevel = 4, UpdatedBy = "test" };
        var memberRole = new Role { Name = "Jama'at Member", HierarchyLevel = 1, UpdatedBy = "test" };
        var secretary = CreateMember(secretaryRole, "3001");
        var member = CreateMember(memberRole, "5001");
        var form = new MarriageApplicationForm
        {
            ApplicationStage = stage,
            BrideMembershipNo = "1001",
            BridegroomMembershipNo = "1002",
            MarriageApplication = new FormApplication
            {
                Status = ApplicationStatus.ApplicationPending,
                AppliedAt = DateTime.UtcNow,
                CertificateId = Guid.NewGuid()
            }
        };
        form.MarriageApplication.MarriageApplicationForm = form;
        form.MarriageApplication.MarriageApplicationFormId = form.Id;
        form.MarriageApplicationId = form.MarriageApplication.Id;

        _context.JamaatRoles.AddRange(secretaryRole, memberRole);
        _context.JamaatMembers.AddRange(secretary, member);
        _context.FormApplications.Add(form.MarriageApplication);
        await _context.SaveChangesAsync();

        return new SeedResult(form.Id, secretary.Id, member.Id);
    }

    private static JamaatMember CreateMember(Role role, string chandaNo) => new()
    {
        ChandaNo = chandaNo,
        FirstName = "Test",
        Surname = "Member",
        Email = $"{chandaNo}@test.local",
        Role = role,
        RoleId = role.Id,
        JamaatName = "Lagos"
    };

    private sealed record SeedResult(Guid FormId, Guid SecretaryId, Guid MemberId);

    private sealed class NotificationSpy : IMarriageFormNotificationService
    {
        public List<MarriageFormRejection> Calls { get; } = new();
        public MarriageFormRejection? Rejection => Calls.SingleOrDefault();

        public Task NotifyRevertedAsync(MarriageApplicationForm form, MarriageFormRejection rejection, CancellationToken cancellationToken = default)
        {
            Calls.Add(rejection);
            return Task.CompletedTask;
        }
    }
}