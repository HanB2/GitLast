using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ese.Models;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Cs;
using web_ese.Models_Db;



namespace web_ese.Controllers
{
    public class CsController : Controller
	{
		CsDbModels act = new CsDbModels();      //DB커넥션 클래스 선언
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		FilterSessionModels chk = new FilterSessionModels();

		// GET: Cs 고객센터 ETOMARS 공지
		public ActionResult CsEsmNotice()
        {
			//권한 체크===================================================
			if (!chk.chkPermission("CsEsmNotice", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			CsEsmNoticeModels model = new CsEsmNoticeModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			//리스트 가져오기
			model = act.GetCsEsmNoticeList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}

		[HttpPost]
		public ActionResult CsEsmNotice(CsEsmNoticeModels model)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("CsEsmNotice", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			model = act.GetCsEsmNoticeList(model); //리스트 가져오기
			
			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}

		public ActionResult CsEsmNoticeView(string seqNo)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("CsEsmNotice", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			CsEsmNoticeModels model = new CsEsmNoticeModels();
			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
			}

			model.Item = act.GetCsEsmNoticeView(model);

			return View(model);
		}
		
		// GET: Cs 고객센터 STATION 공지
		public ActionResult CsEstNotice()
		{
			//권한 체크===================================================
			if (!chk.chkPermission("CsEstNotice", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			CsEstNoticeModels model = new CsEstNoticeModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			//리스트 가져오기
			model = act.GetCsEstNoticeList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
		}

		[HttpPost]
		public ActionResult CsEstNotice(CsEstNoticeModels model)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("CsEstNotice", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			model = act.GetCsEstNoticeList(model); //리스트 가져오기

			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}

		public ActionResult CsEstNoticeView(string seqNo)
		{
			CsEstNoticeModels model = new CsEstNoticeModels();
			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
			}

			model.Item = act.GetCsEstNoticeView(model);

			return View(model);
		}


		// GET: Cs 고객센터 1:1 문의
		public ActionResult CsQna()
        {
			//권한 체크===================================================
			if (!chk.chkPermission("CsQna", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			CsDbModels act = new CsDbModels();
			CsQnaModels model = new CsQnaModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			//리스트 가져오기
			model = act.GetCsQnaList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			return View(model);
        }


		[HttpPost]
		public ActionResult CsQna(CsQnaModels model)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("CsQna", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			model = act.GetCsQnaList(model); //리스트 가져오기
			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			return View(model);
		}


		public ActionResult CsQnaView(string seqNo, string Msg, string act_type)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("CsQna", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			
			CsQnaModels model = new CsQnaModels();

					int pSeqNo = 0;
					if (int.TryParse(seqNo, out pSeqNo))
					{

						model.act_type = act_type;
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
					return RedirectToAction("CsQna", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("CsQna", "PER_UPDATE"))
					return RedirectToAction("CsQna", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "view")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("CsQna", "PER_UPDATE"))
					return RedirectToAction("CsQna", new { msg = chk.alertStr });
				//===========================================================
			}
			model.Item.QNA_TYPE = int.Parse(model.schType);
			
			// 유효성 검사
			if (!ModelState.IsValid)
				return View(model);

			model.Item.ESE_CODE = (string)Session["ESE_CODE"];
			model.Item.WRITER_ID = (int)Session["MANAGE_NO"];
			
			string PublicPopupMsg = act.setCsQnaModels(model); 
			return RedirectToAction("CsQnaView", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}
	}
}