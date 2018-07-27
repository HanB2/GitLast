using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class ConfReleaseType
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		[MaxLength(2)]
		[Required(ErrorMessage = "국가코드를 입력해주세요.")]
		public string NATION_CODE { get; set; }     //	char(2)			국가코드	
		[MaxLength(50)]
		public string RELEASE_NAME { get; set; }        //	varchar(50)			출고타입명
		[MaxLength(20)]
		[Required(ErrorMessage = "출고타입코드 값을 입력해주세요.")]
		public string RELEASE_CODE { get; set; }        //	varchar(20)			출고타입 코드	
		[MaxLength(100)]
		public string MEMO { get; set; }        //	varchar(100)			출고타입 설명	
		[MaxLength(1)]
		public string DELV_CODE { get; set; }       //	char(1)			출고타입 기호(A~Z)	

	}
}
