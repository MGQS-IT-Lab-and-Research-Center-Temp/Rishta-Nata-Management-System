using System;

namespace Presentation.Requests;

public class GuardianOrWakeelSignatureRequest
{
    public string Signature { get; set; } = string.Empty;
}

public class WitnessSignatureRequest
{
    public Guid WitnessSignatureId { get; set; }
    public string Signature { get; set; } = string.Empty;
}