using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EsmStation
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)	UNI		STATION 코드	
		public string EST_NAME { get; set; }        //	varchar(50)			STATION 이름	
		public string ZIPCODE { get; set; }     //	varchar(10)			우편번호	
		public string ADDR { get; set; }        //	varchar(100)			주소	
		public string ADDR_EN { get; set; }     //	varchar(100)			영문주소	
		public string TELNO { get; set; }       //	varchar(20)			전화번호	
		public string CREATETIME { get; set; }      //	datetime			생성날짜	
		public string MEMO { get; set; }        //	varchar(200)			메모	
		public string NATION_CODE { get; set; }     //	char(2)			출발국가 코드	
		public string START_AIRPORT { get; set; }       //	char(3)			출발공항 코드	
		public string WEIGHT_UNIT { get; set; }     //	char(2)			무게단위(KG / LB)	
		public int UTC_HOUR { get; set; }       //	smallint(5)			UTC 적용 시간	
		public int UTC_MINUTE { get; set; }     //	smallint(5)			UTC 적용 분	
		public int UTC_SUMMER_TIME { get; set; }        //	smallint(5)			Summer Time 적용여부(0=적용안함, 1=적용함)	
		public string USERINPUTCODE { get; set; }       //	varchar(20)			사용자 입력 코드(web에서 고객이 신규등록 할때 입력)	
		public int STATUS { get; set; }     //	smallint(6)			상태(0=사용중, 1=중지됨)	

	}
}
