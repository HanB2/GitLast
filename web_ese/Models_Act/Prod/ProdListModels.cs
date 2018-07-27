using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Prod
{
    public class ProdListModels : SeachModel
	{
		 
		public PagingModel Paging { get; set; }
		public StcGoods Item { get; set; }
		public List<StcGoods> Items { get; set; }
		public PROD_EXCEL Excel { get; set; }


		public string resultMsg { get; set; }
		public string delChk { get; set; }

		public int cate1 { get; set; }
		public int cate2 { get; set; }
		public int cate3 { get; set; }
		public int cate4 { get; set; }
		
		public List<schTypeArray> cateList1 { get; set; }
		public List<schTypeArray> cateList2 { get; set; }
		public List<schTypeArray> cateList3 { get; set; }
		public List<schTypeArray> cateList4 { get; set; }
		

		public List<schTypeArray> schTypeTxtArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "대표 상품명",        opt_value = "PRODUCT_NAME" },
			new schTypeArray {      opt_key = "바코드",        opt_value = "BARCODE" }			
		};

		
		public List<schTypeArray> viewWEIGHT_UNIT = new List<schTypeArray> {
			new schTypeArray {      opt_key = "l",        opt_value = "l" },
			new schTypeArray {      opt_key = "ml",        opt_value = "ml" },
			new schTypeArray {      opt_key = "ㅎ",        opt_value = "g" },
			new schTypeArray {      opt_key = "mg",        opt_value = "mg" }
		};

		// 정렬부분 세팅
		public List<schTypeArray> sortKeyArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "SEQNO",        opt_value = "최신" },
			new schTypeArray {      opt_key = "PRODUCT_NAME",        opt_value = "상품명" }
		};


		//모델 생성자
		public ProdListModels()
		{
			this.Paging = new PagingModel();
			this.Item = new StcGoods();
			this.Items = new List<StcGoods>();
			this.cateList1 = new List<schTypeArray>();
			this.cateList2 = new List<schTypeArray>();
			this.cateList3 = new List<schTypeArray>();
			this.cateList4 = new List<schTypeArray>();
			this.cate1 = 0;
			this.cate2 = 0;
			this.cate3 = 0;
			this.cate4 = 0;
			this.Excel = new PROD_EXCEL();
			this.Excel.errList = new List<string>();
		}




		//엑셀 업로드/다운로드 용 클래스
		public class PROD_EXCEL
		{
			public bool result { get; set; }
			public string FileType { get; set; }
			public List<string> errList { get; set; }
			public set_File File { get; set; }  //HttpPostedFileBase 
		}
		
	}
}