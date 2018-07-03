using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using DAL.Model;
using MemberShipMVCApp.Controllers;
using MemberShipMVCApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MemberShipMVCAppTest
{
    [TestClass]
    public class MembershipControllerTest
    {
        private Mock<MembershipViewModel> mockMembershipModel = new Mock<MembershipViewModel>();
        MembershipController membershipController = new MembershipController();

        [TestInitialize]
        public void Initialize()
        {
            ConfigurationManager.AppSettings.Set("APILocation", "1111111");
            ConfigurationManager.AppSettings.Set("MembershipCatEntities", "1111111");
        }

        [TestMethod]
        public void GetCategoryDetailsSuccess()
        {
            mockMembershipModel = new Mock<MembershipViewModel>();

            membershipController = new MembershipController(mockMembershipModel.Object);
             
            List<MembershipCommon.MemberShipCategory> lstMembershipCategories = new List<MembershipCommon.MemberShipCategory>();
            MembershipCommon.MemberShipCategory membershipCategory = new MembershipCommon.MemberShipCategory();
            membershipCategory.Id = 2;
            lstMembershipCategories.Add(membershipCategory);

            mockMembershipModel.Setup(m => m.GetMembershipCategoryById(It.IsAny<int>())).Returns(lstMembershipCategories);

            JsonResult actionResult = membershipController.GetCategoryDetails(2);

            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void GetCategoryDetailsException()
        {
            mockMembershipModel = new Mock<MembershipViewModel>();

            membershipController = new MembershipController(mockMembershipModel.Object);

            mockMembershipModel.Setup(m => m.GetMembershipCategoryById(It.IsAny<int>())).Throws(new Exception());

            JsonResult actionResult = membershipController.GetCategoryDetails(2);
        }
    }
}
