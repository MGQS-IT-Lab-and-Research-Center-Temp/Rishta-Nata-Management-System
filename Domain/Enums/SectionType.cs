namespace Domain.Enums;

/// <summary>
/// The parties that fill their parts of the form anonymously via a
/// shared link (policy §8/design §4). Every section is filled at the
/// AwaitingWitnesses stage except the two Nikah-ceremony witnesses, which
/// are filled at AwaitingImamSignoff.
/// GroomWakeel only applies when the groom cannot attend the Nikah in
/// person (BridegroomFormSection.CanAttendNikahInPerson == false).
/// Representative and the wakeel-appointment witnesses only apply when the
/// bride's guardian appoints one (GuardianOrWakeelSection.AppointsRepresentative == true).
/// WitnessOne/WitnessTwo are the WitnessContext.GuardianAgreement pair.
/// </summary>
public enum SectionType
{
    Guardian = 1,
    WitnessOne = 2,
    WitnessTwo = 3,
    GroomWakeel = 4,
    Representative = 5,
    WakeelAppointmentWitnessOne = 6,
    WakeelAppointmentWitnessTwo = 7,
    GroomDeclarationWitnessOne = 8,
    GroomDeclarationWitnessTwo = 9,
    NikahCeremonyWitnessOne = 10,
    NikahCeremonyWitnessTwo = 11
}