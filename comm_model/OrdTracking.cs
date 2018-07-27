using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class OrdTracking
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드
		public string WAYBILLNO { get; set; }       //	varchar(30)			송장번호
		public string DATETIME_UPD { get; set; }        //	datetime			날짜시간
		public string CONTENT { get; set; }     //	varchar(300)			내용
		public string LOCATION { get; set; }        //	varchar(50)			위치

	}
}
