using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_esm.Models;
using web_esm.Models_Act.Comm;
using web_esm.Models_Act.Ese;
using web_esm.Models_Db;

namespace web_esm.Controllers
{
    public class EseController : Controller
    {
		EseDbModels act = new EseDbModels();      //DB커넥션 클래스 선언
		FilterSessionModels chk = new FilterSessionModels();
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		CommFunction comF = new CommFunction();

		// GET: Ese SENDER ESE 정보 관리
		public ActionResult EseInfo(string searchString, string msg, string NATION_CODE)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseInfoModels model = new EseInfoModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			//리스트 가져오기
			model = act.GetEseInfoList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			//셀렉트박스
			model.nationArray = act.GetNationCodeSelectBox();
			model.Item.EST_CODE = model.station;
			model.Item.ESE_CODE = model.sender;
			model.stationArray = act.GetNationCodeSelectBox();
			model.senderArray = act.GetSenderSelectBox(NATION_CODE);


			return View(model);
        }

		[HttpPost]
		public ActionResult EseInfo(EseInfoModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			model = act.GetEseInfoList(model); //리스트 가져오기

			if (TempData["PublicMsg"] != null)
				ViewBag.PublicMsg = TempData["PublicMsg"].ToString();

			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			//셀렉트박스
			model.nationArray = act.GetNationCodeSelectBox();
			model.stationArray = act.GetNationCodeSelectBox();
			//model.senderArray = act.GetSenderSelectBox();

			return View(model);
		}


		// GET: Est EST STATION EST 정보 관리	뷰 팝업 
		public ActionResult EseInfoView(string msg, string eseCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseInfoModels model = new EseInfoModels();
			model.viewEseCode = eseCode;

			return View(model);
		}



		//ESE 기본정보 ====================================================================================================

		// GET: Ese ESE SENDER Ese 정보 관리 -> ESE 기본정보
		public ActionResult EseIframeInfo(string msg, string eseCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseIframeInfoModels model = new EseIframeInfoModels();
			model.viewEseCode = eseCode;

			model.Item = act.GetEstIframeInfo(model, eseCode);


			//라디오버튼 쓰기
			if (model.RadioBoxPop == "0")
			{
				model.Item.STATUS = 0;
			}
			else if (model.RadioBoxPop == "1")
			{
				model.Item.STATUS = 1;
			}
			else
			{
				model.Item.STATUS = 2;
			}

			model.RadioBoxPop = "2";
			if (model.Item.STATUS == 1)
			{
				model.RadioBoxPop = "1";
			}
			else if (model.Item.STATUS == 0)
			{
				model.RadioBoxPop = "0";
			}


			return View(model);
		}

		//계좌 관리 =========================================================================================================
		// GET: Ese ESE STATION Ese 정보 관리 -> 계좌정보
		public ActionResult EseIframeAccount(string msg, string eseCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("EseIframeAccount", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseAccountModels model = act.GetEseAccountModel(eseCode);
			model.viewEseCode = eseCode;


			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EseIframeAccount(EseAccountModels viewModel)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("EseIframeAccount", "Home", new { msg = chk.alertStr });
			//===========================================================

			if (!ModelState.IsValid)
				return View(viewModel);


			string message = "성공";

			if (act.SetEseAccountModel(viewModel))
			{
				return RedirectToAction("EseIframeAccount", "Ese", new { eseCode = viewModel.viewEseCode, msg = message });
			}

			message = "실패";
			ViewBag.result = message;



			return View(viewModel);
		}

		//계정 등급 관리 ====================================================================================================
		// GET: Ese ESE STATION Ese 정보 관리 -> 계정 등급 관리
		public ActionResult EseIframeGrade(string msg, string eseCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseGradeModels model = new EseGradeModels();
			model.viewEseCode = eseCode;
			model = act.GetEseGradeList(model, eseCode);

			return View(model);
		}

		[HttpPost]
		public ActionResult EseIframeGrade(EseGradeModels model, string viewEseCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			//삭제일 경우
			if (model.act_type == "del")
			{

				//권한 체크===================================================
				if (!chk.chkPermission("EseInfo", "PER_DELETE"))
					return RedirectToAction("EseIframeGrade", new { msg = chk.alertStr });
				//===========================================================


				TempData["PublicMsg"] = act.DelEseGrade(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
				return RedirectToAction("EseIframeGrade", model);
			}


			model = act.GetEseGradeList(model, viewEseCode); //리스트 가져오기

			if (TempData["PublicMsg"] != null)
				ViewBag.PublicMsg = TempData["PublicMsg"].ToString();


			return View(model);
		}

		public ActionResult EseIframeGradeView(string Msg, string groupId, string ese_code)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			EseGradeViewModels model = new EseGradeViewModels();

			model = act.GetEseGradeView(groupId, ese_code);
			int pSeqNo = 0;
			if (int.TryParse(groupId, out pSeqNo))
			{
				model.act_type = "updt";

			}
			else
			{
				model.act_type = "ins";
			}

			if (!String.IsNullOrEmpty(Msg))
				ViewBag.PublicPopupMsg = Msg;



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
		public ActionResult EseIframeGradeView(EseGradeViewModels model, string GROUP_ID)
		{
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseInfo", "PER_INSERT"))
					return RedirectToAction("EseIframeGradeView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseInfo", "PER_UPDATE"))
					return RedirectToAction("EseIframeGradeView", new { msg = chk.alertStr });
				//===========================================================
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


			string PublicPopupMsg = act.SetEseGrade(model, GROUP_ID);

			return RedirectToAction("EseIframeGradeView", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}

		//계정 관리 ========================================================================================================
		public ActionResult EseIframeUser(string msg, string eseCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EseUserModels model = new EseUserModels();
			
			model.viewEseCode = eseCode;

			model = act.GetEseUserList(model, eseCode);


			return View(model);
		}

		[HttpPost]
		public ActionResult EseIframeUser(EseUserModels model, string eseCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			//삭제일 경우
			if (model.act_type == "del")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseInfo", "PER_DELETE"))
					return RedirectToAction("EseIframeUser", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.DelEseUser(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
				return RedirectToAction("EseIframeUser", model);
			}

			model = act.GetEseUserList(model, eseCode);

			if (TempData["PublicMsg"] != null)
				ViewBag.PublicMsg = TempData["PublicMsg"].ToString();

			model.viewEseCode = eseCode;

			return View(model);
		}

		// GET: Est EST STATION Est 정보 관리 -> 계정 관리 상세
		public ActionResult EseIframeUserView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EseInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			EseUserModels model = new EseUserModels();
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

			model.Item = act.GetEseUserView(model);
			model.RadioBoxPop = false;
			if (model.Item.STATUS == 1)
				model.RadioBoxPop = true;

			//SELECT BOX ARRAY 데이터 설정 comF에 그룹 id 추가해야한다.
			model.GroupIdArray = comF.GroupIdSelectBox();

			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EseIframeUserView(EseUserModels model)
		{
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseInfo", "PER_INSERT"))
					return RedirectToAction("EseIframeUserView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EseInfo", "PER_UPDATE"))
					return RedirectToAction("EseIframeUserView", new { msg = chk.alertStr });
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


			string PublicPopupMsg = act.SetEseUser(model);
			return RedirectToAction("EseIframeUser", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}
	}
}