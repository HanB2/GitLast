using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Ese
{
	public class EseGradeViewModels : SeachModel
	{

		//등급 정보
		public EseGroup Item { get; set; }

		//권한 정보
		public List<EseGroupPermission> Items { get; set; }

		//액션 리스트
		public List<schTypeArray> gradeList { get; set; }

		//Station
		public string viewEseCode { get; set; }

		//모델 생성자
		public EseGradeViewModels()
		{
			this.Item = new EseGroup();
			this.Items = new List<EseGroupPermission>();
			this.gradeList = new List<schTypeArray>();
		}
	}
}