using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_ese.Models_Act.Comm
{
	public class SendMailModel
	{
		public string SERVER { get; set; }  // 메일서버
		public int PORT { get; set; }  // 포트
		public string USERID { get; set; }  // 사용자계정(이메일)
		public string PASSWD { get; set; }  // 비밀번호
		public string SENDER_EMAIL { get; set; }  // 보내는사람 이메일
		public string SENDER_NAME { get; set; }  // 보내는사람 이름
		public bool ENABLE_SSL { get; set; }  // 보안연결(SSL) 사용여부

		public string TITLE { get; set; }  // 제목
		public string BODY { get; set; }  // 내용

		public List<string> MAIL_TO { get; set; }  // 받는사람 이메일
		public List<string> MAIL_CC { get; set; }  // 참조 이메일


		public SendMailModel()
		{
			SERVER = "mail.etomars.com";  // 메일서버
			PORT = 995;  // 포트
			USERID = "minsung@etomars.com";  // 사용자계정(이메일)
			PASSWD = "qwe3449621!!";  // 비밀번호
			SENDER_EMAIL = "minsung@etomars.com";  // 보내는사람 이메일
			SENDER_NAME = "강민성";  // 보내는사람 이름
			ENABLE_SSL = true;  // 보안연결(SSL) 사용여부

			TITLE = "test";  // 제목
			BODY = "가냐?";  // 내용

			MAIL_TO = new List<string>();  // 받는사람 이메일
			MAIL_TO.Add("lakantoq@naver.com");
			MAIL_CC = new List<string>();  // 참조 이메일
		}
	}
}
