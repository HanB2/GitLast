using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using comm_model;

namespace web_est
{
    // 출고 타입
    public class ConfReleaseTypeModels : ConfReleaseType
    {
        // 커스텀 필드
        public string NATION_NAME { get; set; }  // 국가명


        // 생성자에서 변수 초기화
        public ConfReleaseTypeModels()
        {
            SEQNO = 0;  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
            NATION_CODE = "";  //	char(2)			국가코드	
            RELEASE_NAME = "";  //	varchar(50)			출고타입명
            RELEASE_CODE = "";  //	varchar(20)			출고타입 코드	
            MEMO = "";  //	varchar(100)			출고타입 설명	
            DELV_CODE = "";  //	char(1)			출고타입 기호(A~Z)

            // 커스텀 필드
            NATION_NAME = "";  // 국가명
        }
    }


    // web에 표시하기 위한 데이터들
    public class ConfReleaseTypeListModels
    {
        public List<ConfReleaseTypeModels> Items { get; set; }  // 기준통화 및 환율 정보
        public PagenationModels Pager { get; set; }  // 페이지 정보
    }
}