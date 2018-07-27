using comm_dbconn;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Diagnostics;
using System.Web.Mvc;
namespace web_admin.Models
{
    public class CommonController : Controller
    {
		// 메뉴 권한 체크
		// actionName : 
		public Boolean MenuAuthCheck( string actionName, string type , string userId)
        {

			//권한 가져오기

			//REAL ==================================
			/*
			string query = "SELECT AUTH FROM esm_user ";
			query += string.Format(" WHERE EMAIL = '{0}'", userId);

			string err1 = "";
			DataTable dt1 = DatabaseConnection.GetDataTableMySQL(query, out err1);

			if (dt1 == null)	//권한 데이터 없을 경우 권한 없음 처리
				return false;

			string jsonStr = dt1.Rows[0]["AUTH"].ToString();
			
			JObject obj = JObject.Parse(jsonStr);
			int auth = (int)obj[actionName];
			*/
			//REAL ==================================


			//TEST ===================================
			string json = @"{test : 10}";
			
			JObject obj = JObject.Parse(json);
			int auth = (int)obj["test"];
			Debug.WriteLine(auth, " : kms test");
			//TEST ===================================


			string auth_Binary = Convert.ToString(auth, 2).PadLeft(4,'0');   //0000 4자리 문자열 형식 권한 int형으로 저장된 데이터 2진법으로 변환

			//type : list 조회권한 체크 auth : 1XXX 인경우만 통과 
			if (type == "list" && auth_Binary.Substring(1, 1) == "1")
				return true;

			//type : ins 조회권한 체크 auth : X1XX 인경우만 통과 
			if (type == "ins" && auth_Binary.Substring(2, 1) == "1")
				return true;

			//type : updt 수정권한 체크 auth : XX1X 인경우만 통과 
			if (type == "updt" && auth_Binary.Substring(3, 1) == "1")
				return true;

			//type : del 삭제권한 체크 auth : XXX1 인경우만 통과 
			if (type == "del" && auth_Binary.Substring(4, 1) == "1")
				return true;

			return false;
        }



		
    }
}