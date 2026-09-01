using System.Security.Cryptography;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly RishtanataDbContext _db;
        private readonly IInvitationEmailService _invitationEmailService;

        public InvitationService(RishtanataDbContext db, IInvitationEmailService invitationEmailService)
        {
            _db = db;
            _invitationEmailService = invitationEmailService;
        }

        // Generate an invitation and return the invitation object.
        public async Task<Invitation> GenerateInvitationAsync(InvitationTargetType targetType, Guid? marriageApplicationId, string? marriageReferenceNumber, Guid? recipientJamaatMemberId, string? recipientMembershipNo, string? createdBy, string? emailTo = null)
        {
            // We Generate a URL-safe token (32 chars) here
            var token = GenerateToken(32);

            var invitation = new Invitation
            {
                Token = token,
                TargetType = targetType,
                MarriageApplicationId = marriageApplicationId,
                MarriageReferenceNumber = marriageReferenceNumber,
                RecipientJamaatMemberId = recipientJamaatMemberId,
                RecipientMembershipNo = recipientMembershipNo,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // 7 days as per requirement
                Used = false  // set to false initially
            };

            _db.Invitations.Add(invitation);
            await _db.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(emailTo))
            {
                await _invitationEmailService.SendAsync(invitation, emailTo);
            }

            return invitation;
        }

        public async Task<Invitation?> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            var inv = await _db.Invitations.FirstOrDefaultAsync(i => i.Token == token);
            if (inv == null) return null;
            if (inv.Used) return null;
            if (inv.ExpiresAt < DateTime.UtcNow) return null;
            return inv;
        }

        public async Task MarkUsedAsync(Guid invitationId)
        {
            var inv = await _db.Invitations.FindAsync(invitationId);
            if (inv == null) return;
            inv.Used = true;
            await _db.SaveChangesAsync();
        }

        private static string GenerateToken(int length)
        {
            // Generate secure random bytes and Base64-url encode
            var bytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            // Convert to URL-safe base64
            var token = Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
            return token;
        }
    }
}
