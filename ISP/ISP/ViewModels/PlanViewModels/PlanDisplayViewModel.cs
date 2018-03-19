using System.Collections.Generic;
using ISP.ViewModels.ProfileViewModels;

namespace ISP.ViewModels.PlanViewModels
{
    public class PlanDisplayViewModel
    {
        public string ServiceTitle { get; set; }

        public UserViewModel User { get; set; }

        public ICollection<PlanViewModel> PlanViewModels { get; set; }

        public Dictionary<PlanViewModel, bool> PlanSubscribtionsDictionary { get; set; }

        public bool IsUnsubscribeDisabled { get; set; }

        public string Message { get; set; }
    }
}