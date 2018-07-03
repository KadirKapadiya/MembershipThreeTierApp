using DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using static DAL.Model.MembershipCommon;
using System.IO;

namespace DAL
{
    public class MembershipOps
    {
        //Membership & Benefit types
        public string MembershipName { get; set; }
        public string MembershipPrefixCode { get; set; }
        public int MembershipCategoryId { get; set; }
        public int DurationId { get; set; }
        public decimal Fees { get; set; }
        public int CreditLimit { get; set; }
        public int MaxAdult { get; set; }
        public int MaxChild { get; set; }
        
        string BaseUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();

        #region Bind dropdown values

        public List<SelectListItem> BindCategoryType()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem
            {
                Text = "Select Category",
                Value = "0",
                Selected = true
            });

            items.Add(new SelectListItem
            { Text = "Regular", Value = "Regular" });

            items.Add(new SelectListItem
            { Text = "Corporate", Value = "Corporate" });

            return items;
        }

        public async Task<List<SelectListItem>> BindMembershipCategory()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string apiUrl = BaseUrl + Constant.BIND_DROPDOWN;
            try
            {
                //Bind member category drop down
                using (var client = new HttpClient())
                {
                    MemberRegisterBindDropDownsOnRequest myModel;
                    myModel = new MemberRegisterBindDropDownsOnRequest();
                    myModel.Flag = Constant.CATEGORY;
                    myModel.Id = 0;

                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format  
                    var json = JsonConvert.SerializeObject(myModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.PostAsync(apiUrl, stringContent);

                    items = BindDropdownLists(Res, myModel.Flag);
                }

                //using (MembershipCatEntities db = new MembershipCatEntities())
                //{
                //    items = db.GetMembeshipCategoryAndDuration(null, "C").Select(c => new SelectListItem
                //    {
                //        Value = c.Value.ToString(),
                //        Text = c.Text.ToString()
                //    }).ToList<SelectListItem>();
                //}
                //items.Insert(0, (new SelectListItem { Text = "Select Category", Value = "0", Selected = true }));
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindMembershipDuration()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    items = db.GetMembeshipCategoryAndDuration(null, Constant.DURATION).Select(c => new SelectListItem
                    {
                        Value = c.Value.ToString(),
                        Text = c.Text.ToString()
                    }).ToList<SelectListItem>();
                }

                items.Insert(0, (new SelectListItem { Text = "Select Duration", Value = "0", Selected = true }));
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindCountry()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem
            {
                Text = "Select Country",
                Value = "0",
                Selected = true
            });

            items.Add(new SelectListItem
            { Text = "India", Value = "Ind" });

            items.Add(new SelectListItem
            { Text = "UK", Value = "UK" });

            items.Add(new SelectListItem
            { Text = "US", Value = "US" });

            items.Add(new SelectListItem
            { Text = "South Africa", Value = "SA" });

            return items;
        }

        public List<SelectListItem> BindGender()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem
            {
                Text = "Select Gender",
                Value = "0",
                Selected = true
            });

            items.Add(new SelectListItem
            { Text = "Male", Value = "M" });

            items.Add(new SelectListItem
            { Text = "Female", Value = "F" });

            items.Add(new SelectListItem
            { Text = "Other", Value = "O" });

            return items;
        }

        public async Task<List<SelectListItem>> BindReferred()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string apiUrl = BaseUrl + Constant.BIND_DROPDOWN;
            try
            {
                //Bind referred member drop down
                using (var client = new HttpClient())
                {
                    MemberRegisterBindDropDownsOnRequest myModel;
                    myModel = new MemberRegisterBindDropDownsOnRequest();
                    myModel.Flag = Constant.REFERRED;
                    myModel.Id = 0;

                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format  
                    var json = JsonConvert.SerializeObject(myModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.PostAsync(apiUrl, stringContent);

                    items = BindDropdownLists(Res, myModel.Flag);
                }

                //using (MembershipCatEntities db = new MembershipCatEntities())
                //{
                //    items = db.GetMembeshipCategoryAndDuration(null, "M").Select(c => new SelectListItem
                //    {
                //        Value = c.Value.ToString(),
                //        Text = c.Text.ToString()
                //    }).ToList<SelectListItem>();
                //}
                //items.Insert(0, (new SelectListItem { Text = "Select Member", Value = "0", Selected = true }));
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindDropdownLists(HttpResponseMessage Res, string flag)
        {
            List<SelectListItem> itemsList = new List<SelectListItem>();
            if (Res != null)
            {
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var memberResponse = Res.Content.ReadAsStringAsync().Result;

                    var details = JObject.Parse(memberResponse);
                    var objResult = details["Result"].ToList();

                    try
                    {
                        foreach (var item in objResult)
                        {
                            SelectListItem items = new SelectListItem();
                            items.Value = Convert.ToString(item["Value"]);
                            items.Text = Convert.ToString(item["Text"]);
                            itemsList.Add(items);
                        }

                        if (flag == Constant.REFERRED)
                        {
                            itemsList.Insert(0, (new SelectListItem { Text = "Select Member", Value = "0", Selected = true }));
                            //ReferredList = itemsList;
                        }
                        else if (flag == Constant.CATEGORY)
                        {
                            itemsList.Insert(0, (new SelectListItem { Text = "Select Category", Value = "0", Selected = true }));
                            //MembershipCategoryList = itemsList;
                        }
                        else if (flag == Constant.BLOOD_GROUP)
                        {
                            itemsList.Insert(0, (new SelectListItem { Text = "Select Blood Group", Value = "0", Selected = true }));
                            //BloodGroupList = itemsList;
                        }
                        //else if (flag == "MT")
                        //{
                        //    itemsList.Insert(0, (new SelectListItem { Text = "Select Membership Type", Value = "0", Selected = true }));
                        //    //BloodGroupList = itemsList;
                        //}
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return itemsList;
        }

        public List<SelectListItem> BindMembershipName()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {
                items.Insert(0, (new SelectListItem { Text = "Select Membership Type", Value = "0", Selected = true }));
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SelectListItem>> FetchMembershipType(int SelectedMembershipCategory)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string apiUrl = BaseUrl + Constant.BIND_DROPDOWN;
            try
            {
                //Bind referred member drop down
                using (var client = new HttpClient())
                {
                    MemberRegisterBindDropDownsOnRequest myModel;
                    myModel = new MemberRegisterBindDropDownsOnRequest();
                    myModel.Flag = Constant.MEMBERSHIP_TYPE;
                    myModel.Id = SelectedMembershipCategory;

                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format  
                    var json = JsonConvert.SerializeObject(myModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.PostAsync(apiUrl, stringContent);

                    items = BindDropdownLists(Res, myModel.Flag);
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //List<SelectListItem> items = new List<SelectListItem>();

            //try
            //{
            //    using (MembershipCatEntities db = new MembershipCatEntities())
            //    {
            //        items = db.GetMembeshipCategoryAndDuration(SelectedMembershipCategory, "MT").Select(c => new SelectListItem
            //        {
            //            Value = c.Value.ToString(),
            //            Text = c.Text.ToString()
            //        }).ToList<SelectListItem>();
            //    }
            //    //items.Insert(0, (new SelectListItem { Text = "Select Membership Type", Value = "0", Selected = true }));
            //    return items;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<List<SelectListItem>> BindBloodGroup()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            string apiUrl = BaseUrl + Constant.BIND_DROPDOWN;
            try
            {
                //Bind referred member drop down
                using (var client = new HttpClient())
                {
                    MemberRegisterBindDropDownsOnRequest myModel;
                    myModel = new MemberRegisterBindDropDownsOnRequest();
                    myModel.Flag = Constant.BLOOD_GROUP;
                    myModel.Id = 0;
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format  
                    var json = JsonConvert.SerializeObject(myModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.PostAsync(apiUrl, stringContent);

                    items = BindDropdownLists(Res, myModel.Flag);
                }

                //using (MembershipCatEntities db = new MembershipCatEntities())
                //{
                //    items = db.GetMasterTypes("BG").Select(c => new SelectListItem
                //    {
                //        Value = c.Value.ToString(),
                //        Text = c.Text.ToString()
                //    }).ToList<SelectListItem>();
                //}
                //items.Insert(0, (new SelectListItem { Text = "Select Blood Group", Value = "0", Selected = true }));
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //Insert Membership category details
        public static string InsertMembershipCategory(string MembershipName, string CategoryType, string Description)//MembershipOps objMembershipOps)
        {
            string result = string.Empty;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    var data = db.AddUpdateDeleteMembershipCategory(MembershipName, CategoryType, Description, null, null, null, null, null, null, null, null, null, "IC").FirstOrDefault();

                    result = Convert.ToString(data.result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                return result;
            }
            return result;

        }

        //Insert Membership duration details
        public static string InsertMembershipDuration(string DurationName, int DurationDays)
        {
            string result = string.Empty;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    var data = db.AddUpdateDeleteMembershipCategory(null, null, null, DurationName, DurationDays, null, null, null, null, null, null, null, "ID").FirstOrDefault();

                    result = Convert.ToString(data.result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                return result;
            }
            return result;

        }

        //Insert Membership banefit details
        public static string InsertMembershipBenefit(MembershipOps ops)
        {
            string result = string.Empty;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    var data = db.AddUpdateDeleteMembershipCategory(ops.MembershipName, null, null, null, null, ops.MembershipPrefixCode, ops.MembershipCategoryId, ops.DurationId, ops.Fees, ops.CreditLimit, ops.MaxAdult, ops.MaxChild, "IMB").FirstOrDefault();

                    result = Convert.ToString(data.result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                return result;
            }
            return result;

        }

        //Fetch Membership details
        public async Task<List<MembershipCommon>> FetchMembershipDetails(int SelectedMembershipCategory, int SelectedMembershipType)
        {
            List<MembershipCommon> listItems = new List<MembershipCommon>();
            string apiUrl = BaseUrl + Constant.FETCH_MEMBER_DETAILS;
            try
            {
                using (var client = new HttpClient())
                {
                    MemberRegisterDetailsOnRequest myModel;
                    myModel = new MemberRegisterDetailsOnRequest();
                    myModel.SelectedMembershipCategory = SelectedMembershipCategory;
                    myModel.SelectedMembershipType = SelectedMembershipType;
                    myModel.Flag = Constant.MEMBERSHIP_DETAILS_TYPE;

                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format  
                    var json = JsonConvert.SerializeObject(myModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.PostAsync(apiUrl, stringContent);

                    if (Res != null)
                    {
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var memberResponse = Res.Content.ReadAsStringAsync().Result;

                            var details = JObject.Parse(memberResponse);
                            var objResult = details["Result"].ToList();
                            if (objResult != null)
                            {
                                foreach (var item in objResult)
                                {
                                    MembershipCommon items = new MembershipCommon();
                                    items.Fees = Convert.ToDecimal(item["Fees"]);
                                    items.CreditLimit = Convert.ToDecimal(item["CreditLimit"]);
                                    listItems.Add(items);
                                }
                            }
                        }
                    }

                    return listItems;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //try
            //{
            //    using (MembershipCatEntities db = new MembershipCatEntities())
            //    {
            //        items = db.GetMembeshipDetails(SelectedMembershipCategory, SelectedMembershipType, "Type").Select(c => new MembershipCommon
            //        {
            //            Fees = Convert.ToDecimal(c.Fees),
            //            CreditLimit = Convert.ToDecimal(c.CreditLimit)
            //        }).ToList<MembershipCommon>();


            //    }
            //    //items.Insert(0, (new SelectListItem { Text = "Select Membership Type", Value = "0", Selected = true }));
            //    return items;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        //Insert new member
        public static async Task<string> RegisterMember(MemberRegisterOnRequestModel objMC, string path, string fileName)
        {
            string lastId = string.Empty;

            string APIUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
            APIUrl = APIUrl + Constant.INSERT_UPDATE_MEMBER;

            objMC.Flag = Constant.INSERT_MEMBER;

            try
            {
                //Insert new member data in table using WEB API with image upload
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format  
                    var json = JsonConvert.SerializeObject(objMC);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("fileToUpload");
                    form.Add(stringContent, "fileToUpload");
                    form.Add(content, "medicineOrder");

                    var stream = new FileStream(path, FileMode.Open);
                    content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "fileToUpload",
                        FileName = fileName
                    };
                    form.Add(content);

                    //Sending request to find web api REST service resource insert member details using HttpClient  
                    HttpResponseMessage Res = await client.PostAsync(APIUrl, form);

                    if (Res != null)
                    {
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var memberResponse = Res.Content.ReadAsStringAsync().Result;
                            var details = JObject.Parse(memberResponse);
                            var objResult = details["Result"].ToString();

                            lastId = Convert.ToString(objResult);
                            if (!string.IsNullOrEmpty(lastId))
                            {
                                return lastId;
                            }
                        }
                    }
                }

                //using (MembershipCatEntities db = new MembershipCatEntities())
                //{
                //    var data = db.InsertUpdateMemberDetails
                //        (objMC.FirstName, objMC.LastName, objMC.Address, objMC.ZipCode, objMC.City, objMC.State, objMC.Country, objMC.Gender,
                //        objMC.Comment, objMC.CategoryTypeId, objMC.Nationality, objMC.Referred, objMC.IsActive, objMC.MembershipCategoryId, objMC.MembershipName,
                //        objMC.CardNumber, objMC.MemberCode1, objMC.MemberCode2, objMC.MembershipEndDate, objMC.Fees, objMC.CreditLimit, objMC.AvailableCredit,
                //        objMC.TotalTaxAmount, objMC.NetAmount, objMC.AmountPaid, null, "I").FirstOrDefault();

                //    lastId = Convert.ToString(objResult);
                //    if (!string.IsNullOrEmpty(lastId))
                //    {
                //        return lastId;
                //    }
                //}
            }
            catch (Exception ex)
            {
                lastId = ex.Message.ToString();
                return lastId;
            }
            return lastId;
        }

        //Update photo path of new member 
        public static void UpdateMemberImagePath(string path, string Id)
        {
            int id = Convert.ToInt32(Id);
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    MemberRegister data = db.MemberRegisters.Where(x => x.Id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.ImagePath = path;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Inert all Family member details
        public static async Task<List<string>> RegisterFamilyMember(FormCollection frm, int totalFamilyCount, string lastMemberId)
        {
            string lastId = string.Empty;
            List<string> objList = new List<string>();
            string APIUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
            APIUrl = APIUrl + Constant.INSERT_UPDATE_FAMILY_MEMBER;

            try
            {
                for (int i = 1; i <= totalFamilyCount; i++)
                {
                    FamilyMemberRegisterOnRequest objMC = new FamilyMemberRegisterOnRequest();

                    objMC.Name = frm["Name_" + i];
                    objMC.BirthDate = Convert.ToDateTime(frm["BirthDate_" + i]);
                    objMC.FamilyCategoryTypeId = frm["FamilyMemberCategory_" + i];
                    objMC.BloodGroupName = frm["BloodGroup_" + i];
                    objMC.Relation = frm["Relation_" + i];

                    objMC.ContactNo = frm["ContactNo_" + i];
                    objMC.Email = frm["Email_" + i];
                    objMC.MaritulStatus = frm["MaritalStatus_" + i];
                    objMC.Age = Convert.ToInt32(frm["Age_" + i]);
                    objMC.FamilyCardNumber = frm["FamilyCardNumber_" + i];

                    string imgPath = Convert.ToString(frm["FamilyPhoto_" + i]);

                    if (imgPath != null)
                        imgPath = imgPath.Split(new string[] { "Family" }, StringSplitOptions.None)[1];

                    objMC.imagePath2 = imgPath;
                    objMC.Flag = Constant.INSERT_FAMILY_MEMBER;

                    objMC.LastRegisterMemberId = Convert.ToInt32(lastMemberId);

                    //Insert new family member data in table using WEB API
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(APIUrl);
                        client.DefaultRequestHeaders.Clear();

                        //Define request data format  
                        var json = JsonConvert.SerializeObject(objMC);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                        HttpResponseMessage Res = await client.PostAsync(APIUrl, stringContent);

                        if (Res != null)
                        {
                            if (Res.IsSuccessStatusCode)
                            {
                                //Storing the response details recieved from web api   
                                var memberResponse = Res.Content.ReadAsStringAsync().Result;
                                var details = JObject.Parse(memberResponse);
                                var objResult = details["Result"].ToString();

                                lastId = Convert.ToString(objResult);
                                if (!string.IsNullOrEmpty(lastId))
                                {
                                    objList.Add(lastId);
                                }
                            }
                        }
                    }

                    //var data = db.InsertUpdateFamilyMemberDetails
                    //(objMC.Name, objMC.BirthDate, objMC.FamilyCategoryTypeId, objMC.BloodGroupName, objMC.Relation, objMC.ContactNo, objMC.Email, objMC.MaritulStatus,
                    //objMC.Age, objMC.FamilyCardNumber, lastRegisterMemberId, objMC.imagePath2, "IF").FirstOrDefault();

                    //lastId = Convert.ToString(data.result);
                    //if (!string.IsNullOrEmpty(lastId))
                    //{
                    //    objList.Add(lastId);
                    //}
                }

                return objList;
            }
            catch (Exception ex)
            {
                lastId = ex.Message.ToString();
                objList.Add(lastId);
                return objList;
            }
        }

        //Update photo path of family member 
        public static void UpdateFamilyMemberImagePath(string path, string Id)
        {
            int id = Convert.ToInt32(Id);
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    FamilyMember data = db.FamilyMembers.Where(x => x.Id == id).FirstOrDefault();
                    if (data != null)
                    {
                        data.ImagePath = path;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Membership Category List
        public static List<MemberShipCategory> GetMembershipCategory()
        {
            List<MemberShipCategory> items = new List<MemberShipCategory>();

            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    items = db.MembershipCategories.Where(x => x.IsDeleted == false).Select(c => new MemberShipCategory
                    {
                        Id = c.Id,
                        MembershipName = c.MembershipName,
                        Category = c.CategoryType,
                        Description = c.Description
                    }).ToList<MemberShipCategory>();
                }
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Membership Duration List
        public static List<MemberShipDuration> GetMembershipDuration()
        {
            List<MemberShipDuration> items = new List<MemberShipDuration>();

            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    items = db.MembershipDurations.Where(x => x.IsDeleted == false).Select(c => new MemberShipDuration
                    {
                        Id = c.Id,
                        DurationName = c.DurationName,
                        DurationDays = c.DurationDays
                    }).ToList<MemberShipDuration>();
                }
                return items;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Get Membership Benefit List
        public static List<MemberShipBenefit> GetMembershipBenefit()
        {
            List<MemberShipBenefit> items = new List<MemberShipBenefit>();

            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    items = db.MembershipBenefits.Where(x => x.IsDeleted == false).Select(c => new MemberShipBenefit
                    {
                        Id = c.Id,
                        MembershipName = c.MembershipName,
                        MemberPrefixCode = c.MemberPrefixCode,
                        MembershipCategory = c.MembershipCategory.MembershipName,
                        Duration = c.MembershipDuration.DurationName,
                        Fees = c.Fees,
                        CreditLimit = c.CreditLimit,
                        MaxAdult = c.MaxAdult,
                        MaxChild = c.MaxChild
                    }).ToList<MemberShipBenefit>();
                }
                return items;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Get Registered Members List
        public static List<RegisteredMember> GetRegisteredMembers()
        {
            List<RegisteredMember> items = new List<RegisteredMember>();

            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    items = db.MemberRegisters.Where(x => x.IsDeleted == false).Select(c => new RegisteredMember
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Address = c.Address,
                        ZipCode = c.ZipCode,
                        City = c.City,
                        State = c.State,
                        Country = c.Country,
                        Gender = c.Gender,
                        Comment = c.Comments
                    }).ToList<RegisteredMember>();
                }
                return items;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Get Membership Category details by ID
        public static List<MemberShipCategory> GetMembershipCategoryById(int Id)
        {
            List<MemberShipCategory> items = new List<MemberShipCategory>();

            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    items = db.MembershipCategories.Where(x => x.Id == Id && x.IsDeleted == false).Select(c => new MemberShipCategory
                    {
                        MembershipName = c.MembershipName,
                        Category = c.CategoryType,
                        Description = c.Description
                    }).ToList<MemberShipCategory>();
                }
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Membership Duration details by ID
        public static List<MemberShipDuration> GetDurationDetailsById(int Id)
        {
            List<MemberShipDuration> items = new List<MemberShipDuration>();

            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    items = db.MembershipDurations.Where(x => x.Id == Id && x.IsDeleted == false).Select(c => new MemberShipDuration
                    {
                        DurationName = c.DurationName,
                        DurationDays = c.DurationDays
                    }).ToList<MemberShipDuration>();
                }
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Membership Benefit details by ID
        public static List<MemberShipBenefit> GetMembershipBenefitDetailsById(int Id)
        {
            List<MemberShipBenefit> items = new List<MemberShipBenefit>();

            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    items = db.MembershipBenefits.Where(x => x.Id == Id && x.IsDeleted == false).Select(c => new MemberShipBenefit
                    {
                        Id = c.Id,
                        MembershipName = c.MembershipName,
                        MemberPrefixCode = c.MemberPrefixCode,
                        MembershipCategoryId = c.MembershipCategoryId,
                        DurationId = c.DurationId,
                        Fees = c.Fees,
                        CreditLimit = c.CreditLimit,
                        MaxAdult = c.MaxAdult,
                        MaxChild = c.MaxChild
                    }).ToList<MemberShipBenefit>();
                }
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Registered Members details by ID
        public static async Task<List<RegisteredMember>> GetRegisteredMembersDetailsById(int? Id)
        {
            List<RegisteredMember> items = new List<RegisteredMember>();

            string APIUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
            APIUrl = APIUrl + Id;
            RegisteredMember objItem = new RegisteredMember();
            try
            {
                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(APIUrl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource Get specific member details using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync(APIUrl);

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res != null)
                    {
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var memberResponse = Res.Content.ReadAsStringAsync().Result;
                            var details = JObject.Parse(memberResponse);
                            var objResult = details["Result"].ToList();

                            objItem.Id = Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)objResult[1]).Value);
                            objItem.FirstName = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[3]).Value);
                            objItem.LastName = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[4]).Value);
                            objItem.Address = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[5]).Value);
                            objItem.ZipCode = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[6]).Value);
                            objItem.City = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[7]).Value);
                            objItem.State = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[8]).Value);
                            objItem.Country = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[9]).Value);
                            objItem.Gender = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[10]).Value);
                            objItem.Comment = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[11]).Value);
                            objItem.CategoryTypeId = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[12]).Value);
                            objItem.Nationality = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[13]).Value);
                            objItem.Referred = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[14]).Value);
                            objItem.imagePath1 = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[15]).Value);
                            objItem.IsActive = Convert.ToBoolean(((Newtonsoft.Json.Linq.JProperty)objResult[16]).Value);
                            objItem.CategoryId = Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)objResult[17]).Value);
                            objItem.MembershipId = Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)objResult[18]).Value);
                            objItem.CardNumber = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[19]).Value);
                            objItem.MemberCode1 = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[20]).Value);
                            objItem.MemberCode2 = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)objResult[21]).Value);

                            objItem.MembershipEndDate = Convert.ToDateTime(((Newtonsoft.Json.Linq.JProperty)objResult[22]).Value);
                            objItem.Fees = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[23]).Value);
                            objItem.CreditLimit = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[24]).Value);
                            objItem.TotalTaxAmount = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[25]).Value);
                            objItem.NetAmount = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[26]).Value);
                            objItem.AmountPaid = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[27]).Value);
                            objItem.AvailableCredit = Convert.ToDecimal(((Newtonsoft.Json.Linq.JProperty)objResult[28]).Value);

                            var objFamily2 = objResult[0].ToList();

                            foreach (var item in objFamily2.Children())
                            {
                                FamilyRegisteredMember objList = new FamilyRegisteredMember();

                                objList.FamilyId = Convert.ToInt32(item["Id"]);
                                objList.Name = Convert.ToString(item["Name"]);
                                objList.BirthDate = Convert.ToDateTime(item["BirthDate"]);
                                objList.CategoryTypeId = Convert.ToString(item["CategoryTypeId"]);
                                objList.BloodGroupId = Convert.ToInt32(item["BloodGroupId"]);
                                objList.BloodGroupName = Convert.ToString(item["BloodGroupName"]);
                                objList.Relation = Convert.ToString(item["Relation"]);
                                objList.ContactNo = Convert.ToString(item["ContactNo"]);
                                objList.Email = Convert.ToString(item["Email"]);
                                objList.MaritulStatus = Convert.ToString(item["MaritulStatus"]);
                                objList.Age = Convert.ToInt32(item["Age"]);
                                objList.FamilyCardNumber = Convert.ToString(item["FamilyCardNumber"]);
                                objList.imagePath2 = Convert.ToString(item["imagePath2"]);

                                objItem.familyItems.Add(objList);
                            }
                        }
                    }
                }
                items.Add(objItem);
                return items;

                #region Old Code
                //using (MembershipCatEntities db = new MembershipCatEntities())
                //{
                //    items = db.MemberRegisters.Where(x => x.IsDeleted == false && x.Id == Id).Select(c => new RegisteredMember
                //    {
                //        Id = c.Id,
                //        FirstName = c.FirstName,
                //        LastName = c.LastName,
                //        Address = c.Address,
                //        ZipCode = c.ZipCode,
                //        City = c.City,
                //        State = c.State,
                //        Country = c.Country,
                //        Gender = c.Gender,
                //        Comment = c.Comments,
                //        CategoryTypeId = c.Category,
                //        Nationality = c.Nationality,
                //        Referred = c.ReferredBy,
                //        imagePath1 = c.ImagePath,
                //        IsActive = c.IsActive,
                //        CategoryId = c.MembershipCategoryId,
                //        MembershipId = c.MembershipName,
                //        CardNumber = c.CardNumber,
                //        MemberCode1 = c.MemberCode1,
                //        MemberCode2 = c.MemberCode2,

                //        MembershipEndDate = c.MembershipEndDate,
                //        Fees = c.Fees,
                //        CreditLimit = c.CreditLimit,
                //        TotalTaxAmount = c.TotalTaxAmount,
                //        NetAmount = c.NetAmount,
                //        AmountPaid = c.AmountPaid,
                //        AvailableCredit = c.AvailableCredit,
                //        familyItems = db.FamilyMembers.Where(x => x.IsDeleted == false && x.MemberRegisterId == Id).Select(y => new FamilyRegisteredMember
                //        {
                //            FamilyId = y.Id,
                //            Name = y.Name,
                //            BirthDate = y.BirthDate,
                //            CategoryTypeId = y.Category,
                //            BloodGroupId = y.BloodGroupId,
                //            BloodGroupName = y.MasterBloodGroup.BloodGroup,
                //            Relation = y.Relation,
                //            ContactNo = y.ContactNo,
                //            Email = y.Email,
                //            MaritulStatus = y.MaritalStatus,
                //            Age = y.Age,
                //            FamilyCardNumber = y.CardNo,
                //            imagePath2 = y.ImagePath
                //        }).ToList<FamilyRegisteredMember>(),
                //    }).ToList<RegisteredMember>();
                //    //y.ImagePath != null ? y.ImagePath.Split(new string[] { "Family" }, StringSplitOptions.None)[1] : null

                //    //Call web api method


                //}
                //return items;

                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Hosted web API REST Service base url 
        //public async Task<List<RegisteredMember>> Index()
        //{
        //    string Baseurl = "http://localhost:55221/api/MemberRegister/20";
        //    List<RegisteredMember> EmpInfo = new List<RegisteredMember>();

        //    using (var client = new HttpClient())
        //    {
        //        //Passing service base url  
        //        client.BaseAddress = new Uri(Baseurl);

        //        client.DefaultRequestHeaders.Clear();
        //        //Define request data format  
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
        //        HttpResponseMessage Res = await client.GetAsync(Baseurl);

        //        //Checking the response is successful or not which is sent using HttpClient  
        //        if (Res.IsSuccessStatusCode)
        //        {
        //            //Storing the response details recieved from web api   
        //            var EmpResponse = Res.Content.ReadAsStringAsync().Result;

        //            //Deserializing the response recieved from web api and storing into the Employee list  
        //            EmpInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RegisteredMember>>(EmpResponse);

        //        }
        //        return EmpInfo;
        //        //returning the employee list to view  
        //        //return View(EmpInfo);
        //    }
        //}

        //Delete Membership Category deails by ID

        public static int DeleteMembershipCategory(int Id)
        {
            int status = 0;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    MembershipCategory data = db.MembershipCategories.Where(x => x.Id == Id).FirstOrDefault();
                    if (data != null)
                    {
                        data.IsDeleted = true;
                        data.UpdatedBy = 1;
                        data.UpdatedOn = DateTime.Now;

                        db.SaveChanges();

                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                status = 1;
            }
            return status;
        }

        //Delete Membership Duration details by ID
        public static int DeleteMembershipDuration(int Id)
        {
            int status = 0;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    MembershipDuration data = db.MembershipDurations.Where(x => x.Id == Id).FirstOrDefault();
                    if (data != null)
                    {
                        data.IsDeleted = true;
                        data.UpdatedBy = 1;
                        data.UpdatedOn = DateTime.Now;

                        db.SaveChanges();

                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                status = 1;
            }
            return status;
        }

        //Delete Membership Benefit details by ID
        public static int DeleteMembershipBenefit(int Id)
        {
            int status = 0;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    MembershipBenefit data = db.MembershipBenefits.Where(x => x.Id == Id).FirstOrDefault();
                    if (data != null)
                    {
                        data.IsDeleted = true;
                        data.UpdatedBy = 1;
                        data.UpdatedOn = DateTime.Now;

                        db.SaveChanges();

                        status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                status = 1;
            }
            return status;
        }

        //Delete any registered member by SP
        public async Task<string> DeleteMembershipDetails(int Id, string Type)
        {
            string status = string.Empty;
            string APIUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
            APIUrl = APIUrl + Constant.DELETE_MEMBERSHIP_DETAILS;

            try
            {
                MemberRegisterDeleteOnRequest objMD = new MemberRegisterDeleteOnRequest();
                objMD.Id = Id;
                objMD.Flag = Type;

                //delete member in table using WEB API
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format  
                    var json = JsonConvert.SerializeObject(objMD);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.PostAsync(APIUrl, stringContent);

                    if (Res != null)
                    {
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var memberResponse = Res.Content.ReadAsStringAsync().Result;
                            var details = JObject.Parse(memberResponse);
                            var objResult = details["Result"].ToString();

                            status = Convert.ToString(objResult);
                            if (!string.IsNullOrEmpty(status))
                            {
                                return status;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                status = ex.Message.ToString();

                return status;
            }
            return status;

            #region Old Code
            //try
            //{
            //    using (MembershipCatEntities db = new MembershipCatEntities())
            //    {
            //        var data = db.spDeleteMembershipDetails(Id, Type).FirstOrDefault();

            //        status = Convert.ToInt32(data.Status);



            //        //MembershipBenefit data = db.MembershipBenefits.Where(x => x.Id == Id).FirstOrDefault();
            //        //if (data != null)
            //        //{
            //        //    data.IsDeleted = true;
            //        //    data.UpdatedBy = 1;
            //        //    data.UpdatedOn = DateTime.Now;

            //        //    db.SaveChanges();

            //        //    status = 1;
            //        //}
            //    }
            //}
            //catch (Exception ex)
            //{
            //    status = 1;
            //}
            //return status;
            #endregion
        }

        //Update Membership Category details by ID
        public static string UpdateCategorydetails(string MembershipName, string CategoryType, string Description, int Id)//MembershipOps objMembershipOps)
        {
            string result = string.Empty;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    var data = db.AddUpdateDeleteMembershipCategory(MembershipName, CategoryType, Description, null, null, null, Id, null, null, null, null, null, "ICU").FirstOrDefault();

                    result = Convert.ToString(data.result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                return result;
            }
            return result;

        }

        //Update Membership Duration details by ID
        public static string UpdateDurationdetails(string DurationName, int DurationDays, int Id)//MembershipOps objMembershipOps)
        {
            string result = string.Empty;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    var data = db.AddUpdateDeleteMembershipCategory(null, null, null, DurationName, DurationDays, null, null, Id, null, null, null, null, "DU").FirstOrDefault();

                    result = Convert.ToString(data.result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                return result;
            }
            return result;

        }

        //Update Membership Benefit details by ID
        public static string UpdateMembershipBenefitDetails(MemberShipBenefit ops)
        {
            string result = string.Empty;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    var data = db.AddUpdateDeleteMembershipCategory(ops.MembershipName, null, null, null, ops.Id, ops.MemberPrefixCode, ops.MembershipCategoryId, ops.DurationId, ops.Fees, ops.CreditLimit, ops.MaxAdult, ops.MaxChild, "UMB").FirstOrDefault();

                    result = Convert.ToString(data.result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                return result;
            }
            return result;

        }

        //Update Family Member details
        public async Task<string> UpdateFamilyMemberDetails(FamilyMemberRegisterOnRequest objMC)
        {
            string lastId = string.Empty;
            //List<string> objList = new List<string>();
            string APIUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
            APIUrl = APIUrl + Constant.INSERT_UPDATE_FAMILY_MEMBER;

            objMC.Flag = Constant.UPDATE_FAMILY_MEMBER;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    //update family member data in table using WEB API
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(APIUrl);
                        client.DefaultRequestHeaders.Clear();

                        //Define request data format  
                        var json = JsonConvert.SerializeObject(objMC);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                        HttpResponseMessage Res = await client.PostAsync(APIUrl, stringContent);

                        if (Res != null)
                        {
                            if (Res.IsSuccessStatusCode)
                            {
                                //Storing the response details recieved from web api   
                                var memberResponse = Res.Content.ReadAsStringAsync().Result;
                                var details = JObject.Parse(memberResponse);
                                var objResult = details["Result"].ToString();

                                lastId = Convert.ToString(objResult);
                                if (!string.IsNullOrEmpty(lastId))
                                {
                                    return lastId;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lastId = ex.Message.ToString();

                return lastId;
            }
            return lastId;

            #region Old Code
            //string result = string.Empty;
            //try
            //{
            //    using (MembershipCatEntities db = new MembershipCatEntities())
            //    {
            //        var data = db.InsertUpdateFamilyMemberDetails(ops.Name, ops.BirthDate, ops.CategoryTypeId, ops.BloodGroupName, ops.Relation, ops.ContactNo, ops.Email, ops.MaritulStatus, ops.Age, ops.FamilyCardNumber, ops.FamilyId, null, "UF").FirstOrDefault();

            //        result = Convert.ToString(data.result);
            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            return result;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result = ex.Message.ToString();
            //    return result;
            //}
            //return result;
            #endregion
        }

        //Insert Family Member details
        public async Task<string> InsertFamilyMemberDetails(FamilyMemberRegisterOnRequest objMC, string type)
        {
            string lastId = string.Empty;
            //List<string> objList = new List<string>();
            string APIUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
            APIUrl = APIUrl + Constant.INSERT_UPDATE_FAMILY_MEMBER;

            if (type == Constant.INSERT_FAMILY_MEMBER)
                objMC.Flag = Constant.INSERT_FAMILY_MEMBER;
            else if (type == Constant.UPDATE_FAMILY_MEMBER)
                objMC.Flag = Constant.UPDATE_FAMILY_MEMBER;

            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    //Insert new family member data in table using WEB API
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(APIUrl);
                        client.DefaultRequestHeaders.Clear();

                        //Define request data format  
                        var json = JsonConvert.SerializeObject(objMC);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                        HttpResponseMessage Res = await client.PostAsync(APIUrl, stringContent);

                        if (Res != null)
                        {
                            if (Res.IsSuccessStatusCode)
                            {
                                //Storing the response details recieved from web api   
                                var memberResponse = Res.Content.ReadAsStringAsync().Result;
                                var details = JObject.Parse(memberResponse);
                                var objResult = details["Result"].ToString();

                                lastId = Convert.ToString(objResult);
                                if (!string.IsNullOrEmpty(lastId))
                                {
                                    return lastId;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lastId = ex.Message.ToString();

                return lastId;
            }
            return lastId;

            #region Old Code
            //string result = string.Empty;
            //try
            //{
            //    using (MembershipCatEntities db = new MembershipCatEntities())
            //    {
            //        var data = db.InsertUpdateFamilyMemberDetails(ops.Name, ops.BirthDate, ops.CategoryTypeId, ops.BloodGroupName, ops.Relation, ops.ContactNo, ops.Email, ops.MaritulStatus, ops.Age, ops.FamilyCardNumber, ops.Id, ops.imagePath2, "IF").FirstOrDefault();

            //        result = Convert.ToString(data.result);
            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            return result;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result = ex.Message.ToString();
            //    return result;
            //}
            //return result;
            #endregion
        }

        //Update member with file upload
        public static async Task<string> UpdateRegisterMember(MemberRegisterOnRequestModel objMC, string tempFilePath, string tempfileName)
        {
            string lastId = string.Empty;
            string APIUrl = string.Empty;
            if (!string.IsNullOrEmpty(tempFilePath))
            {
                APIUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
                APIUrl = APIUrl + Constant.INSERT_UPDATE_MEMBER;
            }
            else
            {
                APIUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
                APIUrl = APIUrl + Constant.UPDATE_REGISTER_MEMBER;
            }

            objMC.Flag = Constant.UPDATE_MEMBER;

            try
            {
                //update member data in table using WEB API
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format  
                    var json = JsonConvert.SerializeObject(objMC);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = new HttpResponseMessage();

                    if (!string.IsNullOrEmpty(tempFilePath))
                    {
                        MultipartFormDataContent form = new MultipartFormDataContent();
                        HttpContent content = new StringContent("fileToUpload");
                        form.Add(stringContent, "fileToUpload");
                        form.Add(content, "medicineOrder");

                        var stream = new FileStream(tempFilePath, FileMode.Open);
                        content = new StreamContent(stream);
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "fileToUpload",
                            FileName = tempfileName
                        };
                        form.Add(content);

                        //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                        Res = await client.PostAsync(APIUrl, form);
                    }
                    else
                    {
                        //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                        Res = await client.PostAsync(APIUrl, stringContent);
                    }

                    if (Res != null)
                    {
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var memberResponse = Res.Content.ReadAsStringAsync().Result;
                            var details = JObject.Parse(memberResponse);
                            var objResult = details["Result"].ToString();

                            lastId = Convert.ToString(objResult);
                            if (!string.IsNullOrEmpty(lastId))
                            {
                                return lastId;
                            }
                        }
                    }
                }
            }

            #region Old Code
            //string lastId = string.Empty;
            //try
            //{
            //    using (MembershipCatEntities db = new MembershipCatEntities())
            //    {
            //        var data = db.InsertUpdateMemberDetails
            //            (objMC.FirstName, objMC.LastName, objMC.Address, objMC.ZipCode, objMC.City, objMC.State, objMC.Country, objMC.Gender,
            //            objMC.Comment, objMC.CategoryTypeId, objMC.Nationality, objMC.Referred, objMC.IsActive, objMC.CategoryId, objMC.MembershipId,
            //            objMC.CardNumber, objMC.MemberCode1, objMC.MemberCode2, objMC.MembershipEndDate, objMC.Fees, objMC.CreditLimit, objMC.AvailableCredit,
            //            objMC.TotalTaxAmount, objMC.NetAmount, objMC.AmountPaid, objMC.Id, "U").FirstOrDefault();                    

            //        lastId = Convert.ToString(objMC.Id);
            //        if (!string.IsNullOrEmpty(lastId))
            //        {
            //            return lastId;
            //        }
            //    }
            //}
            #endregion

            catch (Exception ex)
            {
                lastId = ex.Message.ToString();
                return lastId;
            }
            return lastId;
        }

        //Get Image list
        public static List<ActorActressImageList> GetImageList(string searchType, string sortType, int pageNo)
        {
            int pageSize = Constant.PAGE_NO;
            string lastId = string.Empty;

            List<ActorActressImageList> objList = new List<ActorActressImageList>();
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    ObjectParameter objTotalCount = new ObjectParameter("pTotalCount", typeof(int));

                    objList = db.spGetActorActressImageDetails(searchType, sortType, pageNo, pageSize, null, null, objTotalCount).Select(x => new ActorActressImageList
                    {
                        ImageId = x.Id,
                        StarName = x.StarName,
                        StarType = x.StarType,
                        ImagePath = x.ImagePath,
                        //TotalCount = objTotalCount.Value
                    }).ToList<ActorActressImageList>();

                    ActorActressImageList obj = new ActorActressImageList();
                    obj.TotalCount = Convert.ToInt32(objTotalCount.Value);
                    objList.Insert(0, obj);

                    // MembershipCommon objMC = new MembershipCommon();
                    //objMC.TotalCount = Convert.ToInt32(objTotalCount.Value);



                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objList;
        }

        //Get Image Total Count
        public static int GetImageTotalCount()
        {
            int tCount = 0;
            try
            {
                using (MembershipCatEntities db = new MembershipCatEntities())
                {
                    tCount = db.tblActerActressLists.Count();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tCount;
        }

        //Upload family photo
        public static async Task<string> UploadFamilyPhoto(string file, string path)
        {
            string lastId = string.Empty;

            string APIUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
            APIUrl = APIUrl + Constant.FAMILY_UPLOAD_FILE;

            try
            {
                //Upload family photo in table using WEB API
                using (var client = new HttpClient())
                {                    
                    client.BaseAddress = new Uri(APIUrl);
                    client.DefaultRequestHeaders.Clear();

                    //Define request data format  
                    //var json = JsonConvert.SerializeObject(objMC);
                    //var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent content = new StringContent("fileToUpload");
                    //form.Add(stringContent, "fileToUpload");
                    form.Add(content, "medicineOrder");

                    var stream = new FileStream(file, FileMode.Open);
                    content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "fileToUpload",
                        FileName = path
                    };
                    form.Add(content);
                    
                    //Sending request to find web api REST service resource to upload image using HttpClient  
                    HttpResponseMessage Res = await client.PostAsync(APIUrl, form);

                    if (Res != null)
                    {
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var memberResponse = Res.Content.ReadAsStringAsync().Result;
                            var details = JObject.Parse(memberResponse);
                            var objResult = details["Result"].ToString();

                            lastId = Convert.ToString(objResult);
                            if (!string.IsNullOrEmpty(lastId))
                            {
                                return lastId;
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                lastId = ex.Message.ToString();
                return lastId;
            }
            return lastId;
        }
    }
}
