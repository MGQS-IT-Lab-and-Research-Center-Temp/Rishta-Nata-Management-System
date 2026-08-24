using Domain.Enums;
using Infrastructure.DTOs.JamaatMember;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DTOs.MarriedCoupleDto;
using Infrastructure.DTOs.RishtanataSecretaryDashboardDto;
using Infrastructure.Persistence;
using Infrastructure.Mapper;
namespace Application.Services
{
   public class RishtanataSecretaryService : IRishtanataSecretaryService
{

        private readonly RishtanataDbContext _context;

        public RishtanataSecretaryService(RishtanataDbContext context)
        {
            _context = context;
        }
        public RishtanataSecretaryDashboardDto GetDashboard()
        {
            var pendingApplications = _context.FormApplications
                .Where(x => x.Status == ApplicationStatus.ApplicationPending)
                .ToList();

            var dto = new RishtanataSecretaryDashboardDto
            {
                PendingApprovals = pendingApplications.Count,

                ApprovedApplications = _context.FormApplications
                    .Count(x => x.Status == ApplicationStatus.ApplicationApproved),

                MarriedCouples = _context.FormApplications
                    .Count(x => x.Certificate != null)
            };

            return dto;
        }
        public List<PendingApprovalDto> GetPendingApprovals()
        {
            return _context.MarriageApplicationForms
                .Where(f => f.MarriageApplication.Status ==
                            ApplicationStatus.ApplicationPending)

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
        public ReviewApplicationDto GetById(Guid id)
        {
            var form = _context.MarriageApplicationForms
                .FirstOrDefault(x => x.MarriageApplicationId == id);

            if (form == null)
                throw new Exception("Application not found.");

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

                Status = form.MarriageApplication.Status.ToString()
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

        public MemberProfileDto GetMemberProfile(Guid id)
        {
            var member = _context.JamaatMembers
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Id == id);

            if (member == null)
                throw new Exception("Member not found.");

            return new MemberProfileDto
            {
                Id = member.Id,
                Surname = member.Surname,
                FirstName = member.FirstName,
                MiddleName = member.MiddleName,
                MaidenName = member.MaidenName,
                Title = member.Title,
                FullName = member.FullName,
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
                NextOfKinName = member.NextOfKinName,
                NextOfKinPhoneNo = member.NextOfKinPhoneNo,
                NextOfKinAddress = member.NextOfKinAddress,
                Nationality = member.Nationality,
                RoleName = member.Role?.Name
            };
        }
        public void ReturnToPresident(Guid id)
        {
            var application = _context.FormApplications
                .FirstOrDefault(x => x.Id == id);

            if (application == null)
                throw new Exception("Application not found.");

            application.Status = ApplicationStatus.ApplicationPending;

            _context.SaveChangesAsync();
        }

        public List<JamaatMemberDto> GetMembers()
        {
            return _context.JamaatMembers
                .Select(x => JamaatMemberMapper.ToDto(x))
                .ToList();
        }
        public void Reject(Guid id)
        {
            var application = _context.FormApplications
                .FirstOrDefault(x => x.Id == id);

            if (application == null)
                throw new Exception("Application not found.");

            application.Status = ApplicationStatus.ApplicationRejected;

            _context.SaveChangesAsync();
        }
        public void Approve(Guid id)
        {
            var application = _context.FormApplications
                .FirstOrDefault(x => x.Id == id);

            if (application == null)
                throw new Exception("Application not found.");

            application.Status = ApplicationStatus.ApplicationApproved;

            _context.SaveChangesAsync();
        }
    }
}
