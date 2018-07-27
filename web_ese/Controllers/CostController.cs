using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ese.Models;
using web_ese.Models_Act.Cost;
using web_ese.Models_Db;

namespace web_ese.Controllers
{
    public class CostController : Controller
    {
		CostDbModels act = new CostDbModels();
		CommFunction comModel = new CommFunction();
		FilterSessionModels chk = new FilterSessionModels();

		// GET: Cost 배송요금
		public ActionResult CostIndex(string searchString,string msg)
        {

			//권한 체크===================================================
			if (!chk.chkPermission("CostIndex", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			CostIndexModels model = new CostIndexModels();
			
			
			model.nationArray = act.GetNationCodeSelectBox();
			model.stationArray = act.GetEstCodeSelectBox();
			
			return View(model);
        }

		



	}
}