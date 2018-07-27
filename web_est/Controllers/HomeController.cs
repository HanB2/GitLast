#region Using

using System.Web.Mvc;
using System.Web.Security;

#endregion

namespace web_est.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: home/index
        public ActionResult Index()
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}