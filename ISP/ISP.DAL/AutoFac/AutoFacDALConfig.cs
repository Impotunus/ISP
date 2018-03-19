using Autofac;
using ISP.DAL.Context;
using ISP.DAL.Interfaces;

namespace ISP.DAL.AutoFac
{
    public class AutoFacDALConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            var database = new ISPDBContext();
            builder.RegisterType<UnitOfWork.UnitOfWork>().As<IUnitOfWork>().WithParameter("database", database);
        }
    }
}
