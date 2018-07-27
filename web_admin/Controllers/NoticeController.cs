using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_admin.Filter;

namespace web_admin.Controllers
{
	[CustomFilter]
	public class NoticeController : Controller
    {

        public ActionResult Notice()
        {
            return View();
        }
    }
}