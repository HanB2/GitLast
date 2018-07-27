using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EstSender
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드

	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	

	
		public string ESE_NAME { get; set; }        //	varchar(100)			SENDER 이름	

		public string ZIPCODE { get; set; }     //	varchar(10)			우편번호

		
		public string ADDR { get; set; }        //	varchar(100)			주소	

		
		public string BIZNO { get; set; }       //	varchar(20)			사업자번호

		
		public string REPRESENTATIVE { get; set; }      //	varchar(30)			대표자 이름

		
		public string TELNO_REP { get; set; }       //	varchar(20)			대표자 전화번호

		
		public string EMAIL_REP { get; set; }       //	varchar(50)			대표자 e-mail

		
		public string TASKMAN { get; set; }     //	varchar(30)			담당자 이름

		
		public string TELNO_TASK { get; set; }      //	varchar(20)			담당자 전화번호

		public string EMAIL_TASK { get; set; }      //	varchar(50)			담당자 e-mail	

		public double MAR { get; set; }     //	double			현재 보유 MAR	

		public string CREATEDATE { get; set; }      //	datetime			생성날짜	

		
		public int STATUS { get; set; }     //	tinyint(4)			상태(0=사용중, 1=삭제됨, 2=신규등록)

		
		public string MEMO { get; set; }        //	varchar(100)			메모

	
		public string HOMEPAGE { get; set; }        //	varchar(100)			홈페이지 URL

		
		public string API_KEY { get; set; }     //	varchar(30)			API 연동 key	

	}
}
