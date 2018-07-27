using comm_model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Ese
{
	public class EseInfoMyModels : SeachModel
	{

		//ESE_CODE
		public string email { get; set; }

		[Required(ErrorMessage = "현재 비밀번호값을 입력해주세요.")]
		[DataType(DataType.Password)]
		public string passwd { get; set; }            //	현재 비밀번호

		[Required(ErrorMessage = "새 비밀번호값을 입력해주세요.")]
		public string passwd_new { get; set; }            //	새 비밀번호

		[Required(ErrorMessage = "비밀번호 확인값을 입력해주세요.")]
		public string passwd_check { get; set; }         //비밀번호 확인

		//ESE_CODE
		public string viewEseCode { get; set; }

		//뷰
		public EseUser Item { get; set; }

		//리스트 
		public List<EseUser> Items { get; set; }

		//모델 생성자
		public EseInfoMyModels()
		{
			this.Item = new EseUser();
			this.Items = new List<EseUser>();
		}
	}
}