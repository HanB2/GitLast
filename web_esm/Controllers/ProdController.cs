using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_esm.Models;

namespace web_esm.Controllers
{
    public class ProdController : Controller
    {
		FilterSessionModels chk = new FilterSessionModels();

		// GET: Prod 통관 상품 관리 
		public ActionResult ProdList(string msg)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("ProdList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			return View();
        }
    }
}