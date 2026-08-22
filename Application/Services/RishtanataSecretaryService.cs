using Application.Interfaces;
using Domain.Enums;
using Infrastructure.DTOs.JamaatMember;
using Microsoft.EntityFrameworkCore;
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
                .Include(f => f.MarriageApplication)
                .Where(f => f.MarriageApplication.Status ==
                            ApplicationStatus.ApplicationPending)
                .AsEnumerable()
                .Select(f => f.ToPendingApprovalDto())
                .ToList();
        }


        public ReviewApplicationDto GetById(Guid id)
        {
            var form = _context.MarriageApplicationForms
                .Include(f => f.MarriageApplication)
                .FirstOrDefault(f =>
                    f.MarriageApplicationId == id);

            if (form == null)
                throw new Exception("Application not found.");

            return form.ToReviewApplicationDto();
        }


        public List<MarriedCoupleDto> GetMarriedCouples()
        {
            return _context.MarriageApplicationForms
                .Include(f => f.MarriageApplication)
                .Where(f => f.MarriageApplication.Certificate != null)
                .AsEnumerable()
                .Select(f => f.ToMarriedCoupleDto())
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
                .Select(x => new JamaatMemberDto
                {
                    Id = x.Id,
                    ChandaNo = x.ChandaNo,
                    FirstName = (x.FirstName + " " + x.Surname).Trim(),
                    PhoneNo = x.PhoneNo ?? string.Empty,
                    Sex = x.Sex,
                    MaritalStatus = x.MaritalStatus ?? string.Empty,
                    JamaatName = x.JamaatName
                })
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
