
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace web_ese_old.Filter
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



		//네비 자동 구성 
		private void SetNaviString(ActionExecutingContext filterContext)
		{
			var controllerName = filterContext.RouteData.Values["controller"].ToString();
			var actionName = filterContext.RouteData.Values["action"].ToString();

			var NaviMainString = "";
			var NaviSubString = "";
			
			switch (controllerName)
			{
				case "Home":	NaviMainString = "메인";				break;
				case "StcProd":	NaviMainString = "보관상품";			break;
				case "StcReq":	NaviMainString = "보관신청";			break;
				case "PicReq":	NaviMainString = "픽업신청";			break;
				case "EtsReq":	NaviMainString = "배송";				break;
				case "EtsSch":	NaviMainString = "배송";				break;
				case "EtsCost":	NaviMainString = "배송 요율표 조회";	break;
				case "EseInfo":	NaviMainString = "회원정보변경";		break;
				case "Mar":		NaviMainString = "MAR";				break;
				case "Notice":	NaviMainString = "공지사항";			break;
				default:		NaviMainString = "메인";				break;
			}
			
			switch (actionName)
			{
				case "index":			NaviSubString = "메인";					break;
				case "StcProdAdd":		NaviSubString = "상품추가";				break;
				case "StcProdInfo":		NaviSubString = "상품정보 팝업";			break;
				case "StcProdList":		NaviSubString = "상품조회";				break;
				case "StcReqAdd":		NaviSubString = "보관신청";				break;
				case "StcReqSelect":	NaviSubString = "보관상품선택 팝업";		break;
				case "StcReqInfo":		NaviSubString = "보관신청정보 팝업";		break;
				case "StcReqList":		NaviSubString = "보관신쳥현황 조회";		break;
				case "StcReqStcList":	NaviSubString = "보관상품 조회";			break;
				case "PicReqAdd":		NaviSubString = "픽업신청";				break;
				case "PicReqInfo":		NaviSubString = "픽업정보 팝업";			break;
				case "PicReqList":		NaviSubString = "픽업조회";				break;
				case "EtsReqStcadd":	NaviSubString = "보관상품 배송신청";		break;
				case "EtsReqChnadd":	NaviSubString = "중국 배송신청";			break;
				case "EtsReqEtcadd":	NaviSubString = "일반 배송신청";			break;
				case "EtsReqStcinfo":	NaviSubString = "보관상품 배송신청 팝업"; break;
				case "EtsReqChninfo":	NaviSubString = "중국 배송신청 팝업";		break;
				case "EtsReqEtcinfo":	NaviSubString = "일반 배송신청 팝업";		break;
				case "EtsSchLabel":		NaviSubString = "배송라벨출력";			break;
				case "EtsSchStat":		NaviSubString = "배송상태조회";			break;
				case "EtsSchMemo":		NaviSubString = "메모 팝업";				break;
				case "EtsCost":			NaviSubString = "배송 요율표 조회";		break;
				case "EseInfo":			NaviSubString = "ESE 정보";				break;
				case "EseInfoAccount":	NaviSubString = "ESE 계좌정보";			break;
				case "EseInfoMy":		NaviSubString = "ESE 계정정보";			break;
				case "EseInfoManage":	NaviSubString = "ESE 계정관리";			break;
				case "MarChargepg":		NaviSubString = "MAR 충전(온라인 결제)";	break;
				case "MarChargereq":	NaviSubString = "MAR 충전(계좌이체)";		break;
				case "MarRefundreq":	NaviSubString = "MAR 환불 신청";			break;
				case "MarCharge":		NaviSubString = "MAR 충전 이력";			break;
				case "MarUse":			NaviSubString = "MAR 사용 이력";			break;
				case "MarRefund":		NaviSubString = "MAR 환불 이력";			break;
				case "NoticeEtm":		NaviSubString = "ETOMARS 공지사항";		break;
				case "NoticeEts":		NaviSubString = "EST 공지사항";			break;
				case "NoticeQna":		NaviSubString = "1:1 문의";				break;
				case "NoticeFaq":		NaviSubString = "자주묻는 질문";			break;
				case "NoticeFile":		NaviSubString = "자료실";				break;
				default:				NaviSubString = "";						break;
			}

			var viewBag = filterContext.Controller.ViewBag;

			viewBag.NaviMainString = NaviMainString;
			viewBag.NaviSubString = NaviSubString;
			viewBag.Title = NaviSubString;
		}







		//네비 자동 구성 
		private void SetNaviString_new(ActionExecutingContext filterContext)
		{
			var controllerName = filterContext.RouteData.Values["controller"].ToString();
			var actionName = filterContext.RouteData.Values["action"].ToString();

			var NaviMainString = "";
			var NaviSubString = "";

			switch (controllerName)
			{
				case "	Home	": NaviMainString = "	메인	"; break;
				case "	Base	": NaviMainString = "	기본정보 관리	"; break;
				case "	Est	": NaviMainString = "	EST STATION	"; break;
				case "	Ese	": NaviMainString = "	ESE SENDER	"; break;
				case "	Prod	": NaviMainString = "	통관 상품 관리	"; break;
				case "	Mar	": NaviMainString = "	MAR 관리	"; break;
				case "	Cs	": NaviMainString = "	C / S	"; break;
				case "	Setting	": NaviMainString = "	설정	"; break;
				case "	Esm	": NaviMainString = "	ESM 관리자 설정	"; break;
				default: NaviMainString = "메인"; break;
			}

			switch (actionName)
			{
				case "index": NaviSubString = "메인"; break;
				case "StcProdAdd": NaviSubString = "상품추가"; break;
				case "StcProdInfo": NaviSubString = "상품정보 팝업"; break;
				case "StcProdList": NaviSubString = "상품조회"; break;
				case "StcReqAdd": NaviSubString = "보관신청"; break;
				case "StcReqSelect": NaviSubString = "보관상품선택 팝업"; break;
				case "StcReqInfo": NaviSubString = "보관신청정보 팝업"; break;
				case "StcReqList": NaviSubString = "보관신쳥현황 조회"; break;
				case "StcReqStcList": NaviSubString = "보관상품 조회"; break;
				case "PicReqAdd": NaviSubString = "픽업신청"; break;
				case "PicReqInfo": NaviSubString = "픽업정보 팝업"; break;
				case "PicReqList": NaviSubString = "픽업조회"; break;
				case "EtsReqStcadd": NaviSubString = "보관상품 배송신청"; break;
				case "EtsReqChnadd": NaviSubString = "중국 배송신청"; break;
				case "EtsReqEtcadd": NaviSubString = "일반 배송신청"; break;
				case "EtsReqStcinfo": NaviSubString = "보관상품 배송신청 팝업"; break;
				case "EtsReqChninfo": NaviSubString = "중국 배송신청 팝업"; break;
				case "EtsReqEtcinfo": NaviSubString = "일반 배송신청 팝업"; break;
				case "EtsSchLabel": NaviSubString = "배송라벨출력"; break;
				case "EtsSchStat": NaviSubString = "배송상태조회"; break;
				case "EtsSchMemo": NaviSubString = "메모 팝업"; break;
				case "EtsCost": NaviSubString = "배송 요율표 조회"; break;
				case "EseInfo": NaviSubString = "ESE 정보"; break;
				case "EseInfoAccount": NaviSubString = "ESE 계좌정보"; break;
				case "EseInfoMy": NaviSubString = "ESE 계정정보"; break;
				case "EseInfoManage": NaviSubString = "ESE 계정관리"; break;
				case "MarChargepg": NaviSubString = "MAR 충전(온라인 결제)"; break;
				case "MarChargereq": NaviSubString = "MAR 충전(계좌이체)"; break;
				case "MarRefundreq": NaviSubString = "MAR 환불 신청"; break;
				case "MarCharge": NaviSubString = "MAR 충전 이력"; break;
				case "MarUse": NaviSubString = "MAR 사용 이력"; break;
				case "MarRefund": NaviSubString = "MAR 환불 이력"; break;
				case "NoticeEtm": NaviSubString = "ETOMARS 공지사항"; break;
				case "NoticeEts": NaviSubString = "EST 공지사항"; break;
				case "NoticeQna": NaviSubString = "1:1 문의"; break;
				case "NoticeFaq": NaviSubString = "자주묻는 질문"; break;
				case "NoticeFile": NaviSubString = "자료실"; break;
				default: NaviSubString = ""; break;
			}

			var viewBag = filterContext.Controller.ViewBag;

			viewBag.NaviMainString = NaviMainString;
			viewBag.NaviSubString = NaviSubString;
			viewBag.Title = NaviSubString;
		}


	}
}







