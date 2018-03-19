using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISP.BLL.DTO.Domain
{
    public class PlanDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual ICollection<FeatureDTO> Features { get; set; }

        [DataType(DataType.Currency)]
        public double Cost { get; set; }

        public int ServiceId { get; set; }

        public ServiceDTO Service { get; set; }

        public bool IsDeleted { get; set; }
    }
}
