using System;
using Domain.Abstractions;

namespace Domain.Entities;

public class Certificate : AuditableEntity
{
    // Required FK to MarriageApplication (one-to-one; unique index enforced in EF configuration).
    public Guid MarriageApplicationId { get; set; }
    public MarriageApplication MarriageApplication { get; set; } = null!;

    // A certificate row only exists once issued, so this is never nullable.
    public DateTime IssueDate { get; set; }

    // Who issued it. FK to ApplicationUser (Identity), Restrict on delete so the record
    // survives account removal — configured in CertificateConfiguration.
    //this will be implemented after identity is done
    // public Guid IssuedByUserId { get; set; }

    // Path only, consistent with Document. Nullable because whether a file is produced
    // at all is still pending clarification (#34).
    public string? CertificateFilePath { get; set; }
    public  object? IssuedByUserId { get; set; }

    // Deliberately NOT declared here: Nikah serial number.
    // It belongs to MarriageApplication; read it through the relationship once wired up.
}
