using Application.Authorization;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Application.Tests;

/// Unit tests for the stage-authorization policy.

// Required coverage (AC):
//   1. correct role + correct stage → allow
//   2. correct role + wrong stage   → deny (WrongStage)
//   3. wrong role entirely          → deny (WrongRole)
//   4. no matching membership record found → deny (UnknownMember)

// Uses the EF Core InMemory provider; each test gets an isolated database.

public sealed class StageAuthorizationServiceTests : IDisposable
{
    // Chanda numbers printed on the seeded form (Kind A parties, policy §4.1).
    private const string BrideChandaNo = "1001";
    private const string GroomChandaNo = "1002";

    private readonly RishtanataDbContext _context;

    public StageAuthorizationServiceTests()
    {
        var options = new DbContextOptionsBuilder<RishtanataDbContext>()
            .UseInMemoryDatabase($"StageAuthTests_{Guid.NewGuid():N}")
            .Options;

        _context = new RishtanataDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private StageAuthorizationService CreateSut() =>
        new(_context, NullLogger<StageAuthorizationService>.Instance);

    // =====================================================================
    // AC 1 — correct role + correct stage → allow
    // =====================================================================

    [Fact]
    public async Task CanUserActAsync_PresidentAtPresidentReviewStage_Allows()
    {
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.PresidentId, seed.FormId, ApplicationStage.JamaatPresidentReview);

        Assert.True(result.IsAllowed);
        Assert.Null(result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_SecretaryAtSecretaryVerificationStage_Allows()
    {
        var seed = await SeedAsync(ApplicationStage.NationalRishtanataSecretaryVerification);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.SecretaryId, seed.FormId,
            ApplicationStage.NationalRishtanataSecretaryVerification);

        Assert.True(result.IsAllowed);
        Assert.Null(result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_AmirAtAmirApprovalStage_Allows()
    {
        var seed = await SeedAsync(ApplicationStage.AmirApproval);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.AmirId, seed.FormId, ApplicationStage.AmirApproval);

        Assert.True(result.IsAllowed);
        Assert.Null(result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_BrideAtApplicantsReviewStage_Allows()
    {
        var seed = await SeedAsync(ApplicationStage.ApplicantsReview);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.BrideId, seed.FormId, ApplicationStage.ApplicantsReview);

        Assert.True(result.IsAllowed);
        Assert.Null(result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_GroomAtApplicantsReviewStage_Allows()
    {
        var seed = await SeedAsync(ApplicationStage.ApplicantsReview);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.GroomId, seed.FormId, ApplicationStage.ApplicantsReview);

        Assert.True(result.IsAllowed);
        Assert.Null(result.Reason);
    }

    // =====================================================================
    // AC 2 — correct role + wrong stage → deny (WrongStage)
    // =====================================================================

    [Fact]
    public async Task CanUserActAsync_PresidentBeforeHisStage_DeniesWithWrongStage()
    {
        // Form is still sitting at the applicants' stage — the president's
        // role would match, but the stage gate must fail 
        var seed = await SeedAsync(ApplicationStage.ApplicantsReview);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.PresidentId, seed.FormId, ApplicationStage.JamaatPresidentReview);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongStage, result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_SecretaryAfterHerStage_DeniesWithWrongStage()
    {
        // Form has moved on to Amir approval — the secretary can no longer act.
        var seed = await SeedAsync(ApplicationStage.AmirApproval);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.SecretaryId, seed.FormId,
            ApplicationStage.NationalRishtanataSecretaryVerification);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongStage, result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_FormNotYetInWorkflow_DeniesWithWrongStage()
    {
        // CurrentStage is null: the form has not entered the staged workflow.
        var seed = await SeedAsync(currentStage: null);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.PresidentId, seed.FormId, ApplicationStage.JamaatPresidentReview);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongStage, result.Reason);
    }

    // =====================================================================
    // AC 3 — wrong role entirely → deny (WrongRole)
    // =====================================================================

    [Fact]
    public async Task CanUserActAsync_OrdinaryMemberAtPresidentStage_DeniesWithWrongRole()
    {
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.OrdinaryMemberId, seed.FormId, ApplicationStage.JamaatPresidentReview);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongRole, result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_OfficeHolderForDifferentOffice_DeniesWithWrongRole()
    {
        // The president cannot act at the Amir approval stage even though he
        // holds an office — it is not HIS office for that stage.
        var seed = await SeedAsync(ApplicationStage.AmirApproval);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.PresidentId, seed.FormId, ApplicationStage.AmirApproval);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongRole, result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_MemberWhoIsNeitherBrideNorGroom_DeniesWithWrongRole()
    {
        // Kind A matching is exact membership-number equality — another
        // member may never act on the applicants' sections
        var seed = await SeedAsync(ApplicationStage.ApplicantsReview);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.OrdinaryMemberId, seed.FormId, ApplicationStage.ApplicantsReview);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongRole, result.Reason);
    }

    // =====================================================================
    // AC 4 — no matching membership record found → deny (UnknownMember)
    // =====================================================================

    [Fact]
    public async Task CanUserActAsync_UserIdResolvesToNoMemberRecord_DeniesWithUnknownMember()
    {
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut();
        var unknownUserId = Guid.NewGuid(); // no JamaatMember row with this id

        var result = await sut.CanUserActAsync(
            unknownUserId, seed.FormId, ApplicationStage.JamaatPresidentReview);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.UnknownMember, result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_EmptyUserId_DeniesWithNoMembershipClaim()
    {
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            Guid.Empty, seed.FormId, ApplicationStage.JamaatPresidentReview);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.NoMembershipClaim, result.Reason);
    }

    // =====================================================================
    // Additional policy coverage
    // =====================================================================

    [Fact]
    public async Task CanUserActAsync_UnknownFormId_DeniesWithFormNotFound()
    {
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.PresidentId, Guid.NewGuid(), ApplicationStage.JamaatPresidentReview);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.FormNotFound, result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_ApprovedFormIsLocked_DeniesWithFormCompleted()
    {
        // Even the right office-holder at the right stage is denied once the
        // form reached final approval.
        var seed = await SeedAsync(
            ApplicationStage.AmirApproval, ApplicationStatus.ApplicationApproved);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.AmirId, seed.FormId, ApplicationStage.AmirApproval);

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.FormCompleted, result.Reason);
    }

    [Fact]
    public async Task CanUserActAsync_MembershipNumberComparison_TrimsAndIgnoresCase()
    {
        // ChandaNo values are compared as trimmed strings,
        // case-insensitive (no zero-padding guarantees downstream).
        var seed = await SeedAsync(ApplicationStage.ApplicantsReview);

        var bride = await _context.JamaatMembers.SingleAsync(m => m.Id == seed.BrideId);
        bride.ChandaNo = $"  {BrideChandaNo.ToUpperInvariant()}  ";
        await _context.SaveChangesAsync();

        var sut = CreateSut();
        var result = await sut.CanUserActAsync(
            seed.BrideId, seed.FormId, ApplicationStage.ApplicantsReview);

        Assert.True(result.IsAllowed);
    }

    [Fact]
    public async Task CanUserActAsync_FormAddressedByOwningApplicationId_IsResolved()
    {
        // Controllers address forms by the owning FormApplication id too;
        // both addressing conventions must resolve to the same decision.
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut();

        var result = await sut.CanUserActAsync(
            seed.PresidentId, seed.ApplicationId, ApplicationStage.JamaatPresidentReview);

        Assert.True(result.IsAllowed);
    }

    // =====================================================================
    // Seeding helpers
    // =====================================================================

    private sealed record SeedResult(
        Guid FormId,
        Guid ApplicationId,
        Guid BrideId,
        Guid GroomId,
        Guid PresidentId,
        Guid SecretaryId,
        Guid AmirId,
        Guid OrdinaryMemberId);

    /// <summary>
    ///   <param name="currentStage"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    /// Seeds roles, members and a pending application whose form sits at
    /// <paramref name="currentStage"/> (or in no stage when null).
    /// </summary>
   
    private async Task<SeedResult> SeedAsync(
        ApplicationStage? currentStage,
        ApplicationStatus status = ApplicationStatus.ApplicationPending)
    {
        // Role.HierarchyLevel contract (Domain/Entities/Role.cs):
        // 1 = Jama'at Member, 2 = Jama'at President, 3 = Circuit President,
        // 4 = National Rishtanata Secretary, 5 = Amir.
        var memberRole = CreateRole("Jama'at Member", hierarchyLevel: 1);
        var presidentRole = CreateRole("Jamaat President", hierarchyLevel: 2);
        var circuitPresidentRole = CreateRole("Circuit President", hierarchyLevel: 3);
        var secretaryRole = CreateRole("National Rishtanata Secretary", hierarchyLevel: 4);
        var amirRole = CreateRole("Amir", hierarchyLevel: 5);

        var bride = CreateMember(memberRole, BrideChandaNo, sex: "F");
        var groom = CreateMember(memberRole, GroomChandaNo, sex: "M");
        var president = CreateMember(presidentRole, "2001", sex: "M");
        var secretary = CreateMember(secretaryRole, "3001", sex: "F");
        var amir = CreateMember(amirRole, "4001", sex: "M");
        var ordinary = CreateMember(memberRole, "5001", sex: "M");

        var form = new MarriageApplicationForm
        {
            ReferenceNumber = "NIK-2026-0001",
            ProposedNikahDate = DateTime.UtcNow.AddDays(30),
            Venue = "Lagos",
            BrideMembershipNo = BrideChandaNo,
            BridegroomMembershipNo = GroomChandaNo,
            ApplicationStage = currentStage
        };

        var application = new FormApplication
        {
            Status = status,
            AppliedAt = DateTime.UtcNow,
            MarriageApplicationFormId = form.Id,
            MarriageApplicationForm = form,
            CertificateId = Guid.NewGuid()
        };
        form.MarriageApplicationId = application.Id;
        form.MarriageApplication = application;

        _context.JamaatRoles.AddRange(
            memberRole, presidentRole, circuitPresidentRole, secretaryRole, amirRole);
        _context.JamaatMembers.AddRange(bride, groom, president, secretary, amir, ordinary);
        _context.FormApplications.Add(application);
        await _context.SaveChangesAsync();

        return new SeedResult(
            FormId: form.Id,
            ApplicationId: application.Id,
            BrideId: bride.Id,
            GroomId: groom.Id,
            PresidentId: president.Id,
            SecretaryId: secretary.Id,
            AmirId: amir.Id,
            OrdinaryMemberId: ordinary.Id);
    }

    private static Role CreateRole(string name, int hierarchyLevel) => new()
    {
        Name = name,
        HierarchyLevel = hierarchyLevel,
        UpdatedBy = "test-seed"
    };

    private static JamaatMember CreateMember(Role role, string chandaNo, string sex) => new()
    {
        Surname = "Abimbola",
        FirstName = "Mukhtar",
        Email = $"{chandaNo}@test.local",
        ChandaNo = chandaNo,
        JamaatName = "Lagos",
        CircuitName = "Lagos Circuit",
        Sex = sex,
        MemberRoles = new List<JamaatMemberRole>
        {
            new() { RoleId = role.Id, Role = role }
        }
    };
}