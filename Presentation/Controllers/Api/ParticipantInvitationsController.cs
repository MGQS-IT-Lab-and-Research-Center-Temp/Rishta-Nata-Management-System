using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantInvitationsController : ControllerBase
    {
        private readonly IParticipantInvitationService _service;

        public ParticipantInvitationsController(IParticipantInvitationService service)
        {
            _service = service;
        }

        // POST: /api/ParticipantInvitations/applications/{applicationId}/invitations
        [HttpPost("applications/{applicationId}/invitations")]
        public async Task<IActionResult> CreateInvitation(Guid applicationId, [FromBody] CreateInvitationRequest request)
        {
            if (!Enum.TryParse<Side>(request.Side, true, out var side))
            {
                return BadRequest("Invalid side");
            }

            if (!Enum.TryParse<ParticipantRole>(request.Role, true, out var role))
            {
                return BadRequest("Invalid role");
            }

            var dto = await _service.CreateInvitationAsync(applicationId, side, role, request.WitnessOrder);
            return Ok(dto);
        }

        // GET: /api/participant/invitations/{token}
        [HttpGet("/api/participant/invitations/{token}")]
        public async Task<IActionResult> GetByToken(string token)
        {
            try
            {
                var dto = await _service.GetInvitationByTokenAsync(token);
                return Ok(dto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class CreateInvitationRequest
    {
        public string Side { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? WitnessOrder { get; set; }
    }
}
