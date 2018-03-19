using System.Collections.Generic;
using ISP.BLL.DTO.Domain;

namespace ISP.BLL.Interfaces
{
    public interface IServiceService
    {
        ICollection<ServiceDTO> GetServices();

        ServiceDTO GetService(int id);

        void CreateService(ServiceDTO serviceDTO);

        void UpdateService(ServiceDTO serviceDTO);

        void DeleteService(int id);
    }
}
