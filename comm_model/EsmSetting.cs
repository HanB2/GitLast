using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EsmSetting
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string SET_KEY { get; set; }     //	varchar(40)	UNI			
		public string SET_VALUE { get; set; }       //	varchar(250)				

	}
}
