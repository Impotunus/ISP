using System.Collections.Generic;
using ISP.BLL.DTO.Domain;
using ISP.BLL.Interfaces;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;

namespace ISP.BLL.Services
{
    public class PlanService : IPlanService
    {
        private IUnitOfWork Database { get; }

        private IUserPlanService UserPlanService { get; }

        public void AddFeature(int planId, int featureId)
        {
            var plan = Database.PlansRepository.Get(planId);
            var feature = Database.FeaturesRepository.Get(featureId);
            plan.Features.Add(feature);
            Database.PlansRepository.Update(plan);
            Database.SaveChanges();
        }

        public void RemoveFeature(int planId, int featureId)
        {
            var plan = Database.PlansRepository.Get(planId);
            var feature = Database.FeaturesRepository.Get(featureId);
            plan.Features.Remove(feature);
            Database.PlansRepository.Update(plan);
            Database.SaveChanges();
        }

        public PlanService(IUnitOfWork database, IUserPlanService userPlanService)
        {
            Database = database;
            UserPlanService = userPlanService;
        }

        public void UpdatePlan(PlanDTO planDTO)
        {
            var plan = Database.PlansRepository.Get(planDTO.Id);
            var service = Database.ServicesRepository.Get(plan.ServiceId);
            AutoMapper.Mapper.Map(planDTO, plan);
            plan.Service = service;
            Database.PlansRepository.Update(plan);
            
            Database.SaveChanges();
        }

        public void CreatePlan(PlanDTO planDTO)
        {
            var plan = AutoMapper.Mapper.Map<Plan>(planDTO);
            Database.PlansRepository.Create(plan);

            Database.SaveChanges();
        }

        public void DeletePlan(int id)
        {
            var userPlans = Database.UserPlansRepository.Find(t => t.PlanId == id);

            foreach (var userPlan in userPlans)
            {
                UserPlanService.UnsubscribeUserFromPlan(userPlan.ApplicationUser.UserName, AutoMapper.Mapper.Map<PlanDTO>(userPlan.Plan));
            }

            Database.PlansRepository.Delete(id);
            Database.SaveChanges();
        }

        public ICollection<PlanDTO> GetPlans(int serviceId)
        {
            var plans = Database.PlansRepository.Find(t => t.ServiceId == serviceId);
            var planDTOs = AutoMapper.Mapper.Map<List<PlanDTO>>(plans);

            return planDTOs;
        }

        public ICollection<PlanDTO> GetPlans()
        {
            var plans = Database.PlansRepository.GetAll();
            var planDTOs = AutoMapper.Mapper.Map<List<PlanDTO>>(plans);

            return planDTOs;
        }

        public PlanDTO GetPlan(int id)
        {
            var plan = Database.PlansRepository.Get(id);
            var planDTO = AutoMapper.Mapper.Map<PlanDTO>(plan);

            return planDTO;
        }
    }
}
