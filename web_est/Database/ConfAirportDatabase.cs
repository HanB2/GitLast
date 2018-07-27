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
    // 공항코드 DB 연결
    public static class ConfAirportDatabase
    {
        // 공항코드 목록 갯수를 리턴한다
        public static int GetAirportDataCount()
        {
            string sql1 = "select count(*) as COUNT from conf_airport";
            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return -1;
            else if (dt1.Rows.Count == 0)
                return 0;

            int COUNT = GlobalFunction.GetInt(dt1.Rows[0]["COUNT"].ToString().Trim());

            return COUNT;
        }


        // 공항코드 목록을 리턴한다
        public static List<ConfAirportModels> GetAirportDataList(int offset = 0, int limit = 0)
        {
            string sql1 = "select "
                        + "A.SEQNO"
                        + ", A.NATION_CODE"
                        + ", A.AIRPORT_CODE"
                        + ", A.AIRPORT_NAME"
                        + ", A.AIRPORT_LOCATION"

                        + ", N.NATIONNAME"
                        + ", N.NATIONNAME_ko_KR"
                        + ", N.NATIONNAME_zh_CN"

                        + " from conf_airport A"

                        + " left outer join comm_nation N"
                        + " on N.NATIONNO=A.NATION_CODE"

                        + " order by A.SEQNO"
                        + ((offset >= 0 && limit > 0) ? string.Format(" limit {0}, {1}", offset, limit) : "")
                        + "";

            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return null;



            // 현재 선택된 언어
            string org_language = System.Globalization.CultureInfo.CurrentCulture.Name;

            List<ConfAirportModels> AIRPORT_LIST = new List<ConfAirportModels>();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                ConfAirportModels AIRPORT = new ConfAirportModels();

                AIRPORT.SEQNO = GlobalFunction.GetInt(dt1.Rows[i]["SEQNO"].ToString().Trim());  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
                AIRPORT.NATION_CODE = dt1.Rows[i]["NATION_CODE"].ToString().Trim().ToUpper();  //	char(2)			국가코드	
                AIRPORT.AIRPORT_CODE = dt1.Rows[i]["AIRPORT_CODE"].ToString().Trim().ToUpper();  //	char(3)			공항코드	
                AIRPORT.AIRPORT_NAME = dt1.Rows[i]["AIRPORT_NAME"].ToString().Trim();  //	varchar(50)			공항이름
                AIRPORT.AIRPORT_LOCATION = dt1.Rows[i]["AIRPORT_LOCATION"].ToString().Trim();  //	varchar(50)			공항위치

                // 커스텀 필드
                if (org_language == "ko-KR")
                    AIRPORT.NATION_NAME = dt1.Rows[i]["NATIONNAME_ko_KR"].ToString().Trim();  // 국가명
                else if (org_language == "zh-CN")
                    AIRPORT.NATION_NAME = dt1.Rows[i]["NATIONNAME_zh_CN"].ToString().Trim();  // 국가명
                else
                    AIRPORT.NATION_NAME = dt1.Rows[i]["NATIONNAME"].ToString().Trim();  // 국가명

                AIRPORT_LIST.Add(AIRPORT);
            }

            return AIRPORT_LIST;
        }
    }
}