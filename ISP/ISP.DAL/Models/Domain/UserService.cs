using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISP.DAL.Enums;
using ISP.DAL.Models.Identity;

namespace ISP.DAL.Models.Domain
{
    public class UserService : CommonEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int ServiceId { get; set; }

        public virtual Service Service { get; set; }

        public ServiceStatus Status { get; set; }
    }
}
