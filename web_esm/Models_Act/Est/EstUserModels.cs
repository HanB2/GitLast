using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Est
{
	public class EstUserModels : SeachModel
	{
		//페이징
		public PagingModel Paging { get; set; }
		//Station
		public string viewEstCode { get; set; }

		public EstUser Item { get; set; }
		public List<EstUser> Items { get; set; }

		//모델 생성자
		public EstUserModels()
		{
			this.Paging = new PagingModel();
			this.Item = new EstUser();
			this.Items = new List<EstUser>();

			this.GroupIdArray = new List<schTypeArray>();
		}


		public bool RadioBoxPop { get; set; }

		//그룹ID
		public List<schTypeArray> GroupIdArray { get; set; }
		public int GroupId { get; set; }
	}
}