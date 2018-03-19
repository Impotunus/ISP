using System.Collections.Generic;

namespace ISP.ViewModels.PlanViewModels
{
    public class PlanOutputViewModel
    {
        public int ServiceId { get; set; }

        public string ServiceTitle { get; set; }

        public ICollection<PlanViewModel> PlanViewModels { get; set; }
    }
}