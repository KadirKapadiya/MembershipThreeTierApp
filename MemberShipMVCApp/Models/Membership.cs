using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberShipMVCApp.Models
{
    public class Membership
    {
        public class MembershipCategory
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string CategoryTypeId { get; set; }            
        }
    }
}