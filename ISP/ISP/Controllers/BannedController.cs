using System.Web.Mvc;

namespace ISP.Controllers
{
    public class BannedController : Controller
    {
        public ActionResult BannedByAdmin()
        {
            return View();
        }

        public ActionResult BannedBySystem()
        {
            return RedirectToAction("PutMoney", "Balance", new { userName = User.Identity.Name });
        }
    }
}