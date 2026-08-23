
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class BridegroomFormViewModel
    {
        // =========================
        // Application
        // =========================




        // =========================
        // Bridegroom Personal Information
        // =========================
        public Guid MarriageApplicationFormId { get; set; }


        [Required(ErrorMessage = "⚠️Membership number is required⚠️.")]
        [Display(Name = "Membership Number")]
        public string BridegroomMembershipNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "⚠️Full name is required⚠️.")]
        [Display(Name = "Full Name")]
        public string BridegroomName { get; set; } = string.Empty;

        [Required(ErrorMessage = "⚠️Date of birth is required⚠️.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime BridegroomDateOfBirth { get; set; }

        [Required(ErrorMessage = "⚠️Residential address is required⚠️.")]
        [Display(Name = "Residential Address")]
        public string BridegroomResidentOf { get; set; } = string.Empty;

        [Required(ErrorMessage = "⚠️Phone number is required⚠️.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "⚠️Phone number must contain exactly 11 digits⚠️.")]
        [Display(Name = "Phone Number")]
        public string BridegroomPhoneNumber { get; set; } = string.Empty;

        public string BridegroomGenotype { get; set; } = string.Empty;

        public string BridegroomBloodGroup { get; set; } = string.Empty;


        // =========================
        // Dower Information
        // =========================

        public decimal BridegroomDowerAmountPaidInCash { get; set; }

        public decimal BridegroomDowerAmountToBePaid { get; set; }


        // =========================
        // Nikah Information
        // =========================

        public bool IsFirstNikah { get; set; }

        public bool IsSecondThirdOrFourthNikah { get; set; }


        // =========================
        // Previous Wife Information
        // =========================

        public bool FormerWifeIsDead { get; set; }

        public bool HasDivorcedFormerWife { get; set; }

        public bool FormerWifeIsPresent { get; set; }

        public bool FormerWifeObtainedKhula { get; set; }
    }
}

