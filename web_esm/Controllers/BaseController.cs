using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_esm.Models;
using web_esm.Models_Act.Base;
using web_esm.Models_Act.Comm;
using web_esm.Models_Db;

namespace web_esm.Controllers
{
    public class BaseController : Controller
    {
		BaseDbModels act = new BaseDbModels();
		CommFunction comModel = new CommFunction();
		FilterSessionModels chk = new FilterSessionModels();
		CommFunction comF = new CommFunction();

		//Base 통화 관리 =================================================================================================
		// GET: Base 통화 관리
		public ActionResult BaseCurrency(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseCurrency", "PER_SELECT"))
				return RedirectToAction("Index", "Home" , new { msg = chk.alertStr } );
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			BaseCurrencyModels model = new BaseCurrencyModels();
			model = act.GetBaseCurrencyList();
			return View(model);
		}

		[HttpPost]
		public ActionResult BaseCurrency(BaseCurrencyModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseCurrency", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			//환율 일괄 저장일 경우
			if (model.act_type == "set_AMNT")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseCurrency", "PER_INSERT"))
					return RedirectToAction("basecurrency", new { msg = chk.alertStr } );
				//===========================================================


				TempData["PublicMsg"] = act.setAMNTList(model); //삭제
				model.act_type = "list";
				model.act_key = 0;

				return RedirectToAction("BaseCurrency", model);
			}

			//삭제일 경우
			if (model.act_type == "del")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseCurrency", "PER_DELETE"))
					return RedirectToAction("basecurrency", new { msg = chk.alertStr });
				//===========================================================


				TempData["PublicMsg"] = act.delBaseCurrency(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;

				return RedirectToAction("BaseCurrency", model);
			}

			model = act.GetBaseCurrencyList();

			return View(model);
		}

		public ActionResult BaseCurrencyView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseCurrency", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			BaseCurrencyModels model = new BaseCurrencyModels();

			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
			}
			else
			{
				model.act_type = "ins";
				model.Item.SEQNO = 0;
			}

			if (!String.IsNullOrEmpty(Msg))
				ViewBag.PublicPopupMsg = Msg;


			model.Item = act.GetBaseCurrencyView(pSeqNo);


			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult BaseCurrencyView(BaseCurrencyModels model)
		{
			if(model.act_type == "ins")
			{
				//권한 체크===================================================
				if(!chk.chkPermission("BaseCurrency", "PER_INSERT"))
					return RedirectToAction("BaseCurrencyView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if(!chk.chkPermission("BaseCurrency", "PER_UPDATE"))
					return RedirectToAction("BaseCurrencyView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (!ModelState.IsValid)
				return View(model);

			string PublicPopupMsg = act.setBaseCurrency(model);
			return RedirectToAction("BaseCurrencyView", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}



		[HttpPost]
		public JsonResult GetExchangeRate()
		{
			BaseCurrencyModels model = new BaseCurrencyModels();
			model = act.GetBaseCurrencyList();
			double basicUnt = 0;
			foreach (var item in model.Items)
			{
				basicUnt = item.BASIC_UNIT * 100;
				item.AMNT = comModel.GetExchangeRate_ExRateOrg(item.CURRENCY_UNIT, "KRW", Convert.ToInt32(basicUnt));  // EXCHANGE-RATES.ORG 환율 가져오기
			}
			return Json(model.Items);
		}

		
		

		//Base 배송가능 국가 관리 =================================================================================================
		// GET: Base 배송가능 국가 관리
		public ActionResult BaseNation(string msg)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("BaseNation", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			BaseNationModels model = new BaseNationModels();
			model = act.GetBaseNationList();
			
			return View(model);
        }

		[HttpPost]
		public ActionResult BaseNation(BaseNationModels model)
		{
			

			//권한 체크===================================================
			if (!chk.chkPermission("BaseNation", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			string PublicMsg = "";
			//삭제일 경우
			if (model.act_type == "del")
			{

				//권한 체크===================================================
				if (!chk.chkPermission("BaseNation", "PER_DELETE"))
					return RedirectToAction("BaseNation", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.delBaseNation(model.act_key); //삭제

				model.act_type = "list";
				model.act_key = 0;
			}
			
			return RedirectToAction("BaseNation", new { Msg = PublicMsg });
		}


		public ActionResult BaseNationView(string seqNo, string Msg)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("BaseNation", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			BaseNationModels model = new BaseNationModels();
			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_key = pSeqNo;
				model.act_type = "updt";
			}
			else
			{
				model.act_type = "ins";
				model.Item.SEQNO = 0;
			}

			if(!String.IsNullOrEmpty(Msg))
				ViewBag.PublicPopupMsg = Msg;

			//단일 데이타 정보 가져오기
			model.Item = act.GetBaseNationView(pSeqNo);

			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = comModel.GetCommNationSelectBox();
			model.weightArray = comModel.GetWeightSelectBox();
			model.currencyArray = comModel.GetConfCurrencySelectBox();

			return View(model);
		}

		[HttpPost]
		public ActionResult BaseNationView(BaseNationModels model)
		{

			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseNation", "PER_INSERT"))
					return RedirectToAction("BaseNationView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseNation", "PER_UPDATE"))
					return RedirectToAction("BaseNationView", new { msg = chk.alertStr });
				//===========================================================
			}
			//유효성 검사
			if (!ModelState.IsValid)
				return View(model);


			model.nationArray = comModel.GetCommNationSelectBox();
			foreach (schTypeArray tempS in model.nationArray)
			{
				if (tempS.opt_key == model.Item.NATION_CODE)
				{
					model.Item.NATION_NAME = tempS.opt_value;
				}
			}

			model.Item.CURRENCY_UNIT = Request.Form["CURRENCY_UNIT"];
			model.Item.WEIGHT_UNIT = Request.Form["WEIGHT_UNIT"];
			string PublicPopupMsg = act.setBaseNation(model);
			
			return RedirectToAction("BaseNationView", new { seqNo = model.act_key , Msg  = PublicPopupMsg });
		}



		//Base 공항 관리 =================================================================================================
		// GET: Base 공항 관리
		public ActionResult BaseAirport(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseAirport", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//============================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			BaseAirportModels model = new BaseAirportModels();
			model = act.GetBaseAirportList(model);
			
			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = act.GetNationCodeSelectBox();

			return View(model);
		}

		[HttpPost]
		public ActionResult BaseAirport(BaseAirportModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseAirport", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================



			string PublicMsg = "";
			//삭제일 경우
			if (model.act_type == "del")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseAirport", "PER_DELETE"))
					return RedirectToAction("BaseAirport", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.delBaseAirport(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;

				
				return RedirectToAction("BaseAirport", new { Msg = PublicMsg });

			}
			model = act.GetBaseAirportList(model);

			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = comModel.GetCommNationSelectBox();
			return View(model);
		}

		public ActionResult BaseAirportView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseAirport", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			BaseAirportModels model = new BaseAirportModels();
			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
			}
			else
			{
				model.act_type = "ins";
				model.Item.SEQNO = 0;
			}

			if (!String.IsNullOrEmpty(Msg))
				ViewBag.PublicPopupMsg = Msg;

			//단일 데이타 정보 가져오기
			model.Item = act.GetBaseAirportView(pSeqNo);

			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = act.GetNationCodeSelectBox();

			return View(model);
		}

		[HttpPost]
		public ActionResult BaseAirportView(BaseAirportModels model)
		{
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseAirport", "PER_INSERT"))
					return RedirectToAction("BaseAirportView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseAirport", "PER_UPDATE"))
					return RedirectToAction("BaseAirportView", new { msg = chk.alertStr });
				//===========================================================
			}
			//유효성 검사
			if (!ModelState.IsValid)
				return View(model);

			string PublicPopupMsg = act.setBaseAirport(model);
			return RedirectToAction("BaseAirportView", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}



		//Base 현지 배송업체 설정 =================================================================================================
		// GET: Base 현지 배송업체 설정
		public ActionResult BaseLocal(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseLocal", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			BaseLocalModels model = new BaseLocalModels();
			model = act.GetBaseLocalList(model);

			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = act.GetNationCodeSelectBox();

			return View(model);
		}

		[HttpPost]
		public ActionResult BaseLocal(BaseLocalModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseLocal", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================



			string PublicMsg = "";
			//삭제일 경우
			if (model.act_type == "del")
			{

				//권한 체크===================================================
				if (!chk.chkPermission("BaseLocal", "PER_DELETE"))
					return RedirectToAction("BaseLocal", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.delBaseLocal(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;

				return RedirectToAction("BaseLocal", new { Msg = PublicMsg });

			}

			model = act.GetBaseLocalList(model);

			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = comModel.GetCommNationSelectBox();
			return View(model);

		}

		public ActionResult BaseLocalView(string seqNo, string Msg)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("BaseLocal", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			BaseLocalModels model = new BaseLocalModels();
			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
			}
			else
			{
				model.act_type = "ins";
				model.Item.SEQNO = 0;
			}

			if (!String.IsNullOrEmpty(Msg))
				ViewBag.PublicPopupMsg = Msg;


			model.Item = act.GetBaseLocalView(pSeqNo);

			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = act.GetNationCodeSelectBox();

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult BaseLocalView(BaseLocalModels model)
		{
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseLocal", "PER_INSERT"))
					return RedirectToAction("BaseLocalView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseLocal", "PER_UPDATE"))
					return RedirectToAction("BaseLocalView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (!ModelState.IsValid)
				return View(model);

			model.Item.NATION_CODE = model.schType;

			string PublicPopupMsg = act.setBaseLocal(model);


			

			return RedirectToAction("BaseLocalView", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}


		//Base 출고 타입 설정 =================================================================================================
		// GET: Base 출고 타입 설정
		[HttpGet]
		public ActionResult BaseOutPutType(string msg, BaseOutPutTypeModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseOutPutType", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			model = act.GetBaseOutPutTypeList(model);
			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = act.GetNationCodeSelectBox();

			return View(model);
		}
		

		[HttpPost]
		public ActionResult BaseOutPutType(BaseOutPutTypeModels model)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("BaseOutPutType", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			string PublicMsg = "";
			//삭제일 경우
			if (model.act_type == "del")
			{

				//권한 체크===================================================
				if (!chk.chkPermission("BaseOutPutType", "PER_DELETE"))
					return RedirectToAction("BaseOutPutType", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.delBaseOutPutType(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;

				return RedirectToAction("BaseOutPutType", new { Msg = PublicMsg });
			}

			model = act.GetBaseOutPutTypeList(model);

			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = comModel.GetCommNationSelectBox();
			return View(model);

		}

		public ActionResult BaseOutPutTypeView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("BaseOutPutType", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			BaseOutPutTypeModels model = new BaseOutPutTypeModels();
			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
			}
			else
			{
				model.act_type = "ins";
				model.Item.SEQNO = 0;
			}

			if (!String.IsNullOrEmpty(Msg))
				ViewBag.PublicPopupMsg = Msg;

			model.Item = act.GetBaseOutPutTypeView(pSeqNo);

			//SELECT BOX ARRAY 데이터 설정
			model.nationArray = act.GetNationCodeSelectBox();


			return View(model);
		}

		[HttpPost]
		public ActionResult BaseOutPutTypeView(BaseOutPutTypeModels model)
		{

			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseOutPutType", "PER_INSERT"))
					return RedirectToAction("BaseOutPutTypeView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("BaseOutPutType", "PER_UPDATE"))
					return RedirectToAction("BaseOutPutTypeView", new { msg = chk.alertStr });
				//===========================================================
			}

			//유효성 검사
			//if (!ModelState.IsValid)
				//return View(model);


			model.Item.NATION_CODE = model.schType;


			string PublicPopupMsg = act.setBaseOutPutType(model);
			return RedirectToAction("BaseOutPutTypeView", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		
		}

		
		public ActionResult BaseOutPutRate(string seqNo, string schType)
		{
			BaseOutPutRateModels model = new BaseOutPutRateModels();
			model.schType = schType;
			model.act_type = "list";
			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_key = pSeqNo;
				model = act.GetBaseOutPutRateView(model);
				return View(model);
			}

			//시퀸스 넘버 오류 일경우
			return RedirectToAction("BaseOutPutType", new { Msg = "잘못된 경로입니다." });
		}

		[HttpPost]
		public ActionResult BaseOutPutRate(BaseOutPutRateModels model)
		{

			string PublicMsg = "";
			string error_str = "";

			//목록 버튼 클릭
			if (model.act_type == "list")
			{
				BaseOutPutTypeModels returlModel = new BaseOutPutTypeModels();
				returlModel.schType = model.schType;
				return RedirectToAction("BaseOutPutType", returlModel);
			}
			
			if (model.act_type == "excel")
			{

				act.setBaseOutPutRate(model, out error_str);
				if(error_str != "")
					ViewBag.PublicMsg = error_str; 
				
				model = act.GetBaseOutPutRateView(model);
				return View(model);
			}


			//시퀸스 넘버 오류 일경우
			return RedirectToAction("BaseOutPutType", new { Msg = "잘못된 경로입니다." });
		}



		public ActionResult DownloadSample()
		{
			string contentType = "application/vnd.ms-excel";

			string download_filename = "CostTable.xls";
			string download_filepath = System.IO.Path.Combine(Server.MapPath("~/Content/Templete"), download_filename);
			
			return File(download_filepath, contentType, download_filename);
		}

		






	}
}
