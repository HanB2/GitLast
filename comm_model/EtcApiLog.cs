using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EtcApiLog
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드
		public string WAYBILLNO { get; set; }       //	varchar(30)			송장번호
		public string API_TYPE { get; set; }        //	varchar(20)			API 종류(STO, YTO, …)
		public string API_URL { get; set; }     //	varchar(100)			API 요청URL
		public string DATETIME_LOG { get; set; }        //	datetime			입력시간
		public string REQUEST { get; set; }     //	varchar(4000)			요청값
		public string RESPONSE { get; set; }        //	varchar(1000)			응답값 
		public string ERROR_MESSAGE { get; set; }       //	varchar(1000)			오류내용

	}
}
