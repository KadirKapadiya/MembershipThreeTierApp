using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL.Model
{
    public class ImageCommon
    {
        public class ImageOnRequestModel
        {
            public string searchType { get; set; }
            public string sortType { get; set; }
            public int pageNo { get; set; }
            public int pageSize { get; set; }

        }

        public int TotalCount { get; set; }

        public class MemberShipCategory
        {
            public int Id { get; set; }
            public string MembershipName { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
        }

        public class MemberShipDuration
        {
            public int Id { get; set; }
            public string DurationName { get; set; }
            public int? DurationDays { get; set; }

        }

        public class MemberShipBenefit
        {
            public int Id { get; set; }
            public string MembershipName { get; set; }
            public string MemberPrefixCode { get; set; }
            public int MembershipCategoryId { get; set; }
            public string MembershipCategory { get; set; }
            public int DurationId { get; set; }
            public string Duration { get; set; }

            public decimal? Fees { get; set; }
            public decimal? CreditLimit { get; set; }
            public decimal? MaxAdult { get; set; }
            public decimal? MaxChild { get; set; }

        }

        //Member Register types
        public class RegisteredMember
        {
            public int Id { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string ZipCode { get; set; }
            public string City { get; set; }
            public string State { get; set; }

            public string Country { get; set; }
            public string Gender { get; set; }
            public string Comment { get; set; }
            public string CategoryTypeId { get; set; }
            public int CategoryId { get; set; }
            public string Nationality { get; set; }
            public string Referred { get; set; }
            public int ReferredBy { get; set; }

            public int MembershipId { get; set; }
            public bool? IsActive { get; set; }

            public string CardNumber { get; set; }
            public string MemberCode1 { get; set; }
            public string MemberCode2 { get; set; }

            public DateTime? MembershipEndDate { get; set; }

            public string imagePath1 { get; set; }

            public decimal? Fees { get; set; }
            public decimal? CreditLimit { get; set; }
            public decimal? AvailableCredit { get; set; }
            public decimal? TotalTaxAmount { get; set; }
            public decimal? NetAmount { get; set; }
            public decimal? AmountPaid { get; set; }

            public List<FamilyRegisteredMember> familyItems = new List<FamilyRegisteredMember>();
        }

        //Family Member Register types
        public class FamilyRegisteredMember
        {
            //Family Info types          
            public int Id { get; set; }
            public int FamilyId { get; set; }
            public string Name { get; set; }

            public DateTime? BirthDate { get; set; }

            public string CategoryTypeId { get; set; }

            public int BloodGroupId { get; set; }
            public string BloodGroupName { get; set; }

            public string Relation { get; set; }
            public string ContactNo { get; set; }
            public string Email { get; set; }
            public string MaritulStatus { get; set; }
            public int? Age { get; set; }
            public string FamilyCardNumber { get; set; }

            public string imagePath2 { get; set; }
        }
        //Image list
        public class ActorActressImageList
        {
            public int ImageId { get; set; }
            public string StarName { get; set; }
            public string StarType { get; set; }
            public string ImagePath { get; set; }

            public int TotalCount { get; set; }
        }
        public class MemberRegisterBindDropDownsOnRequest
        {
            public int Id { get; set; }
            public string Flag { get; set; }
        }
        public class MemberRegisterOnRequestModel
        {
            //public int ErrorType { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Address { get; set; }

            public string ZipCode { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string Country { get; set; }

            public string Gender { get; set; }

            public string Comment { get; set; }

            public string CategoryTypeId { get; set; }

            public string Nationality { get; set; }

            public string Referred { get; set; }

            //public string ImagePath { get; set; }

            public bool? IsActive { get; set; }

            public int MembershipCategoryId { get; set; }

            public int MembershipName { get; set; }

            public string CardNumber { get; set; }

            public string MemberCode1 { get; set; }

            public string MemberCode2 { get; set; }

            public DateTime? MembershipEndDate { get; set; }

            public decimal? Fees { get; set; }

            public decimal? CreditLimit { get; set; }

            public decimal? AvailableCredit { get; set; }

            public decimal? TotalTaxAmount { get; set; }

            public decimal? NetAmount { get; set; }

            public decimal? AmountPaid { get; set; }

            public int Id { get; set; }

            public string Flag { get; set; }

            public HttpPostedFileBase File { get; set; }
        }

        public class FamilyMemberRegisterOnRequest
        {
            //Family Info types          
            public int Id { get; set; }
            public int FamilyId { get; set; }
            public string Name { get; set; }

            public DateTime? BirthDate { get; set; }

            public string FamilyCategoryTypeId { get; set; }

            //public int BloodGroupId { get; set; }
            public string BloodGroupName { get; set; }

            public string Relation { get; set; }
            public string ContactNo { get; set; }
            public string Email { get; set; }
            public string MaritulStatus { get; set; }
            public int? Age { get; set; }
            public string FamilyCardNumber { get; set; }
            public int LastRegisterMemberId { get; set; }
            public string imagePath2 { get; set; }
            public string Flag { get; set; }
        }

        public class MemberRegisterDetailsOnRequest
        {
            public int SelectedMembershipCategory { get; set; }
            public int SelectedMembershipType { get; set; }
            public string Flag { get; set; }
        }
        public class MemberRegisterDeleteOnRequest
        {
            public int Id { get; set; }
            public string Flag { get; set; }
        }
    }
}
