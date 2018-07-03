using DAL;
using DAL.Model;
using MemberShipMVCApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static DAL.Model.MembershipCommon;

namespace MemberShipMVCApp.ViewModel
{
    public class MemberRegisterViewModel
    {
        //Member Register types
        public List<RegisteredMember> objRegisteredMembers = new List<RegisteredMember>();

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

        public bool IsActive { get; set; }
        public int Id { get; set; }
        public int FamilyId { get; set; }

        public List<SelectListItem> CountryList;
        public List<SelectListItem> GenderList;
        public List<SelectListItem> ReferredList;

        public List<SelectListItem> CategoryType;

        public List<SelectListItem> MembershipTypeList;

        public HttpPostedFileBase File { get; set; }
        public HttpPostedFileBase File2 { get; set; }

        public string imagePath1 { get; set; }
        public string imagePath2 { get; set; }

        //Membership Info types
        public int MembershipCategoryId { get; set; }
        public int MembershipTypeId { get; set; }

        public List<SelectListItem> MembershipNameList;
        public List<SelectListItem> MembershipCategoryList;

        public string CardNumber { get; set; }
        public string MemberCode1 { get; set; }
        public string MemberCode2 { get; set; }
        public DateTime MembershipEndDate { get; set; }


        public decimal Fees { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal AvailableCredit { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal AmountPaid { get; set; }

        //Family Info types

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string FamilyCategoryTypeId { get; set; }
        public List<SelectListItem> FamilyCategoryType;
        public string BloodGroupName { get; set; }
        public int BloodGroupId { get; set; }
        public List<SelectListItem> BloodGroupList;
        public string Relation { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string MaritulStatus { get; set; }
        public int Age { get; set; }
        public string FamilyCardNumber { get; set; }

        public int FamilyMemberCount { get; set; }

        MembershipOps objMembershipOps;

        //Display messages on page
        public string message { get; set; }

        public MemberRegisterViewModel()
        {
            PopulateDropDown();
        }

        public void PopulateDropDown()
        {
            objMembershipOps = new MembershipOps();

            //Bind category type drop down
            CategoryType = objMembershipOps.BindCategoryType();

            //Bind Membership Category drop down
            //MembershipCategoryList = objMembershipOps.BindMembershipCategory();

            //Bind Membership Category drop down
            //DurationList = objMembershipOps.BindMembershipDuration();

            //Bind Country drop down
            CountryList = objMembershipOps.BindCountry();

            //Bind Gender drop down
            GenderList = objMembershipOps.BindGender();

            //Bind referred member drop down
            //ReferredList = objMembershipOps.BindReferred();

            //Bind Membership Category drop down
            //MembershipCategoryList = objMembershipOps.BindMembershipCategory();

            //Bind Membership Name drop down
            MembershipNameList = objMembershipOps.BindMembershipName();

            //Bind BloodGroup drop down
            //BloodGroupList = objMembershipOps.BindBloodGroup();
        }

        public async Task<int> CallAPIForBindDropdown()
        {
            try
            {
                //List<SelectListItem> itemsList = new List<SelectListItem>();

                objMembershipOps = new MembershipOps();

                //Bind referred member drop down
                ReferredList = await objMembershipOps.BindReferred();
                //ReferredList = itemsList;

                //Bind Membership Category drop down
                MembershipCategoryList = await objMembershipOps.BindMembershipCategory();
                //MembershipCategoryList = itemsList;

                //Bind BloodGroup drop down
                BloodGroupList = await objMembershipOps.BindBloodGroup();
                //MembershipCategoryList = itemsList;                                
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        //public string InsertMembershipCategory(string MembershipName, string CategoryType, string Description)//MembershipViewModel vm)
        //{
        //    //objMembershipOps = new MembershipOps();
        //    //if (vm != null)
        //    //{
        //    //    objMembershipOps.Name = vm.Name;
        //    //    objMembershipOps.Description = vm.Description;
        //    //    objMembershipOps.Type = vm.CategoryTypeId;
        //    //}
        //    //string result = string.Empty;
        //    // MembershipOps objMembershipOps = new MembershipOps();
        //    //result = objMembershipOps.InsertMembershipCategory(vm.Name, vm.Description, vm.CategoryTypeId);
        //    //return result;
        //    return MembershipOps.InsertMembershipCategory(MembershipName, CategoryType, Description);
        //}

        //public string InsertMembershipDuration(string DurationName, int DurationDays)
        //{
        //    return MembershipOps.InsertMembershipDuration(DurationName, DurationDays);
        //}

        //public string InsertMembershipBenefit(MembershipViewModel vm)
        //{
        //    objMembershipOps = new MembershipOps();
        //    if (vm != null)
        //    {
        //        objMembershipOps.MembershipName = vm.MembershipName;
        //        objMembershipOps.MembershipPrefixCode = vm.MembershipPrefixCode;
        //        objMembershipOps.MembershipCategoryId = vm.MembershipCategoryId;
        //        objMembershipOps.DurationId = vm.DurationId;
        //        objMembershipOps.Fees = Convert.ToDecimal(vm.Fees);
        //        objMembershipOps.CreditLimit = Convert.ToInt32(vm.CreditLimit);
        //        objMembershipOps.MaxAdult = vm.MaxAdult;
        //        objMembershipOps.MaxChild = vm.MaxChild;
        //    }

        //    return MembershipOps.InsertMembershipBenefit(objMembershipOps);
        //}

        public async Task<string> FetchMembershipType(int SelectedMembershipCategory)
        {
            objMembershipOps = new MembershipOps();
            string HTML = string.Empty;

            try
            {
                MembershipTypeList = await objMembershipOps.FetchMembershipType(SelectedMembershipCategory);

                HTML += "<select data-val = 'true' data-val-number = 'The field MembershipTypeId must be a number.' data-val-required = 'The MembershipTypeId field is required.' id = 'ddlMembershipTypeList' name = 'MembershipTypeId' onchange = 'FetchMembershipDetails();' tabindex = '10'>";
                HTML += "<option selected = 'selected' value = '0'> Select Membership Type </option>";
                if (MembershipTypeList != null && MembershipTypeList.Count() > 0)
                {
                    foreach (var item in MembershipTypeList)
                    {
                        HTML += "<option value = '" + item.Value + "'>" + item.Text + "</option>";
                    }
                }
                HTML += "</select>";

            }
            catch (Exception ex)
            {
                return HTML;
            }
            return HTML;
        }

        public async Task<List<MembershipCommon>> FetchMembershipDetails(int SelectedMembershipCategory, int SelectedMembershipType)
        {
            objMembershipOps = new MembershipOps();

            List<MembershipCommon> objList = new List<MembershipCommon>();

            objList = await objMembershipOps.FetchMembershipDetails(SelectedMembershipCategory, SelectedMembershipType);

            return objList;
        }

        public async Task<string> RegisterMember(MemberRegisterViewModel vm, FormCollection frm, string personalPhotoPath, string tempPhotoPath) //string personalPhotoPath, string familyPhotoPath, string tempPhotoPath)
        {
            string result = string.Empty;
            string lastId = string.Empty;
            objMembershipOps = new MembershipOps();

            MemberRegisterOnRequestModel objMC = new MemberRegisterOnRequestModel();

            if (vm != null)
            {
                objMC.FirstName = vm.FirstName;
                objMC.LastName = vm.LastName;

                objMC.Address = vm.Address;
                objMC.ZipCode = vm.ZipCode;

                objMC.City = vm.City;
                objMC.State = vm.State;
                objMC.Country = vm.Country;
                objMC.Gender = vm.Gender;
                objMC.Comment = vm.Comment;
                objMC.CategoryTypeId = vm.CategoryTypeId;

                objMC.Nationality = vm.Nationality;
                objMC.Referred = vm.Referred;
                //objMC.imagePath1 = vm.imagePath1;
                //objMC.imagePath2 = vm.imagePath2;

                objMC.IsActive = vm.IsActive;

                objMC.MembershipCategoryId = vm.MembershipCategoryId;
                objMC.MembershipName = vm.MembershipTypeId;

                objMC.CardNumber = vm.CardNumber;
                objMC.MemberCode1 = vm.MemberCode1;
                objMC.MemberCode2 = vm.MemberCode2;
                objMC.MembershipEndDate = vm.MembershipEndDate;

                objMC.Fees = vm.Fees;
                objMC.CreditLimit = vm.CreditLimit;

                objMC.AvailableCredit = vm.AvailableCredit;
                objMC.TotalTaxAmount = vm.TotalTaxAmount;

                objMC.NetAmount = vm.NetAmount;
                objMC.AmountPaid = vm.AmountPaid;
                //objMC.File = vm.File;
            }

            //insert member details

            string tempFilePath = string.Empty;
            string tempfileName = string.Empty;
            if (vm.File != null)
            {
                tempfileName = Path.GetFileName(vm.File.FileName);
                tempFilePath = Path.Combine(tempPhotoPath, tempfileName);
                vm.File.SaveAs(tempFilePath);
            }
            lastId = await MembershipOps.RegisterMember(objMC, tempFilePath, tempfileName);

            if (!string.IsNullOrEmpty(lastId))
            {
                //if (vm.File != null)
                //{

                // let's generate a filename to store the file on the server
                var fileName = lastId + "_" + Path.GetFileName(vm.File.FileName);
                var path = Path.Combine(personalPhotoPath, fileName);
                // store the uploaded file on the file system
                vm.File.SaveAs(path);

                //MembershipOps.UpdateMemberImagePath(fileName, lastId);


                if ((System.IO.File.Exists(tempFilePath)))
                {
                    System.IO.File.Delete(tempFilePath);
                }

                //Insert family member details
                int totalFamilyCount = vm.FamilyMemberCount;
                List<string> objList = await MembershipOps.RegisterFamilyMember(frm, totalFamilyCount, lastId);

                result = "Record/s saved successfully.";
            }
            else
            {
                result = "Data save failure.";
            }
            return result;
        }

        public async Task<int> GetRegisteredMembersDetailsById(int? memberId)
        {
            objRegisteredMembers = await MembershipOps.GetRegisteredMembersDetailsById(memberId);

            #region Old code

            //using (var client = new HttpClient())
            //{
            //    //Passing service base url  
            //    client.BaseAddress = new Uri(BaseUrl);

            //    client.DefaultRequestHeaders.Clear();
            //    //Define request data format  
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
            //    HttpResponseMessage Res = await client.GetAsync(BaseUrl);

            //    //Checking the response is successful or not which is sent using HttpClient  
            //    if (Res != null)
            //    {
            //        if (Res.IsSuccessStatusCode)
            //        {
            //            //Storing the response details recieved from web api   
            //            var memberResponse = Res.Content.ReadAsStringAsync().Result;
            //            var details = JObject.Parse(memberResponse);
            //            var objResult = details["Result"].ToList();

            //            Id = Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)objResult[1]).Value);
            //            FirstName = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[3]).Value);
            //            LastName = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[4]).Value);
            //            Address = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[5]).Value);
            //            ZipCode = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[6]).Value);
            //            City = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[7]).Value);
            //            State = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[8]).Value);
            //            Country = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[9]).Value);
            //            Gender = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[10]).Value);
            //            Comment = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[11]).Value);
            //            CategoryTypeId = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[12]).Value);
            //            Nationality = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[13]).Value);
            //            Referred = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[14]).Value);
            //            imagePath1 = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[15]).Value);
            //            IsActive = Convert.ToBoolean(((Newtonsoft.Json.Linq.JProperty)objResult[16]).Value);
            //            MembershipCategoryId = Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)objResult[17]).Value);
            //            MembershipTypeId = Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)objResult[18]).Value);
            //            CardNumber = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[19]).Value);
            //            MemberCode1 = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[20]).Value);
            //            MemberCode2 = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[21]).Value);

            //            MembershipEndDate = Convert.ToDateTime(((Newtonsoft.Json.Linq.JProperty)objResult[22]).Value);
            //            Fees = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[23]).Value);
            //            CreditLimit = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[24]).Value);
            //            TotalTaxAmount = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[25]).Value);
            //            NetAmount = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[26]).Value);
            //            AmountPaid = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[27]).Value);
            //            AvailableCredit = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[28]).Value);

            //            var objFamily2 = objResult[0].ToList();

            //            foreach (var item in objFamily2.Children())
            //            {
            //                FamilyRegisteredMember objList = new FamilyRegisteredMember();
            //                //objList.Id = Convert.ToInt32(item["Id"]);

            //                objList.FamilyId = Convert.ToInt32(item["Id"]);
            //                objList.Name = Convert.ToString(item["Name"]);
            //                objList.BirthDate = Convert.ToDateTime(item["BirthDate"]);
            //                objList.CategoryTypeId = Convert.ToString(item["CategoryTypeId"]);
            //                objList.BloodGroupId = Convert.ToInt32(item["BloodGroupId"]);
            //                objList.BloodGroupName = Convert.ToString(item["BloodGroupName"]);
            //                objList.Relation = Convert.ToString(item["Relation"]);
            //                objList.ContactNo = Convert.ToString(item["ContactNo"]);
            //                objList.Email = Convert.ToString(item["Email"]);
            //                objList.MaritulStatus = Convert.ToString(item["MaritulStatus"]);
            //                objList.Age = Convert.ToInt32(item["Age"]);
            //                objList.FamilyCardNumber = Convert.ToString(item["FamilyCardNumber"]);
            //                objList.imagePath2 = Convert.ToString(item["imagePath2"]);

            //                familyList.Add(objList);
            //            }
            //        }
            //    }
            //}
            //return 1;
            #endregion

            //objRegisteredMembers = MembershipOps.GetRegisteredMembersDetailsById(memberId);

            if (objRegisteredMembers != null)
            {
                Id = objRegisteredMembers.FirstOrDefault().Id;
                FirstName = objRegisteredMembers.FirstOrDefault().FirstName;
                LastName = objRegisteredMembers.FirstOrDefault().LastName;
                Address = objRegisteredMembers.FirstOrDefault().Address;
                ZipCode = objRegisteredMembers.FirstOrDefault().ZipCode;
                City = objRegisteredMembers.FirstOrDefault().City;
                State = objRegisteredMembers.FirstOrDefault().State;
                Country = objRegisteredMembers.FirstOrDefault().Country;
                Gender = objRegisteredMembers.FirstOrDefault().Gender;
                Comment = objRegisteredMembers.FirstOrDefault().Comment;
                CategoryTypeId = objRegisteredMembers.FirstOrDefault().CategoryTypeId;
                Nationality = objRegisteredMembers.FirstOrDefault().Nationality;
                Referred = objRegisteredMembers.FirstOrDefault().Referred;
                imagePath1 = objRegisteredMembers.FirstOrDefault().imagePath1;
                IsActive = Convert.ToBoolean(objRegisteredMembers.FirstOrDefault().IsActive);
                MembershipCategoryId = objRegisteredMembers.FirstOrDefault().CategoryId;
                MembershipTypeId = objRegisteredMembers.FirstOrDefault().MembershipId;
                CardNumber = objRegisteredMembers.FirstOrDefault().CardNumber;
                MemberCode1 = objRegisteredMembers.FirstOrDefault().MemberCode1;
                MemberCode2 = objRegisteredMembers.FirstOrDefault().MemberCode2;

                MembershipEndDate = Convert.ToDateTime(objRegisteredMembers.FirstOrDefault().MembershipEndDate);
                Fees = Convert.ToDecimal(objRegisteredMembers.FirstOrDefault().Fees);
                CreditLimit = Convert.ToDecimal(objRegisteredMembers.FirstOrDefault().CreditLimit);
                TotalTaxAmount = Convert.ToDecimal(objRegisteredMembers.FirstOrDefault().TotalTaxAmount);
                NetAmount = Convert.ToDecimal(objRegisteredMembers.FirstOrDefault().NetAmount);
                AmountPaid = Convert.ToDecimal(objRegisteredMembers.FirstOrDefault().AmountPaid);
                AvailableCredit = Convert.ToDecimal(objRegisteredMembers.FirstOrDefault().AvailableCredit);
            }

            return 1;
        }

        public async Task<string> UpdateFamilyMemberDetails(MemberRegisterViewModel vm)
        {
            objMembershipOps = new MembershipOps();
            FamilyMemberRegisterOnRequest objFRM = new FamilyMemberRegisterOnRequest();
            if (vm != null)
            {
                objFRM.Name = vm.Name;

                objFRM.BirthDate = vm.BirthDate;
                objFRM.FamilyCategoryTypeId = vm.FamilyCategoryTypeId;
                objFRM.BloodGroupName = vm.BloodGroupName;

                objFRM.Relation = vm.Relation;
                objFRM.ContactNo = vm.ContactNo;
                objFRM.Email = vm.Email;
                objFRM.MaritulStatus = vm.MaritulStatus;
                objFRM.Age = vm.Age;
                objFRM.FamilyCardNumber = vm.FamilyCardNumber;
                objFRM.LastRegisterMemberId = vm.FamilyId;
                //objFRM.FamilyId = vm.FamilyId;
            }
            return await objMembershipOps.InsertFamilyMemberDetails(objFRM, "UF");
            //return await objMembershipOps.UpdateFamilyMemberDetails(objFRM);
        }

        public async Task<string> InsertFamilyMemberDetails(MemberRegisterViewModel vm)
        {
            objMembershipOps = new MembershipOps();
            FamilyMemberRegisterOnRequest objFRM = new FamilyMemberRegisterOnRequest();
            if (vm != null)
            {
                objFRM.Name = vm.Name;

                objFRM.BirthDate = vm.BirthDate;
                objFRM.FamilyCategoryTypeId = vm.FamilyCategoryTypeId;
                objFRM.BloodGroupName = vm.BloodGroupName;

                string imgPath = vm.imagePath2;

                if (imgPath != null)
                    imgPath = imgPath.Split(new string[] { "Family" }, StringSplitOptions.None)[1];

                objFRM.imagePath2 = imgPath;
                objFRM.Relation = vm.Relation;
                objFRM.ContactNo = vm.ContactNo;
                objFRM.Email = vm.Email;
                objFRM.MaritulStatus = vm.MaritulStatus;
                objFRM.Age = vm.Age;
                objFRM.FamilyCardNumber = vm.FamilyCardNumber;
                objFRM.FamilyId = vm.FamilyId;
                objFRM.Id = vm.Id;
                objFRM.LastRegisterMemberId = vm.Id;
            }
            return await objMembershipOps.InsertFamilyMemberDetails(objFRM, "IF");
            //return await objMembershipOps.InsertFamilyMemberDetails(objFRM);
        }

        public async Task<string> UpdateRegisterMember(MemberRegisterViewModel vm, string personalPhotoPath, string tempPhotoPath) //, FormCollection frm, string personalPhotoPath, string familyPhotoPath)
        {
            string result = string.Empty;
            string lastId = string.Empty;
            objMembershipOps = new MembershipOps();
            MemberRegisterOnRequestModel objMC = new MemberRegisterOnRequestModel();

            if (vm != null)
            {
                objMC.Id = vm.Id;
                objMC.FirstName = vm.FirstName;
                objMC.LastName = vm.LastName;

                objMC.Address = vm.Address;
                objMC.ZipCode = vm.ZipCode;

                objMC.City = vm.City;
                objMC.State = vm.State;
                objMC.Country = vm.Country;
                objMC.Gender = vm.Gender;
                objMC.Comment = vm.Comment;
                objMC.CategoryTypeId = vm.CategoryTypeId;

                objMC.Nationality = vm.Nationality;
                objMC.Referred = vm.Referred;
                //objMC.imagePath1 = vm.imagePath1;
                //objMC.imagePath2 = vm.imagePath2;

                objMC.IsActive = vm.IsActive;

                objMC.MembershipCategoryId = vm.MembershipCategoryId;
                objMC.MembershipName = vm.MembershipTypeId;

                objMC.CardNumber = vm.CardNumber;
                objMC.MemberCode1 = vm.MemberCode1;
                objMC.MemberCode2 = vm.MemberCode2;
                objMC.MembershipEndDate = vm.MembershipEndDate;

                objMC.Fees = vm.Fees;
                objMC.CreditLimit = vm.CreditLimit;

                objMC.AvailableCredit = vm.AvailableCredit;
                objMC.TotalTaxAmount = vm.TotalTaxAmount;

                objMC.NetAmount = vm.NetAmount;
                objMC.AmountPaid = vm.AmountPaid;
            }

            string tempFilePath = string.Empty;
            string tempfileName = string.Empty;
            if (vm.File != null)
            {
                tempfileName = Path.GetFileName(vm.File.FileName);
                tempFilePath = Path.Combine(tempPhotoPath, tempfileName);
                vm.File.SaveAs(tempFilePath);
            }

            //update member details
            lastId = await MembershipOps.UpdateRegisterMember(objMC, tempFilePath, tempfileName);

            if (!string.IsNullOrEmpty(lastId))
            {
                if (vm.File != null)
                {
                    // let's generate a filename to store the file on the server
                    var fileName = lastId + "_" + Path.GetFileName(vm.File.FileName);
                    var path = Path.Combine(personalPhotoPath, fileName);
                    // store the uploaded file on the file system
                    vm.File.SaveAs(path);

                    //MembershipOps.UpdateMemberImagePath(fileName, lastId);

                    if ((System.IO.File.Exists(tempFilePath)))
                    {
                        System.IO.File.Delete(tempFilePath);
                    }
                }

                result = "Record/s updated successfully.";
            }
            else
            {
                result = "Data save failure.";
            }
            return result;
        }

        public async Task<string> UploadFamilyPhoto(string file, string familyPhotoPath)
        {
            //update member details
            string status = string.Empty;
            if (file != null)
            {
                status = await MembershipOps.UploadFamilyPhoto(file, familyPhotoPath);
                //lastId = Convert.ToInt32(status);
            }
            return status;
        }
    }

}