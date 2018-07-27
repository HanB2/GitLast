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
    // 출고타입별 배송비 DB 연결
    public static class EstShippingFeeDatabase
    {
        // 출고타입별 기본 배송비를 가져온다
        public static List<EstShippingFeeModels> GetCostTableData(
                    string est_code
                    , string ese_code
                    , string nation_code
                    , string release_code
                    )
        {
            string sql1 = "select "
                        + "SEQNO"
                        + ", EST_CODE"
                        + ", ESE_CODE"
                        + ", NATION_CODE"
                        + ", RELEASE_CODE"
                        + ", WEIGHT"
                        + ", SHIPPING_FEE_NOR"
                        + ", SHIPPING_FEE_STC"
                        + " from est_shipping_fee"
                        + string.Format(" where EST_CODE='{0}'", est_code)
                        + string.Format(" and ESE_CODE='{0}'", ese_code)
                        + string.Format(" and NATION_CODE='{0}'", nation_code)
                        + string.Format(" and RELEASE_CODE='{0}'", release_code)
                        + " order by WEIGHT"
                        + "";

            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return null;



            List<EstShippingFeeModels> SHIPPING_FEE_LIST = new List<EstShippingFeeModels>();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                EstShippingFeeModels SHIPPING_FEE = new EstShippingFeeModels();

                SHIPPING_FEE.SEQNO = GlobalFunction.GetInt(dt1.Rows[i]["SEQNO"].ToString().Trim());  //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
                SHIPPING_FEE.EST_CODE = dt1.Rows[i]["EST_CODE"].ToString().Trim();  //	varchar(5)			STATION 코드	
                SHIPPING_FEE.ESE_CODE = dt1.Rows[i]["ESE_CODE"].ToString().Trim();  //	varchar(8)  SENDER 코드("00000000" : 기본 요율표)
                SHIPPING_FEE.NATION_CODE = dt1.Rows[i]["NATION_CODE"].ToString().Trim();  //	char(2)			국가코드	
                SHIPPING_FEE.RELEASE_CODE = dt1.Rows[i]["RELEASE_CODE"].ToString().Trim();  //	varchar(20)			출고타입 코드	
                SHIPPING_FEE.WEIGHT = GlobalFunction.GetDouble(dt1.Rows[i]["WEIGHT"].ToString().Trim(), 3);  //	double			무게(kg : 소수점 3자리)	
                SHIPPING_FEE.SHIPPING_FEE_NOR = GlobalFunction.GetDouble(dt1.Rows[i]["SHIPPING_FEE_NOR"].ToString().Trim(), 2);  //	double			일반신청 배송비(MAR : 소수점 2자리)	
                SHIPPING_FEE.SHIPPING_FEE_STC = GlobalFunction.GetDouble(dt1.Rows[i]["SHIPPING_FEE_STC"].ToString().Trim(), 2);  //	double			보관신청 배송비(MAR : 소수점 2자리)

                SHIPPING_FEE_LIST.Add(SHIPPING_FEE);
            }

            return SHIPPING_FEE_LIST;
        }


        // 기본요율표를 설정하기 위한 템플릿을 리턴한다
        public static List<EstShippingFeeModels> GetEmptyCostTable(string weight_unit)
        {
            string WEIGHT_UNIT = weight_unit.ToUpper();

            double weight = 0;
            double add_weight = ((weight_unit == "KG") ? 0.5 : 1);
            double max_weight = ((weight_unit == "KG") ? 30 : 60);

            List<EstShippingFeeModels> SHIPPING_FEE_LIST = new List<EstShippingFeeModels>();

            while (Math.Abs(max_weight - weight) > 0.001)
            {
                weight += add_weight;

                EstShippingFeeModels Fee = new EstShippingFeeModels();
                Fee.WEIGHT = weight;
                Fee.SHIPPING_FEE_NOR = 0.0;
                Fee.SHIPPING_FEE_STC = 0.0;
                SHIPPING_FEE_LIST.Add(Fee);
            }

            return SHIPPING_FEE_LIST;
        }


        // 기본요율표 저장
        public static bool BasicCostTableUpdate(List<EstShippingFeeModels> SHIPPING_FEE_LIST)
        {
            List<string> sql_list = new List<string>();

            string sql1 = "delete from est_shipping_fee"
                        + string.Format(" where EST_CODE='{0}'", SHIPPING_FEE_LIST[0].EST_CODE)
                        + string.Format(" and ESE_CODE='{0}'", SHIPPING_FEE_LIST[0].ESE_CODE)
                        + string.Format(" and NATION_CODE='{0}'", SHIPPING_FEE_LIST[0].NATION_CODE)
                        + string.Format(" and RELEASE_CODE='{0}'", SHIPPING_FEE_LIST[0].RELEASE_CODE)
                        + "";
            sql_list.Add(sql1);

            string query = "";
            int cnt = 0;
            for (int i = 0; i < SHIPPING_FEE_LIST.Count; i++)
            {
                if (cnt == 0)
                {
                    query = "insert into est_shipping_fee("
                          + "EST_CODE"
                          + ", ESE_CODE"
                          + ", NATION_CODE"
                          + ", RELEASE_CODE"
                          + ", WEIGHT"
                          + ", SHIPPING_FEE_NOR"
                          + ", SHIPPING_FEE_STC"
                          + ") values("
                          + "";
                }
                else
                {
                    query += ", (";
                }

                query += string.Format("'{0}'", SHIPPING_FEE_LIST[i].EST_CODE);
                query += string.Format(", '{0}'", SHIPPING_FEE_LIST[i].ESE_CODE);
                query += string.Format(", '{0}'", SHIPPING_FEE_LIST[i].NATION_CODE);
                query += string.Format(", '{0}'", SHIPPING_FEE_LIST[i].RELEASE_CODE);
                query += string.Format(", {0:0.000}", SHIPPING_FEE_LIST[i].WEIGHT);
                query += string.Format(", {0:0.00}", SHIPPING_FEE_LIST[i].SHIPPING_FEE_NOR);
                query += string.Format(", {0:0.00}", SHIPPING_FEE_LIST[i].SHIPPING_FEE_STC);
                query += ")";

                cnt++;
                if (cnt >= GlobalSettings.BULK_QUERY_LIMIT || (i + 1) >= SHIPPING_FEE_LIST.Count)
                {
                    sql_list.Add(query);
                    cnt = 0;
                    query = "";
                }
            }

            string err1 = "";
            return DatabaseConnection.ExcuteQueryMySQL(sql_list, out err1);
        }
    }
}