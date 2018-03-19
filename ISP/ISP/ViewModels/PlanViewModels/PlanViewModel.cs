using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ISP.ViewModels.FeatureViewModels;
using ISP.ViewModels.ServiceViewModels;

namespace ISP.ViewModels.PlanViewModels
{
    public class PlanViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<FeatureViewModel> Features { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Cost must be positive.")]
        public double Cost { get; set; }

        public int ServiceId { get; set; }

        public ServiceViewModel Service { get; set; }

        public bool IsDeleted { get; set; }
    }
}
