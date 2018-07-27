using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Base
{
    public class BaseOutPutTypeModels : SeachModel
	{
		public ConfReleaseType Item { get; set; }
		public List<ConfReleaseType> Items { get; set; }

		//모델 생성자
		public BaseOutPutTypeModels()
		{
			this.Item = new ConfReleaseType();
			this.Items = new List<ConfReleaseType>();
			this.nationArray = new List<schTypeArray>();
		}

		public List<schTypeArray> nationArray { get; set; }

	
	
	}
}