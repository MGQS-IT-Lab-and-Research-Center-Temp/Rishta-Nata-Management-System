namespace Domain.Enums;

/// <summary>
/// Whose divorce certificate a DivorceEvidenceDocument is (Gap 8): the
/// bride's Khula or the bridegroom's Talaq. Separate from PartyType, which is
/// Guardian/Wakeel.
/// </summary>
public enum DivorceEvidenceParty
{
    Bride = 1,
    Bridegroom = 2
}
