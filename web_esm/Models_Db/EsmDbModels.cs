using comm_dbconn;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_esm.Models;
using web_esm.Models_Act.Comm;
using web_esm.Models_Act.Esm;

namespace web_esm.Models_Db
{
	public class EsmDbModels : DatabaseConnection
	{
		
		//ESM 관리자 설정 ESM 계정 그룹 관리
		public EsmGradeModels GetEsmGradeList()
		{
			string errorStr = "";
			string listQuery = " SELECT GROUP_ID , GROUP_NAME FROM esm_group order by GROUP_ID ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			EsmGradeModels model = new EsmGradeModels();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EsmGroup temp = new EsmGroup();
					temp.GROUP_ID = int.Parse(listDt.Rows[i]["GROUP_ID"].ToString().Trim());
					temp.GROUP_NAME = listDt.Rows[i]["GROUP_NAME"].ToString().Trim();
					model.Items.Add(temp);
				}
			}
			
			return model;
		}


		public EsmGradeViewModels GetEsmGradeView(EsmGradeViewModels model)
		{
			string errorStr = "";

			//그룹 가져오기
			string listQuery = " SELECT GROUP_ID , GROUP_NAME FROM esm_group WHERE GROUP_ID = " + model.act_key;

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			

			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.Item.GROUP_ID = int.Parse(listDt.Rows[0]["GROUP_ID"].ToString().Trim());
				model.Item.GROUP_NAME = listDt.Rows[0]["GROUP_NAME"].ToString().Trim();
			}

			//권한 가져오기
			listQuery = " SELECT GROUP_ID , MENU_ID, PER_SELECT, PER_INSERT, PER_UPDATE, PER_DELETE FROM esm_group_permission WHERE GROUP_ID = " + model.Item.GROUP_ID +" ORDER BY SEQNO ";

			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EsmGroupPermisson temp = new EsmGroupPermisson();
					temp.GROUP_ID = int.Parse(listDt.Rows[i]["GROUP_ID"].ToString().Trim());
					temp.MENU_ID = listDt.Rows[i]["MENU_ID"].ToString().Trim();
					temp.PER_SELECT = int.Parse(listDt.Rows[i]["PER_SELECT"].ToString().Trim());
					temp.PER_INSERT = int.Parse(listDt.Rows[i]["PER_INSERT"].ToString().Trim());
					temp.PER_UPDATE = int.Parse(listDt.Rows[i]["PER_UPDATE"].ToString().Trim());
					temp.PER_DELETE = int.Parse(listDt.Rows[i]["PER_DELETE"].ToString().Trim());
					temp.CHK_PER_SELECT = false;
					if(temp.PER_SELECT == 1 )
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
			{	//등록된 데이터가 없을 경우 
				CommFunction comF = new CommFunction();

				List<schTypeArray> tempGrade = comF.GetGradeList();

				foreach (schTypeArray tempItem in tempGrade)
				{
					EsmGroupPermisson temp = new EsmGroupPermisson();

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

		public string SetEsmGrade(EsmGradeViewModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";
			List<string> queryList = new List<string>();
			
			if (model.act_type != null && model.act_type == "ins")
			{
				//esm_group 그룹 
				exeQueryStr = " INSERT INTO esm_group ( GROUP_ID , GROUP_NAME )VALUES(  ";
				exeQueryStr += " " + model.Item.GROUP_ID + "";
				exeQueryStr += ", '" + model.Item.GROUP_NAME + "' ";
				exeQueryStr += " ) ";

				queryList.Add(exeQueryStr);

				//esm_group_permission 권한
				foreach (EsmGroupPermisson temp in model.Items)
				{
					exeQueryStr = " INSERT INTO esm_group_permission ( GROUP_ID , MENU_ID, PER_SELECT, PER_INSERT, PER_UPDATE, PER_DELETE )VALUES(  ";
					exeQueryStr += " ( SELECT MAX(GROUP_ID) FROM esm_group ) ";// + model.Item.GROUP_ID;
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
				//esm_group 그룹 
				exeQueryStr = " UPDATE esm_group SET ";
				exeQueryStr += "GROUP_NAME = '" + model.Item.GROUP_NAME + "'";
				//exeQueryStr += " WHERE GROUP_ID = " + model.Item.GROUP_ID;
				exeQueryStr += " WHERE GROUP_ID = '" + model.Item.GROUP_ID+ "'";

				queryList.Add(exeQueryStr);
				
				//esm_group_permission 권한
				foreach (EsmGroupPermisson temp in model.Items)
				{
					exeQueryStr = " UPDATE esm_group_permission SET  ";
					exeQueryStr += "  PER_SELECT = " + temp.PER_SELECT;
					exeQueryStr += " , PER_INSERT = " + temp.PER_INSERT;
					exeQueryStr += " , PER_UPDATE = " + temp.PER_UPDATE;
					exeQueryStr += " , PER_DELETE = " + temp.PER_DELETE;
					exeQueryStr += " WHERE GROUP_ID = " + model.Item.GROUP_ID + " AND MENU_ID = '" + temp.MENU_ID + "'";

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


		public string DelEsmGrade(int SEQNO)
		{
			string errorStr = "";
			string result = "";
			List<string> queryList = new List<string>();

			string exeQueryStr = " DELETE FROM esm_group WHERE GROUP_ID = " + SEQNO;
			queryList.Add(exeQueryStr);

			exeQueryStr = " DELETE FROM esm_group_permission WHERE GROUP_ID = " + SEQNO;
			queryList.Add(exeQueryStr);
			
			result = "실패.";
			if (exeQuery(queryList, out errorStr))
				result = "성공.";
		
			/*
			if (exeQuery(queryList, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}
			*/

			return result;
		}


		//ESM 관리자 설정 ESM 계정 관리
		

		public EsmAccountModels GetEsmAccountList(EsmAccountModels model)
		{
			string errorStr = "";

			string listQuery = "  SELECT SEQNO, EMAIL,PASSWD,USERNAME,TELNO,eu.GROUP_ID,eg.GROUP_NAME,CREATETIME,DEPARTMENT,POSITION,MEMO,STATUS FROM esm_user eu left outer join esm_group eg on eu.GROUP_ID = eg.GROUP_ID ORDER BY SEQNO  ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EsmUser temp = new EsmUser();
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
					

					model.Items.Add(temp);
				}
			}

			return model;
		}


		public EsmUser GetEsmAccountView(EsmAccountModels getModel)
		{
			string errorStr = "";

			EsmUser model = new EsmUser();
			
			//string listQuery = " SELECT SEQNO, EMAIL, PASSWD, USERNAME, TELNO, GROUP_ID, CREATETIME, DEPARTMENT, POSITION, MEMO, STATUS  FROM esm_user WHERE SEQNO = " + getModel.act_key;
			string listQuery = " SELECT SEQNO, EMAIL, PASSWD, USERNAME, TELNO, eu.GROUP_ID,eg.GROUP_NAME,CREATETIME,DEPARTMENT,POSITION,MEMO,STATUS FROM esm_user eu left outer join esm_group eg on eu.GROUP_ID = eg.GROUP_ID WHERE eu.SEQNO = " + getModel.act_key; 

			

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{ 
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.EMAIL = listDt.Rows[0]["EMAIL"].ToString().Trim();
				model.PASSWD = listDt.Rows[0]["PASSWD"].ToString().Trim();
				model.USERNAME = listDt.Rows[0]["USERNAME"].ToString().Trim();
				model.TELNO = listDt.Rows[0]["TELNO"].ToString().Trim();
				model.GROUP_ID = int.Parse(listDt.Rows[0]["GROUP_ID"].ToString().Trim());
				model.GROUP_NAME = listDt.Rows[0]["GROUP_NAME"].ToString().Trim();
				model.CREATETIME = listDt.Rows[0]["CREATETIME"].ToString().Trim();
				model.DEPARTMENT = listDt.Rows[0]["DEPARTMENT"].ToString().Trim();
				model.POSITION = listDt.Rows[0]["POSITION"].ToString().Trim();
				model.MEMO = listDt.Rows[0]["MEMO"].ToString().Trim();
				model.STATUS = int.Parse(listDt.Rows[0]["STATUS"].ToString().Trim());
				
			}
			
			
			return model;
		}


		public string SetEsmAccount(EsmAccountModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";

			if (model.act_type != null && model.act_type == "ins")
			{
				exeQueryStr = " INSERT INTO esm_user ( EMAIL, PASSWD, USERNAME, TELNO, GROUP_ID, DEPARTMENT, POSITION, MEMO, STATUS )VALUES(  ";
				exeQueryStr += " '" + model.Item.EMAIL + "'";
				exeQueryStr += " , '" + model.Item.PASSWD + "'";
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
				exeQueryStr = " UPDATE esm_user SET ";
				exeQueryStr += " EMAIL = '" + model.Item.EMAIL + "'";
				//exeQueryStr += " ,PASSWD = '" + model.Item.PASSWD + "'";
				exeQueryStr += " ,USERNAME = '" + model.Item.USERNAME + "'";
				exeQueryStr += " ,TELNO = '" + model.Item.TELNO + "'";
				exeQueryStr += " ,GROUP_ID = " + model.Item.GROUP_ID ;
				exeQueryStr += " ,DEPARTMENT = '" + model.Item.DEPARTMENT + "'";
				exeQueryStr += " ,POSITION = '" + model.Item.POSITION + "'";
				exeQueryStr += " ,MEMO = '" + model.Item.MEMO + "'";
				exeQueryStr += " ,STATUS = " + model.Item.STATUS ;
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


		public string DelEsmAccount(int SEQNO)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM esm_user WHERE SEQNO = " + SEQNO;
			
			result = "실패.";
			if (exeQuery(exeQueryStr, out errorStr))
				result = "성공.";

			return result;
		}


		//그룹ID(MSH)
		public List<schTypeArray> GroupIdSelectBox(string GroupId)
		{
			string errorStr = "";

			string listQuery = " SELECT GROUP_ID,GROUP_NAME FROM esm_group WHERE GROUP_ID= " + GroupId;//

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


		//ESM 관리자 설정 로그인 이력 조회
		public EsmLoginHisModels GetEsmLoginHisList(EsmLoginHisModels model) 
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , EST_CODE, ESE_CODE, EMAIL, LOGDATETIME, IPADDR, TYPE  ";
			string cntQuery = " SELECT count(*) as cnt ";

			string baseQuery = " FROM comm_login_log WHERE 1=1 ";

			if (!String.IsNullOrEmpty(model.schType))  //관리자 ID
				baseQuery += " AND  EMAIL = '" + model.schType.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  LOGDATETIME >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  LOGDATETIME <= '" + model.schEdt.Trim() + " 23:59:59'";

			string endQuery = " ORDER BY " + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리

			int totCnt = getQueryCnt(cntQuery, out errorStr);   //전체 리스트 갯수 구하기

			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;  //총 페이징 갯수 구하기
			
			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					CommLoginLog temp = new CommLoginLog();
					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.ESE_CODE = listDt.Rows[i]["ESE_CODE"].ToString().Trim();
					temp.EMAIL = listDt.Rows[i]["EMAIL"].ToString().Trim();
					temp.LOGDATETIME = listDt.Rows[i]["LOGDATETIME"].ToString().Trim();
					temp.IPADDR = listDt.Rows[i]["IPADDR"].ToString().Trim();
					temp.TYPE = listDt.Rows[i]["TYPE"].ToString().Trim();
					model.Items.Add(temp);
				}
			}
			
			return model;
		}




		

























	}
}