# Mathna / Thulatha / Arba'a Ordinal Split Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the single boolean "Second, third or fourth Nikah" option with a fine-grained choice — Mathna (second), Thulatha (third), Arba'a (fourth) — stored as one nullable enum, across the Create screen, the Continue partner-groom section, and the JamaatPresident review screen.

**Architecture:** Adds `Domain.Enums.MarriageOrdinal` (Mathna=2, Thulatha=3, Arbaa=4); the two groom-carrying entities (`MarriageApplicationForm`, `BridegroomFormSection`) swap `bool IsSecondThirdOrFourthNikah` for `MarriageOrdinal? CurrentNikahOrdinal`. The ordinal flows through DTOs → mappers → Application services → view models unchanged in shape (only the type changes), the three screens present three ordinal radios bound to `asp-for="CurrentNikahOrdinal"`, and a new EF migration drops the old bool columns and adds nullable int columns on `NikahApplications` and `NikahGrooms`.

**Tech Stack:** ASP.NET Core MVC (.NET 10), EF Core 10 (`MySql.EntityFrameworkCore`), FluentValidation, Razor. DB `rishtanatahdb` (MySQL).

> **Testing note:** This repository has no test project (AGENTS.md). "Verify" = `dotnet build AMJNRishtanata.slnx` must be clean at each task gate, plus the targeted runtime checks in Task 9. Conversely, the repo has a dead global-namespace enum `NikahOrdinal { First, SecondThirdOrFourth }` (declared at the bottom of `Domain/Entities/BridegroomFormSection.cs`); its only consumers are `BridegroomSectionDto.NikahOrdinal` (never assigned anywhere — a latent bug: the validator branch guarding it is dead code) and `SubmissionValidators.HaveValidNikahHistory`. It is retired in Task 2/Task 3 and its validator branch rewired in Task 4, because leaving both `NikahOrdinal` and the new `MarriageOrdinal` in place would be confusing.

> **Design refinement (one display detail):** The approved spec said Review renders `CurrentNikahOrdinal?.Display() ?? "First nikah"`. Because a widower/divorced remarriage records an ordinal of `null`, Task 7 renders "First nikah" only when `IsFirstNikah` is true and falls back to status-aware labels for null ordinals. The enum/storage design is unchanged.

## File Map (by task)

| Task | Files |
|------|-------|
| 1 | `Domain/Enums/MarriageOrdinal.cs` (new), `Domain/Constants/MarriageOrdinalDisplay.cs` (new) |
| 2 | `Domain/Entities/MarriageApplicationForm.cs`, `Domain/Entities/BridegroomFormSection.cs` |
| 3 | `Infrastructure/DTOs/BrideGroom/BridegroomSectionDto.cs`, `Infrastructure/DTOs/MarriageApplicationFormDetail/BridegroomSectionDetailDto.cs`, `Infrastructure/DTOs/ReadOnlyFormDto.cs`, `Infrastructure/DTOs/JamaatPresidentDashboardDto/JamaatPresidentReviewDto.cs`, `Infrastructure/Mapper/BrideGroomMapper.cs`, `Infrastructure/Mapper/MarriageApplicationFormDetailMapper.cs`, `Infrastructure/Mapper/ReadOnlyFormMapper.cs` |
| 4 | `Application/Services/BridegroomService.cs`, `Application/Services/BridegroomSectionService.cs`, `Application/Services/MarriageApplicationFormService.cs`, `Application/Services/JamaatPresidentService.cs`, `Application/Validators/SubmissionValidators.cs` |
| 5 | `Presentation/ViewModels/NewApplicationViewModel.cs`, `Presentation/ViewModels/ContinueApplicationViewModel.cs`, `Presentation/ViewModels/BridegroomFormViewModel.cs`, `Presentation/ViewModels/MarriageApplicationFormViewModel.cs`, `Presentation/ViewModels/JamaatPresidentReviewViewModel.cs`, `Presentation/Requests/BridegroomSectionRequest.cs` |
| 6 | `Presentation/Mapping/Bridegroom/BridegroomMapping.cs`, `Presentation/Mapping/JamaatPresident/JamaatPresidentMapping.cs`, `Presentation/Mapping/MarriageFormRequestMapping.cs`, `Presentation/Controllers/MarriageApplicationController.cs` |
| 7 | `Presentation/Views/MarriageApplication/Create.cshtml`, `Presentation/Views/MarriageApplication/Continue.cshtml`, `Presentation/Views/JamaatPresident/Review.cshtml` |
| 8 | `Infrastructure/Migrations/2026xxxx_SplitCurrentNikahOrdinal.cs` (+ Designer + snapshot, generated) |
| 9 | runtime verification only / test-data cleanup |

---

## Task 1: Domain enum + display helper

**Files:**
- Create: `Domain/Enums/MarriageOrdinal.cs`
- Create: `Domain/Constants/MarriageOrdinalDisplay.cs`

- [ ] **Step 1: Create `Domain/Enums/MarriageOrdinal.cs`**

```csharp
namespace Domain.Enums;

public enum MarriageOrdinal
{
    Mathna = 2,
    Thulatha = 3,
    Arbaa = 4
}
```

> Member name `Arbaa` (no apostrophe — C# identifiers). Display strings carry the punctuation.

- [ ] **Step 2: Create `Domain/Constants/MarriageOrdinalDisplay.cs`**

```csharp
using Domain.Enums;

namespace Domain.Constants;

public static class MarriageOrdinalDisplay
{
    public static string Display(this MarriageOrdinal ordinal) => ordinal switch
    {
        MarriageOrdinal.Mathna => "Mathna (second)",
        MarriageOrdinal.Thulatha => "Thulatha (third)",
        MarriageOrdinal.Arbaa => "Arba'a (fourth)",
        _ => throw new ArgumentOutOfRangeException(nameof(ordinal))
    };
}
```

> Match the existing file style in `Domain/Enums/*` (e.g. `ApplicationStage.cs`): simple file-scoped namespace, no BOM. If the surrounding enum files use block-scoped namespaces, match them.

- [ ] **Step 3: Verify**

Run: `dotnet build Domain/Domain.csproj`
Expected: `0 Warning(s), 0 Error(s)`.

- [ ] **Step 4: Commit**

```bash
git add Domain/Enums/MarriageOrdinal.cs Domain/Constants/MarriageOrdinalDisplay.cs
git commit -m "feat: add MarriageOrdinal enum and display helper"
```

---

## Task 2: Domain entities — swap bool → nullable enum, retire dead `NikahOrdinal`

**Files:**
- Modify: `Domain/Entities/MarriageApplicationForm.cs:54-55`
- Modify: `Domain/Entities/BridegroomFormSection.cs:1,19-20,31-35`

- [ ] **Step 1: Edit `Domain/Entities/MarriageApplicationForm.cs`**

Properties (lines 54-55) become:

```csharp
        public bool IsFirstNikah { get; set; }
        public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

This file already has `using Domain.Enums;` (line 3) — no import change needed.

- [ ] **Step 2: Edit `Domain/Entities/BridegroomFormSection.cs`**

Add `using Domain.Enums;` after `using Domain.Abstractions;` (line 1):

```csharp
using Domain.Abstractions;
using Domain.Enums;
```

Replace the bool property (lines 19-20) with:

```csharp
        public bool IsFirstNikah { get; set; }
        public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

Delete the trailing global-namespace enum entirely (current lines 31-35 — do NOT leave it in the file):

```csharp
public enum NikahOrdinal
{
    First,
    SecondThirdOrFourth
}
```

- [ ] **Step 3: Verify**

Run: `dotnet build Domain/Domain.csproj`
Expected: `0 Warning(s), 0 Error(s)`. Note — `NikahOrdinal` references will not surface until Task 3 rewires the DTO, at which point the next solution-wide build is the real gate.

- [ ] **Step 4: Commit**

```bash
git add Domain/Entities/MarriageApplicationForm.cs Domain/Entities/BridegroomFormSection.cs
git commit -m "feat: replace IsSecondThirdOrFourthNikah bool with MarriageOrdinal? CurrentNikahOrdinal on entities"
```

---

## Task 3: Infrastructure DTOs + mappers

**Files:**
- Modify: `Infrastructure/DTOs/BrideGroom/BridegroomSectionDto.cs:1,14,16`
- Modify: `Infrastructure/DTOs/MarriageApplicationFormDetail/BridegroomSectionDetailDto.cs:1,14`
- Modify: `Infrastructure/DTOs/ReadOnlyFormDto.cs:44`
- Modify: `Infrastructure/DTOs/JamaatPresidentDashboardDto/JamaatPresidentReviewDto.cs:37`
- Modify: `Infrastructure/Mapper/BrideGroomMapper.cs:22,45`
- Modify: `Infrastructure/Mapper/MarriageApplicationFormDetailMapper.cs:70`
- Modify: `Infrastructure/Mapper/ReadOnlyFormMapper.cs:39`

- [ ] **Step 1: `BridegroomSectionDto.cs`**

Add at top (before `namespace`):

```csharp
using Domain.Enums;
```

Remove the never-assigned coarse property (line 14) and the bool (line 16); add the ordinal:

```csharp
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
    public bool IsFirstNikah { get; set; }
```

The member replacing BOTH the `NikahOrdinal NikahOrdinal` line AND the `bool IsSecondThirdOrFourthNikah` line — final member block is:

```csharp
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
    public bool IsFirstNikah { get; set; }
    public bool FormerWifeIsDead { get; set; }
    public bool HasDivorcedFormerWife { get; set; }
    public string BridegroomDivorceEvidence { get; set; } = string.Empty;
```

- [ ] **Step 2: `BridegroomSectionDetailDto.cs`**

Add `using Domain.Enums;` at top. Replace line 14:

```csharp
    public bool IsFirstNikah { get; set; }
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

- [ ] **Step 3: `ReadOnlyFormDto.cs`**

Has `using Domain.Enums;` already (line 2). Replace line 44:

```csharp
    public bool IsFirstNikah { get; set; }
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

- [ ] **Step 4: `JamaatPresidentReviewDto.cs`**

Has `using Domain.Enums;` already (line 1). Replace line 37:

```csharp
    public bool IsFirstNikah { get; set; }
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

- [ ] **Step 5: `Infrastructure/Mapper/BrideGroomMapper.cs`**

Line 22 (in `ToDto`):

```csharp
            IsFirstNikah = entity.IsFirstNikah,
            CurrentNikahOrdinal = entity.CurrentNikahOrdinal,
```

Line 45 (in `ToEntity`):

```csharp
            IsFirstNikah = dto.IsFirstNikah,
            CurrentNikahOrdinal = dto.CurrentNikahOrdinal,
```

- [ ] **Step 6: `Infrastructure/Mapper/MarriageApplicationFormDetailMapper.cs`**

Line 70:

```csharp
                    IsFirstNikah = form.IsFirstNikah,
                    CurrentNikahOrdinal = form.CurrentNikahOrdinal,
```

- [ ] **Step 7: `Infrastructure/Mapper/ReadOnlyFormMapper.cs`**

Line 39:

```csharp
                IsFirstNikah = form.IsFirstNikah,
                CurrentNikahOrdinal = form.CurrentNikahOrdinal,
```

- [ ] **Step 8: Verify**

Run: `dotnet build Infrastructure/Infrastructure.csproj`
Expected: `0 Warning(s), 0 Error(s)`. (This is where any leftover `NikahOrdinal` reference would fail to compile.)

- [ ] **Step 9: Commit**

```bash
git add Infrastructure/DTOs Infrastructure/Mapper
git commit -m "feat: thread MarriageOrdinal? through bridegroom DTOs and mappers"
```

---

## Task 4: Application services + validator

**Files:**
- Modify: `Application/Services/BridegroomService.cs:46`
- Modify: `Application/Services/BridegroomSectionService.cs:74,99,124`
- Modify: `Application/Services/MarriageApplicationFormService.cs:119`
- Modify: `Application/Services/JamaatPresidentService.cs:160`
- Modify: `Application/Validators/SubmissionValidators.cs:27`

- [ ] **Step 1: `BridegroomService.cs`** — line 46:

```csharp
        existingBridegroom.IsFirstNikah = bridegroom.IsFirstNikah;
        existingBridegroom.CurrentNikahOrdinal = bridegroom.CurrentNikahOrdinal;
```

- [ ] **Step 2: `BridegroomSectionService.cs`** — eligibility call, line 74. The parameter changed from a bool to "declares a subsequent nikah"; pass the derived flag:

```csharp
        var eligibility = await _eligibility.ValidateSectionAsync(
            dto.BridegroomMembershipNo,
            partnerIsGroom: true,
            dto.CurrentNikahOrdinal is not null,
            dto.FormerWifeIsDead,
            dto.HasDivorcedFormerWife,
            dto.BridegroomDivorceEvidence,
            brideMaritalStatus: string.Empty,
            brideDivorceEvidence: string.Empty,
            applicationFormId,
            cancellationToken);
```

Lines 99 and 124:

```csharp
        form.IsFirstNikah = dto.IsFirstNikah;
        form.CurrentNikahOrdinal = dto.CurrentNikahOrdinal;
```

and

```csharp
        bridegroomSection.IsFirstNikah = dto.IsFirstNikah;
        bridegroomSection.CurrentNikahOrdinal = dto.CurrentNikahOrdinal;
```

- [ ] **Step 3: `MarriageApplicationFormService.cs`** — line 119:

```csharp
                IsFirstNikah = application.IsFirstNikah,
                CurrentNikahOrdinal = application.CurrentNikahOrdinal,
```

- [ ] **Step 4: `JamaatPresidentService.cs`** — line 160:

```csharp
            IsFirstNikah = form.IsFirstNikah,
            CurrentNikahOrdinal = form.CurrentNikahOrdinal,
```

- [ ] **Step 5: `SubmissionValidators.cs`** — rewire `HaveValidNikahHistory`. Replace line 27 so the fine ordinal decides whether the remarriage-former-status rule applies:

```csharp
    private static bool HaveValidNikahHistory(BridegroomSectionDto dto)
    {
        // Ordinal null → groom declared a first nikah (nothing about former wives applies).
        if (dto.CurrentNikahOrdinal is null)
            return true;

        var selectedStatuses = new[]
        {
            dto.FormerWifeIsDead,
            dto.HasDivorcedFormerWife,
            dto.FormerWifeObtainedKhula
        }.Count(value => value);

        return selectedStatuses == 1;
    }
```

This preserves the rule's original intent ("For a previous Nikah, select exactly one former-wife status") now that the selector actually fires. There is no other `NikahOrdinal` reference left in the Application project.

- [ ] **Step 6: Verify**

Run: `dotnet build Application/Application.csproj`
Expected: `0 Warning(s), 0 Error(s)`.

- [ ] **Step 7: Commit**

```bash
git add Application/Services Application/Validators
git commit -m "feat: swap Application layer to CurrentNikahOrdinal and fix remarriage-history validation branch"
```

---

## Task 5: Presentation view models + request

**Files:**
- Modify: `Presentation/ViewModels/NewApplicationViewModel.cs:1,34`
- Modify: `Presentation/ViewModels/ContinueApplicationViewModel.cs:1,50`
- Modify: `Presentation/ViewModels/BridegroomFormViewModel.cs:1,62`
- Modify: `Presentation/ViewModels/MarriageApplicationFormViewModel.cs:1,58`
- Modify: `Presentation/ViewModels/JamaatPresidentReviewViewModel.cs:75`
- Modify: `Presentation/Requests/BridegroomSectionRequest.cs:1,17`

All five view models and the request replace `IsSecondThirdOrFourthNikah` with a nullable enum property and gain `using Domain.Enums;` (only `JamaatPresidentReviewViewModel.cs` already has it — line 2).

- [ ] **Step 1: `NewApplicationViewModel.cs`** — add after `using System.ComponentModel.DataAnnotations;`:

```csharp
using Domain.Enums;
```

Line 34:

```csharp
    public bool IsFirstNikah { get; set; }
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

- [ ] **Step 2: `ContinueApplicationViewModel.cs`** — add `using Domain.Enums;` after `using System.ComponentModel.DataAnnotations;`. Line 50:

```csharp
    public bool IsFirstNikah { get; set; }
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

- [ ] **Step 3: `BridegroomFormViewModel.cs`** — add `using Domain.Enums;` after `using System.ComponentModel.DataAnnotations;`. Line 62:

```csharp
    public bool IsFirstNikah { get; set; }

    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

- [ ] **Step 4: `MarriageApplicationFormViewModel.cs`** (namespace is `Presentation.ViewModel`, singular) — add `using Domain.Enums;` after `using System.ComponentModel.DataAnnotations;`. Line 58:

```csharp
    public bool IsFirstNikah { get; set; }
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

- [ ] **Step 5: `JamaatPresidentReviewViewModel.cs`** — has `using Domain.Enums;` already. Line 75:

```csharp
    public bool IsFirstNikah { get; set; }

    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

- [ ] **Step 6: `BridegroomSectionRequest.cs`** — add `using Domain.Enums;` after `using System;`. Line 17:

```csharp
    public bool IsFirstNikah { get; set; }
    public MarriageOrdinal? CurrentNikahOrdinal { get; set; }
```

- [ ] **Step 7: Verify**

Run: `dotnet build Presentation/Presentation.csproj`
Expected: COMPILES. (Will fail inside the mappings/controller still referencing the removed property — those are fixed in Task 6. If isolated build fails on the old references, that is expected mid-plan; continue to Task 6 and gate on the solution-wide build there.)

- [ ] **Step 8: Commit**

Only if the Presentation project builds. If it fails due to residual `.IsSecondThirdOrFourthNikah` references in Presentation, skip this commit and fold it into Task 6's commit.

```bash
git add Presentation/ViewModels Presentation/Requests
git commit -m "feat: use MarriageOrdinal? CurrentNikahOrdinal in presentation view models and request"
```

---

## Task 6: Presentation mappings + controller

**Files:**
- Modify: `Presentation/Mapping/Bridegroom/BridegroomMapping.cs:22,44`
- Modify: `Presentation/Mapping/JamaatPresident/JamaatPresidentMapping.cs:86`
- Modify: `Presentation/Mapping/MarriageFormRequestMapping.cs:42,81`
- Modify: `Presentation/Controllers/MarriageApplicationController.cs:180,309`

- [ ] **Step 1: `BridegroomMapping.cs`** — line 22 (`ToDto`):

```csharp
            IsFirstNikah = model.IsFirstNikah,
            CurrentNikahOrdinal = model.CurrentNikahOrdinal,
```

Line 44 (`ToViewModel`):

```csharp
            IsFirstNikah = dto.IsFirstNikah,
            CurrentNikahOrdinal = dto.CurrentNikahOrdinal,
```

- [ ] **Step 2: `JamaatPresidentMapping.cs`** — line 86:

```csharp
            IsFirstNikah = dto.IsFirstNikah,
            CurrentNikahOrdinal = dto.CurrentNikahOrdinal,
```

- [ ] **Step 3: `MarriageFormRequestMapping.cs`** — line 42 (`ToDto(BridegroomSectionRequest)`):

```csharp
            IsFirstNikah = request.IsFirstNikah,
            CurrentNikahOrdinal = request.CurrentNikahOrdinal,
```

Line 81 (`ToBridegroomDto(ContinueApplicationViewModel)`):

```csharp
            IsFirstNikah = model.IsFirstNikah,
            CurrentNikahOrdinal = model.CurrentNikahOrdinal,
```

- [ ] **Step 4: `MarriageApplicationController.cs`** — POST Create, line 180:

```csharp
            IsFirstNikah = model.IsFirstNikah,
            CurrentNikahOrdinal = model.CurrentNikahOrdinal,
```

`ToBridegroomViewModel`, line 309:

```csharp
        IsFirstNikah = form.IsFirstNikah,
        CurrentNikahOrdinal = form.CurrentNikahOrdinal,
```

- [ ] **Step 5: Verify (solution-wide gate)**

Run: `dotnet build AMJNRishtanata.slnx`
Expected: `0 Warning(s), 0 Error(s)`. This is the first real gate after the entity enum swap — if any `.IsSecondThirdOrFourthNikah`, bare `NikahOrdinal`, or `.NikahOrdinal` reference remains anywhere, this fails.

- [ ] **Step 6: Commit**

```bash
git add Presentation/Mapping Presentation/Controllers
git commit -m "feat: map CurrentNikahOrdinal through presentation mappings and Create/Continue controller"
```

---

## Task 7: Views — Create, Continue, Review

**Files:**
- Modify: `Presentation/Views/MarriageApplication/Create.cshtml:208-222`
- Modify: `Presentation/Views/MarriageApplication/Continue.cshtml:126-140`
- Modify: `Presentation/Views/JamaatPresident/Review.cshtml:387-402`

- [ ] **Step 1: `Create.cshtml`** — replace the single "Second, third or fourth Nikah" radio (lines 210-213) with three ordinal radios. Final block for `#remarriageGroup`:

```html
                    <div class="col-md-6 d-none" id="remarriageGroup">
                        <label class="form-label d-block">If not your first Nikah, which applies?</label>
                        <div class="form-check">
                            <input asp-for="CurrentNikahOrdinal" class="form-check-input remar-opt" type="radio" value="Mathna" id="renMathna" />
                            <label class="form-check-label" for="renMathna">Mathna (second)</label>
                        </div>
                        <div class="form-check">
                            <input asp-for="CurrentNikahOrdinal" class="form-check-input remar-opt" type="radio" value="Thulatha" id="renThulatha" />
                            <label class="form-check-label" for="renThulatha">Thulatha (third)</label>
                        </div>
                        <div class="form-check">
                            <input asp-for="CurrentNikahOrdinal" class="form-check-input remar-opt" type="radio" value="Arbaa" id="renArbaa" />
                            <label class="form-check-label" for="renArbaa">Arba'a (fourth)</label>
                        </div>
                        <div class="form-check">
                            <input asp-for="FormerWifeIsDead" class="form-check-input remar-opt" type="radio" value="true" id="renWidower" />
                            <label class="form-check-label" for="renWidower">Widower</label>
                        </div>
                        <div class="form-check">
                            <input asp-for="HasDivorcedFormerWife" class="form-check-input remar-opt" type="radio" value="true" id="renDivorced" />
                            <label class="form-check-label" for="renDivorced">Divorced (provide evidence below)</label>
                        </div>
                    </div>
```

> The `.remar-opt` cluster in `wireRemarriage()` (unchanged) makes all five radios mutually exclusive — the ordinal trio is exclusive among itself and vs Widower/Divorced, exactly the previous option-set semantics. `wireRemarriage` inside each page must NOT be edited. `renDivorced` keeps its id because the talaq-evidence toggle reads `document.getElementById('renDivorced')`.

- [ ] **Step 2: `Continue.cshtml`** — same replacement in the groom branch (`#remarriageGroup`, lines 126-140) as Step 1, keeping the surrounding `col-md-6 d-none` div and the `renWidower`/`renDivorced` radios identical.

- [ ] **Step 3: `Review.cshtml`** — add to the top of the file (before `@model`):

```razor
@using Domain.Constants
```

Replace the "Second / Third / Fourth Nikah: Yes/No" block (lines 387-402) with status-aware ordinal display:

```html
                <div class="col-md-6">
                    <strong>Current Nikah:</strong>

                    @if (Model.IsFirstNikah)
                    {
                        <span class="badge bg-success ms-2">
                            First nikah
                        </span>
                    }
                    else if (Model.CurrentNikahOrdinal is { } ordinal)
                    {
                        <span class="badge bg-warning text-dark ms-2">
                            @ordinal.Display()
                        </span>
                    }
                    else if (Model.FormerWifeIsDead)
                    {
                        <span class="badge bg-warning text-dark ms-2">
                            Remarriage (widower)
                        </span>
                    }
                    else if (Model.HasDivorcedFormerWife)
                    {
                        <span class="badge bg-warning text-dark ms-2">
                            Remarriage (divorced)
                        </span>
                    }
                    else
                    {
                        <span class="badge bg-secondary ms-2">
                            Remarriage
                        </span>
                    }
                </div>
```

- [ ] **Step 4: Verify**

Run: `dotnet build AMJNRishtanata.slnx`
Expected: `0 Warning(s), 0 Error(s)`.

- [ ] **Step 5: Commit**

```bash
git add Presentation/Views
git commit -m "feat: present Mathna/Thulatha/Arbaa radios and ordinal-aware review display"
```

---

## Task 8: EF migration + apply

> AGENTS.md: do not run `dotnet ef migrations add` blindly — reconcile deliberately against the live database. The model snapshot is currently at `ProductVersion 10.0.9`. Before starting, run the SHOW-COLUMNS probes in Step 0 below and confirm the live `NikahApplications`/`NikahGrooms` tables actually carry `IsSecondThirdOrFourthNikah`. `dotnet ef` must run from the repo root so `DotNetEnv.TraversePath()` finds `.env` (the `.env` supplies `ConnectionStrings:DefaultConnection`).

- [ ] **Step 0: Confirm live schema prerequisite**

Run (PowerShell, from a scratch dir or the dbprobe console — the raw SQL is what matters):

```sql
SHOW COLUMNS FROM NikahApplications LIKE 'IsSecondThirdOrFourthNikah';
SHOW COLUMNS FROM NikahGrooms LIKE 'IsSecondThirdOrFourthNikah';
```

Expected: one row each (`type` tinyint(1)). If either returns empty, STOP — the live DB is out of sync with the snapshot; reconcile before proceeding.

- [ ] **Step 1: Stop the running app** (it locks `Presentation.exe`)

```powershell
$c = Get-NetTCPConnection -LocalPort 5077 -State Listen -ErrorAction SilentlyContinue
if ($c) { Stop-Process -Id $c[0].OwningProcess -Force }
```

- [ ] **Step 2: Generate the migration**

Run (repo root): `dotnet ef migrations add SplitCurrentNikahOrdinal --project Infrastructure --startup-project Presentation`

Expected: new files `Infrastructure/Migrations/<timestamp>_SplitCurrentNikahOrdinal.cs` (+ `.Designer.cs`), and `RishtanataDbContextModelSnapshot.cs` updated. If the command fails on a dotnet-ef missing tool: `dotnet tool install --global dotnet-ef` then retry.

- [ ] **Step 3: Inspect the generated `Up`**

Open the generated `.cs`. It must drop `IsSecondThirdOrFourthNikah` and add `CurrentNikahOrdinal` (nullable `int`) on both tables:

```csharp
migrationBuilder.DropColumn(
    name: "IsSecondThirdOrFourthNikah",
    table: "NikahApplications");
migrationBuilder.AddColumn<int>(
    name: "CurrentNikahOrdinal",
    table: "NikahApplications",
    type: "int",
    nullable: true);
```

and the same pair for `NikahGrooms`. Correct anything that is off (wrong table names, non-nullable, missing drop) by editing the generated file — do not accept a migration that leaves the old bool or makes the column required. `Down` should reverse the pair.

- [ ] **Step 4: Apply**

Run (repo root): `dotnet ef database update`

Expected: applies the pending migration; output lists the migration name.

- [ ] **Step 5: Verify the live schema**

```sql
SHOW COLUMNS FROM NikahApplications LIKE 'CurrentNikahOrdinal';
SHOW COLUMNS FROM NikahGrooms LIKE 'CurrentNikahOrdinal';   -- both rows present, int / nullable
SHOW COLUMNS FROM NikahApplications LIKE 'IsSecondThirdOrFourthNikah';  -- both empty now
```

- [ ] **Step 6: Build**

Run: `dotnet build AMJNRishtanata.slnx`
Expected: `0 Warning(s), 0 Error(s)`.

- [ ] **Step 7: Commit**

```bash
git add Infrastructure/Migrations
git commit -m "feat: migrate IsSecondThirdOrFourthNikah to CurrentNikahOrdinal (int) on NikahApplications and NikahGrooms"
```

---

## Task 9: End-to-end runtime verification + cleanup

> Uses the running app, real Tajneed login (`971362`/`971362`), partner bride `327691` (HAFSAT DERE, eligible) and the dbprobe console (recreate at `C:\Users\user\AppData\Local\Temp\opencode\dbprobe` if the temp dir was wiped: net10.0 console, package `MySql.Data 9.2.0`, connection `Server=::1;Port=3306;Database=rishtanatahdb;User=root;Password=Html5001#;`).

- [ ] **Step 1: Start the app**

```powershell
Start-Process dotnet -ArgumentList "run --project Presentation --urls http://localhost:5077 --no-build" -RedirectStandardOutput C:\Users\user\AppData\Local\Temp\opencode\ordinal.out.log -RedirectStandardError C:\Users\user\AppData\Local\Temp\opencode\ordinal.err.log -WindowStyle Hidden
```

Wait ~12s, then `Invoke-WebRequest http://localhost:5077/Auth/Login` must return 200.

- [ ] **Step 2: Create screen**

- Log in as `971362`; open `http://localhost:5077/MarriageApplication/Create`.
- Assert the groom card shows three ordinal radios (Mathna/Thulatha/Arba'a) inside `#remarriageGroup`, and that NOT ONE carries `data-val-required` (the nullable enum must not be implicitly required — this is the trap the earlier fix addressed for strings).
- Fill the form normally setting "First Nikah = No" and "Mathna (second)"; `checkForm()` must return `true` and the Save & Continue button must be enabled.
- Submit → expect a **302 redirect** to `/Applications` and a row in DB.

- [ ] **Step 3: DB write check**

Via dbprobe: `SELECT ReferenceNumber, BridegroomMembershipNo, IsFirstNikah, CurrentNikahOrdinal FROM NikahApplications ORDER BY id DESC LIMIT 1;` and the matching `NikahGrooms` row. Expected: `IsFirstNikah=0`, `CurrentNikahOrdinal=2`.

- [ ] **Step 4: Continue + review screens**

- As groom `971362`, Continue the new application: the groom section must show the same three ordinal radios.
- As the JamaatPresident, open Review for the application: the groom block must read "Mathna (second)", never "Second / Third / Fourth Nikah".
- Submit the groom section with "Mathna (second)" + no former-wife status → the FluentValidation message "For a previous Nikah, select exactly one former-wife status." must appear (evidence the previously dead validator branch now fires). Pick "Divorced" + evidence → submission passes.

- [ ] **Step 5: First-nikah path**

New application logged in as groom `971362`, "First Nikah = Yes" → submits fine, `CurrentNikahOrdinal` stores `NULL`.

- [ ] **Step 6: Cleanup test data**

Delete the test `NikahApplications`/`NikahGrooms`/`ApplicationSubmissions` rows you created (keep the pre-existing orphans `f5cf2ad3…`, `378e850f…`, `8f681ff8…` untouched). Verify `SELECT COUNT(*) FROM NikahApplications` returns 0.

- [ ] **Step 7: Report**

Summarize for the user: root cause of the original "Save & Continue does nothing" bug (implicit-required validation blocking, already fixed), and this feature now live end-to-end: enum storage, three ordinal radios on Create/Continue, ordinal-aware Review display, migration applied. Include the build verification and the DB probes you ran.

---

## Self-review notes

- **Spec coverage:** spec §1 (enum + display helper) → Task 1; entities §1 → Task 2; §2 persistence/mapping/migration → Tasks 3 + 8; services/validator §2 → Task 4; VMs/request §3 → Task 5; controller/mappings §3 → Task 6; views §3 → Task 7; verification §5 → Task 9. The spec's four "Unchanged" items are untouched.
- **Placeholders:** none — every step carries literal code or a literal command.
- **Type consistency:** the property is `MarriageOrdinal? CurrentNikahOrdinal` everywhere; enum members `Mathna`, `Thulatha`, `Arbaa`; radio values are the enum member names; `NikahOrdinal` (old global enum) is deleted in Task 2 and its two consumers rewired (DTO in Task 3, validator in Task 4) — no task references it after Task 4.
- **Known mid-plan state:** after Task 2 removes `IsSecondThirdOrFourthNikah` from the entities and deletes `NikahOrdinal`, the solution stops compiling until Tasks 3/4/5/6 replace every reference. Each task gates on its own project build where possible; Task 6 Step 5 is the single full-solution gate that must be green before moving on.