using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using comm_model;

namespace web_est
{
    // 기본통화 및 환율
    public class ConfCurrencyModels : ConfCurrency
    {
        // 생성자에서 변수 초기화
        public ConfCurrencyModels()
        {
            // DB 필드
            SEQNO = 0;  // 일련번호 (자동증가)
            CURRENCY_UNIT = "";  // 화폐단위	
            BASIC_UNIT = 0.0;  // 기준단위	
            MEMO = "";  // 메모	

            // 커스텀 필드
            AMNT = 0.0;  // 환율	
            DATETIME_UPD = "";  // 환율입력날짜
        }
    }


    // web에 표시하기 위한 데이터들
    public class ConfCurrencyListModels
    {
        public List<ConfCurrencyModels> Items { get; set; }  // 기준통화 및 환율 정보
        public PagenationModels Pager { get; set; }  // 페이지 정보
    }
}