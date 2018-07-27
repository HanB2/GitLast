using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Base
{
	public class BaseOutPutRateModels : SeachModel
	{
		public ConfReleaseType Item { get; set; }
		public List<ConfCustomsFee> Items { get; set; }
		public DataTable dt { get; set; }
		public set_File File { get; set; }


		//모델 생성자
		public BaseOutPutRateModels()
		{
			this.Item = new ConfReleaseType();
			this.Items = new List<ConfCustomsFee>();
			this.dt = new DataTable();
			this.File = new set_File();
		}
		
	}
}