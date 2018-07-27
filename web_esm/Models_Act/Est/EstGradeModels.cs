using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Est
{
    public class EstGradeModels : SeachModel
	{
		//뷰
		public EstGroup Item { get; set; }

		//리스트 
		public List<EstGroup> Items { get; set; }

		//Station
		public string viewEstCode { get; set; }

		//GroupId
		public string groupId { get; set; }

		//모델 생성자
		public EstGradeModels()
		{
			this.Item = new EstGroup();
			this.Items = new List<EstGroup>();
		}
	}
}