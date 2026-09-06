# Member Lookup for Fathers & Witnesses — Design

Date: 2026-09-06
Status: Approved (pending implementation plan)

## 1. Goal

When filling in the bride's father, bridegroom's father, or a witness on the
Nikah form, a user can enter a membership number (ChandaNo) and the party's
name (and, for witnesses, address + telephone) is auto-populated — **without
the party logging in**. If the party is **not** a member, the same fields are
plain free-text. The membership number is persisted alongside the name.

This turns the §4.2 "Kind B" parties (guardian/wakeel/witnesses — recorded by
name/address/tel only) into parties whose ChandaNo is captured when they are
members, matching `docs/stage-authorization-policy.md` §4.2 "preferred" path.

## 2. Scope (decided)

- Parties covered: **bride's father, bridegroom's father, witness 1, witness 2.**
- Mechanism: **live client-side lookup** (JS fetch on membership-number entry).
- Membership number is **persisted** in new DB columns.
- Witness storage: **flat fields on `MarriageApplicationForm` only** (no change
  to `WitnessSignatureSection`).
- Witness side: **build a minimal witness entry form** (unblocks part of
  backlog D2), in addition to wiring the father lookup.

Out of scope (not in this change):

- Witness **signature** capture flow (`WitnessSignatureSection` /
  `SubmitWitnessSignatureAsync`) — the existing sign-by-ID service remains as is.
- Guardian/wakeel membership lookup (guardian stays free-text; father is the
  lookup target here). Extending the same pattern to guardian is trivial later.
- Authorization changes. The lookup endpoint is intentionally unauthenticated
  (data-only); the new witness entry form mirrors the existing `BrideGuardian`
  form's plain-MVC (no `[Authorize]`) posture.

## 3. Components

### 3.1 Member lookup endpoint (public, no login)

- `GET /api/members/lookup/{chandaNo}`
  - `200` body:
    ```json
    {
      "chandaNo": "…",
      "firstName": "…",
      "middleName": "…",
      "surname": "…",
      "fullName": "…",
      "phoneNo": "…",
      "address": "…",
      "jamaatName": "…"
    }
    ```
  - `404` when the membership number resolves to no member.
  - Trims/normalizes `chandaNo` (trim + case-insensitive compare), mirroring
    `MembershipNumbersMatch` in `StageAuthorizationService`.
- **No `[Authorize]`.** This is data-only pre-fill; policy §3.2 permits a
  ChandaNo in the request path to be used as data. It is never used for an
  authorization decision.
- Resolution strategy (decided): **Tajneed gateway first**
  (`IGatewayHandler.GetMemberByChandaNoAsync`), **fallback to the local
  `JamaatMembers` table**, else `404`. No writes happen on this endpoint.

### 3.2 New service

Add `IMemberLookupService` + `MemberLookupService` in `Application/Services`:

```csharp
Task<MemberLookupDto?> LookupAsync(string chandaNo, CancellationToken ct);
```

- Returns `null` → controller maps to `404`.
- `MemberLookupDto` lives in `Infrastructure/DTOs/Members/`.
- Injects `IGatewayHandler` and `RishtanataDbContext`.
- `fullName` = `BuildFullName(firstName, surname)` (same normalization as
  `StageAuthorizationService.BuildFullName`).

### 3.3 New controller

`Presentation/Controllers/MemberLookupController.cs` (or a `MembersController`),
`[ApiController]`, `[Route("api/members")]`, **no `[Authorize]`**.

### 3.4 Data model changes

On `Domain/Entities/MarriageApplicationForm.cs`, add four columns (string,
default `string.Empty`, matching existing style):

- `BrideFatherMembershipNo`
- `BridegroomFatherMembershipNo`
- `WitnessOneMembershipNo`
- `WitnessTwoMembershipNo`

EF config in `MarriageApplicationFormConfiguration.cs`: `.HasMaxLength(50)`
(matching `BrideMembershipNo`).

### 3.5 Migration

Add one migration for the four columns.

**Gotcha (per AGENTS.md):** there is no design-time factory and `Program.cs`
runs `DbInitializer.InitializeAsync` (a DB connection) at startup, which breaks
`dotnet ef migrations add`. To migrate cleanly:

- Add a minimal `IDesignTimeDbContextFactory<RishtanataDbContext>` under
  `Infrastructure` (restores the pattern the old removed
  `RishtanataDbContextFactory` provided), reading the connection string from
  the same `.env`/config the app uses, **or**
- Guard `Program.cs` so `DbInitializer`/migrations do not run during design-time
  host building.

Either way, verify against the live MySQL (`rishtanatahdb`) rather than
blindly generating — see `docs/bugs-and-gaps.md` #1/#2.

### 3.6 Father lookup (guardian form)

In `BrideGuardian/Create.cshtml` (and `Confirm.cshtml`), beside the currently
read-only `BrideFatherName` / `BridegroomFatherName`:

- Add a "membership number" text input per father.
- JS (a small shared `site.js` helper, e.g. `initMemberLookup(inputSelector,
  nameSelector, extraFields)`) that on input/blur fetches
  `/api/members/lookup/{chandaNo}`:
  - success → fills the name field (and, for witnesses, address + tel);
  - `404` → leaves the name field empty/editable so the user types free-text.
- The membership-number input is captured back into the model and persisted via
  `BrideGuardianController` (needs the father membership fields added to
  `BrideGuardianViewModel` and the controller mapping/update to
  `MarriageApplicationForm`).

### 3.7 Witness entry form (new)

A minimal MVC witness entry page (view + controller, mirroring
`BrideGuardian`):

- Fields for witness 1 and witness 2: membership number (with lookup), name,
  address, telephone.
- Writes to the flat `MarriageApplicationForm` fields
  (`WitnessOneName/Address/Tel/MembershipNo`, `WitnessTwo…`).
- Read-only display of the already-captured bride/groom/father context.
- Mirrors the existing `BrideGuardianController` auth posture (plain MVC form,
  no `[Authorize]`), since the person filling it is not the witness — the
  witness is identified by lookup or free-text, not by logging in.
- This change does **not** implement the witness **signature** submission
  (`SubmitWitnessSignatureAsync` / `WitnessSignatureSection`), which stays as
  the D2 backlog item.

## 4. Data flow

```
Form (father/witness membership number typed)
  └─ fetch /api/members/lookup/{chandaNo}
       └─ MemberLookupService
            ├─ IGatewayHandler.GetMemberByChandaNoAsync  (Tajneed)
            ├─ fallback: RishtanataDbContext.JamaatMembers (ChandaNo match)
            └─ null → 404 → UI enables free-text
  └─ on submit: controller persists membershipNo + resolved name
       (and address/tel for witnesses) into MarriageApplicationForm
```

## 5. Error handling

- Lookup `404` → UI falls back to free-text (not an error to the user).
- Gateway/network failure during lookup → treat as "not found" (free-text),
  log a warning; never block the form.
- No side effects from lookup: it never writes, never changes stage.

## 6. Testing / verification

- `dotnet build AMJNRishtanata.slnx` (only available verification; no test
  project).
- Manual: enter a valid member ChandaNo → name/address/tel auto-fill; enter an
  unknown number → free-text remains editable.
- Migration applies cleanly against live MySQL.

## 7. Open items

- Guardian/wakeel membership lookup (deferred; same pattern).
- Witness **signature** submission (deferred; remains backlog D2).
