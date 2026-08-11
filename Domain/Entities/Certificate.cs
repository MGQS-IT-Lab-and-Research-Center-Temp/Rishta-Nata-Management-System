using System;
using Domain.Abstractions;

namespace Domain.Entities;

public class Certificate : AuditableEntity
{
    // Required FK to MarriageApplication (one-to-one; unique index enforced in EF configuration).
    // TODO: Add the MarriageApplication navigation property once that entity exists in Domain/Entities.
    // public MarriageApplication MarriageApplication { get; set; } = null!;
    public Guid MarriageApplicationId { get; set; }

    // A certificate row only exists once issued, so this is never nullable.
    public DateTime IssueDate { get; set; }

    // Who issued it. FK to ApplicationUser (Identity), Restrict on delete so the record
    // survives account removal — configured in CertificateConfiguration.
    // TODO: Add the IssuedByUser navigation property if/when needed for querying convenience.
    public string IssuedByUserId { get; set; } = null!;

    // Path only, consistent with Document. Nullable because whether a file is produced
    // at all is still pending clarification (#34).
    public string? CertificateFilePath { get; set; }

    // Deliberately NOT declared here: Nikah serial number.
    // It belongs to MarriageApplication; read it through the relationship once wired up.
}