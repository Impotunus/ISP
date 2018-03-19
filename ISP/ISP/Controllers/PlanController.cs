using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;
using ISP.Attributes;
using ISP.BLL.DTO.Domain;
using ISP.BLL.Interfaces;
using ISP.ViewModels.PlanViewModels;
using ISP.ViewModels.ProfileViewModels;

namespace ISP.Controllers
{
    [RedirectBanned()]
    public class PlanController : Controller
    {
        private IPlanService PlanService { get; }

        private IServiceService ServiceService { get; }

        private IUserPlanService UserPlanService { get; }

        private IDownloadService DownloadService { get; }

        private IUserService UserService { get; set; }

        public PlanController(IPlanService planService, IDownloadService downloadService,
            IServiceService serviceService, IUserPlanService userPlanService, IUserService userService)
        {
            PlanService = planService;
            DownloadService = downloadService;
            ServiceService = serviceService;
            UserPlanService = userPlanService;
            UserService = userService;
        }

        public ActionResult Index(string userName, int serviceId, string sortBy = "Title", bool Asc = false)
        {
            var planDisplayViewModel = GetPlanDisplayViewModel(userName, serviceId, sortBy, Asc);
            var userUnsubscribedPlan = UserPlanService.GetUserPlans(userName, true).Where(t => t.Plan.ServiceId == serviceId).Any(t => t.WillUnsubscribe);
            planDisplayViewModel.IsUnsubscribeDisabled = userUnsubscribedPlan;

            return View(planDisplayViewModel);
        }

        [HttpGet]
        public ActionResult GetPlans()
        {
            var fileContents = DownloadService.GetServices();

            return File(fileContents, "text/plain", "Services-And-Plans.txt");
        }

        [Authorize(Roles = "admin")]
        public ActionResult GetPlansForAdmin(int serviceId)
        {
            var planDTOs = PlanService.GetPlans(serviceId);
            var planViewModels = AutoMapper.Mapper.Map<List<PlanViewModel>>(planDTOs);

            var outputViewModel = new PlanOutputViewModel()
            {
                PlanViewModels = planViewModels,
                ServiceId = serviceId,
                ServiceTitle = ServiceService.GetService(serviceId).Title
            };

            return View(outputViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult CreatePlan(int serviceId)
        {
            return View(new PlanCreateViewModel() {PlanViewModel = new PlanViewModel() {ServiceId = serviceId}, ServiceId = serviceId});
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult AddPlan(PlanViewModel planViewModel)
        {
            if (ModelState.IsValid)
            {
                PlanService.CreatePlan(AutoMapper.Mapper.Map<PlanDTO>(planViewModel));
                return RedirectToAction("GetPlansForAdmin", new {serviceId = planViewModel.ServiceId});
            }

            var createViewModel = new PlanCreateViewModel();
            createViewModel.PlanViewModel = planViewModel;
            createViewModel.ServiceId = planViewModel.ServiceId;

            return View("CreatePlan", createViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult EditPlan(int planId)
        {
            var planDTO = PlanService.GetPlan(planId);
            var planViewModel = AutoMapper.Mapper.Map<PlanViewModel>(planDTO);
            var editViewModel = new PlanCreateViewModel();
            editViewModel.PlanViewModel = planViewModel;
            editViewModel.ServiceId = planDTO.ServiceId;

            return View(editViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult UpdatePlan(PlanViewModel planViewModel)
        {
            if (ModelState.IsValid)
            {
                PlanService.UpdatePlan(AutoMapper.Mapper.Map<PlanDTO>(planViewModel));
                return RedirectToAction("GetPlansForAdmin", new { serviceId = planViewModel.ServiceId });
            }
            var editViewModel = new PlanCreateViewModel();
            editViewModel.PlanViewModel = planViewModel;
            editViewModel.ServiceId = planViewModel.ServiceId;

            return View("EditPlan", editViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult DeletePlan(int planId)
        {
            var serviceId = PlanService.GetPlan(planId).ServiceId;
            PlanService.DeletePlan(planId);

            return RedirectToAction("GetPlansForAdmin", new {serviceId = serviceId });
        }

        [HttpPost]
        public ActionResult UnsubscribeFromPlan(SubscribeToPlanViewModel subscribeViewModel)
        {
            var result = UserPlanService.UnsubscribeUserFromPlan(subscribeViewModel.UserName,
                PlanService.GetPlan(subscribeViewModel.PlanId));

            var service = PlanService.GetPlan(subscribeViewModel.PlanId).Service;
            var planDisplayViewModel = GetPlanDisplayViewModel(subscribeViewModel.UserName, service.Id);

            var userUnsubscribedPlan = UserPlanService.GetUserPlans(subscribeViewModel.UserName, true).First(t => t.PlanId == subscribeViewModel.PlanId);
            planDisplayViewModel.IsUnsubscribeDisabled = userUnsubscribedPlan.WillUnsubscribe;

            if (result.Succedeed)
            {
                planDisplayViewModel.Message = Messages.Messages.UserWillBeUnsubscribed;
            }
            else
            {
                planDisplayViewModel.Message = Messages.Messages.UserIsNotSubscribed;
            }
          
            return View("Index", planDisplayViewModel);
        }

        [HttpPost]
        public ActionResult SubscribeToPlan(SubscribeToPlanViewModel subscribeViewModel)
        {
            var result = UserPlanService.SubscribeUserToPlan(subscribeViewModel.UserName,
                PlanService.GetPlan(subscribeViewModel.PlanId));

            var service = PlanService.GetPlan(subscribeViewModel.PlanId).Service;
            var planDisplayViewModel = GetPlanDisplayViewModel(subscribeViewModel.UserName, service.Id);

            if (result.Succedeed)
            {
                planDisplayViewModel.Message = Messages.Messages.UserHasBeenSubscribed;
            }
            else
            {
                if (result.Property == "money")
                {
                    planDisplayViewModel.Message = Messages.Messages.UserHasNotEnoughMoney;
                }
                if (result.Property == "plans")
                {
                    planDisplayViewModel.Message = Messages.Messages.UserIsAlreadySubscribedToPlan;
                }

            }

            return View("Index", planDisplayViewModel);
        }

        private PlanDisplayViewModel GetPlanDisplayViewModel(string userName, int serviceId, string sortBy = "Title", bool Asc = false)
        {
            var planDTOs = PlanService.GetPlans(serviceId);
            var planViewModel = AutoMapper.Mapper.Map<List<PlanViewModel>>(planDTOs);

            var planSubscribtionsDictionary = new Dictionary<PlanViewModel, bool>();
            var userSubscribedPlans = UserPlanService.GetUserPlans(userName, true);

            if (sortBy.ToLowerInvariant() == "title")
            {
                planViewModel = Asc ? planViewModel.OrderBy(t => t.Title).ToList()
                    : planViewModel.OrderByDescending(t => t.Title).ToList();
            }
            else if (sortBy.ToLowerInvariant() == "cost")
            {
                planViewModel = Asc ? planViewModel.OrderBy(t => t.Cost).ToList()
                    : planViewModel.OrderByDescending(t => t.Cost).ToList();
            }

            var planDisplayViewModel = new PlanDisplayViewModel()
            {
                PlanViewModels = planViewModel,
                ServiceTitle = ServiceService.GetService(serviceId).Title,
                User = AutoMapper.Mapper.Map<UserViewModel>(UserService.GetUser(userName)),
                PlanSubscribtionsDictionary = planSubscribtionsDictionary
            };

            foreach (var viewModel in planViewModel)
            {
                var isUserSubscribed = userSubscribedPlans.Any(t => t.PlanId == viewModel.Id);
                planSubscribtionsDictionary.Add(viewModel, isUserSubscribed);
            }

            return planDisplayViewModel;
        }
    }
}