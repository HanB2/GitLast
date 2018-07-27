using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Base
{
	public class BaseLocalModels : SeachModel
	{
		public ConfLocalDelivery Item { get; set; }
		public List<ConfLocalDelivery> Items { get; set; }

		//모델 생성자
		public BaseLocalModels()
		{
			this.Item = new ConfLocalDelivery();
			this.Items = new List<ConfLocalDelivery>();
			this.nationArray = new List<schTypeArray>();
		}

		public List<schTypeArray> nationArray { get; set; }
	}
}