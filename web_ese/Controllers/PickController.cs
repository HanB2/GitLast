using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_ese.Controllers
{
    public class PickController : Controller
    {
		// GET: Pick 픽업 신청 보류 *************
		public ActionResult PickReq()
        {
            return View();
        }

		// GET: Pick 픽업 신청 조회 보류 *************
		public ActionResult PickList()
        {
            return View();
        }
    }
}