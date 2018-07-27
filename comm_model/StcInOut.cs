using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class StcInOut
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드 (상품 보관요청할 STATION)	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public string BARCODE { get; set; }     //	varchar(50)			바코드	
		public int GOODS_CNT { get; set; }      //	smallint			입출고 수량	
		public string INOUT_YMD { get; set; }       //	date			입출고 일자	
		public int INOUT_TYPE { get; set; }     //	TINYINT			0=입고, 1=출고, 2=불량, 3=재고조정	
		public string RACKNO { get; set; }      //	varchar(20)			랙번호	
		public string NOTE { get; set; }        //	varchar(100)			메모	
		public string EST_EMAIL { get; set; }       //	char(50)			EST_USER - EMAIL / 사용자 이메일 / ID (승인자)	
		public string UPDTIME { get; set; }     //	datetime			입출고 날짜시간	

	}
}
