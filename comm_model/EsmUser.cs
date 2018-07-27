using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EsmUser
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)

		[Required(ErrorMessage = "계정ID 값이 존재하지 않습니다.")]
		[MaxLength(50)]
		public string EMAIL { get; set; }       //	varchar(50)	UNI		이메일 주소	

		[Required(ErrorMessage = "비밀번호 값이 존재하지 않습니다.")]
		[MaxLength(255)]
		public string PASSWD { get; set; }      //	varchar(255)			비밀번호 (암호화)	
		[MaxLength(30)]
		public string USERNAME { get; set; }        //	varchar(30)			사용자 이름
		[MaxLength(20)]
		public string TELNO { get; set; }       //	varchar(20)			사용자 전화번호	

		[Required(ErrorMessage = "GROUP ID 값이 존재하지 않습니다.")]
		public int GROUP_ID { get; set; }       //	int(10) unsigned			그룹 ID	ESM_GROUP

		public string CREATETIME { get; set; }      //	datetime			생성날짜	
		[MaxLength(30)]
		public string DEPARTMENT { get; set; }      //	varchar(30)			부서	
		[MaxLength(30)]
		public string POSITION { get; set; }        //	varchar(30)			직급	
		[MaxLength(100)]
		public string MEMO { get; set; }        //	varchar(100)			메모	
		
		public int STATUS { get; set; }     //	tinyint(4)			상태(0=사용중, 1=중지됨)	


		public string GROUP_NAME { get; set; }       //	

	}
}
