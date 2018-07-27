using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using comm_model;

namespace web_est
{
    // 공항코드
    public class ConfAirportModels : ConfAirpot
    {
        // 커스텀 필드
        public string NATION_NAME { get; set; }  // 국가명


        // 생성자에서 변수 초기화
        public ConfAirportModels()
        {
            SEQNO = 0;  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
            NATION_CODE = "";  //	char(2)			국가코드	
            AIRPORT_CODE = "";  //	char(3)			공항코드	
            AIRPORT_NAME = "";  //	varchar(50)			공항이름
            AIRPORT_LOCATION = "";  //	varchar(50)			공항위치

            // 커스텀 필드
            NATION_NAME = "";  // 국가명
        }
    }


    // web에 표시하기 위한 데이터들
    public class ConfAirportListModels
    {
        public List<ConfAirportModels> Items { get; set; }  // 공항코드 정보
        public PagenationModels Pager { get; set; }  // 페이지 정보
    }
}