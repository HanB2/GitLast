using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using comm_model;

namespace web_est
{
    // 출고타입별 배송비
    public class EstShippingFeeModels : EstShippingFee
    {
        // 커스텀 변수
        public double CUSTOMS_FEE { get; set; }  // ESM에서 설정한 무게별 통관배송비


        // 생성자에서 변수 초기화
        public EstShippingFeeModels()
        {
            SEQNO = 0;  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
            EST_CODE = "";  //	varchar(5)			STATION 코드	
            ESE_CODE = "";  //	varchar(8)  SENDER 코드("00000000" : 기본 요율표)
            NATION_CODE = "";  //	char(2)			국가코드	
            RELEASE_CODE = "";  //	varchar(20)			출고타입 코드	
            WEIGHT = 0.0;  //	double			무게(kg : 소수점 3자리)	
            SHIPPING_FEE_NOR = 0.0;  //	double			일반신청 배송비(MAR : 소수점 2자리)	
            SHIPPING_FEE_STC = 0.0;  //	double			보관신청 배송비(MAR : 소수점 2자리)

            // 커스텀 변수
            CUSTOMS_FEE = 0.0;  // ESM에서 설정한 무게별 통관배송비(MAR : 소수점 2자리)
        }
    }
}