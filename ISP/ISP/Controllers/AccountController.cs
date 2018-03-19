using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ISP.BLL.DTO.Identity;
using ISP.BLL.Interfaces;
using ISP.BLL.Utility;
using ISP.ViewModels;
using ISP.ViewModels.AccountViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace ISP.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Profile", new {userName = User.Identity.Name});
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDTO = new UserDTO() { UserName = model.UserName, Password = model.Password };
                ClaimsIdentity claim = UserService.Authenticate(userDTO);

                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false,
                        AllowRefresh = true
                    }, claim);

                    return RedirectToAction("Index", "Profile", new { userName = claim.GetUserName() });
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDTO = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = "user"
                };

                OperationDetails operationDetails = UserService.Create(userDTO);

                if (operationDetails.Succedeed)
                {
                    return RedirectToAction("Index", "Profile", new { userName = userDTO.UserName });
                }
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }

            return View(model);
        }

        private void SeedAdmin()
        {
            UserService.SetInitialData(new UserDTO
            {
                Email = "admin@ma.il",
                UserName = "AdmiN",
                Password = "Password1.",
                FirstName = "Kirill",
                LastName = "Prihodko",
                Address = "Somewhere in th Universe",
                Role = "admin",
            }, new List<string> { "admin" });
        }
    }
}