using ISP.BLL.DTO.Identity;
using ISP.DAL.Enums;

namespace ISP.BLL.DTO.Domain
{
    public class UserServiceDTO
    {
        public string Id { get; set; }

        public UserDTO ApplicationUser { get; set; }

        public int ServiceId { get; set; }

        public ServiceDTO Service { get; set; }

        public ServiceStatus Status { get; set; }
    }
}
