using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ese.Models;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Ese;
using web_ese.Models_Db;

namespace web_ese.Controllers
{
    public class EseController : Controller
    {
		EseDbModels act = new EseDbModels();
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		CommFunction comF = new CommFunction();
		FilterSessionModels chk = new FilterSessionModels();


		// GET: Ese 기본 정보
		public ActionResult EseInfo(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseInfoModels model = new EseInfoModels();
			model.viewEseCode = (string)Session["ESE_CODE"];
			model = act.GetEseInfo(model);
			
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EseInfo(EseInfoModels model, string viewEseCode)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_UPDATE"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			// 중복체크를 사용을 위해 act_key, schTxt2
			model.act_key = 1;
			model.schTxt2 = "updt";
			if (model.Item.ESE_NAME == "")
			{
				model.act_key = 0;
				model.schTxt2 = "ins";
			}
				
			string chkDupl = act.ChkUpdtEseInfo(model);
			
			if (chkDupl != "")
			{
				return RedirectToAction("EseInfo", new { msg = chkDupl });
			}
			
			model.viewEseCode = (string)Session["ESE_CODE"];
			string PublicPopupMsg = act.SetUpdtEseInfo(model);
						
			return RedirectToAction("EseInfo", new { msg = PublicPopupMsg });
		}


		// GET:ESE 관리자 설정 ESE 계좌 관리
		public ActionResult EseAccount(string msg)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("EseAccount", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseAccountModels model = new EseAccountModels();
			model.viewEseCode = (string)Session["ESE_CODE"];
			model = act.GetEseAccountModel(model);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EseAccount(EseAccountModels model)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("EseAccount", "PER_SELECT"))
				return RedirectToAction("EseAccount", "Home", new { msg = chk.alertStr });
			//===========================================================

			if (!ModelState.IsValid)
				return View(model);

			string message = "성공";

			model.viewEseCode = (string)Session["ESE_CODE"];
			if (act.SetEseAccountModel(model))
			{
				return RedirectToAction("EseAccount", "Ese", new { msg = message });
			}

			message = "실패";
			ViewBag.result = message;

		
			return View(model);
		}



		// GET: EseInfoMy 내 계정 관리
		public ActionResult EseInfoMy(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfoMy", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseInfoMyModels model = new EseInfoMyModels();
			model.email = (string)Session["CURRENT_LOGIN_EMAIL"];
			
			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EseInfoMy(EseInfoMyModels model)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("EseInfoMy", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			if (!ModelState.IsValid)
				return View(model);

			string message = "새 비밀번호와 비밀번호 확인 값이 일치하지 않습니다.";

			if (model.passwd_new != model.passwd_check)
			{
				TempData["PublicMsg"] = message;
				return View(model);
			}
			
			message = act.changePassword(model);
			return RedirectToAction("EseInfoMy", "Ese", new { msg = message });

		}

		// GET: Ese 계정 등급 관리
		public ActionResult EseGrade(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseGrade", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;


			EseGradeModels model = new EseGradeModels();

			model.Item.ESE_CODE = (string)Session["ESE_CODE"];

			model = act.GetEseGradeList(model);
			

			return View(model);
		}

		[HttpPost]
		public ActionResult EseGrade(EseGradeModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseGrade", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			//삭제일 경우
			if (model.act_type == "del")
			{

				//권한 체크===================================================
				if (!chk.chkPermission("EseGrade", "PER_DELETE"))
					return RedirectToAction("EseGrade", new { msg = chk.alertStr });
				//===========================================================


				TempData["PublicMsg"] = act.DelEseGrade(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
				return RedirectToAction("EseGrade", model);
			}

			model = act.GetEseGradeList(model); //리스트 가져오기
			
			return View(model);
		}


		public ActionResult EseGradeView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseGrade", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================



			EseGradeViewModels model = new EseGradeViewModels();
			int pSeqNo = 0;
			if (int.TryParse(seqNo, out pSeqNo))
			{
				model.act_type = "updt";
				model.act_key = pSeqNo;
			}
			else
			{
				model.act_type = "ins";
				model.Item.ESE_CODE = (string)Session["ESE_CODE"];
				model.Item.GROUP_ID = 0;
			}

			if (!String.IsNullOrEmpty(Msg))
				ViewBag.PublicPopupMsg = Msg;

			model = act.GetEseGradeView(model);


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
		public ActionResult EseGradeView(EseGradeViewModels model)
		{
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseGrade", "PER_INSERT"))
					return RedirectToAction("EseGradeView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseGrade", "PER_UPDATE"))
					return RedirectToAction("EseGradeView", new { msg = chk.alertStr });
				//===========================================================
			}

			// 중복체크를 사용을 위해 act_key, act_type
			// 중복체크를 사용을 위해 act_key, schTxt2
			model.act_key = 1;
			model.schTxt2 = "updt";
			if (model.Item.GROUP_NAME == "")
			{
				model.act_key = 0;
				model.schTxt2 = "ins";
			}

			string chkDupl = act.ChkUpdtEseGrade(model);

			if (chkDupl != "")
			{

				return RedirectToAction("EseGradeView", new { msg = chkDupl });
			}

			//ese_group_permission 권한

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

			string PublicPopupMsg = act.SetEseGrade(model);
			
			return RedirectToAction("EseGradeView", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}




		// GET: EseManager 계정관리
		public ActionResult EseManager(string msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseManager", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseManagerModels model = new EseManagerModels();
			model = act.GetEseManagerList(model);

			return View(model);
		}

		[HttpPost]
		public ActionResult EseManager(EseManagerModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseManager", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			string resultMsg = null;

			//삭제일 경우
			if (model.act_type == "del")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseManager", "PER_DELETE"))
					return RedirectToAction("EseManager", new { msg = chk.alertStr });
				//===========================================================

				resultMsg = act.DelEseManager(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
				return RedirectToAction("EseManager", new { msg = resultMsg });
			}

			model = act.GetEseManagerList(model);
			

			return View(model);
		}

		public ActionResult EseManagerView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseManager", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			
			EseManagerModels model = new EseManagerModels();

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

			model.Item = act.GetEseManagerView(model);
			model.RadioBoxPop = false;
			if (model.Item.STATUS == 1)
				model.RadioBoxPop = true;

			//SELECT BOX ARRAY 데이터 설정 comF에 그룹 id 추가해야한다.
			model.GroupIdArray = act.GroupIdSelectBox();

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EseManagerView(EseManagerModels model)
		{
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseManager", "PER_INSERT"))
					return RedirectToAction("EseMangerView", new { msg = chk.alertStr });
				//===========================================================

				if (model.Item.PASSWD != model.Item.PASSWD_CHK)
				{
					ViewBag.TempMsg = "비밀번호가 일치 하지 않습니다.";
					return View(model);
				}
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseManager", "PER_UPDATE"))
					return RedirectToAction("EseMangerView", new { msg = chk.alertStr });
				//===========================================================
			}


			string chkDupl = act.ChkUpdtEseManager(model);

			if (chkDupl != "")
			{
				return RedirectToAction("EseManagerView", new { msg = chkDupl });
			}


			//라디오버튼 쓰기
			model.Item.STATUS = 1;
			if (model.RadioBoxPop == false)
				model.Item.STATUS = 0;

			model.Item.GROUP_ID = model.GroupId;
			

			//SELECT BOX ARRAY 데이터 설정
			model.GroupIdArray = comF.GroupIdSelectBox();


			string PublicPopupMsg = act.SetEseManager(model);
			return RedirectToAction("EseManagerView", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}

	}
}