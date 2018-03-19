using System.Web.Mvc;

namespace ISP.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("~/Views/Shared/Errors/404.cshtml");
        }

        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View("~/Views/Shared/Errors/403.cshtml");
        }
    }
}