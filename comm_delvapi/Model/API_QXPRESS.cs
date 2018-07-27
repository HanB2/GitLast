using comm_dbconn;
using comm_global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace comm_delvapi.Model
{
	class API_QXPRESS
	{
		public enum ApiType
		{
			TEST,
			REAL
		}


		ApiType m_TYPE = new ApiType();

		string m_URL = "";
		string m_AccountID = "";
		string m_APIKEY = "";


		public API_QXPRESS(
					ApiType type
					, string account_id = ""
					, string api_key = ""
					)
		{
			m_TYPE = type;

			if (m_TYPE == ApiType.TEST)
			{
				m_URL = "http://apitest.qxpress.asia/shipment";
				m_AccountID = "QxTestAPI_KR";
				m_APIKEY = "kA0RPUcvmRJkzyeDFIKx";
			}
			else if (m_TYPE == ApiType.REAL)
			{
				m_URL = "http://api.qxpress.asia/shipment";
				m_AccountID = account_id;
				m_APIKEY = api_key;
			}
		}










		// 주문정보 업로드
		public bool CreateOrder(ref QxpressModels model, ref string RET_MSG)
		{
			RET_MSG = "";

			// API 사용시 특수문자는 "+", "(", ")" 만 가능함
			string contents = Regex.Replace(model.contents, @"[^0-9a-zA-Z +()]", String.Empty);
			if (contents.Length > 50)
				contents = contents.Substring(0, 50);

			//string param_str = string.Format("apiKey={0}", m_APIKEY);  // API Key
			//param_str += string.Format("&accountId={0}", m_AccountID);  // Partner id
			//param_str += string.Format("&refOrderNo={0}", model.refOrderNo);  // Your reference delivery order number
			//param_str += string.Format("&svcType={0}", model.svcType);  // "RM" (It is fixed value)
			//param_str += string.Format("&rcptName={0}", System.Web.HttpUtility.UrlEncode(model.rcptName));  // Recipient name
			//param_str += string.Format("&rcptEmail={0}", System.Web.HttpUtility.UrlEncode(model.rcptEmail));  // Recipient email address
			//param_str += string.Format("&rcptCountry={0}", model.rcptCountry);  // Recipient country ("SG")
			//param_str += string.Format("&rcptAddr1={0}", System.Web.HttpUtility.UrlEncode(model.rcptAddr1));  // Recipient address 1 (State, City, Street) * JP - 도도부현 (都道府県)
			//param_str += string.Format("&rcptAddr2={0}", System.Web.HttpUtility.UrlEncode(model.rcptAddr2));  // Recipient address 2 (Bldg, Ste#) * JP - 시구정촌(市区町村) + 번지(番地)
			//param_str += string.Format("&rcptZipcode={0}", System.Web.HttpUtility.UrlEncode(model.rcptZipcode));  // Recipient postal code of address
			//param_str += string.Format("&rcptPhone={0}", System.Web.HttpUtility.UrlEncode(model.rcptPhone));  // Recipient telephone
			//param_str += string.Format("&rcptMobile={0}", System.Web.HttpUtility.UrlEncode(model.rcptMobile));  // Recipient mobile
			//param_str += string.Format("&rcptMemo={0}", System.Web.HttpUtility.UrlEncode(model.rcptMemo));  // Recipient request memo
			//param_str += string.Format("&contents={0}", System.Web.HttpUtility.UrlEncode(contents));  // Item name
			//param_str += string.Format("&quantity={0}", model.quantity);  // Quantity of Items (is not box qty.)
			//param_str += string.Format("&value={0}", model.value);  // Parcel amount value
			//param_str += string.Format("&currency={0}", model.currency);  // Currency of value ("SGD")

			string param = string.Format("apiKey={0}", m_APIKEY);  // API Key
			param += string.Format("&accountId={0}", m_AccountID);  // Partner id
			param += string.Format("&refOrderNo={0}", model.refOrderNo);  // Your reference delivery order number
			param += string.Format("&svcType={0}", model.svcType);  // "RM" (It is fixed value)
			param += string.Format("&rcptName={0}", model.rcptName);  // Recipient name
			param += string.Format("&rcptEmail={0}", model.rcptEmail);  // Recipient email address
			param += string.Format("&rcptCountry={0}", model.rcptCountry);  // Recipient country ("SG")
			param += string.Format("&rcptAddr1={0}", model.rcptAddr1);  // Recipient address 1 (State, City, Street) * JP - 도도부현 (都道府県)
			param += string.Format("&rcptAddr2={0}", model.rcptAddr2);  // Recipient address 2 (Bldg, Ste#) * JP - 시구정촌(市区町村) + 번지(番地)
			param += string.Format("&rcptZipcode={0}", model.rcptZipcode);  // Recipient postal code of address
			param += string.Format("&rcptPhone={0}", model.rcptPhone);  // Recipient telephone
			param += string.Format("&rcptMobile={0}", model.rcptMobile);  // Recipient mobile
			param += string.Format("&rcptMemo={0}", model.rcptMemo);  // Recipient request memo
			param += string.Format("&contents={0}", contents);  // Item name
			param += string.Format("&quantity={0}", model.quantity);  // Quantity of Items (is not box qty.)
			param += string.Format("&value={0}", model.value);  // Parcel amount value
			param += string.Format("&currency={0}", model.currency);  // Currency of value ("SGD")
			param += string.Format("&useFukuokaRoute={0}", model.useFukuokaRoute);  // 출발국 KR 도착국 JP 의 부산선편 유무(Y OR N)
			param += string.Format("&rcptNameFurigana={0}", model.rcptNameFurigana);  // 수취인 명 (후리가나) : 도착국 JP 일 경우 선택입력
			param += string.Format("&premiumService={0}", model.premiumService);  // Premium delivery service (only for KR-US route) : Y OR N

			//string add_url = "/CreateOrder.php";
			string add_url = "/RegistOrder.php";  // 2018-04-30 jsy : URL 변경됨
			string error = "";
			//string result = GetAPIResult(param_str, add_url, ref error);
			string result = GetAPIResult(param, add_url, ref error);

			// API 로그를 저장한다
			SetAPILOG(model.RAINBOWCODE, model.AGENTCODE, model.INVOICENO, add_url, param, result, error);

			if (result.Length == 0)
			{
				RET_MSG = error;
				model.ResultCode = -1;
				model.ResultMsg = error;
				return false;
			}

			if (result.Length < 5 || result.Substring(0, 5).ToUpper() != "<?XML")
			{
				// 에러인 경우는 xml 형식이 아닌 오류내용 string을 리턴한다
				// ex : An error occurred on the server.
				RET_MSG = result;
				model.ResultCode = -2;
				model.ResultMsg = result;
				return false;
			}



			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(result);

				// 실패
				//<?xml version="1.0" encoding="utf-8"?>
				//<OrderResult xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://tempuri.org/">
				//  <ResultCode>-360</ResultCode>
				//  <ResultMsg>Invalied rcptZipcode [JP] - rcptZipcode does not exist.</ResultMsg>
				//  <ShippingNo />
				//  <RefOrderNo />
				//</OrderResult>

				// 성공
				//<?xml version="1.0" encoding="utf-8"?>
				//<OrderResult xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://tempuri.org/">
				//  <ResultCode>0</ResultCode>
				//  <ResultMsg>SUCCESS</ResultMsg>
				//  <ShippingNo>QS100276424</ShippingNo>
				//  <RefOrderNo>FV160614012345</RefOrderNo>
				//</OrderResult>

				XmlNode StdResult = doc.ChildNodes[1];

				model.ResultCode = GlobalFunction.GetInt(StdResult["ResultCode"].InnerText);
				model.ResultMsg = StdResult["ResultMsg"].InnerText.Trim();
				model.ShippingNo = StdResult["ShippingNo"].InnerText.Trim();  // Q번호
				model.RefOrderNo = StdResult["RefOrderNo"].InnerText.Trim();  // 고객이 입력한 주문번호

				if (model.ResultCode != 0)
				{
					RET_MSG = string.Format("CreateOrder Error : {0} (code {1})", model.ResultMsg, model.ResultCode);
					return false;
				}
			}
			catch (Exception ex1)
			{
				RET_MSG = string.Format("Xml Parser Error : {0}", ex1.Message);
				return false;
			}

			return true;
		}


		public string GetAPIResult(string param, string url, ref string error_str)
		{
			error_str = "";

			string result = "";
			try
			{
				byte[] param_bytes = Encoding.UTF8.GetBytes(param);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_URL + url);
				request.Method = "POST";
				request.ContentLength = param_bytes.Length;
				request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

				Stream reqstream = request.GetRequestStream();
				reqstream.Write(param_bytes, 0, param_bytes.Length);
				reqstream.Close();

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream streamReceive = response.GetResponseStream();
				Encoding encoding = Encoding.UTF8;

				StreamReader streamReader = new StreamReader(streamReceive, encoding);
				result = streamReader.ReadToEnd();

				streamReader.Close();
				response.Close();
			}
			catch (WebException ex1)
			{
				error_str = ex1.Message;
				return "";
			}
			catch (Exception ex2)
			{
				error_str = ex2.Message;
				return "";
			}

			return result;
		}


		// API 로그를 기록한다
		public void SetAPILOG(string rainbow_code, string agent_code, string invoiceno, string url, string request, string response, string error_msg)
		{
			string URL = url;
			string REQUEST = request;
			string RESPONSE = response;
			string ERROR_MESSAGE = error_msg;

			REQUEST = REQUEST.Replace("'", ""); REQUEST = REQUEST.Replace("`", "");
			RESPONSE = RESPONSE.Replace("'", ""); RESPONSE = RESPONSE.Replace("`", "");
			ERROR_MESSAGE = ERROR_MESSAGE.Replace("'", ""); ERROR_MESSAGE = ERROR_MESSAGE.Replace("`", "");

			if (URL.Length > 100) URL = URL.Substring(0, 100);
			if (REQUEST.Length > 4000) REQUEST = REQUEST.Substring(0, 4000);
			if (RESPONSE.Length > 1000) RESPONSE = RESPONSE.Substring(0, 1000);
			if (ERROR_MESSAGE.Length > 1000) ERROR_MESSAGE = ERROR_MESSAGE.Substring(0, 1000);

			string sql1 = "insert into api_log("
						+ "RAINBOWCODE"
						+ ", AGENTCODE"
						+ ", INVOICENO"
						+ ", API_TYPE"
						+ ", API_URL"
						+ ", DATETIME_LOG"
						+ ", REQUEST"
						+ ", RESPONSE"
						+ ", ERROR_MESSAGE"
						+ ") values ("
						+ string.Format("'{0}'", rainbow_code)
						+ string.Format(", '{0}'", agent_code)
						+ string.Format(", '{0}'", invoiceno)
						+ string.Format(", 'JP_QXP'")
						+ string.Format(", '{0}'", URL)
						+ string.Format(", now()")
						+ string.Format(", '{0}'", REQUEST)
						+ string.Format(", '{0}'", RESPONSE)
						+ string.Format(", '{0}'", ERROR_MESSAGE)
						+ ")";

			string err1 = "";
			DatabaseConnection.ExcuteQueryMySQL(sql1, out err1);

			return;
		}










		// 트랙킹
		public bool Tracking(ref QxpressTrackingModels tracking, ref string RET_MSG)
		{
			RET_MSG = "";

			string param = string.Format("apiKey={0}", m_APIKEY);  // API Key
			param += string.Format("&trackingNo={0}", tracking.DELVNO);  // Tracking No

			string add_url = "/Tracking.php";
			string error = "";
			string result = GetAPIResult(param, add_url, ref error);

			// API 로그를 저장한다
			SetAPILOG(tracking.RAINBOWCODE, tracking.AGENTCODE, tracking.INVOICENO, add_url, param, result, error);

			if (result.Length == 0)
			{
				RET_MSG = error;
				return false;
			}

			if (result.Length < 5 || result.Substring(0, 5).ToUpper() != "<?XML")
			{
				// 에러인 경우는 xml 형식이 아닌 오류내용 string을 리턴한다
				// ex : An error occurred on the server.
				RET_MSG = result;
				return false;
			}



			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(result);

				// 실패
				//<?xml version="1.0" encoding="utf-8"?>
				//<StdCustomResultOfTrackingInfo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://tempuri.org/">
				//  <ResultCode>-100</ResultCode>
				//  <ResultMsg>Error Message</ResultMsg>
				//  <ResultObject />
				//</StdCustomResultOfTrackingInfo>

				// 성공
				//<?xml version="1.0" encoding="utf-8"?>
				//<StdCustomResultOfTrackingInfo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://tempuri.org/">
				//  <ResultCode>0</ResultCode>
				//  <ResultMsg>Success</ResultMsg>
				//  <ResultObject>
				//    <info>
				//      <shipping_no>517340968211</shipping_no>
				//      <ref_no>FVM180313010137</ref_no>
				//      <qs_no>QS100242697</qs_no>
				//      <recipient>瀧本修介</recipient>
				//    </info>
				//    <tracking_history>
				//      <History>
				//        <status>Delivered</status>
				//        <date>2018-02-02 21:42:59</date>
				//        <location>Sagawa</location>
				//      </History>
				//      <History>
				//        <status>Delivery Partner Assigned</status>
				//        <date>2018-02-01 12:24:14</date>
				//        <location>Sagawa</location>
				//      </History>
				//      <History>
				//        <status>Arrived at the Distribution Center</status>
				//        <date>2018-02-01 12:23:29</date>
				//        <location>Qxpress JP Tokyo</location>
				//      </History>
				//      <History>
				//        <status>Starting Delivery to the Destination Country</status>
				//        <date>2018-01-30 17:45:48</date>
				//        <location />
				//      </History>
				//      <History>
				//        <status>Starting Delivery to the Destination Country</status>
				//        <date>2018-01-30 17:40:47</date>
				//        <location />
				//      </History>
				//      <History>
				//        <status>Arrived at the Processing Facility</status>
				//        <date>2018-01-30 14:22:11</date>
				//        <location>Qxpress CN Guangzhou</location>
				//      </History>
				//    </tracking_history>
				//  </ResultObject>
				//</StdCustomResultOfTrackingInfo>

				XmlNode TrackingInfo = doc.ChildNodes[1];

				tracking.ResultCode = GlobalFunction.GetInt(TrackingInfo["ResultCode"].InnerText);
				tracking.ResultMsg = TrackingInfo["ResultMsg"].InnerText;

				if (tracking.ResultCode != 0)
				{
					RET_MSG = string.Format("Tracking Error : {0} (code {1})", tracking.ResultMsg, tracking.ResultCode);
					return false;
				}

				XmlElement info = TrackingInfo["ResultObject"]["info"];
				tracking.shipping_no = info["shipping_no"].InnerText.Trim();  // 일본 택배번호
				tracking.ref_no = info["ref_no"].InnerText.Trim();  // 주문번호
				tracking.qs_no = info["qs_no"].InnerText.Trim();  // Qxpress 번호
				tracking.recipient = info["recipient"].InnerText.Trim();  // 수취인 이름

				XmlNodeList tracking_history = TrackingInfo["ResultObject"]["tracking_history"].ChildNodes;
				string old_status = "";
				for (int i = 0; i < tracking_history.Count; i++)
				{
					XmlNode History = tracking_history[i];

					QxpressTrackingDetailModels Detail = new QxpressTrackingDetailModels();
					Detail.status = History["status"].InnerText;
					Detail.date = History["date"].InnerText;
					Detail.location = History["location"].InnerText;

					//if (Detail.status.IndexOf("Arrived at the Processing Facility", StringComparison.OrdinalIgnoreCase) == 0)
					//    continue;

					//if (old_status == Detail.status)
					//    continue;

					tracking.DETAIL.Insert(0, Detail);

					old_status = Detail.status;
				}
			}
			catch (Exception ex1)
			{
				RET_MSG = string.Format("Xml Parser Error : {0}", ex1.Message);
				return false;
			}

			return true;
		}
	}
}
