using System;
using ISP.BLL.DTO.Identity;

namespace ISP.BLL.DTO.Domain
{
    public class UserPlanDTO
    {
        public string Id { get; set; }

        public int PlanId { get; set; }

        public UserDTO ApplicationUser { get; set; }

        public DateTime LastPaidDate { get; set; }

        public PlanDTO Plan { get; set; }

        public ServiceStatusDTO Status { get; set; }

        public bool WillUnsubscribe { get; set; }
    }
}
