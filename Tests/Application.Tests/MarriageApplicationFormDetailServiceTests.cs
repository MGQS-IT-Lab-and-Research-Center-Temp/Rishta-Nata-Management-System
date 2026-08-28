using Application.Authorization;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Security.Claims;
using Xunit;

namespace Application.Tests;

/// <summary>
/// Unit tests for the read-side detail DTO assembly (Epic C3).
///
/// Covers: shared fields + CurrentStage mapping, sections null when not yet
/// submitted, witness list contents, rejection history, and a
/// CanCurrentUserEdit flag computed through IStageAuthorizationService (the
/// same logic as Epic B — policy §7.3).
/// </summary>
public sealed class MarriageApplicationFormDetailServiceTests : IDisposable
{
    private const string BrideChandaNo = "1001";
    private const string GroomChandaNo = "1002";

    private readonly RishtanataDbContext _context;

    public MarriageApplicationFormDetailServiceTests()
    {
        var options = new DbContextOptionsBuilder<RishtanataDbContext>()
            .UseInMemoryDatabase($"FormDetailTests_{Guid.NewGuid():N}")
            .Options;

        _context = new RishtanataDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    // =====================================================================
    // Not found
    // =====================================================================

    [Fact]
    public async Task GetDetailAsync_UnknownId_ReturnsNull()
    {
        await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut(userId: null);

        var dto = await sut.GetDetailAsync(Guid.NewGuid());

        Assert.Null(dto);
    }

    // =====================================================================
    // Shared fields + CurrentStage
    // =====================================================================

    [Fact]
    public async Task GetDetailAsync_MapsSharedFieldsAndCurrentStage()
    {
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut(userId: null);

        var dto = await sut.GetDetailAsync(seed.FormId);

        Assert.NotNull(dto);
        Assert.Equal(seed.FormId, dto!.FormId);
        Assert.Equal(seed.ApplicationId, dto.ApplicationId);
        Assert.Equal("NIK-2026-0001", dto.ReferenceNumber);
        Assert.Equal("Lagos", dto.Venue);
        Assert.Equal(ApplicationStatus.ApplicationPending.ToString(), dto.Status);
        Assert.Equal(ApplicationStage.JamaatPresidentReview, dto.CurrentStage);
    }

    // =====================================================================
    // Sections null when not yet submitted
    // =====================================================================

    [Fact]
    public async Task GetDetailAsync_UnsubmittedSections_AreNull()
    {
        // Seed fills only bride/bridegroom identity; every other section is
        // untouched, so it must come back null (witnesses empty).
        var seed = await SeedAsync(ApplicationStage.ApplicantsReview);
        var sut = CreateSut(userId: null);

        var dto = await sut.GetDetailAsync(seed.FormId);

        Assert.NotNull(dto);
        Assert.NotNull(dto!.Bride);
        Assert.NotNull(dto.Bridegroom);
        Assert.Null(dto.Guardian);
        Assert.Null(dto.Representative);
        Assert.Empty(dto.Witnesses);
        Assert.Null(dto.OfficiatingImam);
        Assert.Null(dto.JamaatPresident);
        Assert.Null(dto.NationalRishtanataSecretary);
        Assert.Null(dto.AmirApproval);
        Assert.Empty(dto.Rejections);
    }

    [Fact]
    public async Task GetDetailAsync_SubmittedSections_AreMappedWithValues()
    {
        var seed = await SeedAsync(ApplicationStage.AmirApproval);
        await FillAllSectionsAsync(seed.FormId);
        var sut = CreateSut(userId: null);

        var dto = await sut.GetDetailAsync(seed.FormId);

        Assert.NotNull(dto);
        Assert.Equal("Aisha Bello", dto!.Bride!.Name);
        Assert.Equal(BrideChandaNo, dto.Bride.MembershipNo);
        Assert.Equal("Ibrahim Yusuf", dto.Bridegroom!.Name);
        Assert.Equal("Malam Sani", dto.Guardian!.Name);
        Assert.Equal("Bashir Lawal", dto.Representative!.Name);
        Assert.Equal("Umar Farouk", dto.OfficiatingImam!.Name);
        Assert.Equal("President Musa", dto.JamaatPresident!.Name);
        Assert.Equal("Secretary Hauwa", dto.NationalRishtanataSecretary!.Name);
        Assert.NotNull(dto.AmirApproval);
        Assert.Equal(new DateTime(2026, 9, 1), dto.AmirApproval.ApprovedDateOfNikah);
    }

    // =====================================================================
    // Witness list
    // =====================================================================

    [Fact]
    public async Task GetDetailAsync_WitnessList_IncludesOnlySubmittedWitnessesInOrder()
    {
        var seed = await SeedAsync(ApplicationStage.ApplicantsReview);
        var form = await _context.MarriageApplicationForms.SingleAsync(f => f.Id == seed.FormId);
        form.WitnessOneName = "Witness One";
        form.WitnessOneAddress = "1 Kano Road";
        form.WitnessOneTel = "08011111111";
        // Witness Two left blank.
        await _context.SaveChangesAsync();

        var sut = CreateSut(userId: null);
        var dto = await sut.GetDetailAsync(seed.FormId);

        var witness = Assert.Single(dto!.Witnesses);
        Assert.Equal(1, witness.Position);
        Assert.Equal("Witness One", witness.Name);
    }

    [Fact]
    public async Task GetDetailAsync_BothWitnessesSubmitted_ListedInPaperFormOrder()
    {
        var seed = await SeedAsync(ApplicationStage.ApplicantsReview);
        var form = await _context.MarriageApplicationForms.SingleAsync(f => f.Id == seed.FormId);
        form.WitnessOneName = "Witness One";
        form.WitnessTwoName = "Witness Two";
        await _context.SaveChangesAsync();

        var sut = CreateSut(userId: null);
        var dto = await sut.GetDetailAsync(seed.FormId);

        Assert.Equal(2, dto!.Witnesses.Count);
        Assert.Equal(1, dto.Witnesses[0].Position);
        Assert.Equal(2, dto.Witnesses[1].Position);
    }

    // =====================================================================
    // Rejection history
    // =====================================================================

    [Fact]
    public async Task GetDetailAsync_RejectionHistory_IsMappedOldestFirst()
    {
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        _context.MarriageFormRejections.AddRange(
            new MarriageFormRejection
            {
                MarriageApplicationFormId = seed.FormId,
                RejectedAtStage = ApplicationStage.NationalRishtanataSecretaryVerification,
                RevertedToStage = ApplicationStage.JamaatPresidentReview,
                Reason = "Dower amount unclear",
                CreatedAt = new DateTime(2026, 8, 20)
            },
            new MarriageFormRejection
            {
                MarriageApplicationFormId = seed.FormId,
                RejectedAtStage = ApplicationStage.JamaatPresidentReview,
                RevertedToStage = ApplicationStage.ApplicantsReview,
                Reason = "Missing guardian details",
                CreatedAt = new DateTime(2026, 8, 10)
            });
        await _context.SaveChangesAsync();

        var sut = CreateSut(userId: null);
        var dto = await sut.GetDetailAsync(seed.FormId);

        Assert.Equal(2, dto!.Rejections.Count);
        Assert.Equal("Missing guardian details", dto.Rejections[0].Reason);
        Assert.Equal("Dower amount unclear", dto.Rejections[1].Reason);
        Assert.Equal(
            ApplicationStage.NationalRishtanataSecretaryVerification,
            dto.Rejections[1].RejectedAtStage);
    }

    // =====================================================================
    // CanCurrentUserEdit — Epic B parity via IStageAuthorizationService
    // =====================================================================

    [Fact]
    public async Task GetDetailAsync_CanCurrentUserEdit_TrueForAuthorizedUserAtCurrentStage()
    {
        // The president, authenticated, viewing a form sitting at his stage.
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut(userId: seed.PresidentId);

        var dto = await sut.GetDetailAsync(seed.FormId);

        Assert.NotNull(dto);
        Assert.True(dto!.CanCurrentUserEdit);
    }

    [Fact]
    public async Task GetDetailAsync_CanCurrentUserEdit_FalseForUnauthorizedUser()
    {
        // An ordinary member who is neither party nor office-holder.
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut(userId: seed.OrdinaryMemberId);

        var dto = await sut.GetDetailAsync(seed.FormId);

        Assert.NotNull(dto);
        Assert.False(dto!.CanCurrentUserEdit);
    }

    [Fact]
    public async Task GetDetailAsync_CanCurrentUserEdit_FalseWhenFormNotInWorkflow()
    {
        // Even the right office-holder cannot act on a form that has not
        // entered the staged workflow (CurrentStage is null).
        var seed = await SeedAsync(currentStage: null);
        var sut = CreateSut(userId: seed.PresidentId);

        var dto = await sut.GetDetailAsync(seed.FormId);

        Assert.NotNull(dto);
        Assert.False(dto!.CanCurrentUserEdit);
    }

    [Fact]
    public async Task GetDetailAsync_CanCurrentUserEdit_FalseWhenUnauthenticated()
    {
        var seed = await SeedAsync(ApplicationStage.JamaatPresidentReview);
        var sut = CreateSut(userId: null); // no NameIdentifier claim

        var dto = await sut.GetDetailAsync(seed.FormId);

        Assert.NotNull(dto);
        Assert.False(dto!.CanCurrentUserEdit);
    }

    // =====================================================================
    // Helpers
    // =====================================================================

    private MarriageApplicationFormDetailService CreateSut(Guid? userId)
    {
        var stageAuthorization = new StageAuthorizationService(
            _context, NullLogger<StageAuthorizationService>.Instance);

        return new MarriageApplicationFormDetailService(
            _context,
            stageAuthorization,
            CreateAccessor(userId));
    }

    private static IHttpContextAccessor CreateAccessor(Guid? userId)
    {
        var accessor = new HttpContextAccessor();
        var context = new DefaultHttpContext();

        if (userId.HasValue)
        {
            context.User = new ClaimsPrincipal(new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()) },
                authenticationType: "Test"));
        }

        accessor.HttpContext = context;
        return accessor;
    }

    private sealed record SeedResult(
        Guid FormId,
        Guid ApplicationId,
        Guid PresidentId,
        Guid OrdinaryMemberId);

    private async Task<SeedResult> SeedAsync(ApplicationStage? currentStage)
    {
        var memberRole = CreateRole("Jama'at Member", hierarchyLevel: 1);
        var presidentRole = CreateRole("Jamaat President", hierarchyLevel: 2);

        var president = CreateMember(presidentRole, "2001");
        var ordinary = CreateMember(memberRole, "5001");

        var form = new MarriageApplicationForm
        {
            ReferenceNumber = "NIK-2026-0001",
            ProposedNikahDate = DateTime.UtcNow.AddDays(30),
            Venue = "Lagos",
            BrideMembershipNo = BrideChandaNo,
            BrideName = "Aisha Bello",
            BridegroomMembershipNo = GroomChandaNo,
            BridegroomName = "Ibrahim Yusuf",
            ApplicationStage = currentStage
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

        _context.JamaatRoles.AddRange(memberRole, presidentRole);
        _context.JamaatMembers.AddRange(president, ordinary);
        _context.FormApplications.Add(application);
        await _context.SaveChangesAsync();

        return new SeedResult(form.Id, application.Id, president.Id, ordinary.Id);
    }

    /// <summary>Fills every remaining section so all DTO sections map.</summary>
    private async Task FillAllSectionsAsync(Guid formId)
    {
        var form = await _context.MarriageApplicationForms.SingleAsync(f => f.Id == formId);

        form.BrideDateOfBirth = new DateTime(2000, 1, 2);
        form.BrideResidentOf = "Lagos";
        form.BrideGenotype = "AA";
        form.BrideBloodGroup = "O+";
        form.BrideMaritalStatus = "Single";
        form.BrideProposedDowerAmount = 500m;
        form.BrideDowerAmountReceivedInCash = 500m;
        form.BrideSignatureTel = "08010000001";

        form.BridegroomDateOfBirth = new DateTime(1998, 3, 4);
        form.BridegroomResidentOf = "Lagos";
        form.BridegroomGenotype = "AS";
        form.BridegroomBloodGroup = "A+";
        form.BridegroomSignatureTel = "08010000002";

        form.GuardianName = "Malam Sani";
        form.GuardianRelationToBride = "Father";
        form.GuardianAddress = "5 Awolowo Road";
        form.GuardianTel = "08010000003";
        form.GuardianSignatureDate = "2026-08-01";

        form.RepresentativeName = "Bashir Lawal";
        form.RepresentativeAddress = "7 Herbert Macaulay Way";
        form.RepresentativeActingFor = "Bridegroom";
        form.RepresentativeSignatureDate = "2026-08-01";

        form.WitnessOneName = "Witness One";
        form.WitnessTwoName = "Witness Two";

        form.OfficiatingImamName = "Umar Farouk";
        form.OfficiatingImamAddressJamaat = "Lagos Central Mosque";
        form.OfficiatingImamSignatureDate = "2026-08-05";

        form.JamaatPresidentName = "President Musa";
        form.JamaatPresidentSignatureDate = "2026-08-10";

        form.NationalRishtanataSecretaryName = "Secretary Hauwa";
        form.NationalRishtanataSecretarySignatureDate = "2026-08-15";

        form.ApprovedDateOfNikah = new DateTime(2026, 9, 1);
        form.NationalAmirOrMissionarySignatureDate = "2026-08-20";

        await _context.SaveChangesAsync();
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
  
        RoleId = role.Id,
        Role = role
    };
}