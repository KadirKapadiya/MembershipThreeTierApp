using DAL.Model;
using MemberShipMVCApp.ViewModel;
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
using System.Web.Script.Serialization;
using static DAL.Model.MembershipCommon;
using static MemberShipMVCApp.ViewModel.MemberRegisterViewModel;

namespace MemberShipMVCApp.Controllers
{
    public class MemberRegisterController : Controller
    {
        MemberRegisterViewModel vm = new MemberRegisterViewModel();
        public MemberRegisterController(){}

        public MemberRegisterController(MemberRegisterViewModel memberRegisterViewModel) {
            this.vm = memberRegisterViewModel;
        }

        // GET: MembershipCategory
        public async Task<ActionResult> Index()
        {
            //MemberRegisterViewModel vm = new MemberRegisterViewModel();

            if (TempData["Message"] != null)
            {
                vm.message = TempData["Message"].ToString();
            }

            int result = await vm.CallAPIForBindDropdown();
            
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Index(MemberRegisterViewModel vm, FormCollection frm)
        {
            try
            {
                string result = string.Empty;

                //var personalPhotoPath = Server.MapPath("~/Content/Personal");
                string personalPhotoPath = Server.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings["MemberPhotoPath"].ToString());
                //var familyPhotoPath = Server.MapPath("~/Content/Family");
                
                string tempPhotoPath = Server.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings["TempPhotoPath"].ToString());

                //Insert new register member and family details
                //result = await vm.RegisterMember(vm, frm, personalPhotoPath, familyPhotoPath, tempPhotoPath);
                result = await vm.RegisterMember(vm, frm, personalPhotoPath, tempPhotoPath);

                TempData["Message"] = result;
                return RedirectToAction("Dashboard", "MemberShip");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
                return RedirectToAction("Index", "MemberRegister");
            }
        }

        //public ActionResult Edit(int? Id)
        //{
        //    MemberRegisterViewModel vm = new MemberRegisterViewModel();

        //    vm.GetRegisteredMembersDetailsById(Id);

        //    if (TempData["Message"] != null)
        //    {
        //        vm.message = TempData["Message"].ToString();
        //    }


        //    return View(vm);
        //}

        public async Task<ActionResult> Edit(int? Id)
        {
            //MemberRegisterViewModel vm = new MemberRegisterViewModel();

            int result = await vm.CallAPIForBindDropdown();

            int getResult = await vm.GetRegisteredMembersDetailsById(Id);
            
            if (TempData["Message"] != null)
            {
                vm.message = TempData["Message"].ToString();
            }
            return View(vm);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(MemberRegisterViewModel vm)
        {
            string result = string.Empty;
            //var personalPhotoPath = Server.MapPath("~/Content/Personal");
            string personalPhotoPath = Server.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings["MemberPhotoPath"].ToString());
            string tempPhotoPath = Server.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings["TempPhotoPath"].ToString());

            //Insert new register member and family details
            result = await vm.UpdateRegisterMember(vm, personalPhotoPath, tempPhotoPath);//, frm, personalPhotoPath, familyPhotoPath);

            TempData["Message"] = result;

            return RedirectToAction("Dashboard", "Membership");

            //return View(vm);
        }


        public async Task<JsonResult> FetchMembershipType(int SelectedMembershipCategory)
        {
            string result = string.Empty;
            try
            {
                //MemberRegisterViewModel vm = new MemberRegisterViewModel();
                result = await vm.FetchMembershipType(SelectedMembershipCategory);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> FetchMembershipDetails(int SelectedMembershipCategory, int SelectedMembershipType)
        {

            List<MembershipCommon> objList = new List<MembershipCommon>();
            try
            {
                //MemberRegisterViewModel vm = new MemberRegisterViewModel();
                objList = await vm.FetchMembershipDetails(SelectedMembershipCategory, SelectedMembershipType);
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return Json(objList, JsonRequestBehavior.AllowGet);
        }
                
        [HttpPost]
        public async Task<ActionResult> UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                string fname = string.Empty;
                string result = string.Empty;
                try
                {
                    //MemberRegisterViewModel vm = new MemberRegisterViewModel();
                    //int result = await vm.UploadFamilyPhoto(Request.Files);

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];


                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  

                        string familyPhotoPath = Server.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings["FamilyPhotoPath"].ToString());


                        //string tempPhotoPath = Server.MapPath(System.Web.Configuration.WebConfigurationManager.AppSettings["TempPhotoPath"].ToString());
                        string photoName = Path.Combine(fname.Split('.')[0] + "_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second) + "." + fname.Split('.')[1];
                        fname = Path.Combine(familyPhotoPath, fname.Split('.')[0] + "_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second) + "." + fname.Split('.')[1];
                        file.SaveAs(fname);

                        //Insert new register member and family details
                        result = await vm.UploadFamilyPhoto(fname, photoName);

                        //if(!string.IsNullOrEmpty(result))
                        //{
                        //    if ((System.IO.File.Exists(tempPhotoPath)))
                        //    {
                        //        System.IO.File.Delete(tempPhotoPath);
                        //    }
                        //}
                    }
                    // Returns message that successfully uploaded  
                    return Json(fname);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> UpdateFamilyMemberDetails(MemberRegisterViewModel vm)
        {
            string result = string.Empty;
            try
            {
                result = await vm.UpdateFamilyMemberDetails(vm);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> InsertFamilyMemberDetails(MemberRegisterViewModel vm)
        {
            string result = string.Empty;
            try
            {
                result = await vm.InsertFamilyMemberDetails(vm);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        



    }
}