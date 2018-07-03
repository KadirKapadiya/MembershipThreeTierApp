using MemberShipMVCApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

using static DAL.Model.MembershipCommon;

namespace MemberShipMVCApp.Controllers
{
    public class ImageDetailsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
