using System.Web.Mvc;
using System.Web.Routing;

namespace ISP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "UnsubscribeFromService",
                url: "Profile/UnsubscribeFromService",
                defaults: new { controller = "Profile", action = "UnsubscribeFromService" }
            );

            routes.MapRoute(
                name: "SubscribeToService",
                url: "Profile/SubscribeToService",
                defaults: new { controller = "Profile", action = "SubscribeToService" }
            );

            routes.MapRoute(
                name: "BanUser",
                url: "Profile/{userName}/Ban",
                defaults: new { controller = "Profile", action = "Ban" }
            );

            routes.MapRoute(
                name: "UnBanUser",
                url: "Profile/{userName}/Unban",
                defaults: new { controller = "Profile", action = "UnBan" }
            );

            routes.MapRoute(
                name: "UserProfile",
                url: "Profile/{userName}",
                defaults: new { controller = "Profile", action = "Index" }
            );

            routes.MapRoute(
                name: "PlanDownload",
                url: "Plan/Download",
                defaults: new { controller = "Plan", action = "GetPlans" }
            );

            routes.MapRoute(
                name: "PlansTitleSorted",
                url: "Plan/{userName}/{serviceId}/{sortBy}/{Asc}",
                defaults: new { controller = "Plan", action = "Index", sortBy = UrlParameter.Optional, Asc = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BannedByAdmin",
                url: "Banned/",
                defaults: new { controller = "Banned", action = "BannedByAdmin" }
            );

            routes.MapRoute(
                name: "BannedBySystem",
                url: "Deactivated/",
                defaults: new { controller = "Banned", action = "BannedBySystem" }
            );

            routes.MapRoute(
                name: "CreatePlan",
                url: "Admin/Service/{serviceId}/Plan/Create",
                defaults: new { controller = "Plan", action = "CreatePlan" }
            );

            routes.MapRoute(
                name: "AddPlan",
                url: "Admin/Service/{serviceId}/Plan/Add",
                defaults: new { controller = "Plan", action = "AddPlan" }
            );

            routes.MapRoute(
                name: "EditService",
                url: "Admin/Service/{serviceId}/Edit",
                defaults: new { controller = "Service", action = "EditService" }
            );

            routes.MapRoute(
                name: "UpdateService",
                url: "Admin/Service/{serviceId}/Update",
                defaults: new { controller = "Service", action = "UpdateService" }
            );

            routes.MapRoute(
                name: "DeleteService",
                url: "Admin/Service/{serviceId}/Delete",
                defaults: new { controller = "Service", action = "DeleteService" }
            );

            routes.MapRoute(
                name: "AddService",
                url: "Admin/Service/Add",
                defaults: new { controller = "Service", action = "AddService" }
            );

            routes.MapRoute(
                name: "CreateService",
                url: "Admin/Service/Create",
                defaults: new { controller = "Service", action = "CreateService" }
            );

            routes.MapRoute(
                name: "AdminServices",
                url: "Admin/Services",
                defaults: new { controller = "Service", action = "GetServicesForAdmin" }
            );

            routes.MapRoute(
                name: "UpdatePlan",
                url: "Admin/Plan/{planId}/Update",
                defaults: new { controller = "Plan", action = "UpdatePlan" }
            );

            routes.MapRoute(
                name: "EditPlan",
                url: "Admin/Plan/{planId}/Edit",
                defaults: new { controller = "Plan", action = "EditPlan" }
            );

            routes.MapRoute(
                name: "DeletePlan",
                url: "Admin/Plan/{planId}/Delete",
                defaults: new { controller = "Plan", action = "DeletePlan" }
            );

            routes.MapRoute(
                name: "AdminPlans",
                url: "Admin/Service/{serviceId}/Plans",
                defaults: new { controller = "Plan", action = "GetPlansForAdmin" }
            );

            routes.MapRoute(
                name: "SearchUser",
                url: "Admin/Users/Search/{userName}",
                defaults: new { controller = "Admin", action = "FindUser" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
            
        }
    }
}
