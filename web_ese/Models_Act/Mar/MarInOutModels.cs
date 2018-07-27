using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Mar
{
    public class MarInOutModels : SeachModel
	{
		public PagingModel Paging { get; set; }

		public List<MAR_REQ_LIST> reqList { get; set; }

		public List<MAR_OUT_REQ_LIST> outReqList { get; set; }

		public List<MAR_INOUT_LIST> inOutList { get; set; }
		
		//모델 생성자
		public MarInOutModels()
		{
			this.Paging = new PagingModel();
			this.reqList = new List<MAR_REQ_LIST>();
			this.outReqList = new List<MAR_OUT_REQ_LIST>();
			this.inOutList = new List<MAR_INOUT_LIST>();
		}


		
		public class MAR_REQ_LIST
		{
			public int STATUS { get; set; }
			public string STATUS_TEXT { get; set; }
			public double DEPOSIT_AMOUNT { get; set; }
			public double MAR { get; set; }
			public string CREATETIME { get; set; }
			public string DATETIME_UPD { get; set; }
		}

		public class MAR_OUT_REQ_LIST
		{
			public int STATUS { get; set; }
			public string STATUS_TEXT { get; set; }
			public double REQ_AMOUNT { get; set; }
			public double AMOUNT { get; set; }
			public string CREATETIME { get; set; }
			public string DATETIME_UPD { get; set; }
		}


		public class MAR_INOUT_LIST
		{
			public int INOUT_TYPE { get; set; }
			public string INOUT_TYPE_TEXT { get; set; }
			public double MAR { get; set; }
			public string DATETIME_UPD { get; set; }
		}
	}
}