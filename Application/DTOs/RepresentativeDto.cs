using System;

namespace Application.DTOs;

public class RepresentativeDto
{
    public Guid MarriageApplicationId { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string ActingFor { get; set; } = string.Empty;

    public string SignatureDate { get; set; } = string.Empty;
}