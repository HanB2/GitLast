using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Ese
{
	public class EseIframeInfoModels : SeachModel
	{
		public string viewEseCode { get; set; }   //Sender

		//뷰
		public EseUser Item { get; set; }

		//리스트 
		public List<EseUser> Items { get; set; }


		//모델 생성자
		public EseIframeInfoModels()
		{
			this.Item = new EseUser();
			this.Items = new List<EseUser>();		
		}

		//라디오버튼
		public string RadioBoxPop { get; set; }

	}
}