using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Esm
{
    public class EsmGradeModels : SeachModel
	{
		
		//뷰
		public EsmGroup Item { get; set; }  

		//리스트 
		public List<EsmGroup> Items { get; set; }  
		

		//모델 생성자
		public EsmGradeModels()
		{
			this.Item = new EsmGroup();
			this.Items = new List<EsmGroup>();
		}


	}
}