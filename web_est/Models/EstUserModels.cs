using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using comm_model;

namespace web_est
{
    // Station 사용자
    public class EstUserModels : EstUser
    {
        // 2018-06-18 jsy : 생성자에서 변수 초기화
        public EstUserModels()
        {
            SEQNO = 0;       //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
            EST_CODE = "";        //	varchar(5)			STATION 코드	
            EMAIL = "";       //	varchar(50)	UNI		이메일 주소	
            PASSWD = "";      //	varchar(255)			비밀번호 (암호화)	
            USERNAME = "";        //	varchar(30)			사용자 이름	
            TELNO = "";       //	varchar(20)			사용자 전화번호	
            GROUP_ID = 0;       //	int(10) unsigned			그룹 ID	EST_GROUP
            CREATETIME = "";      //	datetime			생성날짜	
            DEPARTMENT = "";      //	varchar(30)			부서	
            POSITION = "";        //	varchar(30)			직급	
            MEMO = "";        //	varchar(100)			메모	
            STATUS = -1;     //	tinyint(4)			상태(0=사용중, 1=중지됨)
        }
    }
}