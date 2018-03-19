using System.Collections.Generic;
using System.Linq;
using ISP.BLL.DTO.Domain;
using ISP.BLL.DTO.Identity;
using ISP.BLL.Interfaces;
using ISP.BLL.Utility;
using ISP.DAL.Enums;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;
using Microsoft.AspNet.Identity;

namespace ISP.BLL.Services
{
    public class UserServicesService : IUserServicesService
    {
        private IUnitOfWork Database { get; }

        private IUserService UserService { get; }

        private IUserPlanService UserPlanService { get; set; }

        public UserServicesService(IUnitOfWork database, IUserService userService, IUserPlanService userPlanService)
        {
            Database = database;
            UserService = userService;
            UserPlanService = userPlanService;
        }

        public ICollection<UserServiceDTO> GetUserServices(string userName, bool onlySubscribed = false)
        {
            var userDTO = UserService.GetUser(userName);

            ICollection<DAL.Models.Domain.UserService> userServices;
            if (onlySubscribed)
            {
                userServices = Database.UserServicesRepository
                    .Find(t => t.ApplicationUser.Id == userDTO.Id && 
                    (t.Status == ServiceStatus.Active || t.Status == ServiceStatus.Subscribed))
                    .ToList();
            }
            else
            {
                userServices = Database.UserServicesRepository.Find(t => t.ApplicationUser.Id == userDTO.Id).ToList();
            }
            var userServiceDTOs = AutoMapper.Mapper.Map<List<UserServiceDTO>>(userServices);

            return userServiceDTOs;
        }

        public ICollection<ServiceDTO> GetServicesOfTheUser(string userName, bool onlySubscribed = false)
        {
            var userDTO = UserService.GetUser(userName);

            ICollection<DAL.Models.Domain.UserService> userServices = new List<DAL.Models.Domain.UserService>();
            if (onlySubscribed)
            {
                userServices = Database.UserServicesRepository
                    .Find(t => t.ApplicationUser.Id == userDTO.Id &&
                     (t.Status == ServiceStatus.Active || t.Status == ServiceStatus.Subscribed))
                    .ToList();
            }
            else
            {
                userServices = Database.UserServicesRepository.Find(t => t.ApplicationUser.Id == userDTO.Id).ToList();
            }
            ICollection<Service> services = userServices.Select(userService => userService.Service).ToList();
            var serviceDTOs = AutoMapper.Mapper.Map<List<ServiceDTO>>(services);

            return serviceDTOs;
        }

        public OperationDetails SubscribeUserToService(string userName, ServiceDTO serviceDTO)
        {
            if (!CheckIfUserExists(userName))
            {
                return new OperationDetails(false, "The user does not exist.", string.Empty);
            }

            var userDTO = UserService.GetUser(userName);
            var userService = GetUserSubscriptionEntry(userDTO, serviceDTO);

            if (userService == null)
            {
                Database.UserServicesRepository.Create(new DAL.Models.Domain.UserService()
                {
                    Service = Database.ServicesRepository.Get(serviceDTO.Id),
                    ApplicationUser = Database.UserManager.FindByName(userDTO.UserName),
                    Status = ServiceStatus.Subscribed
                });
                Database.SaveChanges();

                return new OperationDetails(true, "The user has been subscribed.", string.Empty);
            }

            if (userService.Status != ServiceStatus.Unsubscribed)
            {
                return new OperationDetails(false, "The user is already subscribed to the service.", string.Empty);
            }

            foreach (var userPlanDto in UserPlanService.GetUserPlans(userName, true))
            {
                if (userPlanDto.Plan.ServiceId == serviceDTO.Id)
                {
                    UserPlanService.SubscribeUserToPlan(userName, userPlanDto.Plan);
                }
            }

            userService.Status = ServiceStatus.Subscribed;
            Database.SaveChanges();

            return new OperationDetails(true, "The user has been subscribed to the specified service.", string.Empty);
        }

        public OperationDetails UnsubscribeUserFromService(string userName, ServiceDTO serviceDTO)
        {
            if (!CheckIfUserExists(userName))
            {
                return new OperationDetails(false, "The user does not exist.", string.Empty);
            }

            var userDTO = UserService.GetUser(userName);
            var userService = GetUserSubscriptionEntry(userDTO, serviceDTO);
            var isUserSubscribedToService = userService != null;

            if (!isUserSubscribedToService)
            {
                return new OperationDetails(true, "The user is not subscribed to the service.", string.Empty);
            }

            userService.Status = ServiceStatus.Unsubscribed;
            Database.SaveChanges();

            foreach (var userPlanDto in UserPlanService.GetUserPlans(userName, true))
            {
                if (userPlanDto.Plan.ServiceId == serviceDTO.Id)
                {
                    UserPlanService.UnsubscribeUserFromPlan(userName, userPlanDto.Plan);
                }
            }
            
            return new OperationDetails(true, "The user has been unsubscribed from the service.", string.Empty);
        }

        private DAL.Models.Domain.UserService GetUserSubscriptionEntry(UserDTO userDTO, ServiceDTO serviceDTO)
        {
            var userService = Database.UserServicesRepository
                .Find(t => t.ServiceId == serviceDTO.Id && t.ApplicationUserId == userDTO.Id)
                .FirstOrDefault();

            return userService;
        }

        private bool CheckIfUserExists(string userName)
        {
            var userDTO = UserService.GetUser(userName);

            return userDTO != null;
        }
    }
}
