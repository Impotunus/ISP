using System;
using System.Collections.Generic;
using System.Security.Claims;
using ISP.BLL.DTO.Identity;
using ISP.BLL.Utility;

namespace ISP.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        ICollection<UserDTO> GetUsers();
        
        UserDTO GetUser(string userName);

        OperationDetails Create(UserDTO userDTO);

        ClaimsIdentity Authenticate(UserDTO userDTO);

        void SetInitialData(UserDTO adminDTO, List<string> roles);

        void SetInitialRoles(List<string> roles);

        double GetUserBalance(string userName);

        void Ban(string userName, bool asAdmin);

        void UnBan(string userName);

        OperationDetails SetUserBalance(string userName, double bank);
    }
}
