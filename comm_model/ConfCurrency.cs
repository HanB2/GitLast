using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class ConfCurrency
	{
	
		//DB 필드
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)

		[MaxLength(3)]
		[Required(ErrorMessage = "화폐단위를 입력해주세요.")]
		public string CURRENCY_UNIT { get; set; }       //	char(3)			화폐단위	
		[Required(ErrorMessage = "기준단위를 입력해주세요.")]
		public double BASIC_UNIT { get; set; }      //	double			기준단위	
		[MaxLength(100)]
		public string MEMO { get; set; }        //	varchar(100)			메모	

		//커스텀 필드
		public double AMNT { get; set; }        //	double			환율	
		public string DATETIME_UPD { get; set; }        //	datetime			환율입력날짜	

	}
}
