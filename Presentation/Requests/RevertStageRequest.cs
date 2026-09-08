using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Presentation.Requests;

public class RevertStageRequest
{
    [Required]
    public ApplicationStage TargetStage { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "A rejection reason is required.")]
    public string Reason { get; set; } = string.Empty;
}
