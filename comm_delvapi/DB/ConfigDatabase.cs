using comm_dbconn;
using comm_delvapi.Model;
using comm_global;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace comm_delvapi.DB
{
	class ConfigDatabase : DatabaseConnection
	{

		// 송장번호 가져오기
		public static string GetInvoiceNoOnes(string EST_CODE, out string error_str, string invoice_type, string invoice_type_ex = "")
		{
			error_str = "";

			//"CONFIG_INVOICE_RANGE";  // 송장번호 권역대 테이블명 ??
			string sql1 = "SELECT SEQNO, EST_CODE, HBLNO_TYPE, HBLNO_START, HBLNO_END, HBLNO_CURRENT, DIGIT, PREFIX, POSTFIX, HBLNO_TYPE_EX "
						+ " FROM " + GlobalSettings.INVOICE_RANGE_TABLE_COMM
						+ string.Format(" where EST_CODE='{0}'", EST_CODE)
						+ string.Format(" and HBLNO_TYPE='{0}'", invoice_type)
						+ ((invoice_type_ex.Length > 0) ? string.Format(" and HBLNO_TYPE_EX='{0}'", invoice_type_ex) : "")
						+ " and USE_YN='y' order by SEQNO";
			

			DataTable dtInvoice = new DataTable();
			MySqlConnection conn = new MySqlConnection();
			MySqlTransaction tran = null;
			try
			{
				conn = new MySqlConnection(GetDBConnStr());
				conn.Open();
				
				tran = conn.BeginTransaction(IsolationLevel.Serializable); // 트랜잭션시작

				MySqlCommand cmd2 = new MySqlCommand(sql1, conn, tran);
				MySqlDataAdapter sda = new MySqlDataAdapter();
				sda.SelectCommand = cmd2;
				sda.Fill(dtInvoice);
			}
			catch (MySqlException e)
			{
				error_str = e.Message;
				conn.Close();
				return null;
			}

			if (dtInvoice == null)
			{
				error_str = comm_global.Language.Resources.API_DATABASE_ERROR;
				conn.Close();
				return null;
			}
			if (dtInvoice.Rows.Count == 0)
			{
				error_str = comm_global.Language.Resources.API_YOU_HAVE_USED_ALL_THE_INVOICE_NUMBERS;
				error_str += " " + comm_global.Language.Resources.API_REMAINING_NUMBER;
				error_str += " : 0";
				conn.Close();
				return null;
			}
						
			long TOTAL_NUM = 0;
			try
			{
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					long HBLNO_END = long.Parse(dtInvoice.Rows[i]["HBLNO_END"].ToString().Trim());  // 끝번호
					long HBLNO_CURRENT = long.Parse(dtInvoice.Rows[i]["HBLNO_CURRENT"].ToString().Trim());  // 마지막사용번호
					long REMAIN_COUNT = 0;  // 남은갯수

					// 2018-02-26 jsy : AIRPORTLOG 인 경우는 11씩 증가한다
					if (invoice_type == "CN_AIRPORTLOG")
						REMAIN_COUNT = (HBLNO_END - (HBLNO_CURRENT + 11)) / 11 + 1;  // 남은갯수
					else
						REMAIN_COUNT = (HBLNO_END - HBLNO_CURRENT);  // 남은갯수

					if (REMAIN_COUNT > 0)
					{
						TOTAL_NUM += REMAIN_COUNT;
					}
				}
			}
			catch (MySqlException e)
			{
				error_str = e.Message;
				conn.Close();
				return null;
			}

			if (TOTAL_NUM == 0)
			{
				error_str = comm_global.Language.Resources.API_YOU_HAVE_USED_ALL_THE_INVOICE_NUMBERS;
				error_str += " " + comm_global.Language.Resources.API_REMAINING_NUMBER;
				error_str += string.Format(" : {0}", TOTAL_NUM);
				conn.Close();
				return null;
			}
			
			string invoiceList = "";


			List<string> sql_list = new List<string>();

			//DateTime CURRENT_TIME = BranchDatabase.GetCurrentTime(rainbow_code);	//개별 시간 사용 안함 으로 변경
			DateTime CURRENT_TIME = DateTime.Now;

			int num = 0;
			for (int i = 0; i < dtInvoice.Rows.Count; i++)
			{
				long HBLNO_END = long.Parse(dtInvoice.Rows[i]["HBLNO_END"].ToString().Trim());  // 끝번호
				long HBLNO_CURRENT = long.Parse(dtInvoice.Rows[i]["HBLNO_CURRENT"].ToString().Trim());  // 마지막사용번호
				long REMAIN_COUNT = 0;  // 남은갯수

				// 2018-02-26 jsy : AIRPORTLOG 인 경우는 11씩 증가한다
				if (invoice_type == "CN_AIRPORTLOG")
					REMAIN_COUNT = (HBLNO_END - (HBLNO_CURRENT + 11)) / 11 + 1;  // 남은갯수
				else
					REMAIN_COUNT = (HBLNO_END - HBLNO_CURRENT);  // 남은갯수

				if (REMAIN_COUNT <= 0)
					continue;



				long SEQNO = long.Parse(dtInvoice.Rows[i]["SEQNO"].ToString().Trim());
				int DIGIT = GlobalFunction.GetInt(dtInvoice.Rows[i]["DIGIT"].ToString().Trim());
				string PREFIX = dtInvoice.Rows[i]["PREFIX"].ToString().Trim();  // 접두어
				string POSTFIX = dtInvoice.Rows[i]["POSTFIX"].ToString().Trim();  // 접미어

				long START_NUM = HBLNO_CURRENT + 1;
				int INCREASE_NUM = 1;
				if (invoice_type == "CN_AIRPORTLOG")  // 2018-02-26 jsy : AIRPORTLOG 인 경우는 11씩 증가한다
				{
					START_NUM = ((HBLNO_CURRENT <= 0) ? 1 : (HBLNO_CURRENT + 11));   // "000001" 부터 시작해서 11씩 증가한다
					INCREASE_NUM = 11;
				}

				string retInvoiceNo = "";
				for (long k = START_NUM; k <= HBLNO_END; k += INCREASE_NUM)
				{
					retInvoiceNo = k.ToString();
					while (retInvoiceNo.Length < DIGIT)
					{
						retInvoiceNo = "0" + retInvoiceNo;
					}

					// 2017-11-22 jsy : 대만 EMS인 경우 ==> 마지막 9번째 숫자를 계산해서 추가한다
					string FLAG = "";
					if (invoice_type == "TW_EMS")
					{
						FLAG = GetTwEmsLastNumber(retInvoiceNo);
					}

					if (invoice_type == "MY_DDEX" || invoice_type == "MY_ABX")  // 말레이시아 DD Express
					{
						long invoiceno = GlobalFunction.GetLong(retInvoiceNo);
						long checksum = (invoiceno + 1) % 7;
						invoiceList = string.Format("{0}{1:0000000}{2}{3}", PREFIX, invoiceno, checksum, POSTFIX);
					}
					else if (invoice_type == "KR_CJ")  // 2018-01-23 jsy : 한국 CJ대한통운
					{
						long invoiceno = GlobalFunction.GetLong(retInvoiceNo);
						long checksum = invoiceno % 7;
						invoiceList = string.Format("{0}{1:000000000}{2}", PREFIX, invoiceno, checksum);
					}
					else if (invoice_type == "JP_POST")  // 2018-05-30 jsy : 일본 JapanPost
					{
						long invoiceno = GlobalFunction.GetLong(retInvoiceNo);
						long checksum = invoiceno % 7;
						invoiceList = string.Format("{0:00000000000}{1}", invoiceno, checksum);
					}
					else if (invoice_type == "JP_SAGAWA")  // 일본 Sagawa
					{
						long invoiceno = GlobalFunction.GetLong(retInvoiceNo);
						long checksum = invoiceno % 7;
						invoiceList = string.Format("{0:00000000000}{1}", invoiceno, checksum);
					}
					else
					{
						invoiceList = string.Format("{0}{1}{2}{3}", PREFIX, retInvoiceNo, FLAG, POSTFIX);
					}
					
				}

				string sql2 = "update " + GlobalSettings.INVOICE_RANGE_TABLE_COMM + " set"
							+ string.Format(" HBLNO_CURRENT='{0}'", retInvoiceNo)
							+ string.Format(", UPDATETIME='{0}'", CURRENT_TIME.ToString("yyyy-MM-dd HH:mm:ss"))
							+ string.Format(" where SEQNO={0}", SEQNO)
							+ "";
				sql_list.Add(sql2);
				
			}
			
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.Transaction = tran; // 현재사용할트랜잭션객체지정

				for (int i = 0; i < sql_list.Count; i++)
				{
					cmd.CommandText = sql_list[i];// 쿼리지정
					cmd.ExecuteNonQuery(); // 실행
				}

				tran.Commit(); // 트랜잭션commit
				conn.Close();
			}
			catch (MySqlException e)
			{
				tran.Rollback();
				conn.Close();
				error_str = e.Message;
				return null;
			}

			return invoiceList;
		}





		// 송장번호 가져오기
		public static string[] GetInvoiceNo(string rainbow_code, int cnt, out string error_str, string invoice_type/* = "COMM"*/, string invoice_type_ex = "")
		{
			error_str = "";

			if (cnt <= 0)
			{
				error_str = comm_global.Language.Resources.API_INVALID_PARAMETER;
				return null;
			}
			if (cnt > GlobalSettings.MAX_UPLOAD_EXCEL_COUNT)
			{
				error_str = comm_global.Language.Resources.API_MAXIMUM_UPLOAD_LIMIT + string.Format(" : {0}", GlobalSettings.MAX_UPLOAD_EXCEL_COUNT);
				return null;
			}



			string sql1 = "select"
						+ " SEQNO"
						+ ", RAINBOWCODE"
						+ ", HBLNO_TYPE"
						+ ", HBLNO_START"
						+ ", HBLNO_END"
						+ ", HBLNO_CURRENT"
						+ ", DIGIT"
						+ ", PREFIX"
						+ ", POSTFIX"
						+ ", HBLNO_TYPE_EX"
						+ " from " + GlobalSettings.INVOICE_RANGE_TABLE_COMM
						+ string.Format(" where RAINBOWCODE='{0}'", rainbow_code)
						+ string.Format(" and HBLNO_TYPE='{0}'", invoice_type)
						+ ((invoice_type_ex.Length > 0) ? string.Format(" and HBLNO_TYPE_EX='{0}'", invoice_type_ex) : "")
						+ " and USE_YN='y'"
						+ " order by SEQNO"
						+ "";

			DataTable dtInvoice = new DataTable();
			MySqlConnection conn = new MySqlConnection();
			MySqlTransaction tran = null;
			try
			{
				conn = new MySqlConnection(DatabaseConnection.GetDBConnStr());
				conn.Open();

				//OleDbDataAdapter sda = new OleDbDataAdapter(sql1, conn);
				//sda.Fill(dtInvoice);
				//conn.Close();

				tran = conn.BeginTransaction(IsolationLevel.Serializable); // 트랜잭션시작

				MySqlCommand cmd2 = new MySqlCommand(sql1, conn, tran);
				MySqlDataAdapter sda = new MySqlDataAdapter();
				sda.SelectCommand = cmd2;
				sda.Fill(dtInvoice);
			}
			catch (MySqlException e)
			{
				error_str = e.Message;
				conn.Close();
				return null;
			}

			if (dtInvoice == null)
			{
				error_str = comm_global.Language.Resources.API_DATABASE_ERROR;
				conn.Close();
				return null;
			}
			if (dtInvoice.Rows.Count == 0)
			{
				error_str = comm_global.Language.Resources.API_YOU_HAVE_USED_ALL_THE_INVOICE_NUMBERS;
				error_str += " " + comm_global.Language.Resources.API_REMAINING_NUMBER;
				error_str += " : 0";
				conn.Close();
				return null;
			}





			long TOTAL_NUM = 0;
			try
			{
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					long HBLNO_END = long.Parse(dtInvoice.Rows[i]["HBLNO_END"].ToString().Trim());  // 끝번호
					long HBLNO_CURRENT = long.Parse(dtInvoice.Rows[i]["HBLNO_CURRENT"].ToString().Trim());  // 마지막사용번호
					long REMAIN_COUNT = 0;  // 남은갯수

					// 2018-02-26 jsy : AIRPORTLOG 인 경우는 11씩 증가한다
					if (invoice_type == "CN_AIRPORTLOG")
						REMAIN_COUNT = (HBLNO_END - (HBLNO_CURRENT + 11)) / 11 + 1;  // 남은갯수
					else
						REMAIN_COUNT = (HBLNO_END - HBLNO_CURRENT);  // 남은갯수

					if (REMAIN_COUNT > 0)
					{
						TOTAL_NUM += REMAIN_COUNT;
					}
				}
			}
			catch (MySqlException e)
			{
				error_str = e.Message;
				conn.Close();
				return null;
			}

			if (cnt > TOTAL_NUM)
			{
				error_str = comm_global.Language.Resources.API_YOU_HAVE_USED_ALL_THE_INVOICE_NUMBERS;
				error_str += " " + comm_global.Language.Resources.API_REMAINING_NUMBER;
				error_str += string.Format(" : {0}", TOTAL_NUM);
				conn.Close();
				return null;
			}





			string[] invoiceList = new string[cnt];
			List<string> sql_list = new List<string>();

			//DateTime CURRENT_TIME = BranchDatabase.GetCurrentTime(rainbow_code);	//개별 시간 사용 안함 으로 변경
			DateTime CURRENT_TIME = DateTime.Now;

			int num = 0;
			for (int i = 0; i < dtInvoice.Rows.Count; i++)
			{
				long HBLNO_END = long.Parse(dtInvoice.Rows[i]["HBLNO_END"].ToString().Trim());  // 끝번호
				long HBLNO_CURRENT = long.Parse(dtInvoice.Rows[i]["HBLNO_CURRENT"].ToString().Trim());  // 마지막사용번호
				long REMAIN_COUNT = 0;  // 남은갯수

				// 2018-02-26 jsy : AIRPORTLOG 인 경우는 11씩 증가한다
				if (invoice_type == "CN_AIRPORTLOG")
					REMAIN_COUNT = (HBLNO_END - (HBLNO_CURRENT + 11)) / 11 + 1;  // 남은갯수
				else
					REMAIN_COUNT = (HBLNO_END - HBLNO_CURRENT);  // 남은갯수

				if (REMAIN_COUNT <= 0)
					continue;



				long SEQNO = long.Parse(dtInvoice.Rows[i]["SEQNO"].ToString().Trim());
				int DIGIT = GlobalFunction.GetInt(dtInvoice.Rows[i]["DIGIT"].ToString().Trim());
				string PREFIX = dtInvoice.Rows[i]["PREFIX"].ToString().Trim();  // 접두어
				string POSTFIX = dtInvoice.Rows[i]["POSTFIX"].ToString().Trim();  // 접미어

				long START_NUM = HBLNO_CURRENT + 1;
				int INCREASE_NUM = 1;
				if (invoice_type == "CN_AIRPORTLOG")  // 2018-02-26 jsy : AIRPORTLOG 인 경우는 11씩 증가한다
				{
					START_NUM = ((HBLNO_CURRENT <= 0) ? 1 : (HBLNO_CURRENT + 11));   // "000001" 부터 시작해서 11씩 증가한다
					INCREASE_NUM = 11;
				}

				string retInvoiceNo = "";
				for (long k = START_NUM; k <= HBLNO_END; k += INCREASE_NUM)
				{
					retInvoiceNo = k.ToString();
					while (retInvoiceNo.Length < DIGIT)
					{
						retInvoiceNo = "0" + retInvoiceNo;
					}

					// 2017-11-22 jsy : 대만 EMS인 경우 ==> 마지막 9번째 숫자를 계산해서 추가한다
					string FLAG = "";
					if (invoice_type == "TW_EMS")
					{
						FLAG = GetTwEmsLastNumber(retInvoiceNo);
					}

					if (invoice_type == "MY_DDEX" || invoice_type == "MY_ABX")  // 말레이시아 DD Express
					{
						long invoiceno = GlobalFunction.GetLong(retInvoiceNo);
						long checksum = (invoiceno + 1) % 7;
						invoiceList[num++] = string.Format("{0}{1:0000000}{2}{3}", PREFIX, invoiceno, checksum, POSTFIX);
					}
					else if (invoice_type == "KR_CJ")  // 2018-01-23 jsy : 한국 CJ대한통운
					{
						long invoiceno = GlobalFunction.GetLong(retInvoiceNo);
						long checksum = invoiceno % 7;
						invoiceList[num++] = string.Format("{0}{1:000000000}{2}", PREFIX, invoiceno, checksum);
					}
					else if (invoice_type == "JP_POST")  // 2018-05-30 jsy : 일본 JapanPost
					{
						long invoiceno = GlobalFunction.GetLong(retInvoiceNo);
						long checksum = invoiceno % 7;
						invoiceList[num++] = string.Format("{0:00000000000}{1}", invoiceno, checksum);
					}
					else if (invoice_type == "JP_SAGAWA")  // 일본 Sagawa
					{
						long invoiceno = GlobalFunction.GetLong(retInvoiceNo);
						long checksum = invoiceno % 7;
						invoiceList[num++] = string.Format("{0:00000000000}{1}", invoiceno, checksum);
					}
					else
					{
						invoiceList[num++] = string.Format("{0}{1}{2}{3}", PREFIX, retInvoiceNo, FLAG, POSTFIX);
					}

					if (num == cnt)
						break;
				}

				string sql2 = "update " + GlobalSettings.INVOICE_RANGE_TABLE_COMM + " set"
							+ string.Format(" HBLNO_CURRENT='{0}'", retInvoiceNo)
							+ string.Format(", UPDATETIME='{0}'", CURRENT_TIME.ToString("yyyy-MM-dd HH:mm:ss"))
							+ string.Format(" where SEQNO={0}", SEQNO)
							+ "";
				sql_list.Add(sql2);

				if (num == cnt)
					break;
			}





			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.Transaction = tran; // 현재사용할트랜잭션객체지정

				for (int i = 0; i < sql_list.Count; i++)
				{
					cmd.CommandText = sql_list[i];// 쿼리지정
					cmd.ExecuteNonQuery(); // 실행
				}

				tran.Commit(); // 트랜잭션commit
				conn.Close();
			}
			catch (MySqlException e)
			{
				tran.Rollback();
				conn.Close();
				error_str = e.Message;
				return null;
			}

			return invoiceList;
		}



		// AirportLog 송장번호 가져오기
		// 송장번호를 미리 생성해놓고 랜덤하게 설정한다
		public static string[] GetInvoiceNo_AirportLogUsed(string rainbow_code, int cnt, out string error_str)
		{
			error_str = "";

			if (cnt <= 0)
			{
				error_str = comm_global.Language.Resources.API_INVALID_PARAMETER;
				return null;
			}
			if (cnt > GlobalSettings.MAX_UPLOAD_EXCEL_COUNT)
			{
				error_str = comm_global.Language.Resources.API_MAXIMUM_UPLOAD_LIMIT + string.Format(" : {0}", GlobalSettings.MAX_UPLOAD_EXCEL_COUNT);
				return null;
			}



			// AirportLog 송장번호가 몇개 사용되고 몇개 남았는지를 리턴한다
			List<InvoiceRangeUsedModels> USED_LIST = GetInvoiceRangeUsed(rainbow_code);
			if (USED_LIST == null || USED_LIST.Count == 0)
			{
				error_str = comm_global.Language.Resources.API_YOU_HAVE_USED_ALL_THE_INVOICE_NUMBERS;
				error_str += " " + comm_global.Language.Resources.API_REMAINING_NUMBER;
				error_str += " : 0";
				return null;
			}



			string invoice_type = "CN_AIRPORTLOG";

			string sql1 = "select"
						+ " SEQNO"
						+ ", RAINBOWCODE"
						+ ", HBLNO_TYPE"
						+ ", HBLNO_START"
						+ ", HBLNO_END"
						+ ", HBLNO_CURRENT"
						+ ", DIGIT"
						+ ", PREFIX"
						+ ", POSTFIX"
						+ " from " + GlobalSettings.INVOICE_RANGE_TABLE_COMM
						+ string.Format(" where RAINBOWCODE='{0}'", rainbow_code)
						+ string.Format(" and HBLNO_TYPE='{0}'", invoice_type)
						+ " and USE_YN='y'"
						+ " order by SEQNO"
						+ "";

			DataTable dtInvoice = new DataTable();
			MySqlConnection conn = new MySqlConnection();
			MySqlTransaction tran = null;
			try
			{
				conn = new MySqlConnection(DatabaseConnection.GetDBConnStr());
				conn.Open();

				//OleDbDataAdapter sda = new OleDbDataAdapter(sql1, conn);
				//sda.Fill(dtInvoice);
				//conn.Close();

				tran = conn.BeginTransaction(IsolationLevel.Serializable); // 트랜잭션시작

				MySqlCommand cmd2 = new MySqlCommand(sql1, conn, tran);
				MySqlDataAdapter sda = new MySqlDataAdapter();
				sda.SelectCommand = cmd2;
				sda.Fill(dtInvoice);
			}
			catch (MySqlException e)
			{
				error_str = e.Message;
				conn.Close();
				return null;
			}

			if (dtInvoice == null)
			{
				error_str = comm_global.Language.Resources.API_DATABASE_ERROR;
				conn.Close();
				return null;
			}
			if (dtInvoice.Rows.Count == 0)
			{
				error_str = comm_global.Language.Resources.API_YOU_HAVE_USED_ALL_THE_INVOICE_NUMBERS;
				error_str += " " + comm_global.Language.Resources.API_REMAINING_NUMBER;
				error_str += " : 0";
				conn.Close();
				return null;
			}





			long TOTAL_NUM = 0;
			try
			{
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					int SEQNO = GlobalFunction.GetInt(dtInvoice.Rows[i]["SEQNO"].ToString().Trim());
					long HBLNO_END = long.Parse(dtInvoice.Rows[i]["HBLNO_END"].ToString().Trim());  // 끝번호
					long HBLNO_CURRENT = long.Parse(dtInvoice.Rows[i]["HBLNO_CURRENT"].ToString().Trim());  // 마지막사용번호
					long REMAIN_COUNT = 0;  // 남은갯수

					InvoiceRangeUsedModels REMAIN = USED_LIST.Find(m => m.RANGE_SEQNO == SEQNO && m.USE_YN == "n");
					if (REMAIN != null && REMAIN.RANGE_SEQNO > 0)
						REMAIN_COUNT = REMAIN.COUNT;

					if (REMAIN_COUNT > 0)
					{
						TOTAL_NUM += REMAIN_COUNT;
					}
				}
			}
			catch (MySqlException e)
			{
				error_str = e.Message;
				conn.Close();
				return null;
			}

			if (cnt > TOTAL_NUM)
			{
				error_str = comm_global.Language.Resources.API_YOU_HAVE_USED_ALL_THE_INVOICE_NUMBERS;
				error_str += " " + comm_global.Language.Resources.API_REMAINING_NUMBER;
				error_str += string.Format(" : {0}", TOTAL_NUM);
				conn.Close();
				return null;
			}





			string[] invoiceList = new string[cnt];
			List<string> sql_list = new List<string>();
			int num = 0;

			try
			{
				for (int i = 0; i < dtInvoice.Rows.Count; i++)
				{
					int SEQNO = GlobalFunction.GetInt(dtInvoice.Rows[i]["SEQNO"].ToString().Trim());
					long HBLNO_END = long.Parse(dtInvoice.Rows[i]["HBLNO_END"].ToString().Trim());  // 끝번호
					long HBLNO_CURRENT = long.Parse(dtInvoice.Rows[i]["HBLNO_CURRENT"].ToString().Trim());  // 마지막사용번호
					long REMAIN_COUNT = 0;  // 남은갯수

					InvoiceRangeUsedModels REMAIN = USED_LIST.Find(m => m.RANGE_SEQNO == SEQNO && m.USE_YN == "n");
					if (REMAIN != null && REMAIN.RANGE_SEQNO > 0)
						REMAIN_COUNT = REMAIN.COUNT;

					if (REMAIN_COUNT <= 0)
						continue;



					// 사용하지 않은 송장번호를 가져온다
					DataTable dtInvoiceAirportLog = new DataTable();
					try
					{
						string sql3 = "select "
									+ "SEQNO"
									+ ", DELVNO"
									+ " from config_invoice_airportlog"
									+ string.Format(" where RAINBOWCODE='{0}'", rainbow_code)
									+ string.Format(" and RANGE_SEQNO={0}", SEQNO)
									+ " and USE_YN='n'"
									+ "";

						MySqlCommand cmd3 = new MySqlCommand(sql3, conn, tran);
						MySqlDataAdapter sda3 = new MySqlDataAdapter();
						sda3.SelectCommand = cmd3;
						sda3.Fill(dtInvoiceAirportLog);
					}
					catch (MySqlException e)
					{
						error_str = e.Message;
						conn.Close();
						return null;
					}

					if (dtInvoiceAirportLog == null || dtInvoiceAirportLog.Rows.Count == 0)
						continue;

					// 송장번호를 랜덤하게 섞는다
					int[] SEQNO_LIST = new int[dtInvoiceAirportLog.Rows.Count];
					string[] DELVNO_LIST = new string[dtInvoiceAirportLog.Rows.Count];
					for (int k = 0; k < dtInvoiceAirportLog.Rows.Count; k++)
					{
						SEQNO_LIST[k] = GlobalFunction.GetInt(dtInvoiceAirportLog.Rows[k]["SEQNO"].ToString().Trim());
						DELVNO_LIST[k] = dtInvoiceAirportLog.Rows[k]["DELVNO"].ToString().Trim();
					}

					Random rand = new Random((int)DateTime.Now.Ticks);
					int idx, old_int;
					string old_str;
					for (int m = 0; m < dtInvoiceAirportLog.Rows.Count; m++)
					{
						idx = rand.Next(dtInvoiceAirportLog.Rows.Count);
						if (idx == m)
							continue;

						old_int = SEQNO_LIST[m];
						SEQNO_LIST[m] = SEQNO_LIST[idx];
						SEQNO_LIST[idx] = old_int;

						old_str = DELVNO_LIST[m];
						DELVNO_LIST[m] = DELVNO_LIST[idx];
						DELVNO_LIST[idx] = old_str;
					}

					// 송장번호 갯수만큼 리턴한다
					for (int k = 0; k < DELVNO_LIST.Length; k++)
					{
						invoiceList[num++] = DELVNO_LIST[k];

						string sql2 = "update config_invoice_airportlog set "
									+ "USE_YN='y'"
									+ string.Format(" where SEQNO={0}", SEQNO_LIST[k])
									+ "";
						sql_list.Add(sql2);

						if (num == cnt)
							break;
					}

					// 마지막 사용날짜 업데이트
					string sql4 = "update config_invoice_range set "
								+ "UPDATETIME=now()"
								+ string.Format(" where SEQNO={0}", SEQNO)
								+ "";
					sql_list.Add(sql4);

					if (num == cnt)
						break;
				}
			}
			catch (MySqlException e)
			{
				error_str = e.Message;
				conn.Close();
				return null;
			}

			if (cnt > num)
			{
				error_str = comm_global.Language.Resources.API_YOU_HAVE_USED_ALL_THE_INVOICE_NUMBERS;
				error_str += " " + comm_global.Language.Resources.API_REMAINING_NUMBER;
				error_str += string.Format(" : {0}", num);
				conn.Close();
				return null;
			}





			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.Transaction = tran; // 현재사용할트랜잭션객체지정

				for (int i = 0; i < sql_list.Count; i++)
				{
					cmd.CommandText = sql_list[i];// 쿼리지정
					cmd.ExecuteNonQuery(); // 실행
				}

				tran.Commit(); // 트랜잭션commit
				conn.Close();
			}
			catch (MySqlException e)
			{
				tran.Rollback();
				conn.Close();
				error_str = e.Message;
				return null;
			}

			return invoiceList;
		}




		// 금액을 목적지 화폐로 변환한다
		public static double ExchangePrice(
					string rainbow_code
					, string branch_currency
					, double price
					, string from_currency
					, string to_currency
					, List<CurrencyModels> CURRENCY_LIST = null
					)
		{
			string BRANCH_CURRENCY = branch_currency.Trim().ToUpper();  // 출발국가 화폐단위
			double exchange_price = price;
			string FROM_CURRENCY = from_currency.Trim().ToUpper();  // 변환전 화폐단위
			string TO_CURRENCY = to_currency.Trim().ToUpper();  // 목적지 화폐단위

			// 일단 출발국가 화폐로 변경한뒤 목적지 화폐로 변환한다
			if (FROM_CURRENCY != TO_CURRENCY)
			{
				// 먼저 출발국가 화폐로 변경한다
				if (FROM_CURRENCY != BRANCH_CURRENCY)
				{
					double from_rate = 0.0;
					int basic_unit = 0;
					if (CURRENCY_LIST != null && CURRENCY_LIST.Count > 0)
					{
						CurrencyModels CURRENCY = CURRENCY_LIST.Find(m => m.RAINBOWCODE == rainbow_code && m.CURRENCY_UNIT == FROM_CURRENCY);
						if (CURRENCY != null && CURRENCY.CURRENCY_UNIT == FROM_CURRENCY)
						{
							from_rate = CURRENCY.EXCHANGE_RATE;
							basic_unit = (int)CURRENCY.BASIC_UNIT;
						}
					}
					else
					{
						GetExchangeRate(rainbow_code, FROM_CURRENCY, ref from_rate, ref basic_unit);
					}

					if (from_rate <= 0.0 || basic_unit < 0)
						return price;

					if (BRANCH_CURRENCY == "KRW")
						exchange_price = exchange_price * from_rate / basic_unit;
					else //if (BRANCH_CURRENCY == "USD")
						exchange_price = exchange_price / from_rate * basic_unit;
				}

				// 목적지 화폐로 변경한다
				if (TO_CURRENCY != BRANCH_CURRENCY)
				{
					double to_rate = 0.0;
					int basic_unit = 0;
					if (CURRENCY_LIST != null && CURRENCY_LIST.Count > 0)
					{
						CurrencyModels CURRENCY = CURRENCY_LIST.Find(m => m.RAINBOWCODE == rainbow_code && m.CURRENCY_UNIT == TO_CURRENCY);
						if (CURRENCY != null && CURRENCY.CURRENCY_UNIT.Length > 0)
						{
							to_rate = CURRENCY.EXCHANGE_RATE;
							basic_unit = (int)CURRENCY.BASIC_UNIT;
						}
					}
					else
					{
						GetExchangeRate(rainbow_code, TO_CURRENCY, ref to_rate, ref basic_unit);
					}

					if (to_rate <= 0.0 || basic_unit < 0)
						return price;

					if (BRANCH_CURRENCY == "KRW")
						exchange_price = exchange_price / to_rate * basic_unit;
					else //if (BRANCH_CURRENCY == "USD")
						exchange_price = exchange_price * to_rate / basic_unit;
				}
			}

			if (TO_CURRENCY == "KRW")
				exchange_price = Math.Round(exchange_price, 0);
			else //if (BRANCH_CURRENCY == "USD")
				exchange_price = Math.Round(exchange_price, 2);

			return exchange_price;
		}





		// 대만 EMS의 마지막 9번째 숫자를 계산해서 리턴한다
		// ISO MOD 11 계산방법 사용
		public static string GetTwEmsLastNumber(string A)
		{
			string FLAG = "";

			if (A.Length != 8)
				return FLAG;

			string B = "86423597";

			int D = 0;
			for (int i = 0; i < 8; i++)
			{
				int A1 = GlobalFunction.GetInt(A.Substring(i, 1));
				int B1 = GlobalFunction.GetInt(B.Substring(i, 1));
				int C = A1 * B1;
				D += C;
			}

			int E = D / 11;
			int F = E * 11;
			int G = D - F;

			if (G == 0)
			{
				FLAG = "5";
			}
			else if (G == 1)
			{
				FLAG = "0";
			}
			else
			{
				FLAG = (11 - G).ToString();
			}

			return FLAG;
		}




		// 환율을 리턴한다
		public static void GetExchangeRate(string rainbow_code, string currency_unit, ref double exchange_rete, ref int basic_unit)
		{
			exchange_rete = 0.0;
			basic_unit = 0;

			string sql1 = "select "
						+ "AMNT"
						+ string.Format(", (select BASIC_UNIT from config_currency where RAINBOWCODE='{0}' and CURRENCY_UNIT='{1}') as BASIC_UNIT", rainbow_code, currency_unit)
						+ " from config_exchange_rate"
						+ string.Format(" where RAINBOWCODE='{0}'", rainbow_code)
						+ string.Format(" and CURRENCY_UNIT='{0}'", currency_unit)
						+ " order by SEQNO desc"
						+ " limit 1"
						+ "";
			string err1 = "";
			DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
			if (dt1 != null && dt1.Rows.Count > 0)
			{
				exchange_rete = GlobalFunction.GetDouble(dt1.Rows[0]["AMNT"].ToString().Trim(), 4);
				basic_unit = GlobalFunction.GetInt(dt1.Rows[0]["BASIC_UNIT"].ToString().Trim());
			}

			return;
		}




		// AirportLog 송장번호가 몇개 사용되고 몇개 남았는지를 리턴한다
		public static List<InvoiceRangeUsedModels> GetInvoiceRangeUsed(string rainbow_code)
		{
			string sql1 = "select "
						+ "RAINBOWCODE"
						+ ", RANGE_SEQNO"
						+ ", USE_YN"
						+ ", count(*) as COUNT"
						+ " from config_invoice_airportlog"
						+ ((rainbow_code.Length > 0) ? string.Format(" where RAINBOWCODE='{0}'", rainbow_code) : "")
						+ " group by RAINBOWCODE, RANGE_SEQNO, USE_YN"
						+ "";

			string err1 = "";
			DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
			if (dt1 == null)
				return null;



			List<InvoiceRangeUsedModels> INVOICE_RANGE_USED_LIST = new List<InvoiceRangeUsedModels>();

			for (int i = 0; i < dt1.Rows.Count; i++)
			{
				InvoiceRangeUsedModels USED = new InvoiceRangeUsedModels();

				USED.RAINBOWCODE = dt1.Rows[i]["RAINBOWCODE"].ToString().Trim();  // 지점코드
				USED.RANGE_SEQNO = GlobalFunction.GetInt(dt1.Rows[i]["RANGE_SEQNO"].ToString().Trim());  // 송장번호 권역대 순번
				USED.USE_YN = dt1.Rows[i]["USE_YN"].ToString().Trim().ToLower();  // y=사용함, n=사용안함
				USED.COUNT = GlobalFunction.GetInt(dt1.Rows[i]["COUNT"].ToString().Trim());  // 갯수

				INVOICE_RANGE_USED_LIST.Add(USED);
			}

			return INVOICE_RANGE_USED_LIST;
		}



		// Setting값 가져오기
		public static string GetSettings(string agent_code, string meta_key)
		{
			string value = "";

			string sql1 = "select "
						+ "META_VALUE"
						+ " from agent_meta"
						+ string.Format(" where AGENTCODE='{0}'", agent_code)
						+ string.Format(" and META_KEY='{0}'", meta_key)
						+ "";
			string err1 = "";
			DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
			if (dt1 != null && dt1.Rows.Count > 0)
			{
				value = dt1.Rows[0]["META_VALUE"].ToString().Trim();
			}

			return value;
		}




		// 기준통화 데이터 목록을 리턴한다
		public static List<CurrencyModels> GetCurrencyDataList(string rainbow_code)
		{
			string sql1 = "select "
						+ "CC.SEQNO"
						+ ", CC.RAINBOWCODE"
						+ ", CC.CURRENCY_UNIT"
						+ ", CC.CURRENCY_SYMBOL"
						+ ", CC.BASIC_UNIT"
						+ ", CC.MEMO"
						+ ", (select CE.AMNT from CONFIG_EXCHANGE_RATE CE where CE.RAINBOWCODE=CC.RAINBOWCODE and CE.CURRENCY_UNIT=CC.CURRENCY_UNIT order by CE.SEQNO desc limit 1) as EXCHANGE_RATE"
						+ ", (select DATE_FORMAT(CE.DATETIME_UPD,'%Y-%m-%d %T') from CONFIG_EXCHANGE_RATE CE where CE.RAINBOWCODE=CC.RAINBOWCODE and CE.CURRENCY_UNIT=CC.CURRENCY_UNIT order by CE.SEQNO desc limit 1) as DATETIME_UPD"
						+ " from CONFIG_CURRENCY CC"
						+ string.Format(" where CC.RAINBOWCODE='{0}'", rainbow_code)
						+ " order by CC.SEQNO"
						+ "";
			string err1 = "";
			DataTable dt1 = DatabaseConnection.GetDataTableMySQL(sql1, out err1);
			if (dt1 == null)
				return null;



			List<CurrencyModels> CURRENCY_LIST = new List<CurrencyModels>();

			for (int i = 0; i < dt1.Rows.Count; i++)
			{
				CurrencyModels CURRENCY = new CurrencyModels();

				CURRENCY.SEQNO = GlobalFunction.GetInt(dt1.Rows[i]["SEQNO"].ToString().Trim());  // 순번
				CURRENCY.RAINBOWCODE = dt1.Rows[i]["RAINBOWCODE"].ToString().Trim();  // 지점코드
				CURRENCY.CURRENCY_UNIT = dt1.Rows[i]["CURRENCY_UNIT"].ToString().Trim().ToUpper();  // 기준통화 종류(USD, CNY, KRW, EUR ...)
				CURRENCY.CURRENCY_SYMBOL = dt1.Rows[i]["CURRENCY_SYMBOL"].ToString().Trim();  // 기준통화 기호($, ¥, ₩, € ...)
				CURRENCY.BASIC_UNIT = GlobalFunction.GetDouble(dt1.Rows[i]["BASIC_UNIT"].ToString().Trim(), 2);  // 기준 단위
				CURRENCY.MEMO = dt1.Rows[i]["MEMO"].ToString().Trim();  // 메모
				CURRENCY.EXCHANGE_RATE = GlobalFunction.GetDouble(dt1.Rows[i]["EXCHANGE_RATE"].ToString().Trim(), 4);  // 최근 환율
				CURRENCY.DATETIME_UPD = dt1.Rows[i]["DATETIME_UPD"].ToString().Trim();  // 최근 환율 저장날짜

				CURRENCY_LIST.Add(CURRENCY);
			}

			return CURRENCY_LIST;
		}


	}
}
