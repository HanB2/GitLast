using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class CommNation
	{

		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		[MaxLength(2)]
		public string NATIONNO { get; set; }        //	char(2)			국가코드	
		[MaxLength(100)]
		public string NATIONNAME { get; set; }      //	varchar(100)			국가명(영어)	
		[MaxLength(100)]
		public string NATIONNAME_ko_KR { get; set; }        //	varchar(100)			국가명(한국어)
		[MaxLength(100)]
		public string NATIONNAME_zh_CN { get; set; }        //	varchar(100)			국가명(중국어)	

	}
}
