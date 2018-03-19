using System.Web;
using System.Web.Mvc;
using Autofac;
using ISP.BLL.Interfaces;

namespace ISP.Attributes
{
    public class RedirectBannedAttribute : AuthorizeAttribute
    {
        private IUserService UserService { get; }

        private string UserName { get; set; }

        public RedirectBannedAttribute()
        {
            UserService = AutoFac.AutoFacConfig.Container.Resolve<IUserService>();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isUserInBannedRole = httpContext.User.IsInRole("banned");
            UserName = httpContext.User.Identity.Name;

            return !isUserInBannedRole;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (UserService.GetUser(UserName).AdminBanned)
            {
                filterContext.Result = new RedirectToRouteResult("BannedByAdmin", null);
                return;
            }
            filterContext.Result = new RedirectToRouteResult("BannedBySystem", null);
        }
    }
}