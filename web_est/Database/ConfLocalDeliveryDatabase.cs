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
    // 현지 배송업체 DB 연결
    public static class ConfLocalDeliveryDatabase
    {
        // 현지 배송업체 목록 갯수를 리턴한다
        public static int GetLocalDeliveryDataCount()
        {
            string sql1 = "select count(*) as COUNT from conf_local_delivery";
            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return -1;
            else if (dt1.Rows.Count == 0)
                return 0;

            int COUNT = GlobalFunction.GetInt(dt1.Rows[0]["COUNT"].ToString().Trim());

            return COUNT;
        }


        // 현지 배송업체 목록을 리턴한다
        public static List<ConfLocalDeliveryModels> GetLocalDeliveryDataList(int offset = 0, int limit = 0)
        {
            string sql1 = "select "
                        + "L.SEQNO"
                        + ", L.NATION_CODE"
                        + ", L.NAME"
                        + ", L.HOMEPAGE"
                        + ", L.COM_ID"
                        + ", L.MEMO"

                        + ", N.NATIONNAME"
                        + ", N.NATIONNAME_ko_KR"
                        + ", N.NATIONNAME_zh_CN"

                        + " from conf_local_delivery L"

                        + " left outer join comm_nation N"
                        + " on N.NATIONNO=L.NATION_CODE"

                        + " order by L.SEQNO"
                        + ((offset >= 0 && limit > 0) ? string.Format(" limit {0}, {1}", offset, limit) : "")
                        + "";

            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return null;



            // 현재 선택된 언어
            string org_language = System.Globalization.CultureInfo.CurrentCulture.Name;

            List<ConfLocalDeliveryModels> DELIVERY_LIST = new List<ConfLocalDeliveryModels>();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                ConfLocalDeliveryModels DELIVERY = new ConfLocalDeliveryModels();

                DELIVERY.SEQNO = GlobalFunction.GetInt(dt1.Rows[i]["SEQNO"].ToString().Trim());  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
                DELIVERY.NATION_CODE = dt1.Rows[i]["NATION_CODE"].ToString().Trim().ToUpper();  //	char(2)			국가코드
                DELIVERY.NAME = dt1.Rows[i]["NAME"].ToString().Trim();  //	varchar(50)			배송회사 이름	
                DELIVERY.HOMEPAGE = dt1.Rows[i]["HOMEPAGE"].ToString().Trim();  //	varchar(100)			배송회사 홈페이지	
                DELIVERY.COM_ID = dt1.Rows[i]["COM_ID"].ToString().Trim().ToUpper();  //	varchar(20)			배송회사 ID
                DELIVERY.MEMO = dt1.Rows[i]["MEMO"].ToString().Trim();  //	varchar(100)			설명

                // 커스텀 필드
                if (org_language == "ko-KR")
                    DELIVERY.NATION_NAME = dt1.Rows[i]["NATIONNAME_ko_KR"].ToString().Trim();  // 국가명
                else if (org_language == "zh-CN")
                    DELIVERY.NATION_NAME = dt1.Rows[i]["NATIONNAME_zh_CN"].ToString().Trim();  // 국가명
                else
                    DELIVERY.NATION_NAME = dt1.Rows[i]["NATIONNAME"].ToString().Trim();  // 국가명

                DELIVERY_LIST.Add(DELIVERY);
            }

            return DELIVERY_LIST;
        }
    }
}