using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI_Basic.Models;
using static DAL.Model.MembershipCommon;

namespace WebAPI_Basic.Controllers
{
    public class ImageDetailsController : ApiController
    {
        [HttpGet]
        public IEnumerable<ActorActressImageList> GetAllImageDetails(string searchType, string sortType, int pageNo)
        {
            List<ActorActressImageList> objList = new List<ActorActressImageList>();

            objList = Image.GetImageList(searchType, sortType, pageNo);

           // int totalCount = Image.GetImageTotalCount();

            //ActorActressImageList obj = new ActorActressImageList();
            //obj.TotalCount = totalCount;
            //objList.Add(obj);
            //Image ST = new Image();
            //List<Image> li = new List<Image>();

            //ST.ImageName = "Salman";            

            //li.Add(ST);

            return objList;

        }
                
        // GET: api/CarDetails/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CarDetails
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/CarDetails/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CarDetails/5
        public void Delete(int id)
        {
        }

        
    }
}
