using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_admin.Controllers
{
    public class EsmController : Controller
    {
		//ESM 계정 그룹 관리
		public ActionResult EsmGrade()
		{
			return View();
		}


		//ESM 계정 관리
		public ActionResult EsmAccount()
		{
			return View();
		}

		//로그인 이력 조회
		public ActionResult LoginHis()
		{
			return View();
		}


	}
}