using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class OrdMemo
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public string WAYBILLNO { get; set; }       //	varchar(30)			송장번호	
		public string DATETIME_UPD { get; set; }        //	datetime			입력일자	
		public int USER_TYPE { get; set; }      //	smallint(5)			입력한 사람(100=ESM, 50=EST, 10=ESE)	
		public int DISPLAY_MODE { get; set; }       //	smallint(5)			볼수있는 사람(100=ESM, 50=ESM/EST, 10=ESM/EST/ESE, 0=전체)	
		public string MEMO { get; set; }        //	varchar(1000)			메모	
		public string AUTHOR { get; set; }      //	varchar(50)			입력한 사람 이름	
		public string IPADDR { get; set; }      //	varchar(15)			입력한 사람 IP주소	
		public string AUTHOR_EMAIL { get; set; }        //	varchar(20)			입력한 사람 EMAIL	
		public int DELETE_STATUS { get; set; }      //	smallint(5)			0=입력됨, 1=삭제됨	
		public int READ_YN { get; set; }        //	smallint(5)			0=읽지않음, 1=읽음	

	}
}
