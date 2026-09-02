# Application Layer - Cleanup Summary

This document records the restructuring performed on the **Application** layer
(structural cleanup, plus completing a few half-implemented service lifelines —
no new features added).

## Layer layout

| Folder | Purpose |
|---|---|
| `Services/` | Use-case implementations |
| `Interfaces/` | Contracts consumed by the Presentation layer |
| `Authorization/` | Stage-based authorization policy primitives |
| `Workflow/` | Submission payloads for the staged paper-form sections |
| `Validators/` | FluentValidation rules for section submissions |
| `EventHandlers/` | Domain-event reactions (e.g. revert notifications) |

## What was done

### 1. Namespace mismatches fixed (folder now mirrors namespace)
- `IGatewayHandler` moved `Interfaces/Auth/` → `Interfaces/Gateway/` and
  namespace `Application.Interfaces.Identity` → `Application.Interfaces.Gateway`.
- `IRoleService` namespace `Application.Roles` → `Application.Interfaces` (the
  `Application.Roles` folder never existed).
- `JamaatPresidentService` namespace `Application.Interfaces` → `Application.Services`.

### 2. File names aligned with the types they declare
- `IRishtanataSecretaryServices.cs` → `IRishtanataSecretaryService.cs`
- `IRoleManagementService.cs` → `IRoleAssignmentService.cs`
- `Services/RoleManagementService.cs` → `Services/RoleAssignmentService.cs`
- `Services/CertificatesService.cs` → `Services/CertificateService.cs`

### 3. Dead code and duplication removed
- Deleted empty `Interfaces/INotificationService.cs` and unused
  `Interfaces/INotificationDispatcher.cs` (zero references, never registered).
- Merged the two identical `ComputeCanCurrentUserEditAsync` /
  `ComputeCanCurrentUserRejectAsync` methods into one
  `ComputeCanCurrentUserActAsync` (both UI flags gate identically).

### 4. Documentation added
- Short summary (`///`) added to every service and interface explaining its
  responsibility, with cleanup notes where a file was moved/renamed.
- `Application.csproj` now carries a layer overview comment.

### 5. Half-implemented services completed
- `RoleAssignmentService` (`Application/Services/RoleAssignmentService.cs`):
  removed the throwing constructor and the stale ASP.NET Identity
  (`RoleManager<ApplicationRole>`) overload; implemented `AssignRoleAsync` /
  `RemoveRoleAsync` / `ResetToBaseRoleAsync` against the `JamaatMemberRole`
  join rows (they validated guards but never wrote before).
- `RishtanataSecretaryService`: `Approve` / `Reject` / `ReturnToPresident` were
  fire-and-forget (`SaveChangesAsync` unawaited); now `Task`-based and awaited.
  Touched: `Application/Services/RishtanataSecretaryService.cs`,
  `Application/Interfaces/IRishtanataSecretaryService.cs`,
  `Presentation/Controllers/RishtanataSecretaryController.cs`.
- `RequestMoreInformationAsync` (both `FormApplicationService` and
  `JamaatPresidentService`) was a no-op — it flipped a pending form back to
  pending. Added `ApplicationStatus.AwaitingMoreInformation` to
  `Domain/Enums/ApplicationStatus.cs` so the state is distinguishable, made the
  approve/reject guards accept both pending-ish states so a returned form is
  not orphaned, and updated the pending dashboards
  (`FormApplicationService`, `RishtanataSecretaryService`,
  `JamaatPresidentService`) to count it.

### 6. Mixed-responsibility services split
- Section submission moved out of the record-CRUD services: new
  `IBrideSectionService` / `BrideSectionService` and `IBridegroomSectionService`
  / `BridegroomSectionService`
  (`Application/Interfaces/IBrideSectionService.cs`,
  `Application/Services/BrideSectionService.cs`,
  `Application/Interfaces/IBridegroomSectionService.cs`,
  `Application/Services/BridegroomSectionService.cs`).
  `IBrideGuardianService`/`BrideGuardianService` and
  `IBridegroomService`/`BridegroomService` now contain only their CRUD.
  Registration and call sites updated in
  `Presentation/Extensions/DependencyInjection.cs` and
  `Presentation/Controllers/MarriageApplicationFormController.cs`.
- Signature submission (`SubmitGuardianOrWakeelAsync` /
  `SubmitWitnessSignatureAsync`) stays in `MarriageApplicationFormService` —
  those routes are blocked on backlog D2 and no live code calls them, so
  splitting them would create a service for dead code.

## Deliberately left untouched

These need product decisions or a sizable architectural commitment before they
can be changed — implementing them here would contradict the project's own
policy document or "no new things" constraint:

- Two parallel stage enums (`ApplicationStage` vs `MarriageFormStage`) still
  drive two `CanUserActAsync` overloads. Unifying them is a product decision
  (see the open questions in `docs/stage-authorization-policy.md`) and touches
  authorization, controllers and the database — no test project exists to catch
  stage regressions.
- `Application` still references `Infrastructure` directly (DTOs + EF context);
  a purist layer would depend only on `Domain`. Inverting it requires introducing
  abstraction/repository contracts and moving DTOs — a new architectural fixture
  for the cohort to own, not a cleanup.

## Verification

`dotnet build AMJNRishtanata.slnx` — build succeeds