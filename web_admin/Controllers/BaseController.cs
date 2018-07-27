using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_admin.Filter;

namespace web_admin.Controllers
{

	[CustomFilter]
	public class BaseController : Controller
    {
        
		//통화 관리
        public ActionResult Currency()
        {
            return View();
        }

		//배송가능 국가 관리
		public ActionResult Nation()
		{
			return View();
		}

		//공항 관리
		public ActionResult Airport()
		{
			return View();
		}

		//송장 번호 설정
		public ActionResult EtsNo()
		{
			return View();
		}

		//현지 배송업체 설정
		public ActionResult Local()
		{
			return View();
		}

		//출고 타입 설정
		public ActionResult OutPutType()
		{
			return View();
		}

		//출고 타입 설정 상세
		public ActionResult OutPutTypeView()
		{
			return View();
		}

	}
}