using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Prod
{
    public class ProdAddModels : SeachModel
	{

		public StcGoods Item { get; set; }

		public PROD_EXCEL Excel { get; set; }


		public List<schTypeArray> cateList  { get; set; }

		//모델 생성자
		public ProdAddModels()
		{
			this.cateList = new List<schTypeArray>();
			this.Item = new StcGoods();
			this.Excel = new PROD_EXCEL();
		}


		//엑셀 업로드/다운로드 용 클래스
		public class PROD_EXCEL
		{
			public bool result { get; set; }
			public string FileType { get; set; }
			public List<string> errList { get; set; }
			public set_File File { get; set; }
		}

	}
}