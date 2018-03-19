using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ISP.ViewModels.PlanViewModels;

namespace ISP.ViewModels.ServiceViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<UserServiceViewModel> UserServices { get; set; }

        public virtual ICollection<PlanViewModel> Plans { get; set; }
    }
}