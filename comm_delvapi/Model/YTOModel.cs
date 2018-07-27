using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_delvapi.Model
{
	public class YTOModel
	{
		public string reference_no { get; set; }
		public string shipping_method { get; set; }
		public string order_weight { get; set; }
		public string shipper_name { get; set; }
		public string shipper_countrycode { get; set; }
		public string shipper_street { get; set; }
		public string shipper_telephone { get; set; }
		public string shipper_mobile { get; set; }
		public string consignee_name { get; set; }
		public string consignee_countrycode { get; set; }
		public string consignee_province { get; set; }
		public string consignee_city { get; set; }
		public string consignee_district { get; set; }
		public string consignee_street { get; set; }
		public string consignee_postcode { get; set; }
		public string consignee_telephone { get; set; }
		public string consignee_mobile { get; set; }
		public string consignee_certificatetype { get; set; }
		public string consignee_certificatecode { get; set; }
		public List<YTO_INVOICE_MODEL> invoiceList { get; set; }

		public string RAINBOWCODE { get; set; }  // 지점코드
		public string AGENTCODE { get; set; }  // 업체코드
		public string INVOICENO { get; set; }  // 시스템 송장번호
		public string DELVNO { get; set; }  // 배송번호(YTO)


		public YTOModel()
		{
			invoiceList = new List<YTO_INVOICE_MODEL>();
			shipping_method = "KR-IMP";//getshippingmethod를 이용해 받은값 (한국-중국)
			shipper_countrycode = "KR";//발송인 국가코드
			consignee_countrycode = "CN";//수취인 국가코드      

			RAINBOWCODE = "";  // 지점코드
			AGENTCODE = "";  // 업체코드
			INVOICENO = "";  // 시스템 송장번호
			DELVNO = "";  // 배송번호(YTO)
		}
	}

	public class YTO_INVOICE_MODEL
	{
		public string invoice_enname { get; set; }
		public string invoice_cnname { get; set; }
		public int invoice_quantity { get; set; }
		public double invoice_unitcharge { get; set; }
	}


	// 2017-12-13 jsy
	// YTO GLOBAL 에 전송할 트랙킹정보
	public class YTOTrackInfoModel
	{
		public string EventCode { get; set; }
		public string EventDetail { get; set; }
		public string EventLocation { get; set; }
		public string EventTime { get; set; }


		public YTOTrackInfoModel()
		{
			EventCode = "";
			EventDetail = "";
			EventLocation = "";
			EventTime = "";
		}
	}


	// 2017-12-13 jsy
	// YTO GLOBAL 트랙킹정보 전송 결과
	public class YTOTrackResultModel
	{
		public string WaybillNo { get; set; }
		public string EventCode { get; set; }
		public bool Status { get; set; }
		public string Message { get; set; }


		public YTOTrackResultModel()
		{
			WaybillNo = "";
			EventCode = "";
			Status = false;
			Message = "";
		}
	}


	// 2017-12-13 jsy
	// 트랙킹 데이터 가져오기
	public class YTOTrackingModel
	{
		public string WaybillNo { get; set; }
		public int success { get; set; }
		public string cnmessage { get; set; }
		public string enmessage { get; set; }

		public List<YTOTrackingDetailModel> DETAIL { get; set; }


		public YTOTrackingModel()
		{
			WaybillNo = "";
			success = 0;  // 0=실패, 1=성공
			cnmessage = "";
			enmessage = "";

			DETAIL = new List<YTOTrackingDetailModel>();
		}
	}


	// 2017-12-13 jsy
	// 트랙킹 상세 데이터
	public class YTOTrackingDetailModel
	{
		public string track_occur_date { get; set; }
		public string track_location { get; set; }
		public string track_description { get; set; }


		public YTOTrackingDetailModel()
		{
			track_occur_date = "";
			track_location = "";
			track_description = "";
		}
	}
}
