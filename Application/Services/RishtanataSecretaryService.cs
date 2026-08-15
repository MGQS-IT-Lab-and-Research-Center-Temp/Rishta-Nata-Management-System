using Infrastructure.DTOs.RishtanataSecretaryDashboardDTO;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

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
            var pendingApplications = _context.MarriageApplications
                .Where(x => x.Status == ApplicationStatus.NationalRishtanataSecretaryPendingApproval)
                .ToList();

            var dto = new RishtanataSecretaryDashboardDto
            {
                PendingApprovals = pendingApplications.Count,

                ApprovedApplications = _context.MarriageApplications
                    .Count(x => x.Status == ApplicationStatus.NationalRishtanataSecretaryReviewApproved),

                MarriedCouples = _context.MarriageApplications
                    .Count(x => x.Certificate != null)
            };

            return dto;
        }
        public List<PendingApprovalDto> GetPendingApprovals()
        {
            return _context.MarriageApplicationForms
                .Where(f => f.MarriageApplication.Status ==
                            ApplicationStatus.NationalRishtanataSecretaryPendingApproval)

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

                    CertificateNumber = x.MarriageApplication.SerialNumber,

                    HusbandName = x.BridegroomName,

                    WifeName = x.BrideName,

                    MarriageDate = x.ApprovedDateOfNikah ?? DateTime.MinValue,

                    Status = x.MarriageApplication.Status.ToString()
                })
                .ToList();
        }
         public void ReturnToPresident(Guid id)
        {
            var application = _context.MarriageApplications
                .FirstOrDefault(x => x.Id == id);

            if (application == null)
                throw new Exception("Application not found.");
            application.Status = ApplicationStatus.JamaatPresidentPendingApproval;
            _context.SaveChanges();
        }
        public ReviewApplicationDto GetApplication(Guid id)
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
                Status = form.MarriageApplication.Status.ToString(),
                SubmittedDate = form.CreatedAt
            };
        }
        //public List<JamaatMemberDto> GetMembers()
        //{
        //    return _context.JamaatMembers
        //        .Select(x => new JamaatMemberDto
        //        {
        //            Id = x.Id,
        //            MemberNumber = x.MembershipNumber,
        //            FullName = x.FullName,
        //            PhoneNumber = x.PhoneNumber,
        //            Gender = x.Gender,
        //            Occupation = x.Occupation,
        //            MaritalStatus = x.MaritalStatus,
        //            JamaatName = x.Jamaat.Name
        //        })
        //        .ToList();
        //}

        public void Approve(Guid id)
        {
            var application = _context.MarriageApplications
                .FirstOrDefault(x => x.Id == id);

            if (application == null)
                throw new Exception("Application not found.");

            application.Status = ApplicationStatus.NationalRishtanataSecretaryReviewApproved;

            _context.SaveChanges();
        }
        public void Reject(Guid id)
        {
            var application = _context.MarriageApplications
                .FirstOrDefault(x => x.Id == id);

            if (application == null)
                throw new Exception("Application not found.");

            application.Status = ApplicationStatus.NationalRishtanataSecretaryReviewRejected;

            _context.SaveChanges();
        }
      
    }
}
