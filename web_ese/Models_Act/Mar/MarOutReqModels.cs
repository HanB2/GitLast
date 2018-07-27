using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Mar
{
    public class MarOutReqModels : SeachModel
	{
		
		public MarWithdrawReq Item { get; set; }

		//보유 마르
		public double MY_MAR { get; set; }

		//계좌정보 입력 체크
		public bool CHK_DATA { get; set; }

		//환불 요청 값
		public double REQ_AMOUNT { get; set; }
		public string REQ_MEMO { get; set; }


		//모델 생성자
		public MarOutReqModels()
		{
			this.Item = new MarWithdrawReq();
		}

	}
}