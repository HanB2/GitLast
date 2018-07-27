#region Using

using System.Web.Mvc;
using web_ese.Models_Act.Comm;
using web_ese.Models_Db;

#endregion

namespace web_ese.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
		HomeDbModels act = new HomeDbModels();
		
		// GET: home/index
		public ActionResult Index(string msg)
		{

			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			HomeModels model = new HomeModels();

			model = act.getBase(model);

			return View(model);
		}
	}
}