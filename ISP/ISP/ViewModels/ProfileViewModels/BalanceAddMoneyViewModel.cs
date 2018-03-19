using System;
using System.ComponentModel.DataAnnotations;

namespace ISP.ViewModels.ProfileViewModels
{
    public class BalanceAddMoneyViewModel
    {
        [Required]
        public string userName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Money must be positive.")]
        public double Money { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.0, Double.MaxValue)]
        public double RequiredMoney { get; set; }
    }
}