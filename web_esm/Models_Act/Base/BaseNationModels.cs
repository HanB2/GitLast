using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Base
{
    public class BaseNationModels : SeachModel
	{

		//모델 생성자
		public BaseNationModels()
		{
			this.Item = new ConfShippingCountry();
			this.Items = new List<ConfShippingCountry>();
			this.nationArray = new List<schTypeArray>();
			this.weightArray = new List<schTypeArray>();
			this.currencyArray = new List<schTypeArray>();
		}

		//뷰
		public ConfShippingCountry Item { get; set; }
		//리스트
		public List<ConfShippingCountry> Items { get; set; }
		//국가  
		public List<schTypeArray> nationArray { get; set; }
		//무게 단위
		public List<schTypeArray> weightArray { get; set; }
		//화폐 단위
		public List<schTypeArray> currencyArray { get; set; }
		

	}
}