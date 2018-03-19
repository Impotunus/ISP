using ISP.BLL.Interfaces;
using ISP.DAL.Context;
using ISP.DAL.UnitOfWork;

namespace ISP.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService()
        {
            return new UserService(new UnitOfWork(new ISPDBContext()));
        }
    }
}
