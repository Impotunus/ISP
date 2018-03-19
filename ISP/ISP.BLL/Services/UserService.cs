using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using ISP.BLL.DTO.Identity;
using ISP.BLL.Interfaces;
using ISP.BLL.Utility;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Identity;
using Microsoft.AspNet.Identity;

namespace ISP.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; }

        public void Ban(string userName, bool asAdmin)
        {
            var user = Database.UserManager.FindByName(userName);
            if (asAdmin)
            {
                user.AdminBanned = true;
            }
            Database.UserManager.AddToRole(user.Id, "banned");
        }

        public void UnBan(string userName)
        {
            var user = Database.UserManager.FindByName(userName);
            user.AdminBanned = false;
            Database.UserManager.RemoveFromRole(user.Id, "banned");
        }

        public ICollection<UserDTO> GetUsers()
        {
            var users = Database.UserManager.Users.ToList();
            var usersDTO = new List<UserDTO>();

            foreach (var applicationUser in users)
            {
                var userProfile = Database.UserProfileRepository.Find(t => t.ApplicationUserId == applicationUser.Id).First();

                usersDTO.Add(new UserDTO()
                {
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    UserName = applicationUser.UserName,
                    Email = applicationUser.Email,
                    Address = userProfile.Address,
                    Balance = userProfile.Balance,
                    Id = applicationUser.Id,
                    AdminBanned = applicationUser.AdminBanned,
                    Role = Database.UserManager.IsInRole(applicationUser.Id, "admin") ? "admin" : "user"
                });
            }

            return usersDTO;
        }

        public UserDTO GetUser(string userName)
        {
            var user = Database.UserManager.FindByName(userName);

            if (user == null)
            {
                return null;
            }

            var userProfile = Database.UserProfileRepository.Find(t => t.ApplicationUserId == user.Id).First();

            var userDTO = new UserDTO()
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Address = userProfile.Address,
                Balance = userProfile.Balance,
                Id = user.Id,
                AdminBanned = user.AdminBanned,
                Role = Database.UserManager.IsInRole(user.Id, "admin") ? "admin" : "user"
            };

            return userDTO;
        }

        public UserService(IUnitOfWork database)
        {
            Database = database;
        }

        public OperationDetails Create(UserDTO userDTO)
        {
            var isUserUnique = Database.UserManager.FindByEmail(userDTO.Email) == null &&
                Database.UserManager.FindByName(userDTO.UserName) == null;

            if (!isUserUnique)
            {
                return new OperationDetails(false, "The user with specified email and/or username already exists.", "");
            }

            var newUser = new ApplicationUser() { Email = userDTO.Email, UserName = userDTO.UserName };
            var operationResult = Database.UserManager.Create(newUser, userDTO.Password);

            if (!operationResult.Succeeded)
            {
                return new OperationDetails(false, "Failed to create the user.", "");
            }

            operationResult = Database.UserManager.AddToRole(newUser.Id, userDTO.Role);

            if (!operationResult.Succeeded)
            {
                return new OperationDetails(false, "Failed to add the user to the role.", "");
            }

            var newUserProfile = new UserProfile() {
                ApplicationUser = newUser,
                Address = userDTO.Address,
                Balance = Utility.Constants.DefaultRegisterBalanceValue,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName
            };

            Database.UserProfileRepository.Create(newUserProfile);
            Database.SaveChanges();

            return new OperationDetails(true, "The user has been successfully created.", "");
        }

        public ClaimsIdentity Authenticate(UserDTO userDTO)
        {
            ClaimsIdentity identity = null;

            var user = Database.UserManager.Find(userDTO.UserName, userDTO.Password);

            if (user != null)
            {
                identity = Database.UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                identity.AddClaim(new Claim("Bank", GetUserBalance(user.UserName).ToString(CultureInfo.InvariantCulture)));
            }

            return identity;
        }

        public OperationDetails SetUserBalance(string userName, double bank)
        {
            var userProfile = Database.UserManager.FindByName(userName).UserProfile;

            if (bank >= 0)
            {
                userProfile.Balance = bank;
                Database.SaveChanges();
                return new OperationDetails(true, $"User bank is set to {bank}.", string.Empty);
            }

            return new OperationDetails(false, $"Bank value of {bank} is incorrect.", "bank");
        }

        public double GetUserBalance(string userName)
        {
            double bank = 0;
            var user = Database.UserManager.FindByName(userName);
            var userProfile = Database.UserProfileRepository.Find(t => t.ApplicationUserId == user.Id).FirstOrDefault();

            if (userProfile != null)
            {
                bank = userProfile.Balance;
            }

            return bank;
        }

        public void SetInitialData(UserDTO adminDTO, List<string> roles)
        {
            foreach (var roleName in roles)
            {
                var role =  Database.RoleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    Database.RoleManager.Create(role);
                }
            }
            Create(adminDTO);
        }

        public void SetInitialRoles(List<string> roles)
        {
            foreach (var roleName in roles)
            {
                var role = Database.RoleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    Database.RoleManager.Create(role);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
