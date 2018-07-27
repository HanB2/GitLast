using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using comm_model;

namespace web_est
{
    // 현지 배송업체
    public class ConfLocalDeliveryModels : ConfLocalDelivery
    {
        // 커스텀 필드
        public string NATION_NAME { get; set; }  // 국가명


        // 생성자에서 변수 초기화
        public ConfLocalDeliveryModels()
        {
            SEQNO = 0;  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
            NATION_CODE = "";  //	char(2)			국가코드
            NAME = "";  //	varchar(50)			배송회사 이름	
            HOMEPAGE = "";  //	varchar(100)			배송회사 홈페이지	
            COM_ID = "";  //	varchar(20)			배송회사 ID
            MEMO = "";  //	varchar(100)			설명

            // 커스텀 필드
            NATION_NAME = "";  // 국가명
        }
    }


    // web에 표시하기 위한 데이터들
    public class ConfLocalDeliveryListModels
    {
        public List<ConfLocalDeliveryModels> Items { get; set; }  // 현지 배송업체 정보
        public PagenationModels Pager { get; set; }  // 페이지 정보
    }
}