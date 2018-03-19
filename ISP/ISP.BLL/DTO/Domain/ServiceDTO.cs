using System.Collections.Generic;

namespace ISP.BLL.DTO.Domain
{
    public class ServiceDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ICollection<UserServiceDTO> UserServices { get; set; }

        public virtual ICollection<PlanDTO> Plans { get; set; }
    }
}
