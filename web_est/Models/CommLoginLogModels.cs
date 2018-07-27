using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using comm_model;

namespace web_est
{
    // 로그인 이력 관리
    public class CommLoginLogModels : CommLoginLog
    {
        // 2018-06-18 jsy : 생성자에서 변수 초기화
        public CommLoginLogModels()
        {
            SEQNO = 0;      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
            EST_CODE = "";        //	varchar(5)			STATION 코드	
            ESE_CODE = "";        //	varchar(8)			SENDER 코드	
            EMAIL = "";       //	varchar(50)			사용자 이메일	
            LOGDATETIME = "";     //	datetime			로그인 시간	
            IPADDR = "";      //	varchar(20)			로그인 IP	
            TYPE = "";        //	varchar(10)			입력구분
        }
    }
}