using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class ConfLocalDelivery
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
		[MaxLength(2)]
		public string NATION_CODE { get; set; }     //	char(2)			국가코드

		[Required(ErrorMessage = "배송회사명을 입력해주세요.")]
		[MaxLength(50)]
		public string NAME { get; set; }        //	varchar(50)			배송회사 이름	

		[MaxLength(100)]
		public string HOMEPAGE { get; set; }        //	varchar(100)			배송회사 홈페이지	

		[MaxLength(20)]
		public string COM_ID { get; set; }      //	varchar(20)			배송회사 ID

		[MaxLength(100)]
		public string MEMO { get; set; }        //	varchar(100)			설명	

	}
}
