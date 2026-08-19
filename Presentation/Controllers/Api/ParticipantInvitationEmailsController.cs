using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantInvitationEmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ParticipantInvitationEmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendTestInvitationEmail([FromBody] SendParticipantInvitationEmailRequest request)
        {
            try
            {
                await _emailService.SendParticipantInvitationAsync(
                    request.Email,
                    request.Name,
                    request.InvitationUrl,
                    request.Side,
                    request.Role,
                    request.WitnessOrder);

                return Ok(new { message = "Invitation email sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

    public class SendParticipantInvitationEmailRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string InvitationUrl { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? WitnessOrder { get; set; }
    }
}
