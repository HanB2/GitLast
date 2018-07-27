using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_delvapi.Model
{

	// 배송가능국가 설정
	public class ShippingCountryModels
	{
		public int SEQNO { get; set; }  // 순번
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string NATION_CODE { get; set; }  // 국가코드
		public string NATION_NAME { get; set; }  // 국가명
		public string WEIGHT_UNIT { get; set; }  // 무게단위
		public string CURRENCY_UNIT { get; set; }  // 화페단위


		public ShippingCountryModels()
		{
			SEQNO = 0;  // 순번
			RAINBOWCODE = "";  // 지점코드
			NATION_CODE = "";  // 국가코드
			NATION_NAME = "";  // 국가명
			WEIGHT_UNIT = "";  // 무게단위
			CURRENCY_UNIT = "";  // 화페단위
		}
	}


	public class NATION
	{
		public string NATIONNO = "";
		public string NATIONNAME = "";
		public string NATIONNAME_ko_KR = "";
		public string NATIONNAME_zh_CN = "";
	}










	// 송장번호 권역대 설정
	public class InvoiceRangeModels
	{
		public int SEQNO { get; set; }  // 순번
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string HBLNO_TYPE { get; set; }  // 송장번호 종류(YTO, ZTO ...)
		public string HBLNO_START { get; set; }  // 시작번호
		public string HBLNO_END { get; set; }  // 끝번호
		public string HBLNO_CURRENT { get; set; }  // 마지막 사용번호
		public int DIGIT { get; set; }  // 송장번호 전체 자릿수
		public string PREFIX { get; set; }  // 접두어
		public string POSTFIX { get; set; }  // 접미어
		public string CREATETIME { get; set; }  // 생성일자
		public string UPDATETIME { get; set; }  // 마지막 사용일자
		public string USE_YN { get; set; }  // y=사용함, n=사용안함
		public string HBLNO_TYPE_EX { get; set; }  // 송장번호 종류 추가(N:일반, D:다이비끼 ...)
		public string MEMO { get; set; }  // 메모


		public InvoiceRangeModels()
		{
			SEQNO = 0;  // 순번
			RAINBOWCODE = "";  // 지점코드
			HBLNO_TYPE = "";  // 송장번호 종류(YTO, ZTO ...)
			HBLNO_START = "";  // 시작번호
			HBLNO_END = "";  // 끝번호
			HBLNO_CURRENT = "";  // 마지막 사용번호
			DIGIT = 0;  // 송장번호 전체 자릿수
			PREFIX = "";  // 접두어
			POSTFIX = "";  // 접미어
			CREATETIME = "";  // 생성일자
			UPDATETIME = "";  // 마지막 사용일자
			USE_YN = "";  // y=사용함, n=사용안함
			HBLNO_TYPE_EX = "";  // 송장번호 종류 추가(N:일반, D:다이비끼 ...)
			MEMO = "";  // 메모
		}
	}


	// AirportLog 송장번호 사용/미사용 갯수
	public class InvoiceRangeUsedModels
	{
		public string RAINBOWCODE { get; set; }  // 지점코드
		public int RANGE_SEQNO { get; set; }  // 송장번호 권역대 순번
		public string USE_YN { get; set; }  // y=사용함, n=사용안함
		public int COUNT { get; set; }  // 갯수


		public InvoiceRangeUsedModels()
		{
			RAINBOWCODE = "";  // 지점코드
			RANGE_SEQNO = 0;  // 송장번호 권역대 순번
			USE_YN = "";  // y=사용함, n=사용안함
			COUNT = 0;  // 갯수
		}
	}










	// 기준통화 설정
	public class CurrencyModels
	{
		public int SEQNO { get; set; }  // 순번
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string CURRENCY_UNIT { get; set; }  // 기준통화 종류(USD, CNY, KRW, EUR ...)
		public string CURRENCY_SYMBOL { get; set; }  // 기준통화 기호($, ¥, ₩, € ...)
		public double BASIC_UNIT { get; set; }  // 기준 단위
		public string MEMO { get; set; }  // 메모


		// Join해서 가져오는 Field
		public double EXCHANGE_RATE { get; set; }  // 최근 환율
		public string DATETIME_UPD { get; set; }  // 최근 환율 저장날짜


		public CurrencyModels()
		{
			SEQNO = 0;  // 순번
			RAINBOWCODE = "";  // 지점코드
			CURRENCY_UNIT = "";  // 기준통화 종류(USD, CNY, KRW, EUR ...)
			CURRENCY_SYMBOL = "";  // 기준통화 기호($, ¥, ₩, € ...)
			BASIC_UNIT = 1.0;  // 기준 단위
			MEMO = "";  // 메모


			EXCHANGE_RATE = 0.0;  // 최근 환율
			DATETIME_UPD = "";  // 최근 환율 저장날짜
		}
	}










	// 공항코드 설정
	public class AirportModels
	{
		public int SEQNO { get; set; }  // 순번
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string NATION_CODE { get; set; }  // 국가코드
		public string AIRPORT_CODE { get; set; }  // 공항코드
		public string AIRPORT_NAME { get; set; }  // 공항이름
		public string AIRPORT_LOCATION { get; set; }  // 공항위치


		// Join 해서 가져오는 Field
		public string NATION_NAME { get; set; }  // 국가명


		public AirportModels()
		{
			SEQNO = 0;  // 순번
			RAINBOWCODE = "";  // 지점코드
			NATION_CODE = "";  // 국가코드
			AIRPORT_CODE = "";  // 공항코드
			AIRPORT_NAME = "";  // 공항이름
			AIRPORT_LOCATION = "";  // 공항위치


			NATION_NAME = "";  // 국가명
		}
	}










	// 현지배송회사 설정
	public class LocalDeliveryModels
	{
		public int SEQNO { get; set; }  // 순번
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string NATION_CODE { get; set; }  // 국가코드
		public string NAME { get; set; }  // 배송회사 이름
		public string HOMEPAGE { get; set; }  // 배송회사 홈페이지
		public string COM_ID { get; set; }  // 배송회사 ID
		public string COMMENT { get; set; }  // 설명
		public string USER_INPUT { get; set; }  // 사용자가 엑셀파일에 입력하는 택배회사 이름(zto, yto ...)


		public LocalDeliveryModels()
		{
			SEQNO = 0;  // 순번
			RAINBOWCODE = "";  // 지점코드
			NATION_CODE = "";  // 국가코드
			NAME = "";  // 배송회사 이름
			HOMEPAGE = "";  // 배송회사 홈페이지
			COM_ID = "";  // 배송회사 ID
			COMMENT = "";  // 설명
			USER_INPUT = "";  // 사용자가 엑셀파일에 입력하는 택배회사 이름(zto, yto ...)
		}
	}










	// 출고타입 설정
	public class OutPutTypeModels
	{
		public int SEQNO { get; set; }  // 순번
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string NATION_CODE { get; set; }  // 국가코드
		public string OUTPUT_TYPE { get; set; }  // 출고타입
		public string OUTPUT_CODE { get; set; }  // 출고타입 코드
		public string OUTPUT_DESC { get; set; }  // 출고타입 설명
		public string DELV_CODE { get; set; }  // 출고타입 기호


		// 기타
		public string DELV_TYPE_NAME { get; set; }  // 출고타입명


		public OutPutTypeModels()
		{
			SEQNO = 0;  // 순번
			RAINBOWCODE = "";  // 지점코드
			NATION_CODE = "";  // 국가코드
			OUTPUT_TYPE = "";  // 출고타입
			OUTPUT_CODE = "";  // 출고타입 코드
			OUTPUT_DESC = "";  // 출고타입 설명
			DELV_CODE = "";  // 출고타입 기호


			// 기타
			DELV_TYPE_NAME = "";  // 출고타입명
		}
	}










	// 배송회사 구역설정
	public class DeliveryZoneModels
	{
		public int SEQNO { get; set; }  // 순번
		public string RAINBOWCODE { get; set; }  // 지점코드
		public string NATION_CODE { get; set; }  // 배송국가 코드
		public string COM_ID { get; set; }  // 배송회사 ID
		public string STATE { get; set; }  // 지역(성)
		public int ZONE { get; set; }  // 구역번호


		public DeliveryZoneModels()
		{
			SEQNO = 0;  // 순번
			RAINBOWCODE = "";  // 지점코드
			NATION_CODE = "";  // 배송국가 코드
			COM_ID = "";  // 배송회사 ID
			STATE = "";  // 지역(성)
			ZONE = 0;  // 구역번호
		}
	}
}
