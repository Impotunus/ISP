using System.Collections.Generic;
using ISP.ViewModels.ServiceViewModels;

namespace ISP.ViewModels.ProfileViewModels
{
    public class ProfileViewModel
    {
        public UserViewModel User { get; set; }

        public ICollection<ServiceViewModel> Services { get; set; }

        public ICollection<UserServiceViewModel> UserServices { get; set; }

        public IDictionary<ServiceViewModel, bool> ServiceSubscribtions { get; set; }

        public List<string> ActivePlans { get; set; }
    }
}