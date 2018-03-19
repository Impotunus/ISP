using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.DAL.Models.Domain
{
    public class Plan : CommonEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual ICollection<Feature> Features { get; set; }

        [DataType(DataType.Currency)]
        public double Cost { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }

        public virtual Service Service { get; set; }
    }
}
