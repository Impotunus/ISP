using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ISP.AutoMapperProfiles;
using ISP.BLL.AutoMapperProfiles;
using ISP.Jobs;

namespace ISP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(typeof(DTOToDomainProfile));
                cfg.AddProfile(typeof(DomainToDTOProfile));
                cfg.AddProfile(typeof(ViewModelToDTOProfile));
                cfg.AddProfile(typeof(DTOToViewModelProfile));
            });

            AutoMapper.Mapper.Configuration.CompileMappings();

            AutoFac.AutoFacConfig.Configure();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            UserPlansUpdaterScheduler.Start();
        }
    }
}
