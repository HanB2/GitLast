﻿using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Ese
{
    public class EseGradeModels : SeachModel
	{
		//뷰
		public EseGroup Item { get; set; }

		//리스트 
		public List<EseGroup> Items { get; set; }

		public int chkCnt { get; set; }

		//모델 생성자
		public EseGradeModels()
		{
			this.Item = new EseGroup();
			this.Items = new List<EseGroup>();
		}

	}
}