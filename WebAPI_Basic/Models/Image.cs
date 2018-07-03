using DAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DAL.Model.MembershipCommon;

namespace WebAPI_Basic.Models
{
    public class Image
    {
        public string ImageName { get; set; }

        public int TotalCount { get; set; }

        
        public static List<ActorActressImageList> GetImageList(string searchType, string sortType, int pageNo)
        {
            List<ActorActressImageList> objList = new List<ActorActressImageList>();

            int pageSize = Common.Constant.PAGE_NO;

            //objList = MembershipOps.GetImageList(searchType, sortType, pageNo, pageSize);

            //int tCount = MembershipOps.GetImageTotalCount();

            //TotalCount = Convert.ToInt32(tCount);

            return objList;
        }

        public static int GetImageTotalCount()
        {
            int totalCount = MembershipOps.GetImageTotalCount();

            return totalCount;
        }
        
    }
}