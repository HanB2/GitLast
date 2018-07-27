using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Ese
{
    public class EseInfoModels : SeachModel
	{
		public string viewEseCode { get; set; }   //ESECODE

		//라디오버튼
		public string RadioBoxPop { get; set; }

		public EstSender Item { get; set; }


		//모델 생성자
		public EseInfoModels()
		{
			this.Item = new EstSender();
		}

	}
}