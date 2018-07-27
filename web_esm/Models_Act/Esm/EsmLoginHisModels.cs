using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Esm
{
    public class EsmLoginHisModels : SeachModel 
	{

		//페이징
		public PagingModel Paging { get; set; }  

		//뷰
		public CommLoginLog Item { get; set; }  

		//리스트 
		public List<CommLoginLog> Items { get; set; }  

		//액션 리스트
		public List<schTypeArray> schTypeArray { get; set; }

		//모델 생성자
		public EsmLoginHisModels()
		{
			this.Item = new CommLoginLog();
			this.Items = new List<CommLoginLog>();
			this.schTypeArray = new List<schTypeArray>();
			this.Paging = new PagingModel();
		}


	}
}