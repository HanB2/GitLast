using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Mar
{
    public class MarReqModels : SeachModel
	{

		//페이징
		public PagingModel Paging { get; set; }

		//화폐 단위
		public List<schTypeArray> currencyArray { get; set; }
		//
		public List<MarChargeReq> Items { get; set; }

		public MarChargeReq Item { get; set; }

		



		//모델 생성자
		public MarReqModels()
		{
			this.Paging = new PagingModel();
			this.Item = new MarChargeReq();
			this.Items = new List<MarChargeReq>();
			this.currencyArray = new List<schTypeArray>();
		}

	}
}