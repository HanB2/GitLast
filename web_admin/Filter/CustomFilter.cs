
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

using web_admin.Models;
namespace web_admin.Filter
{
	public class CustomFilter : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			SetNaviString(filterContext);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
		}


		//DEBUG 용 
		private void Log(string methodName, RouteData routeData)
		{
			var controllerName = routeData.Values["controller"];
			var actionName = routeData.Values["action"];
			var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
			Debug.WriteLine(message, "Action Filter Log");
		}



		//네비 자동 구성 및 로그인 / 권한 체크
		private void SetNaviString(ActionExecutingContext filterContext)
		{
			CommonController common = new CommonController();

			var viewBag = filterContext.Controller.ViewBag;     //viewBag 생성

			var controllerName = filterContext.RouteData.Values["controller"].ToString();	//컨트롤러명 가져오기
			var actionName = filterContext.RouteData.Values["action"].ToString();           //액션명 가져오기

			string userId = null;
			if (filterContext.HttpContext.Session != null)
			{
				//userId = filterContext.HttpContext.Session["userId"].ToString();
			}


			userId = "test";

			//로그인 체크
			if (userId == null)
			{
				viewBag.Title = "잘못된 접근입니다.";
				//session ID 없을 경우 로그인 페이지 이동

				//filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Home", controller = "메인" }));
			}

			
			//권한 체크 - 엑션명과 조작 타입을 이용하여 권한을 가져온다
			if (!common.MenuAuthCheck(actionName, "list", userId))
			{
				viewBag.Title = "잘못된 접근입니다.";
				//권한 없을 경우 메인 페이지 이동
				//filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "index", controller = "Home" }));
			}

			
			var NaviMainString = "";	// 네비 메뉴 명
			var NaviSubString = "";     // 네비 화면 명

			switch (controllerName)
			{
				case "Home":	NaviMainString = "메인";				break;
				case "Base":	NaviMainString = "기본정보";			break;
				case "Est":		NaviMainString = "EST STATION";		break;
				case "Ese":		NaviMainString = "ESE SENDER";		break;
				case "Prod":	NaviMainString = "중국배송상품";		break;
				case "Mar":		NaviMainString = "MAR 충전/환전";	break;
				case "Setting":	NaviMainString = "게시판";			break;
				default:		NaviMainString = "설정";				break;
			}
			
			switch (actionName)
			{
				case "Currency":	NaviSubString = "통화 관리";					break;
				case "Nation":		NaviSubString = "배송가능국가 관리";			break;
				case "Airport":		NaviSubString = "공항 관리";					break;
				case "EtsNo":		NaviSubString = "송장번호설정";				break;
				case "Local":		NaviSubString = "현지배송업체 설정";			break;
				case "Manage":		NaviSubString = "스테이션 관리";				break;
				case "Account":		NaviSubString = "스테이션 유저 관리";			break;
				case "InOut":		NaviSubString = "스테이션 출고 현황";			break;
				case "InOutStat":	NaviSubString = "수익률 분석(보류)";			break;
				case "Ese":			NaviSubString = "SENDER상품 관리";			break;
				case "Chn":			NaviSubString = "중국혜관등록관리";			break;
				case "InReq":		NaviSubString = "마르충전요청 관리";			break;
				case "OutEst":		NaviSubString = "마르출금요청 관리(STATION)"; break;
				case "OutEse":		NaviSubString = "마르출금요청 관리(SENDER)";	break;
				case "MarInOut":	NaviSubString = "마르입출고 현황";			break;
				case "Notice":		NaviSubString = "공지 사항 관리";				break;
				case "Qna":			NaviSubString = "문의 사항 관리";				break;
				case "Mail":		NaviSubString = "메일서버설정";				break;
				case "SetAccount":	NaviSubString = "매니저 계정 관리";			break;
				case "Log":			NaviSubString = "작업 이력 관리";				break;
				default:			NaviSubString = "";							break;
			}


			viewBag.NaviMainString = NaviMainString;
			viewBag.NaviSubString = NaviSubString;
			viewBag.Title = NaviSubString;
		}


	}
}






