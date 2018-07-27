
using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_esm.Models;
using web_esm.Models_Act.Comm;
using web_esm.Models_Act.Cs;
using web_esm.Models_Db;

namespace web_esm.Controllers
{
    public class CsController : Controller
	{
		CsDbModels act = new CsDbModels();		//DB커넥션 클래스 선언
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		FilterSessionModels chk = new FilterSessionModels();

		// GET: Cs C/S 공지사항 관리
		public ActionResult CsNotice(string searchString, string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("CsNotice", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			CsNoticeModels model = new CsNoticeModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			//리스트 가져오기
			model = act.GetCsNoticeList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			

			return View(model);
        }

		[HttpPost]
		public ActionResult CsNotice(CsNoticeModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("CsNotice", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================



			//삭제일 경우
			if (model.act_type == "del")
			{

				//권한 체크===================================================
				if (!chk.chkPermission("CsNotice", "PER_DELETE"))
					return RedirectToAction("CsNotice", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.delCsNotice(model.act_key); //삭제

				model.act_type = "list";
				model.act_key = 0;

				//return RedirectToAction("csnotice", model); 삭제 후 ALERT 띄우기 아직 불확실
			}

			model = act.GetCsNoticeList(model); //리스트 가져오기
			
			if(TempData["PublicMsg"] != null)
				ViewBag.PublicMsg = TempData["PublicMsg"].ToString();

			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}

		public ActionResult CsNoticeView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("CsNotice", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			CsNoticeModels model = new CsNoticeModels();

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
				model.Item.WRITER_NAME = (string)Session["CURRENT_LOGIN_EMAIL"];
				model.Item.WRITER_ID = (int)Session["MANAGE_NO"];

				return View(model);
			}

			model.Item = act.GetCsNoticeView(model);
			model.RadioBoxPop = false;
			if (model.Item.POPUP_DISPLAY == 1)
				model.RadioBoxPop = true;
			


			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CsNoticeView(CsNoticeModels model)
		{

			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("CsNotice", "PER_INSERT"))
					return RedirectToAction("CsNoticeView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("CsNotice", "PER_UPDATE"))
					return RedirectToAction("CsNoticeView", new { msg = chk.alertStr });
				//===========================================================
			}


			// 유효성 검사
			if (!ModelState.IsValid)
				return View(model);
			
			// 라디오 버튼
			if (model.RadioBoxPop == true)
			{
				model.Item.POPUP_DISPLAY = 1;
			}
			else
			{
				model.Item.POPUP_DISPLAY = 0;
			}

			model.RadioBoxPop = false;
			if (model.Item.POPUP_DISPLAY == 1)
				model.RadioBoxPop = true;

			
			
			ViewBag.PublicPopupMsg = act.setCsNotice(model);

			return View(model);
		}






		// GET: Cs C/S 문의 사항 관리
		public ActionResult CsQna(string searchString, string msg)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("CsQna", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			CsDbModels act = new CsDbModels();
			CsQnaModels model = new CsQnaModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			//리스트 가져오기
			model = act.GetCsQnaList(model);
			
			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);

			
        }

		[HttpPost]
		public ActionResult CsQna(CsQnaModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("CsQna", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			model = act.GetCsQnaList(model);
			
			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}

		public ActionResult CsQnaView(string seqNo, string Msg)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("CsQna", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			CsQnaModels model = new CsQnaModels();
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

			model.Item = act.GetCsQnaView(model);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CsQnaView(CsQnaModels model)
		{

			


			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("CsQna", "PER_INSERT"))
					return RedirectToAction("CsQnaView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("CsQna", "PER_UPDATE"))
					return RedirectToAction("CsQnaView", new { msg = chk.alertStr });
				//===========================================================
			}

			//if (!ModelState.IsValid)
			//return View(model);

			if (!String.IsNullOrEmpty(Session["MANAGE_NO"] as string))
				model.Item.WRITER_ID = (int)Session["MANAGE_NO"];
			string PublicPopupMsg = act.setCsQnaModels(model);

			return RedirectToAction("CsQnaView", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}


	}
}