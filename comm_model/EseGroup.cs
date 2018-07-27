using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EseGroup
	{
		public int GROUP_ID { get; set; }       //	int(10) unsigned	PRI	auto_increment	그룹 ID (자동증가)
		public string GROUP_NAME { get; set; }      //	varchar(20)	UNI		그룹명	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	

	}
}
