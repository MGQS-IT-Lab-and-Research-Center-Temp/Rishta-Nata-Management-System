using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums;

public enum RevertStageResult
{
    Success,
    FormNotFound,
    InvalidTargetStage,
    ApplicationAlreadyApproved,
    Unauthorized
}