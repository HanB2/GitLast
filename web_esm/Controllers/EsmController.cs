using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_esm.Models;
using web_esm.Models_Act.Comm;
using web_esm.Models_Act.Esm;
using web_esm.Models_Db;

namespace web_esm.Controllers
{
    public class EsmController : Controller
    {
		EsmDbModels act = new EsmDbModels();    //DB커넥션 클래스 선언
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		CommFunction comF = new CommFunction();
		FilterSessionModels chk = new FilterSessionModels();

		// GET:ESM 관리자 설정 ESM 계정 그룹 관리
		public ActionResult EsmGrade(string msg)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("EsmGrade", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EsmGradeModels model = new EsmGradeModels();

			model = act.GetEsmGradeList();


			return View(model);
        }

		[HttpPost]
		public ActionResult EsmGrade(EsmGradeModels model)
		{

			//삭제일 경우
			if (model.act_type == "del")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EsmGrade", "PER_DELETE"))
					return RedirectToAction("EsmGrade", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.DelEsmGrade(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
				//return RedirectToAction("EsmGrade", model); 삭제 후 ALERT 띄우기 아직 불확실
			}

			model = act.GetEsmGradeList(); //리스트 가져오기
			

			return View(model);
		}

		public ActionResult EsmGradeView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EsmGrade", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			EsmGradeViewModels model = new EsmGradeViewModels();

			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
			}
			else
			{
				model.act_type = "ins";
				model.Item.GROUP_ID = 0;
			}

			if (!String.IsNullOrEmpty(Msg))
				ViewBag.PublicPopupMsg = Msg;

			model = act.GetEsmGradeView(model);


			//메뉴명 가져오기
			List<schTypeArray> tempGrade = comF.GetGradeList();

			for (int i = 0; i < model.Items.Count; i++)
			{
				foreach (schTypeArray tempS in tempGrade)
				{
					if (tempS.opt_key == model.Items[i].MENU_ID)
					{
						model.Items[i].MENU_NAME = tempS.opt_value;
					}
				}
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EsmGradeView(EsmGradeViewModels model)
		{

			//esm_group_permission 권한
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EsmGrade", "PER_INSERT"))
					return RedirectToAction("EsmGradeView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EsmGrade", "PER_UPDATE"))
					return RedirectToAction("EsmGradeView", new { msg = chk.alertStr });
				//===========================================================
			}

			for (int i = 0; i < model.Items.Count; i++)
			{
				model.Items[i].PER_SELECT = 0;
				model.Items[i].PER_INSERT = 0;
				model.Items[i].PER_UPDATE = 0;
				model.Items[i].PER_DELETE = 0;

				if (model.Items[i].CHK_PER_SELECT) model.Items[i].PER_SELECT = 1;
				if (model.Items[i].CHK_PER_INSERT) model.Items[i].PER_INSERT = 1;
				if (model.Items[i].CHK_PER_UPDATE) model.Items[i].PER_UPDATE = 1;
				if (model.Items[i].CHK_PER_DELETE) model.Items[i].PER_DELETE = 1;
			}

			//그룹명 유효성 검사
			if (!ModelState.IsValid)
				return View(model);

			string PublicPopupMsg = act.SetEsmGrade(model);
			return RedirectToAction("EsmGradeView", new { Msg = PublicPopupMsg });
		}



		// GET:ESM 관리자 설정 ESM 계정 관리
		public ActionResult EsmAccount(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EsmAccount", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EsmAccountModels model = new EsmAccountModels();
			model = act.GetEsmAccountList(model);
			
			return View(model);
		}

		[HttpPost]
		public ActionResult EsmAccount(EsmAccountModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EsmAccount", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			//삭제일 경우
			if (model.act_type == "del")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EsmAccount", "PER_DELETE"))
					return RedirectToAction("EsmAccount", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.DelEsmAccount(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
				return RedirectToAction("EsmAccount", model);
			}

			model = act.GetEsmAccountList(model);

			if (TempData["PublicMsg"] != null)
				ViewBag.PublicMsg = TempData["PublicMsg"].ToString();

			return View(model);
		}

		public ActionResult EsmAccountView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EsmAccount", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			EsmAccountModels model = new EsmAccountModels();

			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
			}
			else
			{
				model.act_type = "ins";
				model.Item.GROUP_ID = 0;
			}

			if (!String.IsNullOrEmpty(Msg))
				ViewBag.PublicPopupMsg = Msg;

			model.Item = act.GetEsmAccountView(model);
			model.RadioBoxPop = false;
			if (model.Item.STATUS == 1)
				model.RadioBoxPop = true;

			//SELECT BOX ARRAY 데이터 설정 comF에 그룹 id 추가해야한다.
			model.GroupIdArray = comF.GroupIdSelectBox();


			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EsmAccountView(EsmAccountModels model)
		{
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EsmAccount", "PER_INSERT"))
					return RedirectToAction("EsmAccountView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EsmAccount", "PER_UPDATE"))
					return RedirectToAction("EsmAccountView", new { msg = chk.alertStr });
				//===========================================================
			}


			//if (!ModelState.IsValid)
				//return View(model);

			//라디오버튼 쓰기
			if (model.RadioBoxPop == true)
			{
				model.Item.STATUS = 1;
			}
			else
			{
				model.Item.STATUS = 0;
			}

			model.Item.GROUP_ID = model.GroupId;

			model.RadioBoxPop = false;
			if (model.Item.STATUS == 1)
				model.RadioBoxPop = true;

			//SELECT BOX ARRAY 데이터 설정
			model.GroupIdArray = comF.GroupIdSelectBox();

			//권한 관련과 같이 갈 것들 이걸 적용시키면 Radio 버튼이 안된다.추후 같이 갈 수 있는 방식을 찾아야 한다.
			//string PublicPopupMsg = act.SetEsmAccount(model);
			//return RedirectToAction("EsmAccountView", new { seqNo = model.act_key, Msg = PublicPopupMsg });

			//string PublicPopupMsg = act.SetEsmAccount(model);
			//return RedirectToAction("EsmAccount", new { seqNo = model.act_key, Msg = PublicPopupMsg });

			ViewBag.PublicPopupMsg = act.SetEsmAccount(model);
			return View(model);
		}




		// GET:ESM 관리자 설정 로그인 이력 조회
		public ActionResult EsmLoginHis()
		{
			EsmLoginHisModels model = new EsmLoginHisModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			model = act.GetEsmLoginHisList(model);	//리스트 가져오기

			model.schTypeArray = comF.GetEsmUserSelectBox();    //관리자 ID 가져오기

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기
			return View(model);
        }

		[HttpPost]
		public ActionResult EsmLoginHis(EsmLoginHisModels model)
		{
			model = act.GetEsmLoginHisList(model); //리스트 가져오기

			model.schTypeArray = comF.GetEsmUserSelectBox();	//관리자 ID 가져오기

			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기
			return View(model);
		}
	}
}