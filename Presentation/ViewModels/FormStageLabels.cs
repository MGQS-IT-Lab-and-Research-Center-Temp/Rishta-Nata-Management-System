using Domain.Enums;

namespace Presentation.ViewModels;

public static class FormStageLabels
{
    public static string? For(MarriageFormStage stage) => stage switch
    {
        MarriageFormStage.AwaitingApplicants or MarriageFormStage.AwaitingBride =>
            "Waiting on the bride's section",
        MarriageFormStage.AwaitingBridegroom => "Waiting on the bridegroom's section",
        MarriageFormStage.AwaitingWitnesses =>
            "Signatures needed — Guardian/Waliy, Witness 1, Witness 2",
        MarriageFormStage.AwaitingBrideJamaatPresident =>
            "Waiting on the Bride's Jama'at President",
        MarriageFormStage.AwaitingGroomJamaatPresident =>
            "Waiting on the Bridegroom's Jama'at President",
        MarriageFormStage.AwaitingRishtanataSecretary =>
            "Waiting on the National Rishtanata Secretary",
        MarriageFormStage.AwaitingAmirApproval => "Waiting on the National Amir's approval",
        MarriageFormStage.AwaitingImamSignoff =>
            "Waiting on the Imam's sign-off after the ceremony",
        MarriageFormStage.Completed => "Completed",
        _ => null
    };
}