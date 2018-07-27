using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class OrdGoods
	{
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드
		public string UPLOADYMD { get; set; }       //	varchar(8)			업로드일자
		public string WAYBILLNO { get; set; }       //	varchar(30)			송장번호
		public int ITEMSN { get; set; }     //	smallint(5)			상품정보 순번
		public string GOODS_NAME { get; set; }      //	varchar(200)			상품명
		public string BRAND { get; set; }       //	varchar(50)			브랜드명
		public double PRICE { get; set; }       //	double			단가
		public int QTY { get; set; }        //	smallint(5)			갯수
		public string HSCODE { get; set; }      //	varchar(20)			HSCODE
		public double TAX_RATE { get; set; }        //	double			세율
		public double VOLUME { get; set; }      //	double			용량
		public string UNIT { get; set; }        //	varchar(10)			단위
		public string PURCHASE_URL { get; set; }        //	varchar(300)			구매 URL
		public string BARCODE { get; set; }     //	varchar(50)			바코드
		public string SKU { get; set; }     //	varchar(50)			재고관리번호
		public int OUTPUTNO { get; set; }       //	int(11)			출고번호
		public string ITEMOPTION { get; set; }      //	varchar(150)			제품옵션




		// Join 해서 가져오는 Field
		public string PRODUCT_NAME_KR { get; set; }  // 상품명(한글)
		public string PRODUCT_NAME_CN { get; set; }  // 상품명(중문)
		public string PRODUCT_NAME_EN { get; set; }  // 상품명(영문)
		public string RACKNO { get; set; }  // 랙번호


		public int REAL_CNT { get; set; }     //	재고 수량
		public int BAD_CNT { get; set; }     //	재고 수량
		public int stoc_cnt { get; set; }     //	재고 수량
		public string BOXNUM { get; set; }      //박스번호'	,

	}
}
