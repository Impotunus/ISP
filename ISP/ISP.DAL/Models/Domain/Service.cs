using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISP.DAL.Models.Domain
{
    public class Service : CommonEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ICollection<UserService> UserServices { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}
