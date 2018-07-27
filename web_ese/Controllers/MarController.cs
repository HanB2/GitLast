using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ese.Models;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Mar;
using web_ese.Models_Db;

namespace web_ese.Controllers
{
    public class MarController : Controller
    {
		MarDbModels act = new MarDbModels();
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		FilterSessionModels chk = new FilterSessionModels();
		CommFunction comModel = new CommFunction();


		// GET: Mar MAR 충전(PG) 보류 *************
		public ActionResult MarReqPg()
        {
            return View();
		}


		// MAR 충전(이체) ===================================================================
		// GET: Mar MAR 충전(이체)	mar_charge_req	
		public ActionResult MarReq()
        {

			//권한 체크===================================================
			if (!chk.chkPermission("MarReq", "PER_UPDATE"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			MarReqModels model = new MarReqModels();
			model = act.GetMarReqBase(model);
			
			model.currencyArray = comModel.GetConfCurrencySelectBox();

			return View(model);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult MarReq(MarReqModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("MarReq", "PER_UPDATE"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			model.Item.DEPOSIT_CURRENCY = Request.Form["CURRENCY_UNIT"];

			string PublicPopupMsg = act.SetMarReq(model);
			return RedirectToAction("MarInOut", new { msg = PublicPopupMsg });
		}



		

		//  Mar 충전/사용 이력 ================================================================
		// GET: Mar Mar 충전/사용 이력
		public ActionResult MarInOut(string msg , string type)
		{
			MarInOutModels model = new MarInOutModels();

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			TempData["getType"] = null;
			if (!string.IsNullOrEmpty(type))
			{
				TempData["getType"] = type;
			}
				
			return View(model);
		}


		[HttpPost]
		public ActionResult MarInOut(MarInOutModels model)
		{
			return View(model);
		}


		// GET: Mar MAR 충전/사용 이력  MAR 충전(계좌이체)===================================
		public ActionResult MarInOut1()
		{
			MarInOutModels model = new MarInOutModels();
			
			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;

			//리스트 가져오기
			model = act.GetMarInOut1(model);

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}
		
		[HttpPost]
		public ActionResult MarInOut1(MarInOutModels model)
		{
			//리스트 가져오기
			model = act.GetMarInOut1(model);

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}
		
		// GET: Mar MAR 충전/사용 이력 MAR 충전(PG) - 보류===================================
		public ActionResult MarInOut2()
		{
			MarInOutModels model = new MarInOutModels();
			return View(model);
		}
		
		[HttpPost]
		public ActionResult MarInOut2(MarInOutModels model)
		{
			return View(model);
		}


		// GET: Mar MAR 충전/사용 이력 MAR 환전===================================
		public ActionResult MarInOut3()
		{
			MarInOutModels model = new MarInOutModels();
			
			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;

			//리스트 가져오기
			model = act.GetMarInOut3(model);

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}


		[HttpPost]
		public ActionResult MarInOut3(MarInOutModels model)
		{
			//리스트 가져오기
			model = act.GetMarInOut3(model);

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}



		// GET: Mar MAR 충전/사용 이력 =================================================
		public ActionResult MarInOut4()
		{
			MarInOutModels model = new MarInOutModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;

			//리스트 가져오기
			model = act.GetMarInOut4(model);

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult MarInOut4(MarInOutModels model)
		{
			//리스트 가져오기
			model = act.GetMarInOut4(model);

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}


		// MAR 환불 신청 ===================================================================
		// GET: Mar MAR 환불 신청
		public ActionResult MarOutReq()
		{
			//권한 체크===================================================
			if (!chk.chkPermission("MarOutReq", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			MarOutReqModels model = new MarOutReqModels();

			model = act.GetMarOutReqBase(model);

			//계좌정보 입력값 확인
			if (model.CHK_DATA == false)
			{
				string PublicPopupMsg = "환불 계좌정보가 없습니다.";
				return View(model);
				//return RedirectToAction("MarOutReq","Mar", new { msg = PublicPopupMsg });
			}


			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult MarOutReq(MarOutReqModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("MarOutReq", "PER_UPDATE"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			string PublicPopupMsg = act.SetMarOutReq(model);


			return RedirectToAction("MarInOut", new { msg = PublicPopupMsg , type = 2});
		}




	}
}