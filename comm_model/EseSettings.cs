using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EseSettings
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public string SET_KEY { get; set; }     //	varchar(40)				
		public string SET_VALUE { get; set; }       //	varchar(250)				

	}
}
