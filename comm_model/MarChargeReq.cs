using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class MarChargeReq
	{
		/*
		 * 
		SEQNO
		EST_CODE
		ESE_CODE
		CREATETIME

		BANK_NAME
		BANK_ACCOUNT
		DEPOSIT_NAME
		DEPOSIT_AMOUNT
		DEPOSIT_CURRENCY
		DEPOSIT_DATETIME
		MEMO

		STATUS
		MEMO_ESM
		 */
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public string CREATETIME { get; set; }      //	datetime			입력일자	

		[MaxLength(50)]
		[Required(ErrorMessage = "입금 은행명을 입력해주세요.")]
		public string BANK_NAME { get; set; }       //	varchar(50)			입금은행명
		[MaxLength(50)]
		[Required(ErrorMessage = "계좌번호를 입력해주세요.")]
		public string BANK_ACCOUNT { get; set; }        //	varchar(50)			계좌번호	
		[MaxLength(30)]
		[Required(ErrorMessage = "입금한 사람을 입력해주세요.")]
		public string DEPOSIT_NAME { get; set; }        //	varchar(30)			입금한 사람	

		[Required(ErrorMessage = "입금 금액을 입력해주세요.")]
		public double DEPOSIT_AMOUNT { get; set; }      //	double			입금 금액
		[MaxLength(3)]
		[Required(ErrorMessage = "입금 화폐단위를 입력해주세요.")]
		public string DEPOSIT_CURRENCY { get; set; }        //	char(3)			입금 화폐단위	

		public string DEPOSIT_DATETIME { get; set; }        //	datetime			입금 날짜
		[MaxLength(200)]
		public string MEMO { get; set; }        //	varchar(200)			입금 메모	
		public int STATUS { get; set; }     //	int			처리상태(0=삭제됨, 10=입력됨, 20=입금확인안됨, 30=충전완료)
		[MaxLength(200)]
		public string MEMO_ESM { get; set; }        //	varchar(200)			관리자 메모	

	}
}
