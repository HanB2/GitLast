using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EstMasterBl
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		public string MASTERNO { get; set; }        //	varchar(20)			마스터 번호	
		public string CREATETIME { get; set; }      //	datetime			생성일자	
		public string START_AIRPORT { get; set; }       //	char(3)			출발공항기호	
		public string ARRIVAL_AIRPORT { get; set; }     //	char(3)			도착공항기호	
		public int STATUS { get; set; }     //	smallint(5)			마스터에 포함된 송장상태(100, 200 …)	
		public int OUTREG_STATUS { get; set; }      //	smallint(5)			0=미출고, 1=출고	
		public string DATETIME_OUT { get; set; }        //	datetime			출고된 날짜	
		public string DATETIME_ETD { get; set; }        //	datetime			예상출발 날짜시간	
		public string DATETIME_ETA { get; set; }        //	datetime			예상도착 날짜시간	
		public string RELEASE_CODE { get; set; }        //	varchar(20)			출고타입 코드	
		public double WEIGHT { get; set; }      //	double			무게(사용자 입력)	
		public string MEMO { get; set; }        //	varchar(100)			메모	
		public string NOTICE { get; set; }      //	varchar(200)			공지사항(배송조회 site에 표시됨)	

	}
}
