using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class MarWithdrawReq
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public string CREATETIME { get; set; }      //	datetime			입력일자	
		public string BANK_NAME { get; set; }       //	varchar(200)			입금은행명
		public string BANK_ACCOUNT { get; set; }        //	varchar(50)			계좌번호	
		public string HOLDER_NAME { get; set; }     //	varchar(30)			이름	

		public double REQ_AMOUNT { get; set; }      //	double			출금요청금액(MAR)	
		public string MEMO { get; set; }        //	varchar(200)			출금요청 메모	
		public int STATUS { get; set; }     //	int			처리상태(0=삭제됨, 10=입력됨, 20=출금거부, 30=출금완료)	
		public string MEMO_ESM { get; set; }        //	varchar(200)			관리자 메모	

		public double Mar { get; set; } // varchar(22) 현재 보유 마르

	}
}
