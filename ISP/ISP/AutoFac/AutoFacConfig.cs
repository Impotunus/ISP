using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ISP.BLL.AutoFac;

namespace ISP.AutoFac
{
    public class AutoFacConfig
    {
        public static IContainer Container { get; set; }

        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            AutoFacWebConfig.Configure(builder);
            AutoFacBLLConfig.Configure(builder);

            var container = builder.Build();
            var mvcResolver = new AutofacDependencyResolver(container);

            Container = container;
            DependencyResolver.SetResolver(mvcResolver);
        }
    }
}