using System;
using System.Collections.Generic;
using AutoMapper;
using ISP.AutoMapperProfiles;
using ISP.BLL.AutoMapperProfiles;
using ISP.BLL.DTO.Domain;
using ISP.BLL.DTO.Identity;
using ISP.BLL.Interfaces;
using ISP.BLL.Services;
using ISP.DAL.Enums;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ISP.Tests.Services
{
    [TestClass]
    public class UserPlanServiceTest
    {
        private string TestUserName { get; set; }

        private UserDTO TestUserDTO { get; set; }

        private ServiceDTO TestServiceDTO { get; set; }

        private PlanDTO TestPlanDTO { get; set; }

        private ICollection<UserPlanDTO> TestUserPlans { get; set; }

        private ICollection<UserPlan> TestUserDomainPlans { get; set; }

        private DAL.Models.Domain.UserService TestUserService { get; set; }

        public void Inititalize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(typeof(DTOToDomainProfile));
                cfg.AddProfile(typeof(DomainToDTOProfile));
                cfg.AddProfile(typeof(ViewModelToDTOProfile));
                cfg.AddProfile(typeof(DTOToViewModelProfile));
            });

            Mapper.Configuration.CompileMappings();

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
            TestPlanDTO = new PlanDTO()
            {
                Service = TestServiceDTO,
                Id = 1,
                Cost = 5,
                ServiceId = TestServiceDTO.Id,
                Features = new List<FeatureDTO>(),
                IsDeleted = false,
                Title = "TestPlan"
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
            TestUserDomainPlans = new List<UserPlan>()
            {
            };
        }

        [TestMethod]
        public void SubscribeUnexistingUserToPlanTest()
        {
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(t => t.GetUser(It.IsAny<string>())).Returns(TestUserDTO);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(t => t.UserPlansRepository.Find(It.IsAny<Func<UserPlan, bool>>(), false)).Returns(TestUserDomainPlans);

            uowMock.Setup(t => t.UserServicesRepository.Create(It.IsAny<DAL.Models.Domain.UserService>())).Verifiable();

            var userPlanService = new UserPlanService(uowMock.Object, userServiceMock.Object);
            var result = userPlanService.SubscribeUserToPlan(TestUserName, TestPlanDTO);

            Assert.AreEqual(result.Succedeed, false);
        }

    }
}
