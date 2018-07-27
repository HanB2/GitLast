using comm_dbconn;
using comm_global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace comm_delvapi.Model
{
	// 중국 YTO 내륙 API 연동
	// 1. 주문정보 업로드
	// 2. 트랙킹 데이터 가져오기
	// 3. 트랙킹 데이터 설정하기
	public class API_YTO_LOCAL : DatabaseConnection
	{
		public enum YTO_APIType
		{
			TEST,
			REAL
		}


		YTO_APIType m_TYPE = new YTO_APIType();

		// 주문정보 업로드
		public string m_ORDERADD_URL = "";
		public string m_CLIENT_ID = "";
		public string m_PARTNER_ID = "";
		public string m_ORDERTYPE = "1";  // 1=일반, 5=YTO GLOBAL(중국내륙지점)

		// 트랙킹 데이터 가져오기
		public string m_TRACKING_URL = "";
		public string m_USER_ID = "";
		public string m_APP_KEY = "";
		public string m_SECRET_KEY = "";

		// 트랙킹 데이터 설정하기
		public string m_TRACKING_INFO_URL = "";
		public string m_CLIENT_ID_TRACK_INFO = "";
		public string m_PARTNER_ID_TRACK_INFO = "";


		public API_YTO_LOCAL(
					YTO_APIType type
					, string client_id = ""
					, string partner_id = ""
					, string client_id_track_info = ""
					, string partner_id_track_info = ""
					, string order_type = "1"  // 1=일반, 5=YTO GLOBAL(중국내륙지점)
					)
		{
			m_TYPE = type;

			if (m_TYPE == YTO_APIType.TEST)
			{
				// 주문정보 업로드
				m_ORDERADD_URL = "http://58.32.246.71:8000/CommonOrderModeBPlusServlet.action";
				m_CLIENT_ID = "K21000119";
				m_PARTNER_ID = "u2Z1F7Fh";
				m_ORDERTYPE = order_type;
				if (m_ORDERTYPE == "5")  // 5=YTO GLOBAL(중국내륙지점)
					m_ORDERADD_URL = "http://service.yto56.net.cn/overseaOrderServlet";

				// 트랙킹 데이터 가져오기
				m_TRACKING_URL = "http://MarketingInterface.yto.net.cn";
				m_USER_ID = "";
				m_APP_KEY = "";
				m_SECRET_KEY = "";

				// 트랙킹 데이터 설정하기
				m_TRACKING_INFO_URL = "http://180.153.190.90/globalunion/outcall/trackinginfo";
				m_CLIENT_ID_TRACK_INFO = "TESTSTD";
				m_PARTNER_ID_TRACK_INFO = "TESTSTD";
			}
			else if (m_TYPE == YTO_APIType.REAL)
			{
				// 주문정보 업로드
				m_ORDERADD_URL = "http://service.yto56.net.cn/CommonOrderModeBPlusServlet.action";
				m_CLIENT_ID = client_id;
				m_PARTNER_ID = partner_id;
				m_ORDERTYPE = order_type;
				if (m_ORDERTYPE == "5")  // 5=YTO GLOBAL(중국내륙지점)
					m_ORDERADD_URL = "http://service.yto56.net.cn/overseaOrderServlet";

				// 트랙킹 데이터 가져오기
				m_TRACKING_URL = "http://MarketingInterface.yto.net.cn";
				m_USER_ID = "";
				m_APP_KEY = "";
				m_SECRET_KEY = "";

				// 트랙킹 데이터 설정하기
				m_TRACKING_INFO_URL = "http://lmtest.yto.net.cn/globalunion/outcall/trackinginfo";
				m_CLIENT_ID_TRACK_INFO = client_id_track_info;
				m_PARTNER_ID_TRACK_INFO = partner_id_track_info;
			}
		}


		// 주문등록
		public bool OrderAdd(YTOLocalModel model, ref YTOLocalResultModel yto_result, ref string RET_MSG)
		{
			yto_result = new YTOLocalResultModel();
			RET_MSG = "";

			if (m_CLIENT_ID.Length == 0 || m_PARTNER_ID.Length == 0)
			{
				RET_MSG = "환경설정에서 API 연동에 필요한 값을 먼저 설정해야 합니다.";
				return false;
			}

			XmlDocument xmldoc = new XmlDocument();

			XmlElement RequestOrder = xmldoc.CreateElement("RequestOrder");

			XmlElement clientID = xmldoc.CreateElement("clientID");  // customerId 와 동일
			clientID.InnerText = m_CLIENT_ID;
			RequestOrder.AppendChild(clientID);

			XmlElement logisticProviderID = xmldoc.CreateElement("logisticProviderID");  // 물류회사ID ==> YTO 고정값
			logisticProviderID.InnerText = model.logisticProviderID;
			RequestOrder.AppendChild(logisticProviderID);

			XmlElement customerId = xmldoc.CreateElement("customerId");  // clientID 와 동일
			customerId.InnerText = m_CLIENT_ID;
			RequestOrder.AppendChild(customerId);

			XmlElement txLogisticID = xmldoc.CreateElement("txLogisticID");  // 주문번호(중복되지 않게)
			txLogisticID.InnerText = model.txLogisticID;
			RequestOrder.AppendChild(txLogisticID);

			XmlElement tradeNo = xmldoc.CreateElement("tradeNo");  // 비즈니스 트랜잭션 번호 (선택 사항)
			tradeNo.InnerText = model.tradeNo;
			RequestOrder.AppendChild(tradeNo);

			XmlElement totalServiceFee = xmldoc.CreateElement("totalServiceFee");  // 보험 값 = insuranceValue * 상품 수량 (기본값은 0.0)
			totalServiceFee.InnerText = model.totalServiceFee.ToString("0.00");
			RequestOrder.AppendChild(totalServiceFee);

			XmlElement codSplitFee = xmldoc.CreateElement("codSplitFee");  // 물류 회사 포인트 [COD] 실행 (일시적으로 사용되지 않음, 기본값은 0.0)
			codSplitFee.InnerText = model.codSplitFee.ToString("0.00");
			RequestOrder.AppendChild(codSplitFee);

			XmlElement orderType = xmldoc.CreateElement("orderType");  // 주문 유형 (0-COD, 1- 정상 주문, 2- 휴대용 주문, 3- 반환 주문, 4- 지불)
																	   //orderType.InnerText = model.orderType.ToString();
			orderType.InnerText = m_ORDERTYPE;
			RequestOrder.AppendChild(orderType);

			XmlElement serviceType = xmldoc.CreateElement("serviceType");  // 서비스 유형 (1 - 집 수신, 2 - 다음날 4 - 아침 8 - 날짜, 0 - 연락처). 기본값은 0입니다.
			serviceType.InnerText = model.serviceType.ToString();
			RequestOrder.AppendChild(serviceType);

			XmlElement flag = xmldoc.CreateElement("flag");  // 주문 플래그, 기본값은 0, 의미가 없습니다.
			flag.InnerText = model.flag.ToString();
			RequestOrder.AppendChild(flag);

			XmlElement mailNo = xmldoc.CreateElement("mailNo");  // 물류청구서 번호
			mailNo.InnerText = model.mailNo;
			RequestOrder.AppendChild(mailNo);

			XmlElement type = xmldoc.CreateElement("type");  // 주문 유형 (이 필드는 이전 버전과의 호환성을 위해 예약되어 있습니다)
			type.InnerText = model.type.ToString();
			RequestOrder.AppendChild(type);



			// sender
			XmlElement sender = xmldoc.CreateElement("sender");

			XmlElement sender_name = xmldoc.CreateElement("name");  // 이름
			sender_name.InnerText = model.sender_name;
			sender.AppendChild(sender_name);

			XmlElement sender_postCode = xmldoc.CreateElement("postCode");  // 우편번호
			sender_postCode.InnerText = model.sender_postCode;
			sender.AppendChild(sender_postCode);

			XmlElement sender_phone = xmldoc.CreateElement("phone");  // 전화번호
			sender_phone.InnerText = model.sender_phone;
			sender.AppendChild(sender_phone);

			XmlElement sender_mobile = xmldoc.CreateElement("mobile");  // 휴대폰번호
			sender_mobile.InnerText = model.sender_mobile;
			sender.AppendChild(sender_mobile);

			XmlElement sender_prov = xmldoc.CreateElement("prov");  // 성
			sender_prov.InnerText = model.sender_prov;
			sender.AppendChild(sender_prov);

			XmlElement sender_city = xmldoc.CreateElement("city");  // 시, 구
			sender_city.InnerText = model.sender_city;
			sender.AppendChild(sender_city);

			XmlElement sender_address = xmldoc.CreateElement("address");  // 세부주소
			sender_address.InnerText = model.sender_address;
			sender.AppendChild(sender_address);

			RequestOrder.AppendChild(sender);



			// receiver
			XmlElement receiver = xmldoc.CreateElement("receiver");

			XmlElement receiver_name = xmldoc.CreateElement("name");  // 이름
			receiver_name.InnerText = model.receiver_name;
			receiver.AppendChild(receiver_name);

			XmlElement receiver_postCode = xmldoc.CreateElement("postCode");  // 우편번호
			receiver_postCode.InnerText = model.receiver_postCode;
			receiver.AppendChild(receiver_postCode);

			XmlElement receiver_phone = xmldoc.CreateElement("phone");  // 전화번호
			receiver_phone.InnerText = model.receiver_phone;
			receiver.AppendChild(receiver_phone);

			XmlElement receiver_mobile = xmldoc.CreateElement("mobile");  // 휴대폰번호
			receiver_mobile.InnerText = model.receiver_mobile;
			receiver.AppendChild(receiver_mobile);

			XmlElement receiver_prov = xmldoc.CreateElement("prov");  // 성
			receiver_prov.InnerText = model.receiver_prov;
			receiver.AppendChild(receiver_prov);

			XmlElement receiver_city = xmldoc.CreateElement("city");  // 시, 구
			receiver_city.InnerText = model.receiver_city;
			receiver.AppendChild(receiver_city);

			XmlElement receiver_address = xmldoc.CreateElement("address");  // 세부주소
			receiver_address.InnerText = model.receiver_address;
			receiver.AppendChild(receiver_address);

			RequestOrder.AppendChild(receiver);



			XmlElement sendStartTime = xmldoc.CreateElement("sendStartTime");  // yyyy-MM-dd HH:mm:ss
			sendStartTime.InnerText = model.sendStartTime;
			RequestOrder.AppendChild(sendStartTime);

			XmlElement sendEndTime = xmldoc.CreateElement("sendEndTime");  // yyyy-MM-dd HH:mm:ss
			sendEndTime.InnerText = model.sendEndTime;
			RequestOrder.AppendChild(sendEndTime);

			XmlElement goodsValue = xmldoc.CreateElement("goodsValue");  // 상품 및 배송비는 포함하지만 서비스 요금은 포함되지 않은 제품 금액
			goodsValue.InnerText = model.goodsValue.ToString("0.00");
			RequestOrder.AppendChild(goodsValue);

			XmlElement totalValue = xmldoc.CreateElement("totalValue");  // goodsValue + 총 서비스 수수료
			totalValue.InnerText = model.totalValue.ToString("0.00");
			RequestOrder.AppendChild(totalValue);

			XmlElement agencyFund = xmldoc.CreateElement("agencyFund");  // 수금액이 수금 순서 인 경우 수거 금액은 0보다 커야하며 비 수거는 0.0으로 설정되어야합니다.
			agencyFund.InnerText = model.agencyFund.ToString("0.00");
			RequestOrder.AppendChild(agencyFund);

			XmlElement itemsValue = xmldoc.CreateElement("itemsValue");  // 상품가격
			itemsValue.InnerText = model.itemsValue.ToString("0.00");
			RequestOrder.AppendChild(itemsValue);

			XmlElement itemsWeight = xmldoc.CreateElement("itemsWeight");  // 총중량
			itemsWeight.InnerText = model.itemsWeight.ToString("0.00");
			RequestOrder.AppendChild(itemsWeight);



			// items
			XmlElement items = xmldoc.CreateElement("items");

			for (int i = 0; i < model.ITEMS.Count; i++)
			{
				XmlElement item = xmldoc.CreateElement("item");

				XmlElement itemName = xmldoc.CreateElement("itemName");  // 상품명
				itemName.InnerText = model.ITEMS[i].itemName;
				item.AppendChild(itemName);

				XmlElement number = xmldoc.CreateElement("number");  // 상품갯수
				number.InnerText = model.ITEMS[i].number.ToString();
				item.AppendChild(number);

				XmlElement itemValue = xmldoc.CreateElement("itemValue");  // 단가 (2 자리 소수점)
				itemValue.InnerText = model.ITEMS[i].itemValue.ToString("0.00");
				item.AppendChild(itemValue);

				items.AppendChild(item);
			}



			XmlElement insuranceValue = xmldoc.CreateElement("insuranceValue");  // 보험 금액의 금액 (보험 금액은 제품의 가치입니다 (100보다 크거나 같음 3w 미만), 기본값은 0.0입니다)
			insuranceValue.InnerText = model.insuranceValue.ToString("0.00");
			RequestOrder.AppendChild(insuranceValue);

			XmlElement special = xmldoc.CreateElement("special");  // 제품 유형 (필드 유지, 일시적으로 보유하지 않음)
			special.InnerText = model.special.ToString();
			RequestOrder.AppendChild(special);

			XmlElement remark = xmldoc.CreateElement("remark");  // 비고
			remark.InnerText = model.remark;
			RequestOrder.AppendChild(remark);





			string xml_str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + RequestOrder.OuterXml;
			string error = "";
			string result = GetAPIResult(xml_str, ref error);

			// YTO API 로그를 저장한다
			SetYTOAPILOG(model.RAINBOWCODE, model.AGENTCODE, model.INVOICENO, "OrderAdd", xml_str, result, error);

			if (result.Length == 0)
			{
				RET_MSG = error;
				return false;
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(result);

				//실패 1
				//<Response>
				//  <txLogisticID></txLogisticID>
				//  <logisticProviderID>YTO</logisticProviderID>
				//  <code>S02</code>
				//  <success>false</success>
				//  <reason>S02,illegal digital signature, please check your secret key：非法的数字签名，请检查您的密钥</reason>
				//</Response>

				//실패 2
				//<Response>
				//  <txLogisticID>MIR20171220-1</txLogisticID>
				//  <logisticProviderID>YTO</logisticProviderID>
				//  <code>S01</code>
				//  <success>false</success>
				//  <reason>mailNo must be empty：运单号mailNo必须为空</reason>
				//</Response>

				//성공
				//<Response>
				//  <logisticProviderID>YTO</logisticProviderID>
				//  <txLogisticID>MIR20171220-1</txLogisticID>
				//  <clientID>K21000119</clientID>
				//  <mailNo>899932564524</mailNo>
				//  <distributeInfo>
				//    <shortAddress>302-046-206</shortAddress>
				//    <consigneeBranchCode>210201</consigneeBranchCode>
				//    <packageCenterCode>210901</packageCenterCode>
				//    <packageCenterName>上海转运中心</packageCenterName>
				//  </distributeInfo>
				//  <code>200</code>
				//  <success>true</success>
				//</Response>

				XmlNode ret_response = doc.ChildNodes[0];

				for (int i = 0; i < ret_response.ChildNodes.Count; i++)
				{
					XmlNode node = ret_response.ChildNodes[i];

					if (string.Compare(node.Name, "logisticProviderID", StringComparison.OrdinalIgnoreCase) == 0)
					{
						yto_result.logisticProviderID = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "txLogisticID", StringComparison.OrdinalIgnoreCase) == 0)
					{
						yto_result.txLogisticID = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "clientID", StringComparison.OrdinalIgnoreCase) == 0)
					{
						yto_result.clientID = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "code", StringComparison.OrdinalIgnoreCase) == 0)  // error code
					{
						yto_result.code = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "success", StringComparison.OrdinalIgnoreCase) == 0)  // result
					{
						yto_result.success = GlobalFunction.GetBool(node.InnerText.Trim());
						continue;
					}
					else if (string.Compare(node.Name, "reason", StringComparison.OrdinalIgnoreCase) == 0)  // error desc
					{
						yto_result.reason = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "mailNo", StringComparison.OrdinalIgnoreCase) == 0)  // YTO번호
					{
						yto_result.mailNo = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "distributeInfo", StringComparison.OrdinalIgnoreCase) == 0)
					{
						for (int k = 0; k < node.ChildNodes.Count; k++)
						{
							XmlNode node2 = node.ChildNodes[k];

							if (string.Compare(node2.Name, "shortAddress", StringComparison.OrdinalIgnoreCase) == 0) yto_result.shortAddress = node2.InnerText.Trim();
							else if (string.Compare(node2.Name, "consigneeBranchCode", StringComparison.OrdinalIgnoreCase) == 0) yto_result.consigneeBranchCode = node2.InnerText.Trim();
							else if (string.Compare(node2.Name, "packageCenterCode", StringComparison.OrdinalIgnoreCase) == 0) yto_result.packageCenterCode = node2.InnerText.Trim();
							else if (string.Compare(node2.Name, "packageCenterName", StringComparison.OrdinalIgnoreCase) == 0) yto_result.packageCenterName = node2.InnerText.Trim();
						}
					}
				}
			}
			catch (Exception ex1)
			{
				RET_MSG = string.Format("Xml Parser Error : {0}", ex1.Message);
				return false;
			}



			if (!yto_result.success)  // api error
			{
				RET_MSG = string.Format("OrderAdd Error : {0} (code {1})", yto_result.reason, yto_result.code);
				return false;
			}

			return true;
		}


		public string GetAPIResult(string message, ref string error_str)
		{
			error_str = "";

			string sign_plane = message + m_PARTNER_ID;

			MD5 md5 = MD5.Create();
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sign_plane);
			byte[] newBuffer = md5.ComputeHash(bytes, 0, bytes.Length);
			string sign_encrypt = Convert.ToBase64String(newBuffer);



			string result = "";
			try
			{
				// parameter의 특수문자가 잘못 전달될수 있으므로 UrlEncode 해준다
				string param = string.Format("logistics_interface={0}", System.Web.HttpUtility.UrlEncode(message));
				param += string.Format("&data_digest={0}", System.Web.HttpUtility.UrlEncode(sign_encrypt));
				param += string.Format("&type=offline");
				param += string.Format("&clientId={0}", m_CLIENT_ID);

				byte[] sign_bytes = Encoding.UTF8.GetBytes(param);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_ORDERADD_URL);
				request.Method = "POST";
				request.ContentLength = sign_bytes.Length;
				request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

				Stream reqstream = request.GetRequestStream();
				reqstream.Write(sign_bytes, 0, sign_bytes.Length);
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


		// YTO API 로그를 기록한다
		public void SetYTOAPILOG(string rainbow_code, string agent_code, string invoiceno, string url, string request, string response, string error_msg)
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
						+ string.Format(", 'CN_YTO_LOCAL'")
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










		// 배송조회
		public bool Tracking(ref YTOLocalTrackingModel tracking, ref string RET_MSG)
		{
			RET_MSG = "";

			if (m_USER_ID.Length == 0 || m_APP_KEY.Length == 0 || m_SECRET_KEY.Length == 0)
			{
				RET_MSG = "환경설정에서 API 연동에 필요한 값을 먼저 설정해야 합니다.";
				return false;
			}

			XmlDocument xmldoc = new XmlDocument();

			XmlElement ufinterface = xmldoc.CreateElement("ufinterface");
			XmlElement Result = xmldoc.CreateElement("Result");
			XmlElement WaybillCode = xmldoc.CreateElement("WaybillCode");

			XmlElement Number = xmldoc.CreateElement("Number");
			Number.InnerText = tracking.WaybillNo;

			WaybillCode.AppendChild(Number);
			Result.AppendChild(WaybillCode);
			ufinterface.AppendChild(Result);



			string xml_str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + ufinterface.OuterXml;
			string method = "yto.Marketing.WaybillTrace";
			string error = "";
			string result = GetAPIMethodResult(xml_str, method, ref error);

			// YTO API 로그를 저장한다
			SetYTOAPILOG(tracking.RAINBOWCODE, "", tracking.INVOICENO, "Tracking", xml_str, result, error);

			if (result.Length == 0)
			{
				RET_MSG = error;
				return false;
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(result);

				// 실패 1
				//<Response>
				//  <success>false</success>
				//  <reason>签名不一致</reason>
				//</Response>

				// 실패 2
				//<Response>
				//  <success>false</success>
				//  <reason>请核对您的单号是否正确</reason>
				//</Response>

				// 성공
				//<?xml version="1.0"?>
				//<ufinterface>
				//  <Result>
				//    <WaybillProcessInfo>
				//      <Waybill_No>560624585061</Waybill_No>
				//      <Upload_Time>2017-4-19 19:25:22</Upload_Time>
				//      <ProcessInfo>【新疆乌鲁木齐市公司】 已收件</ProcessInfo>
				//    </WaybillProcessInfo>
				//    <WaybillProcessInfo>
				//      <Waybill_No>560624585061</Waybill_No>
				//      <Upload_Time>2017-4-19 19:26:40</Upload_Time>
				//      <ProcessInfo>【新疆乌鲁木齐市公司】 已收入</ProcessInfo>
				//    </WaybillProcessInfo>
				//    <WaybillProcessInfo>
				//      <Waybill_No>560624585061</Waybill_No>
				//      <Upload_Time>2017-4-20 6:24:27</Upload_Time>
				//      <ProcessInfo>【新疆乌鲁木齐市公司】 已发出 下一站 【新疆乌鲁木齐市沙依巴克区公司】</ProcessInfo>
				//    </WaybillProcessInfo>
				//    <WaybillProcessInfo>
				//      <Waybill_No>560624585061</Waybill_No>
				//      <Upload_Time>2017-4-20 10:08:24</Upload_Time>
				//      <ProcessInfo>【新疆乌鲁木齐市沙依巴克区公司】 已收入</ProcessInfo>
				//    </WaybillProcessInfo>
				//    <WaybillProcessInfo>
				//      <Waybill_No>560624585061</Waybill_No>
				//      <Upload_Time>2017-4-20 10:43:31</Upload_Time>
				//      <ProcessInfo>【新疆乌鲁木齐市沙依巴克区公司】 派件人 :尚文涛 派件中 派件员电话18699171457</ProcessInfo>
				//    </WaybillProcessInfo>
				//    <WaybillProcessInfo>
				//      <Waybill_No>560624585061</Waybill_No>
				//      <Upload_Time>2017-4-20 20:02:20</Upload_Time>
				//      <ProcessInfo>客户 签收人 :null 已签收 感谢使用圆通速递，期待再次为您服务</ProcessInfo>
				//    </WaybillProcessInfo>
				//  </Result>
				//</ufinterface>

				XmlNode ret_response = doc.ChildNodes[0];

				if (string.Compare(ret_response.Name, "Response", StringComparison.OrdinalIgnoreCase) == 0)
				{
					tracking.success = GlobalFunction.GetBool(ret_response["success"].InnerText.Trim());
					tracking.reason = ret_response["reason"].InnerText.Trim();
				}
				else// if (string.Compare(ret_response.Name, "ufinterface", StringComparison.OrdinalIgnoreCase) == 0)
				{
					XmlNode ret_ufinterface = doc.ChildNodes[1];
					XmlElement ret_result = ret_ufinterface["Result"];

					for (int i = 0; i < ret_result.ChildNodes.Count; i++)
					{
						XmlNode WaybillProcessInfo = ret_result.ChildNodes[i];

						YTOLocalTrackingDetailModel DETAIL = new YTOLocalTrackingDetailModel();
						DETAIL.Upload_Time = WaybillProcessInfo["Upload_Time"].InnerText.Trim();
						DETAIL.ProcessInfo = WaybillProcessInfo["ProcessInfo"].InnerText.Trim();
						tracking.DETAIL.Add(DETAIL);
					}

					tracking.success = true;
				}
			}
			catch (Exception ex1)
			{
				RET_MSG = string.Format("Xml Parser Error : {0}", ex1.Message);
				return false;
			}



			if (!tracking.success)
			{
				RET_MSG = string.Format("Tracking Error : {0}", tracking.reason);
				return false;
			}

			return true;
		}


		public string GetAPIMethodResult(string message, string method, ref string error_str)
		{
			error_str = "";

			DateTime TIMESTAMP = DateTime.UtcNow.AddHours(8);  // 중국시간 설정

			string sign_plane = m_SECRET_KEY;
			sign_plane += string.Format("app_key{0}", m_APP_KEY);
			sign_plane += string.Format("format{0}", "XML");
			sign_plane += string.Format("method{0}", method);
			sign_plane += string.Format("timestamp{0}", TIMESTAMP.ToString("yyyy-MM-dd HH:mm:ss"));
			sign_plane += string.Format("user_id{0}", m_USER_ID);
			sign_plane += string.Format("v{0}", "1.01");

			MD5 md5 = MD5.Create();
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sign_plane);
			byte[] newBuffer = md5.ComputeHash(bytes, 0, bytes.Length);

			// MD5 32bit
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < newBuffer.Length; i++)
			{
				sb.Append(newBuffer[i].ToString("X2").ToLower());
			}
			string sign_encrypt = sb.ToString().ToUpper();



			string result = "";
			try
			{
				string param = string.Format("sign={0}", sign_encrypt);
				param += string.Format("&app_key={0}", m_APP_KEY);
				param += string.Format("&format={0}", "XML");
				param += string.Format("&method={0}", method);
				param += string.Format("&timestamp={0}", TIMESTAMP.ToString("yyyy-MM-dd HH:mm:ss"));
				param += string.Format("&user_id={0}", m_USER_ID);
				param += string.Format("&v={0}", "1.01");
				param += string.Format("&param={0}", message);

				byte[] sign_bytes = Encoding.UTF8.GetBytes(param);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_TRACKING_URL);
				request.Method = "POST";
				request.ContentLength = sign_bytes.Length;
				request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

				Stream reqstream = request.GetRequestStream();
				reqstream.Write(sign_bytes, 0, sign_bytes.Length);
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










		// 배송정보 추가하기 테스트
		public string SetTrackingInfo_test()
		{
			string apiUrl = "http://180.153.190.90/globalunion/outcall/trackinginfo";
			string clientId = "TESTSTD";
			string partnerId = "TESTSTD";

			string xmlBuilder = "<?xml version='1.0' encoding='UTF-8'?><Message><Header><SeqNo>MIRASDFGHJKL20180105</SeqNo><SendTimeStamp>2018-01-05 13:46:19:932</SendTimeStamp></Header><RequestTrackingInfo>";
			xmlBuilder += "<RecordCount>1000</RecordCount>";
			for (int i = 0; i < 1000; i++)
			{
				xmlBuilder += "<SetInfo><ClientID>STD</ClientID><LogisticProviderID>YTO</LogisticProviderID><TrackingInfoProvider>STD</TrackingInfoProvider>";
				xmlBuilder += string.Format("<BillID>9707563901{0:000}</BillID>", i);
				xmlBuilder += string.Format("<OrderID>9707563901{0:000}</OrderID>", i);
				xmlBuilder += "<DepName>迪拜</DepName><CreateDateTime>2016-08-19 18:47:36</CreateDateTime><StatusCode>P771</StatusCode><StatusDesc>航班已起飞</StatusDesc>      <FacilityType>1</FacilityType><FacilityName>迪拜机场</FacilityName> <Contacter>张敏</Contacter><ContactInfo>13844231221</ContactInfo><Remark>备注信息</Remark></SetInfo>";
			}
			xmlBuilder += "</RequestTrackingInfo></Message>";

			string logistics_interface = DesEncrypt(xmlBuilder.ToString());
			string data_digest = UrlEncodingUft8(EncryptMd5Utf8(partnerId + logistics_interface + partnerId));
			string param = "logistics_interface=" + logistics_interface
						 + "&data_digest=" + data_digest
						 + "&clientID=" + clientId;

			string error = "";
			string responseStr = SendAndGetStr(apiUrl, param, ref error);

			return responseStr;
		}


		// 배송정보 추가하기
		public bool SetTrackingInfo(YTOLocalTrackingInfoModel INFO, ref YTOLocalTrackingInfoResultModel RESULT, ref string RET_MSG)
		{
			RESULT = new YTOLocalTrackingInfoResultModel();
			RET_MSG = "";

			DateTime TIMESTAMP = DateTime.UtcNow.AddHours(8);  // 중국시간 설정

			string xml_str = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
			xml_str += "<Message>";

			xml_str += "<Header>";
			xml_str += string.Format("<SeqNo>{0}</SeqNo>", INFO.SeqNo);
			xml_str += string.Format("<SendTimeStamp>{0}</SendTimeStamp>", INFO.SendTimeStamp);
			xml_str += "</Header>";

			xml_str += "<RequestTrackingInfo>";
			xml_str += string.Format("<RecordCount>{0}</RecordCount>", INFO.DETAIL.Count);
			for (int i = 0; i < INFO.DETAIL.Count; i++)
			{
				xml_str += "<SetInfo>";
				xml_str += string.Format("<ClientID>{0}</ClientID>", m_CLIENT_ID_TRACK_INFO);
				xml_str += string.Format("<LogisticProviderID>{0}</LogisticProviderID>", INFO.DETAIL[i].LogisticProviderID);
				xml_str += string.Format("<TrackingInfoProvider>{0}</TrackingInfoProvider>", m_CLIENT_ID_TRACK_INFO);
				xml_str += string.Format("<BillID>{0}</BillID>", INFO.DETAIL[i].BillID);
				xml_str += string.Format("<OrderID>{0}</OrderID>", INFO.DETAIL[i].OrderID);
				xml_str += string.Format("<DepName>{0}</DepName>", INFO.DETAIL[i].DepName);
				xml_str += string.Format("<CreateDateTime>{0}</CreateDateTime>", INFO.DETAIL[i].CreateDateTime);
				xml_str += string.Format("<StatusCode>{0}</StatusCode>", INFO.DETAIL[i].StatusCode);
				xml_str += string.Format("<StatusDesc>{0}</StatusDesc>", INFO.DETAIL[i].StatusDesc);
				xml_str += string.Format("<FacilityType>{0}</FacilityType>", INFO.DETAIL[i].FacilityType);
				xml_str += string.Format("<FacilityName>{0}</FacilityName>", INFO.DETAIL[i].FacilityName);
				xml_str += string.Format("<Contacter>{0}</Contacter>", INFO.DETAIL[i].Contacter);
				xml_str += string.Format("<ContactInfo>{0}</ContactInfo>", INFO.DETAIL[i].ContactInfo);
				xml_str += string.Format("<Remark>{0}</Remark>", INFO.DETAIL[i].Remark);
				xml_str += "</SetInfo>";
			}
			xml_str += "</RequestTrackingInfo>";

			xml_str += "</Message>";



			string logistics_interface = DesEncrypt(xml_str);
			string data_digest = UrlEncodingUft8(EncryptMd5Utf8(m_PARTNER_ID_TRACK_INFO + logistics_interface + m_PARTNER_ID_TRACK_INFO));
			string param = "logistics_interface=" + logistics_interface
						 + "&data_digest=" + data_digest
						 + "&clientID=" + m_CLIENT_ID_TRACK_INFO;

			string error = "";
			string responseStr = SendAndGetStr(m_TRACKING_INFO_URL, param, ref error);

			// YTO API 로그를 저장한다
			SetYTOAPILOG(INFO.RAINBOWCODE, "", INFO.MASTERNO, "SetTrackingInfo", xml_str, responseStr, error);

			if (responseStr.Length == 0)
			{
				RET_MSG = error;
				return false;
			}
			if (responseStr.Length < 5 || responseStr.Substring(0, 5).ToUpper() != "<?XML")
			{
				// 에러인 경우는 xml 형식이 아닌 오류내용 string을 리턴한다
				// ex : the data of tracking xml decrypted error
				RET_MSG = responseStr;
				return false;
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(responseStr);

				//<?xml version="1.0" encoding="UTF-8"?>
				//<Message>
				//<Header>
				//  <SeqNo>90000001111120160822</SeqNo>
				//  <SendTimeStamp>2018-01-05 13:40:09</SendTimeStamp>
				//</Header>
				//<ResponseTrackingInfo>
				//  <ResultFlag>1</ResultFlag>
				//  <ErrCode>SUCCESS</ErrCode>
				//  <ErrMsg>operate success</ErrMsg>
				//</ResponseTrackingInfo>
				//</Message>

				XmlNode ret_Message = doc.ChildNodes[1];

				XmlElement ret_Header = ret_Message["Header"];
				RESULT.SeqNo = ret_Header["SeqNo"].InnerText.Trim();
				RESULT.SendTimeStamp = ret_Header["SendTimeStamp"].InnerText.Trim();

				XmlElement ret_ResponseTrackingInfo = ret_Message["ResponseTrackingInfo"];
				RESULT.ResultFlag = ret_ResponseTrackingInfo["ResultFlag"].InnerText.Trim();
				RESULT.ErrCode = ret_ResponseTrackingInfo["ErrCode"].InnerText.Trim();
				RESULT.ErrMsg = ret_ResponseTrackingInfo["ErrMsg"].InnerText.Trim();
			}
			catch (Exception ex1)
			{
				RET_MSG = string.Format("Xml Parser Error : {0}", ex1.Message);
				return false;
			}



			if (RESULT.ErrCode.ToUpper() != "SUCCESS")  // api error
			{
				RET_MSG = string.Format("SetTrackingInfo Error : {0} (code {1})", RESULT.ErrMsg, RESULT.ResultFlag);
				return false;
			}

			return true;
		}


		//
		/// <summary>
		/// 8 byte 的秘钥，由java生成
		/// </summary>
		byte[] btKeys = { 61, 4, 104, (byte)(0xff & -119), 38, (byte)(0xff & -68), (byte)(0xff & -88), (byte)(0xff & -45) };


		/// <summary> 
		/// DES加密 
		/// </summary> 
		/// <param name="encryptString">要加密的字符串</param> 
		/// <returns></returns> 
		public string DesEncrypt(string encryptString)
		{
			byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);

			DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
			provider.Mode = CipherMode.ECB;

			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(btKeys, btKeys), CryptoStreamMode.Write);
			cStream.Write(inputByteArray, 0, inputByteArray.Length);
			cStream.FlushFinalBlock();

			string des = Convert.ToBase64String(mStream.ToArray());
			string des2 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(des));

			mStream.Close();
			cStream.Close();

			return des2;
		}


		// MD5相关的两个方法
		static public string UrlEncodingUft8(string input)
		{
			return HttpUtility.UrlEncode(input, Encoding.UTF8);
		}


		// MD5+base64
		static public string EncryptMd5Utf8(string data)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(data)));
		}


		// HTTP POST
		public static string SendAndGetStr(string url, string data, ref string error_str)
		{
			string result = "";
			error_str = "";

			try
			{
				HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
				request.ServicePoint.Expect100Continue = false;
				request.Method = "POST";
				request.KeepAlive = true;
				request.Timeout = 30000000;
				request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

				using (StreamWriter Writer = new StreamWriter(request.GetRequestStream()))
				{
					Writer.Write(data);
				}

				HttpWebResponse response = request.GetResponse() as HttpWebResponse;
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					result = reader.ReadToEnd();//获取到的返回值
				}
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










		// 쑤저우 해관정보전송 테스트
		public void blcWayBill_test()
		{
			string request_url = "http://180.153.190.90/globalunion/outcall/blcWayBill";
			string partner_id = "TESTSTD";
			string client_id = "TESTSTD";

			string xml_str = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
			xml_str += "<Message>";
			xml_str += "<Header>";
			xml_str += "<seqNo>90000001111120150401</seqNo>";
			xml_str += string.Format("<sendTimeStamp>{0}</sendTimeStamp>", DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"));
			xml_str += "</Header>";
			xml_str += "<RequestOrder>";
			xml_str += string.Format("<clientID>{0}</clientID>", client_id);
			xml_str += "<logisticProviderID>YTO</logisticProviderID>";
			xml_str += string.Format("<customsID>{0}</customsID>", client_id);
			xml_str += "<dataType>BL</dataType>";
			xml_str += "<logisticsCode>L0007</logisticsCode>";
			xml_str += "<logisticsName>河南圆通速递有限公司</logisticsName>";
			xml_str += "<logisticsCodeCiq>4100300005</logisticsCodeCiq>";
			xml_str += "<logisticsNameCiq>国检备案物流企业名称</logisticsNameCiq>";
			xml_str += "<totalLogisticsNo></totalLogisticsNo>";
			xml_str += "<subLogisticsNo></subLogisticsNo>";
			xml_str += "<logisticsNo>DD07153125</logisticsNo>";
			xml_str += "<orderNo>10000013</orderNo>";
			xml_str += "<platformCode>D00001</platformCode>";
			xml_str += "<platformName>测试电商平台名称</platformName>";
			xml_str += "<platformCodeCiq>国检备案的电商平台代码</platformCodeCiq>";
			xml_str += "<platformNameCiq>国检备案的电商平台名称</platformNameCiq>";
			xml_str += "<trackNo></trackNo>";
			xml_str += "<trackStatus></trackStatus>";
			xml_str += "<shipping>刘冬冬</shipping>";
			xml_str += "<shippingAddress>澳大利亚新西兰</shippingAddress>";
			xml_str += "<shippingTelephone></shippingTelephone>";
			xml_str += "<shippingZipCode></shippingZipCode>";
			xml_str += "<shippingCountryCiq>036</shippingCountryCiq>";
			xml_str += "<shippingCountryCus>601</shippingCountryCus>";
			xml_str += "<consignee>杨健</consignee>";
			xml_str += "<consigneeAddress>湖北省荆门市***区****街道</consigneeAddress>";
			xml_str += "<consigneeTelephone>13511111111</consigneeTelephone>";
			xml_str += "<consigneeZipCode>642185</consigneeZipCode>";
			xml_str += "<consigneeCountryCiq>156</consigneeCountryCiq>";
			xml_str += "<consigneeCountryCus>142</consigneeCountryCus>";
			xml_str += "<idType>1</idType>";
			xml_str += "<idNumber>511027198901024586</idNumber>";
			xml_str += "<declarationDate></declarationDate>";
			xml_str += "<internationalFreight></internationalFreight>";
			xml_str += "<domesticFreight></domesticFreight>";
			xml_str += "<supportValue></supportValue>";
			xml_str += "<worth>30.51</worth>";
			xml_str += "<currCode></currCode>";
			xml_str += "<grossWeight>1.00</grossWeight>";
			xml_str += "<quantity></quantity>";
			xml_str += "<packageTypeCiq>4M</packageTypeCiq>";
			xml_str += "<packageTypeCus></packageTypeCus>";
			xml_str += "<packNum></packNum>";
			xml_str += "<netWeight>0.6</netWeight>";
			xml_str += "<goodsName>衣物</goodsName>";
			xml_str += "<deliveryMethod></deliveryMethod>";
			xml_str += "<transportationMethod>7</transportationMethod>";
			xml_str += "<shipCode>50</shipCode>";
			xml_str += "<shipName></shipName>";
			xml_str += "<destinationPort></destinationPort>";
			xml_str += "<ieType>I</ieType>";
			xml_str += "<stockFlag>1</stockFlag>";
			xml_str += "<batchNumbers>123456987</batchNumbers>";
			xml_str += "<tradeCountryCiq>036</tradeCountryCiq>";
			xml_str += "<tradeCountryCus></tradeCountryCus>";
			xml_str += "<agentCode></agentCode>";
			xml_str += "<agentName></agentName>";
			xml_str += "<agentCodeCiq>国检备案的代理企业代码</agentCodeCiq>";
			xml_str += "<agentNameCiq>国检备案的代理企业名称</agentNameCiq>";
			xml_str += "<billType>3</billType>";
			xml_str += "<modifyMark>1</modifyMark>";
			xml_str += "<customsField></customsField>";
			xml_str += "<note></note>";
			xml_str += "<reserve1></reserve1>";
			xml_str += "<reserve2></reserve2>";
			xml_str += "<reserve3></reserve3>";
			xml_str += "<reserve4></reserve4>";
			xml_str += "<reserve5></reserve5>";
			xml_str += "</RequestOrder>";
			xml_str += "</Message>";

			string logistics_interface = DesEncrypt(xml_str);
			string data_digest_md5 = EncryptMd5Utf8(partner_id + logistics_interface + partner_id);
			string data_digest = UrlEncodingUft8(data_digest_md5);
			string param = "logistics_interface=" + logistics_interface
						 + "&data_digest=" + data_digest
						 + "&clientID=" + client_id
						 + "";

			//System.Windows.Forms.MessageBox.Show(data_digest_md5);
			//System.Windows.Forms.MessageBox.Show(data_digest);

			string error = "";
			string responseStr = SendAndGetStr(request_url, param, ref error);

			// 성공
			//<?xml version="1.0" encoding="UTF-8"?>
			//<Message>
			//<Header>
			//  <SeqNo></SeqNo>
			//  <SendTimeStamp>2018-03-23 10:28:47:197</SendTimeStamp>
			//  <success>true</success>
			//</Header>
			//<ResponseOrder>
			//  <ClientID>TESTSTD</ClientID>
			//  <LogisticProviderID>YTO</LogisticProviderID>
			//  <OrderID>DD07153125</OrderID>
			//  <ResultFlag>1</ResultFlag>
			//  <ErrCode>0</ErrCode>
			//  <ErrMsg>运单修改成功！</ErrMsg>
			//</ResponseOrder>
			//</Message>

			// 실패
			//<?xml version="1.0" encoding="UTF-8"?>
			//<Message>
			//<Header>
			//  <SeqNo></SeqNo>
			//  <SendTimeStamp></SendTimeStamp>
			//  <success>false</success>
			//</Header>
			//<ResponseOrder>
			//  <ClientID></ClientID>
			//  <LogisticProviderID></LogisticProviderID>
			//  <OrderID></OrderID>
			//  <ResultFlag></ResultFlag>
			//  <ErrCode>203</ErrCode>
			//  <ErrMsg>签名验证失败！</ErrMsg>
			//</ResponseOrder>
			//</Message>

			//System.Windows.Forms.MessageBox.Show(responseStr);
		}


		// 2018-05-31 jsy : 주문등록 테스트
		// YTO GLOBAL 내륙지점...
		// http://open.yto.net.cn/OpenPlatform/doc
		public bool OrderAdd_Test1(YTOLocalModel model, ref YTOLocalResultModel yto_result, ref string RET_MSG)
		{
			yto_result = new YTOLocalResultModel();
			RET_MSG = "";

			m_ORDERADD_URL = "http://58.32.246.71:8000/overseaOrderServlet";
			m_CLIENT_ID = "K21000119";
			m_PARTNER_ID = "u2Z1F7Fh";

			if (m_CLIENT_ID.Length == 0 || m_PARTNER_ID.Length == 0)
			{
				RET_MSG = "환경설정에서 API 연동에 필요한 값을 먼저 설정해야 합니다.";
				return false;
			}

			XmlDocument xmldoc = new XmlDocument();

			XmlElement RequestOrder = xmldoc.CreateElement("RequestOrder");

			XmlElement clientID = xmldoc.CreateElement("clientID");  // customerId 와 동일
			clientID.InnerText = m_CLIENT_ID;
			RequestOrder.AppendChild(clientID);

			XmlElement logisticProviderID = xmldoc.CreateElement("logisticProviderID");  // 물류회사ID ==> YTO 고정값
			logisticProviderID.InnerText = model.logisticProviderID;
			RequestOrder.AppendChild(logisticProviderID);

			XmlElement customerId = xmldoc.CreateElement("customerId");  // clientID 와 동일
			customerId.InnerText = m_CLIENT_ID;
			RequestOrder.AppendChild(customerId);

			XmlElement txLogisticID = xmldoc.CreateElement("txLogisticID");  // 주문번호(중복되지 않게)
			txLogisticID.InnerText = model.txLogisticID;
			RequestOrder.AppendChild(txLogisticID);

			XmlElement tradeNo = xmldoc.CreateElement("tradeNo");  // 비즈니스 트랜잭션 번호 (선택 사항)
			tradeNo.InnerText = model.tradeNo;
			RequestOrder.AppendChild(tradeNo);

			XmlElement totalServiceFee = xmldoc.CreateElement("totalServiceFee");  // 보험 값 = insuranceValue * 상품 수량 (기본값은 0.0)
			totalServiceFee.InnerText = model.totalServiceFee.ToString("0.00");
			RequestOrder.AppendChild(totalServiceFee);

			XmlElement codSplitFee = xmldoc.CreateElement("codSplitFee");  // 물류 회사 포인트 [COD] 실행 (일시적으로 사용되지 않음, 기본값은 0.0)
			codSplitFee.InnerText = model.codSplitFee.ToString("0.00");
			RequestOrder.AppendChild(codSplitFee);

			XmlElement orderType = xmldoc.CreateElement("orderType");  // 주문 유형 (0-COD, 1- 정상 주문, 2- 휴대용 주문, 3- 반환 주문, 4- 지불, 5- GLOBAL)
																	   //orderType.InnerText = model.orderType.ToString();
			orderType.InnerText = "5";
			RequestOrder.AppendChild(orderType);

			XmlElement serviceType = xmldoc.CreateElement("serviceType");  // 서비스 유형 (1 - 집 수신, 2 - 다음날 4 - 아침 8 - 날짜, 0 - 연락처). 기본값은 0입니다.
			serviceType.InnerText = model.serviceType.ToString();
			RequestOrder.AppendChild(serviceType);

			XmlElement flag = xmldoc.CreateElement("flag");  // 주문 플래그, 기본값은 0, 의미가 없습니다.
			flag.InnerText = model.flag.ToString();
			RequestOrder.AppendChild(flag);

			XmlElement mailNo = xmldoc.CreateElement("mailNo");  // 물류청구서 번호
			mailNo.InnerText = model.mailNo;
			RequestOrder.AppendChild(mailNo);

			XmlElement type = xmldoc.CreateElement("type");  // 주문 유형 (이 필드는 이전 버전과의 호환성을 위해 예약되어 있습니다)
			type.InnerText = model.type.ToString();
			RequestOrder.AppendChild(type);



			// sender
			XmlElement sender = xmldoc.CreateElement("sender");

			XmlElement sender_name = xmldoc.CreateElement("name");  // 이름
			sender_name.InnerText = model.sender_name;
			sender.AppendChild(sender_name);

			XmlElement sender_postCode = xmldoc.CreateElement("postCode");  // 우편번호
			sender_postCode.InnerText = model.sender_postCode;
			sender.AppendChild(sender_postCode);

			XmlElement sender_phone = xmldoc.CreateElement("phone");  // 전화번호
			sender_phone.InnerText = model.sender_phone;
			sender.AppendChild(sender_phone);

			XmlElement sender_mobile = xmldoc.CreateElement("mobile");  // 휴대폰번호
			sender_mobile.InnerText = model.sender_mobile;
			sender.AppendChild(sender_mobile);

			XmlElement sender_prov = xmldoc.CreateElement("prov");  // 성
			sender_prov.InnerText = model.sender_prov;
			sender.AppendChild(sender_prov);

			XmlElement sender_city = xmldoc.CreateElement("city");  // 시, 구
			sender_city.InnerText = model.sender_city;
			sender.AppendChild(sender_city);

			XmlElement sender_address = xmldoc.CreateElement("address");  // 세부주소
			sender_address.InnerText = model.sender_address;
			sender.AppendChild(sender_address);

			RequestOrder.AppendChild(sender);



			// receiver
			XmlElement receiver = xmldoc.CreateElement("receiver");

			XmlElement receiver_name = xmldoc.CreateElement("name");  // 이름
			receiver_name.InnerText = model.receiver_name;
			receiver.AppendChild(receiver_name);

			XmlElement receiver_postCode = xmldoc.CreateElement("postCode");  // 우편번호
			receiver_postCode.InnerText = model.receiver_postCode;
			receiver.AppendChild(receiver_postCode);

			XmlElement receiver_phone = xmldoc.CreateElement("phone");  // 전화번호
			receiver_phone.InnerText = model.receiver_phone;
			receiver.AppendChild(receiver_phone);

			XmlElement receiver_mobile = xmldoc.CreateElement("mobile");  // 휴대폰번호
			receiver_mobile.InnerText = model.receiver_mobile;
			receiver.AppendChild(receiver_mobile);

			XmlElement receiver_prov = xmldoc.CreateElement("prov");  // 성
			receiver_prov.InnerText = model.receiver_prov;
			receiver.AppendChild(receiver_prov);

			XmlElement receiver_city = xmldoc.CreateElement("city");  // 시, 구
			receiver_city.InnerText = model.receiver_city;
			receiver.AppendChild(receiver_city);

			XmlElement receiver_address = xmldoc.CreateElement("address");  // 세부주소
			receiver_address.InnerText = model.receiver_address;
			receiver.AppendChild(receiver_address);

			RequestOrder.AppendChild(receiver);



			XmlElement sendStartTime = xmldoc.CreateElement("sendStartTime");  // yyyy-MM-dd HH:mm:ss
			sendStartTime.InnerText = model.sendStartTime;
			RequestOrder.AppendChild(sendStartTime);

			XmlElement sendEndTime = xmldoc.CreateElement("sendEndTime");  // yyyy-MM-dd HH:mm:ss
			sendEndTime.InnerText = model.sendEndTime;
			RequestOrder.AppendChild(sendEndTime);

			XmlElement goodsValue = xmldoc.CreateElement("goodsValue");  // 상품 및 배송비는 포함하지만 서비스 요금은 포함되지 않은 제품 금액
			goodsValue.InnerText = model.goodsValue.ToString("0.00");
			RequestOrder.AppendChild(goodsValue);

			XmlElement totalValue = xmldoc.CreateElement("totalValue");  // goodsValue + 총 서비스 수수료
			totalValue.InnerText = model.totalValue.ToString("0.00");
			RequestOrder.AppendChild(totalValue);

			XmlElement agencyFund = xmldoc.CreateElement("agencyFund");  // 수금액이 수금 순서 인 경우 수거 금액은 0보다 커야하며 비 수거는 0.0으로 설정되어야합니다.
			agencyFund.InnerText = model.agencyFund.ToString("0.00");
			RequestOrder.AppendChild(agencyFund);

			XmlElement itemsValue = xmldoc.CreateElement("itemsValue");  // 상품가격
			itemsValue.InnerText = model.itemsValue.ToString("0.00");
			RequestOrder.AppendChild(itemsValue);

			XmlElement itemsWeight = xmldoc.CreateElement("itemsWeight");  // 총중량
			itemsWeight.InnerText = model.itemsWeight.ToString("0.00");
			RequestOrder.AppendChild(itemsWeight);



			// items
			XmlElement items = xmldoc.CreateElement("items");

			for (int i = 0; i < model.ITEMS.Count; i++)
			{
				XmlElement item = xmldoc.CreateElement("item");

				XmlElement itemName = xmldoc.CreateElement("itemName");  // 상품명
				itemName.InnerText = model.ITEMS[i].itemName;
				item.AppendChild(itemName);

				XmlElement number = xmldoc.CreateElement("number");  // 상품갯수
				number.InnerText = model.ITEMS[i].number.ToString();
				item.AppendChild(number);

				XmlElement itemValue = xmldoc.CreateElement("itemValue");  // 단가 (2 자리 소수점)
				itemValue.InnerText = model.ITEMS[i].itemValue.ToString("0.00");
				item.AppendChild(itemValue);

				items.AppendChild(item);
			}



			XmlElement insuranceValue = xmldoc.CreateElement("insuranceValue");  // 보험 금액의 금액 (보험 금액은 제품의 가치입니다 (100보다 크거나 같음 3w 미만), 기본값은 0.0입니다)
			insuranceValue.InnerText = model.insuranceValue.ToString("0.00");
			RequestOrder.AppendChild(insuranceValue);

			XmlElement special = xmldoc.CreateElement("special");  // 제품 유형 (필드 유지, 일시적으로 보유하지 않음)
			special.InnerText = model.special.ToString();
			RequestOrder.AppendChild(special);

			XmlElement remark = xmldoc.CreateElement("remark");  // 비고
			remark.InnerText = model.remark;
			RequestOrder.AppendChild(remark);





			string xml_str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + RequestOrder.OuterXml;
			string error = "";
			string result = GetAPIResult(xml_str, ref error);

			// YTO API 로그를 저장한다
			SetYTOAPILOG(model.RAINBOWCODE, model.AGENTCODE, model.INVOICENO, "OrderAdd", xml_str, result, error);

			if (result.Length == 0)
			{
				RET_MSG = error;
				return false;
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(result);

				//실패 1
				//<Response>
				//  <txLogisticID></txLogisticID>
				//  <logisticProviderID>YTO</logisticProviderID>
				//  <code>S02</code>
				//  <success>false</success>
				//  <reason>S02,illegal digital signature, please check your secret key：非法的数字签名，请检查您的密钥</reason>
				//</Response>

				//실패 2
				//<Response>
				//  <txLogisticID>MIR20171220-1</txLogisticID>
				//  <logisticProviderID>YTO</logisticProviderID>
				//  <code>S01</code>
				//  <success>false</success>
				//  <reason>mailNo must be empty：运单号mailNo必须为空</reason>
				//</Response>

				//성공
				//<Response>
				//  <logisticProviderID>YTO</logisticProviderID>
				//  <txLogisticID>MIR20171220-1</txLogisticID>
				//  <clientID>K21000119</clientID>
				//  <mailNo>899932564524</mailNo>
				//  <distributeInfo>
				//    <shortAddress>302-046-206</shortAddress>
				//    <consigneeBranchCode>210201</consigneeBranchCode>
				//    <packageCenterCode>210901</packageCenterCode>
				//    <packageCenterName>上海转运中心</packageCenterName>
				//  </distributeInfo>
				//  <code>200</code>
				//  <success>true</success>
				//</Response>

				XmlNode ret_response = doc.ChildNodes[0];

				for (int i = 0; i < ret_response.ChildNodes.Count; i++)
				{
					XmlNode node = ret_response.ChildNodes[i];

					if (string.Compare(node.Name, "logisticProviderID", StringComparison.OrdinalIgnoreCase) == 0)
					{
						yto_result.logisticProviderID = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "txLogisticID", StringComparison.OrdinalIgnoreCase) == 0)
					{
						yto_result.txLogisticID = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "clientID", StringComparison.OrdinalIgnoreCase) == 0)
					{
						yto_result.clientID = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "code", StringComparison.OrdinalIgnoreCase) == 0)  // error code
					{
						yto_result.code = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "success", StringComparison.OrdinalIgnoreCase) == 0)  // result
					{
						yto_result.success = GlobalFunction.GetBool(node.InnerText.Trim());
						continue;
					}
					else if (string.Compare(node.Name, "reason", StringComparison.OrdinalIgnoreCase) == 0)  // error desc
					{
						yto_result.reason = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "mailNo", StringComparison.OrdinalIgnoreCase) == 0)  // YTO번호
					{
						yto_result.mailNo = node.InnerText.Trim();
						continue;
					}
					else if (string.Compare(node.Name, "distributeInfo", StringComparison.OrdinalIgnoreCase) == 0)
					{
						for (int k = 0; k < node.ChildNodes.Count; k++)
						{
							XmlNode node2 = node.ChildNodes[k];

							if (string.Compare(node2.Name, "shortAddress", StringComparison.OrdinalIgnoreCase) == 0) yto_result.shortAddress = node2.InnerText.Trim();
							else if (string.Compare(node2.Name, "consigneeBranchCode", StringComparison.OrdinalIgnoreCase) == 0) yto_result.consigneeBranchCode = node2.InnerText.Trim();
							else if (string.Compare(node2.Name, "packageCenterCode", StringComparison.OrdinalIgnoreCase) == 0) yto_result.packageCenterCode = node2.InnerText.Trim();
							else if (string.Compare(node2.Name, "packageCenterName", StringComparison.OrdinalIgnoreCase) == 0) yto_result.packageCenterName = node2.InnerText.Trim();
						}
					}
				}
			}
			catch (Exception ex1)
			{
				RET_MSG = string.Format("Xml Parser Error : {0}", ex1.Message);
				return false;
			}



			if (!yto_result.success)  // api error
			{
				RET_MSG = string.Format("OrderAdd Error : {0} (code {1})", yto_result.reason, yto_result.code);
				return false;
			}

			return true;
		}
	}
}
