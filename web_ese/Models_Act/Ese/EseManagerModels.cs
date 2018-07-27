using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Ese
{
	public class EseManagerModels : SeachModel
	{
		
		//뷰
		public EseUser Item { get; set; }

		//리스트 
		public List<EseUser> Items { get; set; }


		public int chkCnt { get; set; }
		public int chkGRADE { get; set; }


		public bool RadioBoxPop { get; set; }

		//그룹ID
		public List<schTypeArray> GroupIdArray { get; set; }
		public int GroupId { get; set; }



		//모델 생성자
		public EseManagerModels()
		{
			this.Item = new EseUser();
			this.Items = new List<EseUser>();
			this.GroupIdArray = new List<schTypeArray>();
		}
	}
}