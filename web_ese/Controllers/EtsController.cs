using comm_model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ese.Models;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Ets;
using web_ese.Models_Act.Prod;
using web_ese.Models_Db;

namespace web_ese.Controllers
{
    public class EtsController : Controller
    {
		EtsDbModels act = new EtsDbModels();
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		FilterSessionModels chk = new FilterSessionModels();
		CommFunction comF = new CommFunction();

		// GET: Ets 배송 일반 배송 신청
		public ActionResult EtsReq(EtsReqModels model, string msg)
        {

			model = act.EtsReqSetBase(model);

			if (model.act_type == "updt")
			{
				model = act.EtsReqGetInfo(model);
			}
			else
			{
				model.act_type = "ins";
			}


			return View(model);
        }

		[HttpPost]
		public ActionResult EtsReq(EtsReqModels model)
		{
			string resultStr = "";

			StringSplitOptions option = new StringSplitOptions();

			string[] divideGoods = new string[1];
			divideGoods[0] = "^||^";

			string[] divideAttr = new string[1];
			divideAttr[0] = "^|^";
			
			string[] tmpGoods = model.goodsLst.Split(divideGoods, option);

			int itemSno = 0;
			foreach (var tmpItem in tmpGoods)
			{
				if (tmpItem != "") {
					string[] tmpData = tmpItem.Split(divideAttr, option);

					OrdGoods tmpGood = new OrdGoods();
					tmpGood.ITEMSN = itemSno;
					tmpGood.BARCODE = tmpData[0];
					tmpGood.GOODS_NAME = tmpData[1];
					tmpGood.BRAND = tmpData[2];
					tmpGood.PRICE = int.Parse(tmpData[3]);
					tmpGood.QTY = int.Parse(tmpData[4]);
					tmpGood.PURCHASE_URL = tmpData[5];
					tmpGood.HSCODE = tmpData[6];
					model.Items.Add(tmpGood);
					itemSno++;
				}
			}
			
			resultStr = act.EtsReqIns(model);
			
			return RedirectToAction("EtsLabel", "Ets", new { msg = resultStr });
			
		}


		// GET: Ets 배송 일반 배송 신청 바코드 조회
		[HttpPost]
		public JsonResult EtsReqChkBarcode(EtsReqModels model)
		{
			model.AjaxEseCode = (string)Session["ESE_CODE"];

			model = act.EtsReqChkBarcode(model);
			return Json(model.InItem);
		}

		/// GET: Ets 배송 일반 배송 신청 나라별 스테이션 조회
		[HttpPost]
		public JsonResult EtsReqChkStation(EtsReqModels model)
		{
			model.AjaxEseCode = (string)Session["ESE_CODE"];

			return Json(act.EtsReqChkStation(model));
		}

		// GET: Ets 배송 일반 배송 신청 나라 / 스테이션 / 도착국가 / 출고 타입 가져오기
		[HttpPost]
		public JsonResult EtsReqChkReleaseCode(EtsReqModels model)
		{
			model.AjaxEseCode = (string)Session["ESE_CODE"];

			return Json(act.EtsReqChkReleaseCode(model));
		}

		// GET: Ets 배송 일반 배송 신청 나라 / 스테이션 / 도착국가 / 출고 타입 / 요금표 가져오기
		[HttpPost]
		public JsonResult EtsReqChkCost(EtsReqModels model)
		{
			model.AjaxEseCode = (string)Session["ESE_CODE"];

			return Json(act.EtsReqChkCost(model));
		}





		// GET: Ets 배송 일반 배송 신청 - 보관상품 선택하기
		public ActionResult EtsReqSelect()
		{
			ProdDbModels actProd = new ProdDbModels();      //DB커넥션 클래스 선언
			ProdListModels model = new ProdListModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			//리스트 가져오기
			model = actProd.GetProdListList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EtsReqSelect(ProdListModels model)
		{
			ProdDbModels actProd = new ProdDbModels();      //DB커넥션 클래스 선언
			model = actProd.GetProdListList(model); //리스트 가져오기
			
			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}



		






		// GET: Ets 배송 일반 배송 신청 - 중국 주소 AJAX
		[HttpPost]
		public JsonResult PostCn(int level, int selectItem)
		{
			return Json(act.CnAddrSearch(level, selectItem));
		}

		// GET: Ets 배송 일반 배송 신청 - 미국 주소 AJAX
		[HttpPost]
		public JsonResult PostEn(int level, string selectItem)
		{
			return Json(act.UsAddrSearch(level, selectItem));
		}

		// GET: Ets 배송 일반 배송 신청 - 한국 주소 팝업
		[HttpPost]
		public JsonResult PostKr()
		{
			PostModels model = new PostModels();
			return Json(model);
		}
		






























		// GET: Ets 배송 대량 배송 신청
		public ActionResult EtsReqExcel()
        {
            return View();
        }











		// GET: Ets 배송 배송 라벨 출력
		public ActionResult EtsLabel(EtsLabelModels model, string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EtsLabel", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			model = act.EtsLabelSetBase(model);

			//셀렉트박스 세팅
			model.arrayENation = act.GetNationCodeSelectBox();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "WAYBILLNO";


			//리스트 가져오기
			model = act.GetEtsLabelList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
        }

		[HttpPost]
		public ActionResult EtsLabel(EtsLabelModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EtsLabel", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			// 선택 삭제일 경우
			if (model.act_type == "delChk")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EtsLabel", "PER_DELETE"))
					return RedirectToAction("EtsLabel", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.delEtsLabelList(model.delChk); //삭제
				model.act_type = "list";
				model.act_key = 0;
			}

			model.arrayENation = act.GetNationCodeSelectBox();

			/*
			foreach (schTypeArray tempS in model.arrayENation)
			{
				if (tempS.opt_key == model.Item.NATION_CODE)
				{
					model.Item.NATION_NAME = tempS.opt_value;
				}
			}
			*/

			//리스트 가져오기
			model = act.EtsLabelSetBase(model);
			model = act.GetEtsLabelList(model);

			//페이징 HTML 만들기
			ViewData["pageing"] = comM.setPaging(model.Paging);   

			return View(model);
		}







		// GET: Ets 배송 배송 상태 조회
		public ActionResult EtsList(EtsListModels model, string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EtsList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			model = act.EtsListSetBase(model);

			//셀렉트박스 세팅
			model.arrayENation = act.GetNationCodeSelectBox();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "WAYBILLNO";


			//리스트 가져오기
			model = act.GetEtsList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}


		[HttpPost]
		public ActionResult EtsList(EtsListModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EtsList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			// 선택 삭제일 경우
			if (model.act_type == "delChk")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EtsList", "PER_DELETE"))
					return RedirectToAction("EtsList", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.delEtsLabelList(model.delChk); //삭제
				model.act_type = "list";
				model.act_key = 0;
			}

			model.arrayENation = act.GetNationCodeSelectBox();

			/*
			foreach (schTypeArray tempS in model.arrayENation)
			{
				if (tempS.opt_key == model.Item.NATION_CODE)
				{
					model.Item.NATION_NAME = tempS.opt_value;
				}
			}
			*/

			//리스트 가져오기
			model = act.EtsListSetBase(model);
			model = act.GetEtsList(model);

			//페이징 HTML 만들기
			ViewData["pageing"] = comM.setPaging(model.Paging);

			return View(model);
		}

		public ActionResult EtsListView()
		{
			EtsListModels model = new EtsListModels();

			return View();
		}
		[HttpPost]
		public ActionResult EtsListView(EtsListModels model)
		{
			return View(model);
		}
	}
}