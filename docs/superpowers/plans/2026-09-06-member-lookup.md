# Member Lookup for Fathers & Witnesses — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Let a user enter a membership number (ChandaNo) for the bride's father, bridegroom's father, or a witness and have the party's name (and for witnesses, address + phone) auto-populate from the member API without that party logging in; non-members use free-text; the membership number is persisted.

**Architecture:** A public (unauthenticated) `GET /api/members/lookup/{chandaNo}` endpoint backed by a new `IMemberLookupService` (gateway-first, local `JamaatMembers` fallback). Four new `MarriageApplicationForm` columns store the membership numbers. The existing guardian form gains father lookup inputs; a new witness entry form captures witnesses with the same lookup. Shared vanilla-JS helper `member-lookup.js` drives the client-side fill.

**Tech Stack:** ASP.NET Core MVC (.NET 10), EF Core 10 + MySql.EntityFrameworkCore, DotNetEnv 3.2.0, Bootstrap 5 (views), vanilla JS + fetch.

**Note on verification & commits:** There is **no test project** in this repo (per AGENTS.md, "verify" = `dotnet build AMJNRishtanata.slnx`). Steps therefore use build + manual checks instead of automated tests. **No commit steps are included** — the user has not authorized commits; do not commit unless explicitly asked.

---

## File map

- Create: `Infrastructure/DTOs/Members/MemberLookupDto.cs` — lookup response shape.
- Create: `Application/Interfaces/IMemberLookupService.cs`
- Create: `Application/Services/MemberLookupService.cs`
- Modify: `Application/Extensions/DependencyInjection.cs` — register service.
- Create: `Presentation/Controllers/MemberLookupController.cs` — public endpoint.
- Modify: `Domain/Entities/MarriageApplicationForm.cs` — 4 new columns.
- Modify: `Infrastructure/Configurations/MarriageApplicationFormConfiguration.cs` — column lengths.
- Modify: `Infrastructure/Infrastructure.csproj` — add DotNetEnv.
- Create: `Infrastructure/Persistence/RishtanataDbContextFactory.cs` — design-time factory.
- Create (generated): `Infrastructure/Migrations/*_AddPartyMembershipNumbers.cs` + `.Designer.cs`; modify `RishtanataDbContextModelSnapshot.cs`.
- Modify: `Presentation/ViewModels/MarriageApplicationFormViewModel.cs` — father membership fields.
- Modify: `Presentation/ViewModels/BrideGuardianViewModel.cs` — father membership fields.
- Modify: `Presentation/Mapping/BrideGuardianViewModelMapper.cs` — map father membership.
- Modify: `Presentation/Controllers/BrideGuardianController.cs` — fix GET, persist father membership.
- Modify: `Presentation/Views/BrideGuardian/Create.cshtml` — father lookup inputs + JS.
- Modify: `Presentation/Views/BrideGuardian/Confirm.cshtml` — hidden fields + cancel link.
- Create: `Presentation/wwwroot/js/member-lookup.js` — shared lookup helper.
- Create: `Presentation/ViewModels/WitnessViewModel.cs`
- Create: `Presentation/Controllers/WitnessController.cs`
- Create: `Presentation/Views/Witness/Create.cshtml`, `Presentation/Views/Witness/Confirm.cshtml`

---

### Task 1: Member lookup DTO, interface, and service

**Files:**
- Create: `Infrastructure/DTOs/Members/MemberLookupDto.cs`
- Create: `Application/Interfaces/IMemberLookupService.cs`
- Create: `Application/Services/MemberLookupService.cs`
- Modify: `Application/Extensions/DependencyInjection.cs`

- [ ] **Step 1: Create the lookup DTO**

Create `Infrastructure/DTOs/Members/MemberLookupDto.cs`:

```csharp
namespace Infrastructure.DTOs.Members;

public class MemberLookupDto
{
    public string ChandaNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string JamaatName { get; set; } = string.Empty;
}
```

- [ ] **Step 2: Create the service interface**

Create `Application/Interfaces/IMemberLookupService.cs`:

```csharp
using Infrastructure.DTOs.Members;

namespace Application.Interfaces;

public interface IMemberLookupService
{
    Task<MemberLookupDto?> LookupAsync(
        string chandaNo,
        CancellationToken cancellationToken = default);
}
```

- [ ] **Step 3: Create the service implementation**

Create `Application/Services/MemberLookupService.cs`:

```csharp
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

        JamaatMember? member = null;
        try
        {
            member = await _gateway.GetMemberByChandaNoAsync(no);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Gateway member lookup failed for {ChandaNo}; falling back to local cache.",
                no);
        }

        member ??= await _context.JamaatMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ChandaNo == no, cancellationToken);

        return member is null ? null : Map(member, no);
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
            JamaatName = member.JamaatName
        };
    }

    private static string BuildFullName(string? firstName, string? surname) =>
        $"{firstName} {surname}".Trim();
}
```

- [ ] **Step 4: Register the service**

In `Application/Extensions/DependencyInjection.cs`, add after the `IStageAuthorizationService` registration (line 31):

```csharp
        services.AddScoped<IMemberLookupService, MemberLookupService>();
```

- [ ] **Step 5: Build**

Run: `dotnet build AMJNRishtanata.slnx`
Expected: Build succeeded, 0 errors.

---

### Task 2: Public lookup endpoint

**Files:**
- Create: `Presentation/Controllers/MemberLookupController.cs`

- [ ] **Step 1: Create the controller**

Create `Presentation/Controllers/MemberLookupController.cs`:

```csharp
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/members")]
public class MemberLookupController : ControllerBase
{
    private readonly IMemberLookupService _lookup;

    public MemberLookupController(IMemberLookupService lookup)
    {
        _lookup = lookup;
    }

    [HttpGet("lookup/{chandaNo}")]
    public async Task<IActionResult> Lookup(string chandaNo, CancellationToken ct)
    {
        var dto = await _lookup.LookupAsync(chandaNo, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
}
```

- [ ] **Step 2: Build**

Run: `dotnet build AMJNRishtanata.slnx`
Expected: Build succeeded.

- [ ] **Step 3: Manual smoke test (optional, needs a running app + MySQL)**

Run: `dotnet run --project Presentation`
Then `Invoke-RestMethod http://localhost:5032/api/members/lookup/000000`
Expected: HTTP 404 (no such member) — confirms the endpoint is reachable and unauthenticated. A real member ChandaNo would return 200 with JSON.

---

### Task 3: Add membership-number columns + EF config

**Files:**
- Modify: `Domain/Entities/MarriageApplicationForm.cs`
- Modify: `Infrastructure/Configurations/MarriageApplicationFormConfiguration.cs`

- [ ] **Step 1: Add columns to the entity**

In `Domain/Entities/MarriageApplicationForm.cs`, make these four insertions:

After `public string BrideFatherName { get; set; } = string.Empty;` (line 64) add:

```csharp
        public string BrideFatherMembershipNo { get; set; } = string.Empty;
```

After `public string BridegroomFatherName { get; set; } = string.Empty;` (line 67) add:

```csharp
        public string BridegroomFatherMembershipNo { get; set; } = string.Empty;
```

After `public string WitnessOneName { get; set; } = string.Empty;` (line 83) add:

```csharp
        public string WitnessOneMembershipNo { get; set; } = string.Empty;
```

After `public string WitnessTwoName { get; set; } = string.Empty;` (line 89) add:

```csharp
        public string WitnessTwoMembershipNo { get; set; } = string.Empty;
```

- [ ] **Step 2: Configure column lengths**

In `Infrastructure/Configurations/MarriageApplicationFormConfiguration.cs`:

After the Parents block (`BridegroomFatherName` `.HasMaxLength(200);`, lines 143-144) add:

```csharp
        builder.Property(f => f.BrideFatherMembershipNo)
            .HasMaxLength(50);

        builder.Property(f => f.BridegroomFatherMembershipNo)
            .HasMaxLength(50);
```

After `WitnessOneName` `.HasMaxLength(200);` (lines 185-186) add:

```csharp
        builder.Property(f => f.WitnessOneMembershipNo)
            .HasMaxLength(50);
```

After `WitnessTwoName` `.HasMaxLength(200);` (lines 201-202) add:

```csharp
        builder.Property(f => f.WitnessTwoMembershipNo)
            .HasMaxLength(50);
```

- [ ] **Step 3: Build**

Run: `dotnet build AMJNRishtanata.slnx`
Expected: Build succeeded.

---

### Task 4: Design-time factory + migration

**Files:**
- Modify: `Infrastructure/Infrastructure.csproj`
- Create: `Infrastructure/Persistence/RishtanataDbContextFactory.cs`
- Create (generated): migration files under `Infrastructure/Migrations/`

- [ ] **Step 1: Add DotNetEnv to Infrastructure**

In `Infrastructure/Infrastructure.csproj`, add inside the existing `<ItemGroup>` that holds the EF packages:

```xml
		<PackageReference Include="DotNetEnv" Version="3.2.0" />
```

- [ ] **Step 2: Create the design-time factory**

Create `Infrastructure/Persistence/RishtanataDbContextFactory.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public class RishtanataDbContextFactory : IDesignTimeDbContextFactory<RishtanataDbContext>
{
    public RishtanataDbContext CreateDbContext(string[] args)
    {
        DotNetEnv.Env.TraversePath().Load();

        var connectionString = Environment.GetEnvironmentVariable(
            "ConnectionStrings__DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "ConnectionStrings__DefaultConnection not found. " +
                "Create a .env file at the repository root.");

        var options = new DbContextOptionsBuilder<RishtanataDbContext>()
            .UseMySQL(connectionString)
            .Options;

        return new RishtanataDbContext(options);
    }
}
```

- [ ] **Step 3: Build**

Run: `dotnet build AMJNRishtanata.slnx`
Expected: Build succeeded.

- [ ] **Step 4: Generate the migration**

From the repo root, run:

```
dotnet ef migrations add AddPartyMembershipNumbers --project Infrastructure --startup-project Presentation
```

Expected: a new `Infrastructure/Migrations/<timestamp>_AddPartyMembershipNumbers.cs` (with `AddColumn` for the 4 columns), its `.Designer.cs`, and an updated `RishtanataDbContextModelSnapshot.cs`.

> If `dotnet ef` is not installed: `dotnet tool install --global dotnet-ef`. Run on **Windows** only (the case-collision `BrideGroomFormSection`/`BridegroomFormSection` would break a case-sensitive build).

- [ ] **Step 5: Verify the migration applies**

Run: `dotnet run --project Presentation`
Expected: after startup, the log shows `Applying migration '<timestamp>_AddPartyMembershipNumbers'` and the app reaches "Now listening on…" (no exception).

- [ ] **Step 6: Confirm columns in MySQL (optional)**

```
podman exec mysql mysql -uroot -p'Html5001#' -e "SHOW COLUMNS FROM rishtanatahdb.MarriageApplicationForms LIKE '%MembershipNo%';"
```
Expected: 6 rows (Bride, Bridegroom, BrideFather, BridegroomFather, WitnessOne, WitnessTwo membership-no columns).

---

### Task 5: Father lookup in the guardian form

**Files:**
- Modify: `Presentation/ViewModels/MarriageApplicationFormViewModel.cs`
- Modify: `Presentation/ViewModels/BrideGuardianViewModel.cs`
- Modify: `Presentation/Mapping/BrideGuardianViewModelMapper.cs`
- Modify: `Presentation/Controllers/BrideGuardianController.cs`
- Modify: `Presentation/Views/BrideGuardian/Create.cshtml`
- Modify: `Presentation/Views/BrideGuardian/Confirm.cshtml`
- Create: `Presentation/wwwroot/js/member-lookup.js`

- [ ] **Step 1: Add father membership fields to the view models**

In `Presentation/ViewModels/MarriageApplicationFormViewModel.cs`, after `BridegroomFatherName` (line 69) add:

```csharp
    public string BrideFatherMembershipNo { get; set; } = string.Empty;
    public string BridegroomFatherMembershipNo { get; set; } = string.Empty;
```

In `Presentation/ViewModels/BrideGuardianViewModel.cs`, after `BrideFatherName` (line 18) add:

```csharp
    public string BrideFatherMembershipNo { get; set; } = string.Empty;
```

and after `BridegroomFatherName` (line 37) add:

```csharp
    public string BridegroomFatherMembershipNo { get; set; } = string.Empty;
```

- [ ] **Step 2: Map the father membership fields**

In `Presentation/Mapping/BrideGuardianViewModelMapper.cs`, in `ToViewModel`, add to the object initializer (after `BrideFatherName = application.BrideFatherName,` and `BridegroomFatherName = application.BridegroomFatherName,` respectively):

```csharp
            BrideFatherMembershipNo = application.BrideFatherMembershipNo,
```

and

```csharp
            BridegroomFatherMembershipNo = application.BridegroomFatherMembershipNo,
```

- [ ] **Step 3: Re-enable the guardian GET and persist father fields**

In `Presentation/Controllers/BrideGuardianController.cs`:

(a) Replace the commented-out `[HttpGet("Create/{referenceNumber}")]` block (lines 27-71) with a working GET keyed by `marriageApplicationId`:

```csharp
    [HttpGet("Create/{marriageApplicationId:guid}")]
    public async Task<IActionResult> Create(
        Guid marriageApplicationId,
        CancellationToken cancellationToken)
    {
        var application = await _applicationService.GetByMarriageApplicationIdAsync(
            marriageApplicationId);

        if (application is null)
            return NotFound("The marriage application was not found.");

        return View(BrideGuardianViewModelMapper.ToViewModel(
            new MarriageApplicationFormViewModel
            {
                MarriageApplicationId = application.MarriageApplicationId,
                ReferenceNumber = application.ReferenceNumber,
                BrideName = application.BrideName,
                BrideFatherName = application.BrideFatherName,
                BrideFatherMembershipNo = application.BrideFatherMembershipNo,
                BrideDateOfBirth = application.BrideDateOfBirth,
                BrideResidentOf = application.BrideResidentOf,
                BrideGenotype = application.BrideGenotype,
                BrideBloodGroup = application.BrideBloodGroup,
                BrideMaritalStatus = application.BrideMaritalStatus,
                BrideProposedDowerAmount = application.BrideProposedDowerAmount,
                BrideDowerAmountReceivedInCash = application.BrideDowerAmountReceivedInCash,
                BridegroomName = application.BridegroomName,
                BridegroomFatherName = application.BridegroomFatherName,
                BridegroomFatherMembershipNo = application.BridegroomFatherMembershipNo,
                BridegroomDateOfBirth = application.BridegroomDateOfBirth,
                BridegroomResidentOf = application.BridegroomResidentOf
            },
            application.ReferenceNumber));
    }
```

Note: the controller file already has `using Presentation.ViewModel;` and `using Application.Interfaces;`. Ensure `using System.Threading;` and `using System.Threading.Tasks;` are present (they are).

(b) Change the existing `[HttpPost("Create/{referenceNumber}")]` to `[HttpPost("Create")]` so the form (which posts to `/BrideGuardian/Create`) matches:

```csharp
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BrideGuardianViewModel model)
```

(c) In the `Confirm` action (after line 114, before `await _applicationService.UpdateAsync`), add the father field writes:

```csharp
        application.BrideFatherName = model.BrideFatherName;
        application.BrideFatherMembershipNo = model.BrideFatherMembershipNo;
        application.BridegroomFatherName = model.BridegroomFatherName;
        application.BridegroomFatherMembershipNo = model.BridegroomFatherMembershipNo;
```

- [ ] **Step 4: Add the shared JS helper**

Create `Presentation/wwwroot/js/member-lookup.js`:

```js
// member-lookup.js — live ChandaNo -> member detail auto-fill (no login).
(function (window) {
  'use strict';

  function bind(inputId, targets) {
    var input = document.getElementById(inputId);
    if (!input) { return; }

    input.addEventListener('change', function () {
      var chandaNo = (input.value || '').trim();
      if (!chandaNo) { return; }

      fetch('/api/members/lookup/' + encodeURIComponent(chandaNo))
        .then(function (res) {
          if (!res.ok) { return; } // 404 => free-text entry
          return res.json();
        })
        .then(function (m) {
          if (!m) { return; }
          if (targets.name) {
            var n = document.getElementById(targets.name);
            if (n) { n.value = m.fullName || ''; }
          }
          if (targets.address) {
            var a = document.getElementById(targets.address);
            if (a) { a.value = m.address || ''; }
          }
          if (targets.tel) {
            var t = document.getElementById(targets.tel);
            if (t) { t.value = m.phoneNo || ''; }
          }
        })
        .catch(function (err) { console.warn('Member lookup failed', err); });
    });
  }

  window.RNMemberLookup = { bind: bind };
})(window);
```

- [ ] **Step 5: Add father lookup inputs to the guardian Create view**

In `Presentation/Views/BrideGuardian/Create.cshtml`:

(a) Remove the two hidden inputs that previously round-tripped the read-only father names (lines 83 and 92):
`<input asp-for="BrideFatherName" type="hidden" />` and
`<input asp-for="BridegroomFatherName" type="hidden" />`.

(b) Insert a new "Fathers" card between the "Marriage Application" card (closes at line 73) and the `<form asp-action="Create" ...>` opening tag, inside the `<form>` element (i.e. after `<input asp-for="BridegroomResidentOf" type="hidden" />`, before the Guardian card at line 97):

```html
        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-white">
                <h5 class="mb-0">Fathers</h5>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3 mb-3">
                        <label asp-for="BrideFatherMembershipNo" class="form-label"></label>
                        <input asp-for="BrideFatherMembershipNo" class="form-control"
                               placeholder="Leave blank if not a member" />
                    </div>
                    <div class="col-md-3 mb-3">
                        <label asp-for="BrideFatherName" class="form-label"></label>
                        <input asp-for="BrideFatherName" class="form-control" />
                    </div>
                    <div class="col-md-3 mb-3">
                        <label asp-for="BridegroomFatherMembershipNo" class="form-label"></label>
                        <input asp-for="BridegroomFatherMembershipNo" class="form-control"
                               placeholder="Leave blank if not a member" />
                    </div>
                    <div class="col-md-3 mb-3">
                        <label asp-for="BridegroomFatherName" class="form-label"></label>
                        <input asp-for="BridegroomFatherName" class="form-control" />
                    </div>
                </div>
            </div>
        </div>
```

(c) Update the `@section Scripts` at the bottom of the file to include the helper and bindings:

```html
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
    <script src="~/js/member-lookup.js" asp-append-version="true"></script>
    <script>
        RNMemberLookup.bind('BrideFatherMembershipNo', { name: 'BrideFatherName' });
        RNMemberLookup.bind('BridegroomFatherMembershipNo', { name: 'BridegroomFatherName' });
    </script>
}
```

- [ ] **Step 6: Update the Confirm view hidden fields + cancel link**

In `Presentation/Views/BrideGuardian/Confirm.cshtml`:

(a) Add hidden fields for the new properties (after the existing `BrideFatherName`/`BridegroomFatherName` hidden inputs, lines 90 and 99):

```html
        <input asp-for="BrideFatherMembershipNo" type="hidden" />
        <input asp-for="BridegroomFatherMembershipNo" type="hidden" />
```

(b) Fix the cancel link (line 111) to route by `marriageApplicationId`:

```html
        <a asp-action="Create" asp-route-marriageApplicationId="@Model.MarriageApplicationId" class="btn btn-secondary">
            &#8592; Cancel &#8211; Go Back
        </a>
```

- [ ] **Step 7: Build**

Run: `dotnet build AMJNRishtanata.slnx`
Expected: Build succeeded.

---

### Task 6: Witness entry form

**Files:**
- Create: `Presentation/ViewModels/WitnessViewModel.cs`
- Create: `Presentation/Controllers/WitnessController.cs`
- Create: `Presentation/Views/Witness/Create.cshtml`
- Create: `Presentation/Views/Witness/Confirm.cshtml`

- [ ] **Step 1: Create the view model**

Create `Presentation/ViewModels/WitnessViewModel.cs`:

```csharp
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModel;

public class WitnessViewModel
{
    [Required]
    public Guid MarriageApplicationId { get; set; }

    [Display(Name = "Reference Number")]
    [Required]
    public string ReferenceNumber { get; set; } = string.Empty;

    public string BrideName { get; set; } = string.Empty;
    public string BridegroomName { get; set; } = string.Empty;

    [Display(Name = "Witness 1 Membership No")]
    public string WitnessOneMembershipNo { get; set; } = string.Empty;

    [Display(Name = "Witness 1 Name")]
    [Required]
    public string WitnessOneName { get; set; } = string.Empty;

    [Display(Name = "Witness 1 Address")]
    [Required]
    public string WitnessOneAddress { get; set; } = string.Empty;

    [Display(Name = "Witness 1 Telephone")]
    [Required]
    public string WitnessOneTel { get; set; } = string.Empty;

    [Display(Name = "Witness 1 Signature Date")]
    public string WitnessOneSignatureDate { get; set; } = string.Empty;

    [Display(Name = "Witness 2 Membership No")]
    public string WitnessTwoMembershipNo { get; set; } = string.Empty;

    [Display(Name = "Witness 2 Name")]
    [Required]
    public string WitnessTwoName { get; set; } = string.Empty;

    [Display(Name = "Witness 2 Address")]
    [Required]
    public string WitnessTwoAddress { get; set; } = string.Empty;

    [Display(Name = "Witness 2 Telephone")]
    [Required]
    public string WitnessTwoTel { get; set; } = string.Empty;

    [Display(Name = "Witness 2 Signature Date")]
    public string WitnessTwoSignatureDate { get; set; } = string.Empty;
}
```

- [ ] **Step 2: Create the controller**

Create `Presentation/Controllers/WitnessController.cs`:

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModel;

namespace Presentation.Controllers;

[Route("Witness")]
public class WitnessController : Controller
{
    private readonly IMarriageApplicationFormService _applicationService;

    public WitnessController(IMarriageApplicationFormService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet("Create/{marriageApplicationId:guid}")]
    public async Task<IActionResult> Create(Guid marriageApplicationId)
    {
        var application = await _applicationService.GetByMarriageApplicationIdAsync(
            marriageApplicationId);

        if (application is null)
            return NotFound("The marriage application was not found.");

        return View(ToViewModel(application));
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(WitnessViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        return View("Confirm", model);
    }

    [HttpPost("Confirm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(
        WitnessViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var application = await _applicationService.GetByMarriageApplicationIdAsync(
            model.MarriageApplicationId);

        if (application is null || application.ReferenceNumber != model.ReferenceNumber)
            return NotFound("The marriage application was not found.");

        application.WitnessOneMembershipNo = model.WitnessOneMembershipNo;
        application.WitnessOneName = model.WitnessOneName;
        application.WitnessOneAddress = model.WitnessOneAddress;
        application.WitnessOneTel = model.WitnessOneTel;
        application.WitnessOneSignatureDate = model.WitnessOneSignatureDate;

        application.WitnessTwoMembershipNo = model.WitnessTwoMembershipNo;
        application.WitnessTwoName = model.WitnessTwoName;
        application.WitnessTwoAddress = model.WitnessTwoAddress;
        application.WitnessTwoTel = model.WitnessTwoTel;
        application.WitnessTwoSignatureDate = model.WitnessTwoSignatureDate;

        await _applicationService.UpdateAsync(application, cancellationToken);

        return RedirectToAction(
            nameof(Create),
            new { marriageApplicationId = model.MarriageApplicationId });
    }

    private static WitnessViewModel ToViewModel(MarriageApplicationForm application) =>
        new WitnessViewModel
        {
            MarriageApplicationId = application.MarriageApplicationId,
            ReferenceNumber = application.ReferenceNumber,
            BrideName = application.BrideName,
            BridegroomName = application.BridegroomName,
            WitnessOneMembershipNo = application.WitnessOneMembershipNo,
            WitnessOneName = application.WitnessOneName,
            WitnessOneAddress = application.WitnessOneAddress,
            WitnessOneTel = application.WitnessOneTel,
            WitnessOneSignatureDate = application.WitnessOneSignatureDate,
            WitnessTwoMembershipNo = application.WitnessTwoMembershipNo,
            WitnessTwoName = application.WitnessTwoName,
            WitnessTwoAddress = application.WitnessTwoAddress,
            WitnessTwoTel = application.WitnessTwoTel,
            WitnessTwoSignatureDate = application.WitnessTwoSignatureDate
        };
}
```

- [ ] **Step 3: Create the Create view**

Create `Presentation/Views/Witness/Create.cshtml`:

```html
@model Presentation.ViewModel.WitnessViewModel

@{
    ViewData["Title"] = "Witnesses Form";
}

<div class="container py-4">
    <div class="mb-4">
        <h2 class="fw-bold">Witnesses Form</h2>
        <p class="text-muted mb-0">To be completed with the details of both witnesses.</p>
    </div>

    <div class="card shadow-sm border-0 mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0">Marriage Application</h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-4 mb-3">
                    <span class="text-muted small d-block">Reference Number</span>
                    <strong>@Model.ReferenceNumber</strong>
                </div>
                <div class="col-md-4 mb-3">
                    <span class="text-muted small d-block">Bride</span>
                    <strong>@Model.BrideName</strong>
                </div>
                <div class="col-md-4 mb-3">
                    <span class="text-muted small d-block">Bridegroom</span>
                    <strong>@Model.BridegroomName</strong>
                </div>
            </div>
        </div>
    </div>

    <form asp-action="Create" method="post">
        @Html.AntiForgeryToken()

        <div asp-validation-summary="ModelOnly" class="text-danger mb-3"></div>
        <input asp-for="MarriageApplicationId" type="hidden" />
        <input asp-for="ReferenceNumber" type="hidden" />
        <input asp-for="BrideName" type="hidden" />
        <input asp-for="BridegroomName" type="hidden" />

        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-white">
                <h5 class="mb-0">Witness One</h5>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3 mb-3">
                        <label asp-for="WitnessOneMembershipNo" class="form-label"></label>
                        <input asp-for="WitnessOneMembershipNo" class="form-control"
                               placeholder="Leave blank if not a member" />
                    </div>
                    <div class="col-md-3 mb-3">
                        <label asp-for="WitnessOneName" class="form-label"></label>
                        <input asp-for="WitnessOneName" class="form-control" />
                        <span asp-validation-for="WitnessOneName" class="text-danger"></span>
                    </div>
                    <div class="col-md-3 mb-3">
                        <label asp-for="WitnessOneTel" class="form-label"></label>
                        <input asp-for="WitnessOneTel" class="form-control" />
                        <span asp-validation-for="WitnessOneTel" class="text-danger"></span>
                    </div>
                    <div class="col-md-3 mb-3">
                        <label asp-for="WitnessOneSignatureDate" class="form-label"></label>
                        <input asp-for="WitnessOneSignatureDate" type="date" class="form-control" />
                    </div>
                    <div class="col-12 mb-3">
                        <label asp-for="WitnessOneAddress" class="form-label"></label>
                        <textarea asp-for="WitnessOneAddress" class="form-control" rows="2"></textarea>
                        <span asp-validation-for="WitnessOneAddress" class="text-danger"></span>
                    </div>
                </div>
            </div>
        </div>

        <div class="card shadow-sm border-0 mb-4">
            <div class="card-header bg-white">
                <h5 class="mb-0">Witness Two</h5>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3 mb-3">
                        <label asp-for="WitnessTwoMembershipNo" class="form-label"></label>
                        <input asp-for="WitnessTwoMembershipNo" class="form-control"
                               placeholder="Leave blank if not a member" />
                    </div>
                    <div class="col-md-3 mb-3">
                        <label asp-for="WitnessTwoName" class="form-label"></label>
                        <input asp-for="WitnessTwoName" class="form-control" />
                        <span asp-validation-for="WitnessTwoName" class="text-danger"></span>
                    </div>
                    <div class="col-md-3 mb-3">
                        <label asp-for="WitnessTwoTel" class="form-label"></label>
                        <input asp-for="WitnessTwoTel" class="form-control" />
                        <span asp-validation-for="WitnessTwoTel" class="text-danger"></span>
                    </div>
                    <div class="col-md-3 mb-3">
                        <label asp-for="WitnessTwoSignatureDate" class="form-label"></label>
                        <input asp-for="WitnessTwoSignatureDate" type="date" class="form-control" />
                    </div>
                    <div class="col-12 mb-3">
                        <label asp-for="WitnessTwoAddress" class="form-label"></label>
                        <textarea asp-for="WitnessTwoAddress" class="form-control" rows="2"></textarea>
                        <span asp-validation-for="WitnessTwoAddress" class="text-danger"></span>
                    </div>
                </div>
            </div>
        </div>

        <button type="submit" class="btn btn-primary">Review &amp; Confirm</button>
    </form>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
    <script src="~/js/member-lookup.js" asp-append-version="true"></script>
    <script>
        RNMemberLookup.bind('WitnessOneMembershipNo', {
            name: 'WitnessOneName', address: 'WitnessOneAddress', tel: 'WitnessOneTel'
        });
        RNMemberLookup.bind('WitnessTwoMembershipNo', {
            name: 'WitnessTwoName', address: 'WitnessTwoAddress', tel: 'WitnessTwoTel'
        });
    </script>
}
```

- [ ] **Step 4: Create the Confirm view**

Create `Presentation/Views/Witness/Confirm.cshtml`:

```html
@model Presentation.ViewModel.WitnessViewModel

@{
    ViewData["Title"] = "Confirm Witnesses";
}

<div class="container py-4">
    <div class="mb-4">
        <h2 class="fw-bold">Confirm Witnesses</h2>
        <p class="text-muted mb-0">Review the witness details before saving.</p>
    </div>

    <div class="card shadow-sm border-0 mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0">Witness One</h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6 mb-3">
                    <span class="text-muted small d-block">Name</span>
                    <strong>@Model.WitnessOneName</strong>
                </div>
                <div class="col-md-6 mb-3">
                    <span class="text-muted small d-block">Telephone</span>
                    <span>@Model.WitnessOneTel</span>
                </div>
                <div class="col-12 mb-3">
                    <span class="text-muted small d-block">Address</span>
                    <span>@Model.WitnessOneAddress</span>
                </div>
            </div>
        </div>
    </div>

    <div class="card shadow-sm border-0 mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0">Witness Two</h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6 mb-3">
                    <span class="text-muted small d-block">Name</span>
                    <strong>@Model.WitnessTwoName</strong>
                </div>
                <div class="col-md-6 mb-3">
                    <span class="text-muted small d-block">Telephone</span>
                    <span>@Model.WitnessTwoTel</span>
                </div>
                <div class="col-12 mb-3">
                    <span class="text-muted small d-block">Address</span>
                    <span>@Model.WitnessTwoAddress</span>
                </div>
            </div>
        </div>
    </div>

    <form asp-action="Confirm" method="post" class="d-flex gap-2">
        @Html.AntiForgeryToken()
        <input asp-for="MarriageApplicationId" type="hidden" />
        <input asp-for="ReferenceNumber" type="hidden" />
        <input asp-for="BrideName" type="hidden" />
        <input asp-for="BridegroomName" type="hidden" />
        <input asp-for="WitnessOneMembershipNo" type="hidden" />
        <input asp-for="WitnessOneName" type="hidden" />
        <input asp-for="WitnessOneAddress" type="hidden" />
        <input asp-for="WitnessOneTel" type="hidden" />
        <input asp-for="WitnessOneSignatureDate" type="hidden" />
        <input asp-for="WitnessTwoMembershipNo" type="hidden" />
        <input asp-for="WitnessTwoName" type="hidden" />
        <input asp-for="WitnessTwoAddress" type="hidden" />
        <input asp-for="WitnessTwoTel" type="hidden" />
        <input asp-for="WitnessTwoSignatureDate" type="hidden" />

        <button type="submit" class="btn btn-success">&#10003; Confirm and Save</button>
        <a asp-action="Create" asp-route-marriageApplicationId="@Model.MarriageApplicationId" class="btn btn-secondary">
            &#8592; Cancel &#8211; Go Back
        </a>
    </form>
</div>
```

- [ ] **Step 5: Build**

Run: `dotnet build AMJNRishtanata.slnx`
Expected: Build succeeded, 0 errors.

---

## Final verification

- [ ] `dotnet build AMJNRishtanata.slnx` succeeds.
- [ ] `dotnet run --project Presentation` reaches "Now listening on…" (migration applies).
- [ ] `GET /api/members/lookup/{unknown}` → 404; `GET /api/members/lookup/{valid}` → 200 JSON.
- [ ] Guardian form: entering a member ChandaNo fills the father name; an unknown number leaves the name editable.
- [ ] Witness form: entering a member ChandaNo fills name/address/phone; unknown number leaves fields editable; submit persists to `MarriageApplicationForm`.
