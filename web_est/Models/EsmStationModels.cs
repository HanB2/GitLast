using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using comm_model;

namespace web_est
{
    // Station 정보
    public class EsmStationModels : EsmStation
    {
        // 2018-06-18 jsy : 생성자에서 변수 초기화
        public EsmStationModels()
        {
            SEQNO = 0;      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
            EST_CODE = "";        //	varchar(5)	UNI		STATION 코드	
            EST_NAME = "";        //	varchar(50)			STATION 이름	
            ZIPCODE = "";     //	varchar(10)			우편번호	
            ADDR = "";        //	varchar(100)			주소	
            ADDR_EN = "";     //	varchar(100)			영문주소	
            TELNO = "";       //	varchar(20)			전화번호	
            CREATETIME = "";      //	datetime			생성날짜	
            MEMO = "";        //	varchar(200)			메모	
            NATION_CODE = "";     //	char(2)			출발국가 코드	
            START_AIRPORT = "";       //	char(3)			출발공항 코드	
            WEIGHT_UNIT = "";     //	char(2)			무게단위(KG / LB)	
            UTC_HOUR = 0;       //	smallint(5)			UTC 적용 시간	
            UTC_MINUTE = 0;     //	smallint(5)			UTC 적용 분	
            UTC_SUMMER_TIME = 0;        //	smallint(5)			Summer Time 적용여부(0=적용안함, 1=적용함)	
            USERINPUTCODE = "";       //	varchar(20)			사용자 입력 코드(web에서 고객이 신규등록 할때 입력)	
            STATUS = -1;     //	smallint(6)			상태(0=사용중, 1=중지됨)	
        }
    }
}