namespace Presentation.ViewModels.Imam;

public class ImamSignoffViewModel
{
    public Guid FormId { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public string BrideName { get; set; } = string.Empty;

    public string BridegroomName { get; set; } = string.Empty;

    public DateTime? ApprovedDateOfNikah { get; set; }

    public string Venue { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string AddressJamaat { get; set; } = string.Empty;

    public string Tel { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;
}