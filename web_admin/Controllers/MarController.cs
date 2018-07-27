using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_admin.Filter;

namespace web_admin.Controllers
{
	[CustomFilter]
	public class MarController : Controller
    {
        public ActionResult InReq()
        {
            return View();
        }

		public ActionResult OutEst()
		{
			return View();
		}

		public ActionResult OutEse()
		{
			return View();
		}

		public ActionResult MarInOut()
		{
			return View();
		}
	}
}