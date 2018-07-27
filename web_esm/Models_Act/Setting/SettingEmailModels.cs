using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web_esm.Models_Act.Setting
{
    public class SettingEmailModels
    {
	    [Required(ErrorMessage ="SMTP 서버명값이 존재하지 않습니다.")]
		public string emaile_sender_server { get; set; }            //	SMTP 서버명
		[Required(ErrorMessage = "SMTP 포트번호값이 존재하지 않습니다.")]
		public string emaile_sender_port { get; set; }          //	SMTP 포트번호
		[Required(ErrorMessage = "ID 값이 존재하지 않습니다.")]
		public string emaile_sender_id { get; set; }            //	ID
		[Required(ErrorMessage = "PASSWORD 값이 존재하지 않습니다.")]
		public string emaile_sender_pw { get; set; }            //	PASSWORD

		[EmailAddress(ErrorMessage = "E-MAIL 값이 존재하지 않습니다.")]
		public string emaile_sender_email { get; set; }         //	보내는 사람 E-MAIL
		[Required(ErrorMessage = "이름 값이 존재하지 않습니다.")]
		public string emaile_sender_name { get; set; }          //	보내는 사람 이름
	}
}