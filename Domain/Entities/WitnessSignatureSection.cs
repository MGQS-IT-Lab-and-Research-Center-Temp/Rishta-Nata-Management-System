using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;

public class WitnessSignatureSection : AuditableEntity
{

    public string Name { get; set; }

    public string Address { get; set; }

    public string Tel { get; set; }

    public DateTime SignatureDate { get; set; }
    public string? Signature { get; set; }


    public WitnessContext WitnessContext { get; set; }

    public int WitnessNumber { get; set; }
    public Guid MarriageApplicationFormId { get; set; }

    public MarriageApplicationForm MarriageApplicationForm { get; set; } = null!;
}
