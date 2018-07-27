using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ese.Models;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Prod;
using web_ese.Models_Db;
using static web_ese.Models_Act.Prod.ProdListModels;

namespace web_ese.Controllers
{
    public class ProdController : Controller
    {
		ProdDbModels act = new ProdDbModels();      //DB커넥션 클래스 선언
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		FilterSessionModels chk = new FilterSessionModels();


		// GET: Prod 상품 등록
		public ActionResult ProdAdd(ProdListModels model, string msg)
        {

			//권한 체크===================================================
			if (!chk.chkPermission("ProdAdd", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			model.cateList1 = act.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기

			if (model.act_key == 0)
			{
				model.act_type = "ins";
			}
			else
			{
				model.Item = act.GetProdAddView(model);
				model.act_type = "updt";
				model.cateList2 = act.GetCategorySelectBox(2, model.Item.CATEGORY1);
				model.cateList3 = act.GetCategorySelectBox(3, model.Item.CATEGORY2);
				model.cateList4 = act.GetCategorySelectBox(4, model.Item.CATEGORY3);
			}

			return View(model);
        }

		[HttpPost]
		public ActionResult ProdAdd(ProdListModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("ProdAdd", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			model.Item.EST_CODE = (string)Session["EST_CODE"];
			model.Item.ESE_CODE = (string)Session["ESE_CODE"];
			string PublicPopupMsg = act.SetProdAdd(model);


			// 중복체크를 사용을 위해 act_key, schTxt2
			if (model.Item.BARCODE == "")
			{
				model.act_key = 0; //
			}
			else
			{
				model.act_key = 1; //
			}

			if (model.act_key == 0)
			{
				model.schTxt2 = "ins";
			}
			else
			{
				model.schTxt2 = "updt";
			}

			string chkDupl = act.ChkUpdtProdAdd(model);

			if (chkDupl != "")
			{

				return RedirectToAction("ProdAdd", new { msg = chkDupl });
			}

			if (model.act_type == "excel")
			{
				model.Excel = act.UploadExcelProd(model.Excel);
				TempData["PublicMsg"] = "등록 완료";
				model.act_type = "list";
				return View(model);
			}

			if (model.act_type == "ins")
			{
				return RedirectToAction("prodlist", new { msg = PublicPopupMsg });
			}

			


			model.Item = act.GetProdAddView(model);
			model.cateList1 = act.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기
			model.cateList2 = act.GetCategorySelectBox(2, model.Item.CATEGORY1);
			model.cateList3 = act.GetCategorySelectBox(3, model.Item.CATEGORY2);
			model.cateList4 = act.GetCategorySelectBox(4, model.Item.CATEGORY3);

			//TempData["PublicMsg"] = PublicPopupMsg;
			//return View(model);
			PublicPopupMsg = act.SetProdAdd(model);

			return RedirectToAction("ProdList", new { msg = PublicPopupMsg });
		}

		//카테고리 콤보박스 
		[HttpPost]
		public JsonResult GetCategory(int depth, int parent)
		{
			return Json(act.GetCategorySelectBox(depth, parent));
		}
		

		// GET: Prod 등록 상품 조회
		public ActionResult ProdList(string msg)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("ProdList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			ProdListModels model = new ProdListModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			model.Item.EST_CODE = (string)Session["EST_CODE"];
			model.Item.ESE_CODE = (string)Session["ESE_CODE"];

			//리스트 가져오기
			model.cateList1 = act.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기
			model = act.GetProdListList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}

		[HttpPost]
		public ActionResult ProdList(ProdListModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("ProdList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			// 삭제일 경우
			if (model.act_type == "del")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("ProdList", "PER_DELETE"))
					return RedirectToAction("ProdList", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.delProdList(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
			}

			// 선택 삭제일 경우
			if (model.act_type == "delChk")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("ProdList", "PER_DELETE"))
					return RedirectToAction("ProdList", new { msg = chk.alertStr });
				//===========================================================
				
				TempData["PublicMsg"] = act.delProdList(model.delChk); //삭제
				model.act_type = "list";
				model.act_key = 0;
			}


			model.cateList1 = act.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기
			model.cateList2 = act.GetCategorySelectBox(2, model.cate1);
			model.cateList3 = act.GetCategorySelectBox(3, model.cate2);
			model.cateList4 = act.GetCategorySelectBox(4, model.cate3);
			model = act.GetProdListList(model); //리스트 가져오기
			
			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}

		//상품 등록 엑셀 양식 다운로드
		[HttpPost]
		public ActionResult DownExcelProd(PROD_EXCEL model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("ProdList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			MemoryStream stream = act.DownExcelProd(model);
			return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SamPle.xlsx");  
				
		}

		

		//등록상품 조회 리스트 엑셀 다운로드
		[HttpPost]
		public ActionResult ProdListExcel(ProdListModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("ProdList", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			MemoryStream stream = act.GetProdListExcel(model);
			return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SearchResult.xlsx");

		}


	}
}