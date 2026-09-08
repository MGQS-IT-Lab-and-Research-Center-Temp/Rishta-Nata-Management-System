using Domain.Entities;
using Infrastructure.DTOs;

namespace Infrastructure.Mapper;
    public  static class ReadOnlyFormMapper
    {
        public static ReadOnlyFormDto MapToReadOnlyDto(MarriageApplicationForm form)
        {
            return new ReadOnlyFormDto
            {
                Id = form.Id,
                MarriageApplicationId = form.MarriageApplicationId,
                ApplicationStage = form.ApplicationStage,

                ReferenceNumber = form.ReferenceNumber,
                ProposedNikahDate = form.ProposedNikahDate,
                Venue = form.Venue,

                BrideMembershipNo = form.BrideMembershipNo,
                BrideName = form.BrideName,
                BrideDateOfBirth = form.BrideDateOfBirth,
                BrideResidentOf = form.BrideResidentOf,
                BrideGenotype = form.BrideGenotype,
                BrideBloodGroup = form.BrideBloodGroup,
                BrideMaritalStatus = form.BrideMaritalStatus,
                BrideProposedDowerAmount = form.BrideProposedDowerAmount,
                BrideDowerAmountReceivedInCash = form.BrideDowerAmountReceivedInCash,
                BrideSignatureTel = form.BrideSignatureTel,

                BridegroomMembershipNo = form.BridegroomMembershipNo,
                BridegroomName = form.BridegroomName,
                BridegroomDateOfBirth = form.BridegroomDateOfBirth,
                BridegroomResidentOf = form.BridegroomResidentOf,
                BridegroomGenotype = form.BridegroomGenotype,
                BridegroomBloodGroup = form.BridegroomBloodGroup,
                BridegroomDowerAmountPaidInCash = form.BridegroomDowerAmountPaidInCash,
                BridegroomDowerAmountToBePaid = form.BridegroomDowerAmountToBePaid,
                IsFirstNikah = form.IsFirstNikah,
                IsSecondThirdOrFourthNikah = form.IsSecondThirdOrFourthNikah,
                FormerWifeIsDead = form.FormerWifeIsDead,
                HasDivorcedFormerWife = form.HasDivorcedFormerWife,
                FormerWifeIsPresent = form.FormerWifeIsPresent,
                FormerWifeObtainedKhula = form.FormerWifeObtainedKhula,
                BridegroomSignatureTel = form.BridegroomSignatureTel,

                GuardianName = form.GuardianName,
                GuardianRelationToBride = form.GuardianRelationToBride,
                GuardianAddress = form.GuardianAddress,
                GuardianTel = form.GuardianTel,
                GuardianSignatureDate = form.GuardianSignatureDate,

                RepresentativeName = form.RepresentativeName,
                RepresentativeAddress = form.RepresentativeAddress,
                RepresentativeActingFor = form.RepresentativeActingFor,
                RepresentativeSignatureDate = form.RepresentativeSignatureDate,

                WitnessOneName = form.WitnessOneName,
                WitnessOneAddress = form.WitnessOneAddress,
                WitnessOneTel = form.WitnessOneTel,
                WitnessOneSignatureDate = form.WitnessOneSignatureDate,

                WitnessTwoName = form.WitnessTwoName,
                WitnessTwoAddress = form.WitnessTwoAddress,
                WitnessTwoTel = form.WitnessTwoTel,
                WitnessTwoSignatureDate = form.WitnessTwoSignatureDate,

                OfficiatingImamName = form.OfficiatingImamName,
                OfficiatingImamAddressJamaat = form.OfficiatingImamAddressJamaat,
                OfficiatingImamSignatureDate = form.OfficiatingImamSignatureDate,

                JamaatPresidentName = form.JamaatPresidentName,
                JamaatPresidentSignatureDate = form.JamaatPresidentSignatureDate,

                NationalRishtanataSecretaryName = form.NationalRishtanataSecretaryName,
                NationalRishtanataSecretarySignatureDate = form.NationalRishtanataSecretarySignatureDate,

                ApprovedDateOfNikah = form.ApprovedDateOfNikah,
                NationalAmirOrMissionarySignatureDate = form.NationalAmirOrMissionarySignatureDate,

                FormStage = form.FormStage
            };
        }
    }