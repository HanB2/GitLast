using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class CommLoginLog
	{

		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		[MaxLength(5)]
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		[MaxLength(8)]
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		[MaxLength(50)]
		public string EMAIL { get; set; }       //	varchar(50)			사용자 이메일	
		public string LOGDATETIME { get; set; }     //	datetime			로그인 시간	
		[MaxLength(20)]
		public string IPADDR { get; set; }      //	varchar(20)			로그인 IP
		[MaxLength(10)]
		public string TYPE { get; set; }        //	varchar(10)			입력구분	

	}
}
