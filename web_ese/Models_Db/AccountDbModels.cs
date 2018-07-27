using comm_dbconn;
using comm_global;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using web_ese.Models;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Db
{
	public class AccountDbModels : DatabaseConnection
	{

		public string resetPassword(string email)
		{
			string resultStr = "test";

			CommFunction cf = new CommFunction();
			SendMailModel model = new SendMailModel();

			bool a = cf.SendMail(model);

			return resultStr;
		}




		public EseUser loginChk(AccountLoginModel loginModel)
		{
			string sqlQuery = "";
			sqlQuery += " SELECT esu.SEQNO, esu.EST_CODE, esu.ESE_CODE, esu.EMAIL, esu.USERNAME, esu.GROUP_ID, esu.STATUS as chkSTATUS , est.STATUS ";
			sqlQuery += " FROM ese_user esu LEFT OUTER JOIN est_sender est on esu.ESE_CODE = est.ESE_CODE ";
			sqlQuery += " WHERE EMAIL = '" + loginModel.Email + "' AND PASSWD = '" + AESEncrypt_256("etomarsPw", loginModel.Password) + "' ";



			string resultStr = "";
			
			DataTable dt = GetDataTableMySQL(sqlQuery, out resultStr);
			EseUser model = new EseUser();
			if (dt != null && dt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(dt.Rows[0]["SEQNO"].ToString().Trim());
				model.EST_CODE = dt.Rows[0]["EST_CODE"].ToString().Trim();
				model.ESE_CODE = dt.Rows[0]["ESE_CODE"].ToString().Trim();
				model.EMAIL = dt.Rows[0]["EMAIL"].ToString().Trim();
				model.USERNAME = dt.Rows[0]["USERNAME"].ToString().Trim();
				model.GROUP_ID = int.Parse(dt.Rows[0]["GROUP_ID"].ToString().Trim());
				model.chkSTATUS = int.Parse(dt.Rows[0]["chkSTATUS"].ToString().Trim());
				model.STATUS = int.Parse(dt.Rows[0]["STATUS"].ToString().Trim());
			}
			else
			{
				model = null;
			}

			return model;
		}


		public void loginHis(CommLoginLog model)
		{
			string errorStr = "";
			string login_his_str = "INSERT INTO comm_login_log (ESE_CODE, EMAIL, IPADDR, TYPE) values ('"+ model.ESE_CODE+"', '" + model.EMAIL + "', '" + model.IPADDR + "', 'ese') ";
			exeQuery(login_his_str, out errorStr);
		}


		//이메일 중복 체크
		public bool chkDuplEmail(AccountRegistrationModel model)
		{
			bool result = false;
			string errorStr = "";

			string login_his_str = "SELECT count(SEQNO) as cnt  FROM ese_user WHERE EMAIL = '" + model.Email + "' ";
			int cnt = getQueryCnt(login_his_str, out errorStr);
			if(cnt > 0)
				result = true;

			login_his_str = "SELECT count(SEQNO) as cnt  FROM est_user WHERE EMAIL = '" + model.Email + "' ";
			cnt = getQueryCnt(login_his_str, out errorStr);
			if (cnt > 0)
				result = true;

			login_his_str = "SELECT count(SEQNO) as cnt  FROM esm_user WHERE EMAIL = '" + model.Email + "' ";
			cnt = getQueryCnt(login_his_str, out errorStr);
			if (cnt > 0)
				result = true;


			return result;
		}

		//EST CODE 유효성 체크
		public bool chkEstCode(AccountRegistrationModel model)
		{
			bool result = false;

			if(string.IsNullOrEmpty(model.EST_CODE))	//EST 코드를 입력 하지 않은 경우 유효성 체크하지 않음
				return result;


			string errorStr = "";
			string login_his_str = "SELECT count(SEQNO) as cnt  FROM esm_station WHERE USERINPUTCODE = '" + model.EST_CODE + "' ";
			int cnt = getQueryCnt(login_his_str, out errorStr);
			if (cnt == 0)
				result = true;

			return result;
		}

		//회원가입 setRegister
		public bool setRegister(AccountRegistrationModel model)
		{
			string errorStr = "";
			EseUser setModel = new EseUser();
			GlobalFunction gblFunction = new GlobalFunction();

			//================================================================================
			//ESE코드 생성 
			string sqlQuery = " SELECT IFNULL(MAX(ESE_CODE),0) as ESE_CODE  FROM ese_user ";
			
			DataTable dt = GetDataTableMySQL(sqlQuery, out errorStr);

			int esecode = 0;

			if (dt != null && dt.Rows.Count != 0)
				esecode = int.Parse(dt.Rows[0]["ESE_CODE"].ToString().Trim());

			string ESE_CODE = string.Format("{0:D8}", esecode + 1);
			//================================================================================




			//================================================================================
			//ESE정보 등록

			List<string> queryList = new List<string>();
			string exeQueryStr = "";
			/*
			// 1. ESE 계정 권한 등록
			string exeQueryStr = "INSERT INTO ese_group (GROUP_NAME, ESE_CODE) values (";
			exeQueryStr += " '최고관리자' ";
			exeQueryStr += " ,'" + ESE_CODE + "' ";
			exeQueryStr += " ) ";
			
			queryList.Add(exeQueryStr);


			// 2. ESE 계정 개별 권한 등록
			CommFunction com = new CommFunction();
			List<schTypeArray> tmpGrade = com.GetGradeList();
			foreach (schTypeArray temp in tmpGrade)
			{
				exeQueryStr = " INSERT INTO ese_group_permission ( GROUP_ID , ESE_CODE, MENU_ID, PER_SELECT, PER_INSERT, PER_UPDATE, PER_DELETE )VALUES(  ";
				exeQueryStr += " ( SELECT MAX(GROUP_ID) FROM ese_group ) ";// + model.Item.GROUP_ID;
				exeQueryStr += " , '" + ESE_CODE + "'";
				exeQueryStr += " , '" + temp.opt_key + "'";
				exeQueryStr += " , 1";
				exeQueryStr += " , 1";
				exeQueryStr += " , 1";
				exeQueryStr += " , 1";
				exeQueryStr += " ) ";

				queryList.Add(exeQueryStr);
			}
			*/

			// 3. ESE 계정 등록
			exeQueryStr = "INSERT INTO ese_user (EST_CODE, ESE_CODE, EMAIL, PASSWD, GROUP_ID, STATUS) values (";
			if (string.IsNullOrEmpty(model.EST_CODE))
			{
				exeQueryStr += " ''";
			}
			else
			{
				exeQueryStr += " (SELECT IFNULL(EST_CODE, '')  FROM esm_station WHERE USERINPUTCODE = '" + model.EST_CODE + "') ";
			}

			exeQueryStr += " ,'" + ESE_CODE + "' ";
			exeQueryStr += " ,'" + model.Email + "' ";
			exeQueryStr += " ,'" + AESEncrypt_256("etomarsPw", model.Password) + "' ";
			exeQueryStr += " , 0";
			exeQueryStr += " , 0 " ;
			exeQueryStr += " ) ";

			queryList.Add(exeQueryStr);


			// 4. ESE 기본정보 등록	
			if (string.IsNullOrEmpty(model.EST_CODE))
			{
				queryList.Add(" INSERT INTO est_sender (EST_CODE, ESE_CODE,STATUS) VALUES ( '" + model.EST_CODE + "', '" + ESE_CODE + "', 2)");
			}
			else
			{
				queryList.Add(" INSERT INTO est_sender (EST_CODE, ESE_CODE,STATUS) VALUES ( (SELECT IFNULL(EST_CODE, '')  FROM esm_station WHERE USERINPUTCODE = '" + model.EST_CODE + "'), '" + ESE_CODE + "', 2)");
			}
			


			// 5. ESE 계좌정보 등록
			string[] EseActOPT_KEY = {
				"setting_SwiftCode",
				"setting_BankAddr",
				"setting_AccountNum",
				"setting_ReceiverName_en",
				"setting_Memo"
			};
			for (int i = 0; i < EseActOPT_KEY.Length; i++)
			{
				queryList.Add("INSERT INTO  ese_settings (SET_KEY, SET_VALUE, ESE_CODE) VALUES ('" + EseActOPT_KEY[i] + "', '', '" + ESE_CODE + "') ");
			}


			return exeQuery(queryList, out errorStr);
		}

		public string AESEncrypt_256(string InputText, string Password)
		{
			RijndaelManaged RijndaelCipher = new RijndaelManaged();

			// 입력받은 문자열을 바이트 배열로 변환  
			byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);

			// 딕셔너리 공격을 대비해서 키를 더 풀기 어렵게 만들기 위해서   
			// Salt를 사용한다.  
			byte[] Salt = Encoding.ASCII.GetBytes(Password);

			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

			// Create a encryptor from the existing SecretKey bytes.  
			// encryptor 객체를 SecretKey로부터 만든다.  
			// Secret Key에는 32바이트  
			// Initialization Vector로 16바이트를 사용  
			ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

			MemoryStream memoryStream = new MemoryStream();

			// CryptoStream객체를 암호화된 데이터를 쓰기 위한 용도로 선언  
			CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

			cryptoStream.Write(PlainText, 0, PlainText.Length);

			cryptoStream.FlushFinalBlock();

			byte[] CipherBytes = memoryStream.ToArray();

			memoryStream.Close();
			cryptoStream.Close();

			string EncryptedData = Convert.ToBase64String(CipherBytes);

			return EncryptedData;
		}
	}
}