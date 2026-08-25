using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("invitations")]
    public class InvitationsController : Controller
    {
        private readonly IInvitationService _invitationService;

        public InvitationsController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromForm] InvitationGenerateModel model)
        {
            // This link generation is code or path independent
            var inv = await _invitationService.GenerateInvitationAsync(
                model.TargetType,
                model.MarriageApplicationId,
                model.MarriageReferenceNumber,
                model.RecipientJamaatMemberId,
                model.RecipientMembershipNo,
                model.CreatedBy,
                model.Email);
            return Ok(new { token = inv.Token, expiresAt = inv.ExpiresAt });
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> Accept(string token)
        {
            var inv = await _invitationService.ValidateTokenAsync(token);
            if (inv == null) return NotFound();

            return Ok(new
            {
                target = inv.TargetType.ToString(),
                marriageApplicationId = inv.MarriageApplicationId,
                marriageReferenceNumber = inv.MarriageReferenceNumber
            });
        }
    }

    public class InvitationGenerateModel
    {
        public InvitationTargetType TargetType { get; set; }
        public Guid? MarriageApplicationId { get; set; }
        public string? MarriageReferenceNumber { get; set; }
        public Guid? RecipientJamaatMemberId { get; set; }
        public string? RecipientMembershipNo { get; set; }
        public string? TargetIdentifier { get; set; }
        public string? CreatedBy { get; set; }
        public string? Email { get; set; }
    }
}
