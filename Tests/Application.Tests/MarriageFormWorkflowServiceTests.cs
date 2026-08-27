using Application.Authorization;
using Application.Services;
using Application.Workflow;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Application.Tests;

/// <summary>
/// Unit tests for the verification/approval chain (backlog D3).
///
/// Covers:
///   - each of the four methods persisting its section and advancing the
///     stage on the happy path;
///   - ApproveByAmirAsync setting ApprovedDateOfNikah and Completed;
///   - denials producing NO side effects (wrong role, skip-a-stage,
///     unknown member, already-completed form);
///   - the full imam → president → secretary → amir chain end to end.
///
/// Uses the EF Core InMemory provider; each test gets an isolated database.
/// </summary>
public sealed class MarriageFormWorkflowServiceTests : IDisposable
{
    private const string BrideChandaNo = "1001";
    private const string GroomChandaNo = "1002";

    private readonly RishtanataDbContext _context;

    public MarriageFormWorkflowServiceTests()
    {
        var options = new DbContextOptionsBuilder<RishtanataDbContext>()
            .UseInMemoryDatabase($"WorkflowTests_{Guid.NewGuid():N}")
            .Options;

        _context = new RishtanataDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private MarriageFormWorkflowService CreateSut()
    {
        var auth = new StageAuthorizationService(
            _context, NullLogger<StageAuthorizationService>.Instance);

        return new MarriageFormWorkflowService(
            _context, auth, NullLogger<MarriageFormWorkflowService>.Instance);
    }

    // =====================================================================
    // Happy paths
    // =====================================================================

    [Fact]
    public async Task SubmitImamVerification_PersistsSection_AndAdvancesToPresident()
    {
        var seed = await SeedAsync(MarriageFormStage.AwaitingImamVerification);
        var sut = CreateSut();

        var result = await sut.SubmitImamVerificationAsync(
            seed.ImamId, seed.FormId,
            new ImamVerificationSubmission("Umar Farouk", "Lagos Central Mosque", "08010000009", "2026-08-05"));

        Assert.True(result.IsAllowed);
        Assert.Null(result.Reason);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.NotNull(form.ImamVerification);
        Assert.Equal("Umar Farouk", form.ImamVerification!.Name);
        Assert.Equal("Lagos Central Mosque", form.ImamVerification.AddressJamaat);
        Assert.Equal(seed.ImamId, form.ImamVerification.CreatedBy);
        Assert.Equal(MarriageFormStage.AwaitingJamaatPresident, form.FormStage);
    }

    [Fact]
    public async Task SubmitJamaatPresidentVerification_PersistsSection_AndAdvancesToSecretary()
    {
        var seed = await SeedAsync(MarriageFormStage.AwaitingJamaatPresident);
        var sut = CreateSut();

        var result = await sut.SubmitJamaatPresidentVerificationAsync(
            seed.PresidentId, seed.FormId,
            new JamaatPresidentVerificationSubmission("President Musa", "08010000008", "2026-08-10"));

        Assert.True(result.IsAllowed);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.NotNull(form.JamaatPresidentVerification);
        Assert.Equal("President Musa", form.JamaatPresidentVerification!.Name);
        Assert.Equal(MarriageFormStage.AwaitingRishtanataSecretary, form.FormStage);
    }

    [Fact]
    public async Task SubmitRishtanataRecommendation_PersistsSection_AndAdvancesToAmir()
    {
        var seed = await SeedAsync(MarriageFormStage.AwaitingRishtanataSecretary);
        var sut = CreateSut();

        var result = await sut.SubmitRishtanataRecommendationAsync(
            seed.SecretaryId, seed.FormId,
            new RishtanataRecommendationSubmission("Wakeel Bello", "Recommended without reservation", "2026-08-15"));

        Assert.True(result.IsAllowed);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.NotNull(form.RishtanataRecommendation);
        Assert.Equal("Recommended without reservation", form.RishtanataRecommendation!.WakeelDeclaration);
        Assert.Equal(MarriageFormStage.AwaitingAmirApproval, form.FormStage);
    }

    [Fact]
    public async Task ApproveByAmir_SetsApprovedDateOfNikah_AndCompletesForm()
    {
        var seed = await SeedAsync(MarriageFormStage.AwaitingAmirApproval);
        var sut = CreateSut();
        var approvedDate = new DateTime(2026, 9, 1);

        var result = await sut.ApproveByAmirAsync(
            seed.AmirId, seed.FormId,
            new AmirApprovalSubmission(approvedDate, "2026-08-20"));

        Assert.True(result.IsAllowed);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.Equal(MarriageFormStage.Completed, form.FormStage);
        Assert.Equal(approvedDate, form.ApprovedDateOfNikah);
        Assert.NotNull(form.AmirApproval);
        Assert.Equal(approvedDate, form.AmirApproval!.ApprovedDateOfNikah);
    }

    [Fact]
    public async Task FullChain_ImamToAmir_EndsWithCompletedForm()
    {
        var seed = await SeedAsync(MarriageFormStage.AwaitingImamVerification);
        var sut = CreateSut();

        Assert.True((await sut.SubmitImamVerificationAsync(
            seed.ImamId, seed.FormId,
            new ImamVerificationSubmission("Umar Farouk", "Lagos Central Mosque", "08010000009", "2026-08-05"))).IsAllowed);

        Assert.True((await sut.SubmitJamaatPresidentVerificationAsync(
            seed.PresidentId, seed.FormId,
            new JamaatPresidentVerificationSubmission("President Musa", "08010000008", "2026-08-10"))).IsAllowed);

        Assert.True((await sut.SubmitRishtanataRecommendationAsync(
            seed.SecretaryId, seed.FormId,
            new RishtanataRecommendationSubmission("Wakeel Bello", "Recommended", "2026-08-15"))).IsAllowed);

        var final = await sut.ApproveByAmirAsync(
            seed.AmirId, seed.FormId,
            new AmirApprovalSubmission(new DateTime(2026, 9, 1), "2026-08-20"));

        Assert.True(final.IsAllowed);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.Equal(MarriageFormStage.Completed, form.FormStage);
        Assert.Equal(new DateTime(2026, 9, 1), form.ApprovedDateOfNikah);
    }

    // =====================================================================
    // Denials — no side effects
    // =====================================================================

    [Fact]
    public async Task SubmitImamVerification_WrongRole_DeniesWithoutSideEffects()
    {
        // The president may not perform the imam's verification.
        var seed = await SeedAsync(MarriageFormStage.AwaitingImamVerification);
        var sut = CreateSut();

        var result = await sut.SubmitImamVerificationAsync(
            seed.PresidentId, seed.FormId,
            new ImamVerificationSubmission("President Musa", "Lagos", "08010000008", "2026-08-05"));

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongRole, result.Reason);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.Null(form.ImamVerification);
        Assert.Equal(MarriageFormStage.AwaitingImamVerification, form.FormStage);
    }

    [Fact]
    public async Task SubmitImamVerification_BeforeWitnessesSigned_DeniesWithWrongStage()
    {
        // AC example: attempting to skip a stage — the imam tries to verify
        // while the form is still sitting at AwaitingWitnesses.
        var seed = await SeedAsync(MarriageFormStage.AwaitingWitnesses);
        var sut = CreateSut();

        var result = await sut.SubmitImamVerificationAsync(
            seed.ImamId, seed.FormId,
            new ImamVerificationSubmission("Umar Farouk", "Lagos Central Mosque", "08010000009", "2026-08-05"));

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongStage, result.Reason);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.Null(form.ImamVerification);
        Assert.Equal(MarriageFormStage.AwaitingWitnesses, form.FormStage);
    }

    [Fact]
    public async Task SubmitJamaatPresidentVerification_WrongRole_DeniesWithoutSideEffects()
    {
        // The imam may not perform the president's verification.
        var seed = await SeedAsync(MarriageFormStage.AwaitingJamaatPresident);
        var sut = CreateSut();

        var result = await sut.SubmitJamaatPresidentVerificationAsync(
            seed.ImamId, seed.FormId,
            new JamaatPresidentVerificationSubmission("Umar Farouk", "08010000009", "2026-08-10"));

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongRole, result.Reason);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.Null(form.JamaatPresidentVerification);
        Assert.Equal(MarriageFormStage.AwaitingJamaatPresident, form.FormStage);
    }

    [Fact]
    public async Task SubmitRishtanataRecommendation_WrongRole_DeniesWithoutSideEffects()
    {
        var seed = await SeedAsync(MarriageFormStage.AwaitingRishtanataSecretary);
        var sut = CreateSut();

        var result = await sut.SubmitRishtanataRecommendationAsync(
            seed.AmirId, seed.FormId,
            new RishtanataRecommendationSubmission("Wakeel Bello", "Recommended", "2026-08-15"));

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongRole, result.Reason);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.Null(form.RishtanataRecommendation);
        Assert.Equal(MarriageFormStage.AwaitingRishtanataSecretary, form.FormStage);
    }

    [Fact]
    public async Task ApproveByAmir_WrongRole_DeniesWithoutSideEffects()
    {
        var seed = await SeedAsync(MarriageFormStage.AwaitingAmirApproval);
        var sut = CreateSut();

        var result = await sut.ApproveByAmirAsync(
            seed.SecretaryId, seed.FormId,
            new AmirApprovalSubmission(new DateTime(2026, 9, 1), "2026-08-20"));

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.WrongRole, result.Reason);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.Null(form.AmirApproval);
        Assert.Null(form.ApprovedDateOfNikah);
        Assert.Equal(MarriageFormStage.AwaitingAmirApproval, form.FormStage);
    }

    [Fact]
    public async Task WorkflowSubmission_UnknownMember_DeniesWithUnknownMember()
    {
        var seed = await SeedAsync(MarriageFormStage.AwaitingImamVerification);
        var sut = CreateSut();

        var result = await sut.SubmitImamVerificationAsync(
            Guid.NewGuid(), seed.FormId,
            new ImamVerificationSubmission("Ghost Member", "Nowhere", "00000000000", "2026-08-05"));

        Assert.False(result.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.UnknownMember, result.Reason);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.Null(form.ImamVerification);
    }

    [Fact]
    public async Task ApproveByAmir_AfterCompletion_DeniesWithFormCompleted()
    {
        var seed = await SeedAsync(MarriageFormStage.AwaitingAmirApproval);
        var sut = CreateSut();

        var first = await sut.ApproveByAmirAsync(
            seed.AmirId, seed.FormId,
            new AmirApprovalSubmission(new DateTime(2026, 9, 1), "2026-08-20"));
        Assert.True(first.IsAllowed);

        // A second approval attempt hits the completed-form lock.
        var second = await sut.ApproveByAmirAsync(
            seed.AmirId, seed.FormId,
            new AmirApprovalSubmission(new DateTime(2026, 9, 2), "2026-08-21"));

        Assert.False(second.IsAllowed);
        Assert.Equal(StageAuthorizationDenyReason.FormCompleted, second.Reason);

        var form = await ReloadFormAsync(seed.FormId);
        Assert.Equal(new DateTime(2026, 9, 1), form.ApprovedDateOfNikah);
    }

    // =====================================================================
    // Helpers
    // =====================================================================

    private async Task<MarriageApplicationForm> ReloadFormAsync(Guid formId) =>
        await _context.MarriageApplicationForms
            .Include(f => f.ImamVerification)
            .Include(f => f.JamaatPresidentVerification)
            .Include(f => f.RishtanataRecommendation)
            .Include(f => f.AmirApproval)
            .SingleAsync(f => f.Id == formId);

    private sealed record SeedResult(
        Guid FormId,
        Guid ImamId,
        Guid PresidentId,
        Guid SecretaryId,
        Guid AmirId,
        Guid OrdinaryMemberId);

    private async Task<SeedResult> SeedAsync(MarriageFormStage formStage)
    {
        // Role.HierarchyLevel contract (Domain/Entities/Role.cs):
        // 1 = Jama'at Member, 2 = Jama'at President, 3 = Circuit President,
        // 4 = National Rishtanata Secretary, 5 = Amir.
        var memberRole = CreateRole("Jama'at Member", hierarchyLevel: 1);
        var imamRole = CreateRole("Officiating Imam", hierarchyLevel: 1);
        var presidentRole = CreateRole("Jamaat President", hierarchyLevel: 2);
        var circuitPresidentRole = CreateRole("Circuit President", hierarchyLevel: 3);
        var secretaryRole = CreateRole("National Rishtanata Secretary", hierarchyLevel: 4);
        var amirRole = CreateRole("Amir", hierarchyLevel: 5);

        var imam = CreateMember(imamRole, "3001");
        var president = CreateMember(presidentRole, "2001");
        var secretary = CreateMember(secretaryRole, "4001");
        var amir = CreateMember(amirRole, "5001");
        var ordinary = CreateMember(memberRole, "6001");

        var form = new MarriageApplicationForm
        {
            ReferenceNumber = "NIK-2026-0001",
            ProposedNikahDate = DateTime.UtcNow.AddDays(30),
            Venue = "Lagos",
            BrideMembershipNo = BrideChandaNo,
            BridegroomMembershipNo = GroomChandaNo,
            FormStage = formStage
        };

        var application = new FormApplication
        {
            Status = ApplicationStatus.ApplicationPending,
            AppliedAt = DateTime.UtcNow,
            MarriageApplicationFormId = form.Id,
            MarriageApplicationForm = form,
            CertificateId = Guid.NewGuid()
        };
        form.MarriageApplicationId = application.Id;
        form.MarriageApplication = application;

        _context.JamaatRoles.AddRange(
            memberRole, imamRole, presidentRole, circuitPresidentRole, secretaryRole, amirRole);
        _context.JamaatMembers.AddRange(imam, president, secretary, amir, ordinary);
        _context.FormApplications.Add(application);
        await _context.SaveChangesAsync();

        return new SeedResult(form.Id, imam.Id, president.Id, secretary.Id, amir.Id, ordinary.Id);
    }

    private static Role CreateRole(string name, int hierarchyLevel) => new()
    {
        Name = name,
        HierarchyLevel = hierarchyLevel,
        UpdatedBy = "test-seed"
    };

    private static JamaatMember CreateMember(Role role, string chandaNo) => new()
    {
        Surname = "Okonkwo",
        FirstName = chandaNo,
        Email = $"{chandaNo}@test.local",
        ChandaNo = chandaNo,
        JamaatName = "Lagos",
        CircuitName = "Lagos Circuit",
        Sex = "M",
        PhoneNo = "08010000000",
        Password = "seed-only",
        RoleId = role.Id,
        Role = role
    };
}