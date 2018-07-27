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
    // 기본통화 및 환율 DB 연결
    public static class ConfCurrencyDatabase
    {
        // 기본통화 목록 갯수를 리턴한다
        public static int GetCurrencyDataCount()
        {
            string sql1 = "select count(*) as COUNT from conf_currency";
            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return -1;
            else if (dt1.Rows.Count == 0)
                return 0;

            int COUNT = GlobalFunction.GetInt(dt1.Rows[0]["COUNT"].ToString().Trim());

            return COUNT;
        }


        // 기본통화 목록을 리턴한다
        public static List<ConfCurrencyModels> GetCurrencyDataList(int offset = 0, int limit = 0)
        {
            string sql1 = "select "
                        + "C.SEQNO"
                        + ", C.CURRENCY_UNIT"
                        + ", C.BASIC_UNIT"
                        + ", C.MEMO"

                        + ", (select E.AMNT from conf_exchange_rate E where E.CURRENCY_UNIT=C.CURRENCY_UNIT order by DATETIME_UPD desc limit 1) as AMNT"
                        + ", (select DATE_FORMAT(E.DATETIME_UPD,'%Y-%m-%d %T') from conf_exchange_rate E where E.CURRENCY_UNIT=C.CURRENCY_UNIT order by DATETIME_UPD desc limit 1) as DATETIME_UPD"

                        + " from conf_currency C"
                        + " order by C.SEQNO"
                        + ((offset >= 0 && limit > 0) ? string.Format(" limit {0}, {1}", offset, limit) : "")
                        + "";

            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null)
                return null;



            List<ConfCurrencyModels> CURRENCY_LIST = new List<ConfCurrencyModels>();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                ConfCurrencyModels CURRENCY = new ConfCurrencyModels();

                CURRENCY.SEQNO = GlobalFunction.GetInt(dt1.Rows[i]["SEQNO"].ToString().Trim());  // 일련번호 (자동증가)
                CURRENCY.CURRENCY_UNIT = dt1.Rows[i]["CURRENCY_UNIT"].ToString().Trim();  // 화폐단위	
                CURRENCY.BASIC_UNIT = GlobalFunction.GetDouble(dt1.Rows[i]["BASIC_UNIT"].ToString().Trim(), 4);  // 기준단위	
                CURRENCY.MEMO = dt1.Rows[i]["MEMO"].ToString().Trim();  // 메모	
                CURRENCY.AMNT = GlobalFunction.GetDouble(dt1.Rows[i]["AMNT"].ToString().Trim(), 4);  // 환율	
                CURRENCY.DATETIME_UPD = dt1.Rows[i]["DATETIME_UPD"].ToString().Trim();  // 환율입력날짜

                CURRENCY_LIST.Add(CURRENCY);
            }

            return CURRENCY_LIST;
        }
    }
}