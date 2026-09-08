# Bugs and Gaps

Known issues and gaps in the Rishta-Nata codebase, with evidence. Update this
file as items are fixed.

## Fixed (recent session)

- **Guardian/witness submission is now live (was D2 stub).** Anonymous,
  revocable, stage-gated share links (`SectionLinks` + `SharedSection`,
  `SharedSectionService`) replace the formerly-stubbed endpoints. Legacy
  `WitnessController` / `BrideGuardianController` remain wired but are
  superseded (flag for removal).
- **Test project no longer compiled** — `JamaatMember.Password` was removed but
  still referenced by three test files. Fixed by removing the `Password` seed
  lines. Tests now compile.
- **Duplicate DI registrations** in `Presentation/Extensions/DependencyInjection.cs`
  (`IStageAuthorizationService`, `IMarriageApplicationFormDetailService`,
  `IEmailSender`, `IMarriageFormNotificationService` were each registered twice).
- **Duplicate `AddSwaggerGen()`** in `Presentation/Program.cs`.
- **Unused `Microsoft.EntityFrameworkCore.SqlServer`** reference removed from
  `Presentation.csproj`.
- **`AddIdentity<ApplicationUser, ApplicationRole>`** removed — it was registered
  against a `DbContext` that no longer maps those entities. Authentication is
  external (Tajneed) via cookie auth, so ASP.NET Identity is unused.
- **`RoleAssignmentService` cross-model bug** — it resolved roles via
  `RoleManager<ApplicationRole>` (Identity) and assigned `member.RoleId` from the
  Identity role. Now resolves from `_context.JamaatRoles` (domain `Role`), and no
  longer writes `member.Role.UpdatedBy` (which NRE'd on an unloaded navigation).
- **Login NullReferenceException** — `CookieAuthenticationService` / `AuthController`
  dereferenced a null `Role`. Now null-guarded, and `AuthController` signs in the
  **local** member returned by `JamaatMemberService` (the gateway member's `Id`
  does not match the local DB row's `Id`).

## Blocking

1. **Certificate ↔ FormApplication ↔ MarriageApplicationForm relationships —
   RESOLVED.** The ambiguity is resolved in `RishtanataDbContext.OnModelCreating`
   (`Certificate` is the dependent side via `HasForeignKey(c => c.FormApplicationId)`)
   and `CertificateConfiguration` ignores the dead `MarriageApplicationForm` /
   `MarriageApplicationFormId` leftovers. No product decision pending.

2. **EF migrations — regenerated from scratch.** Local development (no data-loss
   concern), so the DB was dropped and all migrations replaced by a single fresh
   `InitialCreate` (`20260907105655_InitialCreate`) spanning the whole current
   model, applied to the recreated `rishtanatahdb`. ReferenceNumber is now a
   `varchar(50)` column on all four shareable sections, `BrideFormSection`, and
   `BrideGuardian`. The `MySQL:Charset` annotations emitted by the current
   `MySql.EntityFrameworkCore` provider are correct; no further reconciliation
   needed unless the provider changes. Future schema changes must be additive
   migrations on top of this baseline.

## Design / decision

3. **Login role provisioning is unimplemented.** Login authenticates against
   Tajneed (`GenerateToken`) and fetches the member (`GetMemberByChandaNoAsync`),
   but never resolves the member's roles. Tajneed `GetMemberRoleAsync` is defined
   but unused in the login flow, and `JamaatMemberService.CreateOrUpdateAsync`
   copies the gateway's `RoleId` (likely dangling against local `Role` IDs).
   Result: members sign in with no local `Role`, so the `ClaimTypes.Role` claim is
   empty and role-gated policies/pages don't match. Need: map Tajneed role strings
   → local `Domain.Entities.Role` and set it during login/sync.

4. **Policy vs implementation divergence on claims.**
   `docs/stage-authorization-policy.md` §3.2 requires a `membership_no` claim and
   `member_roles` claims and says authorization must read *only* the
   `membership_no` claim. The implementation issues `ClaimTypes.NameIdentifier`
   (member Guid), `ClaimTypes.Name` (ChandaNo), `ClaimTypes.Role`, and
   `StageAuthorizationService` resolves the member by `JamaatMember.Id == userId`
   (Guid), not ChandaNo. Decide which is authoritative and align the other.

## Security

5. **Hardcoded MySQL credentials committed.** `Password=yuzzypizzy2007?` appears
   in both `API/appsettings.json:3` and `Presentation/appsettings.json:4`. Rotate
   and move to user-secrets/env vars.

6. **`AuthController.Login` swallows all exceptions** as "Invalid Chanda number or
   password" (`Presentation/Controllers/AuthController.cs`), hiding API/network
   failures.

## Minor

7. **Witness matching loads the whole table.**
   `StageAuthorizationService.MatchesWitnessSlotAsync` fetches *all* `JamaatMembers`
   into memory and counts matches client-side.

8. **Design-time factory hardcodes `../Presentation`.**
   `Infrastructure/Persistence/RishtanataDbContextFactory.cs:17` — migrations only
   work when `dotnet ef` runs from a directory where `../Presentation` resolves.

9. **Namespace/path mismatches.**
   - `IGatewayHandler` file is `Application/Interfaces/Auth/IGatewayHandler.cs`
     but declares `namespace Application.Interfaces.Identity`.
   - `IRoleService` file is `Application/Interfaces/IRoleService.cs` but declares
     `namespace Application.Roles`.
   - `IRishtanataSecretaryServices.cs` filename is plural; the interface is
     `IRishtanataSecretaryService` (singular).

10. **Dead code:** `ApplicationUser` / `ApplicationRole`
    (`Infrastructure/Identity/`) are now unreferenced after the Identity removal;
    only the stale snapshot references them. Can be deleted once migrations are
    reconciled (item 2). `JamaatMember` still carries leftover fields
    (`ResetToken`, `ResetTokenExpiry`, `IsSystemDefault`, `NewRole`).

11. **Stray committed files.** Root `cls` is an accidental `git branch -a` dump;
    `Presentation/Presentation.csproj.user` and `API/API.csproj.user` are
    committed but should be gitignored.

12. **`AuditLog` does not inherit `AuditableEntity`**
    (`Domain/Entities/AuditLog.cs`) — it rolls its own `Id`/`Timestamp` and lacks
    `CreatedBy`/`ModifiedBy`.

13. **Nullable warnings** (not errors): `JamaatPresidentService.cs:97` (CS8602),
    `MarriageFormStageRevertedEventHandler.cs:96` (CS8629).

14. **Additive migration not yet applied to a live MySQL.**
    `20260908155154_AddDivorceEvidence` (4 × `varchar(500)`:
    `NikahApplications.BrideDivorceEvidence`/`BridegroomDivorceEvidence`,
    `NikahBrides.BrideDivorceEvidence`, `NikahGrooms.BridegroomDivorceEvidence`)
    predates any live-DB apply. Until applied, the new divorce-evidence fields are
    all empty and the eligible-divorce path will store nothing. Apply via
    `dotnet ef database update` (or equivalent SQL) against MySQL when
    appropriate — do not hand-edit `InitialCreate`.

15. **Eligibility denies reuse `WrongStage` deny reason.**
    `BrideSectionService`/`BridegroomSectionService` return
    `StageAuthorizationDenyReason.WrongStage` with the partner-eligibility message
    because no code branches on the reason. Deliberate; an enum addition
    (`PartnerNotEligible = 8`) is a plausible follow-up if reasons ever are acted on.

16. **Divorce-evidence persistence is client-consented, not server-gated.**
    The section services persist `BrideDivorceEvidence`/`BridegroomDivorceEvidence`
    unconditionally; correctness relies on the UI clearing the input when its
    status group is hidden (commit `4316ce3`). A spoofed POST could store evidence
    without the corresponding divorced flag. Harden server-side only if such data
    integrity becomes important.
