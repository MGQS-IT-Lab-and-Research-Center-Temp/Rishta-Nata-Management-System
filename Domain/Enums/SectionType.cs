namespace Domain.Enums;

/// <summary>
/// The three parties that fill their parts of the form anonymously via a
/// shared link at the AwaitingWitnesses stage (policy §8/design §4).
/// </summary>
public enum SectionType
{
    Guardian = 1,
    WitnessOne = 2,
    WitnessTwo = 3
}