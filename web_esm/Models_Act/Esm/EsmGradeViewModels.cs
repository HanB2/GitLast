using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Esm
{
	public class EsmGradeViewModels : SeachModel
	{
		//등급 정보
		public EsmGroup Item { get; set; }

		//권한 정보
		public List<EsmGroupPermisson> Items { get; set; }
		
		//액션 리스트
		public List<schTypeArray> gradeList { get; set; }

		//모델 생성자
		public EsmGradeViewModels()
		{
			this.Item = new EsmGroup();
			this.Items = new List<EsmGroupPermisson>();
			this.gradeList = new List<schTypeArray>();
		}


	}
}