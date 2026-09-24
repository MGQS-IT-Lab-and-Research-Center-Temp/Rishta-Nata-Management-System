
namespace Domain.Enums;

/// <summary>
/// The four witness pairs on the paper Nikah form. GuardianAgreement and
/// NikahCeremony keep their original stored ints; only append new values.
/// </summary>
public enum WitnessContext
{
    /// <summary>F2 §III: the guardian filled the form and the bride agreed.</summary>
    GuardianAgreement = 0,

    /// <summary>F1 §IX: the Nikah ceremony itself (signed after the ceremony).</summary>
    NikahCeremony = 1,

    /// <summary>F2 §II: the guardian's appointment of a representative (Wakeel).</summary>
    WakeelAppointment = 2,

    /// <summary>F1: the bridegroom's own declaration.</summary>
    GroomDeclaration = 3
}
