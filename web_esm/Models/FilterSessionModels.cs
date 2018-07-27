using comm_dbconn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace web_esm.Models
{
	public class FilterSessionModels
	{
		//로그인시 생성하는 세션 리스트
		public string MANAGE_NO { get; set; }
		public string MANAGE_GRADE { get; set; }
		public string CURRENT_LOGIN_EMAIL { get; set; }


		//필터에서 권한 조회를 위한 액션 변수
		public string chkAction { get; set; }
		public string chkController { get; set; }
		

		//컨트롤러에서 권한 조회를 위한 액션 변수
		public string setAction { get; set; }   //대표 액션명을 입력 예시 BaseLocal 등
		public string setType { get; set; }     //PER_SELECT	PER_INSERT	PER_UPDATE	PER_DELETE 를 문자열로 입력

		public string alertStr = "권한이 없습니다.";


		//각 세션 값 비어있는지 확인
		public bool chkSession()
		{
			if (this.chkController == "account")  //로그인일 경우 성공으로 반환
				return true;

			bool result = true;

			if(String.IsNullOrEmpty(this.MANAGE_NO))
				result = false;

			if (String.IsNullOrEmpty(this.MANAGE_GRADE))
				result = false;

			if (String.IsNullOrEmpty(this.CURRENT_LOGIN_EMAIL))
				result = false;

			if(this.chkAction == "Login" || this.chkAction == "login")	//로그인일 경우 성공으로 반환
				result = true;


			return result;
		}

		public bool chkPermission(string action, string type)
		{
			this.setAction = action;
			this.setType = type;
			HttpContext context = HttpContext.Current;
			this.MANAGE_GRADE = context.Session["MANAGE_GRADE"].ToString();


			bool result = false;
			string errorStr = "";

			DatabaseConnection act = new DatabaseConnection();

			string listQuery = " SELECT " + this.setType + " FROM esm_group_permission WHERE 1=1 ";
			listQuery += " AND GROUP_ID = " + this.MANAGE_GRADE;
			listQuery += " AND MENU_ID = '" + this.setAction + "' ";

			DataTable listDt = act.getQueryResult(listQuery, out errorStr);

			string chk = "0";
			if (listDt != null && listDt.Rows.Count != 0)
				chk = listDt.Rows[0][this.setType].ToString().Trim();

			if (chk == "1")
				result = true;

			return result;
		}
		
	}
}