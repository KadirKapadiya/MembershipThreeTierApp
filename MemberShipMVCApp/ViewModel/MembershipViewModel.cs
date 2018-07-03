using DAL;

using MemberShipMVCApp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static DAL.Model.MembershipCommon;

namespace MemberShipMVCApp.ViewModel
{
    public class MembershipViewModel
    {
        public List<MemberShipCategory> objMemberShip;
        public List<MemberShipDuration> objMemberShipDuration;

        public List<MemberShipBenefit> objMembershipBenefit;

        public List<RegisteredMember> objRegisteredMembers;

        public string Operation { get; set; }
        public string Type { get; set; }

        //Membership Category types
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryTypeId { get; set; }
        public List<SelectListItem> CategoryType;

        //Membership Duration types
        public string DurationName { get; set; }
        public int DurationDays { get; set; }

        //Membership & Benefit types
        public string MembershipName { get; set; }
        public string MembershipPrefixCode { get; set; }
        public int MembershipCategoryId { get; set; }
        public int DurationId { get; set; }

        public List<SelectListItem> MembershipCategoryList;
        public List<SelectListItem> DurationList;

        public decimal Fees { get; set; }
        public int CreditLimit { get; set; }
        public int MaxAdult { get; set; }
        public int MaxChild { get; set; }

        public int Id { get; set; }

        //Display messages on page
        public string message { get; set; }

        public MembershipViewModel()
        {
            PopulateDropDown();
        }
        MembershipOps objMembershipOps;

        public void PopulateDropDown()
        {
            objMembershipOps = new MembershipOps();

            //Bind category type drop down
            CategoryType = objMembershipOps.BindCategoryType();

            //Bind Membership Category drop down
            //MembershipCategoryList = objMembershipOps.BindMembershipCategory();

            //Bind Membership Category drop down
            DurationList = objMembershipOps.BindMembershipDuration();
        }

        public async Task<int> CallAPIForBindDropdown()
        {
            try
            {
                List<SelectListItem> itemsList = new List<SelectListItem>();

                objMembershipOps = new MembershipOps();

                //Bind Membership Category drop down
                MembershipCategoryList = await objMembershipOps.BindMembershipCategory();
                //MembershipCategoryList = itemsList;

            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        public string InsertMembershipCategory(string MembershipName, string CategoryType, string Description)//MembershipViewModel vm)
        {
            //objMembershipOps = new MembershipOps();
            //if (vm != null)
            //{
            //    objMembershipOps.Name = vm.Name;
            //    objMembershipOps.Description = vm.Description;
            //    objMembershipOps.Type = vm.CategoryTypeId;
            //}
            //string result = string.Empty;
            // MembershipOps objMembershipOps = new MembershipOps();
            //result = objMembershipOps.InsertMembershipCategory(vm.Name, vm.Description, vm.CategoryTypeId);
            //return result;
            return MembershipOps.InsertMembershipCategory(MembershipName, CategoryType, Description);
        }

        public string InsertMembershipDuration(string DurationName, int DurationDays)
        {
            return MembershipOps.InsertMembershipDuration(DurationName, DurationDays);
        }

        public string InsertMembershipBenefit(MembershipViewModel vm)
        {
            objMembershipOps = new MembershipOps();
            if (vm != null)
            {
                objMembershipOps.MembershipName = vm.MembershipName;
                objMembershipOps.MembershipPrefixCode = vm.MembershipPrefixCode;
                objMembershipOps.MembershipCategoryId = vm.MembershipCategoryId;
                objMembershipOps.DurationId = vm.DurationId;
                objMembershipOps.Fees = Convert.ToDecimal(vm.Fees);
                objMembershipOps.CreditLimit = Convert.ToInt32(vm.CreditLimit);
                objMembershipOps.MaxAdult = vm.MaxAdult;
                objMembershipOps.MaxChild = vm.MaxChild;
            }

            return MembershipOps.InsertMembershipBenefit(objMembershipOps);
        }

        public void GetMembershipCategory()
        {
            objMemberShip = new List<MemberShipCategory>();
            objMemberShip = MembershipOps.GetMembershipCategory();
        }
        public void GetMembershipDuration()
        {
            objMemberShipDuration = new List<MemberShipDuration>();
            objMemberShipDuration = MembershipOps.GetMembershipDuration();
        }

        public void GetMembershipBenefit()
        {
            objMembershipBenefit = new List<MemberShipBenefit>();
            objMembershipBenefit = MembershipOps.GetMembershipBenefit();
        }

        public void GetRegisteredMembers(HttpResponseMessage Res)
        {
            //objRegisteredMembers = new List<RegisteredMember>();
            //objRegisteredMembers = MembershipOps.GetRegisteredMembers();

            if (Res != null)
            {
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var memberResponse = Res.Content.ReadAsStringAsync().Result;

                    objRegisteredMembers = new List<RegisteredMember>();
                    //objRegisteredMembers = MembershipOps.GetRegisteredMembers(memberResponse);

                    List<RegisteredMember> itemsList = new List<RegisteredMember>();

                    var details = JObject.Parse(memberResponse);
                    var objResult = details["Result"].ToList();

                    //Id = Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)objResult[1]).Value);
                    foreach (var item in objResult)
                    {
                        RegisteredMember items = new RegisteredMember();
                        items.Id = Convert.ToInt32(item["Id"]);
                        items.FirstName = Convert.ToString(item["FirstName"]);
                        items.LastName = Convert.ToString(item["LastName"]);
                        items.Address = Convert.ToString(item["Address"]);
                        items.ZipCode = Convert.ToString(item["ZipCode"]);
                        items.City = Convert.ToString(item["City"]);
                        items.State = Convert.ToString(item["State"]);
                        items.Country = Convert.ToString(item["Country"]);
                        items.Gender = Convert.ToString(item["Gender"]);
                        items.Comment = Convert.ToString(item["Comment"]);

                        itemsList.Add(items);
                    }

                    objRegisteredMembers = itemsList;

                }
            }
        }

        public virtual List<MemberShipCategory> GetMembershipCategoryById(int Id)
        {
            //List<string> result = new List<string>();
            objMemberShip = MembershipOps.GetMembershipCategoryById(Id);

            //if(objMemberShip != null)
            //{
            //    Name = objMemberShip.FirstOrDefault().MembershipName;
            //    CategoryTypeId = objMemberShip.FirstOrDefault().Category;
            //    Description = objMemberShip.FirstOrDefault().Description;
            //}
            return objMemberShip;
        }

        public List<MemberShipDuration> GetDurationDetails(int Id)
        {
            objMemberShipDuration = MembershipOps.GetDurationDetailsById(Id);
            return objMemberShipDuration;
        }

        public List<MemberShipBenefit> GetMembershipBenefitDetails(int Id)
        {
            objMembershipBenefit = MembershipOps.GetMembershipBenefitDetailsById(Id);
            return objMembershipBenefit;
        }


        public int DeleteMembershipCategory(int Id)
        {
            int status = MembershipOps.DeleteMembershipCategory(Id);

            return status;
        }
        public int DeleteMembershipDuration(int Id)
        {
            int status = MembershipOps.DeleteMembershipDuration(Id);

            return status;
        }
        public int DeleteMembershipBenefit(int Id)
        {
            int status = MembershipOps.DeleteMembershipBenefit(Id);

            return status;
        }

        public async Task<int> DeleteMembershipDetails(MembershipViewModel vm)
        {
            objMembershipOps = new MembershipOps();
            string status = await objMembershipOps.DeleteMembershipDetails(vm.Id, vm.Type);

            int result = Convert.ToInt32(status);

            return result;
        }

        public string UpdateCategorydetails(string MembershipName, string CategoryType, string Description, int Id)//MembershipViewModel vm)
        {
            return MembershipOps.UpdateCategorydetails(MembershipName, CategoryType, Description, Id);
        }

        public string UpdateDurationdetails(string DurationName, int DurationDays, int Id)//MembershipViewModel vm)
        {
            return MembershipOps.UpdateDurationdetails(DurationName, DurationDays, Id);
        }

        public string UpdateMembershipBenefitDetails(MembershipViewModel vm)//MembershipViewModel vm)
        {
            objMembershipOps = new MembershipOps();

            MemberShipBenefit objList = new MemberShipBenefit();
            if (vm != null)
            {
                objList.MembershipName = vm.MembershipName;
                objList.MemberPrefixCode = vm.MembershipPrefixCode;
                objList.MembershipCategoryId = vm.MembershipCategoryId;
                objList.DurationId = vm.DurationId;
                objList.Fees = Convert.ToDecimal(vm.Fees);
                objList.CreditLimit = Convert.ToInt32(vm.CreditLimit);
                objList.MaxAdult = vm.MaxAdult;
                objList.MaxChild = vm.MaxChild;
                objList.Id = vm.Id;
            }

            return MembershipOps.UpdateMembershipBenefitDetails(objList);

        }
    }
}