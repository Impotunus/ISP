using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.DAL.Models.Domain
{
    public class Feature : CommonEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
