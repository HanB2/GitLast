using comm_model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Ese
{
    public class EseAccountModels : SeachModel
	{

		[Required(ErrorMessage = "SWIFT CODE를 입력해주세요.")]
		public string setting_SwiftCode { get; set; }            //	SWIFT CODE

		[Required(ErrorMessage = "은행 주소를 입력해주세요.")]
		public string setting_BankAddr { get; set; }            //	은행 주소

		[Required(ErrorMessage = "당행 계좌번호를 입력해주세요.")]
		public string setting_AccountNum { get; set; }         //당행 계좌번호

		[Required(ErrorMessage = "영문 수취인 명을 입력해주세요.")]
		public string setting_ReceiverName_en { get; set; }    //영문 수취인 명


		public string setting_Memo { get; set; }               //메모

		//ESE_CODE
		public string viewEseCode { get; set; }

		//뷰
		public EseUser Item { get; set; }

		//리스트 
		public List<EseUser> Items { get; set; }


		//모델 생성자
		public EseAccountModels()
		{
			this.Item = new EseUser();
			this.Items = new List<EseUser>();
		}

	}
}