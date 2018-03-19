using ISP.ViewModels.ProfileViewModels;

namespace ISP.ViewModels.ServiceViewModels
{
    public class UserServiceViewModel
    {
        public string Id { get; set; }

        public UserViewModel ApplicationUser { get; set; }

        public int ServiceId { get; set; }

        public ServiceViewModel Service { get; set; }

        public ServiceStatusViewModel Status { get; set; }
    }
}
