using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class ConfAirpot
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
		[MaxLength(2)]
		[Required(ErrorMessage = "국가코드 값을 입력해주세요.")]
		public string NATION_CODE { get; set; }     //	char(2)			국가코드	
		[MaxLength(3)]
		public string AIRPORT_CODE { get; set; }        //	char(3)			공항코드	
		[MaxLength(50)]
		[Required(ErrorMessage = "공항이름 값을 입력해주세요.")]
		public string AIRPORT_NAME { get; set; }        //	varchar(50)			공항이름
		[MaxLength(50)]
		public string AIRPORT_LOCATION { get; set; }        //	varchar(50)			공항위치	

	}
}
