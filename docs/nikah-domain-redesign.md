# Nikah Domain Redesign

## Scope

Version one models the official Nikah paper form directly. It is **not** a
generic form-template engine. Future ceremony types, such as Aqeeqah, can
reuse cross-cutting services (identity, storage, email and audit logging) but
must have their own typed aggregates when their requirements are agreed.

## Aggregate

`NikahApplication` is the aggregate root. It owns:

- `BrideDetails` and `BridegroomDetails` — verified member identity plus the
  form-specific details each party attests to.
- `GuardianRepresentation` — a guardian attends or a Wakeel is appointed;
  the Wakeel fields are required only in the latter case.
- Four `NikahWitnessAttestation` records: two bride-confirmation witnesses and
  two Nikah-ceremony witnesses. Witnesses use one-time links and do not need
  accounts.
- `SupportingDocument` records — Talaq/divorce/Khula evidence is required when
  a relevant prior-marriage answer requires it.
- Immutable `NikahWorkflowDecision` history and field-specific
  `NikahCorrectionRequest` records.
- Immutable `NikahCertificate` snapshots. A correction produces a linked
  replacement certificate; no issued record or serial number is silently
  changed.

## Workflow

```text
Draft / contributor completion
  -> Jama'at President (bride's Jama'at)
  -> National Rishta Nata Reviewer
  -> Amir
  -> approved -> certificate issued
```

At every reviewer stage the only decisions are:

1. **Approve** — advances to the next stage.
2. **Request correction** — includes a mandatory comment and selected stable
   field keys. The applicant edits only those fields and resubmits directly to
   the reviewer who requested it.

There is no terminal rejection, appeal or override workflow in version one.

## Certificate number

After Amir approval the system assigns a globally sequential, never-reused
number using `AMJN-YYMMDD-####`, where the date is the application submission
date and the sequence begins at `5001`. The database must enforce uniqueness;
number allocation must occur in a transaction before certificate generation.

## Access and retention

- Bride, groom, guardian and Wakeel authenticate as verified Jama'at members.
- Only witnesses are unauthenticated, via expiring one-time links.
- Supporting documents are visible only to involved parties and authorized
  reviewers/administrators.
- Notifications are email-only in version one.
- System Admin and Secretary may view audit data.
- Applications, documents, certificates and audit history are never
  hard-deleted; administrative archive is the only removal action.

## Migration strategy

The repository currently has a heavily coupled legacy model built around
`FormApplication`, `MarriageApplicationForm`, duplicated section data and two
different Amir-approval models. The new `Nikah*` tables are intentionally
introduced alongside those legacy tables so the schema can be migrated without
destroying existing applications.

1. Apply a migration that creates the `Nikah*` tables only; do not alter or
   drop legacy tables.
2. Build new application services/endpoints against `NikahApplication`.
3. Backfill legacy applications only after mapping rules are tested on a copy
   of production data.
4. Switch reads/writes to the new aggregate behind a deliberate release flag.
5. Retire legacy tables only after the retained-record and certificate
   requirements have been met.
