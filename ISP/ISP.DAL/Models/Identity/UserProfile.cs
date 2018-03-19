using System.ComponentModel.DataAnnotations;

namespace ISP.DAL.Models.Identity
{
    public class UserProfile : CommonEntity
    {
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public double Balance { get; set; }
    }
}
