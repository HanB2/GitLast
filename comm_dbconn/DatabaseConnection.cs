using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;  // MySQL Connection

using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web;
using comm_global;

namespace comm_dbconn
{
    public class DatabaseConnection
    {
#if DEBUG
        static string DBCONN_STR = "SERVER=14.49.37.164;DATABASE=etomarsv2db;UID=etomarsv2dbadmin;PASSWORD=ewru8tbw54y67SED(TGWERJY%$(&^);Port=16033;CharSet=utf8;";
#else
        static string DBCONN_STR = "SERVER=localhost;DATABASE=etomarsv2db;UID=etomarsv2dbadmin;PASSWORD=ewru8tbw54y67SED(TGWERJY%$(&^);Port=3306;CharSet=utf8;";
#endif










        // select query 결과 리턴
        public static DataTable GetDataTableMySQL(string sql, out string errorStr)
        {
            DataTable dt = null;
            MySqlConnection conn = null;

            errorStr = "";

            try
            {
                conn = new MySqlConnection(DBCONN_STR);
                conn.Open();

                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);

                dt = new DataTable();
                adpt.Fill(dt);

                conn.Close();
            }
            catch (MySqlException ex)
            {
                errorStr = ex.Message + " (Error code : " + ex.Number.ToString() + ")";

                if (conn != null)
                    conn.Close();

                return null;
            }
            catch (Exception ex2)
            {
                errorStr = ex2.Message;

                if (conn != null)
                    conn.Close();

                return null;
            }

            return dt;
        }


        // query 실행
        public static bool ExcuteQueryMySQL(string query, out string errorStr)
        {
            errorStr = "";

            if (query == null || query.Length == 0)
                return false;

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(DBCONN_STR);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (MySqlException ex)
            {
                errorStr = ex.Message + " (Error code : " + ex.Number.ToString() + ")";

                if (conn != null)
                    conn.Close();

                return false;
            }
            catch (Exception ex2)
            {
                errorStr = ex2.Message;

                if (conn != null)
                    conn.Close();

                return false;
            }

            return true;
        }


        // query 여러개 실행
        public static bool ExcuteQueryMySQL(List<string> queryList, out string errorStr)
        {
            errorStr = "";

            if (queryList == null || queryList.Count == 0)
                return false;

            MySqlConnection conn = null;
            MySqlTransaction tran = null;

            try
            {
                conn = new MySqlConnection(DBCONN_STR);
                conn.Open();

                tran = conn.BeginTransaction(); // 트랜잭션시작
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conn;
                cmd.Transaction = tran; // 현재사용할트랜잭션객체지정

                for (int i = 0; i < queryList.Count; i++)
                {
                    cmd.CommandText = queryList[i];// 쿼리지정
                    cmd.ExecuteNonQuery(); // 실행
                }
                tran.Commit(); // 트랜잭션commit

                conn.Close();
            }
            catch (MySqlException ex)
            {
                errorStr = ex.Message + " (Error code : " + ex.Number.ToString() + ")";

                if (tran != null)
                    tran.Rollback();

                if (conn != null)
                    conn.Close();

                return false;
            }
            catch (Exception ex2)
            {
                errorStr = ex2.Message;

                if (tran != null)
                    tran.Rollback();

                if (conn != null)
                    conn.Close();

                return false;
            }

            return true;
        }








		//민성 신규 추가


		static string DBCONN_STR_NEW = "SERVER=14.49.37.164;DATABASE=etomarsv2db;UID=etomarsv2dbadmin;PASSWORD=ewru8tbw54y67SED(TGWERJY%$(&^);Port=16033;CharSet=utf8;";

		public static string MAIN_KEY = "";


		// DB Connection string 을 리턴한다
		static public string GetDBConnStr()
		{
			return GlobalFunction.AESDecrypt_256(DBCONN_STR_NEW, MAIN_KEY);
		}

		// select query 결과 리턴
		public DataTable getQueryResult(string sql, out string errorStr)
		{
			DataTable dt = null;
			MySqlConnection conn = null;

			errorStr = "";

			try
			{
				conn = new MySqlConnection(DBCONN_STR_NEW);
				conn.Open();
				MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
				dt = new DataTable();
				adpt.Fill(dt);
			}
			catch (MySqlException ex)
			{
				dt = null;
				errorStr = ex.Message + " (Error code : " + ex.Number.ToString() + ")";
				
			}
			catch (Exception ex2)
			{
				dt = null;
				errorStr = ex2.Message;
			}
			finally
			{
				if (conn != null)
					conn.Close();
			}
			
			return dt;
		}


		// select query 결과 리턴
		public int getQueryCnt(string sql, out string errorStr)
		{
			DataTable dt = null;
			MySqlConnection conn = null;

			errorStr = "";

			int resultCnt = 0;

			try
			{
				conn = new MySqlConnection(DBCONN_STR_NEW);
				conn.Open();
				MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
				dt = new DataTable();
				adpt.Fill(dt);

				if (dt != null && dt.Rows.Count != 0)
				{
					resultCnt = int.Parse(dt.Rows[0]["cnt"].ToString().Trim());
				}

			}
			catch (MySqlException ex)
			{
				dt = null;
				errorStr = ex.Message + " (Error code : " + ex.Number.ToString() + ")";

			}
			catch (Exception ex2)
			{
				dt = null;
				errorStr = ex2.Message;
			}
			finally
			{
				if (conn != null)
					conn.Close();
			}

			return resultCnt;
		}


		// query 실행
		public bool exeQuery(string query, out string errorStr)
		{
			errorStr = "";
			bool result = false;

			if (query == null || query.Length == 0)
				return result;

			MySqlConnection conn = null;

			try
			{
				conn = new MySqlConnection(DBCONN_STR_NEW);
				conn.Open();

				MySqlCommand cmd = new MySqlCommand(query, conn);
				cmd.ExecuteNonQuery();
				result = true;
			}
			catch (MySqlException ex)
			{
				errorStr = ex.Message + " (Error code : " + ex.Number.ToString() + ")";
			}
			catch (Exception ex2)
			{
				errorStr = ex2.Message;
			}
			finally
			{
				if (conn != null)
					conn.Close();
			}

			return result;
		}


		// query 여러개 실행
		public bool exeQuery(List<string> queryList, out string errorStr)
		{
			errorStr = "";
			bool result = false;

			if (queryList == null || queryList.Count == 0)
				return false;

			MySqlConnection conn = null;
			MySqlTransaction tran = null;

			try
			{
				conn = new MySqlConnection(DBCONN_STR_NEW);
				conn.Open();

				tran = conn.BeginTransaction(); // 트랜잭션시작
				MySqlCommand cmd = new MySqlCommand();

				cmd.Connection = conn;
				cmd.Transaction = tran; // 현재사용할트랜잭션객체지정

				for (int i = 0; i < queryList.Count; i++)
				{
					cmd.CommandText = queryList[i];// 쿼리지정
					cmd.ExecuteNonQuery(); // 실행
				}
				tran.Commit(); // 트랜잭션commit
				result = true;
			}
			catch (MySqlException ex)
			{
				if (tran != null)
					tran.Rollback();
				errorStr = ex.Message + " (Error code : " + ex.Number.ToString() + ")";
				
			}
			catch (Exception ex2)
			{
				if (tran != null)
					tran.Rollback();
				errorStr = ex2.Message;
			}
			finally
			{
				if (conn != null)
					conn.Close();
			}

			return result;
		}


		// 커넥션 가져오기
		public MySqlConnection openConn()
		{
			MySqlConnection conn = null;
			
			try
			{
				conn = new MySqlConnection(DBCONN_STR_NEW);
				conn.Open();
			}
			catch (MySqlException ex)
			{
				conn.Close();
				conn = null;
			}
			catch (Exception ex2)
			{
				conn.Close();
				conn = null;
			}
			return conn;
		}
		
		

	}
}
