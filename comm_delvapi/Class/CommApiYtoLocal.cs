using comm_delvapi.DB;
using comm_delvapi.Model;
using comm_global;
using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace comm_delvapi.Class
{
	class CommApiYtoLocal
	{
		// 2018-06-01 jsy : WMS에서 중국YTO 내륙 송장번호 가져오기 - 1개
		public static string GetYTOLocalWaybillNo_One(
					string rainbow_code
					, string agent_code
					, string invoice_no
					, bool is_custom_data
					, ref string error_str
					, YTOLocalApiModel API_INFO
					, List<CurrencyModels> CURRENCY_DATA = null
					)
		{
			error_str = "";



			// 중국YTO 내륙 API 연동에 필요한 값을 체크한다
			if (API_INFO.CLIENT_ID.Length == 0 || API_INFO.PARTNER_ID.Length == 0)
			{
				error_str = "환경설정에서 API 연동에 필요한 값을 먼저 설정해야 합니다.";
				return "";
			}



			// 송장데이터를 가져온다
			string POSTFIX = (is_custom_data ? "_like" : "");  // _like : 통관용데이터

			//*************송장 정보 수정 시 사용 하는 DB 모델을 작성 한 후 가져와야함*******************//
			OrdMaster INVOICE = new OrdMaster(); // InvoiceDatabase.GetInvoiceData(rainbow_code, agent_code, invoice_no, POSTFIX);
			if (INVOICE == null)
			{
				error_str = string.Format("{0} : ", invoice_no) + comm_global.Language.Resources.API_DATABASE_ERROR;
				return "";
			}
			if (INVOICE.WAYBILLNO.Length == 0)
			{
				error_str = string.Format("{0} : ", invoice_no) + comm_global.Language.Resources.API_NO_DATA;
				return "";
			}



			string WaybillNo = GetYTOLocalWaybillNo(ref INVOICE, ref error_str, API_INFO, CURRENCY_DATA);
			if (WaybillNo.Length == 0)
			{
				return "";
			}

			// 가져온 배송번호 저장
			//*************송장 정보 수정 시 사용 하는 DB 모델을 작성 한 후 가져와야함*******************//
			//if (!InvoiceDatabase.Save_YTOWaybillNo(INVOICE.EST_CODE, INVOICE.WAYBILLNO, INVOICE.DELVNO, INVOICE.SDATA1, INVOICE.SDATA2, INVOICE.DELV_COM))
			if (1==1)
			{
				error_str = "배송번호 저장을 실패하였습니다.";
				return "";
			}

			return WaybillNo;  // YTO 송장번호 리턴
		}


		// 2018-06-01 jsy : 중국YTO 내륙 송장번호 가져오기 - 여러개
		public static int GetYTOLocalWaybillNo_Multi(
					ref List<OrdMaster> INVOICE_LIST
					, ref string error_str
					)
		{
			error_str = "";

			int success = 0;

			// 송장데이터 체크
			if (INVOICE_LIST == null || INVOICE_LIST.Count == 0)
			{
				error_str = comm_global.Language.Resources.API_NO_DATA;
				return success;
			}

			// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
			Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(INVOICE_LIST[0].EST_CODE, "_api_cn_yto_local_");
			if (SETTINGS_DIC == null)
				SETTINGS_DIC = new Dictionary<string, string>();

			YTOLocalApiModel model = new YTOLocalApiModel();
			model.CLIENT_ID = (SETTINGS_DIC.ContainsKey("_api_cn_yto_local_client_id") ? SETTINGS_DIC["_api_cn_yto_local_client_id"] : "");
			model.PARTNER_ID = (SETTINGS_DIC.ContainsKey("_api_cn_yto_local_partner_id") ? SETTINGS_DIC["_api_cn_yto_local_partner_id"] : "");
			model.ORDERTYPE = (SETTINGS_DIC.ContainsKey("_api_cn_yto_local_order_type") ? SETTINGS_DIC["_api_cn_yto_local_order_type"] : "1");
			model.SENDER_POSTCODE = (SETTINGS_DIC.ContainsKey("_api_cn_yto_local_sender_postcode") ? SETTINGS_DIC["_api_cn_yto_local_sender_postcode"] : "");
			model.SENDER_MOBILE = (SETTINGS_DIC.ContainsKey("_api_cn_yto_local_sender_mobile") ? SETTINGS_DIC["_api_cn_yto_local_sender_mobile"] : "");
			model.SENDER_PROV = (SETTINGS_DIC.ContainsKey("_api_cn_yto_local_sender_prov") ? SETTINGS_DIC["_api_cn_yto_local_sender_prov"] : "");
			model.SENDER_CITY = (SETTINGS_DIC.ContainsKey("_api_cn_yto_local_sender_city") ? SETTINGS_DIC["_api_cn_yto_local_sender_city"] : "");
			model.SENDER_ADDR = (SETTINGS_DIC.ContainsKey("_api_cn_yto_local_sender_addr") ? SETTINGS_DIC["_api_cn_yto_local_sender_addr"] : "");

			// 중국YTO 내륙 API 연동에 필요한 값을 체크한다
			if (model.CLIENT_ID.Length == 0 || model.PARTNER_ID.Length == 0)
			{
				error_str = "환경설정에서 API 연동에 필요한 값을 먼저 설정해야 합니다.";
				return success;
			}

			// 환율 데이터를 가져온다
			List<CurrencyModels> CURRENCY_LIST = ConfigDatabase.GetCurrencyDataList(INVOICE_LIST[0].EST_CODE);



			for (int i = 0; i < INVOICE_LIST.Count; i++)
			{
				OrdMaster INVOICE = INVOICE_LIST[i];
				string err1 = "";
				string DELVNO = GetYTOLocalWaybillNo(ref INVOICE, ref err1, model, CURRENCY_LIST);
				INVOICE_LIST[i] = INVOICE;

				if (DELVNO.Length == 0)
					error_str += err1 + "\n";
				else
					success++;
			}

			return success;
		}


		// 2018-06-01 jsy : 중국YTO 내륙 송장번호를 API를 사용하여 가져온다
		public static string GetYTOLocalWaybillNo(
					ref OrdMaster INVOICE
					, ref string error_str
					, YTOLocalApiModel API_INFO
					, List<CurrencyModels> CURRENCY_DATA = null
					)
		{
			error_str = "";

			// 중국YTO 내륙 API 연동에 필요한 값을 체크한다
			if (API_INFO.CLIENT_ID.Length == 0 || API_INFO.PARTNER_ID.Length == 0)
			{
				error_str = "환경설정에서 API 연동에 필요한 값을 먼저 설정해야 합니다.";
				return "";
			}

			// 환율 데이터를 가져온다
			List<CurrencyModels> CURRENCY_LIST = CURRENCY_DATA;
			if (CURRENCY_LIST == null)
				CURRENCY_LIST = ConfigDatabase.GetCurrencyDataList(INVOICE.EST_CODE);



			//=====================================================================================
			// 1. 배송번호가 설정되었는지 체크한다
			//=====================================================================================

			if (INVOICE.DELVNO.Length > 0)
			{
				error_str = string.Format("{0} : ", INVOICE.WAYBILLNO) + comm_global.Language.Resources.API_SHIPPING_NUMBER_ALREADY_SET;
				return "";
			}



			//=====================================================================================
			// 2. YTO API에 전달할 데이터를 생성한다
			//=====================================================================================

			if (INVOICE.GoodsList.Count == 0)
			{
				error_str = string.Format("{0} : ", INVOICE.WAYBILLNO) + comm_global.Language.Resources.API_WE_CANNOT_GET_PRODUCT_INFO;
				return "";
			}

			DateTime UPLOAD_TIME = DateTime.UtcNow.AddHours(8);  // 중국시간 설정

			YTOLocalModel model = new YTOLocalModel();
			model.RAINBOWCODE = INVOICE.EST_CODE;
			model.AGENTCODE = INVOICE.ESE_CODE;
			model.INVOICENO = INVOICE.WAYBILLNO;
			//model.clientID = "";  // customerId 와 동일
			//model.logisticProviderID = "YTO";  // 물류회사ID ==> YTO 고정값
			//model.customerId = "";  // clientID 와 동일
			model.txLogisticID = string.Format("{0}{1}", GlobalSettings.ORDERNO_PREFIX, Regex.Replace(INVOICE.WAYBILLNO, @"[^a-zA-Z0-9-]", ""));  // 주문번호(중복되지 않게)
			//model.tradeNo = "1";  // 비즈니스 트랜잭션 번호 (선택 사항)
			//model.totalServiceFee = 0.0;  // 보험 값 = insuranceValue * 상품 수량 (기본값은 0.0)
			//model.codSplitFee = 0.0;  // 물류 회사 포인트 [COD] 실행 (일시적으로 사용되지 않음, 기본값은 0.0)
			//model.orderType = 1;  // 주문 유형 (0-COD, 1- 정상 주문, 2- 휴대용 주문, 3- 반환 주문, 4- 지불)
			//model.serviceType = 1;  // 서비스 유형 (1 - 집 수신, 2 - 다음날 4 - 아침 8 - 날짜, 0 - 연락처). 기본값은 0입니다.
			//model.flag = 1;  // 주문 플래그, 기본값은 0, 의미가 없습니다.
			//model.mailNo = "";  // 물류청구서 번호
			//model.type = 0;  // 주문 유형 (이 필드는 이전 버전과의 호환성을 위해 예약되어 있습니다)

			// sender
			model.sender_name = INVOICE.SENDER_NAME;  // 이름
			model.sender_postCode = API_INFO.SENDER_POSTCODE;  // 우편번호
			model.sender_phone = "0";  // 전화번호
			model.sender_mobile = API_INFO.SENDER_MOBILE;  // 휴대폰번호
			model.sender_prov = API_INFO.SENDER_PROV;  // 성
			model.sender_city = API_INFO.SENDER_CITY;  // 시, 구
			model.sender_address = API_INFO.SENDER_ADDR;  // 세부주소

			// 중국주소를 자동으로 성,시,구 로 나눈다
			string state = "";
			string city = "";
			string district = "";
			string addr2 = "";
			string zipcode = "";
			if (!SettingsDatabase.SplitAddress_CN((INVOICE.RECEIVER_ADDR1 + INVOICE.RECEIVER_ADDR2), ref state, ref city, ref district, ref addr2, ref zipcode, true))
			{
				state = INVOICE.RECEIVER_STATE;
				city = INVOICE.RECEIVER_CITY;
				district = INVOICE.RECEIVER_DISTRICT;
				addr2 = INVOICE.RECEIVER_ADDR2;
				zipcode = INVOICE.RECEIVER_ZIPCODE;
			}

			// receiver
			model.receiver_name = INVOICE.RECEIVER_NAME;  // 이름
			model.receiver_postCode = zipcode;  // 우편번호
			model.receiver_phone = "0";  // 전화번호
			model.receiver_mobile = INVOICE.RECEIVER_TELNO;  // 휴대폰번호
			model.receiver_prov = state;  // 성
			model.receiver_city = ((city.Length > 0 && district.Length > 0) ? string.Format("{0},{1}", city, district) : "");  // 시, 구
			model.receiver_address = addr2;  // 세부주소

			model.sendStartTime = UPLOAD_TIME.ToString("yyyy-MM-dd HH:mm:ss");
			model.sendEndTime = UPLOAD_TIME.ToString("yyyy-MM-dd HH:mm:ss");
			//model.goodsValue = 0.0;  // 상품 및 배송비는 포함하지만 서비스 요금은 포함되지 않은 제품 금액
			//model.totalValue = 0.0;  // goodsValue + 총 서비스 수수료
			//model.agencyFund = 0.0;  // 수금액이 수금 순서 인 경우 수거 금액은 0보다 커야하며 비 수거는 0.0으로 설정되어야합니다.

			string BRANCH_CURR_UNIT = SettingsDatabase.GetCurrencyUnit(INVOICE.EST_CODE);  // 지점 화폐단위

			// items
			for (int i = 0; i < INVOICE.GoodsList.Count; i++)  // 상품정보
			{
				// 단가는 CNY로 변환한다
				double UNIT_PRICE_CNY = ConfigDatabase.ExchangePrice(
											INVOICE.EST_CODE
											, BRANCH_CURR_UNIT
											, INVOICE.GoodsList[i].PRICE
											, INVOICE.CURRENCY_GOODS
											, "CNY"
											, CURRENCY_LIST
											);

				YTOLocalItemsModel ITEM = new YTOLocalItemsModel();
				ITEM.itemName = INVOICE.GoodsList[i].GOODS_NAME;  // 상품명
				ITEM.number = INVOICE.GoodsList[i].QTY;  // 갯수
				ITEM.itemValue = UNIT_PRICE_CNY;  // 단가(CNY)

				model.ITEMS.Add(ITEM);
			}

			model.itemsWeight = INVOICE.GetWeightKG();  // 총중량(kg)
														//model.itemsValue = ISSUE.IssueDetails.Sum(m => m.QTY * m.PRICE);  // 상품가격
			model.itemsValue = model.ITEMS.Sum(m => m.number * m.itemValue);  // 상품가격
																			  //model.insuranceValue = 0.0;  // 보험 금액의 금액 (보험 금액은 제품의 가치입니다 (100보다 크거나 같음 3w 미만), 기본값은 0.0입니다)
																			  //model.special = 0;  // 제품 유형 (필드 유지, 일시적으로 보유하지 않음)
																			  //model.remark = "";  // 비고



			//API_YTO_LOCAL YTOL = new API_YTO_LOCAL(API_YTO_LOCAL.YTO_APIType.TEST, "", "");
			API_YTO_LOCAL YTOL = new API_YTO_LOCAL(API_YTO_LOCAL.YTO_APIType.REAL
												  , API_INFO.CLIENT_ID
												  , API_INFO.PARTNER_ID
												  , ""
												  , ""
												  , API_INFO.ORDERTYPE
												  );

			// 주문등록 API
			YTOLocalResultModel RESULT = new YTOLocalResultModel();
			if (!YTOL.OrderAdd(model, ref RESULT, ref error_str))
			{
				return "";
			}

			INVOICE.DELVNO = RESULT.mailNo;
			INVOICE.DELV_COM = "CN_YTO_LOCAL";
			INVOICE.SDATA1 = RESULT.shortAddress;
			INVOICE.SDATA2 = RESULT.packageCenterCode;

			return RESULT.mailNo;  // YTO 송장번호 리턴
		}
	}
}
