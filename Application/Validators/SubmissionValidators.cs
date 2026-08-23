using Domain.Entities;
using FluentValidation;
using Infrastructure.DTOs;
using Infrastructure.DTOs.BrideGroom;

namespace Application.Validators;

public sealed class BridegroomSectionDtoValidator : AbstractValidator<BridegroomSectionDto>
{
    public BridegroomSectionDtoValidator()
    {
        RuleFor(x => x.BridegroomName).NotEmpty();
        RuleFor(x => x.BridegroomDateOfBirth)
            .Must(HaveMarriageableAge)
            .WithMessage("Bridegroom must meet Jamaat's marriageable-age rule.");
        RuleFor(x => x)
            .Must(HaveValidNikahHistory)
            .WithMessage("For a previous Nikah, select exactly one former-wife status.");
    }

    private static bool HaveMarriageableAge(DateTime dateOfBirth) =>
        HasMarriageableAge(dateOfBirth);

    private static bool HaveValidNikahHistory(BridegroomSectionDto dto)
    {
        if (dto.NikahOrdinal == NikahOrdinal.First)
            return true;

        var selectedStatuses = new[]
        {
            dto.FormerWifeIsDead,
            dto.HasDivorcedFormerWife,
            dto.FormerWifeObtainedKhula
        }.Count(value => value);

        return selectedStatuses == 1;
    }

    private static bool HasMarriageableAge(DateTime dateOfBirth)
    {
        // TODO: Confirm the exact marriageable age with the product owner.
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;

        return age >= 18;
    }
}

public sealed class BrideSectionDtoValidator : AbstractValidator<BrideSectionDto>
{
    public BrideSectionDtoValidator()
    {
        RuleFor(x => x.BrideName).NotEmpty();
        RuleFor(x => x.BrideDateOfBirth)
            .Must(HaveMarriageableAge)
            .WithMessage("Bride must meet Jamaat's marriageable-age rule.");
    }

    private static bool HaveMarriageableAge(DateTime dateOfBirth)
    {
        // TODO: Confirm the exact marriageable age with the product owner.
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;

        return age >= 18;
    }
}

public sealed class GuardianOrWakeelDtoValidator : AbstractValidator<GuardianOrWakeelDto>
{
    public GuardianOrWakeelDtoValidator()
    {
        RuleFor(x => x.GuardianName).NotEmpty();
        RuleFor(x => x.GuardianTel).NotEmpty();
    }
}

public sealed class WitnessDtoValidator : AbstractValidator<WitnessDto>
{
    public WitnessDtoValidator()
    {
        RuleFor(x => x.WitnessOneName).NotEmpty();
        RuleFor(x => x.WitnessTwoName).NotEmpty();
    }
}

public sealed class ImamVerificationDtoValidator : AbstractValidator<ImamVerificationDto>
{
    public ImamVerificationDtoValidator()
    {
        RuleFor(x => x.OfficiatingImamName).NotEmpty();
        RuleFor(x => x.OfficiatingImamSignatureDate).NotEmpty();
    }
}

public sealed class JamaatPresidentVerificationDtoValidator : AbstractValidator<JamaatPresidentVerificationDto>
{
    public JamaatPresidentVerificationDtoValidator()
    {
        RuleFor(x => x.JamaatPresidentName).NotEmpty();
        RuleFor(x => x.JamaatPresidentSignatureDate).NotEmpty();
    }
}

public sealed class RishtanataRecommendationDtoValidator : AbstractValidator<RishtanataRecommendationDto>
{
    public RishtanataRecommendationDtoValidator()
    {
        RuleFor(x => x.NationalRishtanataSecretaryName).NotEmpty();
        RuleFor(x => x.NationalRishtanataSecretarySignatureDate).NotEmpty();
    }
}

public sealed class AmirApprovalDtoValidator : AbstractValidator<AmirApprovalDto>
{
    public AmirApprovalDtoValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .MinimumLength(10)
            .When(x => !x.IsApproved)
            .WithMessage("A rejection reason is required and must be at least 10 characters.");
    }
}