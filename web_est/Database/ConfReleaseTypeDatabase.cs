using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using comm_global;
using comm_model;
using comm_dbconn;

using System.Data;

namespace web_est
{
    // 출고 타입 DB 연결
    public static class ConfReleaseTypeDatabase
    {
        // 출고 타입 목록 갯수를 리턴한다
        public static int GetReleaseTypeDataCount()
        {
            string sql1 = "select count(*) as COUNT from conf_release_type";
            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return -1;
            else if (dt1.Rows.Count == 0)
                return 0;

            int COUNT = GlobalFunction.GetInt(dt1.Rows[0]["COUNT"].ToString().Trim());

            return COUNT;
        }


        // 출고 타입 목록을 리턴한다
        public static List<ConfReleaseTypeModels> GetReleaseTypeDataList(int offset = 0, int limit = 0)
        {
            string sql1 = "select "
                        + "R.SEQNO"
                        + ", R.NATION_CODE"
                        + ", R.RELEASE_NAME"
                        + ", R.RELEASE_CODE"
                        + ", R.MEMO"
                        + ", R.DELV_CODE"

                        + ", N.NATIONNAME"
                        + ", N.NATIONNAME_ko_KR"
                        + ", N.NATIONNAME_zh_CN"

                        + " from conf_release_type R"

                        + " left outer join comm_nation N"
                        + " on N.NATIONNO=R.NATION_CODE"

                        + " order by R.SEQNO"
                        + ((offset >= 0 && limit > 0) ? string.Format(" limit {0}, {1}", offset, limit) : "")
                        + "";

            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return null;



            // 현재 선택된 언어
            string org_language = System.Globalization.CultureInfo.CurrentCulture.Name;

            List<ConfReleaseTypeModels> RELEASE_LIST = new List<ConfReleaseTypeModels>();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                ConfReleaseTypeModels RELEASE = new ConfReleaseTypeModels();

                RELEASE.SEQNO = GlobalFunction.GetInt(dt1.Rows[i]["SEQNO"].ToString().Trim());  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
                RELEASE.NATION_CODE = dt1.Rows[i]["NATION_CODE"].ToString().Trim().ToUpper();  //	char(2)			국가코드	
                RELEASE.RELEASE_NAME = dt1.Rows[i]["RELEASE_NAME"].ToString().Trim();  //	varchar(50)			출고타입명
                RELEASE.RELEASE_CODE = dt1.Rows[i]["RELEASE_CODE"].ToString().Trim().ToUpper();  //	varchar(20)			출고타입 코드	
                RELEASE.MEMO = dt1.Rows[i]["MEMO"].ToString().Trim();  //	varchar(100)			출고타입 설명	
                RELEASE.DELV_CODE = dt1.Rows[i]["DELV_CODE"].ToString().Trim().ToUpper();  //	char(1)			출고타입 기호(A~Z)

                // 커스텀 필드
                if (org_language == "ko-KR")
                    RELEASE.NATION_NAME = dt1.Rows[i]["NATIONNAME_ko_KR"].ToString().Trim();  // 국가명
                else if (org_language == "zh-CN")
                    RELEASE.NATION_NAME = dt1.Rows[i]["NATIONNAME_zh_CN"].ToString().Trim();  // 국가명
                else
                    RELEASE.NATION_NAME = dt1.Rows[i]["NATIONNAME"].ToString().Trim();  // 국가명

                RELEASE_LIST.Add(RELEASE);
            }

            return RELEASE_LIST;
        }
    }
}