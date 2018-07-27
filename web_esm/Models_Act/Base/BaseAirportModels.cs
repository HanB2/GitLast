using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Base
{
    public class BaseAirportModels : SeachModel
	{
		public ConfAirpot Item { get; set; }
		public List<ConfAirpot> Items { get; set; }
				
		//모델 생성자
		public BaseAirportModels()
		{
			this.Item = new ConfAirpot();
			this.Items = new List<ConfAirpot>();
			this.nationArray = new List<schTypeArray>();
		}

		public List<schTypeArray> nationArray { get; set; }

	}
}