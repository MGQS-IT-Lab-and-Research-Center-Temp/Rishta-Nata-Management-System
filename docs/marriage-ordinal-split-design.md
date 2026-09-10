# Design: Split "Second, third or fourth Nikah" into Mathna / Thulatha / Arba'a

Date: 2026-09-10
Status: Approved (in brainstorming)
Scope: New-application Create screen, Continue partner-groom section, JamaatPresident review screen, and the full
stack that stores/reads the groom's current-marriage ordinal.

## Context

The new-application form currently offers a single combined boolean option, `IsSecondThirdOrFourthNikah`,
labelled "Second, third or fourth Nikah". The user cannot record *which* ordinal applies (second, third, or
fourth). This design replaces that single boolean with a ternary choice using the Arabic transliterations
**Mathna** (second), **Thulatha** (third) and **Arba'a** (fourth) so it is unambiguous which wife the groom is taking.

A concurrent bug fix (implicit-required validation from `Nullable` on non-nullable reference types, applied in
`Presentation/Program.cs`) is unrelated and already committed to the working tree; this design only touches the
field split.

## Decisions (from brainstorming)

1. Storage: one **nullable enum field** replaces the boolean (not three bools). Guarantees at most one of
   Mathna/Thulatha/Arba'a can be set.
2. Scope: the split applies to **all three screens** — Create groom card, Continue partner-groom section, and
   the JamaatPresident review display — because they represent the same fact.

## 1. Domain

New enum in `Domain/Enums/MarriageOrdinal.cs`:

```csharp
namespace RishtaNata.Domain.Enums;

public enum MarriageOrdinal
{
    Mathna = 2,
    Thulatha = 3,
    Arbaa = 4
}
```

> C# member names cannot contain an apostrophe, hence `Arbaa`. Display strings handle the punctuation.

Display helper in `Domain/Constants/MarriageOrdinalDisplay.cs`:

```csharp
namespace RishtaNata.Domain.Constants;

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

Entity changes:

- `Domain/Entities/MarriageApplicationForm.cs`: `bool IsSecondThirdOrFourthNikah`
  → `MarriageOrdinal? CurrentNikahOrdinal`.
- `Domain/Entities/BridegroomFormSection.cs`: same replacement.

`null` means "not specified" (first nikah, or the screen did not capture an ordinal).

## 2. Persistence and mapping

New EF migration (applied deliberately against the live dev MySQL per AGENTS.md — do not run
`dotnet ef migrations add` blindly; reconcile with the running database first):

- Both affected tables (NikahApplications and BridegroomFormSections):
  - Drop column `IsSecondThirdOrFourthNikah`.
  - Add column `CurrentNikahOrdinal` as nullable `tinyint unsigned` (`MarriageOrdinal?`, int store).

Data: the dev database currently holds only test rows (all `NikahApplications` test apps deleted;
`ApplicationSubmissions` holds orphan parents). No real data to preserve; the migration drops the old column
unconditionally. If any rows exist they will lose the old boolean — acceptable for the dev database, to be noted
in the migration up step.

DTOs (swap property to `MarriageOrdinal?`):

- `Infrastructure/DTOs/BrideGroom/BridegroomSectionDto.cs`
- `Infrastructure/DTOs/MarriageApplicationFormDetail/BridegroomSectionDetailDto.cs`
- `Infrastructure/DTOs/ReadOnlyFormDto.cs`
- `Infrastructure/DTOs/JamaatPresidentDashboardDto/JamaatPresidentReviewDto.cs`

Mappers (map ordinal ↔ nullable enum, `null` for first-nikah/unset):

- `Infrastructure/Mapper/BrideGroomMapper.cs`
- `Infrastructure/Mapper/MarriageApplicationFormDetailMapper.cs`
- `Infrastructure/Mapper/ReadOnlyFormMapper.cs`

Application services (swap bool → `MarriageOrdinal?`; propagation unchanged):

- `Application/Services/BridegroomService.cs`
- `Application/Services/BridegroomSectionService.cs`
- `Application/Services/MarriageApplicationFormService.cs`
- `Application/Services/JamaatPresidentService.cs`
- `Application/Validators/SubmissionValidators.cs`

`SubmissionValidators` keeps the consistency rule: `CurrentNikahOrdinal` set ⟺ `IsFirstNikah` is false. No
cross-ordinal sequencing checks (Mathna before Thulatha before Arba'a is not trackable from a single field; the
field records only the *current* nikah's ordinal).

## 3. Presentation

View models (swap to `MarriageOrdinal?`):

- `Presentation/ViewModels/NewApplicationViewModel.cs`
- `Presentation/ViewModels/ContinueApplicationViewModel.cs`
- `Presentation/ViewModels/BridegroomFormViewModel.cs`
- `Presentation/ViewModels/MarriageApplicationFormViewModel.cs`
- `Presentation/ViewModels/JamaatPresidentReviewViewModel.cs`

Prompts / mappings / request:

- `Presentation/Requests/BridegroomSectionRequest.cs`
- `Presentation/Mapping/Bridegroom/BridegroomMapping.cs`
- `Presentation/Mapping/JamaatPresident/JamaatPresidentMapping.cs`
- `Presentation/Mapping/MarriageFormRequestMapping.cs`

Controllers:

- `Presentation/Controllers/MarriageApplicationController.cs` — POST Create maps `model.CurrentNikahOrdinal`
  directly (nullable enum, no `?? false` needed for the ordinal itself); everything else unchanged.

Views — Create and Continue groom cards:

- `Presentation/Views/MarriageApplication/Create.cshtml` (~line 211) and
  `Presentation/Views/MarriageApplication/Continue.cshtml` (~line 129): replace the single `renSubseq` radio
  `<input asp-for="IsSecondThirdOrFourthNikah" ... value="true" id="renSubseq" />` with three radios for
  `CurrentNikahOrdinal`:

  - `renMathna` value `Mathna` → "Mathna (second)"
  - `renThulatha` value `Thulatha` → "Thulatha (third)"
  - `renArbaa` value `Arbaa` → "Arba'a (fourth)"

  All three keep class `remar-opt` and live inside `#remarriageGroup`, so `wireRemarriage()` needs **no** changes
  (it only toggles group visibility via `.remar-opt`).

Review screen:

- `Presentation/Views/JamaatPresident/Review.cshtml` (~line 388): replace the
  "Second / Third / Fourth Nikah: Yes/No" block with the specific choice, e.g.
  `Current nikah: @(Model.CurrentNikahOrdinal?.Display() ?? "First nikah")` — `null` renders "First nikah".

## 4. Unchanged

`IsFirstNikah`, `FormerWifeIsDead`, `HasDivorcedFormerWife`, dower amounts, divorce evidence, additional-wives
fields, and the implicit-required bug fix in `Program.cs`.

## 5. Verification

Build: `dotnet build AMJNRishtanata.slnx` (0 warnings/0 errors is the normal state).

Manual/browser checks (app at `http://localhost:5077`, login `971362`/`971362`, partner `327691`):

1. GET Create: three ordinal radios inside `#remarriageGroup`, shown only when "First Nikah = No";
   no `data-val-required` on any of them (nullable enum does not get implicit-required).
2. With "First Nikah = No" + "Mathna (second)" (no other remarriage option): `checkForm()` true, Save & Continue
   enables, click → POST → 302 → application created. Confirms the ordinal no longer blocks submission the way
   the old single-radio bool did.
3. DB check (dbprobe): column `IsSecondThirdOrFourthNikah` gone, `CurrentNikahOrdinal = 2` row written.
4. Continue screen for the partner groom and Review screen show the same ordinal (Mathna/Thulatha/Arba'a),
   never the old combined label.
5. First-nikah path (No → nothing selected on a first nikah): still submits, ordinal stored `NULL`.

## 6. Files touched (summary)

Domain (2 entities + 2 new files), Infrastructure (1 migration + 4 DTOs + 3 mappers), Application
(4 services + 1 validator), Presentation (5 view models + 1 request + 3 mappings + 1 controller + 3 views).