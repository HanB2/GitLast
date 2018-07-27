using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class MarInOut
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public string DATETIME_UPD { get; set; }        //	datetime			처리일자	
		public int INOUT_TYPE { get; set; }     //	tinyint			구분(1=입금, 2=출금)	
		public string METHOD { get; set; }      //	varchar(10)			입출금 방법(bank, paypal, payoneer, …)	
		public double TOTAL_AMOUNT { get; set; }        //	double			전체금액(현지통화)	
		public double FEE { get; set; }     //	double			수수료(현지통화)	
		public double AMOUNT { get; set; }      //	double			실제금액(현지통화 : 전체금액 - 수수료)	
		public string CURRENCY { get; set; }        //	char(3)			현지통화단위	
		public double EXCHANGE_RATE { get; set; }       //	double			환율(USD : 현지통화단위)	
		public double MAR { get; set; }     //	double			전체금액을 MAR로 환산	
		public string MEMO { get; set; }        //	varchar(200)			메모	
		public int ESM_ID { get; set; }     //	int			처리한 사람 ID : ESM_USER의 SEQNO	

	}
}
