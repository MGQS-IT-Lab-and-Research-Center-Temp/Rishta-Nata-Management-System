using System;

namespace Domain.Enums
{
    /// <summary>
    /// UI labels and Tajneed pre-fill for BrideMaritalStatus. The one place that
    /// knows the display wording and the legacy Tajneed strings.
    /// </summary>
    public static class BrideMaritalStatusText
    {
        public static string ToLabel(BrideMaritalStatus? status) => status switch
        {
            BrideMaritalStatus.Unmarried => "Unmarried",
            BrideMaritalStatus.WidowedIddatComplete => "Widowed — waiting period passed",
            BrideMaritalStatus.DivorcedIddatComplete => "Divorced — waiting period passed",
            _ => string.Empty
        };

        /// <summary>
        /// Maps the Tajneed member profile's marital status. Only Single, Widowed
        /// and Divorced (any case) map; anything else (e.g. Married) is left unset
        /// so the applicant must choose.
        /// </summary>
        public static BrideMaritalStatus? FromTajneed(string? value)
        {
            var v = value?.Trim();

            if (string.Equals(v, "Single", StringComparison.OrdinalIgnoreCase))
                return BrideMaritalStatus.Unmarried;
            if (string.Equals(v, "Widowed", StringComparison.OrdinalIgnoreCase))
                return BrideMaritalStatus.WidowedIddatComplete;
            if (string.Equals(v, "Divorced", StringComparison.OrdinalIgnoreCase))
                return BrideMaritalStatus.DivorcedIddatComplete;

            return null;
        }
    }
}
