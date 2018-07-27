using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using unirest_net.http;

namespace web_ese_old.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
			//설치 필요	Install-Package Unirest-API -Version 1.0.7.6

			
			HttpResponse<string> test3 = sendnumber_save();
			

			return View();
        }




		//메시지 전송
		public HttpResponse<string> sendMsg()
		{

			string msg = "강민성 고객님 \r\n ETOMARS에서 알려드립니다. \r\n 수입통관에 해당하는 물품을 구매시 \r\n 관세법 시행령 제289조 에 의거하여 \r\n 아래와 같은 추가 정보를 요청 드립니다. \r\n";
			msg += "- 주민등록번호 / 개인통관고유번호 \r\n 개인통관고유번호 발급 \r\n https://unipass.customs.go.kr/csp/persIndex.do \r\n \r\n ";
			msg += "ETOMARS \r\n https://www.etomars.com \r\n 공식 대행 등록 업체 \r\n ETOMARS \r\n https://www.etomars.com \r\n 070 7661 1030";
			//알림톡 수신자
			string phone = "01020281393";

			//알림톡 발신자 -- sendnumber_save 를 통헤 등록/인증 된 번호만 사용 가능
			string callback = "01020281393";

			HttpResponse<string> request = Unirest.post("http://api.apistore.co.kr/kko/1/msg/etomars")
			.header("x-waple-authorization", "ODI1Ni0xNTI1MzMwOTM1NTIxLTg2YTk0NGZiLTk4NDMtNDJiYS1hOTQ0LWZiOTg0M2IyYmEwNg==")
			.header("Content - Type", "application/x-www-form-urlencoded;charset=UTF-8")
			.field("phone", phone)	//수신자 전화번호
			.field("callback", callback)	//발신자 전화번호
			.field("msg", msg)
			.field("template_code", "test01")
			.field("failed_type", "N") // LMS / SMS / N
			.field("FAILED_SUBJECT","LMS일경우 실패시 제목")
			.field("FAILED_MSG", "SMS/LMS 전송시 내용")

			.field("username", "강민성")
			.field("company", "etomars")

			.asJson<string>();

			return request;

		}


		//전송 상태 조회
		public HttpResponse<string> report()
		{

			//cmid String No 서버에서 생성한 request를 식별할 수 있는 유일한 키 --- 알림톡 전송시 리턴값으로 발급
			string cmid = "";
			HttpResponse<string> request = Unirest.get("http://api.apistore.co.kr/kko/1/report/etomars")
			.header("x-waple-authorization", "ODI1Ni0xNTI1MzMwOTM1NTIxLTg2YTk0NGZiLTk4NDMtNDJiYS1hOTQ0LWZiOTg0M2IyYmEwNg==")
			.header("Content - Type", "application/x-www-form-urlencoded;charset=UTF-8")

			.field("cmid", cmid)
			.asJson<string>();

			return request;

		}



		//탬플릿 리스트
		public HttpResponse<string> template_list()
		{
			HttpResponse<string> request = Unirest.get("http://api.apistore.co.kr/kko/1/template/list/etomars")
			.header("x-waple-authorization", "ODI1Ni0xNTI1MzMwOTM1NTIxLTg2YTk0NGZiLTk4NDMtNDJiYS1hOTQ0LWZiOTg0M2IyYmEwNg==")
			.header("Content - Type", "application/x-www-form-urlencoded;charset=UTF-8")
			.asJson<string>();

			return request;

		}


		//발신번호 인증/등록 
		public HttpResponse<string> sendnumber_save()
		{
			/*
			 * 발신자 전화번호를 먼저 등록하고 인증 하여야 MSG발송이 가능
			 *  최초 등록은 sendnumber / comment / pintype 만 입력하여 API 실행 후
			 *  문자로 받은 인증 키를 pincode 에 입력하여 한번더 호출 
			 */
			HttpResponse<string> request = Unirest.post("http://api.apistore.co.kr/kko/2/sendnumber/save/etomars")
			.header("x-waple-authorization", "ODI1Ni0xNTI1MzMwOTM1NTIxLTg2YTk0NGZiLTk4NDMtNDJiYS1hOTQ0LWZiOTg0M2IyYmEwNg==")
			.header("Content - Type", "application/x-www-form-urlencoded;charset=UTF-8")
			.field("sendnumber", "07076611030")  //발신자 전화번호

			.field("comment", "ETOMARS 테스터")

			.field("pintype", "SMS")
			.field("pincode", "107774")

			.asJson<string>();

			return request;

		}


		//발신번호 리스트
		public HttpResponse<string> sendnumber_list()
		{
			
			HttpResponse<string> request = Unirest.post("http://api.apistore.co.kr/kko/1/sendnumber/list/etomars")
			.header("x-waple-authorization", "ODI1Ni0xNTI1MzMwOTM1NTIxLTg2YTk0NGZiLTk4NDMtNDJiYS1hOTQ0LWZiOTg0M2IyYmEwNg==")
			.header("Content - Type", "application/x-www-form-urlencoded;charset=UTF-8")
			.asJson<string>();

			return request;

		}



	}



}