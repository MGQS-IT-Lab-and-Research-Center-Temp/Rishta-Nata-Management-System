using System;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class ParticipantInvitationService : IParticipantInvitationService
    {
        private readonly RishtanataDbContext _db;
        private readonly IConfiguration _configuration;

        public ParticipantInvitationService(RishtanataDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<ParticipantInvitationDto> CreateInvitationAsync(Guid applicationId, Side side, ParticipantRole role, int? witnessOrder = null)
        {
            // Prevent duplicate active invitation
            var now = DateTime.UtcNow;
            var existing = await _db.ParticipantInvitations
                .Where(p => p.ApplicationId == applicationId
                            && p.Side == side
                            && p.ParticipantRole == role
                            && p.WitnessOrder == witnessOrder
                            && (p.Status == InvitationStatus.Pending || p.Status == InvitationStatus.Opened)
                            && p.ExpiresAt > now)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                return MapToDto(existing, includeUrl: true);
            }

            var rawToken = TokenHelper.GenerateTokenRaw();
            var hash = TokenHelper.HashToken(rawToken);

            var expiryDays = 14;
            var cfgVal = _configuration["ParticipantInvitation:ExpiryInDays"];
            if (!string.IsNullOrWhiteSpace(cfgVal) && int.TryParse(cfgVal, out var parsed)) expiryDays = parsed;

            var invitation = new ParticipantInvitation
            {
                ApplicationId = applicationId,
                Side = side,
                ParticipantRole = role,
                WitnessOrder = witnessOrder,
                TokenHash = hash,
                ExpiresAt = DateTime.UtcNow.AddDays(expiryDays),
                Status = InvitationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            _db.ParticipantInvitations.Add(invitation);
            await _db.SaveChangesAsync();

            var dto = MapToDto(invitation, includeUrl: true, rawToken: rawToken);
            return dto;
        }

        public async Task<ParticipantInvitationDto> GetInvitationByTokenAsync(string token)
        {
            var hash = TokenHelper.HashToken(token);
            var invitation = await _db.ParticipantInvitations
                .FirstOrDefaultAsync(p => p.TokenHash == hash);

            if (invitation == null) throw new InvalidOperationException("Invitation not found.");

            if (invitation.Status == InvitationStatus.Revoked) throw new InvalidOperationException("This invitation has been revoked.");

            if (invitation.ExpiresAt < DateTime.UtcNow)
            {
                invitation.Status = InvitationStatus.Expired;
                invitation.ModifiedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                throw new InvalidOperationException("This invitation link has expired.");
            }

            if (invitation.Status == InvitationStatus.Completed)
            {
                throw new InvalidOperationException("This invitation has already been completed.");
            }

            if (invitation.Status == InvitationStatus.Pending)
            {
                invitation.Status = InvitationStatus.Opened;
                invitation.ModifiedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return MapToDto(invitation, includeUrl: false);
        }

        public async Task RevokeInvitationAsync(Guid invitationId)
        {
            var invitation = await _db.ParticipantInvitations.FindAsync(invitationId);
            if (invitation == null) return;
            invitation.Status = InvitationStatus.Revoked;
            invitation.ModifiedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task MarkCompletedAsync(Guid invitationId)
        {
            var invitation = await _db.ParticipantInvitations.FindAsync(invitationId);
            if (invitation == null) throw new InvalidOperationException("Invitation not found");
            invitation.Status = InvitationStatus.Completed;
            invitation.UsedAt = DateTime.UtcNow;
            invitation.ModifiedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            // Optionally: update application status when all participants completed
            await TryUpdateApplicationStatusAsync(invitation.ApplicationId);
        }

        private async Task TryUpdateApplicationStatusAsync(Guid applicationId)
        {
            var app = await _db.FormApplications.FindAsync(applicationId);
            if (app == null) return;

            // check required roles: Bride, Groom, Guardian, Witnesses (6 participants + bride + groom?)
            // For this project, consider participants complete when all ParticipantInvitations for the application are Completed
            var totalInvitations = await _db.ParticipantInvitations.CountAsync(p => p.ApplicationId == applicationId);
            if (totalInvitations == 0) return;

            var completed = await _db.ParticipantInvitations.CountAsync(p => p.ApplicationId == applicationId && p.Status == InvitationStatus.Completed);
            if (completed >= totalInvitations)
            {
                // set application ready for review if enum supports it
                // add a new enum value ReadyForReview if needed. Here set Submitted as placeholder
                app.Status = Domain.Enums.ApplicationStatus.ApplicationPending;
                app.ModifiedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        private ParticipantInvitationDto MapToDto(ParticipantInvitation p, bool includeUrl = false, string? rawToken = null)
        {
            var dto = new ParticipantInvitationDto
            {
                InvitationId = p.Id,
                ApplicationId = p.ApplicationId,
                Side = p.Side,
                Role = p.ParticipantRole,
                WitnessOrder = p.WitnessOrder,
                Status = p.Status.ToString(),
                ExpiresAt = p.ExpiresAt
            };

            if (includeUrl)
            {
                var baseUrl = _configuration["App:BaseUrl"] ?? _configuration["ParticipantInvitation:BaseUrl"] ?? "https://localhost";
                var token = rawToken ?? "";
                dto.InvitationUrl = $"{baseUrl.TrimEnd('/')}/participant/invite/{token}";
            }

            return dto;
        }
    }
}
