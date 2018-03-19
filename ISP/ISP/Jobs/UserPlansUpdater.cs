using Autofac;
using ISP.AutoFac;
using ISP.BLL.Interfaces;
using Quartz;

namespace ISP.Jobs
{
    public class UserPlansUpdater : IJob
    {
        private IUserService UserService { get; }

        private IUserPlanService UserPlanService { get; }

        public UserPlansUpdater()
        {
            UserService = AutoFacConfig.Container.Resolve<IUserService>();
            UserPlanService = AutoFacConfig.Container.Resolve<IUserPlanService>();
        }

        public void Execute(IJobExecutionContext context)
        {
            var users = UserService.GetUsers();

            foreach (var userDto in users)
            {
                UserPlanService.UpdateUserPlanSubscribtions(userDto.UserName);
            }
        }
    }
}