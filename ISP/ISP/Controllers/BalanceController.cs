using System.Linq;
using System.Web.Mvc;
using ISP.BLL.DTO;
using ISP.BLL.Interfaces;
using ISP.IdentityExtensions;
using ISP.ViewModels.ProfileViewModels;

namespace ISP.Controllers
{
    [Authorize]
    public class BalanceController : Controller
    {
        private IUserService UserService { get; }

        private IUserPlanService UserPlanService { get; }

        public BalanceController(IUserService userService, IUserPlanService userPlanService)
        {
            UserService = userService;
            UserPlanService = userPlanService;
        }

        [HttpGet]
        public ActionResult PutMoney(string userName)
        {
            var balanceAddMoneyViewModel = new BalanceAddMoneyViewModel();

            if (User.IsInRole("banned") && !User.Identity.IsUserBannedByAdmin())
            {
                var requiredMoney = GetUserRequiredMoney(User.Identity.Name);
                balanceAddMoneyViewModel = new BalanceAddMoneyViewModel()
                {
                    Money = 0.0,
                    userName = userName,
                    RequiredMoney = requiredMoney
                };
            }
            else
            {
                balanceAddMoneyViewModel = new BalanceAddMoneyViewModel()
                {
                    Money = 0.0,
                    userName = userName,
                    RequiredMoney = 0
                };
            }

            return View(balanceAddMoneyViewModel);
        }

        [HttpPost]
        public ActionResult AddMoney(BalanceAddMoneyViewModel balanceAddMoneyViewModel)
        {
            if (!ModelState.IsValid)
            {
                balanceAddMoneyViewModel.RequiredMoney = 0.0;
                if (User.IsInRole("banned") && !User.Identity.IsUserBannedByAdmin())
                {
                    balanceAddMoneyViewModel.RequiredMoney = GetUserRequiredMoney(User.Identity.Name);
                }

                return View("PutMoney", balanceAddMoneyViewModel);
            }

            var newBank = UserService.GetUserBalance(balanceAddMoneyViewModel.userName) + balanceAddMoneyViewModel.Money;
            UserService.SetUserBalance(balanceAddMoneyViewModel.userName, newBank);

            if (User.IsInRole("banned") && !User.Identity.IsUserBannedByAdmin())
            {
                UnBanUser(User.Identity.Name);
            }

            return RedirectToAction("Index", "Profile", new { userName = balanceAddMoneyViewModel.userName });
        }

        private double GetUserRequiredMoney(string userName)
        {
            var requiredMoney = UserPlanService.GetUserPlans(userName)
                .Where(t => t.WillUnsubscribe == false)
                .Where(t => t.Status == ServiceStatusDTO.Deactivated)
                .Sum(t => t.Plan.Cost);

            return requiredMoney;
        }

        private void UnBanUser(string userName)
        {
            var userBalance = UserService.GetUser(userName).Balance;
            var requiredMoney = GetUserRequiredMoney(userName);

            if (userBalance >= requiredMoney)
            {
                UserPlanService.UnBan(userName);
            }
        }
    }
}