using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Ese
{
	public class EseAccountModels : SeachModel
	{

		[Required(ErrorMessage = "SWIFT CODE값이 존재하지 않습니다.")]
		public string setting_SwiftCode { get; set; }            //	SWIFT CODE

		[Required(ErrorMessage = "은행 주소값이 존재하지 않습니다.")]
		public string setting_BankAddr { get; set; }            //	은행 주소

		[Required(ErrorMessage = "당행 계좌번호값이 존재하지 않습니다.")]
		public string setting_AccountNum { get; set; }         //당행 계좌번호

		[Required(ErrorMessage = "영문 수취인 명이 존재하지 않습니다.")]
		public string setting_ReceiverName_en { get; set; }    //영문 수취인 명

		[Required(ErrorMessage = "메모값이 존재하지 않습니다.")]
		public string setting_Memo { get; set; }               //메모

		//Sender
		public string viewEseCode { get; set; }

	}
}