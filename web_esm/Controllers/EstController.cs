using comm_dbconn;
using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_esm.Models;
using web_esm.Models_Act.Comm;
using web_esm.Models_Act.Est;
using web_esm.Models_Db;

namespace web_esm.Controllers
{
    public class EstController : Controller 
	{

		EstDbModels act = new EstDbModels();      //DB커넥션 클래스 선언
		FilterSessionModels chk = new FilterSessionModels();
		CommonModel comM = new CommonModel();   //공통 함수 클래스 선언
		CommFunction comF = new CommFunction();

		// GET: Est EST STATION EST 정보 관리
		public ActionResult EstInfo(string searchString, string msg, string NATION_CODE, string estCode)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EstInfoModels model = new EstInfoModels();

			//페이징 설정 초기화
			model.Paging.page = 1;
			model.Paging.pageNum = 10;
			model.sortKey = "SEQNO";

			//리스트 가져오기
			model = act.GetEstInfoList(model);
			ViewData["pageing"] = comM.setPaging(model.Paging); //페이징 HTML 만들기

			//셀렉트박스
			model.nationArray =act.GetNationCodeSelectBox();
			model.Item.NATION_CODE = model.schType;
			NATION_CODE = model.Item.NATION_CODE;
			//estCode
			//model.viewEstCode = estCode;
			//model.Item.EST_CODE = model.viewEstCode;
			model.stationArray = act.GetEstCodeSelectBox(estCode);

			return View(model);
        }

		[HttpPost]
		public ActionResult EstInfo(EstInfoModels model)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			

			if (TempData["PublicMsg"] != null)
				ViewBag.PublicMsg = TempData["PublicMsg"].ToString();

			ViewData["pageing"] = comM.setPaging(model.Paging);    //페이징 HTML 만들기

			//셀렉트박스
			model.Item.NATION_CODE = model.schType;
			model.Item.EST_CODE = model.station;
			model.nationArray = act.GetNationCodeSelectBox();

			model = act.GetEstInfoList(model); //리스트 가져오기

			return View(model);
		}


		[HttpPost]
		public JsonResult GetEstSelectbox(string estCode)
		{
			return Json(act.GetEstCodeSelectBox( estCode));
		}

		// GET: Est EST STATION EST 정보 관리	뷰 팝업 
		public ActionResult EstInfoView(string msg, string estCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EstInfoModels model = new EstInfoModels();
			model.viewEstCode = estCode;

			return View(model);
		}


		//EST 기본정보 ====================================================================================================
		// GET: Est EST STATION Est 정보 관리 -> EST 기본정보
		public ActionResult EstIframeInfo(string msg, string estCode, string NATION_CODE)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EstIframeInfoModels model = new EstIframeInfoModels();
			model.viewEstCode = estCode;

			model.Item = act.GetEstIframeInfo(model,estCode);

			//Select Box 설정
			model.nationArray = comF.GetCommNationSelectBox();
			model.weightArray = comF.GetWeightSelectBox();
			model.stationArray = act.GetEstCodeSelectBox(estCode); 

			//라디오버튼 쓰기
			if (model.RadioBoxPop == true)
			{
				model.Item.STATUS = 1;
			}
			else
			{
				model.Item.STATUS = 0;
			}

			model.RadioBoxPop = false;
			if (model.Item.STATUS == 1)
				model.RadioBoxPop = true;

			//라디오버튼 쓰기2
			if (model.RadioBoxSummerT == true)
			{
				model.Item.STATUS = 1;
			}
			else
			{
				model.Item.STATUS = 0;
			}

			model.RadioBoxSummerT = false;
			if (model.Item.UTC_SUMMER_TIME == 1)
				model.RadioBoxSummerT = true;


			return View(model);
		}

		[HttpPost]
		public ActionResult EstIframeInfo(EstIframeInfoModels model, string viewEstCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			if (!ModelState.IsValid)
				return View(model);

			//Select Box로 설정한 값을 item에 넣기.
			//model.Item.EST_CODE = model.station;
			model.Item.NATION_CODE = model.schType;
			model.Item.WEIGHT_UNIT = model.WEIGHT_UNIT;
			

			
			//라디오버튼 쓰기
			if (model.RadioBoxPop == true)
			{
				model.Item.STATUS = 1;
			}
			else
			{
				model.Item.STATUS = 0;
			}

			model.RadioBoxPop = false;
			if (model.Item.STATUS == 1)
				model.RadioBoxPop = true;

			//라디오버튼 쓰기2
			if (model.RadioBoxSummerT == true)
			{
				model.Item.STATUS = 1;
			}
			else
			{
				model.Item.STATUS = 0;
			}

			model.RadioBoxSummerT = false;
			if (model.Item.UTC_SUMMER_TIME == 1)
				model.RadioBoxSummerT = true;

			if (TempData["PublicMsg"] != null)
				ViewBag.PublicMsg = TempData["PublicMsg"].ToString();

			ViewBag.PublicPopupMsg = act.SetEstIframeInfo(model);

			return View(model);
		}



		//계정 관리 =========================================================================================================

		// GET: Est EST STATION Est 정보 관리 -> 계좌정보
		public ActionResult EstIframeAccount(string msg, string estCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("EstIframeAccount", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;
			
			EstAccountModels model = act.GetEstAccountModel(estCode);
			model.viewEstCode = estCode;


			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EstIframeAccount(EstAccountModels viewModel)
		{

			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("EstIframeAccount", "Home", new { msg = chk.alertStr });
			//===========================================================

			if (!ModelState.IsValid)
				return View(viewModel);


			string message = "성공";

			if (act.SetEstAccountModel(viewModel))  
			{
				return RedirectToAction("EstIframeAccount", "Est",  new { estCode = viewModel.viewEstCode , msg = message });
			}

			message = "실패";
			ViewBag.result = message;
			


			return View(viewModel);
		}


		//통관 비용 =========================================================================================================

		// GET: Est EST STATION Est 정보 관리 -> 통관비용
		public ActionResult EstIframeCost(string msg, string estCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			return View();
		}


		//국가별 요율표 =========================================================================================================




		// GET: Est EST STATION Est 정보 관리 -> 국가별 요율표
		public ActionResult EstIframeCostTable(string msg, string estCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EstIframeCostModels model = new EstIframeCostModels();
			model.viewEstCode = estCode;
			//model.Item.EST_CODE = model.viewEstCode;

			model = act.GetEstIframeCostList(model, estCode);
			//SELECT BOX ARRAY 데이터 설정
			model.stationArray = act.GetEstCodeSelectBox(estCode);
			model.nationArray = act.GetNationCodeSelectBox();			
			model.typeArray = act.GetReleaseCodeSelectBox(model);

			return View(model);
		}

		[HttpPost]
		public ActionResult EstIframeCostTable(EstIframeCostModels model, string estCode, string NATION_CODE)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================
			
			model = act.GetEstIframeCostList(model, estCode);

			model.nationArray = act.GetNationCodeSelectBox();
			model.stationArray = act.GetEstCodeSelectBox(estCode);
			model.typeArray = act.GetReleaseCodeSelectBox(model);

			return View(model);
		}
		/*
		[HttpPost]
		public JsonResult GetRate(EstIframeCostModels getModel, string estCode)
		{
			EstShippingFee model = new EstShippingFee();
			model= act.GetEstIframeCostList(getModel, estCode); //리스트 가져오기

			getModel.typeArray = act.GetReleaseCodeSelectBox(getModel);
			return Json(getModel.typeArray);
		}
		*/

		[HttpPost]
		public JsonResult GetRate(EstIframeCostModels model, string estCode)
		{
			
			model = act.GetEstIframeCostList(model, estCode); //리스트 가져오기

			model.typeArray = act.GetReleaseCodeSelectBox(model);
			return Json(model.typeArray);
		}



		[HttpPost]
		public JsonResult GetEstIframeCostSelectbox(EstIframeCostModels model, string estCode)
		{
			model = act.GetEstIframeCostList(model, estCode); //리스트 가져오기

			model.typeArray = act.GetReleaseCodeSelectBox(model);
			return Json(model.typeArray);
		}


		//계정 등급 관리 ====================================================================================================

		// GET: Est EST STATION Est 정보 관리 -> 계정 등급 관리
		public ActionResult EstIframeGrade(string msg, string estCode)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EstGradeModels model = new EstGradeModels();
			model.viewEstCode = estCode;
			model = act.GetEstGradeList(model,estCode);
			model.Item.EST_CODE = model.viewEstCode;
			return View(model);
        }

		[HttpPost]
		public ActionResult EstIframeGrade(EstGradeModels model, string viewEstCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			//삭제일 경우
			if (model.act_type == "del")
			{

				//권한 체크===================================================
				if (!chk.chkPermission("EstInfo", "PER_DELETE"))
					return RedirectToAction("EstIframeGrade", new { msg = chk.alertStr });
				//===========================================================


				TempData["PublicMsg"] = act.DelEstGrade(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
				model = act.GetEstGradeList(model, viewEstCode); //리스트 가져오기
				//model.Items. = model.viewEstCode; Items 에 넣어야 하는데.
				if (TempData["PublicMsg"] != null)
					ViewBag.PublicMsg = TempData["PublicMsg"].ToString();

				return RedirectToAction("EstIframeGrade", model);
			}
			

			model = act.GetEstGradeList(model, viewEstCode); //리스트 가져오기

			if (TempData["PublicMsg"] != null)
				ViewBag.PublicMsg = TempData["PublicMsg"].ToString();

			
			return View(model);
		}

		public ActionResult EstIframeGradeView(string Msg, string groupId, string estCode)    
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			EstGradeViewModel model = new EstGradeViewModel();

			model = act.GetEstGradeView(groupId, estCode);	
			model.viewEstCode = estCode;
			model.Item.EST_CODE = model.viewEstCode;

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
		public ActionResult EstIframeGradeView(EstGradeViewModel model, string viewEstCode)
		{ 
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EstInfo", "PER_INSERT"))
					return RedirectToAction("EstIframeGradeView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EstInfo", "PER_UPDATE"))
					return RedirectToAction("EstIframeGradeView", new { msg = chk.alertStr });
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

			//그룹명 유효성 검사
			if (!ModelState.IsValid)
				return View(model);

			string PublicPopupMsg = act.SetEstGrade(model, viewEstCode);

			return RedirectToAction("EstIframeGrade", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}

		//계정 관리 ====================================================================================================

		// GET: Est EST STATION Est 정보 관리-> 계정 관리
		public ActionResult EstIframeUser(string msg, string estCode)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			EstUserModels model = new EstUserModels();
			//model.Item.EST_CODE = estCode;
			model.viewEstCode = estCode;

			model = act.GetEstUserList(model, estCode);


			return View(model);
        }

		[HttpPost]
		public ActionResult EstIframeUser(EstUserModels model, string estCode)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================


			//삭제일 경우
			if (model.act_type == "del")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EstInfo", "PER_DELETE"))
					return RedirectToAction("EstIframeUser", new { msg = chk.alertStr });
				//===========================================================

				TempData["PublicMsg"] = act.DelEstUser(model.act_key); //삭제
				model.act_type = "list";
				model.act_key = 0;
				return RedirectToAction("EstIframeUser", model);
			}

			model = act.GetEstUserList(model, estCode);

			if (TempData["PublicMsg"] != null)
				ViewBag.PublicMsg = TempData["PublicMsg"].ToString();

			model.viewEstCode = estCode;

			return View(model);
		}


		// GET: Est EST STATION Est 정보 관리 -> 계정 관리 상세
		public ActionResult EstIframeUserView(string seqNo, string Msg)
		{
			//권한 체크===================================================
			if (!chk.chkPermission("EstInfo", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			EstUserModels model = new EstUserModels();
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

			model.Item = act.GetEstUserView(model);
			model.RadioBoxPop = false;
			if (model.Item.STATUS == 1)
				model.RadioBoxPop = true;

			//SELECT BOX ARRAY 데이터 설정 comF에 그룹 id 추가해야한다.
			model.GroupIdArray = comF.GroupIdSelectBox();

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EstIframeUserView(EstUserModels model)
		{
			if (model.act_type == "ins")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EstInfo", "PER_INSERT"))
					return RedirectToAction("EstIframeUserView", new { msg = chk.alertStr });
				//===========================================================
			}

			if (model.act_type == "updt")
			{
				//권한 체크===================================================
				if (!chk.chkPermission("EstInfo", "PER_UPDATE"))
					return RedirectToAction("EstIframeUserView", new { msg = chk.alertStr });
				//===========================================================
			}

			

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


			string PublicPopupMsg = act.SetEstUser(model);
			return RedirectToAction("EstIframeUser", new { seqNo = model.act_key, Msg = PublicPopupMsg });
		}

		/*	보류
        // GET: Est EST STATION EST 출고 현황
        public ActionResult EstInOutStat(string msg)
        {
			//권한 체크===================================================
			if (!chk.chkPermission("EstInOutStat", "PER_SELECT"))
				return RedirectToAction("Index", "Home", new { msg = chk.alertStr });
			//===========================================================

			TempData["PublicMsg"] = null;
			if (!string.IsNullOrEmpty(msg))
				TempData["PublicMsg"] = msg;

			return View();
        }
		*/
	}
}