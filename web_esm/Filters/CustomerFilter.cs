using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using web_esm.Models;

namespace web_esm.Filters
{
	public class CustomerFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			commonChk(filterContext);
		}


		//네비 자동 구성 
		private void commonChk(ActionExecutingContext filterContext)
		{
			FilterSessionModels chkModel = new FilterSessionModels();
			var viewBag = filterContext.Controller.ViewBag;

			if(filterContext.HttpContext.Session["MANAGE_NO"] != null)
				chkModel.MANAGE_NO = filterContext.HttpContext.Session["MANAGE_NO"].ToString();

			if (filterContext.HttpContext.Session["MANAGE_GRADE"] != null)
				chkModel.MANAGE_GRADE = filterContext.HttpContext.Session["MANAGE_GRADE"].ToString();

			if (filterContext.HttpContext.Session["CURRENT_LOGIN_EMAIL"] != null)
				chkModel.CURRENT_LOGIN_EMAIL = filterContext.HttpContext.Session["CURRENT_LOGIN_EMAIL"].ToString();
			
			chkModel.chkAction = filterContext.RouteData.Values["action"].ToString();
			chkModel.chkController = filterContext.RouteData.Values["controller"].ToString();

			//세션값이 비어있을 경우 로그인 페이지로 리다이랙트
			if (!chkModel.chkSession())
			{
				viewBag.PublicMsg = "로그인 후 이용해 주시기 바랍니다.";
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
			}
			

			var NaviMainString = "";
			var NaviSubString = "";

			switch (chkModel.chkController)
			{
				case "home":	NaviMainString = "메인";				break;
				case "base":	NaviMainString = "기본정보 관리";		break;
				case "est":		NaviMainString = "EST STATION";		break;
				case "ese":		NaviMainString = "ESE SENDER";		break;
				case "prod":	NaviMainString = "통관 상품 관리";	break;
				case "mar":		NaviMainString = "MAR 관리";			break;
				case "cs":		NaviMainString = "C / S";			break;
				case "setting": NaviMainString = "설정";				break;
				case "esm":		NaviMainString = "ESM 관리자 설정";	break;
				default:		NaviMainString = "메인";				break;
			}
			
			switch (chkModel.chkAction)
			{
				case "basecurrency":	NaviSubString = "통화 관리"; break;
				case "basenation":		NaviSubString = "배송 가능 국가 관리"; break;
				case "baseairport":		NaviSubString = "공항 관리"; break;
				case "baselocal":		NaviSubString = "현지 배송업체 설정"; break;
				case "baseoutputtype":	NaviSubString = "출고타입 설정"; break;
				case "estinfo":			NaviSubString = "EST 정보관리"; break;
				case "estgrade":		NaviSubString = "EST 계정 등급 관리"; break;
				case "estaccount":		NaviSubString = "EST 계정 관리"; break;
				case "estinoutstat":	NaviSubString = "EST 출고 현황"; break;
				case "eseinfo":			NaviSubString = "ESE 정보관리"; break;
				case "prodlist":		NaviSubString = "통관 상품 관리"; break;
				case "marinreq":		NaviSubString = "MAR 충전요청"; break;
				case "maroutest":		NaviSubString = "MAR 출금요청(EST)"; break;
				case "maroutese":		NaviSubString = "MAR 출금요청(ESE)"; break;
				case "marinout":		NaviSubString = "MAR 입출금 현황"; break;
				case "csnotice":		NaviSubString = "CsNotice"; break;
				case "csqna":			NaviSubString = "CsQna"; break;
				case "settingemail":	NaviSubString = "메일 서버 설정"; break;
				case "esmgrade":		NaviSubString = "ESM 계정 그룹 관리"; break;
				case "esmgradeview":	NaviSubString = "ESM 계정 관리"; break;
				case "esmaccount":		NaviSubString = "로그인 이력 조회"; break;
				default: NaviSubString = ""; break;
			}


			viewBag.NaviMainString = NaviMainString;
			viewBag.NaviSubString = NaviSubString;
			viewBag.Title = NaviSubString;
		}
	}
}