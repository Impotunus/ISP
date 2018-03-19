using System.Security.Principal;
using Autofac;
using ISP.BLL.Interfaces;

namespace ISP.IdentityExtensions
{
    public static class IdentityBankExtension
    {
        private static IUserService UserService { get; } = AutoFac.AutoFacConfig.Container.Resolve<IUserService>();

        public static double GetUserBalance(this IIdentity identity)
        {
            return identity == null ? 0d : UserService.GetUserBalance(identity.Name);
        }
    }
}