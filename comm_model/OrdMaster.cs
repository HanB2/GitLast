using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class OrdMaster
	{
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public string UPLOADYMD { get; set; }       //	varchar(8)			업로드일자	
		public string WAYBILLNO { get; set; }       //	varchar(30)			송장번호	
		public string MASTERNO { get; set; }        //	varchar(20)			마스터번호	
		public string SUB_MASTERNO { get; set; }        //	varchar(20)			하위마스터번호	
		public string SENDER_NAME { get; set; }     //	varchar(50)			보내는사람 이름	
		public string SENDER_TELNO { get; set; }        //	varchar(20)			보내는사람 전화번호	
		public string SENDER_ADDR { get; set; }     //	varchar(100)			보내는사람 주소	
		public string ORDERNO1 { get; set; }        //	varchar(30)			주문번호1	
		public string ORDERNO2 { get; set; }        //	varchar(30)			주문번호2	
		public string ORDERNO3 { get; set; }        //	varchar(30)			주문번호3	
		public string CUSTOMNO { get; set; }        //	varchar(30)			통관번호	
		public string DELVNO { get; set; }      //	varchar(30)			현지배송번호	
		public string DELV_COM { get; set; }        //	varchar(20)			배송회사 코드	
		public string RECEIVER_NAME { get; set; }       //	varchar(50)			받는사람 이름	
		public string RECEIVER_NAME_ENG { get; set; }       //	varchar(50)			받는사람 영문 이름	
		public string RECEIVER_TELNO { get; set; }      //	varchar(20)			받는사람 전화번호	
		public string RECEIVER_CPHONENO { get; set; }       //	varchar(20)			받는사람 휴대폰번호	
		public string RECEIVER_ZIPCODE { get; set; }        //	varchar(10)			받는사람 우편번호	
		public string RECEIVER_STATE { get; set; }      //	varchar(30)			받는사람 성	
		public string RECEIVER_CITY { get; set; }       //	varchar(30)			받는사람 시	
		public string RECEIVER_DISTRICT { get; set; }       //	varchar(30)			받는사람 구	
		public string RECEIVER_ADDR1 { get; set; }      //	varchar(100)			받는사람 주소1	
		public string RECEIVER_ADDR2 { get; set; }      //	varchar(100)			받는사람 주소2	
		public string RECEIVER_EMAIL { get; set; }      //	varchar(50)			받는사람 이메일	
		public string SOCIALNO_BIZNO { get; set; }      //	varchar(20)			받는사람 신분증번호	
		public string MESSAGE_DELV { get; set; }        //	varchar(200)			배송메세지	
		public string NATION_CODE { get; set; }     //	char(2)			도착국가코드	
		public string DEP_NATION_CODE { get; set; }     //	char(2)			출발국가 코드	
		public int QTY_BOX { get; set; }        //	smallint(5)			박스갯수	
		public double CHARGEABLE_WEIGHT { get; set; }       //	double			적용무게	
		public string DELV_CODE { get; set; }       //	char(1)			출고타입 기호(A~Z)	
		public double DELVFEE { get; set; }     //	double			배송비	
		public double REALWEIGHT { get; set; }      //	double			실제무게	
		public string WEIGHT_UNIT { get; set; }     //	char(2)			무게단위(KG, LB)	
		public double REALVOLUME { get; set; }      //	double			부피무게	
		public double DIM_WIDTH { get; set; }       //	double			박스가로길이	
		public double DIM_LENGTH { get; set; }      //	double			박스세로길이	
		public double DIM_HEIGHT { get; set; }      //	double			박스높이	
		public string DIM_UNIT { get; set; }        //	varchar(10)			박스치수단위(cm,inch)	
		public double TAX { get; set; }     //	double			세금	
		public double INSURANCE { get; set; }       //	double			보험료	
		public double OTHER_COST { get; set; }      //	double			기타비용	
		public string CURRENCY_GOODS { get; set; }      //	char(3)			상품금액 화폐단위	
		public string USER1 { get; set; }       //	varchar(50)			사용자입력1	
		public string USER2 { get; set; }       //	varchar(50)			사용자입력2	
		public string USER3 { get; set; }       //	varchar(50)			사용자입력3	
		public string ACCIDENT_YN { get; set; }     //	varchar(3)			사고 유무(yes / no)	
		public string ACCIDENT_TYPE { get; set; }       //	varchar(50)			사고 종류(간단 설명)	
		public string IMPORTANT { get; set; }       //	varchar(3)			중요메세지 여부(yes / no)	
		public string RECEIVERTYPE { get; set; }        //	varchar(10)			받는이 구분(1=개인, 2=사업자)	
		public int CLEAR_PDT_CODE { get; set; }     //	tinyint(4)			신청타입(0:목록,1:일반) => 배송국가 한국	
		public string SDATA1 { get; set; }      //	varchar(30)			기타데이터	
		public string SDATA2 { get; set; }      //	varchar(30)			기타데이터	
		public string SDATA3 { get; set; }      //	varchar(30)			기타데이터	
		public int PACKING_COUNT { get; set; }      //	smallint(5)			팩킹리스트 출력횟수	
		public int LABEL_COUNT { get; set; }        //	smallint(5)			송장출력 횟수	
		public int PACKING_CHECK { get; set; }      //	smallint(5)			검수 횟수	
		public int MAR_CHECK { get; set; }      //	tinyint(4)			STATION 에 MAR 지급 여부(0=지급안함, 1=지금함)	
		public int ORDERTYPE { get; set; }      //	tinyint(4)			0=일반배송신청, 1=보관배송신청	


		public List<OrdGoods> GoodsList { get; set; }


		public OrdMaster()
		{
			GoodsList = new List<OrdGoods>();
		}



		/*================================================================================================================*/

		// 물품의 전체갯수를 리턴한다
		public int GetGoodsPieces()
		{
			return GoodsList.Sum(m => m.QTY);
		}

		// 물품목록을 리턴한다(물품명만)
		public string GetGoodsDescriptionWithoutQty(string saparator = ", ")
		{
			string Description = "";

			for (int i = 0; i < GoodsList.Count; i++)
			{
				if (i > 0)
					Description += saparator;

				Description += GoodsList[i].GOODS_NAME;
			}

			return Description;
		}

		// 물품의 전체금액을 리턴한다
		public double GetGoodsTotalAmount()
		{
			return GoodsList.Sum(m => m.PRICE * m.QTY);
		}


		// 무게를 kg으로 변환해서 리턴한다
		public double GetWeightKG()
		{
			double weight_kg = this.REALWEIGHT;

			if (this.WEIGHT_UNIT.ToUpper() == "LB")
			{
				weight_kg = this.REALWEIGHT * 0.453592;
			}

			return Math.Round(weight_kg, 3);
		}


		// 무게를 lb로 변환해서 리턴한다
		public double GetWeightLB()
		{
			double weight_lb = this.REALWEIGHT;

			if (this.WEIGHT_UNIT.ToUpper() == "KG")
			{
				weight_lb = this.REALWEIGHT * 2.204622;
			}

			return Math.Round(weight_lb, 1);
		}

	}
}
