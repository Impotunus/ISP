using Autofac;
using Autofac.Integration.Mvc;

namespace ISP.AutoFac
{
    public class AutoFacWebConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
        }
    }
}