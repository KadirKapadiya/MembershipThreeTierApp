using MemberShipMVCApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static DAL.Model.MembershipCommon;

namespace MemberShipMVCApp.Controllers
{
    public class MembershipController : Controller
    {
        MembershipViewModel vm = new MembershipViewModel();

        // GET: MembershipCategory
        public MembershipController()
        { }

        public MembershipController(MembershipViewModel membershipViewModel)
        {
            this.vm = membershipViewModel;
        }

        public async Task<ActionResult> Dashboard()
        {
            string BaseUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["APILocation"].ToString();
            //MembershipViewModel vm = new MembershipViewModel();

            if (TempData["Message"] != null)
            {
                vm.message = Convert.ToString(TempData["Message"]);
            }

            vm.GetMembershipCategory();

            vm.GetMembershipDuration();

            vm.GetMembershipBenefit();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync(BaseUrl);

                vm.GetRegisteredMembers(Res);
            }

            int result = await vm.CallAPIForBindDropdown();

            //

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Dashboard(MembershipViewModel vm)
        {
            switch (vm.Operation)
            {
                case "DELETE":
                    int status = 0;

                    status = await vm.DeleteMembershipDetails(vm);

                    //if (vm.Type == "C")
                    //{                        
                    //    status = vm.DeleteMembershipCategory(vm.MembershipCategoryId);
                    //}
                    //else if (vm.Type == "D")
                    //{
                    //    status = vm.DeleteMembershipDuration(vm.MembershipCategoryId);
                    //}
                    //else if (vm.Type == "B")
                    //{
                    //    status = vm.DeleteMembershipBenefit(vm.MembershipCategoryId);
                    //}
                    //vm.GetMembershipCategory();
                    if (status == 1)
                    {
                        TempData["Message"] = "Record/s  deleted successfully.";
                    }
                    else
                    {
                        TempData["Message"] = "Data delete failure.";
                    }

                    return RedirectToAction("Dashboard", "Membership");

                    break;
                case "EDIT":
                    return RedirectToAction("Edit", "MemberRegister", new { Id = vm.Id });
                    break;
                default:
                    break;
            }

            return View(vm);
        }

        public ActionResult EditCategory(int Id)
        {
            //vm = new MembershipViewModel();
            vm.GetMembershipCategoryById(Id);

            return RedirectToAction("Dashboard", "Membership");
        }

        public async Task<ActionResult> Index()
        {
            //MembershipViewModel vm = new MembershipViewModel();
            
            if (TempData["message"] != null)
            {
                vm.message = TempData["message"].ToString();
            }

            return View(vm);
        }

        //[HttpPost]
        //public ActionResult Index(MembershipViewModel vm)
        //{
        //    try
        //    {
        //        string result = string.Empty;
        //        result = vm.InsertMembershipCategory(vm);

        //        TempData["message"] = result;
        //        return RedirectToAction("Index", "Membership");
        //    }
        //    catch(Exception ex)
        //    {
        //        TempData["message"] = ex.Message.ToString();
        //        return RedirectToAction("Index", "Membership");
        //    }
        //}

        public JsonResult InsertCategorydetails(string MembershipName, string CategoryType, string Description)
        {
            string result = string.Empty;
            try
            {
                //MembershipViewModel vm = new MembershipViewModel();
                result = vm.InsertMembershipCategory(MembershipName, CategoryType, Description);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertDurationdetails(string DurationName, int DurationDays)
        {
            string result = string.Empty;
            try
            {
                //MembershipViewModel vm = new MembershipViewModel();
                result = vm.InsertMembershipDuration(DurationName, DurationDays);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult InsertMembershipBenefitDetails(MembershipViewModel vm)//string MembershipName, string MembershipPrefixCode, int MembershipCategoryId,
                                                                                //int DurationId, string Fees, string CreditLimit, int MaxAdult, int MaxChild)
        {
            string result = string.Empty;
            try
            {
                result = vm.InsertMembershipBenefit(vm);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult DeleteMembershipCategory(int Id)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        MembershipViewModel vm = new MembershipViewModel();
        //        result = vm.DeleteMembershipCategory(Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message.ToString();
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetCategoryDetails(int Id)
        {
            List<MemberShipCategory> result = new List<MemberShipCategory>();
            try
            {
//                vm = new MembershipViewModel();
                result = vm.GetMembershipCategoryById(Id);
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
                throw;                  
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCategorydetails(string MembershipName, string CategoryType, string Description, int Id)
        {
            string result = string.Empty;
            try
            {
                //MembershipViewModel vm = new MembershipViewModel();
                result = vm.UpdateCategorydetails(MembershipName, CategoryType, Description, Id);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateDurationdetails(string DurationName, int DurationDays, int Id)
        {
            string result = string.Empty;
            try
            {
                //MembershipViewModel vm = new MembershipViewModel();
                result = vm.UpdateDurationdetails(DurationName, DurationDays, Id);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateMembershipBenefitDetails(MembershipViewModel vm)
        {
            string result = string.Empty;
            try
            {
                result = vm.UpdateMembershipBenefitDetails(vm);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDurationDetails(int Id)
        {
            List<MemberShipDuration> result = new List<MemberShipDuration>();
            try
            {
                //MembershipViewModel vm = new MembershipViewModel();
                result = vm.GetDurationDetails(Id);
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
                //result.Add(str);
                //return result;  
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMembershipBenefitDetails(int Id)
        {
            List<MemberShipBenefit> result = new List<MemberShipBenefit>();
            try
            {
                //MembershipViewModel vm = new MembershipViewModel();
                result = vm.GetMembershipBenefitDetails(Id);
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
                //result.Add(str);
                //return result; 
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}