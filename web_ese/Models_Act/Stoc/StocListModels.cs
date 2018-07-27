using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Stoc
{
    public class StocListModels : SeachModel
	{
		//BASE SETTING
		public string BaseEstCode { get; set; }
		public string BaseEstName { get; set; }
		public string BaseEseCode { get; set; }
		public string BaseNationCode { get; set; }
		public string BaseNationName { get; set; }

		//검색조건
		public string schNation { get; set; }   //검색 국가
		public string schStation { get; set; }  //검색 스테이션
		public int cate1 { get; set; }
		public int cate2 { get; set; }
		public int cate3 { get; set; }
		public int cate4 { get; set; }

		public List<schTypeArray> arraySNation { get; set; }
		public List<schTypeArray> arrayStation { get; set; }
		public List<schTypeArray> cateList1 { get; set; }
		public List<schTypeArray> cateList2 { get; set; }
		public List<schTypeArray> cateList3 { get; set; }
		public List<schTypeArray> cateList4 { get; set; }
		
		
		public List<schTypeArray> schTypeTxtArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "바코드",        opt_value = "si.BARCODE" },
			new schTypeArray {      opt_key = "상품명",        opt_value = "sg.PRODUCT_NAME" }
		};

		// 정렬부분 세팅
		public List<schTypeArray> sortKeyArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "EST_CODE",        opt_value = "STATION" },
			new schTypeArray {      opt_key = "BARCODE",        opt_value = "바코드" } 
		};


		//페이징
		public PagingModel Paging { get; set; }  // 입출고 정보

		//리스트 
		public List<StcListItem> Items { get; set; }  // 입출고 정보

		//모델 생성자
		public StocListModels()
		{
			this.Paging = new PagingModel();
			this.Items = new List<StcListItem>();
			this.cateList1 = new List<schTypeArray>();
			this.cateList2 = new List<schTypeArray>();
			this.cateList3 = new List<schTypeArray>();
			this.cateList4 = new List<schTypeArray>();
			this.arraySNation = new List<schTypeArray>();
			this.arrayStation = new List<schTypeArray>();
			this.cate1 = 0;
			this.cate2 = 0;
			this.cate3 = 0;
			this.cate4 = 0;
		}

		//재고조회 리스트 출력용 클래스
		public class StcListItem
		{
			public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
			public string BARCODE { get; set; }     //	varchar(50)			바코드	
			public string EST_CODE { get; set; }    
			public string NATION_NAME { get; set; } 
			public string EST_NAME { get; set; }    
			public string CATE_NAME { get; set; }   
			public string GOODS_NAME { get; set; }
			public string BRAND { get; set; }
			public int IN_CNT { get; set; }      //	smallint			입고 수량
			public int OUT_CNT { get; set; }      //	smallint			출고 수량
			public int BAD_CNT { get; set; }      //	smallint			불량 수량
			public int CHANGE_CNT { get; set; }      //	smallint			불량 수량
			public int STOC_CNT { get; set; }      //	smallint			재고 수량
			public int EXP_OUT_CNT { get; set; }      //	smallint			출고 예정 수량
			public int EXP_STOC_CNT { get; set; }      //	smallint			재고 예정 수량

			public int CATEGORY1 { get; set; }
			public int CATEGORY2 { get; set; }
			public int CATEGORY3 { get; set; }
			public int CATEGORY4 { get; set; }
		}


	}
}
 