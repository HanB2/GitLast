using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_ese_old.Filter;

namespace web_ese_old.Controllers
{
    public partial class EseInfoController : Controller
	{
		[CustomFilter]
		public ActionResult EseInfoAccount()
        {



	


		//   string strConn = "Server=14.49.37.164;Port=16033;Database=etomarsv2db;Uid=etomarsv2dbadmin;Pwd=ewru8tbw54y67SED(TGWERJY %$(&^);";

			string DBCONN_STR = "SERVER=14.49.37.164;DATABASE=etomarsv2db;UID=etomarsv2dbadmin;PASSWORD=ewru8tbw54y67SED(TGWERJY%$(&^);Port=16033;CharSet=utf8;";
			string test = "";
			
			try
			{
				MySqlConnection conn = new MySqlConnection(DBCONN_STR);
				conn.Open();
				MySqlCommand cmd = new MySqlCommand("INSERT INTO pic_req values (2)", conn);
				cmd.ExecuteNonQuery();

				conn.Close();
			}
			catch (MySqlException ex)
			{
				test = ex.Message;
			}
			catch (Exception ex2)
			{
				test = ex2.Message;
			}



			string asd = test;










			return View();
        }
    }
}