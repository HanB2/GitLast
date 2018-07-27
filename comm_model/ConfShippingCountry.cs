using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class ConfShippingCountry
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		[MaxLength(2)]
		[Required(ErrorMessage = "국가코드 값이 존재하지 않습니다.")]
		public string NATION_CODE { get; set; }     //	char(2)			국가코드	
		[MaxLength(100)]
		public string NATION_NAME { get; set; }     //	varchar(100)			국가명	
		[MaxLength(2)]
		public string WEIGHT_UNIT { get; set; }     //	char(2)			무게단위(KG / LB)
		[MaxLength(3)]
		public string CURRENCY_UNIT { get; set; }       //	char(3)			화폐단위	
		public int USE_YN { get; set; }             //smallint 사용여부

		public string DATETIME_UPD { get; set; }        //	datetime			등록일자

	}
}
