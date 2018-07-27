using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_delvapi.Model
{
	class YTOLocalApiModel
	{
		// 송장번호 가져오기
		public string CLIENT_ID { get; set; }
		public string PARTNER_ID { get; set; }
		public string ORDERTYPE { get; set; }
		public string SENDER_POSTCODE { get; set; }
		public string SENDER_MOBILE { get; set; }
		public string SENDER_PROV { get; set; }
		public string SENDER_CITY { get; set; }
		public string SENDER_ADDR { get; set; }

		// 상태 업데이트
		public string CLIENT_ID2 { get; set; }
		public string PARTNER_ID2 { get; set; }


		public YTOLocalApiModel()
		{
			CLIENT_ID = "";
			PARTNER_ID = "";
			ORDERTYPE = "1";  // 1=일반, 5=YTO GLOBAL(중국내륙지점)
			SENDER_POSTCODE = "";
			SENDER_MOBILE = "";
			SENDER_PROV = "";
			SENDER_CITY = "";
			SENDER_ADDR = "";

			CLIENT_ID2 = "";
			PARTNER_ID2 = "";
		}
	}


	// 주문정보 업로드
	public class YTOLocalModel
	{
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string AGENTCODE { get; set; }  // 업체코드
		public string INVOICENO { get; set; }  // 시스템 송장번호
		public string DELVNO { get; set; }  // 배송번호(YTO)

		public string clientID { get; set; }  // customerId 와 동일
		public string logisticProviderID { get; set; }  // 물류회사ID ==> YTO 고정값
		public string customerId { get; set; }  // clientID 와 동일
		public string txLogisticID { get; set; }  // 주문번호(중복되지 않게)
		public string tradeNo { get; set; }  // 비즈니스 트랜잭션 번호 (선택 사항)
		public double totalServiceFee { get; set; }  // 보험 값 = insuranceValue * 상품 수량 (기본값은 0.0)
		public double codSplitFee { get; set; }  // 물류 회사 포인트 [COD] 실행 (일시적으로 사용되지 않음, 기본값은 0.0)
		public int orderType { get; set; }  // 주문 유형 (0-COD, 1- 정상 주문, 2- 휴대용 주문, 3- 반환 주문, 4- 지불)
		public int serviceType { get; set; }  // 서비스 유형 (1 - 집 수신, 2 - 다음날 4 - 아침 8 - 날짜, 0 - 연락처). 기본값은 0입니다.
		public int flag { get; set; }  // 주문 플래그, 기본값은 0, 의미가 없습니다.
		public string mailNo { get; set; }  // 물류청구서 번호
		public int type { get; set; }  // 주문 유형 (이 필드는 이전 버전과의 호환성을 위해 예약되어 있습니다)

		// sender
		public string sender_name { get; set; }  // 이름
		public string sender_postCode { get; set; }  // 우편번호
		public string sender_phone { get; set; }  // 전화번호
		public string sender_mobile { get; set; }  // 휴대폰번호
		public string sender_prov { get; set; }  // 성
		public string sender_city { get; set; }  // 시, 구
		public string sender_address { get; set; }  // 세부주소

		// receiver
		public string receiver_name { get; set; }  // 이름
		public string receiver_postCode { get; set; }  // 우편번호
		public string receiver_phone { get; set; }  // 전화번호
		public string receiver_mobile { get; set; }  // 휴대폰번호
		public string receiver_prov { get; set; }  // 성
		public string receiver_city { get; set; }  // 시, 구
		public string receiver_address { get; set; }  // 세부주소

		public string sendStartTime { get; set; }  // yyyy-MM-dd HH:mm:ss
		public string sendEndTime { get; set; }  // yyyy-MM-dd HH:mm:ss
		public double goodsValue { get; set; }  // 상품 및 배송비는 포함하지만 서비스 요금은 포함되지 않은 제품 금액
		public double totalValue { get; set; }  // goodsValue + 총 서비스 수수료
		public double agencyFund { get; set; }  // 수금액이 수금 순서 인 경우 수거 금액은 0보다 커야하며 비 수거는 0.0으로 설정되어야합니다.

		// items
		public double itemsValue { get; set; }  // 상품가격
		public double itemsWeight { get; set; }  // 총중량
		public List<YTOLocalItemsModel> ITEMS { get; set; }  // 상품정보

		public double insuranceValue { get; set; }  // 보험 금액의 금액 (보험 금액은 제품의 가치입니다 (100보다 크거나 같음 3w 미만), 기본값은 0.0입니다)
		public int special { get; set; }  // 제품 유형 (필드 유지, 일시적으로 보유하지 않음)
		public string remark { get; set; }  // 비고


		public YTOLocalModel()
		{
			RAINBOWCODE = "";  // 지점코드
			AGENTCODE = "";  // 업체코드
			INVOICENO = "";  // 시스템 송장번호
			DELVNO = "";  // 배송번호(YTO)

			clientID = "";  // customerId 와 동일
			logisticProviderID = "YTO";  // 물류회사ID ==> YTO 고정값
			customerId = "";  // clientID 와 동일
			txLogisticID = "";  // 주문번호(중복되지 않게)
			tradeNo = "1";  // 비즈니스 트랜잭션 번호 (선택 사항)
			totalServiceFee = 0.0;  // 보험 값 = insuranceValue * 상품 수량 (기본값은 0.0)
			codSplitFee = 0.0;  // 물류 회사 포인트 [COD] 실행 (일시적으로 사용되지 않음, 기본값은 0.0)
			orderType = 1;  // 주문 유형 (0-COD, 1- 정상 주문, 2- 휴대용 주문, 3- 반환 주문, 4- 지불)
			serviceType = 1;  // 서비스 유형 (1 - 집 수신, 2 - 다음날 4 - 아침 8 - 날짜, 0 - 연락처). 기본값은 0입니다.
			flag = 1;  // 주문 플래그, 기본값은 0, 의미가 없습니다.
			mailNo = "";  // 물류청구서 번호
			type = 0;  // 주문 유형 (이 필드는 이전 버전과의 호환성을 위해 예약되어 있습니다)

			// sender
			sender_name = "";  // 이름
			sender_postCode = "0";  // 우편번호
			sender_phone = "0";  // 전화번호
			sender_mobile = "";  // 휴대폰번호
			sender_prov = "";  // 성
			sender_city = "";  // 시, 구
			sender_address = "";  // 세부주소

			// receiver
			receiver_name = "";  // 이름
			receiver_postCode = "0";  // 우편번호
			receiver_phone = "0";  // 전화번호
			receiver_mobile = "";  // 휴대폰번호
			receiver_prov = "";  // 성
			receiver_city = "";  // 시, 구
			receiver_address = "";  // 세부주소

			sendStartTime = "";  // yyyy-MM-dd HH:mm:ss
			sendEndTime = "";  // yyyy-MM-dd HH:mm:ss
			goodsValue = 0.0;  // 상품 및 배송비는 포함하지만 서비스 요금은 포함되지 않은 제품 금액
			totalValue = 0.0;  // goodsValue + 총 서비스 수수료
			agencyFund = 0.0;  // 수금액이 수금 순서 인 경우 수거 금액은 0보다 커야하며 비 수거는 0.0으로 설정되어야합니다.

			// items
			itemsValue = 0.0;  // 상품가격
			itemsWeight = 0.0;  // 총중량
			ITEMS = new List<YTOLocalItemsModel>();  // 상품정보

			insuranceValue = 0.0;  // 보험 금액의 금액 (보험 금액은 제품의 가치입니다 (100보다 크거나 같음 3w 미만), 기본값은 0.0입니다)
			special = 0;  // 제품 유형 (필드 유지, 일시적으로 보유하지 않음)
			remark = "";  // 비고
		}
	}


	// 상품정보
	public class YTOLocalItemsModel
	{
		public string itemName { get; set; }  // 상품명
		public int number { get; set; }  // 상품갯수
		public double itemValue { get; set; }  // 단가 (2 자리 소수점)


		public YTOLocalItemsModel()
		{
			itemName = "";  // 상품명
			number = 0;  // 상품갯수
			itemValue = 0.0;  // 단가 (2 자리 소수점)
		}
	}


	// 주문정보 업로드 결과
	public class YTOLocalResultModel
	{
		public string logisticProviderID { get; set; }
		public string txLogisticID { get; set; }  // 주문번호
		public string clientID { get; set; }

		public string code { get; set; }  // 200 : 성공
		public bool success { get; set; }  // true : 성공
		public string reason { get; set; }  // 오류내용

		public string mailNo { get; set; }  // YTO번호
		public string shortAddress { get; set; }  // 지역코드
		public string consigneeBranchCode { get; set; }
		public string packageCenterCode { get; set; }
		public string packageCenterName { get; set; }


		public YTOLocalResultModel()
		{
			logisticProviderID = "";
			txLogisticID = "";  // 주문번호
			clientID = "";

			code = "0";  // 200 : 성공
			success = false;  // true : 성공
			reason = "";  // 오류내용

			mailNo = "";  // YTO번호
			shortAddress = "";  // 지역코드
			consigneeBranchCode = "";
			packageCenterCode = "";
			packageCenterName = "";
		}
	}










	// 트랙킹 데이터 가져오기
	public class YTOLocalTrackingModel
	{
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string INVOICENO { get; set; }  // 시스템 송장번호

		public string WaybillNo { get; set; }
		public bool success { get; set; }
		public string reason { get; set; }

		public List<YTOLocalTrackingDetailModel> DETAIL { get; set; }


		public YTOLocalTrackingModel()
		{
			RAINBOWCODE = "";  // 지점코드
			INVOICENO = "";  // 시스템 송장번호

			WaybillNo = "";
			success = false;
			reason = "";

			DETAIL = new List<YTOLocalTrackingDetailModel>();
		}
	}


	// 트랙킹 상세 데이터
	public class YTOLocalTrackingDetailModel
	{
		public string Upload_Time { get; set; }
		public string ProcessInfo { get; set; }


		public YTOLocalTrackingDetailModel()
		{
			Upload_Time = "";
			ProcessInfo = "";
		}
	}










	// 배송정보 추가하기
	public class YTOLocalTrackingInfoModel
	{
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string MASTERNO { get; set; }  // 마스터 번호

		public string SeqNo { get; set; }
		public string SendTimeStamp { get; set; }

		public List<YTOLocalTrackingInfoDetailModel> DETAIL { get; set; }


		public YTOLocalTrackingInfoModel()
		{
			RAINBOWCODE = "";  // 지점코드
			MASTERNO = "";  // 마스터 번호

			SeqNo = "";
			SendTimeStamp = "";

			DETAIL = new List<YTOLocalTrackingInfoDetailModel>();
		}
	}


	// 배송정보 추가할 배송번호 목록
	public class YTOLocalTrackingInfoDetailModel
	{
		public string ClientID { get; set; }
		public string LogisticProviderID { get; set; }
		public string TrackingInfoProvider { get; set; }
		public string BillID { get; set; }
		public string OrderID { get; set; }
		public string DepName { get; set; }
		public string CreateDateTime { get; set; }
		public string StatusCode { get; set; }
		public string StatusDesc { get; set; }
		public string FacilityType { get; set; }
		public string FacilityName { get; set; }
		public string Contacter { get; set; }
		public string ContactInfo { get; set; }
		public string Remark { get; set; }


		public YTOLocalTrackingInfoDetailModel()
		{
			ClientID = "";
			LogisticProviderID = "YTO";
			TrackingInfoProvider = "";
			BillID = "";
			OrderID = "";
			DepName = "";
			CreateDateTime = "";
			StatusCode = "";
			StatusDesc = "";
			FacilityType = "1";
			FacilityName = "";
			Contacter = "";
			ContactInfo = "";
			Remark = "";
		}
	}


	// 배송정보 추가하기 결과
	public class YTOLocalTrackingInfoResultModel
	{
		public string SeqNo { get; set; }
		public string SendTimeStamp { get; set; }
		public string ResultFlag { get; set; }
		public string ErrCode { get; set; }
		public string ErrMsg { get; set; }


		public YTOLocalTrackingInfoResultModel()
		{
			SeqNo = "";
			SendTimeStamp = "";
			ResultFlag = "";
			ErrCode = "";
			ErrMsg = "";
		}
	}
}
