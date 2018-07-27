using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class StcGoods
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
		[MaxLength(5)]
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드	
		[MaxLength(8)]
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	

		public int CATEGORY1 { get; set; }      //	int			카테고리 1단계	
		public int CATEGORY2 { get; set; }      //	int			카테고리 2단계	
		public int CATEGORY3 { get; set; }      //	int			카테고리 3단계	
		public int CATEGORY4 { get; set; }      //	int			카테고리 4단계	
		[MaxLength(50)]
		[Required(ErrorMessage = "바코드값이 존재하지 않습니다.")]
		public string BARCODE { get; set; }     //	varchar(50)			바코드	

		[MaxLength(50)]
		public string SKU { get; set; }     //	varchar(50)			상품관리번호	

		[MaxLength(100)]
		public string PRODUCT_NAME { get; set; }        //	varchar(100)			대표상품명

		[MaxLength(100)]
		public string PRODUCT_NAME_KR { get; set; }     //	varchar(100)			상품명(한글)

		[MaxLength(100)]
		public string PRODUCT_NAME_CN { get; set; }     //	varchar(100)			상품명(중문)

		[MaxLength(100)]
		public string PRODUCT_NAME_EN { get; set; }     //	varchar(100)			상품명(영문)

		[MaxLength(50)]
		public string BRAND { get; set; }       //	varchar(50)			브랜드명	

		public double PRICE { get; set; }       //	double			가격	
		public double UNIT_WEIGHT { get; set; }     //	double			중량 / 용량	

		[MaxLength(3)]
		public string WEIGHT_UNIT { get; set; }     //	char(3)			단위	

		[MaxLength(10)]
		public string STANDARD { get; set; }        //	char(10)			규격	
		public int EXPIRATION { get; set; }     //	int			유통기한	
		public string EXPIRATION_DT { get; set; }       //	DATETIME			유통만료일자	

		[MaxLength(30)]
		public string ORIGIN { get; set; }      //	char(30)			원산지	

		[MaxLength(200)]
		public string INGREDIENT { get; set; }      //	varchar(200)			성분	
		public string SPEC { get; set; }        //	text			상세스펙

		[MaxLength(200)]
		public string SALE_SITE_URL { get; set; }       //	varchar(200)			판매 사이트 URL	
		public string PRODUCT_IMAGE { get; set; }       //	text			상품 이미지 URL

		[MaxLength(20)]
		public string RACKNO { get; set; }      //	varchar(20)			랙번호	

		[MaxLength(200)]
		public string COMMENT { get; set; }     //	varchar(200)			메모	
		public string REG_DT { get; set; }      //	DATETIME			등록일자	
		public string UPDT_DT { get; set; }     //	DATETIME			수정일자	


		public string CATEGORY1_txt { get; set; } //카테고리1 문자열 


		//셀렉트 박스 만들기 용 모델
		public class schTypeArray
		{
			public string opt_key { get; set; }   //공지유형
			public string opt_value { get; set; }   //공지유형
		}


		//중량/용량의 단위 셀렉트 박스 데이터 세팅 
		public IEnumerable<schTypeArray> weightUnitArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "ml",        opt_value = "ml" },
			new schTypeArray {      opt_key = "mg",        opt_value = "mg" }
		};

		//규격 셀렉트 박스 데이터 세팅 
		public IEnumerable<schTypeArray> standardArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "개",        opt_value = "개" },
			new schTypeArray {      opt_key = "병",        opt_value = "병" }
	    };

		//act_type (서브밋 결과가 여러가지일 경우 구분을 위해 사용 ex: insert or update ...)
		//act_key  (서브밋 시 키가 사용될 경우 사용)
		public string act_type { get; set; }
		public int act_key { get; set; }

	}
}
