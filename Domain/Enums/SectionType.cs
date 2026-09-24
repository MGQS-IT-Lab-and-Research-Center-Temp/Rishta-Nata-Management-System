namespace Domain.Enums;

/// <summary>
/// The parties that fill their parts of the form anonymously via a
/// shared link at the AwaitingWitnesses stage (policy §8/design §4).
/// GroomWakeel only applies when the groom cannot attend the Nikah in
/// person (BridegroomFormSection.CanAttendNikahInPerson == false).
/// Representative only applies when the bride's guardian appoints one
/// (GuardianOrWakeelSection.AppointsRepresentative == true).
/// </summary>
public enum SectionType
{
    Guardian = 1,
    WitnessOne = 2,
    WitnessTwo = 3,
    GroomWakeel = 4,
    Representative = 5
}