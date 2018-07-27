using System.Web.Mvc;
using web_esm.Models_Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Setting;
using web_esm.Models;

namespace web_esm.Controllers
{
    public class SettingController : Controller
    {
		FilterSessionModels chk = new FilterSessionModels();

		// GET: Setting 설정 메일서버 설정
		public ActionResult SettingEmail(string msg)
        {

			//권한 체크===================================================
			if (!chk.chkPermission("SettingEmail", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

		
			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			SettingDbModels act = new SettingDbModels();
			SettingEmailModels returnModel = act.GetEmailSettingModel();

			return View(returnModel);
        }

	
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SettingEmail(SettingEmailModels viewModel)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("SettingEmail", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			if (!ModelState.IsValid)
				return View(viewModel);

			SettingDbModels act = new SettingDbModels();

			string message =  "성공";

			if (act.SetEmailSettingModel(viewModel))  //
			{
				return RedirectToAction("SettingEmail", "Setting", new {msg = message });
			}

			message = "실패";
			ViewBag.result = message;

	

			return View(viewModel);
		}



	}
}