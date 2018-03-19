using System.Security.Principal;
using Autofac;
using ISP.BLL.Interfaces;

namespace ISP.IdentityExtensions
{
    public static class IdentityBannedExtension
    {
        private static IUserService UserService { get; } = AutoFac.AutoFacConfig.Container.Resolve<IUserService>();

        public static bool IsUserBannedByAdmin(this IIdentity identity)
        {
            return UserService.GetUser(identity.Name).AdminBanned;
        }
    }
}