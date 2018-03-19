using System;
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
    public class UserPlanService : IUserPlanService
    {
        private IUnitOfWork Database { get; }

        private IUserService UserService { get; }

        public UserPlanService(IUnitOfWork database, IUserService userService)
        {
            Database = database;
            UserService = userService;
        }

        public virtual ICollection<UserPlanDTO> GetUserPlans(string userName, bool onlySubscribed = false)
        {
            var userDTO = UserService.GetUser(userName);

            ICollection<UserPlan> userPlans;
            if (onlySubscribed)
            {
                userPlans = Database.UserPlansRepository
                    .Find(t => t.ApplicationUser.Id == userDTO.Id &&
                     (t.Status == ServiceStatus.Active || t.Status == ServiceStatus.Subscribed))
                    .ToList();
            }
            else
            {
                userPlans = Database.UserPlansRepository.Find(t => t.ApplicationUser.Id == userDTO.Id).ToList();
            }
            var userPlanDTOs = AutoMapper.Mapper.Map<List<UserPlanDTO>>(userPlans);

            return userPlanDTOs;
        }

        public virtual OperationDetails SubscribeUserToPlan(string userName, PlanDTO planDTO)
        {
            if (!CheckIfUserExists(userName))
            {
                return new OperationDetails(false, "The user does not exist.", "user");
            }

            var userDTO = UserService.GetUser(userName);
            var userActivePlans = GetUserPlans(userName, true);
            if (userActivePlans.FirstOrDefault(t => t.Plan.ServiceId == planDTO.ServiceId && t.WillUnsubscribe) != null)
            {
                var notUnsubscribedPlan = Database.UserPlansRepository.Find(x => x.Plan.ServiceId == planDTO.ServiceId && x.WillUnsubscribe && !x.IsDeleted) 
                    .First();
                notUnsubscribedPlan.WillUnsubscribe = false;
                Database.SaveChanges();
            }
            var doOtherActivePlanOnTheServiceExist = userActivePlans.Any(t => t.Plan.ServiceId == planDTO.ServiceId);

            if (doOtherActivePlanOnTheServiceExist)
            {
                return new OperationDetails(false, "User has other active plans on the service. Unsubscribe from them first.", "plans");
            }

            var balance = UserService.GetUserBalance(userName);
            if (balance >= planDTO.Cost)
            {
                var oldEntry = Database.UserPlansRepository
                    .Find(t => t.ApplicationUserId == userDTO.Id && t.PlanId == planDTO.Id, true).FirstOrDefault();
                if (oldEntry != null)
                {
                    oldEntry.Status = ServiceStatus.Active;
                    oldEntry.LastPaidDate = DateTime.UtcNow;
                    oldEntry.IsDeleted = false;
                    oldEntry.WillUnsubscribe = false;
                }
                else
                {
                    Database.UserPlansRepository.Create(new DAL.Models.Domain.UserPlan()
                    {
                        Plan = Database.PlansRepository.Get(planDTO.Id),
                        ApplicationUser = Database.UserManager.FindByName(userDTO.UserName),
                        Status = ServiceStatus.Active,
                        LastPaidDate = DateTime.UtcNow,
                        WillUnsubscribe = false
                    });
                    
                }
                UserService.SetUserBalance(userName, balance - planDTO.Cost);
                Database.SaveChanges();

                return new OperationDetails(true, "The user has been subscribed.", string.Empty);
            }

            return new OperationDetails(false, "The user has not enough money to subscribe to the plan.", "money");
        }

        public virtual OperationDetails UnsubscribeUserFromPlan(string userName, PlanDTO planDTO)
        {
            if (!CheckIfUserExists(userName))
            {
                return new OperationDetails(false, "The user does not exist.", string.Empty);
            }

            var userDTO = UserService.GetUser(userName);
            var userPlan = GetUserSubscriptionEntry(userDTO, planDTO);
            if (userPlan == null)
            {
                return new OperationDetails(false, "The user is not subscribed to the specified plan.", string.Empty);
            }

            userPlan.WillUnsubscribe = true;
            Database.SaveChanges();

            return new OperationDetails(true, "The user has been unsubscribed from the plan.", string.Empty);
        }

        public virtual OperationDetails UpdateUserPlanSubscribtions(string userName)
        {
            if (!CheckIfUserExists(userName))
            {
                return new OperationDetails(false, "The user does not exist.", string.Empty);
            }

            var userDTO = UserService.GetUser(userName);
            var userPlans = GetUserPlans(userName, true);

            UnsubscribeUserFromPlans(userDTO, userPlans);

            var userActivePlans = GetUserActivePlans(userDTO);
            var totalPlansCost = userActivePlans.Sum(userActivePlan => userActivePlan.Plan.Cost);
            var userBank = userDTO.Balance;

            if (totalPlansCost > userBank)
            {
                BanUser(userDTO, userPlans, false);
                Database.SaveChanges();
            }
            else if(totalPlansCost <= userBank)
            {
                RenewUserPlans(userName, userBank, totalPlansCost, userActivePlans);
                Database.SaveChanges();
            }
            return new OperationDetails(true, "User subscribtions renewed.", string.Empty);
        }

        private  void UnsubscribeUserFromPlans(UserDTO userDTO, ICollection<UserPlanDTO> userPlans)
        {
            foreach (var userPlanDTO in userPlans)
            {
                if (!userPlanDTO.WillUnsubscribe) continue;
                if (DateTime.UtcNow.Subtract(userPlanDTO.LastPaidDate).TotalHours >=
                    Utility.Constants.UserPlansUpdateRate)
                {
                    var userPlan = Database.UserPlansRepository
                        .Find(t => t.ApplicationUserId == userDTO.Id && t.PlanId == userPlanDTO.PlanId).First();

                    userPlan.WillUnsubscribe = false;
                    userPlan.Status = ServiceStatus.Unsubscribed;

                    Database.SaveChanges();
                }
            }
        }

        private void BanUser(UserDTO userDTO, ICollection<UserPlanDTO> userPlans, bool asAdmin)
        {
            UserService.Ban(userDTO.UserName, asAdmin);
            foreach (var userPlanDTO in userPlans)
            {
                var userPlan = Database.UserPlansRepository
                    .Find(t => t.ApplicationUserId == userDTO.Id && t.PlanId == userPlanDTO.PlanId)
                    .First();

                userPlan.WillUnsubscribe = false;
                userPlan.Status = ServiceStatus.Deactivated;
            }
        }

        public virtual void Ban(string userName, bool asAdmin)
        {
            if (!CheckIfUserExists(userName))
            {
                return;
            }
            var userDTO = UserService.GetUser(userName);
            var userActivePlans =
                Database.UserPlansRepository
                .Find(t => t.ApplicationUserId == userDTO.Id 
                && t.IsDeleted == false 
                && t.Status == ServiceStatus.Active);
            
            BanUser(userDTO, AutoMapper.Mapper.Map<List<UserPlanDTO>>(userActivePlans), asAdmin);
        }

        public virtual void UnBan(string userName)
        {
            if (!CheckIfUserExists(userName))
            {
                return;
            }

            var userDTO = UserService.GetUser(userName);
            var userDeactivatedPlans =
                Database.UserPlansRepository.Find(t => t.ApplicationUserId == userDTO.Id 
                && t.IsDeleted == false 
                && t.Status == ServiceStatus.Deactivated);

            UnbanUser(userDTO, AutoMapper.Mapper.Map<List<UserPlanDTO>>(userDeactivatedPlans));
            Database.SaveChanges();
            UpdateUserPlanSubscribtions(userDTO.UserName);
        }

        private void UnbanUser(UserDTO userDTO, ICollection<UserPlanDTO> userPlans)
        {
            UserService.UnBan(userDTO.UserName);
            foreach (var userPlanDTO in userPlans)
            {
                var userPlan = Database.UserPlansRepository
                    .Find(t => t.ApplicationUserId == userDTO.Id && t.PlanId == userPlanDTO.PlanId)
                    .First();

                userPlan.WillUnsubscribe = false;
                userPlan.Status = ServiceStatus.Active;
            }
        }

        private ICollection<UserPlan> GetUserActivePlans(UserDTO userDTO)
        {
            var userActivePlans = Database.UserPlansRepository
                .Find(t => t.ApplicationUserId == userDTO.Id
                && !t.WillUnsubscribe
                && (DateTime.UtcNow.Subtract(t.LastPaidDate).TotalHours >= Utility.Constants.UserPlansUpdateRate)
                && (t.Status == ServiceStatus.Active || t.Status == ServiceStatus.Subscribed));

            return userActivePlans;
        }

        private void RenewUserPlans(string userName, double userBank, double totalPlansCost, ICollection<UserPlan> userActivePlans)
        {
            UserService.SetUserBalance(userName, userBank - totalPlansCost);
            foreach (var userActivePlan in userActivePlans)
            {
                userActivePlan.LastPaidDate = DateTime.UtcNow;
            }
        }

        private UserPlan GetUserSubscriptionEntry(UserDTO userDTO, PlanDTO planDTO)
        {
            var userPlan = Database.UserPlansRepository
                .Find(t => t.PlanId == planDTO.Id && t.ApplicationUserId == userDTO.Id)
                .FirstOrDefault();

            return userPlan;
        }

        private bool CheckIfUserExists(string userName)
        {
            var userDTO = UserService.GetUser(userName);

            return userDTO != null;
        }
    }
}
