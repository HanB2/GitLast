using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using web_ese.Models;

namespace web_ese.Filters
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

			if (filterContext.HttpContext.Session["ESE_CODE"] != null)
			{
				chkModel.ESE_CODE = filterContext.HttpContext.Session["ESE_CODE"].ToString();
				CommFunction cf = new CommFunction();
				viewBag.MyMar = cf.GetMyMAR();
			}

			if (filterContext.HttpContext.Session["MANAGE_NO"] != null)
				chkModel.MANAGE_NO = filterContext.HttpContext.Session["MANAGE_NO"].ToString();

			if (filterContext.HttpContext.Session["MANAGE_GRADE"] != null)
				chkModel.MANAGE_GRADE = filterContext.HttpContext.Session["MANAGE_GRADE"].ToString();

			if (filterContext.HttpContext.Session["CURRENT_LOGIN_EMAIL"] != null)
				chkModel.CURRENT_LOGIN_EMAIL = filterContext.HttpContext.Session["CURRENT_LOGIN_EMAIL"].ToString();

			chkModel.chkAction = filterContext.RouteData.Values["action"].ToString();
			chkModel.chkController = filterContext.RouteData.Values["controller"].ToString();

			string stringURL = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);

			//로그인 유지 체크 확인

			if (filterContext.HttpContext.Request.Cookies["CHK_LOGIN_REMEMBER"] != null)
			{
				filterContext.HttpContext.Session["MANAGE_NO"] = filterContext.HttpContext.Request.Cookies["MANAGE_NO"].Value.ToString();
				filterContext.HttpContext.Session["MANAGE_GRADE"] = filterContext.HttpContext.Request.Cookies["MANAGE_GRADE"].Value.ToString();
				filterContext.HttpContext.Session["CURRENT_LOGIN_EMAIL"] = filterContext.HttpContext.Request.Cookies["CURRENT_LOGIN_EMAIL"].Value.ToString();
				filterContext.HttpContext.Session["EST_CODE"] = filterContext.HttpContext.Request.Cookies["EST_CODE"].Value.ToString();
				filterContext.HttpContext.Session["ESE_CODE"] = filterContext.HttpContext.Request.Cookies["ESE_CODE"].Value.ToString();
				filterContext.HttpContext.Session["STATUS"] = filterContext.HttpContext.Request.Cookies["STATUS"].Value.ToString();
			}


			//세션값이 비어있을 경우 로그인 페이지로 리다이랙트
			if (!chkModel.chkSession())
			{
				viewBag.PublicMsg = "로그인 후 이용해 주시기 바랍니다.";
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = stringURL }));
			}


			var NaviMainString = "";
			var NaviSubString = "";

			switch (chkModel.chkController)
			{
				case "home": NaviMainString = "메인"; break;
				case "prod": NaviMainString = "상품관리"; break;
				case "stoc": NaviMainString = "보관관리"; break;
				case "pick": NaviMainString = "픽업 관리"; break;
				case "ets": NaviMainString = "배송"; break;
				case "cost": NaviMainString = "배송요금"; break;
				case "ese": NaviMainString = "계정관리"; break;
				case "mar": NaviMainString = "MAR"; break;
				case "cs": NaviMainString = "고객센터"; break;
				default: NaviMainString = "메인"; break;
			}



			switch (chkModel.chkAction)
			{
				case "prodadd":		NaviSubString = "상품 등록"; break;
				case "prodlist":	NaviSubString = "등록 상품 조회"; break;
				case "stocreq":		NaviSubString = "보관 신청"; break;
				case "stocreqlist":	NaviSubString = "보관 신청 현황 조회"; break;
				case "stoclist":	NaviSubString = "재고조회"; break;
				case "stocinout":	NaviSubString = "입출고 내역 조회"; break;
				case "pickreq":		NaviSubString = "픽업 신청"; break;
				case "picklist":	NaviSubString = "픽업 신청 조회"; break;
				case "etsreq":		NaviSubString = "일반 배송 신청"; break;
				case "estreqexcel":	NaviSubString = "대량 배송 신청"; break;
				case "estlabel":	NaviSubString = "배송 라벨 출력"; break;
				case "estlsit":		NaviSubString = "배송 상태 조회"; break;
				case "index":		NaviSubString = "배송요금"; break;
				case "eseinfo":		NaviSubString = "계정 정보"; break;
				case "esegrade":	NaviSubString = "계정 등급 관리"; break;
				case "eseaccount":	NaviSubString = "계정 관리"; break;
				case "marreqpg":	NaviSubString = "MAR 충전(PG)"; break;
				case "marreq":		NaviSubString = "MAR충전(이체)"; break;
				case "marinout":	NaviSubString = "MAR 충전/사용 이력"; break;
				case "maroutreq":	NaviSubString = "MAR 환불 신청"; break;
				case "csesmnotice":	NaviSubString = "ETOMARS 공지"; break;
				case "csestnotice":	NaviSubString = "STATION 공지"; break;
				case "csqna":		NaviSubString = "1 : 1 문의"; break;

				default: NaviSubString = ""; break;
			}


			viewBag.NaviMainString = NaviMainString;
			viewBag.NaviSubString = NaviSubString;
			viewBag.Title = NaviSubString + " - ETOMARS";



		}
	}
}