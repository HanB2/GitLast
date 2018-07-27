using comm_dbconn;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_esm.Models;

namespace web_esm.Models_Db
{
	public class AccountDbModels : DatabaseConnection
	{
		public EsmUser loginChk(AccountLoginModel loginModel) 
		{
			string sqlQuery = " SELECT SEQNO,EMAIL,USERNAME,GROUP_ID,STATUS  FROM esm_user WHERE EMAIL = '" + loginModel.Email + "' AND PASSWD = '" + loginModel.Password + "' " ;
			string resultStr = "";

			DataTable dt = GetDataTableMySQL(sqlQuery, out resultStr);
			EsmUser model = new EsmUser();
			if (dt != null && dt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(dt.Rows[0]["SEQNO"].ToString().Trim());
				model.EMAIL = dt.Rows[0]["EMAIL"].ToString().Trim();
				model.USERNAME = dt.Rows[0]["USERNAME"].ToString().Trim();
				model.GROUP_ID = int.Parse(dt.Rows[0]["GROUP_ID"].ToString().Trim());
				model.STATUS = int.Parse(dt.Rows[0]["STATUS"].ToString().Trim());
			}
			
			return model;
		}


		public void loginHis(CommLoginLog model)
		{
			string errorStr = "";
			string login_his_str = "INSERT INTO comm_login_log (EMAIL, IPADDR, TYPE) values ('" + model.EMAIL + "', '" + model.IPADDR + "', 'esm') ";
			exeQuery(login_his_str, out errorStr);
		}
	}
}