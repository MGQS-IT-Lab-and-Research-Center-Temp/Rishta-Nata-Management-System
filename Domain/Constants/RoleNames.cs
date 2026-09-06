namespace Domain.Constants;

/// <summary>
/// Role names as reported by the external Tajneed API (login response
/// Data.Roles). These are the only roles the system recognizes — there is no
/// local role table anymore. Matching against them is case-insensitive because
/// the API's casing is not guaranteed (see e.g. StageAuthorizationService).
/// </summary>
public static class RoleNames
{
    public const string Amir = "Amir";
    public const string MissionaryInCharge = "Missionary In Charge";
    public const string RishtanataSecretary = "Rishta Nata Secretary";
    public const string NaibRishtanataSecretary = "Naib Rishta Nata Secretary";
    public const string GenSecRistanataDept = "Gen Sec Rishata Nata Dept";
    public const string CircuitPresident = "Circuit President";
    public const string JamaatPresident = "Jamaat President";   
    public const string Member = "Member";

}
