using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Member sync: create or update a member from the external member API.
/// </summary>
public interface IJamaatMemberService
{
    Task<JamaatMember> CreateOrUpdateAsync(JamaatMember jamaatMember);
}