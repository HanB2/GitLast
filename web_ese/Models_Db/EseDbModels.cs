using comm_dbconn;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_ese.Models;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Ese;

namespace web_ese.Models_Db
{
	public class EseDbModels : DatabaseConnection
	{

		AccountDbModels dbAct = new AccountDbModels();

		//est_user
		string[] selectColumn_EseInfo = {
			"EST_CODE" ,
			"ESE_CODE" ,
			"ESE_NAME" ,
			"ZIPCODE" ,
			"ADDR" ,
			"BIZNO" ,
			"REPRESENTATIVE" ,
			"TELNO_REP" ,
			"EMAIL_REP" ,
			"TASKMAN" ,
			"TELNO_TASK" ,
			"EMAIL_TASK" ,
			"MAR" ,
			"CREATEDATE" ,
			"STATUS" ,
			"MEMO" ,
			"HOMEPAGE" ,
			"API_KEY" 				
		};

		//WEB_ESE 계정관리 => 기본 정보
		public EseInfoModels GetEseInfo(EseInfoModels getModel)
		{
			string errorStr = "";

			EstSender model = new EstSender();
			string listQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_EseInfo) + " FROM est_sender WHERE ESE_CODE = '" + getModel.viewEseCode + "' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			

			if (listDt != null && listDt.Rows.Count != 0)
			{
				
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
				model.ESE_CODE = listDt.Rows[0]["ESE_CODE"].ToString().Trim();
				model.ESE_NAME = listDt.Rows[0]["ESE_NAME"].ToString().Trim();
				model.ZIPCODE = listDt.Rows[0]["ZIPCODE"].ToString().Trim();
				model.ADDR = listDt.Rows[0]["ADDR"].ToString().Trim();
				model.BIZNO = listDt.Rows[0]["BIZNO"].ToString().Trim();
				model.REPRESENTATIVE = listDt.Rows[0]["REPRESENTATIVE"].ToString().Trim();
				model.TELNO_REP = listDt.Rows[0]["TELNO_REP"].ToString().Trim();
				model.EMAIL_REP = listDt.Rows[0]["EMAIL_REP"].ToString().Trim();
				model.TASKMAN = listDt.Rows[0]["TASKMAN"].ToString().Trim();
				model.TELNO_TASK = listDt.Rows[0]["TELNO_TASK"].ToString().Trim();
				model.EMAIL_TASK = listDt.Rows[0]["EMAIL_TASK"].ToString().Trim();
				model.MAR = double.Parse(listDt.Rows[0]["MAR"].ToString().Trim());
				model.CREATEDATE = listDt.Rows[0]["CREATEDATE"].ToString().Trim();
				model.STATUS = int.Parse(listDt.Rows[0]["STATUS"].ToString().Trim());
				model.MEMO = listDt.Rows[0]["MEMO"].ToString().Trim();
				model.HOMEPAGE = listDt.Rows[0]["HOMEPAGE"].ToString().Trim();
				model.API_KEY = listDt.Rows[0]["API_KEY"].ToString().Trim();
				
				getModel.Item = model;
			}
			
			return getModel;
		}


		public string ChkUpdtEseInfo(EseInfoModels model)
		{

			string errorStr = "";
			string result = "";
			
			string listQuery = " SELECT count(*) as cnt FROM est_sender WHERE ESE_NAME = '"+ model.Item.ESE_NAME+ "' ";

			int reCnt = getQueryCnt(listQuery, out errorStr);

			if (model.act_type != null && model.schTxt2 == "ins")
			{
				if (reCnt > 0 && model.Item.ESE_NAME != null)
				{
					result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.ESE_EseInfo_SenderName + "]";

				}
				else
				{
					listQuery = " SELECT count(*) as cnt FROM est_sender WHERE BIZNO = '" + model.Item.BIZNO + "' ";

					reCnt = getQueryCnt(listQuery, out errorStr);
					if (reCnt > 0)
					{
						result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.ESE_EseInfo_BIZNO + "]";

					}
				}

			}
			else if (model.act_type != null && model.schTxt2 == "updt")
			{

				if (reCnt > 1 && model.Item.ESE_NAME != null)
				{
					result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.ESE_EseInfo_SenderName + "]";

				}
				else
				{
					listQuery = " SELECT count(*) as cnt FROM est_sender WHERE BIZNO = '" + model.Item.BIZNO + "' ";

					reCnt = getQueryCnt(listQuery, out errorStr);
					if (reCnt > 1)
					{
						result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.ESE_EseInfo_BIZNO + "]";

					}
				}

			}



		
			return result;
		}

		public string SetUpdtEseInfo(EseInfoModels model)
		{

			string result = "";
			string errorStr = "";
			string exeQueryStr = "";

			if(model.act_type == "USER")
			{
				exeQueryStr = " UPDATE est_sender SET  ";
				exeQueryStr += " TASKMAN = '" + model.Item.TASKMAN + "' ";
				exeQueryStr += ", TELNO_TASK = '" + model.Item.TELNO_TASK + "' ";
				exeQueryStr += ", EMAIL_TASK = '" + model.Item.EMAIL_TASK + "' ";
				exeQueryStr += " WHERE ESE_CODE = '" + model.viewEseCode + "' ";
			}
			
			if (model.act_type == "BASE")
			{
				exeQueryStr = " UPDATE est_sender SET  ";
				exeQueryStr += " ESE_NAME = '" + model.Item.ESE_NAME + "' ";
				exeQueryStr += ", ZIPCODE = '" + model.Item.ZIPCODE + "' ";
				exeQueryStr += ", ADDR = '" + model.Item.ADDR + "' ";
				exeQueryStr += ", BIZNO = '" + model.Item.BIZNO + "' ";
				exeQueryStr += ", REPRESENTATIVE = '" + model.Item.REPRESENTATIVE + "' ";
				exeQueryStr += ", TELNO_REP = '" + model.Item.TELNO_REP + "' ";
				exeQueryStr += ", EMAIL_REP = '" + model.Item.EMAIL_REP + "' ";
				exeQueryStr += ", MEMO = '" + model.Item.MEMO + "' ";
				exeQueryStr += ", HOMEPAGE = '" + model.Item.HOMEPAGE + "' ";
				exeQueryStr += ", API_KEY = '" + model.Item.API_KEY + "' ";
				exeQueryStr += " WHERE ESE_CODE = '" + model.viewEseCode + "' ";
			}
			
			if (exeQuery(exeQueryStr, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}

			return result;
		}



		//WEB_ESE 계정 관리 => 계좌 관리
		string[] EseActOPT_KEY = {
			"setting_SwiftCode",
			"setting_BankAddr",
			"setting_AccountNum",
			"setting_ReceiverName_en",
			"setting_Memo"

		};

		public EseAccountModels GetEseAccountModel(EseAccountModels model)
		{
			string errorStr = "";
			
			string sqlQueryStr = "SELECT SET_KEY, SET_VALUE, ESE_CODE FROM ese_settings WHERE ESE_CODE = '" + model.viewEseCode + "' AND SET_KEY in ('" + string.Join("','", EseActOPT_KEY) + "')";
			
			DataTable dt = getQueryResult(sqlQueryStr, out errorStr);

			if (dt == null || dt.Rows.Count == 0)
			{
				List<string> queryList = new List<string>();
				for (int i = 0; i < EseActOPT_KEY.Length; i++)
				{
					queryList.Add("INSERT INTO  ese_settings (SET_KEY, SET_VALUE, ESE_CODE) VALUES ('" + EseActOPT_KEY[i] + "', '', '" + model.viewEseCode + "') ");
				}
				exeQuery(queryList, out errorStr);
			}
			else
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_SwiftCode") { model.setting_SwiftCode = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_BankAddr") { model.setting_BankAddr = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_AccountNum") { model.setting_AccountNum = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_ReceiverName_en") { model.setting_ReceiverName_en = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_Memo") { model.setting_Memo = dt.Rows[i]["SET_VALUE"].ToString(); }
				}
			}

			return model;
		}


		public bool SetEseAccountModel(EseAccountModels model)
		{
			string errorStr = "";
			bool result = false;
			
			List<string> queryList = new List<string>();
			queryList.Add("UPDATE ese_settings SET SET_VALUE ='" + model.setting_SwiftCode + "' WHERE SET_KEY ='setting_SwiftCode' AND ESE_CODE= '" + model.viewEseCode + "' ");
			queryList.Add("UPDATE ese_settings SET SET_VALUE ='" + model.setting_BankAddr + "' WHERE SET_KEY ='setting_BankAddr'AND ESE_CODE= '" + model.viewEseCode + "' ");
			queryList.Add("UPDATE ese_settings SET SET_VALUE ='" + model.setting_AccountNum + "' WHERE SET_KEY ='setting_AccountNum' AND ESE_CODE='" + model.viewEseCode + "' ");
			queryList.Add("UPDATE ese_settings SET SET_VALUE ='" + model.setting_ReceiverName_en + "' WHERE SET_KEY ='setting_ReceiverName_en'AND ESE_CODE= '" + model.viewEseCode + "' ");
			queryList.Add("UPDATE ese_settings SET SET_VALUE ='" + model.setting_Memo + "' WHERE SET_KEY ='setting_Memo'AND ESE_CODE= '" + model.viewEseCode + "' ");

			result = exeQuery(queryList, out errorStr);

			return result;
		}


		//WEB_ESE 계정 관리 => 내 계정 관리
		public string SetEseInfoMy(EseInfoMyModels model)
		{

			string result = "";
			string errorStr = "";
			string exeQueryStr = " UPDATE ese_sender SET  ";

			exeQueryStr += " EST_CODE = '" + model.Item.EST_CODE + "' ";
			exeQueryStr += ", ESE_CODE = '" + model.Item.ESE_CODE + "' ";
			exeQueryStr += ", EMAIL = '" + model.Item.EMAIL + "' ";
			exeQueryStr += ", PASSWD = '" + model.Item.PASSWD + "' ";
			exeQueryStr += ", USERNAME = '" + model.Item.USERNAME + "' ";
			exeQueryStr += ", TELNO = '" + model.Item.TELNO + "' ";
			exeQueryStr += ", GROUP_ID = " + model.Item.GROUP_ID + " ";
			exeQueryStr += ", CREATETIME = '" + model.Item.CREATETIME + "' ";
			exeQueryStr += ", DEPARTMENT = '" + model.Item.DEPARTMENT + "' ";
			exeQueryStr += ", POSITION = '" + model.Item.POSITION + "' ";
			exeQueryStr += ", MEMO = '" + model.Item.MEMO + "' ";
			exeQueryStr += ", STATUS = " + model.Item.STATUS + " ";
			exeQueryStr += " WHERE ESE_CODE = '" + model.viewEseCode + "' ";

			if (exeQuery(exeQueryStr, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}

			return result;
		}




		//WEB_ESE 계정 관리 => 계정 등급 관리
		public EseGradeModels GetEseGradeList(EseGradeModels model)
		{
			string errorStr = "";
			string listQuery = " SELECT GROUP_ID , GROUP_NAME FROM ese_group WHERE ESE_CODE = '"+ model.Item.ESE_CODE + "' order by GROUP_ID ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EseGroup temp = new EseGroup();
					temp.GROUP_ID = int.Parse(listDt.Rows[i]["GROUP_ID"].ToString().Trim());
					temp.GROUP_NAME = listDt.Rows[i]["GROUP_NAME"].ToString().Trim();
					model.Items.Add(temp);
					model.chkCnt = i;
				}
			}

			return model;
		}


		public EseGradeViewModels GetEseGradeView(EseGradeViewModels model)
		{
			string errorStr = "";

			//그룹 가져오기
			string listQuery = " SELECT GROUP_ID , GROUP_NAME FROM ese_group WHERE GROUP_ID = " + model.act_key;

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.Item.GROUP_ID = int.Parse(listDt.Rows[0]["GROUP_ID"].ToString().Trim());
				model.Item.GROUP_NAME = listDt.Rows[0]["GROUP_NAME"].ToString().Trim();
			}

			//권한 가져오기
			listQuery = " SELECT GROUP_ID , MENU_ID, PER_SELECT, PER_INSERT, PER_UPDATE, PER_DELETE FROM ese_group_permission WHERE GROUP_ID = " + model.Item.GROUP_ID;

			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EseGroupPermission temp = new EseGroupPermission();
					temp.GROUP_ID = int.Parse(listDt.Rows[i]["GROUP_ID"].ToString().Trim());
					temp.MENU_ID = listDt.Rows[i]["MENU_ID"].ToString().Trim();
					temp.PER_SELECT = int.Parse(listDt.Rows[i]["PER_SELECT"].ToString().Trim());
					temp.PER_INSERT = int.Parse(listDt.Rows[i]["PER_INSERT"].ToString().Trim());
					temp.PER_UPDATE = int.Parse(listDt.Rows[i]["PER_UPDATE"].ToString().Trim());
					temp.PER_DELETE = int.Parse(listDt.Rows[i]["PER_DELETE"].ToString().Trim());
					temp.CHK_PER_SELECT = false;
					if (temp.PER_SELECT == 1)
						temp.CHK_PER_SELECT = true;
					temp.CHK_PER_INSERT = false;
					if (temp.PER_INSERT == 1)
						temp.CHK_PER_INSERT = true;
					temp.CHK_PER_UPDATE = false;
					if (temp.PER_UPDATE == 1)
						temp.CHK_PER_UPDATE = true;
					temp.CHK_PER_DELETE = false;
					if (temp.PER_DELETE == 1)
						temp.CHK_PER_DELETE = true;
					model.Items.Add(temp);
				}
			}
			else
			{   //등록된 데이터가 없을 경우 
				CommFunction comF = new CommFunction();

				List<schTypeArray> tempGrade = comF.GetGradeList();

				foreach (schTypeArray tempItem in tempGrade)
				{
					EseGroupPermission temp = new EseGroupPermission();

					temp.GROUP_ID = model.Item.GROUP_ID;
					temp.MENU_ID = tempItem.opt_key;
					temp.MENU_NAME = tempItem.opt_value;
					temp.PER_DELETE = 0;
					temp.PER_INSERT = 0;
					temp.PER_SELECT = 0;
					temp.PER_UPDATE = 0;
					temp.CHK_PER_SELECT = true;
					temp.CHK_PER_INSERT = true;
					temp.CHK_PER_UPDATE = true;
					temp.CHK_PER_DELETE = true;
					model.Items.Add(temp);
				}
			}

			return model;
		}

		public string ChkUpdtEseGrade(EseGradeViewModels model)
		{
			//esecode
			HttpContext context = HttpContext.Current;
			string ESE_CODE = context.Session["ESE_CODE"].ToString();

			string errorStr = "";
			string result = "";

			string listQuery = " SELECT count(*) as cnt FROM ese_group WHERE group_name = '" + model.Item.GROUP_NAME + "'  AND ESE_CODE = '" + ESE_CODE + "' ";


			int reCnt = getQueryCnt(listQuery, out errorStr);

			if (model.act_type != null && model.schTxt2 == "ins")
			{
				if (reCnt > 0 && model.Item.GROUP_NAME != null)
				{
					result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.ESE_EseGrade_GroupName + "]";

				}
			}
			else if (model.act_type != null && model.schTxt2 == "updt")
			{
				if (reCnt > 1 && model.Item.GROUP_NAME != null)
				{
					result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.ESE_EseGrade_GroupName + "]";

				}

			}


				return result;
		}


		public string SetEseGrade(EseGradeViewModels model)
		{
			//세션에 저장된 ESE코드 가져오기
			HttpContext context = HttpContext.Current;
			model.Item.ESE_CODE = context.Session["ESE_CODE"].ToString();

			string errorStr = "";
			string result = "";
			string exeQueryStr = "";
			List<string> queryList = new List<string>();

			if (model.act_type != null && model.act_type == "ins")
			{
				//ese_group 그룹 
				exeQueryStr = " INSERT INTO ese_group ( GROUP_ID , GROUP_NAME, ESE_CODE )VALUES(  ";
				exeQueryStr += " " + model.Item.GROUP_ID + "";
				exeQueryStr += ", '" + model.Item.GROUP_NAME + "' ";
				exeQueryStr += ", '" + model.Item.ESE_CODE + "' ";
				exeQueryStr += " ) ";

				queryList.Add(exeQueryStr);

				//ese_group_permission 권한
				foreach (EseGroupPermission temp in model.Items)
				{
					exeQueryStr = " INSERT INTO ese_group_permission ( GROUP_ID , ESE_CODE, MENU_ID, PER_SELECT, PER_INSERT, PER_UPDATE, PER_DELETE )VALUES(  ";
					exeQueryStr += " ( SELECT MAX(GROUP_ID) FROM ese_group ) ";// + model.Item.GROUP_ID;
					exeQueryStr += " , '" + model.Item.ESE_CODE + "'";
					exeQueryStr += " , '" + temp.MENU_ID + "'";
					exeQueryStr += " , " + temp.PER_SELECT;
					exeQueryStr += " , " + temp.PER_INSERT;
					exeQueryStr += " , " + temp.PER_UPDATE;
					exeQueryStr += " , " + temp.PER_DELETE;
					exeQueryStr += " ) ";

					queryList.Add(exeQueryStr);
				}
			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				//ese_group 그룹 
				exeQueryStr = " UPDATE ese_group SET ";
				exeQueryStr += "GROUP_NAME = '" + model.Item.GROUP_NAME + "'";
				exeQueryStr += " WHERE GROUP_ID = " + model.Item.GROUP_ID;

				queryList.Add(exeQueryStr);

				queryList.Add(" DELETE FROM ese_group_permission WHERE GROUP_ID = '" + model.Item.GROUP_ID + "' ");

				//ese_group_permission 권한
				foreach (EseGroupPermission temp in model.Items)
				{
					exeQueryStr = " INSERT INTO ese_group_permission ( GROUP_ID , ESE_CODE, MENU_ID, PER_SELECT, PER_INSERT, PER_UPDATE, PER_DELETE )VALUES(  ";
					exeQueryStr += " , '" + model.Item.GROUP_ID + "'";
					exeQueryStr += " , '" + model.Item.ESE_CODE + "'";
					exeQueryStr += " , '" + temp.MENU_ID + "'";
					exeQueryStr += " , " + temp.PER_SELECT;
					exeQueryStr += " , " + temp.PER_INSERT;
					exeQueryStr += " , " + temp.PER_UPDATE;
					exeQueryStr += " , " + temp.PER_DELETE;
					exeQueryStr += " ) ";

					queryList.Add(exeQueryStr);
				}
			}
			else
			{
				result = "잘못된 접근입니다.";
				return result;
			}

			if (exeQuery(queryList, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}

			return result;
		}


		public string DelEseGrade(int SEQNO)
		{
			string errorStr = "";
			string result = "";
			List<string> queryList = new List<string>();
			string exeQueryStr = "";

			exeQueryStr = " SELECT count(*) as cnt FROM est_sender WHERE GROUP_ID = " + SEQNO;

			int chkCnt = getQueryCnt(exeQueryStr, out errorStr);   //해당 등급을 사용 중인 계정 수 구하기

			if (chkCnt >= 1)	//해당 등급을 사용 중인 계정이 있을 경우 삭제로직을 제한
			{
				result = "실패.";
				return result;
			}
			
			//해당 등급을 사용하는 계정이 없을 경우 계정 등급과 계정 권한 데이터를 모두 삭제 한다.
			exeQueryStr = " DELETE FROM ese_group WHERE GROUP_ID = " + SEQNO;
			queryList.Add(exeQueryStr);

			exeQueryStr = " DELETE FROM ese_group_permission WHERE GROUP_ID = " + SEQNO;
			queryList.Add(exeQueryStr);

			result = "실패.";
			if (exeQuery(queryList, out errorStr))
				result = "성공.";

			return result;
		}




		//WEB_ESE 계정관리 => 계정 관리
		public EseManagerModels GetEseManagerList(EseManagerModels model)
		{
			HttpContext context = HttpContext.Current;
			string ESE_CODE = context.Session["ESE_CODE"].ToString();

			string errorStr = "";

			string listQuery = "";

			listQuery = " SELECT SEQNO, EMAIL, PASSWD, USERNAME, TELNO, eu.GROUP_ID,IFNULL(eg.GROUP_NAME, 'MASTER') as GROUP_NAME,CREATETIME,DEPARTMENT,POSITION,MEMO,STATUS ";
			listQuery += " FROM ese_user eu left outer join ese_group eg on eu.GROUP_ID = eg.GROUP_ID WHERE eu.ESE_CODE = '" + ESE_CODE + "' ORDER BY SEQNO";

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EseUser temp = new EseUser();
					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.EMAIL = listDt.Rows[i]["EMAIL"].ToString().Trim();
					temp.PASSWD = listDt.Rows[i]["PASSWD"].ToString().Trim();
					temp.USERNAME = listDt.Rows[i]["USERNAME"].ToString().Trim();
					temp.TELNO = listDt.Rows[i]["TELNO"].ToString().Trim();
					temp.GROUP_ID = int.Parse(listDt.Rows[i]["GROUP_ID"].ToString().Trim());
					temp.GROUP_NAME = listDt.Rows[i]["GROUP_NAME"].ToString().Trim();
					temp.CREATETIME = listDt.Rows[i]["CREATETIME"].ToString().Trim();
					temp.DEPARTMENT = listDt.Rows[i]["DEPARTMENT"].ToString().Trim();
					temp.POSITION = listDt.Rows[i]["POSITION"].ToString().Trim();
					temp.MEMO = listDt.Rows[i]["MEMO"].ToString().Trim();
					temp.STATUS = int.Parse(listDt.Rows[i]["STATUS"].ToString().Trim());
					temp.STATUS_TEXT = "미사용";
					if (temp.STATUS == 0)
						temp.STATUS_TEXT = "사용";

					model.Items.Add(temp);
					model.chkCnt = i;
				}
			}

			//계정 등급이 하나도 등록 되어 있지 않은 경우를 체크를 위해 계정 등급의 카운트를 가져옴
			listQuery = " SELECT count(*) as cnt FROM ese_group WHERE ESE_CODE = '" + ESE_CODE + "' ";

			model.chkGRADE = getQueryCnt(listQuery, out errorStr);


			return model;
		}

		public EseUser GetEseManagerView(EseManagerModels getModel)
		{
			string errorStr = "";

			EseUser model = new EseUser();

			string listQuery = " SELECT SEQNO, EMAIL, PASSWD, USERNAME, TELNO, GROUP_ID, CREATETIME, DEPARTMENT, POSITION, MEMO, STATUS  FROM ese_user WHERE SEQNO = " + getModel.act_key;

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.EMAIL = listDt.Rows[0]["EMAIL"].ToString().Trim();
				//model.PASSWD = listDt.Rows[0]["PASSWD"].ToString().Trim();
				model.USERNAME = listDt.Rows[0]["USERNAME"].ToString().Trim();
				model.TELNO = listDt.Rows[0]["TELNO"].ToString().Trim();
				model.GROUP_ID = int.Parse(listDt.Rows[0]["GROUP_ID"].ToString().Trim());
				model.CREATETIME = listDt.Rows[0]["CREATETIME"].ToString().Trim();
				model.DEPARTMENT = listDt.Rows[0]["DEPARTMENT"].ToString().Trim();
				model.POSITION = listDt.Rows[0]["POSITION"].ToString().Trim();
				model.MEMO = listDt.Rows[0]["MEMO"].ToString().Trim();
				model.STATUS = int.Parse(listDt.Rows[0]["STATUS"].ToString().Trim());
			}

			return model;
		}

		public string ChkUpdtEseManager(EseManagerModels model)
		{
			//est, esm도
			string errorStr = "";
			string result = "";
			string listQuery = "";
			int reCnt = 0;
			
			listQuery = " SELECT count(*) as cnt FROM ese_user WHERE EMAIL = '" + model.Item.EMAIL + "' ";

			reCnt = getQueryCnt(listQuery, out errorStr);
			
			if (reCnt > 0)
			{
				result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.ESE_EseManagerView_Email + "]";

				return result;
			}

			listQuery = " SELECT count(*) as cnt FROM est_user WHERE EMAIL = '" + model.Item.EMAIL + "' ";
			
			reCnt = getQueryCnt(listQuery, out errorStr);
			
			if (reCnt > 0)
			{
				result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.ESE_EseManagerView_Email + "]";

				return result;
			}

			listQuery = " SELECT count(*) as cnt FROM esm_user WHERE EMAIL = '" + model.Item.EMAIL + "' ";

			reCnt = getQueryCnt(listQuery, out errorStr);

			if (reCnt > 0)
			{
				result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.ESE_EseManagerView_Email + "]";

				return result;
			}


			return result;
		}



		public List<schTypeArray> GroupIdSelectBox()
		{
			HttpContext context = HttpContext.Current;
			string ESE_CODE = context.Session["ESE_CODE"].ToString();

			string errorStr = "";

			string listQuery = " SELECT GROUP_ID,GROUP_NAME FROM ese_group WHERE ESE_CODE = '"+ ESE_CODE + "' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["GROUP_NAME"].ToString().Trim(), opt_key = listDt.Rows[i]["GROUP_ID"].ToString().Trim() });
				}
			}

			return model;
		}


		public string SetEseManager(EseManagerModels model)
		{
			HttpContext context = HttpContext.Current;
			string ESE_CODE = context.Session["ESE_CODE"].ToString();
			string EST_CODE = context.Session["EST_CODE"].ToString();

			string errorStr = "";
			string result = "";
			string exeQueryStr = "";

			if (model.act_type != null && model.act_type == "ins")
			{
				exeQueryStr = " INSERT INTO ese_user ( EST_CODE, ESE_CODE, EMAIL, PASSWD, USERNAME, TELNO, GROUP_ID, DEPARTMENT, POSITION, MEMO, STATUS )VALUES(  ";
				exeQueryStr += " '" + EST_CODE + "'";
				exeQueryStr += " , '" + ESE_CODE + "'";
				exeQueryStr += " , '" + model.Item.EMAIL + "'";
				exeQueryStr += " , '" + dbAct.AESEncrypt_256("etomarsPw", model.Item.PASSWD) + "'";
				exeQueryStr += " , '" + model.Item.USERNAME + "'";
				exeQueryStr += " , '" + model.Item.TELNO + "'";
				exeQueryStr += " , " + model.Item.GROUP_ID;
				exeQueryStr += " , '" + model.Item.DEPARTMENT + "'";
				exeQueryStr += " , '" + model.Item.POSITION + "'";
				exeQueryStr += " , '" + model.Item.MEMO + "'";
				exeQueryStr += " , " + model.Item.STATUS;
				exeQueryStr += " ) ";

			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				exeQueryStr = " UPDATE ese_user SET ";
				//exeQueryStr += " ,PASSWD = '" + model.Item.PASSWD + "'";
				exeQueryStr += " USERNAME = '" + model.Item.USERNAME + "'";
				exeQueryStr += " ,TELNO = '" + model.Item.TELNO + "'";
				exeQueryStr += " ,GROUP_ID = " + model.Item.GROUP_ID;
				exeQueryStr += " ,DEPARTMENT = '" + model.Item.DEPARTMENT + "'";
				exeQueryStr += " ,POSITION = '" + model.Item.POSITION + "'";
				exeQueryStr += " ,MEMO = '" + model.Item.MEMO + "'";
				exeQueryStr += " ,STATUS = " + model.Item.STATUS;
				exeQueryStr += " WHERE SEQNO = " + model.Item.SEQNO;
			}
			else
			{
				result = "잘못된 접근입니다.";
				return result;
			}

			if (exeQuery(exeQueryStr, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}

			return result;
		}


		public string DelEseManager(int SEQNO)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM ese_user WHERE SEQNO = " + SEQNO;

			result = "실패.";
			if (exeQuery(exeQueryStr, out errorStr))
				result = "성공.";

			return result;
		}

		/*
		샌더 업체 정보
		est_sender

		계좌정보 
		est_settings

		*/

		public string changePassword(EseInfoMyModels model)
		{
			string errorStr = "";
			string resultStr = "성공";



			string listQuery = " SELECT count(*) as cnt FROM ese_user WHERE EMAIL = '" + model.email + "' AND PASSWD = '"+ dbAct.AESEncrypt_256("etomarsPw", model.passwd) + "' ";

			int resultCnt = getQueryCnt(listQuery, out errorStr);


			if (resultCnt == 0)
			{
				resultStr = "현재 비밀번호 값이 일치하지 않습니다.";
			}
			else
			{
				//패스워드 업데이트
				string exeQueryStr = "";
				exeQueryStr = " UPDATE ese_user SET ";
				exeQueryStr += " PASSWD = '" + dbAct.AESEncrypt_256("etomarsPw", model.passwd_new) + "'";
				exeQueryStr += " WHERE EMAIL = '" + model.email + "'";
				
				if (!exeQuery(exeQueryStr, out errorStr))
					resultStr = "실패.";
			}


			return resultStr;
		}




	}
}