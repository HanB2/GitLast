using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Est
{
    public class EstAccountModels : SeachModel
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

		//Station
		public string viewEstCode { get; set; }  

		//모델 생성자
		public EstAccountModels()
		{	
			this.stationArray = new List<schTypeArray>();
		}


		//STATION 셀렉트 박스 데이터 세팅
		public List<schTypeArray> stationArray { get; set; }
		public string station { get; set; }   //Station
	}
}