# Domain Layer - Role Management Cleanup

This document records the changes made to the **Domain** layer to remove local
role management and make the domain expect roles from the external Tajneed API.
It also details what developers working on the **Application**, **Presentation**
and **Infrastructure** layers must do to align their code with the new domain
model.

> Roles are now sourced **exclusively** from the external member API's login
> response (`Data.Roles`) — there is **no local role table** anymore. See
> `docs/stage-authorization-policy.md` §3.2 (§8 Q3 for the still-open role-string
> question).

## What was implemented (Domain layer)

### 1. Removed the local role model
| File | Change |
|---|---|
| `Domain/Entities/Role.cs` | **Deleted** — local role entity removed |
| `Domain/Entities/JamaatMemberRole.cs` | **Deleted** — local member↔role join removed |

### 2. `JamaatMember` now holds API-sourced role names
`Domain/Entities/JamaatMember.cs`:
- Removed `ICollection<JamaatMemberRole> MemberRoles` (the many-to-many join).
- Removed `string NewRole` (old single-role field).
- Added `ICollection<string> Roles` — the role-name strings reported by the
  external API login response (`Data.Roles`).
- Removed the leftover dead fields `IsSystemDefault`, `ResetToken`,
  `ResetTokenExpiry` (see `docs/bugs-and-gaps.md` #10).
- Normalized the inconsistent nullable style (`string? ... = string.Empty!` →
  `= string.Empty`), preserving all nullability types.

### 3. New shared role-name constant
`Domain/Constants/RoleNames.cs` (new):
- Canonical role-name constants (JamaatSecretary, CircuitSecretary,
  RishtanataSecretary, Amir, plus workflow roles JamaatPresident, Imam,
  Missionary).
- **PROVISIONAL**: exact strings unconfirmed against the live API (open
  question §8 Q3 in `docs/stage-authorization-policy.md`). Matching should be
  case-insensitive.

### 4. `AuditLog` now inherits `AuditableEntity`
`Domain/Entities/AuditLog.cs` (see `docs/bugs-and-gaps.md` #12):
- Now inherits `AuditableEntity` → gains `Id`, `CreatedAt`, `CreatedBy`,
  `ModifiedAt`, `ModifiedBy`.
- Removed hand-rolled `Id` and `Timestamp`.

## What other layers are expected to do

### Infrastructure
1. **Remove role persistence:**
   - Delete `Infrastructure/Configurations/JamaatMemberRoleConfiguration.cs`.
   - Remove `DbSet<Role> JamaatRoles` and `DbSet<JamaatMemberRole> JamaatMemberRoles`
     from `Infrastructure/Persistence/RishtanataDbContext.cs`.
   - Delete `Infrastructure/Mapper/RoleMapper.cs`.
   - Delete `Infrastructure/DTOs/Roles/RoleDto.cs` and `RoleManagementDto.cs`.
   - Update `Infrastructure/Mapper/JamaatMemberMapper.cs` — stop mapping
     `RoleIds`/`NewRole`/`MemberRoles`; map `JamaatMemberDto.Roles` ↔
     `JamaatMember.Roles` instead.
   - Update `Infrastructure/DTOs/JamaatMember/JamaatMemberDto.cs` — replace
     `RoleIds` and `NewRole` with a `Roles` (list of strings) property.
2. **Migrations:** add a new migration that drops the `JamaatRoles` and
   `JamaatMemberRoles` tables and the removed columns. Do **not** edit the
   existing migrations (keep history); resolve stale-snapshot debt per
   `docs/bugs-and-gaps.md` #2.
3. **AuditLog schema config:** update `AuditLogConfiguration.cs` — remove the
   mapping for the deleted `Timestamp` column (now on `AuditableEntity`).

### Application
1. **Remove local role services:**
   - Delete `Application/Services/RoleService.cs`,
     `Application/Interfaces/IRoleService.cs`,
     `Application/Services/RoleAssignmentService.cs`,
     `Application/Interfaces/IRoleAssignmentService.cs`.
2. **`JamaatMemberService`**: stop resolving local `JamaatRoles` FKs
   (`ResolveRoleIdAsync`) and stop writing `MemberRoles`/`JamaatMemberRole`.
   Copy the API-supplied role names (`member.Roles`) directly on create/update.
3. **`StageAuthorizationService`**: replace the `HierarchyLevel`-based gates
   (`RequireHierarchyLevel`) and the `MemberRoles`/`Role` navigation with
   case-insensitive name matching against `member.Roles` / `Domain.Constants.RoleNames`.
4. **`RishtanataSecretaryService`**: derive `RoleName` from `member.Roles`
   instead of `MemberRoles`; drop the `.Include(MemberRoles)`.
5. **Login role flow**: populate `JamaatMember.Roles` from the API login response
   `Data.Roles` (currently unimplemented — see `docs/bugs-and-gaps.md` #3, #4).

### Presentation
1. **Remove role UI/controllers:**
   - Delete `Presentation/Controllers/RoleController.cs`.
   - Delete `Presentation/Mapping/RishtanataSecretary/RoleManagementMapper.cs`,
     `Presentation/ViewModels/.../RoleManagementViewModel.cs`, and
     `Presentation/Views/RishtanataSecretary/ManageRoles.cshtml`.
   - Remove the `EditRoles` action and the `IRoleAssignmentService` dependency
     from `Presentation/Controllers/RishtanataSecretaryController.cs`.
2. **Remove DI registrations** for `IRoleService` and `IRoleAssignmentService`
   in `Presentation/Extensions/DependencyInjection.cs`.
3. **Authentication claims:**
   - `Presentation/Services/Auth/CookieAuthenticationService.cs`: build the
     `Role`/`member_roles` claims from `jamaatMember.Roles` (API-supplied), not
     `NewRole`/`MemberRoles`.
   - `Presentation/Controllers/AuthController.cs`: wire the login response
     `Data.Roles` into the member before sign-in; update `RedirectUserToDashboard`
     to use `JamaatMember.Roles`.
4. **View models/mappings:** update `JamaatMemberVM` and
   `Presentation/Mapping/JamaatMember/JamaatMemberMapping.cs` to replace
   `RoleIds`/`NewRole` with `Roles`.
5. **Roles constant:** `Presentation/Constants/Roles/RoleNames.cs` currently
   duplicates `Domain.Constants.RoleNames`. Point Presentation usages at the
   Domain constant (single source of truth).

## Deliberately left untouched (needs product decision)

- `Certificate` ↔ `FormApplication` ↔ `MarriageApplicationForm` relationship
  ambiguity (including the dead `Certificate.MarriageApplicationFormId` /
  `MarriageApplicationForm.Certificate` navigations). See
  `docs/bugs-and-gaps.md` #1 — this requires a product decision on which entity
  owns the certificate before anyone edits it.

## Verification

The full solution does **not** currently compile end-to-end until the
Application / Infrastructure / Presentation items above are completed (they still
reference the removed role types). The Domain layer itself is self-consistent.
