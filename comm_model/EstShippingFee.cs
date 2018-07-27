using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EstShippingFee
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
        public string ESE_CODE { get; set; }        //	varchar(8)  SENDER 코드("00000000" : 기본 요율표)
        public string NATION_CODE { get; set; }     //	char(2)			국가코드	
		public string RELEASE_CODE { get; set; }        //	varchar(20)			출고타입 코드	
		public double WEIGHT { get; set; }      //	double			무게(kg : 소수점 3자리)	
		public double SHIPPING_FEE_NOR { get; set; }        //	double			일반신청 배송비(MAR : 소수점 2자리)	
		public double SHIPPING_FEE_STC { get; set; }        //	double			보관신청 배송비(MAR : 소수점 2자리)	

	}
}
