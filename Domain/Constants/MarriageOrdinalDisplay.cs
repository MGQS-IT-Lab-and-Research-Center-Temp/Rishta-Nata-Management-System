using Domain.Enums;

namespace Domain.Constants;

public static class MarriageOrdinalDisplay
{
    public static string Display(this MarriageOrdinal ordinal) => ordinal switch
    {
        MarriageOrdinal.Mathna => "Mathna (second)",
        MarriageOrdinal.Thulatha => "Thulatha (third)",
        MarriageOrdinal.Arbaa => "Arba'a (fourth)",
        _ => throw new ArgumentOutOfRangeException(nameof(ordinal))
    };
}
