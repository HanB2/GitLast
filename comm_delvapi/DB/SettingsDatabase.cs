using comm_dbconn;
using comm_global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_delvapi.DB
{
	class SettingsDatabase : DatabaseConnection
	{
		// Setting값 가져오기 : 1개
		public static string GetSettings(string rainbow_code, string setting_key)
		{
			string value = "";

			string sql1 = "select "
						+ "OPT_VALUE"
						+ " from SETTINGS"
						+ string.Format(" where RAINBOWCODE='{0}'", rainbow_code)
						+ string.Format(" and opt_key='{0}'", setting_key)
						+ "";
			string err1 = "";
			DataTable dt1 = GetDataTableMySQL(sql1, out err1);
			if (dt1 != null && dt1.Rows.Count > 0)
			{
				value = dt1.Rows[0]["opt_value"].ToString().Trim();
			}

			return value;
		}


		// Setting값 가져오기 : 여러개
		public static Dictionary<string, string> GetSettingsDic(string rainbow_code, string setting_key)
		{
			string sql1 = "select "
						+ "OPT_KEY"
						+ ", OPT_VALUE"
						+ " from settings"
						+ string.Format(" where RAINBOWCODE='{0}'", rainbow_code)
						+ ((setting_key.Length > 0) ? string.Format(" and OPT_KEY like '%{0}%'", setting_key) : "")
						+ "";
			string err1 = "";
			DataTable dt1 = GetDataTableMySQL(sql1, out err1);
			if (dt1 == null || dt1.Rows.Count == 0)
			{
				return null;
			}



			Dictionary<string, string> SETTINGS_DIC = new Dictionary<string, string>();

			for (int i = 0; i < dt1.Rows.Count; i++)
			{
				string OPT_KEY = dt1.Rows[i]["OPT_KEY"].ToString().Trim();
				string OPT_VALUE = dt1.Rows[i]["OPT_VALUE"].ToString().Trim();

				SETTINGS_DIC.Add(OPT_KEY, OPT_VALUE);
			}

			return SETTINGS_DIC;
		}


		// Setting 값 저장하기
		public static bool SetSettings(string rainbow_code, string setting_key, string setting_value)
		{
			List<string> sql_list = new List<string>();

			string sql1 = "delete from settings"
						+ string.Format(" where RAINBOWCODE='{0}'", rainbow_code)
						+ string.Format(" and opt_key='{0}'", setting_key)
						+ "";
			sql_list.Add(sql1);

			string sql2 = "insert into settings("
						+ "RAINBOWCODE"
						+ ", OPT_KEY"
						+ ", OPT_VALUE"
						+ ") values("
						+ string.Format("'{0}'", rainbow_code)
						+ string.Format(", '{0}'", setting_key)
						+ string.Format(", '{0}'", setting_value)
						+ ")";
			sql_list.Add(sql2);

			string err1 = "";
			return ExcuteQueryMySQL(sql_list, out err1);
		}


		// Setting 값 저장하기(여러개)
		public static bool SetSettings(string rainbow_code, Dictionary<string, string> setting_dic)
		{
			List<string> sql_list = new List<string>();

			foreach (KeyValuePair<string, string> kv in setting_dic)
			{
				string OPT_KEY = kv.Key;
				string OPT_VALUE = kv.Value;

				string sql1 = "delete from settings"
						+ string.Format(" where RAINBOWCODE='{0}'", rainbow_code)
						+ string.Format(" and opt_key='{0}'", OPT_KEY)
						+ "";
				sql_list.Add(sql1);

				string sql2 = "insert into settings("
							+ "RAINBOWCODE"
							+ ", OPT_KEY"
							+ ", OPT_VALUE"
							+ ") values("
							+ string.Format("'{0}'", rainbow_code)
							+ string.Format(", '{0}'", OPT_KEY)
							+ string.Format(", '{0}'", OPT_VALUE)
							+ ")";
				sql_list.Add(sql2);
			}

			string err1 = "";
			return ExcuteQueryMySQL(sql_list, out err1);
		}



		// 중국주소를 성, 시, 구, 나머지 주소로 나눈다
		public static bool SplitAddress_CN(
					string address
					, ref string state
					, ref string city
					, ref string district
					, ref string addr2
					, ref string zipcode
					, bool valid_check = false
					)
		{
			state = "";
			city = "";
			district = "";
			addr2 = "";
			zipcode = "";

			string ADDRESS = address.Trim().Replace(" ", "");
			if (ADDRESS.IndexOf("中国") == 0) ADDRESS = ADDRESS.Substring(2);



			// 1단계 : 성(省), 특별시(市), 자치구(自治区)
			int index1 = ADDRESS.IndexOf("省");
			if (index1 < 0) index1 = ADDRESS.IndexOf("自治区");
			if (index1 < 0) index1 = ADDRESS.IndexOf("重庆市");
			if (index1 < 0) index1 = ADDRESS.IndexOf("天津市");
			if (index1 < 0) index1 = ADDRESS.IndexOf("北京市");
			if (index1 < 0) index1 = ADDRESS.IndexOf("上海市");
			if (index1 < 0) index1 = ADDRESS.IndexOf("重庆");
			if (index1 < 0) index1 = ADDRESS.IndexOf("天津");
			if (index1 < 0) index1 = ADDRESS.IndexOf("北京");
			if (index1 < 0) index1 = ADDRESS.IndexOf("上海");

			if (index1 >= 0)
			{
				int state_length = 0;

				if (ADDRESS.IndexOf("自治区") > 0)
				{
					state = ADDRESS.Substring(0, (index1 + 3));
					state_length = state.Length;
				}
				else if (ADDRESS.IndexOf("重庆市") >= 0 || ADDRESS.IndexOf("天津市") >= 0 || ADDRESS.IndexOf("北京市") >= 0 || ADDRESS.IndexOf("上海市") >= 0)
				{
					state = ADDRESS.Substring(0, (index1 + 3));
					state_length = state.Length;

					// 北京北京市 처럼 입력하는 경우가 있다...
					if (state.Length > 3)
					{
						int city_index = state.Length - 3;
						state = state.Substring(city_index);
					}
				}
				else if (ADDRESS.IndexOf("重庆") == 0 || ADDRESS.IndexOf("天津") == 0 || ADDRESS.IndexOf("北京") == 0 || ADDRESS.IndexOf("上海") == 0)
				{
					state = ADDRESS.Substring(0, 2) + "市";
					state_length = 2;
				}
				else
				{
					state = ADDRESS.Substring(0, (index1 + 1));  // 省
					state_length = state.Length;
				}

				ADDRESS = ADDRESS.Substring(state_length);
				if (ADDRESS.Length > state_length && ADDRESS.Substring(0, state_length) == state)
				{
					ADDRESS = ADDRESS.Substring(state_length);
				}
			}
			else
			{
				return false;
			}



			// 2단계 : 市, 州, 区, 盟
			if (state == "重庆市" || state == "天津市" || state == "北京市" || state == "上海市")
			{
				city = state;
			}
			else
			{
				int index2 = ADDRESS.IndexOf("市");
				int index3 = ADDRESS.IndexOf("州");
				int index4 = ADDRESS.IndexOf("区");
				int index5 = ADDRESS.IndexOf("盟");
				int index6 = ADDRESS.IndexOf("县");

				int index7 = 999;
				if (index2 >= 2 && index7 > index2) index7 = index2;
				if (index3 >= 2 && index7 > index3) index7 = index3;
				if (index4 >= 2 && index7 > index4) index7 = index4;
				if (index5 >= 2 && index7 > index5) index7 = index5;
				if (index6 >= 2 && index7 > index6) index7 = index6;

				if (index3 == 1 && index2 == 2) index7 = index2;

				if (index7 <= 11)
				{
					city = ADDRESS.Substring(0, (index7 + 1));

					ADDRESS = ADDRESS.Substring(city.Length);
					if (ADDRESS.Length > city.Length && ADDRESS.Substring(0, city.Length) == city)
					{
						ADDRESS = ADDRESS.Substring(city.Length);
					}
				}
				else
				{
					return false;
				}
			}



			// 3단계 : 市, 州, 区, 盟, 县, 旗, 奇
			int index12 = ADDRESS.IndexOf("市");
			int index13 = ADDRESS.IndexOf("州");
			int index14 = ADDRESS.IndexOf("盟");
			int index15 = ADDRESS.IndexOf("旗");
			int index16 = ADDRESS.IndexOf("奇");
			int index17 = ADDRESS.IndexOf("区");
			int index18 = ADDRESS.IndexOf("县");

			int index27 = 999;
			if (index12 >= 2 && index27 > index12) index27 = index12;
			if (index13 >= 2 && index27 > index13) index27 = index13;
			if (index14 >= 2 && index27 > index14) index27 = index14;
			if (index15 >= 2 && index27 > index15) index27 = index15;
			if (index16 >= 2 && index27 > index16) index27 = index16;
			if (index17 >= 1 && index27 > index17) index27 = index17;
			if (index18 >= 1 && index27 > index18) index27 = index18;

			if (index12 == 1) index27 = 2;
			if (index13 == 1)
			{
				if (index12 == 2) index27 = index12;
				else if (index17 == 2) index27 = index17;
				else if (index18 == 2 || index18 == 8) index27 = index18;
			}
			if (index14 == 1 && index18 == 6) index27 = index18;
			if (index15 == 1)
			{
				if (index17 == 2) index27 = index17;
				else if (index18 == 2) index27 = index18;
			}
			if (index18 == 1 && index12 == 2) index27 = index12;

			if (index27 <= 15)
			{
				district = ADDRESS.Substring(0, (index27 + 1));

				ADDRESS = ADDRESS.Substring(district.Length);
				if (ADDRESS.Length > district.Length && ADDRESS.Substring(0, district.Length) == district)
				{
					ADDRESS = ADDRESS.Substring(district.Length);
				}
			}
			else
			{
				//return false;
				district = city;
			}



			// 입력주소 유효성 체크
			if (valid_check)
			{
				string sql3 = "select "
							+ "ZIPCODE"
							+ ", PROV_NAME"
							+ ", CITY_NAME"
							+ ", COUNTY_NAME"
							+ string.Format(" from {0}", GlobalSettings.ZipcodeTable_CN)
							+ string.Format(" where PROV_NAME='{0}'", state)
							+ string.Format(" and CITY_NAME='{0}'", city)
							+ string.Format(" and (COUNTY_NAME='{0}' or COUNTY_NAME like '{1}%')", district, district.Substring(0, district.Length - 1))
							+ " limit 1"
							+ "";
				string err1 = "";
				DataTable dt3 = DatabaseConnection.GetDataTableMySQL(sql3, out err1);
				if (dt3 == null || dt3.Rows.Count == 0)
				{
					//return false;

					// 우편번호가 없는 경우 구는 제외하고 성, 시 두개만 가지고 찾아본다
					string sql4 = "select "
								+ "ZIPCODE"
								+ ", PROV_NAME"
								+ ", CITY_NAME"
								+ ", COUNTY_NAME"
								+ string.Format(" from {0}", GlobalSettings.ZipcodeTable_CN)
								+ string.Format(" where PROV_NAME='{0}'", state)
								+ string.Format(" and CITY_NAME='{0}'", city)
								+ " order by SEQNO"
								+ " limit 1"
								+ "";
					dt3 = DatabaseConnection.GetDataTableMySQL(sql4, out err1);
					if (dt3 == null || dt3.Rows.Count == 0)
					{
						return false;
					}

					// 구가 없는 경우 ==> 시와 동일한 값을 적용한다
					ADDRESS = district + ADDRESS;
					district = city;
				}

				zipcode = dt3.Rows[0]["ZIPCODE"].ToString().Trim();
			}



			if (state.Length > 0 && ADDRESS.Length > state.Length && ADDRESS.Substring(0, state.Length) == state) ADDRESS = ADDRESS.Substring(state.Length);
			if (city.Length > 0 && ADDRESS.Length > city.Length && ADDRESS.Substring(0, city.Length) == city) ADDRESS = ADDRESS.Substring(city.Length);
			if (district.Length > 0 && ADDRESS.Length > district.Length && ADDRESS.Substring(0, district.Length) == district) ADDRESS = ADDRESS.Substring(district.Length);

			addr2 = ADDRESS;

			return true;
		}


		// 지점(출발국가)의 화폐단위를 가져온다
		public static string GetCurrencyUnit(string rainbow_code)
		{
			string CURRENCY_UNIT = "";

			string sql1 = "select "
						+ "CURRENCY_UNIT"
						+ " from branch_info"
						+ string.Format(" where RAINBOWCODE='{0}'", rainbow_code)
						+ "";

			string err1 = "";
			DataTable dt1 = GetDataTableMySQL(sql1, out err1);
			if (dt1 != null && dt1.Rows.Count > 0)
			{
				CURRENCY_UNIT = dt1.Rows[0]["CURRENCY_UNIT"].ToString().Trim().ToUpper();
			}

			return CURRENCY_UNIT;
		}
	}
}
