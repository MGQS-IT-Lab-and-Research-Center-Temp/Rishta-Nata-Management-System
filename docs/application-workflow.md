# Application Workflow

How the Rishta-Nata system works end to end, technically. This is a companion to
`docs/stage-authorization-policy.md` (the authoritative authorization rules) —
read that for *who may act*, read this for *how the data moves*.

---

## 1. Authentication & session

1. The user submits a **ChandaNo + password** at `Auth/Login`.
2. `AuthService.LoginAsync` calls the external Tajneed API
   (`GatewayHandler.GenerateToken`) and, on success, fetches the member record
   (`GetMemberByChandaNoAsync`) and upserts it into the local `JamaatMembers`
   table via `JamaatMemberService.CreateOrUpdateAsync`. The external roles are
   stored as a single comma-separated string on `JamaatMember.Roles`.
3. `CookieAuthenticationService.SignInAsync` issues a cookie with these claims:

   | Claim | Value |
   |---|---|
   | `ClaimTypes.NameIdentifier` | local `JamaatMember.Id` |
   | `ClaimTypes.Name` | ChandaNo |
   | `membership_no` | ChandaNo (the only claim authorization reads) |
   | `FullName` | `FirstName MiddleName Surname` |
   | one `ClaimTypes.Role` per role | each role string |

4. `DashboardRedirector.Resolve(member)` lands the user on the dashboard for
   their role (see §2).

> Real API/network failures are currently swallowed by `AuthController.Login`'s
> catch-all and reported as "Invalid Chanda number or password" — see
> `docs/bugs-and-gaps.md`.

## 2. Role-based landing

`Presentation/Services/DashboardRedirector.cs` maps roles (case-insensitive) to
dashboards, in priority order:

| Role | Dashboard |
|---|---|
| Rishtanata Secretary | `RishtanataSecretary/Dashboard` |
| Naib Rishtanata Secretary / Gen. Sec. | `AssistantRishtanataSecretary/Dashboard` |
| Amir | `Amir/Dashboard` |
| Missionary In-charge | `MissionaryInCharge/Dashboard` |
| Circuit President | `CircuitPresident/Dashboard` |
| Jamaat President | `JamaatPresident/Dashboard` |
| anything else (regular member) | `JamaatMemberDashboard/Index` |

Role strings live in `Domain/Constants/RoleNames.cs` (provisional — confirm
against the live API).

## 3. Starting an application (either party may start)

The member clicks **Apply for Nikah** (`MarriageApplication/Create`). If the
member already has a non-terminal application, this redirects them to continue
that application instead (`MarriageApplication/Continue/{id}`).

1. The controller reads the logged-in member's `Sex` from their local
   `JamaatMember` record (via `IMemberDashboardService.GetProfileAsync`) and
   pre-selects whether they are the **groom** or the **bride**. A toggle lets
   them override an incorrect auto-detection.
2. Their own **name, date of birth, address, and phone** are pre-filled from the
   member record (editable).
3. They enter the **partner's membership number**, which auto-fills the
   partner's name/DOB/address/phone from `GET /api/members/lookup/{chandaNo}`.
4. On submit, `IMarriageApplicationFormService.StartApplicationAsync` creates the
   owning `FormApplication` first, then the `MarriageApplicationForm`, recording:

   - both parties' membership numbers (`BrideMembershipNo`,
     `BridegroomMembershipNo`),
   - the starter's full section,
   - `ApplicationStage = ApplicantsReview`,
   - `FormStage = AwaitingBride` (groom started) **or** `AwaitingBridegroom`
     (bride started).

The **intending partner connects by membership number alone**: their dashboard
(`MemberDashboardService`) lists every `MarriageApplicationForm` where
`BrideMembershipNo == no` or `BridegroomMembershipNo == no`. No explicit "join"
step exists.

## 4. The section workflow (MarriageFormStage)

The micro stage moves through the paper-form order:

```
AwaitingApplicants ──┬─ (bride first) ──→ AwaitingBride ──→ AwaitingWitnesses
                     └─ (groom first) ──→ AwaitingBridegroom ─┘
AwaitingWitnesses → AwaitingImamVerification → AwaitingJamaatPresident
                 → AwaitingRishtanataSecretary → AwaitingAmirApproval → Completed
```

- `AwaitingApplicants` is the neutral start state (added to remove the old
  bride-first hardwiring). `BrideSectionService` and `BridegroomSectionService`
  each accept being first **or** second and advance to the correct next stage.
- `Completed` locks the form against further edits.

Section submission goes through the API surface in
`Presentation/Controllers/MarriageApplicationFormController.cs`:

| Endpoint | Stage | Service |
|---|---|---|
| `PUT .../bride` | `AwaitingApplicants` or `AwaitingBride` | `BrideSectionService` |
| `PUT .../bridegroom` | `AwaitingApplicants` or `AwaitingBridegroom` | `BridegroomSectionService` |
| `PUT .../guardian-or-wakeel` | (blocked, backlog D2) | — |
| `PUT .../witnesses` | (blocked, backlog D2) | — |
| `PUT .../imam-verification` | `AwaitingImamVerification` | `MarriageFormWorkflowService` |
| `PUT .../jamaat-president` | `AwaitingJamaatPresident` | `MarriageFormWorkflowService` |
| `PUT .../rishtanata-recommendation` | `AwaitingRishtanataSecretary` | `MarriageFormWorkflowService` |
| `PUT .../amir-approval` | `AwaitingAmirApproval` | `MarriageFormWorkflowService` |

## 5. Authorization (single source of truth)

`IStageAuthorizationService` (`Application/Services/StageAuthorizationService.cs`)
is the **only** place that decides "can user X act on section Y now?" — never
re-implemented in a controller or view.

It has two overloads:

- `CanUserActAsync(membershipNo, formId, ApplicationStage, ...)` — the macro
  stage, used by the applicant intake (`ApplicantsReview` matches either the
  bride or the groom's membership number).
- `CanUserActAsync(membershipNo, formId, MarriageFormStage, ...)` — the micro
  stage, used by the verification chain.

Every rule is **role gate AND stage gate**, evaluated atomically. Denies produce
no side effects. See `docs/stage-authorization-policy.md` §2–§6 for the full
matrix.

## 6. Non-login parties (fathers, guardian, witnesses)

Guardian/witness details are collected **anonymously** via revocable share
links, stage-gated to `AwaitingWitnesses`:

- `SectionLinks/Index/{applicationId}` (authenticated, party-only) — the
  bride/groom mints, revokes and regenerates links per section (Guardian /
  Witness 1 / Witness 2). The raw token is stored (alongside its SHA-256 hash,
  which stays the validation path) so the active link can be re-displayed on
  the page until revoked.
- `SharedSection/Fill/{token}` (fully anonymous) — validates the token, then
  upserts the guardian/witness section row + the flat mirror columns. Once all
  three sections are recorded the form auto-advances `AwaitingWitnesses` →
  `AwaitingImamVerification`.
- The first form field asks whether the signer is a Jama'at member. If yes, the
  membership number is entered and name/address/phone auto-load from the
  token-gated `SharedSection/MemberLookup` endpoint (→ Tajneed gateway with
  local-cache fallback); fields stay editable. The server stamps the signature
  date at submit.

Legacy authenticated flows (`BrideGuardian/Create/{marriageApplicationId}`,
`Witness/Create/{marriageApplicationId}`) remain wired but are now superseded.

## 7. Verification & approval chain

`MarriageFormWorkflowService` implements the office-holder chain. Each method
re-checks authorization immediately before writing, upserts its section row
(attributing `CreatedBy`/`CreatedAt`), and advances `FormStage`:

1. **Imam** → `ImamVerificationSection` → advances to `AwaitingJamaatPresident`.
2. **Jamaat President** → `JamaatPresidentVerificationSection` → advances to
   `AwaitingRishtanataSecretary`.
3. **National Rishtanata Secretary** → `RishtanataRecommendationSection` →
   advances to `AwaitingAmirApproval`.
4. **Amir** → `AmirApprovalSection`, sets `ApprovedDateOfNikah`, advances to
   `Completed` and locks the form.

The Jamaat President and Secretary also have MVC review views
(`JamaatPresident/Review`, `RishtanataSecretary/Review`) that approve, reject,
or request more information via `FormApplicationService`.

## 8. Rejection & revert

`MarriageApplicationFormService.RevertStageAsync` allows a verifier authorized
for the current stage to revert to an earlier `ApplicationStage`, recording a
`MarriageFormRejection` and clearing forward section rows
(`ClearSectionsAfterAsync`). A domain event triggers
`MarriageFormStageRevertedEventHandler` to notify the original submitters.

## 9. Data model (summary)

- `FormApplication` — the application wrapper: status + 1:1 with
  `MarriageApplicationForm` and 1:1 with `Certificate` (Certificate is the
  dependent; issued after the application).
- `MarriageApplicationForm` — the flat form: both parties' details, witnesses,
  guardian, verification names, `FormStage`, `ApplicationStage`, `ReferenceNumber`.
- `Certificate` — issued certificate, related through `FormApplication`
  (NOT directly off `MarriageApplicationForm`; the direct navigation is
  `Ignore`d in the EF config).
- `JamaatMember` — local cache of the external member record; roles as a
  comma-separated string.
- Section rows — `BrideFormSection`, `BridegroomFormSection`,
  `GuardianOrWakeelSection`, `ImamVerificationSection`,
  `JamaatPresidentVerificationSection`, `RishtanataRecommendationSection`,
  `AmirApprovalSection`, `WitnessSignatureSection`.
- `MarriageFormRejection`, `Invitation`, `Review`, `AuditLog`.

## 10. Known quirks

- **Two parallel stage enums** (`ApplicationStage` vs `MarriageFormStage`) drive
  two `CanUserActAsync` overloads. Unifying them is an open product decision —
  see `docs/stage-authorization-policy.md` §8.
- The **guardian/witness submission** is live via anonymous share links
  (`SectionLinks` + `SharedSection`, §6). The legacy `WitnessController` /
  `BrideGuardianController` MVC flows are superseded (flagged for removal).
- The **Certificate↔FormApplication** relationship is configured in
  `RishtanataDbContext.OnModelCreating`; `MarriageApplicationForm.Certificate`
  is deliberately ignored.
