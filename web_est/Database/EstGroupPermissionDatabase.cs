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
    // STATION 그룹 권한설정
    public class EstGroupPermissionDatabase
    {
        // 권한체크
        public static bool AuthCheck(string menu_id, AUTH_MODE mode, string email)
        {
            string field_name = "";
            if (mode == AUTH_MODE.AUTH_SEARCH)
                field_name = "PER_SELECT";  // 조회(select)
            else if (mode == AUTH_MODE.AUTH_REGISTER)
                field_name = "PER_INSERT";  // 등록(insert)
            else if (mode == AUTH_MODE.AUTH_UPDATE)
                field_name = "PER_UPDATE";  // 수정(update)
            else if (mode == AUTH_MODE.AUTH_DELETE)
                field_name = "PER_DELETE";  // 삭제(delete)
            else
                return false;

            string sql1 = "select "
                        + field_name
                        + " from est_group_permission"
                        + string.Format(" where GROUP_ID=(select GROUP_ID from est_user where EMAIL='{0}')", email)
                        + string.Format(" and EST_CODE=(select EST_CODE from est_user where EMAIL='{0}')", email)
                        + string.Format(" and MENU_ID='{0}'", menu_id)
                        + "";

            string err1 = "";
            DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
            if (dt1 == null || dt1.Rows.Count == 0)
                return false;

            

            int permission = int.Parse(dt1.Rows[0][field_name].ToString().Trim());

            return (permission == 1) ? true : false;
        }
    }
}