using comm_model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ese.Models;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Ets;
using web_ese.Models_Act.Prod;
using web_ese.Models_Act.Stoc;
using web_ese.Models_Db;

namespace web_ese.Controllers
{
    public class StocController : Controller
    {
		StocDbModels act = new StocDbModels();      //DB커넥션 클래스 선언
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		FilterSessionModels chk = new FilterSessionModels();

		//보관 신청==========================================================================================
		// GET: Stoc 보관 신청
		public ActionResult StocReq(StocReqModels model, string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("StocReq", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			model = act.StocReqSetBase(model);
			
			if (model.act_key != 0)
			{
				model.act_type = "updt";
				model = act.GetStocReqView(model);
			}
			else
			{
				model.act_type = "ins";
				model.Item.SEQNO = 0;
			}

			if (model.act_type == "excel")
			{
				model.Excel = act.UploadExcelProd(model.Excel);
				TempData["PublicMsg"] = "등록 완료";
				model.act_type = "list";
				return View(model);
			}


			return View(model);
        }

		[HttpPost]
		public ActionResult StocReq(StocReqModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("StocReq", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			StringSplitOptions option = new StringSplitOptions();

			string[] divideGoods = new string[1];
			divideGoods[0] = "^||^";

			string[] divideAttr = new string[1];
			divideAttr[0] = "^|^";

			string[] tmpGoods = model.goodsLst.Split(divideGoods, option);

			foreach (var tmpItem in tmpGoods)
			{
				if (tmpItem != "")
				{
					string[] tmpData = tmpItem.Split(divideAttr, option);

					StcInReqGoods tmpGood = new StcInReqGoods();
					tmpGood.BARCODE = tmpData[0];
					tmpGood.BOXNUM = tmpData[1];
					tmpGood.CNT = int.Parse(tmpData[2]);
					model.Items.Add(tmpGood);
				}
			}
			
			string PublicPopupMsg = act.SetStocReq(model);

			if (model.act_type == "ins")
			{
				return RedirectToAction("StocReqList", new { msg = PublicPopupMsg });
			}

			model = act.StocReqSetBase(model);
			model = act.GetStocReqView(model);
			TempData["PublicMsg"] = PublicPopupMsg;
			return View(model);
		}

		// GET : 상품 선택
		public ActionResult StocReqSelect()
		{
			ProdDbModels actProd = new ProdDbModels();      //DB커넥션 클래스 선언

			ProdListModels model = new ProdListModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			model.Item.EST_CODE = (string)Session["EST_CODE"];
			model.Item.ESE_CODE = (string)Session["ESE_CODE"];

			//리스트 가져오기
			model.cateList1 = actProd.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기
			model = actProd.GetProdListList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}

		[HttpPost]
		public ActionResult StocReqSelect(ProdListModels model)
		{
			ProdDbModels actProd = new ProdDbModels();      //DB커넥션 클래스 선언

			model.cateList1 = actProd.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기
			model.cateList2 = actProd.GetCategorySelectBox(2, model.cate1);
			model.cateList3 = actProd.GetCategorySelectBox(3, model.cate2);
			model.cateList4 = actProd.GetCategorySelectBox(4, model.cate3);
			model = actProd.GetProdListList(model); //리스트 가져오기

			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}






























		// GET: Stoc 보관 신청 바코드 조회
		[HttpPost]
		public JsonResult StocReqChkBarcode(StocReqModels model)
		{
			model.AjaxEseCode = (string)Session["ESE_CODE"];
			model.AjaxEstCode = (string)Session["EST_CODE"];
			model = act.StocReqChkBarcode(model);
			return Json(model.InItem);
		}
		
		//보관 신청 현황 조회====================================================================================

		// GET: Stoc 보관 신청 현황 조회
		public ActionResult StocReqList()
		{
			//권한 체크===================================================
			if (!chk.chkPermission("StocReqList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			StocReqModels model = new StocReqModels();
			model = act.StocReqSetBase(model);

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			//리스트 가져오기
			model = act.GetStocReqList(model);

			model.arrayStation = new List<schTypeArray>();

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}

		[HttpPost]
		public ActionResult StocReqList(StocReqModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("StocReqList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			//삭제일 경우
			if (model.act_type == "del")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("StocReqList", "PER_DELETE"))
					return RedirectToAction("StocReqList", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.delStocReqList(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
			}

			// 선택 삭제일 경우
			if (model.act_type == "delChk")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("StocReqList", "PER_DELETE"))
					return RedirectToAction("StocReqList", new { msg = chk.alertStr });
				//===========================================================
				
				TempData["PublicMsg"] = act.delStocReqList(model.delChk); //삭제
				model.act_type = "list";
				model.act_key = 0;
			}

			model = act.StocReqSetBase(model);
			model = act.GetStocReqList(model); //리스트 가져오기
			
			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}



		public ActionResult StocReqListView(string seqNo, string msg)
		{
			
			//권한 체크===================================================
			if (!chk.chkPermission("StocReqList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			StocReqModels model = new StocReqModels();
			model = act.StocReqSetBase(model);
			
			int pSeqNo = 0;

			model.act_type = "view";
			model.act_key = pSeqNo;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
				model = act.GetStocReqView(model);
			}

			return View(model);
		}

		[HttpPost]
		public ActionResult StocReqListView(StocReqModels model)
		{
			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("StocReqList", "PER_UPDATE"))
					return RedirectToAction("StocReqListView", new { msg = chk.alertStr });
				//===========================================================

				ViewBag.PublicPopupMsg  = act.chkStocReq(model.act_key);
			}

			model = act.StocReqSetBase(model);
			model = act.GetStocReqView(model);
			return View(model);
		}




		//재고 조회=========================================================================================


		// GET: Stoc 재고조회
		public ActionResult StocList()
        {
			//권한 체크===================================================
			if (!chk.chkPermission("StocList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			StocListModels model = new StocListModels();

			model = act.StocListBase(model);

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "BARCODE";

			//리스트 가져오기
			model = act.GetStocList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기
			
			return View(model);
        }

		[HttpPost]
		public ActionResult StocList(StocListModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("StocList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			model = act.StocListBase(model);

			ProdDbModels actProd = new ProdDbModels();
			EtsDbModels actEtc = new EtsDbModels();

			EtsReqModels tmpModel = new EtsReqModels();
			tmpModel.AjaxEseCode = (string)Session["ESE_CODE"];
			tmpModel.AjaxNationCode = model.schNation;

			model.arrayStation = actEtc.EtsReqChkStation(tmpModel);

			model.cateList1 = actProd.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기
			model.cateList2 = actProd.GetCategorySelectBox(2, model.cate1);
			model.cateList3 = actProd.GetCategorySelectBox(3, model.cate2);
			model.cateList4 = actProd.GetCategorySelectBox(4, model.cate3);
			model = act.GetStocList(model); //리스트 가져오기

			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}



		[HttpPost]
		public ActionResult StocListExcel(StocListModels model)
		{
			MemoryStream stream = act.GetStocListExcel(model);
			return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SearchResult.xlsx");
		}


		public ActionResult StocProdView(string barcode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("StocList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			StcGoods model = new StcGoods();
			
			model.BARCODE = barcode;
			model.ESE_CODE = (string)Session["ESE_CODE"];

			//Prod 컨트롤러  GetProdAddView 참조 대신 검색 조건이 ESE_CDOE + barcode 두게 걸어야 함

			model = act.getStocProdView(model);


			return View(model);
		}


		public ActionResult StocInOutView(string barcode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("StocList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			StocInOutModels model = new StocInOutModels();


			model.BaseBarcode = barcode;
			model.BaseEseCode = (string)Session["ESE_CODE"];

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "BARCODE";


			//act.GetStocInOut(model); 참조 검색조건 없이 바코드 + ESE_CODE  검색

			model = act.StocInOutView(model);

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}



		//입출고 내역 조회=================================================================================================================



		// GET: Stoc 입출고 내역 조회
		public ActionResult StocInOut()
        {
			//권한 체크===================================================
			if (!chk.chkPermission("StocInOut", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			StocInOutModels model = new StocInOutModels();

			model = act.StocInOutBase(model);

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "BARCODE";
			
			//리스트 가져오기
			model = act.GetStocInOut(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기
			
			return View(model);
        }

		[HttpPost]
		public ActionResult StocInOut(StocInOutModels model)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("StocInOut", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			model = act.StocInOutBase(model);

			ProdDbModels actProd = new ProdDbModels();
			EtsDbModels actEtc = new EtsDbModels();

			EtsReqModels tmpModel = new EtsReqModels();
			tmpModel.AjaxEseCode = (string)Session["ESE_CODE"];
			tmpModel.AjaxNationCode = model.schNation;

			model.arrayStation = actEtc.EtsReqChkStation(tmpModel);

			model.cateList1 = actProd.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기
			model.cateList2 = actProd.GetCategorySelectBox(2, model.cate1);
			model.cateList3 = actProd.GetCategorySelectBox(3, model.cate2);
			model.cateList4 = actProd.GetCategorySelectBox(4, model.cate3);

			model = act.GetStocInOut(model); //리스트 가져오기

			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}
	}
}