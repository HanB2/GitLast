using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Stoc
{
    public class StocInOutModels : SeachModel 
	{

		//BASE SETTING
		public string BaseBarcode { get; set; }
		public string BaseEstCode { get; set; }
		public string BaseEstName { get; set; }
		public string BaseEseCode { get; set; }
		public string BaseNationCode { get; set; }
		public string BaseNationName { get; set; }

		//검색조건
		public string schNation { get; set; }   //검색 국가
		public string schStation { get; set; }  //검색 스테이션
		public string schStat { get; set; }  //검색 스테이션
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
			new schTypeArray {      opt_key = "si.BARCODE",        opt_value = "바코드" },
			new schTypeArray {      opt_key = "PRODUCT_NAME",        opt_value = "상품명" } 
		};


		public List<schTypeArray> schTypeArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "0",        opt_value = "입고" },
			new schTypeArray {      opt_key = "1",        opt_value = "출고" },
			new schTypeArray {      opt_key = "2",        opt_value = "불량" },
			new schTypeArray {      opt_key = "3",        opt_value = "재고조정" }
		};

		

		//페이징
		public PagingModel Paging { get; set; }  // 입출고 정보

		//리스트 
		public List<StocInOutItem> Items { get; set; }  // 입출고 정보


		// 정렬부분 세팅
		public List<schTypeArray> sortKeyArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "BARCODE",        opt_value = "바코드" },
			new schTypeArray {      opt_key = "EST_NAME",        opt_value = "STATION" }
		};


		//모델 생성자
		public StocInOutModels()
		{
			this.Paging = new PagingModel();
			this.Items = new List<StocInOutItem>();
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


		//입출고 내역 조회 리스트 출력용 클래스
		public class StocInOutItem
		{

			public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)		
			public string EST_CODE { get; set; }
			public string NATION_NAME { get; set; }
			public string EST_NAME { get; set; }
			public string CATE_NAME { get; set; }
			public string BARCODE { get; set; } 
			public string SKU { get; set; }     
			public string GOODS_NAME { get; set; }
			public string BRAND { get; set; }
			public string STAT_TXT { get; set; }
			public string STAT { get; set; }
			public string CNT { get; set; }
			public string REG_DT { get; set; }

			public int CATEGORY1 { get; set; }
			public int CATEGORY2 { get; set; }
			public int CATEGORY3 { get; set; }
			public int CATEGORY4 { get; set; }


		}

	}
}
 