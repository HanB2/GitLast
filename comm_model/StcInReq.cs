using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class StcInReq
	{ 
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드 (상품 보관요청할 STATION)	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public int INPUT_TYPE { get; set; }     //	TINYINT			입고방법	
		public string INPUT_DELVNO { get; set; }        //	char(30)			입고 배송번호	
		public string SENDER_NAME { get; set; }     //	varchar(50)			발송인 이름	
		public string SENDER_TEL { get; set; }      //	char(20)			발송인 전화번호	
		public string SENDER_ADDR { get; set; }     //	varchar(100)			발송인 주소	
		public string MEMO_ESE { get; set; }        //	varchar(100)			메모	
		public string MEMO_EST { get; set; }        //	varchar(100)			관리자메모	
		public int INPUT_STAT { get; set; }     //	TINYINT unsigned			처리상태	
		public string REG_DT { get; set; }      //	DATETIME			등록일자	
		public string UPDT_DT { get; set; }     //	DATETIME			수정일자	
		public string INPUT_DELVNAME { get; set; }     //	varchar(50)			배송자 이름	
		public string INPUT_DELVTELL { get; set; }     //	char(20)			배송자 연락처
		public string CHK_DT { get; set; }     //	DATETIME			검수일자	


		public string EST_NAME { get; set; }
		public string BOXNUM { get; set; }
		public string CNT { get; set; }
		public string NATIONNAME { get; set; }
		public string INPUT_STAT_TEXT { get; set; }
		public string INPUT_TYPE_TEXT { get; set; }
		public string CHK_TEXT { get; set; }
		
	}
}
