using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_delvapi.Model
{
	// 일본배송 QExpress : 주문정보 업로드
	public class QxpressModels
	{
		public string RAINBOWCODE { get; set; }
		public string AGENTCODE { get; set; }
		public string INVOICENO { get; set; }

		public string refOrderNo { get; set; }  // Your reference delivery order number
		public string svcType { get; set; }  // "RM" (It is fixed value)
		public string rcptName { get; set; }  // Recipient name
		public string rcptEmail { get; set; }  // Recipient email address
		public string rcptCountry { get; set; }  // Recipient country ("SG")
		public string rcptAddr1 { get; set; }  // Recipient address 1 (State, City, Street) * JP - 도도부현 (都道府県)
		public string rcptAddr2 { get; set; }  // Recipient address 2 (Bldg, Ste#) * JP - 시구정촌(市区町村) + 번지(番地)
		public string rcptZipcode { get; set; }  // Recipient postal code of address
		public string rcptPhone { get; set; }  // Recipient telephone
		public string rcptMobile { get; set; }  // Recipient mobile
		public string rcptMemo { get; set; }  // Recipient request memo
		public string contents { get; set; }  // Item name
		public int quantity { get; set; }  // Quantity of Items (is not box qty.)
		public double value { get; set; }  // Parcel amount value
		public string currency { get; set; }  // Currency of value ("SGD")
		public string useFukuokaRoute { get; set; }  // 출발국 KR 도착국 JP 의 부산선편 유무(Y OR N)
		public string rcptNameFurigana { get; set; }  // 수취인 명 (후리가나) : 도착국 JP 일 경우 선택입력
		public string premiumService { get; set; }  // Premium delivery service (only for KR-US route) : Y OR N

		// 결과값 리턴
		public int ResultCode { get; set; }
		public string ResultMsg { get; set; }
		public string ShippingNo { get; set; }  // Q번호
		public string RefOrderNo { get; set; }  // 고객이 입력한 주문번호


		public QxpressModels()
		{
			RAINBOWCODE = "";
			AGENTCODE = "";
			INVOICENO = "";

			refOrderNo = "";  // Your reference delivery order number
			svcType = "";  // "RM" (It is fixed value)
			rcptName = "";  // Recipient name
			rcptEmail = "";  // Recipient email address
			rcptCountry = "";  // Recipient country ("SG")
			rcptAddr1 = "";  // Recipient address 1 (State, City, Street) * JP - 도도부현 (都道府県)
			rcptAddr2 = "";  // Recipient address 2 (Bldg, Ste#) * JP - 시구정촌(市区町村) + 번지(番地)
			rcptZipcode = "";  // Recipient postal code of address
			rcptPhone = "";  // Recipient telephone
			rcptMobile = "";  // Recipient mobile
			rcptMemo = "";  // Recipient request memo
			contents = "";  // Item name
			quantity = 0;  // Quantity of Items (is not box qty.)
			value = 0.0;  // Parcel amount value
			currency = "";  // Currency of value ("SGD")
			useFukuokaRoute = "N";  // 출발국 KR 도착국 JP 의 부산선편 유무(Y OR N)
			rcptNameFurigana = "";  // 수취인 명 (후리가나) : 도착국 JP 일 경우 선택입력
			premiumService = "N";  // Premium delivery service (only for KR-US route) : Y OR N

			// 결과값 리턴
			ResultCode = -1;
			ResultMsg = "";
			ShippingNo = "";  // Q번호
			RefOrderNo = "";  // 고객이 입력한 주문번호
		}
	}










	// 일본배송 QExpress : 트랙킹
	public class QxpressTrackingModels
	{
		public string RAINBOWCODE { get; set; }
		public string AGENTCODE { get; set; }
		public string INVOICENO { get; set; }
		public string DELVNO { get; set; }

		public int ResultCode { get; set; }
		public string ResultMsg { get; set; }

		public string shipping_no { get; set; }
		public string ref_no { get; set; }
		public string qs_no { get; set; }
		public string recipient { get; set; }

		public List<QxpressTrackingDetailModels> DETAIL { get; set; }


		public QxpressTrackingModels()
		{
			RAINBOWCODE = "";
			AGENTCODE = "";
			INVOICENO = "";
			DELVNO = "";

			shipping_no = "";
			ref_no = "";
			qs_no = "";
			recipient = "";

			ResultCode = -1;
			ResultMsg = "";

			DETAIL = new List<QxpressTrackingDetailModels>();
		}
	}


	// 일본배송 QExpress : 트랙킹 상세내용
	public class QxpressTrackingDetailModels
	{
		public string status { get; set; }
		public string date { get; set; }
		public string location { get; set; }


		public QxpressTrackingDetailModels()
		{
			status = "";
			date = "";
			location = "";
		}
	}
}
