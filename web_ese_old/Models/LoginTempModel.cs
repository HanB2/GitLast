using System.ComponentModel.DataAnnotations;

namespace web_ese_old.Models
{
	public class LoginTempModel
	{
		[Required]
		[Display(Name = "전자 메일")]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "암호")]
		public string Password { get; set; }

		[Display(Name = "사용자 이름 및 암호 저장")]
		public bool RememberMe { get; set; }
	}
}