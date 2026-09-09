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
- **Returning-user login avoids a Tajneed member fetch.** `AuthService.LoginAsync`
  validates the Tajneed token on every login (the authoritative credential check),
  then, when a fresh (<=24h) local `JamaatMember` row exists, reuses it and only
  refreshes `Roles` from the token via `JamaatMemberService.UpdateRolesAsync`,
  skipping `GetMemberByMemberNoAsync`. First-time / stale profiles still fall back
  to the full fetch + upsert. If the local row is deleted mid-login, the fast path
  falls back to the slow path.
- **Tajneed transport failures no longer masquerade as bad credentials.**
  `AuthService.LoginAsync` catches `HttpRequestException`/`TaskCanceledException`/
  `TimeoutException` and reports "the member service is temporarily unreachable"
  instead of swallowing them as invalid login.
- **Tajneed resilience via official package.** The hand-rolled
  `Presentation/Services/RetryDelegatingHandler` was replaced by the official
  `Microsoft.Extensions.Http.Resilience` `AddStandardResilienceHandler()` (Polly v8:
  retry on transient/5xx + circuit breaker + attempt/total timeouts) on the
  `IGatewayHandler` HttpClient in `Presentation/Extensions/DependencyInjection.cs`.
  Non-retryable credential codes (400/401/404) are not retried.
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

3. **Login role provisioning — coherent (see note).** Roles are supplied by the
   Tajneed login response (`Data.roles`), persisted as the comma-separated
   `JamaatMember.Roles` string on the local row at every login (both the
   returning-user fast path via `UpdateRolesAsync` and the fresh path via
   upsert in `JamaatMemberService`), and issued as `ClaimTypes.Role` claims in
   the auth cookie. There is intentionally no local role table; role strings are
   matched case-insensitively against `Domain/Constants/RoleNames.cs`. Open
   limitation: role claims are fixed at `SignInAsync` time — a member whose Tajneed
   roles change will only get the updated roles on their next login (this also
   refreshes the DB `Roles` string). No `IClaimsTransformation` refresh exists.

4. **Policy vs implementation divergence on claims — RESOLVED.**
   `docs/stage-authorization-policy.md` §3.2 requires a `membership_no` claim and
   `member_roles` claims and says authorization must read *only* the
   `membership_no` claim. `CookieAuthenticationService` issues `membership_no`
   (ChandaNo, from the login response), `member_roles` (one per role) alongside
   `ClaimTypes.Role`, plus `ClaimTypes.NameIdentifier` (member Guid) and
   `ClaimTypes.Name` (ChandaNo). `StageAuthorizationService.ResolveMemberAsync`
   reads the ChandaNo and resolves by `JamaatMember.ChandaNo`, never by Guid.
   The policy is now authoritative and the implementation aligns.

## Security

- **Closed:** CSRF, anonymous endpoints, swallowed Tajneed failures, and the
  hardcoded DB password are all fixed (see "Fixed (recent session)" above).

## Minor

7. **Witness matching — improved; remaining match uses a phone-filtered set.**
   `StageAuthorizationService.MatchesWitnessSlotAsync` prefers the recorded
   `Witness{n}MembershipNo` exact ChandaNo match (policy §4.2 Kind A) when one is
   captured. Only when no ChandaNo is recorded does it fall back to the
   name+telephone match, and that fallback no longer loads the whole table:
   `CountAmbiguousWitnessMatchesAsync` filters `JamaatMembers` server-side by
   telephone first and counts name matches on the phone-filtered result.

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

11. **Stray committed file — CLEANED.** Root `cls` (an accidental `git branch -a`
    dump) and `Presentation/Presentation.csproj.user` were committed but should be
    gitignored; both are now untracked and covered by `.gitignore` (`/cls`,
    `*.csproj.user`). (The old `API/API.csproj.user` reference — the `API/` folder
    no longer exists — and the `API/appsettings.json` hardcoded password are gone.)

12. **`AuditLog` does not inherit `AuditableEntity`**
    (`Domain/Entities/AuditLog.cs`) — it rolls its own `Id`/`Timestamp` and lacks
    `CreatedBy`/`ModifiedBy`.

13. **Nullable warnings** (not errors): `JamaatPresidentService.cs:97` (CS8602),
    `MarriageFormStageRevertedEventHandler.cs:96` (CS8629).

14. **Additive migrations — applied to the local dev MySQL; live-deploy pending.**
    `20260908155154_AddDivorceEvidence` (4 × `varchar(500)`:
    `NikahApplications.BrideDivorceEvidence`/`BridegroomDivorceEvidence`,
    `NikahBrides.BrideDivorceEvidence`, `NikahGrooms.BridegroomDivorceEvidence`)
    and `20260909110147_MakeAuditableModifiedAtNullable` (makes the 17 auditable
    `ModifiedAt` columns nullable) are both applied to the local `rishtanatahdb`
    via `dotnet ef database update` (`AddDivorceEvidence` was already applied
    earlier). Any *other* MySQL (e.g. a live server) still needs both applied
    with the same command or equivalent SQL — do not hand-edit `InitialCreate`. 
    The new divorce-evidence fields will remain empty on an un-upgraded deploy.

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

17. **`MemberLookupService` stacks its own retry atop the resilience handler.**
    `MemberLookupService.LookupGatewayWithBackoffAsync` maintains its own 3-attempt
    loop with a 5s attempt timeout on top of the standard resilience handler's
    retries (worst case ~9 physical calls on a pathological lookup). Bounded and
    idempotent, so not a correctness bug — a candidate follow-up to simplify.
