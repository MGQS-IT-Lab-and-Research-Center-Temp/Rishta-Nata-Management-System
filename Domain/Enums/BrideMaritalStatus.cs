using System.Text.Json.Serialization;

namespace Domain.Enums
{
    /// <summary>
    /// The bride's marital status as the paper form offers it (Gap 6). Stored
    /// by name (HasConversion&lt;string&gt;) and serialized by name in JSON.
    /// Null on the entity means the bride hasn't chosen yet.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<BrideMaritalStatus>))]
    public enum BrideMaritalStatus
    {
        Unmarried = 0,
        WidowedIddatComplete = 1,
        DivorcedIddatComplete = 2
    }
}
