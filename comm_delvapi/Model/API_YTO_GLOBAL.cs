using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using comm_dbconn;
using comm_global;

using System.Net.Json;

using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using System.Security.Cryptography;
using System.IO;

namespace comm_delvapi.Model
{

	//===================ServiceReference_YtoGlobal===================================
	//해당 DLL 관련 문제로 주석 처리 차후 제거 필요

	// 중국 YTO Global API 연동
	public class API_YTO_GLOBAL
	{
		public enum YTO_APIType
		{
			TEST,
			REAL
		}

		// http://47.90.107.70:8000/webservice/PublicService.asmx test url
		// http://oms.ytoglobal.com/webservice/PublicService.asmx real url

		// 서비스 참조 추가
		// ServiceReference_YtoGlobal.PublicServiceSoapClient 메서드 이용하므로 url따로 필요없음
		// app.config파일 <endpoint>두개들어있는항목 한개 주석


		string API_YTO_APP_TOKEN = "";
		string API_YTO_APP_KEY = "";
		string API_YTO_CLIENT_ID = "";

		string RAINBOWCODE = "";

		public API_YTO_GLOBAL(YTO_APIType apiType, string appToken = "", string appKey = "", string client_id = "", string rainbow_code = "00000")
		{
			API_YTO_APP_TOKEN = appToken;
			API_YTO_APP_KEY = appKey;
			API_YTO_CLIENT_ID = client_id;

			RAINBOWCODE = rainbow_code;

			switch (apiType)
			{
				case YTO_APIType.TEST:
					//appToken = "ae7040e1df5bdabd64e9e568e4312635";
					//appKey = "d7013e389be1824cb7ad698dc5eeac16d7013e389be1824cb7ad698dc5eeac16";
					break;
				case YTO_APIType.REAL:
					////appToken = "es51avlqjc4bhry9";
					////appKey = "hg2ofjrqlp58mkx0u9c3d14tan7vsyzw";
					//appToken = "wq3fyct1anld6k8h";
					//appKey = "16ixsrbvmnq3egc0du8ylaw4z92j5p7k";
					break;
			}
		}



		//getshippingmethod
		public bool getshippingmethod(ref string response, ref string RET_MSG)
		{
			string serviceMethod = "getshippingmethod";


			string error_str = "";

			//string jsontext = GetAPIResult(method, orderscollecction.ToString(), ref error_str);

			string result = string.Empty;

			//===================ServiceReference_YtoGlobal===================================
			//using (ServiceReference_YtoGlobal.PublicServiceSoapClient client = new ServiceReference_YtoGlobal.PublicServiceSoapClient())
			//{
				//result = client.ServiceEntrance(API_YTO_APP_TOKEN, API_YTO_APP_KEY, serviceMethod, string.Empty);
			//}

			Console.Write(result);
			response = result;



			//{"data":[
			//    {"code":"TEST","cnname":"系统测试产品","enname":"Test","note":"系统测试产品"},
			//    {"code":"T1","cnname":"台湾专线","enname":"TaiWan","note":""},
			//    {"code":"PK0001","cnname":"圆通轻包专线","enname":"LaosPost","note":""},
			//    {"code":"PK0002","cnname":"台湾专线宅配【普货】","enname":"TWQDS","note":"普货宅配"},
			//    {"code":"PK0003","cnname":"台湾专线店配【普货】","enname":"TWDP","note":"普货店配"},
			//    {"code":"PK0004","cnname":"台湾专线宅配【特货】","enname":"TWQDTHFW","note":"特货宅配"},
			//    {"code":"PK0005","cnname":"台湾专线店配【特货】","enname":"TWDPTHFW","note":"特货店配"},
			//    {"code":"PK0006","cnname":"上海入境电商包裹服务","enname":"SHRJDSBGFW","note":""},
			//    {"code":"PK0007","cnname":"广州入境电商包裹服务","enname":"GZRJDSBGFW","note":""},
			//    {"code":"PK0008","cnname":"广州入境个人包裹服务","enname":"GZRJGEBGFW","note":""},
			//    {"code":"YTOKRQD","cnname":"【韩国-济南】全链路","enname":"KR-JN","note":"韩国-济南全链路0.5kg进位"},
			//    {"code":"DE0001","cnname":"海淘奶粉专线","enname":"naifen","note":"德国进口"},
			//    {"code":"ID001","cnname":"印尼全境快递普货服务","enname":"INPHEXPRESS","note":""},
			//    {"code":"DI001","cnname":"印尼全境快递专线服务","enname":"yinnipingyou","note":"印尼快递专线"},
			//    {"code":"DI002","cnname":"印尼全境快递挂号小包","enname":"yinniguahao","note":"印尼邮政挂号小包"},
			//    {"code":"JAPAN001","cnname":"日本全境快递普货","enname":"Japan001","note":"日本全境快递"},
			//    {"code":"TWJK0001","cnname":"[台-陆]快件直郵天津口岸","enname":"TW-TJ","note":"台湾-天津"},
			//    {"code":"TWXGCP","cnname":"台湾香港快件专线","enname":"TWXGCP","note":"台湾-香港"},
			//    {"code":"YTOKRJN","cnname":"【韩国-青岛】全链路","enname":"KR-QD","note":"韩国-青岛全链路0.5kg进位"},
			//    {"code":"AU0001","cnname":"圆通澳大利亚经济快递","enname":"AUPOST","note":"澳洲邮政"},
			//    {"code":"YTOKRTW","cnname":"圆通国际快件[韩国-台湾]","enname":"KRTW","note":"韩国-台湾"},
			//    {"code":"TWGZ0001","cnname":"[台-陆]快件直郵广州口岸","enname":"TW-GZ","note":"台湾-广州"},
			//    {"code":"HK-GZ","cnname":"香港出口-广州口岸","enname":"HK-GZ","note":"香港-广州"},
			//    {"code":"HKTW01","cnname":"香港台湾专线【普货】","enname":"HKTW01","note":"香港-台湾淘宝业务"},
			//    {"code":"HKTW02","cnname":"香港台湾专线【特货】","enname":"HKTW02","note":"香港-台湾淘宝业务"},
			//    {"code":"PK0011","cnname":"台湾【集运宅配】服务","enname":"TWJYZP","note":"台湾集运宅配服务"},
			//    {"code":"PK0012","cnname":"台湾【集运店配】服务","enname":"TWJYDP","note":"台湾集运店配服务"},
			//    {"code":"PK0013","cnname":"台湾【直运店配】服务","enname":"TWZYDP","note":"台湾直运店配服务"},
			//    {"code":"PK0014","cnname":"台湾【直运宅配】服务","enname":"TWZYZP","note":"台湾直运宅配服务"},
			//    {"code":"KRQD001","cnname":"韩服小包全链路【0.1kg】","enname":"韩服小包全链路(0.1kg)","note":"韩服小包全链路(0.1kg)"},
			//    {"code":"PK00015","cnname":"上海-台湾专线宅配普货","enname":"SHTWZP","note":"上海-台湾-宅配普货"},
			//    {"code":"PK00016","cnname":"上海-台湾专线店配普货","enname":"SHTWDP","note":"上海-台湾-店配普货"},
			//    {"code":"TWOW_ZPTH","cnname":"台湾属地专线【宅配特货】","enname":"TWOW_ZPTH","note":"台湾专线【宅配特货】台币结算产品"},
			//    {"code":"TWOW_ZPPH","cnname":"台湾属地专线【宅配普货】","enname":"TWOW_ZPPH","note":"台湾专线【宅配普货】台币结算产品"},
			//    {"code":"TWOW_DPTH","cnname":"台湾属地专线【店配特货】","enname":"TWOW_DPTH","note":"台湾专线【店配特货】台币结算产品"},
			//    {"code":"TWOW_DPPH","cnname":"台湾属地专线【店配普货】","enname":"TWOW_DPPH","note":"台湾专线【店配普货】台币结算产品"},
			//    {"code":"YTOKRSZ","cnname":"代理【韩国-深圳】化妆品链路","enname":"KR-SZ","note":"韩国-深圳化妆品链路1kg进位"},
			//    {"code":"KR_JN","cnname":"韩国至济南","enname":"KR_JN1kg","note":""},
			//    {"code":"RUSEXPRESS","cnname":"圆通俄罗斯经济快递","enname":"RUSEXPRESS","note":"俄罗斯电商小包"},
			//    {"code":"YTO_WF","cnname":"韩国-【潍坊中心】","enname":"YTO_WF","note":""},
			//    {"code":"KR_YTOJM","cnname":"韩国-【江门中心】","enname":"KR_YTOJM","note":""},
			//    {"code":"SE_SH","cnname":"瑞典-中国【小包】","enname":"SE_SH","note":"瑞典-上海口岸"},
			//    {"code":"PK0010","cnname":"台湾-福州","enname":"TWFZ","note":"台湾-福州口岸"},
			//    {"code":"PK0015","cnname":"台湾-上海","enname":"TW-SH","note":"台湾至上海口岸"},
			//    {"code":"USA-CN-A","cnname":"美国进口至哈尔滨A类普货","enname":"USA-CN-A","note":""},
			//    {"code":"HK EXPRESS","cnname":"圆通香港专线","enname":"HK EXPRESS","note":""},
			//    {"code":"TWEXPRESS","cnname":"圆通台湾专线","enname":"TWEXPRESS","note":""},
			//    {"code":"TWIMP","cnname":"台湾进口清关配送宅配","enname":"TWIMP","note":"海外(不包含中国大陆)-台湾"},
			//    {"code":"TWIMP2","cnname":"台湾岛内宅配","enname":"TWIMP2","note":"产品名称由台湾进口落地配宅配改为台湾岛内宅配"},
			//    {"code":"TWIMPDP","cnname":"台湾进口清关配送店配","enname":"TWIMPDP","note":"海外(不包含中国大陆)-台湾"},
			//    {"code":"TWIMP2DP","cnname":"台湾进口落地配店配","enname":"TWIMP2DP","note":"海外(不包含中国大陆)-台湾"},
			//    {"code":"AU0002","cnname":"澳大利亚至中国大陆进口落地配服务","enname":"AU0002","note":""},
			//    {"code":"KR-IMP","cnname":"韩国至青岛","enname":"KR-IMP","note":""},
			//    {"code":"HKZS","cnname":"香港直送1.5宅配","enname":"HKZS","note":""},
			//    {"code":"HGZS2","cnname":"香港直送1.5店配","enname":"HGZS2","note":""},
			//    {"code":"TH0001","cnname":"泰国至中国专线","enname":"TH0001","note":""},
			//    {"code":"KR-TW","cnname":"韩国至台湾宅配","enname":"KR-TW","note":""},
			//    {"code":"ID0001","cnname":"圆通印尼经济快递","enname":"ID0001","note":""},
			//    {"code":"TWSPECIAL","cnname":"圆通台湾特货专线","enname":"TWSPECIAL","note":"充电宝、电子类马达、音箱、电子配件、平衡车可发;锂电池、纽扣电子不可发rn可发普通液体,无毒无害粉末,食品,膏状类,妆品(口红,唇膏,美瞳)等包裹"},
			//    {"code":"AU0003","cnname":"澳大利亚至杭州","enname":"AU0003","note":""},
			//    {"code":"KR-HK","cnname":"韩国至香港专线","enname":"KR-HK","note":""},
			//    {"code":"HWIMP","cnname":"圆通海外进口落地配服务","enname":"HWIMP","note":""},
			//    {"code":"KR-TW1","cnname":"韩国至台湾店配","enname":"KR-TW1","note":""},
			//    {"code":"DE0002","cnname":"德国进口至深圳","enname":"DE0002","note":""},
			//    {"code":"DE0003","cnname":"德国进口至广州","enname":"DE0003","note":""},
			//    {"code":"AU0004","cnname":"澳大利亚至广州","enname":"AU0004","note":""},
			//    {"code":"AU0005","cnname":"澳大利亚至天津","enname":"AU0005","note":""},
			//    {"code":"AU0006","cnname":"澳大利亚至西安","enname":"AU0006","note":""},
			//    {"code":"YTODL0001","cnname":"圆通其他经济快递(新马泰韩斯里兰卡)","enname":"YTODL0001","note":"可发新马泰韩/斯里兰卡/沙特/阿联酋7国"},
			//    {"code":"AU0007","cnname":"澳大利亚至江门","enname":"AU0007","note":""},
			//    {"code":"USA-CN-B","cnname":"美国进口至天津","enname":"USA-CN-B","note":""},
			//    {"code":"PK0016","cnname":"台湾属地专线普货店配","enname":"PK0016","note":"台币(执御)"},
			//    {"code":"PK0017","cnname":"台湾属地专线特货店配","enname":"PK0017","note":"台币(执御)"},
			//    {"code":"AE0001","cnname":"阿联酋进口至广州","enname":"AE0001","note":""},
			//    {"code":"PK0018","cnname":"圆通美国小包","enname":"PK0018","note":""},
			//    {"code":"PK0019","cnname":"圆通印度经济快递","enname":"PK0019","note":""},
			//    {"code":"HK0001","cnname":"香港进口至昆明","enname":"HK0001","note":""},
			//    {"code":"PK0020","cnname":"圆通全球标准快递","enname":"PK0020","note":"马其顿/朝鲜/伊朗/叙利亚/土库曼斯坦/科索沃/中非共和国/古巴/苏丹/朝鲜/阿塞拜疆/塔吉克斯坦/俄罗斯暂停包裹与文件服务;亚美尼亚/也门阿拉伯共合国/阿根廷/乌拉圭/巴拉圭/印度暂停包裹服务"},
			//    {"code":"TWOW_DPPH1","cnname":"台湾属地专线店配普货(全家)","enname":"TWOW_DPPH1","note":""},
			//    {"code":"TWOW_DPTH1","cnname":"台湾属地专线店配特货(全家)","enname":"TWOW_DPTH1","note":""},
			//    {"code":"KR-HZ","cnname":"韩国至杭州落地配","enname":"KR-HZ","note":""},
			//    {"code":"ITXPCESHI","cnname":"IT虾皮测试","enname":"IT虾皮测试","note":""},
			//    {"code":"KR-GZ","cnname":"韩国至广州","enname":"KR-GZ","note":""},
			//    {"code":"CA0001","cnname":"加拿大至杭州","enname":"CA0001","note":""},
			//    {"code":"MOEXPRESS","cnname":"圆通澳门专线","enname":"MOEXPRESS","note":""},
			//    {"code":"PK0021","cnname":"圆通印度小包","enname":"PK0021","note":""},
			//    {"code":"PK0022","cnname":"圆通欧洲经济快递","enname":"PK0022","note":""},
			//    {"code":"PK0023","cnname":"圆通","enname":"PK0023","note":""},
			//    {"code":"PK0024","cnname":"圆通中东-非洲经济快递","enname":"PK0024","note":""},
			//    {"code":"PK0025","cnname":"圆通日本经济快递","enname":"PK0025","note":""},
			//    {"code":"PK0026","cnname":"圆通美国经济快递","enname":"PK0026","note":""},
			//    {"code":"PK0027","cnname":"E邮宝","enname":"PK0027","note":""},
			//    {"code":"KR-TWTH","cnname":"韩国至台湾宅配特货","enname":"KR-TWTH","note":""},
			//    {"code":"KR-TW1TH","cnname":"韩国至台湾店配特货","enname":"KR-TW1TH","note":""},
			//    {"code":"PK0028","cnname":"圆通阿联酋经济快递","enname":"PK0028","note":""},
			//    {"code":"PK0029","cnname":"圆通邮政EMS","enname":"PK0029","note":""},
			//    {"code":"PK0030","cnname":"台湾属地专线普货店配(赛维)","enname":"PK0030","note":"台币(赛维)"},
			//    {"code":"PK0031","cnname":"台湾属地专线特货店配(赛维)","enname":"PK0031","note":"台币(赛维)"},
			//    {"code":"USA0001","cnname":"美国进口至香港E特快A类普货","enname":"USA0001","note":""},
			//    {"code":"PK0032","cnname":"圆通中美专线","enname":"PK0032","note":""},
			//    {"code":"HK0002","cnname":"香港进口至虎门","enname":"HK0002","note":""},
			//    {"code":"USA-CN-C","cnname":"美国进口至哈尔滨C类特货","enname":"USA-CN-C","note":"(包包/电子产品/妆品等)"},
			//    {"code":"USA0002","cnname":"美国进口至香港E特快C类特货","enname":"USA0002","note":"(包包/电子产品/妆品等)"},
			//    {"code":"NZ0001","cnname":"新西兰进口至天津","enname":"NZ0001","note":""},
			//    {"code":"USA0003","cnname":"美国进口至湛江A类普货","enname":"USA0003","note":""},
			//    {"code":"USA0004","cnname":"美国进口至湛江C类特货","enname":"USA0004","note":""},
			//    {"code":"USA0005","cnname":"美国进口至湛江(奶粉)","enname":"USA0005","note":""},
			//    {"code":"USA0006","cnname":"美国进口至香港电商A类普货","enname":"USA0006","note":""},
			//    {"code":"USA0007","cnname":"美国进口至香港电商C类普货","enname":"USA0007","note":""},
			//    {"code":"HK0003","cnname":"香港进口至深圳","enname":"HK0003","note":""},
			//    {"code":"HK0004","cnname":"香港进口至广州","enname":"HK0004","note":""},
			//    {"code":"NP0001","cnname":"尼泊尔进口至广州","enname":"NP0001","note":""},
			//    {"code":"NP0002","cnname":"尼泊尔进口至成都","enname":"NP0002","note":""},
			//    {"code":"PK0033","cnname":"纸质面单未录单计费","enname":"PK0033","note":""},
			//    {"code":"KR-HZ1","cnname":"韩国至杭州全链路","enname":"KR-HZ1","note":""}],
			//"success":1,
			//"cnmessage":null,
			//"enmessage":null
			//}



			if (result.Length == 0)
			{
				RET_MSG = string.Format("YTO OrderAdd API result error ({0})", error_str);
				return false;
			}


			return true;
		}




		// 주문등록
		public bool OrderAdd(YTOModel model, ref string delvno, ref string RET_MSG)
		{
			string serviceMethod = "createorder";

			JsonObjectCollection orderscollecction = new JsonObjectCollection();
			orderscollecction.Add(new JsonStringValue("reference_no", model.reference_no));//주문번호 -- test 12345678CN
			orderscollecction.Add(new JsonStringValue("shipping_method", model.shipping_method));//getshippingmethod를 이용해 받은값 (한국-중국)
			orderscollecction.Add(new JsonStringValue("order_weight", model.order_weight));//무게

			JsonObjectCollection shipperInfo = new JsonObjectCollection("shipper");
			shipperInfo.Add(new JsonStringValue("shipper_name", model.shipper_name));//발송인이름
			shipperInfo.Add(new JsonStringValue("shipper_countrycode", model.shipper_countrycode));//발송인 국가코드
			shipperInfo.Add(new JsonStringValue("shipper_street", model.shipper_street));//발송인 주소                        
			shipperInfo.Add(new JsonStringValue("shipper_telephone", ""));//
			shipperInfo.Add(new JsonStringValue("shipper_mobile", model.shipper_mobile));//발송인 번호
			orderscollecction.Add(shipperInfo);

			JsonObjectCollection consigneeInfo = new JsonObjectCollection("consignee");
			consigneeInfo.Add(new JsonStringValue("consignee_name", model.consignee_name));//수취인이름//YE001433395CN
			consigneeInfo.Add(new JsonStringValue("consignee_countrycode", model.consignee_countrycode));//수취인 국가코드
			consigneeInfo.Add(new JsonStringValue("consignee_province", model.consignee_province));//수취인 주소 성
			consigneeInfo.Add(new JsonStringValue("consignee_city", model.consignee_city));//수취인 시
			consigneeInfo.Add(new JsonStringValue("consignee_district", model.consignee_district));//수취인 구
			consigneeInfo.Add(new JsonStringValue("consignee_street", model.consignee_street));//수취인 주소
			consigneeInfo.Add(new JsonStringValue("consignee_postcode", model.consignee_postcode));//수취인 우편번호(필수아님)
			consigneeInfo.Add(new JsonStringValue("consignee_telephone", model.consignee_telephone));//수취인 번호
			consigneeInfo.Add(new JsonStringValue("consignee_mobile", ""));//수취인 번호
			consigneeInfo.Add(new JsonStringValue("consignee_certificatetype", ""));//ID:신분증, PP:여권
			consigneeInfo.Add(new JsonStringValue("consignee_certificatecode", ""));//신분증 번호33040219880723122X
			orderscollecction.Add(consigneeInfo);


			JsonArrayCollection orderItems = new JsonArrayCollection("invoice");

			for (int i = 0; i < model.invoiceList.Count; i++)
			{
				JsonObjectCollection itemJson = new JsonObjectCollection();
				itemJson.Add(new JsonStringValue("invoice_enname", ((model.invoiceList[i].invoice_enname.Length > 0) ? model.invoiceList[i].invoice_enname : "product")));//영문이름 필수!
				itemJson.Add(new JsonStringValue("invoice_cnname", model.invoiceList[i].invoice_cnname));//중문이름 미필수
				itemJson.Add(new JsonNumericValue("invoice_quantity", model.invoiceList[i].invoice_quantity));//갯수
				itemJson.Add(new JsonNumericValue("invoice_unitcharge", model.invoiceList[i].invoice_unitcharge));//개당 가격(USD) float

				orderItems.Add(itemJson);
			}


			//}
			orderscollecction.Add(orderItems);

			JsonArrayCollection extra_service_list = new JsonArrayCollection("extra_service");

			JsonObjectCollection extraService = new JsonObjectCollection();
			extraService.Add(new JsonStringValue("extra_servicecode", ""));//부가서비스
			extraService.Add(new JsonStringValue("extra_servicevalue", ""));//
			extra_service_list.Add(extraService);
			orderscollecction.Add(extra_service_list);

			string paramsJson = orderscollecction.ToString();

			paramsJson = paramsJson.Replace("\r", "");
			paramsJson = paramsJson.Replace("\n", "");
			paramsJson = paramsJson.Replace("\t", "");

			string result = string.Empty;
			string error = "";

			try
			{
				//===================ServiceReference_YtoGlobal===================================
				//using (ServiceReference_YtoGlobal.PublicServiceSoapClient client = new ServiceReference_YtoGlobal.PublicServiceSoapClient())
				//{
					//result = client.ServiceEntrance(API_YTO_APP_TOKEN, API_YTO_APP_KEY, serviceMethod, paramsJson);
				//}
			}
			catch (Exception ex2)
			{
				RET_MSG = string.Format("YTO createorder API error : {0} ({1})", ex2.Message, -100);
				return false;
			}

			// 로그를 저장한다
			SetYTOAPILOG(model.RAINBOWCODE, model.AGENTCODE, model.INVOICENO, serviceMethod, paramsJson.ToString(), result, error);

			if (result.Length == 0)
			{
				RET_MSG = "YTO createorder API 응답값이 없습니다. 관리자에게 문의해주세요";
				return false;
			}



			try
			{
				JsonTextParser parser = new JsonTextParser();
				JsonObject obj = parser.Parse(result);
				JsonObjectCollection col = (JsonObjectCollection)obj;

				//{
				//    "data": {
				//        "order_id": 10394433,
				//        "refrence_no": "3010615000211",
				//        "shipping_method_no": "G10100162403",
				//        "distribution_code": null,
				//        "shipment_third_number": null
				//    },
				//    "success": 1,
				//    "cnmessage": "订单创建成功",
				//    "enmessage": "Order created successfully"
				//}

				string code = (col["success"].GetValue() != null) ? col["success"].GetValue().ToString().Trim() : "";
				if (code.Equals("1"))
				{//성공
				 //{"data":{"order_id":1847543,"refrence_no":"12345678CN","shipping_method_no":"G00016040882"},"success":1,"cnmessage":"订单创建成功","enmessage":"Order created successfully"}
					JsonObject data = (col["data"].GetValue() != null) ? (JsonObject)col["data"] : null;

					if (data != null)
					{
						JsonObjectCollection success = (JsonObjectCollection)data;


						string order_id = "";
						if (success["order_id"].GetValue() != null)
						{
							order_id = success["order_id"].GetValue().ToString().Trim();
						}


						if (success["shipping_method_no"].GetValue() != null)
						{
							delvno = success["shipping_method_no"].GetValue().ToString().Trim();//배송번호
						}

						return true;
					}

				}
				else//실패
				{
					string enmessage = (col["enmessage"].GetValue() != null) ? col["enmessage"].GetValue().ToString().Trim() : "";
					string cnmessage = (col["cnmessage"].GetValue() != null) ? col["cnmessage"].GetValue().ToString().Trim() : "";

					RET_MSG = string.Format("YTO createorder API RESULT : {0})", enmessage);
					return false;
				}

			}
			catch (Exception ex)
			{
				RET_MSG = string.Format("YTO createorder JSON error : {0} ({1})", ex.Message, -100);
				return false;
			}



			return true;
		}


		// 지역코드 가져오기
		public string getdistribute(string rainbow_code, string agent_code, string reference_no, string shipping_method_no, ref string RET_MSG)
		{
			RET_MSG = "";

			string distribute_code = "";
			string serviceMethod = "getdistribute";

			JsonObjectCollection orderscollecction = new JsonObjectCollection();
			orderscollecction.Add(new JsonStringValue("reference_no", reference_no));
			orderscollecction.Add(new JsonStringValue("shipping_method_no", shipping_method_no));

			string paramsJson = orderscollecction.ToString();

			paramsJson = paramsJson.Replace("\r", "");
			paramsJson = paramsJson.Replace("\n", "");
			paramsJson = paramsJson.Replace("\t", "");

			string result = string.Empty;
			try
			{

				//===================ServiceReference_YtoGlobal===================================
				//using (ServiceReference_YtoGlobal.PublicServiceSoapClient client = new ServiceReference_YtoGlobal.PublicServiceSoapClient())
				//{
					//result = client.ServiceEntrance(API_YTO_APP_TOKEN, API_YTO_APP_KEY, serviceMethod, paramsJson);
				//}
			}
			catch (Exception ex2)
			{
				RET_MSG = string.Format("YTO getdistribute : ServiceEntrance error : {0}", ex2.Message);
			}

			// YTO API 로그를 저장한다
			SetYTOAPILOG(rainbow_code, agent_code, reference_no, serviceMethod, paramsJson.ToString(), result, RET_MSG);



			if (result.Length == 0)
			{
				if (RET_MSG.Length == 0)
					RET_MSG = "YTO getdistribute : API 응답값이 없습니다. YTO에 문의해주세요";

				return distribute_code;
			}


			try
			{
				JsonTextParser parser = new JsonTextParser();
				JsonObject obj = parser.Parse(result);
				JsonObjectCollection col = (JsonObjectCollection)obj;

				// 성공
				//{
				//    "data": {
				//        "distribute_code": "302-046-206"
				//    },
				//    "success": 1,
				//    "cnmessage": "获取分拣码成功",
				//    "enmessage": "Get distribute successfully"
				//}

				// 실패
				//{
				//    "success": 0,
				//    "cnmessage": "不存在的参考单号[20171220-2]",
				//    "enmessage": "The reference_no[20171220-2] does not exist"
				//}

				string code = (col["success"].GetValue() != null) ? col["success"].GetValue().ToString().Trim() : "";
				if (code.Equals("1"))  // 1=성공
				{
					JsonObject data = (col["data"].GetValue() != null) ? (JsonObject)col["data"] : null;
					if (data != null)
					{
						JsonObjectCollection success = (JsonObjectCollection)data;
						if (success["distribute_code"].GetValue() != null)
						{
							distribute_code = success["distribute_code"].GetValue().ToString().Trim();
						}
					}
				}
				else  // 0=실패
				{
					string enmessage = (col["enmessage"].GetValue() != null) ? col["enmessage"].GetValue().ToString().Trim() : "";
					string cnmessage = (col["cnmessage"].GetValue() != null) ? col["cnmessage"].GetValue().ToString().Trim() : "";

					RET_MSG = string.Format("YTO getdistribute : API RESULT : {0}", enmessage);
				}
			}
			catch (Exception ex)
			{
				RET_MSG = string.Format("YTO getdistribute : JSON error : {0} ({1})", ex.Message, -100);
			}



			return distribute_code;
		}


		// 라벨
		public bool GetNewLabel(string reference_no, ref Image label_image, ref string RET_MSG)
		{
			string serviceMethod = "getnewlabel";

			JsonObjectCollection labelInfo = new JsonObjectCollection();

			JsonObjectCollection configInfo_collecction = new JsonObjectCollection("configInfo");

			configInfo_collecction.Add(new JsonStringValue("lable_file_type", "1"));  //1:PNG, 2:PDF
																					  //configInfo_collecction.Add(new JsonStringValue("lable_paper_type", "3"));//1: label paper (8.05 * 9 cm), 2: A4 paper (21 * 29.7 cm), 3: label paper (10 * 10 cm), 4: domestic table (18 * 10 cm)
			configInfo_collecction.Add(new JsonStringValue("lable_paper_type", "4"));
			configInfo_collecction.Add(new JsonStringValue("lable_content_type", "1"));//1: label, 2: declaration form, 3: picking orders, 4: label + declaration, 5: label + pick up order, 6: label + declaration + delivery list

			JsonObjectCollection additional_collecction = new JsonObjectCollection("additional_info");//추가 구성 정보
			additional_collecction.Add(new JsonStringValue("lable_print_invoiceinfo", "Y"));// picking information - (Y: Print N: do not print) Default N: Do not print
			additional_collecction.Add(new JsonStringValue("lable_print_buyerid", "Y"));// buyer ID - (Y: Print N: do not print) Default N: Do not print
			additional_collecction.Add(new JsonStringValue("lable_print_datetime", "Y"));// date - (Y: Print N: do not print) Default N: Do not print
			additional_collecction.Add(new JsonStringValue("customsdeclaration_print_actualweight", "Y")); // actual weight(실제 무게가 안들어있으면 0.2kg default) - (Y: Print N: do not print) Default N: Do not print
			configInfo_collecction.Add(additional_collecction);

			labelInfo.Add(configInfo_collecction);
			JsonArrayCollection listorder_Info = new JsonArrayCollection("listorder");

			JsonObjectCollection itemJson = new JsonObjectCollection();
			itemJson.Add(new JsonStringValue("reference_no", reference_no));//주문번호 인거같음..
			listorder_Info.Add(itemJson);

			//JsonObjectCollection itemJson2 = new JsonObjectCollection();
			//itemJson2.Add(new JsonStringValue("reference_no", ""));//상품코드
			//listorder_Info.Add(itemJson2);

			labelInfo.Add(listorder_Info);

			string paramsJson = labelInfo.ToString();

			string result = string.Empty;

			//===================ServiceReference_YtoGlobal===================================
			//using (ServiceReference_YtoGlobal.PublicServiceSoapClient client = new ServiceReference_YtoGlobal.PublicServiceSoapClient())
			//{
				//result = client.ServiceEntrance(API_YTO_APP_TOKEN, API_YTO_APP_KEY, serviceMethod, paramsJson);
			//}
			//Console.Write(result);



			// YTO API 로그를 저장한다
			SetYTOAPILOG(RAINBOWCODE, "", reference_no, serviceMethod, paramsJson.ToString(), result, "");



			if (result.Length == 0)
			{
				RET_MSG = "YTO GetNewLabel API 응답값이 없습니다. YTO에 문의해주세요";
				return false;
			}
			//{"data":[{"lable_file_type":"2","lable_file":"http://47.90.107.70:8000/api-lable/pdf/20170412/62e785e6-839a-4e04-a856-f4ecba79ac41.pdf"}],"success":1,"cnmessage":"获取订单标签成功","enmessage":"Get order label successfully"}
			//{"data":[{"lable_file_type":"1","lable_file":"http://47.90.107.70:8000/api-lable/pdf/20170412/67eb2a9d-2e15-4acf-97dd-b44bd328a07c.png"}],"success":1,"cnmessage":"获取订单标签成功","enmessage":"Get order label successfully"} 4
			//{"data":[{"lable_file_type":"1","lable_file":"http://47.90.107.70:8000/api-lable/pdf/20170412/2adeab37-e825-4074-af11-0843a375ca1d.png"}],"success":1,"cnmessage":"获取订单标签成功","enmessage":"Get order label successfully"} 1
			try
			{
				JsonTextParser parser = new JsonTextParser();
				JsonObject obj = parser.Parse(result);
				JsonObjectCollection col = (JsonObjectCollection)obj;

				string code = (col["success"].GetValue() != null) ? col["success"].GetValue().ToString().Trim() : "";
				if (code.Equals("1"))
				{//성공
					JsonObject data = (col["data"].GetValue() != null) ? (JsonObject)col["data"] : null;

					if (data != null)
					{
						JsonArrayCollection label_files = (JsonArrayCollection)data;

						JsonObjectCollection label = (JsonObjectCollection)label_files[0];


						string label_type = "";
						if (label["lable_file_type"].GetValue() != null)
						{
							label_type = label["lable_file_type"].GetValue().ToString().Trim();
						}

						string label_file_url = "";
						if (label["lable_file"].GetValue() != null)
						{
							label_file_url = label["lable_file"].GetValue().ToString().Trim();

							System.Threading.Thread.Sleep(500);

							//String fileName = "d:/YTO_LABEL1.png";
							label_image = url_to_image(label_file_url);

						}

						return true;
					}

				}
				else//실패
				{
					string enmessage = (col["enmessage"].GetValue() != null) ? col["enmessage"].GetValue().ToString().Trim() : "";
					string cnmessage = (col["cnmessage"].GetValue() != null) ? col["cnmessage"].GetValue().ToString().Trim() : "";

					RET_MSG = string.Format("YTO GetNewLabel API RESULT : {0})", enmessage);
					return false;
				}

			}
			catch (Exception ex)
			{
				RET_MSG = string.Format("YTO GetNewLabel JSON error : {0} ({1})", ex.Message, -100);
				return false;
			}


			//{
			//    "data": [{
			//        "lable_file_type": "1",
			//        "lable_file": "http://47.90.107.70:8000/api-lable/pdf/20170329/de156a4a-1171-430a-9389-2240568ad9a2.png"
			//    }],
			//    "success": 1,
			//    "cnmessage": "获取订单标签成功",
			//    "enmessage": "Get order label successfully"
			//}


			return true;
		}


		//tracking조회 가능한 번호 가져오기
		public bool Gettrackingnumber(ref string request, ref string response, ref string RET_MSG)
		{
			string serviceMethod = "gettrackingnumber";


			JsonObjectCollection configInfo_collecction = new JsonObjectCollection();
			configInfo_collecction.Add(new JsonStringValue("reference_no", "12345678CN"));//주문번호

			string paramsJson = configInfo_collecction.ToString();
			request = paramsJson;
			string error_str = "";

			//string jsontext = GetAPIResult(method, orderscollecction.ToString(), ref error_str);

			string result = string.Empty;

			//===================ServiceReference_YtoGlobal===================================
			//using (ServiceReference_YtoGlobal.PublicServiceSoapClient client = new ServiceReference_YtoGlobal.PublicServiceSoapClient())
			//{
				//result = client.ServiceEntrance(API_YTO_APP_TOKEN, API_YTO_APP_KEY, serviceMethod, paramsJson);
			//}
			Console.Write(result);
			response = result;

			if (result.Length == 0)
			{
				RET_MSG = string.Format("YTO Gettrackingnumber API result error ({0})", error_str);


				return false;
			}


			try
			{
				JsonTextParser parser = new JsonTextParser();
				JsonObject obj = parser.Parse(result);
				JsonObjectCollection col = (JsonObjectCollection)obj;

				string code = (col["success"].GetValue() != null) ? col["success"].GetValue().ToString().Trim() : "";
				if (code.Equals("1"))
				{//성공
					JsonObject data = (col["data"].GetValue() != null) ? (JsonObject)col["data"] : null;

					if (data != null)
					{
						JsonArrayCollection success_data = (JsonArrayCollection)data;

						JsonObjectCollection success = (JsonObjectCollection)success_data[0];

						//배송조회 번호
						string shipping_method_no = "";
						if (success["shipping_method_no"].GetValue() != null)
						{
							shipping_method_no = success["shipping_method_no"].GetValue().ToString().Trim();
						}

						string enmessage = "";
						if (success["enmessage"].GetValue() != null)
						{
							enmessage = success["enmessage"].GetValue().ToString().Trim();
						}

						return true;
					}


					return true;
				}
				else//실패
				{
					string enmessage = (col["enmessage"].GetValue() != null) ? col["enmessage"].GetValue().ToString().Trim() : "";
					string cnmessage = (col["cnmessage"].GetValue() != null) ? col["cnmessage"].GetValue().ToString().Trim() : "";
				}

			}
			catch (Exception ex)
			{
				RET_MSG = string.Format("JSON error : {0} ({1})", ex.Message, -100);
				return false;
			}

			//{"success":0,"cnmessage":"获取的跟踪单号为空","enmessage":"Get Tracking Number is empty"}


			return false;
		}


		//tracking조회 
		public bool Gettrack(ref string request, ref string response, ref string RET_MSG)
		{
			string serviceMethod = "gettrack";


			JsonObjectCollection configInfo_collecction = new JsonObjectCollection();
			//configInfo_collecction.Add(new JsonStringValue("tracking_number", "G00016040882"));//트래킹번호
			configInfo_collecction.Add(new JsonStringValue("tracking_number", "G10100161795"));//트래킹번호

			string paramsJson = configInfo_collecction.ToString();
			request = paramsJson;
			string error_str = "";

			//string jsontext = GetAPIResult(method, orderscollecction.ToString(), ref error_str);

			string result = string.Empty;

			//===================ServiceReference_YtoGlobal===================================
			//using (ServiceReference_YtoGlobal.PublicServiceSoapClient client = new ServiceReference_YtoGlobal.PublicServiceSoapClient())
			//{
				//result = client.ServiceEntrance(API_YTO_APP_TOKEN, API_YTO_APP_KEY, serviceMethod, paramsJson);
			//}
			Console.Write(result);

			response = result;
			if (result.Length == 0)
			{
				RET_MSG = string.Format("YTO Gettrackingnumber API result error ({0})", error_str);
				return false;
			}



			try
			{
				JsonTextParser parser = new JsonTextParser();
				JsonObject obj = parser.Parse(result);
				JsonObjectCollection col = (JsonObjectCollection)obj;

				string code = (col["success"].GetValue() != null) ? col["success"].GetValue().ToString().Trim() : "";
				if (code.Equals("1"))
				{//성공

					JsonObject data = (col["data"].GetValue() != null) ? (JsonObject)col["data"] : null;

					if (data != null)
					{//{"data":[{"server_hawbcode":"G00016040882","destination_country":"CN","track_status":"NO","track_status_name":"操作正常","signatory_name":"","details":[{"track_occur_date":"2017-04-13 15:26:09","track_location":"","track_description":"快件電子信息已經收到"}]}],"success":1,"cnmessage":"获取跟踪记录成功","enmessage":"Get Track successfully"}
						JsonArrayCollection success_data = (JsonArrayCollection)data;

						JsonObjectCollection success = (JsonObjectCollection)success_data[0];


						string track_status_name = "";
						if (success["track_status_name"].GetValue() != null)
						{
							track_status_name = success["track_status_name"].GetValue().ToString().Trim();
						}

						//배송과정
						JsonObject details = (col["details"].GetValue() != null) ? (JsonObject)col["details"] : null;
						JsonArrayCollection detail_array = (JsonArrayCollection)details;

						List<Dictionary<string, string>> tracking_list = new List<Dictionary<string, string>>();
						if (detail_array.Count > 0)
						{
							for (int i = 0; i < detail_array.Count; i++)
							{
								JsonObjectCollection item = (JsonObjectCollection)detail_array[i];

								Dictionary<string, string> dic = new Dictionary<string, string>();
								dic.Add("track_occur_date", item["track_occur_date"].GetValue().ToString().Trim());
								dic.Add("track_location", item["track_location"].GetValue().ToString().Trim());
								dic.Add("track_description", item["track_description"].GetValue().ToString().Trim());

								tracking_list.Add(dic);
							}
						}

						return true;
					}

					return true;
				}
				else//실패
				{//{"success":0,"cnmessage":"获取的跟踪单号为空","enmessage":"Get Tracking Number is empty"}
					string enmessage = (col["enmessage"].GetValue() != null) ? col["enmessage"].GetValue().ToString().Trim() : "";
					string cnmessage = (col["cnmessage"].GetValue() != null) ? col["cnmessage"].GetValue().ToString().Trim() : "";
				}

			}
			catch (Exception ex)
			{
				RET_MSG = string.Format("JSON error : {0} ({1})", ex.Message, -100);
				return false;
			}
			return true;
		}



		public Image url_to_image(string uri)
		{
			WebClient wc = new WebClient();
			byte[] bytes = wc.DownloadData(uri);
			//MemoryStream ms = new MemoryStream(bytes);
			//System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
			System.Drawing.ImageConverter imgcvt = new System.Drawing.ImageConverter();
			System.Drawing.Image img = (System.Drawing.Image)imgcvt.ConvertFrom(bytes);

			Bitmap bm = new Bitmap(img);

			//Crop(bm, bm.Width, bm.Height - 29, 0, +90).Save(fileName, System.Drawing.Imaging.ImageFormat.Png);//378*553

			//img.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

			// 2017-04-17 jsy : 이미지를 자르지 않고 원래크기 그대로 출력하도록 한다
			//return Crop(bm, bm.Width, bm.Height - 29, 0, +90);
			return img;
		}



		static System.Drawing.Image Crop(System.Drawing.Image imgPhoto, int Width, int Height, int adjustX, int adjustY)
		{
			//비트맵 종이한장 만들기
			Bitmap bmPhoto = new Bitmap(Width - adjustX, Height - adjustY, PixelFormat.Format24bppRgb);
			bmPhoto.MakeTransparent();//배경 하얀색으로!
									  //그래픽 이미지 설정            
			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			int OriginalWidth = imgPhoto.Width;
			int OriginalHeight = imgPhoto.Height;

			//싹뚝 자르기
			grPhoto.DrawImage(imgPhoto,
				   new Rectangle(0, 0, Width, Height),
				   new Rectangle(adjustX, adjustY, Width, Height),
				   GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
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
						+ string.Format(", 'CN_YTO_GLOBAL'")
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










		//=========================================================================================
		//=========================================================================================
		// 2017-12-13 jsy
		// 트랙킹정보 전송하기
		// 항공기 출발 / 항공기 도착 / 통관중 / 통관완료 등의 트랙킹정보를
		// 배송조회 사이트에서 확인할수 있도록 트랙킹정보를 전송한다
		//=========================================================================================
		//=========================================================================================

		//string m_TrackEdiURL = "http://test.edi.ytoglobal.com/outerEdiTrack/receiveEdiTrack";  // Test Server
		string m_TrackEdiURL = "http://edi.ytoglobal.com/outerEdiTrack/receiveEdiTrack";  // Real Server


		// 트랙킹 정보 전송하기
		public bool receiveEdiTrack(ref List<YTOTrackResultModel> result_list, YTOTrackInfoModel track_info, ref string RET_MSG)
		{
			RET_MSG = "";

			XmlDocument xmldoc = new XmlDocument();

			XmlElement Message = xmldoc.CreateElement("Message");



			// Header
			XmlElement Header = xmldoc.CreateElement("Header");

			DateTime TIMESTAMP = DateTime.UtcNow.AddHours(8);  // 중국시간 : UTC + 8시간

			XmlElement SeqNo = xmldoc.CreateElement("SeqNo");
			SeqNo.InnerText = string.Format("ETM{0}{1}", RAINBOWCODE, TIMESTAMP.ToString("yyyyMMddHHmmss"));
			Header.AppendChild(SeqNo);

			XmlElement TimeStamp = xmldoc.CreateElement("TimeStamp");
			TimeStamp.InnerText = TIMESTAMP.ToString("yyyy-MM-dd HH:mm:ss");
			Header.AppendChild(TimeStamp);



			// Trackings
			XmlElement Trackings = xmldoc.CreateElement("Trackings");

			for (int i = 0; i < result_list.Count; i++)
			{
				XmlElement Tracking = xmldoc.CreateElement("Tracking");

				XmlElement WaybillNo = xmldoc.CreateElement("WaybillNo");
				WaybillNo.InnerText = result_list[i].WaybillNo;
				Tracking.AppendChild(WaybillNo);

				//XmlElement ReferenceOrderNo = xmldoc.CreateElement("ReferenceOrderNo");
				//ReferenceOrderNo.InnerText = "";
				//Tracking.AppendChild(ReferenceOrderNo);

				//XmlElement ReferenceChangeNo = xmldoc.CreateElement("ReferenceChangeNo");
				//ReferenceChangeNo.InnerText = "";
				//Tracking.AppendChild(ReferenceChangeNo);

				XmlElement EventCode = xmldoc.CreateElement("EventCode");
				EventCode.InnerText = track_info.EventCode;
				Tracking.AppendChild(EventCode);

				XmlElement EventDetail = xmldoc.CreateElement("EventDetail");
				EventDetail.InnerText = track_info.EventDetail;
				Tracking.AppendChild(EventDetail);

				XmlElement EventLocation = xmldoc.CreateElement("EventLocation");
				EventLocation.InnerText = track_info.EventLocation;
				Tracking.AppendChild(EventLocation);

				//XmlElement EventOperater = xmldoc.CreateElement("EventOperater");
				//EventOperater.InnerText = "";
				//Tracking.AppendChild(EventOperater);

				//XmlElement EventOperaterPhone = xmldoc.CreateElement("EventOperaterPhone");
				//EventOperaterPhone.InnerText = "";
				//Tracking.AppendChild(EventOperaterPhone);

				//XmlElement City = xmldoc.CreateElement("City");
				//City.InnerText = "";
				//Tracking.AppendChild(City);

				//XmlElement NextCity = xmldoc.CreateElement("NextCity");
				//NextCity.InnerText = "";
				//Tracking.AppendChild(NextCity);

				XmlElement EventTime = xmldoc.CreateElement("EventTime");
				EventTime.InnerText = track_info.EventTime;
				Tracking.AppendChild(EventTime);

				XmlElement ServiceCode = xmldoc.CreateElement("ServiceCode");
				ServiceCode.InnerText = API_YTO_CLIENT_ID;
				Tracking.AppendChild(ServiceCode);

				//XmlElement CountryCode = xmldoc.CreateElement("CountryCode");
				//CountryCode.InnerText = "";
				//Tracking.AppendChild(CountryCode);

				Trackings.AppendChild(Tracking);
			}

			Message.AppendChild(Header);
			Message.AppendChild(Trackings);

			string xml_str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Message.OuterXml;

			string error = "";
			string result = GetAPIResult(xml_str, ref error);
			if (result.Length == 0)
			{
				RET_MSG = error;
				return false;
			}



			// 성공
			//<?xml version="1.0" encoding="UTF-8"?>
			//<Message>
			//    <Header>
			//        <SeqNo>ETM0000120171213202217</SeqNo>
			//    </Header>
			//    <Response>
			//        <Trackings>
			//            <WaybillNo>G10100162403</WaybillNo>
			//            <EventCode>CC</EventCode>
			//            <Status>true</Status>
			//            <Message></Message>
			//        </Trackings>
			//        <Trackings>
			//            <WaybillNo>G10100162404</WaybillNo>
			//            <EventCode>CC</EventCode>
			//            <Status>true</Status>
			//            <Message></Message>
			//        </Trackings>
			//    </Response>
			//</Message>

			// 실패
			//<?xml version="1.0" encoding="UTF-8"?>
			//<Message>
			//    <Header>
			//        <SeqNo></SeqNo>
			//    </Header>
			//    <Response>
			//       <Status>false</Status>
			//       <Message>数字签名失败!</Message>
			//    </Response>
			//</Message>

			// 실패
			//<?xml version=\"1.0\" encoding=\"UTF-8\"?>
			//<Message>
			//    <Header>
			//        <SeqNo>ETM0000120171213203836</SeqNo>
			//    </Header>
			//    <Response>
			//        <Trackings>
			//            <WaybillNo>G10100162403</WaybillNo>
			//            <EventCode>CP</EventCode>
			//            <Status>false</Status>
			//            <Message>CP对应的圆通走件code不存在;</Message>
			//        </Trackings>
			//        <Trackings>
			//            <WaybillNo>G10100162404</WaybillNo>
			//            <EventCode>CP</EventCode>
			//            <Status>false</Status>
			//            <Message>CP对应的圆通走件code不存在;</Message>
			//        </Trackings>
			//    </Response>
			//</Message>

			bool Ret_Status = true;
			string Ret_Message = "";

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(result);

				XmlNode ret_message = doc.ChildNodes[1];
				XmlElement ret_response = ret_message["Response"];

				for (int i = 0; i < ret_response.ChildNodes.Count; i++)
				{
					XmlNode node = ret_response.ChildNodes[i];

					if (node.Name == "Status")  // error status
					{
						Ret_Status = GlobalFunction.GetBool(node.InnerText.Trim());
						continue;
					}
					else if (node.Name == "Message")  // error message
					{
						Ret_Message = node.InnerText.Trim();
						continue;
					}

					if (node.Name != "Trackings")
						continue;

					string t_WaybillNo = node["WaybillNo"].InnerText.Trim();
					string t_EventCode = node["EventCode"].InnerText.Trim();
					bool t_Status = GlobalFunction.GetBool(node["Status"].InnerText.Trim());
					string t_Message = node["Message"].InnerText.Trim();

					int index = result_list.FindIndex(m => m.WaybillNo == t_WaybillNo);
					if (index < 0)
						continue;

					result_list[index].EventCode = t_EventCode;
					result_list[index].Status = t_Status;
					result_list[index].Message = t_Message;
				}
			}
			catch (Exception ex1)
			{
				RET_MSG = string.Format("Xml Parser Error : {0}", ex1.Message);
				return false;
			}



			if (!Ret_Status || Ret_Message.Length > 0)  // api error
			{
				RET_MSG = string.Format("receiveEdiTrack Error : {0}", Ret_Message);
				return false;
			}

			return true;
		}


		public string GetAPIResult(string message, ref string error_str)
		{
			error_str = "";

			//string xml = "<order></order>";
			//string client_id = "123456";
			//string sign_plane = xml + client_id;

			//MD5 md5 = MD5.Create();
			//byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sign_plane);
			//byte[] newBuffer = md5.ComputeHash(bytes, 0, bytes.Length);

			// 방법 1 (32 bit)
			//StringBuilder sb = new StringBuilder();
			//for (int i = 0; i < newBuffer.Length; i++)
			//{
			//    sb.Append(newBuffer[i].ToString("X2").ToLower());
			//}
			//byte[] baseBuffer = Encoding.UTF8.GetBytes(sb.ToString());
			//string sign_encrypt = Convert.ToBase64String(baseBuffer);

			// 방법 2 (64 bit)
			//string sign_encrypt = Convert.ToBase64String(newBuffer);



			//string xml = "<order></order>";
			//string client_id = "123456";
			//string sign_plane2 = xml + client_id;
			//MD5 md52 = MD5.Create();
			//byte[] bytes2 = System.Text.Encoding.UTF8.GetBytes(sign_plane2);
			//byte[] newBuffer2 = md52.ComputeHash(bytes2, 0, bytes2.Length);
			//string sign_encrypt2 = Convert.ToBase64String(newBuffer2);
			//System.Windows.Forms.MessageBox.Show(sign_encrypt2);



			string sign_plane = message + API_YTO_CLIENT_ID;

			MD5 md5 = MD5.Create();
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sign_plane);
			byte[] newBuffer = md5.ComputeHash(bytes, 0, bytes.Length);
			string sign_encrypt = Convert.ToBase64String(newBuffer);

			string result = "";
			try
			{
				string param = string.Format("message={0}&sign={1}&client_id={2}", message, System.Web.HttpUtility.UrlEncode(sign_encrypt), API_YTO_CLIENT_ID);  // "+" 기호가 사라질수 있으므로 UrlEncode 해준다
				byte[] sign_bytes = Encoding.UTF8.GetBytes(param);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_TrackEdiURL);
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


		// 2017-12-13 jsy : 트랙킹 정보 가져오기
		public bool GetTracking(ref YTOTrackingModel tracking, ref string RET_MSG)
		{
			RET_MSG = "";

			string serviceMethod = "gettrack";

			JsonObjectCollection configInfo_collecction = new JsonObjectCollection();
			configInfo_collecction.Add(new JsonStringValue("tracking_number", tracking.WaybillNo));  // 트래킹번호

			string paramsJson = configInfo_collecction.ToString();

			string result = string.Empty;

			//===================ServiceReference_YtoGlobal===================================
			//using (ServiceReference_YtoGlobal.PublicServiceSoapClient client = new ServiceReference_YtoGlobal.PublicServiceSoapClient())
			//{
				//result = client.ServiceEntrance(API_YTO_APP_TOKEN, API_YTO_APP_KEY, serviceMethod, paramsJson);
			//}

			if (result.Length == 0)
			{
				RET_MSG = string.Format("gettrack Error");
				return false;
			}



			// 성공
			//{
			//    "data": [
			//        {
			//            "server_hawbcode": "G10100161795",
			//            "destination_country": "CN",
			//            "track_status": "NO",
			//            "track_status_name": "操作正常",
			//            "signatory_name": "",
			//            "details": [
			//                {
			//                    "track_occur_date": "2017-12-10 13:13:53",
			//                    "track_location": "广东省中山市京华",
			//                    "track_description": "PDA正常签收扫描 签收人: 他人代收"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-10 09:26:19",
			//                    "track_location": "广东省中山市京华",
			//                    "track_description": "派件扫描 派件人: 胡石炜"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-10 08:51:13",
			//                    "track_location": "广东省中山市京华",
			//                    "track_description": "下车扫描 下一站【广东省中山市京华】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-10 00:22:11",
			//                    "track_location": "广东省中山市",
			//                    "track_description": "装件入车扫描 下一站【广东省中山市京华】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-10 00:22:10",
			//                    "track_location": "广东省中山市",
			//                    "track_description": "装件入车扫描 下一站【广东省中山市京华】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-09 20:21:00",
			//                    "track_location": "江门转运中心",
			//                    "track_description": "装件入车扫描 下一站【广东省中山市】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-09 20:18:47",
			//                    "track_location": "江门转运中心",
			//                    "track_description": "下车扫描 下一站【江门转运中心】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-09 15:58:16",
			//                    "track_location": "佛山转运中心",
			//                    "track_description": "装件入车扫描 下一站【江门转运中心】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-09 15:56:25",
			//                    "track_location": "佛山转运中心",
			//                    "track_description": "下车扫描 下一站【佛山转运中心】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-07 20:28:05",
			//                    "track_location": "青岛转运中心",
			//                    "track_description": "装件入车扫描 下一站【佛山转运中心】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-07 20:26:46",
			//                    "track_location": "青岛转运中心",
			//                    "track_description": "下车扫描 下一站【青岛转运中心】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-07 19:18:29",
			//                    "track_location": "山东省青岛市即墨市",
			//                    "track_description": "装件入车扫描 下一站【青岛转运中心】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-07 19:08:40",
			//                    "track_location": "山东省青岛市即墨市",
			//                    "track_description": "下车扫描 下一站【山东省青岛市即墨市】"
			//                },
			//                {
			//                    "track_occur_date": "2017-12-06 16:04:20",
			//                    "track_location": "",
			//                    "track_description": "快件電子信息已經收到"
			//                }
			//            ]
			//        }
			//    ],
			//    "success": 1,
			//    "cnmessage": "获取跟踪记录成功",
			//    "enmessage": "Get Track successfully"
			//}

			// 실패
			//{
			//    "success":0,
			//    "cnmessage":"跟踪号码不存在",
			//    "enmessage":"Tracking number does not exist"
			//}

			try
			{
				JsonTextParser parser = new JsonTextParser();
				JsonObject obj = parser.Parse(result);
				JsonObjectCollection col = (JsonObjectCollection)obj;

				tracking.success = (col["success"] != null && col["success"].GetValue() != null) ? GlobalFunction.GetInt(col["success"].GetValue().ToString().Trim()) : 0;
				tracking.cnmessage = (col["cnmessage"] != null && col["cnmessage"].GetValue() != null) ? col["cnmessage"].GetValue().ToString().Trim() : "";
				tracking.enmessage = (col["enmessage"] != null && col["enmessage"].GetValue() != null) ? col["enmessage"].GetValue().ToString().Trim() : "";

				if (tracking.success == 0)
					return true;

				JsonObject obj2 = (col["data"] != null && col["data"].GetValue() != null) ? (JsonObject)col["data"] : null;
				if (obj2 == null)
				{
					tracking.success = 0;
					tracking.cnmessage = "트랙킹 데이터가 없습니다";
					tracking.enmessage = "";
					return true;
				}

				JsonArrayCollection data = (JsonArrayCollection)obj2;
				JsonObjectCollection col2 = (JsonObjectCollection)data[0];

				string server_hawbcode = (col2["server_hawbcode"] != null && col2["server_hawbcode"].GetValue() != null) ? col2["server_hawbcode"].GetValue().ToString().Trim() : "";
				string destination_country = (col2["destination_country"] != null && col2["destination_country"].GetValue() != null) ? col2["destination_country"].GetValue().ToString().Trim() : "";
				string track_status = (col2["track_status"] != null && col2["track_status"].GetValue() != null) ? col2["track_status"].GetValue().ToString().Trim() : "";
				string track_status_name = (col2["track_status_name"] != null && col2["track_status_name"].GetValue() != null) ? col2["track_status_name"].GetValue().ToString().Trim() : "";
				string signatory_name = (col2["signatory_name"] != null && col2["signatory_name"].GetValue() != null) ? col2["signatory_name"].GetValue().ToString().Trim() : "";

				JsonObject obj3 = (col2["details"] != null && col2["details"].GetValue() != null) ? (JsonObject)col2["details"] : null;
				if (obj3 == null)
				{
					tracking.success = 0;
					tracking.cnmessage = "트랙킹 데이터가 없습니다";
					tracking.enmessage = "";
					return true;
				}

				JsonArrayCollection details = (JsonArrayCollection)obj3;
				for (int i = 0; i < details.Count; i++)
				{
					JsonObjectCollection detail = (JsonObjectCollection)details[i];

					YTOTrackingDetailModel DETAIL = new YTOTrackingDetailModel();
					DETAIL.track_occur_date = (detail["track_occur_date"] != null && detail["track_occur_date"].GetValue() != null) ? detail["track_occur_date"].GetValue().ToString().Trim() : "";
					DETAIL.track_location = (detail["track_location"] != null && detail["track_location"].GetValue() != null) ? detail["track_location"].GetValue().ToString().Trim() : "";
					DETAIL.track_description = (detail["track_description"] != null && detail["track_description"].GetValue() != null) ? detail["track_description"].GetValue().ToString().Trim() : "";

					tracking.DETAIL.Add(DETAIL);
				}

				tracking.DETAIL.Reverse();  // reverse
			}
			catch (Exception ex1)
			{
				RET_MSG = string.Format("JSON Parser Error : {0}", ex1.Message);
				return false;
			}

			return true;
		}
	}
}
