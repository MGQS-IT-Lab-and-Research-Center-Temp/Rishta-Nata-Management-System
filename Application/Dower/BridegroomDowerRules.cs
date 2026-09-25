namespace Application.Dower;

/// <summary>
/// The groom's dower line on the paper Nikah form (Gap 9): the amount paid in
/// cash plus the amount still to be paid must equal the total dower, and no
/// amount may be negative. The only implementation of this rule; the section
/// service and the Create action both call it.
/// </summary>
public static class BridegroomDowerRules
{
    public const string NegativeMessage =
        "Dower amounts cannot be negative.";

    public const string MismatchMessage =
        "The dower paid in cash plus the dower still to be paid must equal the total dower.";

    /// <summary>Returns an error message, or null when the three amounts are consistent.</summary>
    public static string? Validate(decimal paidInCash, decimal toBePaid, decimal total)
    {
        if (paidInCash < 0 || toBePaid < 0 || total < 0)
            return NegativeMessage;

        // The columns are decimal(18,2); compare at that precision.
        return decimal.Round(paidInCash + toBePaid, 2) == decimal.Round(total, 2)
            ? null
            : MismatchMessage;
    }
}
