using comm_dbconn;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using web_esm.Models_Act.Setting;

namespace web_esm.Models_Db
{
	public class SettingDbModels : DatabaseConnection
	{

		string[] strOPT_KEY = {
			"emaile_sender_server",
			"emaile_sender_port",
			"emaile_sender_id",
			"emaile_sender_pw",
			"emaile_sender_email",
			"emaile_sender_name", 
		};

		public SettingEmailModels GetEmailSettingModel()
		{
			string errorStr = "";


			SettingEmailModels result = new SettingEmailModels();

			string sqlQueryStr = "SELECT SET_KEY, SET_VALUE FROM esm_settings WHERE SET_KEY in ('" + string.Join("','", strOPT_KEY) + "')";

			DataTable dt = getQueryResult(sqlQueryStr, out errorStr);

			if (dt == null || dt.Rows.Count == 0)
			{
				List<string> queryList = new List<string>();
				for (int i = 0; i < strOPT_KEY.Length; i++)
				{
					queryList.Add("INSERT INTO  esm_settings (SET_KEY, SET_VALUE) VALUES ('" + strOPT_KEY[i]+"', '') ");
				}
				exeQuery(queryList, out errorStr);
			}
			else
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (dt.Rows[i]["SET_KEY"].ToString() == "emaile_sender_server") { result.emaile_sender_server = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "emaile_sender_port") { result.emaile_sender_port = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "emaile_sender_id") { result.emaile_sender_id = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "emaile_sender_pw") { result.emaile_sender_pw = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "emaile_sender_email") { result.emaile_sender_email = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "emaile_sender_name") { result.emaile_sender_name = dt.Rows[i]["SET_VALUE"].ToString(); }
				}
			}

			return result;
		}

		public bool SetEmailSettingModel(SettingEmailModels model)
		{
			string errorStr = "";
			bool result = false;
			

			List<string> queryList = new List<string>();
			queryList.Add("UPDATE esm_settings SET SET_VALUE ='" + model.emaile_sender_server	+ "' WHERE SET_KEY ='emaile_sender_server' ");
			queryList.Add("UPDATE esm_settings SET SET_VALUE ='" + model.emaile_sender_port		+ "' WHERE SET_KEY ='emaile_sender_port' ");
			queryList.Add("UPDATE esm_settings SET SET_VALUE ='" + model.emaile_sender_id		+ "' WHERE SET_KEY ='emaile_sender_id' ");
			queryList.Add("UPDATE esm_settings SET SET_VALUE ='" + model.emaile_sender_pw		+ "' WHERE SET_KEY ='emaile_sender_pw' ");
			queryList.Add("UPDATE esm_settings SET SET_VALUE ='" + model.emaile_sender_email	+ "' WHERE SET_KEY ='emaile_sender_email' ");
			queryList.Add("UPDATE esm_settings SET SET_VALUE ='" + model.emaile_sender_name		+ "' WHERE SET_KEY ='emaile_sender_name' ");
			
			result = exeQuery(queryList, out errorStr);
		
			return result;
		}

	}


	
}