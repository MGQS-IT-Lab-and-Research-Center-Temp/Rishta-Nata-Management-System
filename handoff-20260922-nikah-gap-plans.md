# Handoff: Nikah form gap-analysis → implementation plans

**Repo:** `C:\Users\user\source\repos\Rishta-Nata-Management-System` (branch `dev`, git user `absyinka`)
**Date written:** 2026-09-22

## Where things stand

1. **Gap analysis (source material — NOT saved to disk).** Several context-windows
   ago, the user asked for a comparison of the physical Nikah form (screenshots
   `F1.jpeg`/`F2.jpeg` at repo root) against the current domain models, producing a
   **9-gap analysis** presented only in chat. It was never written to a file.
   **Gap 1** = "Bridegroom's own Wakeel (F1 §VII) — not modeled at all": the paper
   form lets the bridegroom appoint a representative/Wakeel to execute the Nikah
   on his behalf when he can't attend in person; nothing in the domain modeled
   this. Gaps 2-9 titles/details are NOT currently known to this session — see
   "Blocked / needs re-derivation" below.

2. **Gap 1 has a complete, approved implementation plan already written:**
   `docs/superpowers/plans/2026-09-20-groom-wakeel-appointment.md`
   (~900 lines, 9 tasks, written per the `superpowers:writing-plans` skill —
   verbatim before/after code for every file, no placeholders, inline
   Self-Review section already included). This followed a `superpowers:brainstorming`
   session where the user approved the design in chat ("looks right, go ahead
   with the plan"). **Do not re-plan Gap 1** — just read that file if you need its
   content.

3. **Execution of the Gap 1 plan has NOT started.** The mandatory
   "Execution Handoff" question (per `writing-plans`) was asked:
   > 1. Subagent-Driven (recommended) vs 2. Inline Execution
   The user did not answer it yet — they instead asked to also draft plans for
   the remaining 8 gaps before deciding on execution. **No source files have
   been touched for Gap 1 implementation.**

4. **User's new request:** draft plans for the other 8 gaps. They initially asked
   for "one file" but after I flagged that the `writing-plans` skill recommends
   one plan file per independent subsystem (each plan should be independently
   testable/reviewable), the user chose via AskUserQuestion:
   **"Separate plan files per gap (recommended)"** — i.e. one
   `docs/superpowers/plans/YYYY-MM-DD-<gap-name>.md` per gap, same as Gap 1's.

## Blocked / needs re-derivation

The original 9-gap analysis text (gaps 2-9) is **not in this session's live
context** (it predates a context compaction) and is **not saved anywhere on
disk**. A fork/subagent was dispatched to grep it out of the raw transcript
(`C:\Users\user\.claude\projects\C--Users-user-source-repos-Rishta-Nata-Management-System\885f4b0c-9260-43f7-8a99-178d4f346c2c.jsonl`)
but **failed with a rate-limit error** (`HTTP 429`, session limit hit,
"resets 4:30pm Africa/Lagos") before finishing. Its last partial progress note
was: "Now the EF configuration, controller, and mapper files" — meaning it may
have drifted into re-reading current source instead of the historical chat
transcript; **do not trust any partial output from that failed run**.

**Next step must be one of:**
- Re-run the same transcript-grep as a fresh fork/subagent once the rate limit
  resets, pointed at the same `.jsonl` path, searching for `"Gap 1"`/`"Gap 2"`
  markers etc. near the start of the file (search *before* assuming it's near
  the end — the gap analysis was several compactions back).
- OR simply re-run the gap analysis from scratch against `F1.jpeg`/`F2.jpeg`
  (still at repo root) and the current models — likely faster/more reliable
  than transcript archaeology, and guarantees the analysis reflects the
  **current** codebase (Gap 1's own plan already changed several files that
  the original analysis was written against, e.g. `SectionType`,
  `BridegroomFormSection` — though that plan is not yet *executed*, so the
  live code hasn't actually changed yet).
- Ask the user directly if they still have the original gap list (e.g. pasted
  it somewhere else, or can restate it).

## Suggested skills for the next session

- **`superpowers:brainstorming`** — if regenerating the gap analysis from
  scratch, or if any of gaps 2-9 turn out to need a design discussion before
  planning (the way Gap 1 did). Each gap should be classified (bounded vs
  architectural) independently — don't assume they're all one size.
- **`superpowers:writing-plans`** — for turning each approved gap design into
  its own `docs/superpowers/plans/YYYY-MM-DD-<gap-name>.md`, following the
  same "no placeholders / verbatim code" discipline as Gap 1's plan. Use
  `docs/superpowers/plans/2026-09-20-groom-wakeel-appointment.md` as the
  structural template.
- **`superpowers:subagent-driven-development`** or **`superpowers:executing-plans`**
  — once the user finally answers the Execution Handoff question for Gap 1 (and
  later for each new gap's plan). Do not start implementation before that
  explicit choice is made.

## Constraints / conventions to carry forward

- Plans live in `docs/superpowers/plans/`, specs/designs in
  `docs/superpowers/specs/` (see existing files there for naming convention:
  `YYYY-MM-DD-<topic>.md` / `YYYY-MM-DD-<topic>-design.md`).
- This repo has **no test project** — verification per plan task is
  `dotnet build AMJNRishtanata.slnx` + manual/functional check + commit, not
  automated tests.
- EF migrations: startup project `Presentation`, DbContext project
  `Infrastructure`. Command shape:
  `dotnet ef migrations add <Name> --project Infrastructure --startup-project Presentation`.
- Dual-storage pattern is pervasive: every section entity's fields are mirrored
  as flat properties on `MarriageApplicationForm` (dates stored as
  `string` `"yyyy-MM-dd"` on the flat mirror, real `DateTime?` on the entity).
- Token-based anonymous "shared link" submission pattern
  (`SectionAccessToken`/`SharedSectionService`/`SharedSectionController`/
  `SectionLinksController`/`RishtanataSecretaryController`) is generic over
  `Domain.Enums.SectionType` — adding a new `SectionType` value generally does
  NOT require touching `SectionLinksController.cs` or the token
  generate/regenerate/revoke methods, only the 4 label-switch sites + the
  submission-upsert/completeness logic in `SharedSectionService.cs`. This
  pattern is very likely reusable for several of gaps 2-9 if they also involve
  a new anonymously-fillable party/section — check each gap against it before
  designing from scratch.
- User's git identity: `absyinka` / `absyinka@gmail.com`. Commit messages in
  this repo follow `type: short description` (e.g. `fix: wire up X-CSRF-TOKEN
  antiforgery header`) — see `git log` for more examples.
- Untracked at last snapshot: `F1.jpeg`, `F2.jpeg` (the physical-form
  screenshots, at repo root — needed again if regenerating the gap analysis),
  and a stale `docs/handoff-20260919-174326.md` from an earlier handoff (unrelated
  prior session — not a dependency of this one, but worth a glance if context
  on *that* session's task is ever needed).

## Explicit non-goals for the next session

- Do not touch any source file for Gap 1 implementation until the user
  answers the Subagent-Driven vs Inline Execution question.
- Do not fabricate gaps 2-9 from guesswork — either recover the real text or
  regenerate the analysis properly from the screenshots + current code.
- Do not write the 8 remaining gaps into a single combined plan file — the
  user explicitly chose one file per gap.
