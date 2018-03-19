using System.ComponentModel.DataAnnotations;

namespace ISP.ViewModels.FeatureViewModels
{
    public class FeatureViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
    }
}
