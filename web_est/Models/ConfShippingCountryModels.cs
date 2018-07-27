using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using comm_model;

namespace web_est
{
    // 배송가능국가
    public class ConfShippingCountryModels : ConfShippingCountry
    {
        // 생성자에서 변수 초기화
        public ConfShippingCountryModels()
        {
            SEQNO = 0;  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
            NATION_CODE = "";  //	char(2)			국가코드	
            NATION_NAME = "";  //	varchar(100)			국가명	
            WEIGHT_UNIT = "";  //	char(2)			무게단위(KG / LB)
            CURRENCY_UNIT = "";  //	char(3)			화폐단위	
            USE_YN = -1;  //smallint 사용여부(0=사용함, 1=사용안함)
        }
    }


    // web에 표시하기 위한 데이터들
    public class ConfShippingCountryListModels
    {
        public List<ConfShippingCountryModels> Items { get; set; }  // 배송가능국가
        public PagenationModels Pager { get; set; }  // 페이지 정보
    }
}