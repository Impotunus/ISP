using System.Collections.Generic;

namespace ISP.ViewModels.FeatureViewModels
{
    public class FeatureListViewModel
    {
        public int PlanId { get; set; }

        public List<FeatureViewModel> FeatureViewModels { get; set; }
    }
}