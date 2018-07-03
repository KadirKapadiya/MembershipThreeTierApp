using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Constant
    {
        public static string INSERT_MEMBER = "I";
        public static string UPDATE_MEMBER = "U";

        public static string INSERT_FAMILY_MEMBER = "IF";
        public static string UPDATE_FAMILY_MEMBER = "UF";

        public static string BIND_DROPDOWN = "BindDropDowns";

        public static string CATEGORY = "C";
        public static string DURATION = "D";
        public static string REFERRED = "M";
        public static string BLOOD_GROUP = "BG";
        public static string MEMBERSHIP_TYPE = "MT";
        public static string MEMBERSHIP_DETAILS_TYPE = "Type";


        public static string FETCH_MEMBER_DETAILS = "FetchMemberDetails";
        public static string INSERT_UPDATE_MEMBER = "InsertUpdateMember";
        public static string UPDATE_REGISTER_MEMBER = "UpdateRegisterMember";

        public static string INSERT_UPDATE_FAMILY_MEMBER = "InsertUpdateFamilyMember";
        public static string DELETE_MEMBERSHIP_DETAILS = "DeleteMembershipDetails";

        public static string FAMILY_UPLOAD_FILE = "FamilyUploadFile";


        //Image types
        public static int PAGE_NO = 10;

        public static int PER_ROW_IMAGE = 5;

        public static int PER_PAGE_ROW = 5;

        public static string NEXT_PAGE = "Next";
        public static string PREVIOUS_PAGE = "Previous";

        public static string PAGING = "Page";

        public static string PAGE_LIST = "List";
        public static string PAGE_ALL = "All";
        public static string PAGE_ATOZ = "AtoZ";
        
    }
}
