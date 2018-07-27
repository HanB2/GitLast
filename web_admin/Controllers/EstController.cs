using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_admin.Filter;

namespace web_admin.Controllers
{
	[CustomFilter]
	public class EstController : Controller
    {
        
        public ActionResult Manage()
        {
            return View();
        }

		public ActionResult Account()
		{
			return View();
		}

		public ActionResult InOut()
		{
			return View();
		}

		public ActionResult InOutStat()
		{
			return View();
		}
	}
}