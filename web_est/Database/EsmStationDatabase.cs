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
    // Station 정보 DB 연결
    public class EsmStationDatabase
    {
        // Station 정보 1개를 리턴한다
        public static EsmStationModels GetStationData(string est_code)
        {
            string sql1 = "select "
                        + "SEQNO"
                        + ", EST_CODE"
                        + ", EST_NAME"
                        + ", ZIPCODE"
                        + ", ADDR"
                        + ", ADDR_EN"
                        + ", TELNO"
                        + ", CREATETIME"
                        + ", MEMO"
                        + ", NATION_CODE"
                        + ", START_AIRPORT"
                        + ", WEIGHT_UNIT"
                        + ", UTC_HOUR"
                        + ", UTC_MINUTE"
                        + ", UTC_SUMMER_TIME"
                        + ", USERINPUTCODE"
                        + ", STATUS"
                        + " from esm_station"
                        + string.Format(" where EST_CODE='{0}'", est_code)
                        + "";

            string err1 = "";
            DataTable EST_STATION = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (EST_STATION == null)
                return null;



            EsmStationModels StationInfo = new EsmStationModels();
            if (EST_STATION.Rows.Count > 0)
            {
                StationInfo.SEQNO = GlobalFunction.GetInt(EST_STATION.Rows[0]["SEQNO"].ToString().Trim());      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
                StationInfo.EST_CODE = EST_STATION.Rows[0]["EST_CODE"].ToString().Trim();        //	varchar(5)	UNI		STATION 코드	
                StationInfo.EST_NAME = EST_STATION.Rows[0]["EST_NAME"].ToString().Trim();        //	varchar(50)			STATION 이름	
                StationInfo.ZIPCODE = EST_STATION.Rows[0]["ZIPCODE"].ToString().Trim();     //	varchar(10)			우편번호	
                StationInfo.ADDR = EST_STATION.Rows[0]["ADDR"].ToString().Trim();        //	varchar(100)			주소	
                StationInfo.ADDR_EN = EST_STATION.Rows[0]["ADDR_EN"].ToString().Trim();     //	varchar(100)			영문주소	
                StationInfo.TELNO = EST_STATION.Rows[0]["TELNO"].ToString().Trim();       //	varchar(20)			전화번호	
                StationInfo.CREATETIME = EST_STATION.Rows[0]["CREATETIME"].ToString().Trim();      //	datetime			생성날짜	
                StationInfo.MEMO = EST_STATION.Rows[0]["MEMO"].ToString().Trim();        //	varchar(200)			메모	
                StationInfo.NATION_CODE = EST_STATION.Rows[0]["NATION_CODE"].ToString().Trim();     //	char(2)			출발국가 코드	
                StationInfo.START_AIRPORT = EST_STATION.Rows[0]["START_AIRPORT"].ToString().Trim();       //	char(3)			출발공항 코드	
                StationInfo.WEIGHT_UNIT = EST_STATION.Rows[0]["WEIGHT_UNIT"].ToString().Trim();     //	char(2)			무게단위(KG / LB)	
                StationInfo.UTC_HOUR = GlobalFunction.GetInt(EST_STATION.Rows[0]["UTC_HOUR"].ToString().Trim());       //	smallint(5)			UTC 적용 시간	
                StationInfo.UTC_MINUTE = GlobalFunction.GetInt(EST_STATION.Rows[0]["UTC_MINUTE"].ToString().Trim());     //	smallint(5)			UTC 적용 분	
                StationInfo.UTC_SUMMER_TIME = GlobalFunction.GetInt(EST_STATION.Rows[0]["UTC_SUMMER_TIME"].ToString().Trim());        //	smallint(5)			Summer Time 적용여부(0=적용안함, 1=적용함)	
                StationInfo.USERINPUTCODE = EST_STATION.Rows[0]["USERINPUTCODE"].ToString().Trim();       //	varchar(20)			사용자 입력 코드(web에서 고객이 신규등록 할때 입력)	
                StationInfo.STATUS = GlobalFunction.GetInt(EST_STATION.Rows[0]["STATUS"].ToString().Trim());     //	smallint(6)			상태(0=사용중, 1=중지됨)
            }

            return StationInfo;
        }


        // Station 무게단위를 리턴한다
        public static string GetWeightUnit(string est_code)
        {
            string sql1 = "select "
                        + "WEIGHT_UNIT"
                        + " from esm_station"
                        + string.Format(" where EST_CODE='{0}'", est_code)
                        + "";

            string err1 = "";
            DataTable EST_STATION = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (EST_STATION == null)
                return "";

            return EST_STATION.Rows[0]["WEIGHT_UNIT"].ToString().Trim().ToUpper();
        }
    }
}