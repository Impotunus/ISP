using Autofac;
using ISP.BLL.Interfaces;
using ISP.BLL.Services;
using ISP.DAL.AutoFac;

namespace ISP.BLL.AutoFac
{
    public class AutoFacBLLConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<ServiceService>().As<IServiceService>();
            builder.RegisterType<UserServicesService>().As<IUserServicesService>();
            builder.RegisterType<PlanService>().As<IPlanService>();
            builder.RegisterType<DownloadService>().As<IDownloadService>();
            builder.RegisterType<UserPlanService>().As<IUserPlanService>();
            builder.RegisterType<FeatureService>().As<IFeatureService>();

            AutoFacDALConfig.Configure(builder);
        }
    }
}
