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
    // 로그인 이력 관리 DB 연결
    public class CommLoginLogDatabase
    {
        public static bool SaveLoginLog(CommLoginLogModels model)
        {
//if (model.IPADDR == "" || model.IPADDR == "::1")  // test mode
//{
//    return false;
//}

            string sql1 = "insert into comm_login_log("
                        + "EST_CODE"
                        + ", ESE_CODE"
                        + ", EMAIL"
                        + ", LOGDATETIME"
                        + ", IPADDR"
                        + ", TYPE"
                        + ") values("
                        + string.Format("'{0}'", model.EST_CODE)  //	varchar(5)			STATION 코드	
                        + string.Format(", '{0}'", model.ESE_CODE)  //	varchar(8)			SENDER 코드	
                        + string.Format(", '{0}'", model.EMAIL)  //	varchar(50)			사용자 이메일	
                        + ((model.LOGDATETIME.Length > 0) ? string.Format(", '{0}'", model.LOGDATETIME) : ", UTC_TIMESTAMP()")  //	datetime			로그인 시간	
                        + string.Format(", '{0}'", model.IPADDR)  //	varchar(20)			로그인 IP	
                        + string.Format(", '{0}'", model.TYPE)  //	varchar(10)			입력구분
                        + ")";

            string err1 = "";
            return DatabaseConnection.ExcuteQueryMySQL(sql1, out err1);
        }
    }
}