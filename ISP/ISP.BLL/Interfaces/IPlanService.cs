using System.Collections.Generic;
using ISP.BLL.DTO.Domain;

namespace ISP.BLL.Interfaces
{
    public interface IPlanService
    {
        ICollection<PlanDTO> GetPlans(int serviceId);

        ICollection<PlanDTO> GetPlans();

        PlanDTO GetPlan(int id);

        void AddFeature(int planId, int featureId);

        void RemoveFeature(int planId, int featureId);

        void UpdatePlan(PlanDTO planDTO);

        void CreatePlan(PlanDTO planDTO);

        void DeletePlan(int id);
    }
}
