using System;
using System.Collections.Generic;
using ISP.BLL.DTO.Domain;
using ISP.BLL.DTO.Identity;
using ISP.BLL.Interfaces;
using ISP.BLL.Services;
using ISP.DAL.Enums;
using ISP.DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ISP.Tests.Services
{
    [TestClass]
    public class UserServicesServiceTest
    {
        private string TestUserName { get; set; }

        private UserDTO TestUserDTO { get; set; }

        private ServiceDTO TestServiceDTO { get; set; }

        private ICollection<UserPlanDTO> TestUserPlans { get; set; }

        private DAL.Models.Domain.UserService TestUserService { get; set; }

        public void Initialize()
        {
            TestUserName = "testuser";

            TestUserDTO = new UserDTO()
            {
                Id = "guid",
                UserName = TestUserName,
                Address = "",
                LastName = "",
                Balance = 0.0,
                FirstName = "",
                Email = "",
                AdminBanned = false,
                Role = "",
                Password = ""
            };
            TestServiceDTO = new ServiceDTO()
            {
                Id = 1,
                Description = "service descr",
                Title = "service title",
                UserServices = new List<UserServiceDTO>(),
                Plans = new List<PlanDTO>()
            };
            TestUserService = new DAL.Models.Domain.UserService()
            {
                Id = 1,
                Status = ServiceStatus.Active,
                IsDeleted = false
            };
            TestUserPlans = new List<UserPlanDTO>()
            {
            };
        }

        [TestMethod]
        public void SubscribeSubscribedUserToServiceTest()
        {
            Initialize();

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(t => t.GetUser(TestUserName)).Returns(TestUserDTO);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(t => t.UserServicesRepository.Find(It.IsAny<Func<DAL.Models.Domain.UserService, bool>>(),false))
                .Returns(new List<DAL.Models.Domain.UserService>() { TestUserService });
            uowMock.Setup(t => t.UserServicesRepository.Create(It.IsAny<DAL.Models.Domain.UserService>())).Verifiable();

            var userPlanServiceMock = new Mock<IUserPlanService>();

            var userServicesService = new UserServicesService(uowMock.Object, userServiceMock.Object, userPlanServiceMock.Object);
            var result = userServicesService.SubscribeUserToService(TestUserName, TestServiceDTO);

            Assert.AreEqual(result.Succedeed, false);
        }

        [TestMethod]
        public void SubscribeUnubscribedUserToServiceTest()
        {
            Initialize();
            TestUserService.Status = ServiceStatus.Unsubscribed;

            var testPlansDTO = new List<UserPlanDTO>(){};

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(t => t.GetUser(TestUserName)).Returns(TestUserDTO);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(t => t.UserServicesRepository.Find(It.IsAny<Func<DAL.Models.Domain.UserService, bool>>(), false))
                .Returns(new List<DAL.Models.Domain.UserService>() { TestUserService });
            uowMock.Setup(t => t.UserServicesRepository.Create(It.IsAny<DAL.Models.Domain.UserService>())).Verifiable();

            var userPlanServiceMock = new Mock<IUserPlanService>();
            userPlanServiceMock.Setup(t => t.GetUserPlans(TestUserName, true)).Returns(testPlansDTO);

            var userServicesService = new UserServicesService(uowMock.Object, userServiceMock.Object, userPlanServiceMock.Object);
            var result = userServicesService.SubscribeUserToService(TestUserName, TestServiceDTO);

            Assert.AreEqual(result.Succedeed, true);
        }

        [TestMethod]
        public void UnsubscribeUserFromServiceTest()
        {
            Initialize();

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(t => t.GetUser(TestUserName)).Returns(TestUserDTO);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(t => t.UserServicesRepository.Find(It.IsAny<Func<DAL.Models.Domain.UserService, bool>>(), false))
                .Returns(new List<DAL.Models.Domain.UserService>() { TestUserService });
            uowMock.Setup(t => t.UserServicesRepository.Create(It.IsAny<DAL.Models.Domain.UserService>())).Verifiable();

            var userPlanServiceMock = new Mock<IUserPlanService>();
            userPlanServiceMock.Setup(t => t.GetUserPlans(TestUserName, true)).Returns(TestUserPlans);

            var userServicesService = new UserServicesService(uowMock.Object, userServiceMock.Object, userPlanServiceMock.Object);
            var result = userServicesService.UnsubscribeUserFromService(TestUserName, TestServiceDTO);

            Assert.AreEqual(result.Succedeed, true);
        }
    }
}
