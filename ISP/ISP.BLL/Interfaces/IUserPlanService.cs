using System.Collections.Generic;
using ISP.BLL.DTO.Domain;
using ISP.BLL.Utility;

namespace ISP.BLL.Interfaces
{
    public interface IUserPlanService
    {
        ICollection<UserPlanDTO> GetUserPlans(string userName, bool onlySubscribed = false);

        OperationDetails SubscribeUserToPlan(string userName, PlanDTO planDTO);

        OperationDetails UnsubscribeUserFromPlan(string userName, PlanDTO planDTO);

        OperationDetails UpdateUserPlanSubscribtions(string userName);

        void Ban(string userName, bool asAdmin);

        void UnBan(string userName);
    }
}
