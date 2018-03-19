using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ISP.BLL.Interfaces;
using ISP.BLL.Utility;
using ISP.ViewModels;
using ISP.ViewModels.ProfileViewModels;
using PagedList;

namespace ISP.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class AdminController : Controller
    {
        private IUserService UserService { get; }

        public AdminController(IUserService userService)
        {
            UserService = userService;
        }

        public ActionResult Users(int page = 1)
        {
            var userDTOs = UserService.GetUsers();
            var userViewModels = AutoMapper.Mapper.Map<List<UserViewModel>>(userDTOs);
            var pagedUsers = userViewModels.ToPagedList(page, Constants.ItemsPerUsersPage);

            return View(pagedUsers);
        }

        public ActionResult FindUser(string userName)
        {
            var users = UserService.GetUsers().Where(t => t.UserName.ToLowerInvariant().StartsWith(userName.ToLowerInvariant()));
            if (!users.Any())
            {
                return View(new List<UserViewModel>());
            }
            var userViewModel = AutoMapper.Mapper.Map<List<UserViewModel>>(users);

            return View(userViewModel);
        }
    }
}