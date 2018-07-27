using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Base
{
    public class BaseCurrencyModels : SeachModel
	{
		public ConfCurrency Item { get; set; }
		public List<ConfCurrency> Items { get; set; }

		//모델 생성자
		public BaseCurrencyModels()
		{
			this.Item = new ConfCurrency();
			this.Items = new List<ConfCurrency>();
		}
		

	}
}