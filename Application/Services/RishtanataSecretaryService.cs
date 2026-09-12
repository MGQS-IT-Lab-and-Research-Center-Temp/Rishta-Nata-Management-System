using Domain.Enums;
using Infrastructure.DTOs.JamaatMember;
using Infrastructure.DTOs.MarriedCoupleDto;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Infrastructure.Persistence;
using Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.Services
{
    /// <summary>
    /// National Rishtanata Secretary dashboard and approve/reject/return.
    /// </summary>
    public class RishtanataSecretaryService : IRishtanataSecretaryService
    {
        private readonly RishtanataDbContext _context;

        public RishtanataSecretaryService(RishtanataDbContext context)
        {
            _context = context;
        }

        public RishtanataSecretaryDashboardDto GetDashboard(string? membershipNo)
        {
            var pendingApplications = _context.FormApplications
                // Cleanup: AwaitingMoreInformation is a pending-ish state too
                // (form sent back to applicants), so count it as pending.
                .Where(x => x.Status == ApplicationStatus.ApplicationPending ||
                            x.Status == ApplicationStatus.AwaitingMoreInformation)
                .ToList();

            var member = string.IsNullOrWhiteSpace(membershipNo)
                ? null
                : _context.JamaatMembers
                    .FirstOrDefault(x => x.ChandaNo == membershipNo);

            var dto = new RishtanataSecretaryDashboardDto
            {
                SecretaryName = member is null
                    ? null
                    : $"{member.FirstName} {member.Surname}".Trim(),

                PendingApprovals = pendingApplications.Count,

                ApprovedApplications = _context.FormApplications
                    .Count(x => x.Status == ApplicationStatus.ApplicationApproved),

                MarriedCouples = _context.FormApplications
                    .Count(x => x.Certificate != null),

                TotalMembers = _context.JamaatMembers.Count()
            };

            return dto;
        }

        public List<PendingApprovalDto> GetPendingApprovals()
        {
            return _context.MarriageApplicationForms
                .Where(f => f.MarriageApplication.Status ==
                // Cleanup: include awaiting-more-info forms in the pending list.
                ApplicationStatus.ApplicationPending ||
                f.MarriageApplication.Status ==
                ApplicationStatus.AwaitingMoreInformation)
                .Select(f => new PendingApprovalDto
                {
                    Id = f.MarriageApplicationId,
                    ApplicationNumber = f.ReferenceNumber,
                    GroomName = f.BridegroomName,
                    BrideName = f.BrideName,
                    PresidentName = f.JamaatPresidentName,
                    SubmittedDate = f.CreatedAt,
                    Status = f.MarriageApplication.Status.ToString()
                })
                .ToList();
        }

        public ReviewApplicationDto? GetById(Guid id)
        {
            var form = _context.MarriageApplicationForms
                .Include(x => x.MarriageApplication)
                .FirstOrDefault(x => x.MarriageApplicationId == id);

            if (form == null)
                return null;

            return new ReviewApplicationDto
            {
                Id = form.MarriageApplicationId,
                ApplicationNumber = form.ReferenceNumber,
                GroomName = form.BridegroomName,
                BrideName = form.BrideName,
                GroomPhone = form.BridegroomSignatureTel,
                BridePhone = form.BrideSignatureTel,
                PresidentName = form.JamaatPresidentName,
                SubmittedDate = form.CreatedAt,
                Status = form.MarriageApplication.Status.ToString(),
                CurrentStage = form.ApplicationStage,
                OfficiatingImamMembershipNo = form.OfficiatingImamMembershipNo,
                ApprovedDateOfNikah = form.ApprovedDateOfNikah,
            };
        }

        public List<MarriedCoupleDto> GetMarriedCouples()
        {
            return _context.MarriageApplicationForms
                .Where(x => x.MarriageApplication.Certificate != null)
                .Select(x => new MarriedCoupleDto
                {
                    Id = x.MarriageApplicationId,
                    ApplicationNumber = x.ReferenceNumber,
                    GroomName = x.BridegroomName,
                    GroomMembershipNo = x.BridegroomMembershipNo,
                    GroomDateOfBirth = x.BridegroomDateOfBirth,
                    BrideName = x.BrideName,
                    BrideMembershipNo = x.BrideMembershipNo,
                    BrideDateOfBirth = x.BrideDateOfBirth,
                    NikahDate = x.ApprovedDateOfNikah ?? DateTime.MinValue,
                    Venue = x.Venue,
                    Status = x.MarriageApplication.Status.ToString()
                })
                .ToList();
        }

        public MemberProfileDto? GetMemberProfile(Guid id)
        {
            var member = _context.JamaatMembers
                //.Include(x => x.MemberRoles)
                //    .ThenInclude(mr => mr.Role)
                .FirstOrDefault(x => x.Id == id);

            if (member == null)
                return null;

            return new MemberProfileDto
            {
                Id = member.Id,
                Surname = member.Surname,
                FirstName = member.FirstName,
                MiddleName = member.MiddleName,
                Title = member.Title,
                FullName = $"{member.FirstName} {member.Surname}".Trim(),
                Email = member.Email,
                ChandaNo = member.ChandaNo,
                WasiyatNo = member.WasiyatNo,
                AuxillaryBodyName = member.AuxillaryBodyName,
                DateOfBirth = member.DateOfBirth,
                PhoneNo = member.PhoneNo,
                JamaatName = member.JamaatName,
                CircuitName = member.CircuitName,
                Sex = member.Sex,
                MaritalStatus = member.MaritalStatus,
                Address = member.Address,
                Nationality = member.Nationality,
                RoleName = string.IsNullOrWhiteSpace(member.Roles)
                    ? null
                    : member.Roles
            };
        }

        // Cleanup: ReturnToPresident/Reject/Approve were fire-and-forget — they
        // called SaveChangesAsync() without await, so the controller redirected
        // before the write had finished. They are now Task-based and awaited.
        public async Task<bool> ReturnToPresident(Guid id)
        {
            var application = _context.FormApplications
                .FirstOrDefault(x => x.Id == id);

            if (application == null)
                return false;

            application.Status = ApplicationStatus.ApplicationPending;

            await _context.SaveChangesAsync();

            return true;
        }

        public List<JamaatMemberDto> GetMembers()
        {
            return _context.JamaatMembers
                .Select(x => JamaatMemberMapper.ToDto(x))
                .ToList();
        }

        // Same fire-and-forget SaveChangesAsync as ReturnToPresident; now awaited.
        public async Task<bool> Reject(Guid id)
        {
            var application = _context.FormApplications
                .FirstOrDefault(x => x.Id == id);

            if (application == null)
                return false;

            application.Status = ApplicationStatus.ApplicationRejected;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Approve(Guid id)
        {
            var application = _context.FormApplications
                .FirstOrDefault(x => x.Id == id);

            if (application == null)
                return false;

            application.Status = ApplicationStatus.ApplicationApproved;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateImamDesignationAsync(
            Guid id,
            string officiatingImamMembershipNo,
            DateTime? approvedDateOfNikah,
            CancellationToken cancellationToken = default)
        {
            var form = await _context.MarriageApplicationForms
                .FirstOrDefaultAsync(
                    x => x.MarriageApplicationId == id || x.Id == id,
                    cancellationToken);

            if (form == null)
                return false;

            form.OfficiatingImamMembershipNo =
                officiatingImamMembershipNo?.Trim() ?? string.Empty;
            form.ModifiedAt = DateTime.UtcNow;

            // The agreed date may change only as communicated by the partners.
            if (approvedDateOfNikah.HasValue)
            {
                form.ApprovedDateOfNikah = approvedDateOfNikah.Value;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}