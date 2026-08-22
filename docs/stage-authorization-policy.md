# Stage-Authorization Policy

---

## 1. Purpose

The Nikah form is filled in stages, by different people, in a fixed order. This
document defines the **single authoritative answer** to the question:

> *"Can this logged-in user act on this section of this application, right now?"*

Every submission endpoint, every service method, and the UI's
`CanCurrentUserEdit` flag (Epic C3) must derive their decision from this policy —
the UI and the API must never disagree.

---

## 2. The golden rule (read this first)

Authorization to act on a form section requires **both** of the following:

1. **Role gate** — the user is the person (or office-holder) responsible for the
   section being submitted, *as recorded on that specific application*; **and**
2. **Stage gate** — the form's `CurrentStage` is exactly the stage that role is
   responsible for.

**Role-matching alone is never sufficient.** Being the bride does not let you
edit the groom's section; being the Rishtanata Secretary does not let you act
while the form is still sitting at `AwaitingWitnesses`. Conversely, being at the
right stage means nothing if you are not the right person. Both gates must pass,
and they must be evaluated together, atomically, in one place
(`IStageAuthorizationService`, Ticket B2).

Formally:

```
CanUserActAsync(user, applicationFormId, targetStage)
    = ResolveMembershipNo(user)            succeeds          (§3)
      AND MatchRoleToApplication(...)      succeeds          (§4)
      AND form.CurrentStage == targetStage                   (§5)
```

If any condition fails, the result is **deny**, with a machine-readable reason
(§6). Denied requests produce **no side effects** — no writes, no stage changes,
no notifications.

---

## 3. Gate 1 — Resolving the user's MembershipNo from their identity

### 3.1 What "MembershipNo" means here

In this system the membership number is the member's **ChandaNo**
(`JamaatMember.ChandaNo`; the paper form prints it as "Membership No"). All
matching below is done on ChandaNo values, compared as trimmed strings
(case-insensitive), because the external member API and the paper form do not
guarantee zero-padding.

### 3.2 Where it comes from

Users authenticate with **ChandaNo + password** against the central member API
(`TokenRequest(ChandaNo, Password)` → `GatewayHandler.GenerateToken`). The
member API's login response carries `Data.UserName`, which is the ChandaNo the
user logged in with, plus `Data.Roles`.

**Policy requirement:** once a session/JWT is established locally, the
authenticated principal **MUST** carry the member's ChandaNo as a claim:

| Claim | Value | Source |
|---|---|---|
| `membership_no` | the member's ChandaNo | `Data.UserName` from the member-API login response |
| `member_roles` | one claim instance per role string | `Data.Roles` from the member-API login response |

Rules:

- Authorization code reads the identity **only** from the `membership_no`
  claim. A ChandaNo supplied in a request body, query string, or header is
  **never** trusted for authorization decisions — it may only be used as
 *data* (e.g. pre-filling a field).
- If the `membership_no` claim is absent, empty, or not resolvable to an
  existing member → **deny** with reason `NoMembershipClaim`. Never fall back
  to username/email guessing.
- Roles are read from `member_roles` claims issued at login time. Long-lived
  sessions must not outlive meaningful role changes; see §7.4.

---

## 4. Gate 2 — Matching the membership to a role on the application

There are three kinds of actors on the form, and each kind has its own matching
rule.

### 4.1 Kind A — Parties named on the form by membership number

These roles are matched by direct comparison against fields on the
application/form:

| Stage | Role | Match rule |
|---|---|---|
| `AwaitingBride` | Bride | `membership_no == form.BrideMembershipNo` |
| `AwaitingBridegroom` | Bridegroom | `membership_no == form.BridegroomMembershipNo` |

Exact equality, after trimming/case-normalization. No wildcarding, no
"close enough" name matching for these two stages.

### 4.2 Kind B — Parties named on the form without a printed membership number

The paper form records the Guardian/Wakeel and both Witnesses by **name,
address, and telephone only**. To keep authorization attributable, the digital
form must establish who these people are. Policy:

- **Preferred:** when the party is a Jamaat member, their ChandaNo is captured
  on the section at submission time (stored alongside the paper-form fields),
  and matching uses `membership_no == section.MembershipNo` exactly like Kind A.
- **Fallback (only when no ChandaNo exists):** the section stores the
  normalized full name + telephone entered at submission; a later edit is
  allowed only for a principal whose member record matches on **both**
  normalized full name and telephone. If more than one member record matches,
  treat as unresolved → **deny** with reason `AmbiguousIdentityMatch` rather
  than guessing.
- This fallback is deliberately strict. If product decides witnesses must
  always be members, drop the fallback entirely (see §8, open question Q1).

### 4.3 Kind C — Office-holders (verifiers)

The four verification/approval stages are held by **offices**, not by people
named on the form. The correct verifier is whoever currently holds the office —
resolved via the member's roles and, where applicable, their Jamaat/circuit
assignment (`Data.JamaatName` / `JamaatMember.JamaatName`):

| Stage | Office | Match rule |
|---|---|---|
| `AwaitingImamVerification` | Officiating Imam / Missionary | principal holds the Imam-or-Missionary role **and** (v1) performed/is performing the ceremony for this application. Attribution is via `CreatedBy` on the `ImamVerification` row. |
| `AwaitingJamaatPresident` | Jamaat (branch) President | principal holds the President role **and** `principal.JamaatName == application's Jamaat` (the branch whose members are marrying). |
| `AwaitingRishtanataSecretary` | National Rishtanata Secretary | principal holds the national Rishtanata Secretary role. |
| `AwaitingAmirApproval` | National Amir  | principal holds the Amir  role. |

Notes:

- Role strings come from the member API (`GetMemberRoleAsync`); the canonical
  mapping from those strings to the offices above lives in **one** place (the
  authorization service), not scattered across controllers.
- Any office-holder satisfying the rule may act; the specific individual who
  acted is permanently attributable through `AuditableEntity.CreatedBy /
  CreatedAt` on the section row. We authorize the *office*, we audit the
  *person*.
- Scoping the Imam and Jamaat-President checks to the application's Jamaat is
  v1 behavior; tightening to circuit/national level later must not change the
  shape of the rule, only the comparison inside it.

### 4.4 Who may reject/revert (Epic D4, F3)

Any user authorized under §4.3 for the stage the form is **currently sitting
at** may submit a rejection. A verifier may never reject a form that has not
yet reached their stage.

---

## 5. Gate 3 — The stage gate

- The form's `CurrentStage` (see `MarriageFormStage`, Ticket A1) must equal the
  `targetStage` the caller claims responsibility for. Comparison is a single
  enum equality check — no ranges, no "stage >= X" logic.
- Consequence: each stage has exactly **one** responsible role at a time, and
  skipping stages is impossible. Example: an Imam attempting to verify while
  the form is at `AwaitingWitnesses` fails the stage gate even though his role
  would match.
- When the final approval (`ApproveByAmirAsync`) advances the stage to
  `Completed`, the form is locked: every subsequent `CanUserActAsync` call
  denies with reason `FormCompleted`. Read access is unaffected.
- Services must **re-check** the stage immediately before writing (inside the
  same transaction/unit-of-work), even though the controller already called the
  authorization service (Ticket D1's AC). Controllers are a convenience check;
  the service-level check is the real gate. No code outside the designated
  service methods may write `CurrentStage`.

---

## 6. Deny results

`CanUserActAsync` returns an allow/deny result carrying one of these reasons
(for logging and debugging — Ticket B2's AC):

| Reason | Meaning |
|---|---|
| `NoMembershipClaim` | Principal has no usable `membership_no` claim. |
| `UnknownMember` | Claim resolves to no known member. |
| `WrongRole` | Member exists but is not the party/office-holder for `targetStage` on this application. |
| `AmbiguousIdentityMatch` | Kind-B fallback matched multiple members (§4.2). |
| `WrongStage` | Role matches, but `form.CurrentStage != targetStage`. |
| `FormNotFound` | No such application/form. |
| `FormCompleted` | Form reached `Completed`; no further edits. |

HTTP semantics (for Ticket B3): denies map to **403 Forbidden** (except
`FormNotFound` → 404, to avoid leaking existence), with **no side effects**.

---

## 7. Edge cases the implementation must honor

7.1 **One writer per section.** A section row is created once by its rightful
actor; subsequent corrections go through the rejection/revert flow (D4), not
by re-submitting over someone else's row.

7.2 **Revert clears forward sections.** After `RevertStageAsync(target)`,
sections after `target` are nulled (D4). When the form climbs back to a
verifier's stage, that verifier acts on fresh rows; stale data must never be
resubmitted implicitly.

7.3 **UI/API parity.** `MarriageApplicationFormDetailDto.CanCurrentUserEdit`
(C3) calls the same `IStageAuthorizationService` logic — never a re-implementation.

7.4 **Stale roles.** Role claims are minted at login. If office-holders change
mid-workflow, the next denial surfaces it; operators re-authenticate. (If this
proves painful, add a role-refresh hook later — out of scope for v1.)

7.5 **Auditability.** Every deny decision should be loggable with
`userId, applicationFormId, targetStage, reason` so support can diagnose
"why can't I submit?" complaints without guesswork.

---

## 8. Open questions (do not silently decide in code)

- **Q1 — Must witnesses/guardian/wakeel be Jamaat members?** If yes, capture
  ChandaNo always and delete the §4.2 fallback. Owner: product owner. Blocks:
  D2, F2 witness endpoints.
- **Q2 — Is `AwaitingWitnesses` one stage or two?** Tracked separately as
  Ticket F1; this policy is agnostic — whichever way it splits, each resulting
  stage gets its own row in the §4 tables.
- **Q3 — Exact role strings** returned by the member API for the four offices
  need to be confirmed and recorded in the authorization service's mapping
  table before B2 ships.

---

## 9. Where this policy is implemented

| Ticket | Implements |
|---|---|
| B2 | `IStageAuthorizationService` — the only place this policy lives in code |
| B3 | Controller wiring: every Epic-D endpoint calls the service first |
| C3 | `CanCurrentUserEdit` via the same service |
| D1–D4 | Service-layer re-checks before writes; stage advancement rules |
| F2/F3 | Endpoint surface gated by the above |