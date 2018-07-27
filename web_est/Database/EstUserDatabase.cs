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
    // Station 사용자 DB연결
    public class EstUserDatabase
    {
        // Station 사용자 정보 1개를 리턴한다
        public static EstUserModels GetUserData(
                    string email
                    , string password
                    )
        {
            string sql1 = "select "
                        + "SEQNO"
                        + ", EST_CODE"
                        + ", EMAIL"
                        + ", PASSWD"
                        + ", USERNAME"
                        + ", TELNO"
                        + ", GROUP_ID"
                        + ", CREATETIME"
                        + ", DEPARTMENT"
                        + ", POSITION"
                        + ", MEMO"
                        + ", STATUS"
                        + " from est_user"
                        + "";
            if (email.Length > 0 && password.Length > 0)
            {
                sql1 += string.Format(" where EMAIL='{0}'", email);
                sql1 += string.Format(" and PASSWD='{0}'", GlobalFunction.AESEncrypt_256(password));
            }
            else
            {
                return null;
            }

            string err1 = "";
            DataTable EST_USER = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (EST_USER == null)
                return null;



            EstUserModels EstUser = new EstUserModels();
            if (EST_USER.Rows.Count > 0)
            {
                EstUser.SEQNO = GlobalFunction.GetInt(EST_USER.Rows[0]["SEQNO"].ToString().Trim());       //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
                EstUser.EST_CODE = EST_USER.Rows[0]["EST_CODE"].ToString().Trim();        //	varchar(5)			STATION 코드	
                EstUser.EMAIL = EST_USER.Rows[0]["EMAIL"].ToString().Trim();       //	varchar(50)	UNI		이메일 주소	
                EstUser.PASSWD = EST_USER.Rows[0]["PASSWD"].ToString().Trim();      //	varchar(255)			비밀번호 (암호화)	
                EstUser.USERNAME = EST_USER.Rows[0]["USERNAME"].ToString().Trim();        //	varchar(30)			사용자 이름	
                EstUser.TELNO = EST_USER.Rows[0]["TELNO"].ToString().Trim();       //	varchar(20)			사용자 전화번호	
                EstUser.GROUP_ID = GlobalFunction.GetInt(EST_USER.Rows[0]["GROUP_ID"].ToString().Trim());       //	int(10) unsigned			그룹 ID	EST_GROUP
                EstUser.CREATETIME = EST_USER.Rows[0]["CREATETIME"].ToString().Trim();      //	datetime			생성날짜	
                EstUser.DEPARTMENT = EST_USER.Rows[0]["DEPARTMENT"].ToString().Trim();      //	varchar(30)			부서	
                EstUser.POSITION = EST_USER.Rows[0]["POSITION"].ToString().Trim();        //	varchar(30)			직급	
                EstUser.MEMO = EST_USER.Rows[0]["MEMO"].ToString().Trim();        //	varchar(100)			메모	
                EstUser.STATUS = GlobalFunction.GetInt(EST_USER.Rows[0]["STATUS"].ToString().Trim());     //	tinyint(4)			상태(0=사용중, 1=중지됨)
            }

            return EstUser;
        }
    }
}
