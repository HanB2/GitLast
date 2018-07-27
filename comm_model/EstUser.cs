using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EstUser
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		public string EMAIL { get; set; }       //	varchar(50)	UNI		이메일 주소

		[MaxLength(255)]
		public string PASSWD { get; set; }      //	varchar(255)			비밀번호 (암호화)	
		public string USERNAME { get; set; }        //	varchar(30)			사용자 이름	
		public string TELNO { get; set; }       //	varchar(20)			사용자 전화번호	
		public int GROUP_ID { get; set; }       //	int(10) unsigned			그룹 ID	EST_GROUP
		public string CREATETIME { get; set; }      //	datetime			생성날짜	
		public string DEPARTMENT { get; set; }      //	varchar(30)			부서	
		public string POSITION { get; set; }        //	varchar(30)			직급	
		public string MEMO { get; set; }        //	varchar(100)			메모	
		public int STATUS { get; set; }     //	tinyint(4)			상태(0=사용중, 1=중지됨)	

		public string GROUP_NAME { get; set; }       //	그룹이름
	}
}
