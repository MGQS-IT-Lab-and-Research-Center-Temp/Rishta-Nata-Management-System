using Domain.Entities;

namespace Application.Interfaces;

public interface IJamaatMemberService
{
    Task<JamaatMember> CreateOrUpdateAsync(JamaatMember jamaatMember);
}