using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EstGroup
	{
		public int GROUP_ID { get; set; }       //	int(10) unsigned	PRI	auto_increment	그룹 ID (자동증가)	
		public string GROUP_NAME { get; set; }      //	varchar(20)	UNI		그룹명	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	

	}
}
