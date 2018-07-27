using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Est
{
	public class EstGradeViewModel : SeachModel
	{
		//등급 정보
		public EstGroup Item { get; set; }

		//권한 정보
		public List<EstGroupPermisson> Items { get; set; }

		//액션 리스트
		public List<schTypeArray> gradeList { get; set; }

		//Station
		public string viewEstCode { get; set; }

		//모델 생성자
		public EstGradeViewModel()
		{
			this.Item = new EstGroup();
			this.Items = new List<EstGroupPermisson>();
			this.gradeList = new List<schTypeArray>();
		}
	}
}