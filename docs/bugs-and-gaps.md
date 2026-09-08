# Bugs and Gaps

Known issues and gaps in the Rishta-Nata codebase, with evidence. Update this
file as items are fixed.

## Fixed (recent session)

- **FormStage/Revert deadlock resolved.** `MarriageApplicationForm.ApplicationStage`
  was initialised once (`ApplicantsReview`) and never advanced, so
  `RevertStageAsync` (which requires `targetStage < currentStage`) always failed.
  `MarriageFormWorkflowService.AdvanceAsync` now derives and writes
  `ApplicationStage` from `FormStage` via the new `Application.Workflow.WorkflowStageMapping`,
  and `RevertStageAsync` keeps the two in sync by also writing `FormStage`. Revert
  can now actually move a form back.
- **Revert enum misalignment resolved.** Forward transitions run on `FormStage`
  (0-8), revert on `ApplicationStage` (1-4). `WorkflowStageMapping` is now the
  single bi-directional mapping; there is no independent second progression.
- **President Review key mismatch resolved.** `JamaatPresidentController.Review`
  previously looked up a `Review.Id` from a `FormApplication.Id` (and the Review
  table is never populated), and the Review page posted a `Review.Id` back to
  `Approve`/`Reject` which resolve `FormApplication`. `JamaatPresidentService.GetReviewByIdAsync`
  now resolves by `FormApplication.Id` and returns `null` on a miss; the review
  page posts the same id, so the whole flow is self-consistent.
- **CSRF holes closed.** `RishtanataSecretaryController.Approve/Reject` now have
  `[ValidateAntiForgeryToken]`; `MarriageFormController.RevertStage`,
  `MarriageApplicationFormController` (all POST/PUT) and `InvitationsController.Generate`
  too. The rejection modal now emits `@Html.AntiForgeryToken()` so the header token
  it posts is actually generated.
- **Anonymous endpoints secured.** `JamaatMemberDashboardController`,
  `MemberLookupController`, `InvitationsController.Generate` and the
  `GetForm` API action now require `[Authorize]`. `InvitationsController.Accept`
  stays anonymous because it is token-scoped.
- **Tajneed transport failures no longer masquerade as bad credentials.**
  `AuthService.LoginAsync` catches `HttpRequestException`/`TaskCanceledException`/
  `TimeoutException` and reports "the member service is temporarily unreachable"
  instead of swallowing them as invalid login.
- **Retry added for Tajneed calls.** New `Presentation/Services/RetryDelegatingHandler`
  (no extra package; fixed backoff, max 3 attempts) wired on the
  `IGatewayHandler` HttpClient in `Presentation/Extensions/DependencyInjection.cs`.
- **Missing null guards fixed.** `RishtanataSecretaryService.GetById` /
  `GetMemberProfile` return `null` instead of throwing; `Approve`/`Reject`/
  `ReturnToPresident` return `false` on a miss. Controllers (`RishtanataSecretaryController`,
  `JamaatPresidentController`) null-guard the results. `RishtanataSecretaryService.GetById`
  now `Include`s `MarriageApplication` (was a latent NRE).
- **`@page` residuals removed** from `RishtanataSecretary/Review.cshtml` and
  `MarriedCouples.cshtml`.
- **Await-less async removed.** `RishtanataSecretaryController.PendingApprovals`,
  `MarriedCouples`, `Review`, `MemberProfile` are no longer `async` (CS1998 gone).
- **`ViewData["CurrentStage"]` now honoured.** The rejection modal renders only
  stages earlier than the form's actual `ApplicationStage` (from the new
  `CurrentStage` on both review DTOs), correcting the previous hardcoded
  mismatched option values (0-3) which never matched the `ApplicationStage`
  enum (1-4).
- **`RevertStage` validates model state.** `MarriageFormController.RevertStage`
  returns 400 on invalid/absent `targetStage` or empty reason;
  `RevertStageRequest.TargetStage` is now nullable `[Required]` so a missing
  value no longer silently binds to `0`.
- **Hardcoded MySQL password removed.** `Presentation/appsettings.json` no longer
  carries a plaintext password (was `Password=yuzzypizzy2007?`). Connection is via
  `.env` / user-secrets.
- **Git case-collision file removed.** Only `BridegroomFormSection.cs` (lowercase
  `g`) exists in the index; the `BrideGroomFormSection.cs` duplicate is gone.

## Previously fixed

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
   `BrideGuardian`. Current migrations are `20260907194101_InitialCreate` and
   `20260907220616_NormalizeBridegroomSectionTypes`. The `MySQL:Charset`
   annotations emitted by the current
   `MySql.EntityFrameworkCore` provider are correct; no further reconciliation
   needed unless the provider changes. Future schema changes must be additive
   migrations on top of this baseline. `Infrastructure/Persistence/RishtanataDbContextFactory.cs`
   still exists (reads `ConnectionStrings__DefaultConnection` from `.env`), so
   `dotnet ef` works from the repository root.

## Design / decision

3. **Login role provisioning is unimplemented.** Login authenticates against
   Tajneed (`GenerateToken`) and fetches the member (`GetMemberByChandaNoAsync`),
   but never resolves the member's roles. Tajneed `GetMemberRoleAsync` is defined
   but unused in the login flow, and `JamaatMemberService.CreateOrUpdateAsync`
   copies the gateway's `RoleId` (likely dangling against local `Role` IDs).
   Result: members sign in with no local `Role`, so the `ClaimTypes.Role` claim is
   empty and role-gated policies/pages don't match. Need: map Tajneed role strings
   → local `Domain.Entities.Role` and set it during login/sync. (Note: the login
   flow does now map the gateway member payload into a local `JamaatMember` and
   role strings are matched case-insensitively; the gap is resolving role strings
   to local roles.)

4. **Policy vs implementation divergence on claims.**
   `docs/stage-authorization-policy.md` §3.2 requires a `membership_no` claim and
   `member_roles` claims and says authorization must read *only* the
   `membership_no` claim. The implementation issues `ClaimTypes.NameIdentifier`
   (member Guid), `ClaimTypes.Name` (ChandaNo), `ClaimTypes.Role`, and
   `StageAuthorizationService` resolves the member by `JamaatMember.Id == userId`
   (Guid), not ChandaNo. Decide which is authoritative and align the other.

## Security

- **Closed:** CSRF, anonymous endpoints, swallowed Tajneed failures, and the
  hardcoded DB password are all fixed (see "Fixed (recent session)" above).

## Minor

7. **Witness matching loads the whole table.**
   `StageAuthorizationService.MatchesWitnessSlotAsync` fetches *all* `JamaatMembers`
   into memory and counts matches client-side.

8. **Design-time factory requires `.env`.** `Infrastructure/Persistence/RishtanataDbContextFactory.cs`
   reads `ConnectionStrings__DefaultConnection` from the environment / `.env`
   (no longer hardcodes `../Presentation`). `dotnet ef` must run where
   `DotNetEnv.TraversePath()` can find the `.env` file.

9. **Namespace/path mismatches.**
   - `IGatewayHandler` file is `Application/Interfaces/Gateway/IGatewayHandler.cs`
     with `namespace Application.Interfaces.Gateway` (now matches).
   - `IRoleService` file is `Application/Interfaces/IRoleService.cs` but declares
     `namespace Application.Roles`.
   - `IRishtanataSecretaryServices.cs` filename is plural; the interface is
     `IRishtanataSecretaryService` (singular).

10. **Dead code:** `ApplicationUser` / `ApplicationRole`
    (`Infrastructure/Identity/`) are now unreferenced after the Identity removal;
    only the stale snapshot references them. Can be deleted once migrations are
    reconciled (item 2). `JamaatMember` still carries leftover fields
    (`ResetToken`, `ResetTokenExpiry`, `IsSystemDefault`, `NewRole`).

11. **Stray committed file.** Root `cls` is an accidental `git branch -a` dump
    and `Presentation/Presentation.csproj.user` are committed but should be
    gitignored. (The old `API/API.csproj.user` reference — the `API/` folder no
    longer exists — and the `API/appsettings.json` hardcoded password are gone.)

12. **`AuditLog` does not inherit `AuditableEntity`**
    (`Domain/Entities/AuditLog.cs`) — it rolls its own `Id`/`Timestamp` and lacks
    `CreatedBy`/`ModifiedBy`.

13. **Nullable warnings** (not errors): `JamaatPresidentService.cs:97` (CS8602),
    `MarriageFormStageRevertedEventHandler.cs:96` (CS8629).
