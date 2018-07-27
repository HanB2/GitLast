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
    // 배송가능국가 DB 연결
    public static class ConfShippingCountryDatabase
    {
        // 배송가능국가 목록 갯수를 리턴한다
        public static int GetShippingCountryDataCount()
        {
            string sql1 = "select count(*) as COUNT from conf_shipping_country where USE_YN=0";  // USE_YN : 0=사용함, 1=사용안함
            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return -1;
            else if (dt1.Rows.Count == 0)
                return 0;

            int COUNT = GlobalFunction.GetInt(dt1.Rows[0]["COUNT"].ToString().Trim());

            return COUNT;
        }


        // 배송가능국가 목록을 리턴한다
        public static List<ConfShippingCountryModels> GetShippingCountryDataList(int offset = 0, int limit = 0)
        {
            string sql1 = "select "
                        + "S.SEQNO"
                        + ", S.NATION_CODE"
                        + ", S.NATION_NAME"
                        + ", S.WEIGHT_UNIT"
                        + ", S.CURRENCY_UNIT"
                        + ", S.USE_YN"

                        + ", N.NATIONNAME"
                        + ", N.NATIONNAME_ko_KR"
                        + ", N.NATIONNAME_zh_CN"

                        + " from conf_shipping_country S"

                        + " left outer join comm_nation N"
                        + " on N.NATIONNO=S.NATION_CODE"

                        + " where S.USE_YN=0"  // USE_YN : 0=사용함, 1=사용안함
                        + " order by S.SEQNO"
                        + ((offset >= 0 && limit > 0) ? string.Format(" limit {0}, {1}", offset, limit) : "")
                        + "";

            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return null;



            // 현재 선택된 언어
            string org_language = System.Globalization.CultureInfo.CurrentCulture.Name;

            List<ConfShippingCountryModels> COUNTRY_LIST = new List<ConfShippingCountryModels>();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                ConfShippingCountryModels COUNTRY = new ConfShippingCountryModels();

                COUNTRY.SEQNO = GlobalFunction.GetInt(dt1.Rows[i]["SEQNO"].ToString().Trim());  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
                COUNTRY.NATION_CODE = dt1.Rows[i]["NATION_CODE"].ToString().Trim().ToUpper();  //	char(2)			국가코드	
                COUNTRY.NATION_NAME = dt1.Rows[i]["NATION_NAME"].ToString().Trim();  //	varchar(100)			국가명	
                COUNTRY.WEIGHT_UNIT = dt1.Rows[i]["WEIGHT_UNIT"].ToString().Trim().ToUpper();  //	char(2)			무게단위(KG / LB)
                COUNTRY.CURRENCY_UNIT = dt1.Rows[i]["CURRENCY_UNIT"].ToString().Trim().ToUpper();  //	char(3)			화폐단위	
                COUNTRY.USE_YN = GlobalFunction.GetInt(dt1.Rows[i]["USE_YN"].ToString().Trim());  //smallint 사용여부(0=사용함, 1=사용안함)

                if (org_language == "ko-KR")
                    COUNTRY.NATION_NAME = dt1.Rows[i]["NATIONNAME_ko_KR"].ToString().Trim();  // 국가명
                else if (org_language == "zh-CN")
                    COUNTRY.NATION_NAME = dt1.Rows[i]["NATIONNAME_zh_CN"].ToString().Trim();  // 국가명
                else
                    COUNTRY.NATION_NAME = dt1.Rows[i]["NATIONNAME"].ToString().Trim();  // 국가명

                COUNTRY_LIST.Add(COUNTRY);
            }

            return COUNTRY_LIST;
        }
    }
}