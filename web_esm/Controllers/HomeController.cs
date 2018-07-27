#region Using

using System.Web.Mvc;

#endregion

namespace web_esm.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: home/index
        public ActionResult Index( string msg )
        {
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg; 

            return View();
        }
    }
}