using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_esm.Models;

namespace web_esm.Controllers
{
    public class MarController : Controller
    {
		//추후 상세화면 컨트롤러나, httppost 가 생길 수 있으니 그때 권한 체크 다시 확인
		FilterSessionModels chk = new FilterSessionModels();

		// GET: Mar Mar 충전요청
		public ActionResult MarInReq(string msg)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("MarInReq", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			return View();
        }


		// GET: Mar Mar 출금요청(EST)
		public ActionResult MarOutEst(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("MarOutEst", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;



			return View();
		}

		// GET: Mar Mar 출금요청(ESE)
		public ActionResult MarOutEse(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("MarOutEse", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			return View();
		}

		// GET: Mar Mar 입출금 현황
		public ActionResult MarInOut(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("MarInOut", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			return View();
		}

	}
}