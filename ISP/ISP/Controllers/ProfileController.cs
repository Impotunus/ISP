using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ISP.Attributes;
using ISP.BLL.Interfaces;
using ISP.ViewModels;
using ISP.ViewModels.ProfileViewModels;
using ISP.ViewModels.ServiceViewModels;

namespace ISP.Controllers
{
    [Authorize]
    [RedirectBanned()]
    public class ProfileController : Controller
    {
        private IServiceService ServiceService { get; }

        private IUserService UserService { get; }

        private IUserServicesService UserServicesService { get; }

        private IUserPlanService UserPlanService { get; }

        public ProfileController(IServiceService serviceService, IUserService userService,
            IUserServicesService userServicesService, IUserPlanService userPlanService)
        {
            ServiceService = serviceService;
            UserService = userService;
            UserServicesService = userServicesService;
            UserPlanService = userPlanService;
        }

        [HttpGet]
        public ActionResult Index(string userName)
        {
            var userDTO = UserService.GetUser(userName);

            if (userDTO == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.Name != userName && !User.IsInRole("admin") && !User.IsInRole("manager"))
            {
                return new HttpStatusCodeResult(403);
            }

            var serviceDTOs = ServiceService.GetServices();
            var userSubscribedServices = UserServicesService.GetServicesOfTheUser(userName, true);
            var serviceSubscribtionsDictionary = new Dictionary<ServiceViewModel, bool>();

            foreach (var serviceDto in serviceDTOs)
            {
                var isUserSubscribed = userSubscribedServices.Any(t => t.Id == serviceDto.Id);
                serviceSubscribtionsDictionary.Add(AutoMapper.Mapper.Map<ServiceViewModel>(serviceDto), isUserSubscribed);
            }

            var profileViewModel = new ProfileViewModel
            {
                User = AutoMapper.Mapper.Map<UserViewModel>(userDTO),
                ServiceSubscribtions = serviceSubscribtionsDictionary,
                ActivePlans = UserPlanService.GetUserPlans(userName, true).Select(t => t.Plan.Title).ToList()
            };

            return View(profileViewModel);
        }

        [Authorize(Roles = "admin, manager")]
        [HttpGet]
        public ActionResult Ban(string userName)
        {
            if (!UserService.GetUser(userName).Role.Equals("admin"))
            {
                UserService.Ban(userName, true);
            }

            return Redirect(Request?.UrlReferrer?.ToString());
        }

        [Authorize(Roles = "admin, manager")]
        [HttpGet]
        public ActionResult UnBan(string userName)
        {
            if (!UserService.GetUser(userName).Role.Equals("admin"))
            {
                UserService.UnBan(userName);
            }

            return Redirect(Request?.UrlReferrer?.ToString());
        }

        [HttpPost]
        public ActionResult UnsubscribeFromService(SubscribeViewModel subscribeViewModel)
        {
            UserServicesService.UnsubscribeUserFromService(subscribeViewModel.userName,
                ServiceService.GetService(subscribeViewModel.serviceId));

            return Redirect(Request?.UrlReferrer?.ToString());
        }

        [HttpPost]
        public ActionResult SubscribeToService(SubscribeViewModel subscribeViewModel)
        {
            UserServicesService.SubscribeUserToService(subscribeViewModel.userName,
                ServiceService.GetService(subscribeViewModel.serviceId));

            return Redirect(Request?.UrlReferrer?.ToString());
        }
    }
}