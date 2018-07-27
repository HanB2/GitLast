using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class ConfCustomsFee
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드("00000" : 기본 통관배송비)	
		public string NATION_CODE { get; set; }     //	char(2)			국가코드	
		public string RELEASE_CODE { get; set; }        //	varchar(20)			출고타입 코드	
		public double WEIGHT { get; set; }      //	double			무게(kg : 소수점 3자리)	
		public double CUSTOMS_FEE { get; set; }     //	double			통관배송비(MAR : 소수점 2자리)	

	}
}
