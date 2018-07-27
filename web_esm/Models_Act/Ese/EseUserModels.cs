using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Ese
{
	public class EseUserModels : SeachModel
	{
		//Sender
		public string viewEseCode { get; set; }

		public EseUser Item { get; set; }
		public List<EseUser> Items { get; set; }

		//모델 생성자
		public EseUserModels()
		{

			this.Item = new EseUser();
			this.Items = new List<EseUser>();

			this.GroupIdArray = new List<schTypeArray>();
		}
	    //그룹ID
		public List<schTypeArray> GroupIdArray { get; set; }
		public int GroupId { get; set; }

		public bool RadioBoxPop { get; set; }
	}
}