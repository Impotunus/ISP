using System;
using System.Collections.Generic;
using ISP.BLL.Services;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserService = ISP.DAL.Models.Domain.UserService;

namespace ISP.Tests.Services
{
    [TestClass]
    public class DownloadServiceTest
    {
        [TestMethod]
        public void GetDownloadableServicesTest()
        {
            var listOfServices = new List<Service>()
            {
                new Service() {Id = 1, Description = "Descr1", Title = "Title1", UserServices = new List<UserService>(), Plans = new List<Plan>(), IsDeleted = false},
                new Service() {Id = 2, Description = "Descr2", Title = "Title2", UserServices = new List<UserService>(), Plans = new List<Plan>(), IsDeleted = false},
                new Service() {Id = 3, Description = "Descr3", Title = "Title3", UserServices = new List<UserService>(), Plans = new List<Plan>(), IsDeleted = false},
                new Service() {Id = 4, Description = "Descr4", Title = "Title4", UserServices = new List<UserService>(), Plans = new List<Plan>(), IsDeleted = false},
            };

            var listOfPlans = new List<Plan>()
            {
            };

            var mock = new Mock<IUnitOfWork>();

            mock.Setup(t => t.ServicesRepository.GetAll(false)).Returns(listOfServices);
            mock.Setup(t => t.PlansRepository.Find(It.IsAny<Func<Plan,bool>>(), false)).Returns(listOfPlans);
            
            var downloadService = new DownloadService(mock.Object);

            var result = downloadService.GetServices();

            Assert.AreNotEqual(result.Length, 0);
        }

        [TestMethod]
        public void GetEmptyDownloadableServicesTest()
        {
            var listOfServices = new List<Service>()
            {
            };
            var listOfPlans = new List<Plan>()
            {
            };
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(t => t.ServicesRepository.GetAll(false)).Returns(listOfServices);
            mock.Setup(t => t.PlansRepository.Find(It.IsAny<Func<Plan, bool>>(), false)).Returns(listOfPlans);

            var downloadService = new DownloadService(mock.Object);
            var result = downloadService.GetServices();

            Assert.AreEqual(result.Length, 0);
        }
    }
}
