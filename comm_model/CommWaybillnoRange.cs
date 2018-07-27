using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class CommWaybillnoRange
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		public string HBLNO_TYPE { get; set; }      //	varchar(20)			송장번호 종류(YTO, ZTO ...)	
		public string HBLNO_START { get; set; }     //	varchar(30)			시작번호	
		public string HBLNO_END { get; set; }       //	varchar(30)			끝번호	
		public string HBLNO_CURRENT { get; set; }       //	varchar(30)			마지막 사용번호	
		public int DIGIT { get; set; }      //	int(11)			송장번호 전체 자릿수	
		public string PREFIX { get; set; }      //	varchar(10)			접두어	
		public string POSTFIX { get; set; }     //	varchar(10)			접미어	
		public string CREATETIME { get; set; }      //	datetime			생성일자	
		public string UPDATETIME { get; set; }      //	datetime			마지막 사용일자	
		public string USE_YN { get; set; }      //	char(1)			y=사용함, n=사용안함	

	}
}
