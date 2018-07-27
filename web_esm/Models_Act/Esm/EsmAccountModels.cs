using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Esm
{
    public class EsmAccountModels : SeachModel
	{

		//뷰
		public EsmUser Item { get; set; }

		//리스트 
		public List<EsmUser> Items { get; set; }


		//모델 생성자
		public EsmAccountModels()
		{
			this.Item = new EsmUser();
			this.Items = new List<EsmUser>();
			this.GroupIdArray = new List<schTypeArray>();
		}

		public bool RadioBoxPop { get; set; }

		//그룹ID 
		public List<schTypeArray> GroupIdArray { get; set; }

		public int GroupId { get; set; }
	}
}