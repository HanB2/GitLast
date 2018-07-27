using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EtcWorkLoad
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public string WAYBILLNO { get; set; }       //	varchar(30)			송장번호	
		public int WORKING_TYPE { get; set; }       //	tinyint(4)			작업내용(20=팩킹검수, 40=무게변경및송장출력, ...)	
		public int WORKER_ID { get; set; }      //	int			작업자ID : EST_USER / ESE_USER 의 SEQNO	
		public string DATETIME_UPD { get; set; }        //	datetime			작업시간	

	}
}
