using ISP.ViewModels.ProfileViewModels;
using ISP.ViewModels.ServiceViewModels;

namespace ISP.ViewModels.PlanViewModels
{
    public class UserPlanViewModel
    {
        public string Id { get; set; }

        public int PlanId { get; set; }

        public UserViewModel ApplicationUser { get; set; }

        public PlanViewModel Plan { get; set; }

        public ServiceStatusViewModel Status { get; set; }

        public bool WillUnsubscribe { get; set; }
    }
}
