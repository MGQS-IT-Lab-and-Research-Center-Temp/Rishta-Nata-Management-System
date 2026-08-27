using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs.MarriageApplicationFormDetail;

public class RevertStageRequestDto
{
    [Required]
    public ApplicationStage TargetStage { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "A rejection reason is required.")]
    public string Reason { get; set; } = string.Empty;
}