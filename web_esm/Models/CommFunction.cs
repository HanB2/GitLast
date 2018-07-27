using comm_dbconn;
using comm_global;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models
{
	//중복 호출 또는 하드코딩으로 지정 하는 리스트 변수들 관리
	public class CommFunction : DatabaseConnection
	{


		//관리자 ID 리스트 가져오기
		public List<schTypeArray> GetEsmUserSelectBox()
		{
			string errorStr = "";

			string listQuery = " SELECT EMAIL FROM esm_user  ORDER BY SEQNO DESC";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["EMAIL"].ToString().Trim(), opt_key = listDt.Rows[i]["EMAIL"].ToString().Trim() });
				}
			}

			return model;
		}
		
		//나라 전체 가져오기
		public List<schTypeArray> GetCommNationSelectBox()
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , NATIONNO, NATIONNAME_ko_KR FROM comm_nation  ORDER BY SEQNO ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["NATIONNAME_ko_KR"].ToString().Trim(), opt_key = listDt.Rows[i]["NATIONNO"].ToString().Trim() });
				}
			}

			return model;
		}

		//배송 가능 국가 가져오기
		public List<schTypeArray> GetConfShippingCountrySelectBox()
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , NATION_CODE, NATION_NAME FROM conf_shipping_country  ORDER BY SEQNO ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["NATIONNAME_ko_KR"].ToString().Trim(), opt_key = listDt.Rows[i]["NATIONNO"].ToString().Trim() });
				}
			}

			return model;
		}


		//화폐 단위
		public List<schTypeArray> GetConfCurrencySelectBox()
		{
			string errorStr = "";
			string listQuery = " SELECT SEQNO , CURRENCY_UNIT FROM conf_currency  ORDER BY SEQNO ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["CURRENCY_UNIT"].ToString().Trim(), opt_key = listDt.Rows[i]["CURRENCY_UNIT"].ToString().Trim() });
				}
			}

			return model;
		}


		//무게 단위
		public List<schTypeArray> GetWeightSelectBox()
		{
			List<schTypeArray> model = new List<schTypeArray> {
				new schTypeArray { opt_key = "KG", opt_value = "KG" },
				new schTypeArray { opt_key = "LB", opt_value = "LB" }
			};
			return model;
		}

		//액션 리스트 (권한 관리에 사용)
		public List<schTypeArray> GetGradeList()
		{
			List<schTypeArray> model = new List<schTypeArray> {
				new schTypeArray {      opt_key = "BaseCurrency",     opt_value = "통화 관리"              },
				new schTypeArray {      opt_key = "BaseNation",     opt_value = "배송 가능 국가 관리"              },
				new schTypeArray {      opt_key = "BaseAirport",     opt_value = "공항 관리"              },
				new schTypeArray {      opt_key = "BaseLocal",     opt_value = "현지 배송업체 설정"              },
				new schTypeArray {      opt_key = "BaseOutPutType",     opt_value = "출고타입 설정"              },
				//ESE STATION
				new schTypeArray {      opt_key = "EstInfo",     opt_value = "EST 정보관리"              },
				//new schTypeArray {      opt_key = "EstGrade",     opt_value = "EST 계정 등급 관리"              },
				//new schTypeArray {      opt_key = "EstAccount",     opt_value = "EST 계정 관리"              },
				new schTypeArray {      opt_key = "EstInOutStat",     opt_value = "EST 출고 현황"              },
				//ESE SENDER
				new schTypeArray {      opt_key = "EseInfo",     opt_value = "ESE 정보관리"              },
				//통관상품 관리
				new schTypeArray {      opt_key = "ProdList",     opt_value = "통관 상품 관리"              },
				//MAR
				new schTypeArray {      opt_key = "MarInReq",     opt_value = "MAR 충전요청"              },
				new schTypeArray {      opt_key = "MarOutEst",     opt_value = "MAR 출금요청(EST)"              },
				new schTypeArray {      opt_key = "MarOutEse",     opt_value = "MAR 출금요청(ESE)"              },
				new schTypeArray {      opt_key = "MarInOut",     opt_value = "MAR 입출금 현황"              },
				//CS
				new schTypeArray {      opt_key = "CsNotice",     opt_value = "CsNotice"              },
				new schTypeArray {      opt_key = "CsQna",     opt_value = "CsQna"              },
				//설정
				new schTypeArray {      opt_key = "SettingEmail",     opt_value = "메일 서버 설정"              },
				//ESM 관리자 설정
				new schTypeArray {      opt_key = "EsmGrade",     opt_value = "ESM 계정 그룹 관리"              },
				new schTypeArray {      opt_key = "EsmGradeView",     opt_value = "ESM 계정 관리"              },
				new schTypeArray {      opt_key = "EsmAccount",     opt_value = "로그인 이력 조회"              },

			};
			return model;
		}

		//환율 가져오기
		public double GetExchangeRate_ExRateOrg(string from_currency, string to_currency, int basic_unit = 1)
		{
			double rate = 0.0;

			string FromCurrency = Regex.Replace(from_currency.Trim().ToUpper(), @"[^A-Z]", String.Empty);
			string ToCurrency = Regex.Replace(to_currency.Trim().ToUpper(), @"[^A-Z]", String.Empty);
			if (FromCurrency.Length != 3 || ToCurrency.Length != 3)
			{
				return rate;
			}

			if (FromCurrency == "KRW")
			{
				string tmp = FromCurrency;
				FromCurrency = ToCurrency;
				ToCurrency = tmp;
			}

			string result = "";
			try
			{
				string url = string.Format("https://www.exchange-rates.org/converter/{0}/{1}/{2}", FromCurrency, ToCurrency, basic_unit);
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Method = "GET";
				req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";

				using (var stream = new StreamReader(req.GetResponse().GetResponseStream()))
				{
					result = stream.ReadToEnd();
				}
			}
			catch (Exception ex1)
			{
				string error = ex1.Message;
				return rate;
			}
			
			string find_string = "<span id=\"ctl00_M_lblToAmount\">";
			int index1 = result.IndexOf(find_string, StringComparison.OrdinalIgnoreCase);
			int index2 = result.IndexOf("<", (index1 + 1), StringComparison.OrdinalIgnoreCase);
			if (index1 > 0 && index2 > 0)
			{
				string EX_RATE = result.Substring((index1 + find_string.Length), (index2 - index1 - find_string.Length));
				EX_RATE = EX_RATE.Replace(",", "");

				rate = GetDouble(EX_RATE, 4);
			}

			return rate;
		}

		// string을 double로 변환하여 리턴한다
		public static double GetDouble(string str, int sosu)
		{
			double value = 0.0;
			if (str == null)
				return value;

			double.TryParse(str, out value);
			value = Math.Round(value, sosu);

			return value;
		}






	
		


		//그룹ID(MSH)
		public List<schTypeArray> GroupIdSelectBox()
		{
			string errorStr = "";

			string listQuery = " SELECT GROUP_ID,GROUP_NAME FROM esm_group ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["GROUP_NAME"].ToString().Trim(), opt_key = listDt.Rows[i]["GROUP_ID"].ToString().Trim() });
				}
			}

			return model;
		}

	}
}