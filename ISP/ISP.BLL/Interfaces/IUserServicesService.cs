using System.Collections.Generic;
using ISP.BLL.DTO.Domain;
using ISP.BLL.Utility;

namespace ISP.BLL.Interfaces
{
    public interface IUserServicesService
    {
        ICollection<UserServiceDTO> GetUserServices(string userName, bool onlySubscribed = false);

        ICollection<ServiceDTO> GetServicesOfTheUser(string userName, bool onlySubscribed = false);

        OperationDetails SubscribeUserToService(string userName, ServiceDTO serviceDTO);

        OperationDetails UnsubscribeUserFromService(string userName, ServiceDTO serviceDTO);
    }
}
