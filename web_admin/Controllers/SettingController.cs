using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_admin.Filter;

namespace web_admin.Controllers
{
	[CustomFilter]
	public class SettingController : Controller
    {

        public ActionResult Mail()
        {
            return View();
        }

		public ActionResult SetAccount()
		{
			return View();
		}

		public ActionResult Log()
		{
			return View();
		}

	}
}