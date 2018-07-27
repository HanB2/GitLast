using comm_dbconn;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_esm.Models;
using web_esm.Models_Act.Comm;
using web_esm.Models_Act.Est;

namespace web_esm.Models_Db
{
	public class EstDbModels : DatabaseConnection
	{
		//EstInfo
		string[] selectColumn_EstInfo = {
			"EST_CODE" ,
			"EST_NAME" ,
			"ZIPCODE" ,
			"ADDR" ,
			"ADDR_EN" ,
			"TELNO" ,
			"CREATETIME" ,
			"MEMO" ,
			"NATION_CODE" ,
			"START_AIRPORT" ,
			"WEIGHT_UNIT" ,
			"UTC_HOUR" ,
			"UTC_MINUTE" ,
			"UTC_SUMMER_TIME" ,
			"USERINPUTCODE" ,
			"STATUS"
		};

		public EstInfoModels GetEstInfoList(EstInfoModels model)
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_EstInfo);
			string cntQuery = " SELECT count(*) as cnt ";

			string baseQuery = " FROM esm_station WHERE 1=1  ";

			if (!String.IsNullOrEmpty(model.schType))  //국가
				baseQuery += " AND  NATION_CODE = '" + model.schType.Trim() + "'";

			if (!String.IsNullOrEmpty(model.station))  //STATION
				baseQuery += " AND  EST_CODE = '" + model.station.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schSdt))      //생성날짜 (시작일)
				baseQuery += " AND  CREATETIME >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //생성날짜 (종료일)
				baseQuery += " AND  CREATETIME <= '" + model.schEdt.Trim() + " 23:59:59'";


			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt))  //검색조건 검색어
				baseQuery += " AND " + model.schTypeTxt.Trim() + " like '%" + model.schTxt.Trim() + "%' ";

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
					EsmStation temp = new EsmStation();

					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.EST_NAME = listDt.Rows[i]["EST_NAME"].ToString().Trim();
					temp.ZIPCODE = listDt.Rows[i]["ZIPCODE"].ToString().Trim();
					temp.ADDR = listDt.Rows[i]["ADDR"].ToString().Trim();
					temp.ADDR_EN = listDt.Rows[i]["ADDR_EN"].ToString().Trim();
					temp.TELNO = listDt.Rows[i]["TELNO"].ToString().Trim();
					temp.CREATETIME = listDt.Rows[i]["CREATETIME"].ToString().Trim();
					temp.MEMO = listDt.Rows[i]["MEMO"].ToString().Trim();
					temp.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();
					temp.START_AIRPORT = listDt.Rows[i]["START_AIRPORT"].ToString().Trim();
					temp.WEIGHT_UNIT = listDt.Rows[i]["WEIGHT_UNIT"].ToString().Trim();
					temp.UTC_HOUR = int.Parse(listDt.Rows[i]["UTC_HOUR"].ToString().Trim());
					temp.UTC_MINUTE = int.Parse(listDt.Rows[i]["UTC_MINUTE"].ToString().Trim());
					temp.UTC_SUMMER_TIME = int.Parse(listDt.Rows[i]["UTC_SUMMER_TIME"].ToString().Trim());
					temp.USERINPUTCODE = listDt.Rows[i]["USERINPUTCODE"].ToString().Trim();
					temp.STATUS = int.Parse(listDt.Rows[i]["STATUS"].ToString().Trim());

					model.Items.Add(temp);
				}
			}

			return model;
		}


		//EST_CODE 콤보박스 
		public List<schTypeArray> GetEstCodeSelectBox(string estCode)
		{
			List<schTypeArray> model = new List<schTypeArray>();
			string errorStr = "";	
			string listQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_EstInfo) + " FROM esm_station WHERE 1=1  ";

			//listQuery += " AND NATION_CODE = '" + NATION_CODE + "' ";
			listQuery += " AND EST_CODE = '" + estCode + "' ";


			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["EST_NAME"].ToString().Trim(), opt_key = listDt.Rows[i]["EST_CODE"].ToString().Trim() });
				}
			}
			return model;
		}


		//NATION_CODE 콤보박스 
		public List<schTypeArray> GetNationCodeSelectBox()
		{
			string lan = "";

			string getCol = "";
			switch (lan)    //lan <== 다국어 지원 관련 언어선택 부분이므로 일단 신경쓰지 말고 진행 할것  그냥 돌리면 알아서 NATIONNAME_ko_KR 을 설정함
			{
				case "CN":
					getCol = "NATIONNAME_zh_CN";
					break;
				case "EN":
					getCol = "NATIONNAME";
					break;
				default:
					getCol = "NATIONNAME_ko_KR";
					break;
			}


			string errorStr = "";
			string listQuery = " SELECT csc.NATION_CODE, cn." + getCol + " FROM conf_shipping_country csc left outer join comm_nation cn on csc.NATION_CODE = cn.NATIONNO ";
			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i][getCol].ToString().Trim(), opt_key = listDt.Rows[i]["NATION_CODE"].ToString().Trim() });
				}
			}

			return model;
		}


		//RELEASE_CODE 콤보박스
		public List<schTypeArray> GetReleaseCodeSelectBox(EstIframeCostModels getModel)
		{

			string errorStr = "";
			string listQuery = " SELECT distinct RELEASE_CODE FROM est_shipping_fee WHERE NATION_CODE = '" + getModel.nation + "' AND EST_CODE = '" + getModel.station + "' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["RELEASE_CODE"].ToString().Trim(), opt_key = listDt.Rows[i]["RELEASE_CODE"].ToString().Trim() });
				}
			}

			return model;
		}


		//EST STATION EST 정보 관리 -> EST 기본 정보

		public EsmStation GetEstIframeInfo(EstIframeInfoModels getModel, string estCode)
		{
			string errorStr = "";

			EsmStation model = new EsmStation();

			string listQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_EstInfo) + " FROM esm_station WHERE EST_CODE = '" + estCode + "' ";
			//string listQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_EstInfo) + " FROM esm_station  ORDER BY SEQNO ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
				model.EST_NAME = listDt.Rows[0]["EST_NAME"].ToString().Trim();
				model.ZIPCODE = listDt.Rows[0]["ZIPCODE"].ToString().Trim();
				model.ADDR = listDt.Rows[0]["ADDR"].ToString().Trim();
				model.ADDR_EN = listDt.Rows[0]["ADDR_EN"].ToString().Trim();
				model.TELNO = listDt.Rows[0]["TELNO"].ToString().Trim();
				model.CREATETIME = listDt.Rows[0]["CREATETIME"].ToString().Trim();
				model.MEMO = listDt.Rows[0]["MEMO"].ToString().Trim();
				model.NATION_CODE = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
				model.START_AIRPORT = listDt.Rows[0]["START_AIRPORT"].ToString().Trim();
				model.WEIGHT_UNIT = listDt.Rows[0]["WEIGHT_UNIT"].ToString().Trim();
				model.UTC_HOUR = int.Parse(listDt.Rows[0]["UTC_HOUR"].ToString().Trim());
				model.UTC_MINUTE = int.Parse(listDt.Rows[0]["UTC_MINUTE"].ToString().Trim());
				model.UTC_SUMMER_TIME = int.Parse(listDt.Rows[0]["UTC_SUMMER_TIME"].ToString().Trim());
				model.USERINPUTCODE = listDt.Rows[0]["USERINPUTCODE"].ToString().Trim();
				model.STATUS = int.Parse(listDt.Rows[0]["STATUS"].ToString().Trim());
			}

			return model;
		}

		public string SetEstIframeInfo(EstIframeInfoModels model)
		{
			string result = "";
			string errorStr = "";
			//(msh)추가 2018-06-26 오후16:39
			string exeQueryStr = " UPDATE esm_station SET  ";
			exeQueryStr += " EST_NAME = '" + model.Item.EST_NAME + "' ";
			exeQueryStr += ", ZIPCODE = '" + model.Item.ZIPCODE + "' ";
			exeQueryStr += ", ADDR = '" + model.Item.ADDR + "' ";
			exeQueryStr += ", ADDR_EN = '" + model.Item.ADDR_EN + "' ";
			exeQueryStr += ", TELNO = '" + model.Item.TELNO + "' ";
			exeQueryStr += ", CREATETIME =  CURRENT_TIMESTAMP ";
			exeQueryStr += ", MEMO = '" + model.Item.MEMO + "' ";
			exeQueryStr += ", NATION_CODE = '" + model.Item.NATION_CODE + "' ";
			exeQueryStr += ", START_AIRPORT = '" + model.Item.START_AIRPORT + "' ";
			exeQueryStr += ", WEIGHT_UNIT = '" + model.Item.WEIGHT_UNIT + "' ";
			exeQueryStr += ", UTC_HOUR = " + model.Item.UTC_HOUR + " ";
			exeQueryStr += ", UTC_MINUTE = " + model.Item.UTC_MINUTE + " ";
			exeQueryStr += ", UTC_SUMMER_TIME = " + model.Item.UTC_SUMMER_TIME + " ";
			exeQueryStr += ", STATUS = " + model.Item.STATUS + " ";

			exeQueryStr += " WHERE EST_CODE = '" + model.viewEstCode + "' ";

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



		//EST STATION EST 정보 관리 -> EST 계좌 정보

		string[] EstActOPT_KEY = {
			"setting_SwiftCode",
			"setting_BankAddr",
			"setting_AccountNum",
			"setting_ReceiverName_en",
			"setting_Memo"

		};


		public EstAccountModels GetEstAccountModel(string estCode)
		{
			string errorStr = "";

			EstAccountModels result = new EstAccountModels();

			//string sqlQueryStr = "SELECT SET_KEY, SET_VALUE, EST_CODE FROM est_settings WHERE SET_KEY in ('" + string.Join("','", EstActOPT_KEY) + "')";

			string sqlQueryStr = "SELECT SET_KEY, SET_VALUE, EST_CODE FROM est_settings WHERE EST_CODE = '" + estCode + "' AND SET_KEY in ('" + string.Join("','", EstActOPT_KEY) + "')";


			DataTable dt = getQueryResult(sqlQueryStr, out errorStr);

			if (dt == null || dt.Rows.Count == 0)
			{
				List<string> queryList = new List<string>();
				for (int i = 0; i < EstActOPT_KEY.Length; i++)
				{
					queryList.Add("INSERT INTO  est_settings (SET_KEY, SET_VALUE, EST_CODE) VALUES ('" + EstActOPT_KEY[i] + "', '', '" + estCode + "') ");
				}
				exeQuery(queryList, out errorStr);
			}
			else
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_SwiftCode") { result.setting_SwiftCode = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_BankAddr") { result.setting_BankAddr = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_AccountNum") { result.setting_AccountNum = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_ReceiverName_en") { result.setting_ReceiverName_en = dt.Rows[i]["SET_VALUE"].ToString(); }
					if (dt.Rows[i]["SET_KEY"].ToString() == "setting_Memo") { result.setting_Memo = dt.Rows[i]["SET_VALUE"].ToString(); }

				}
			}

			return result;
		}


		public bool SetEstAccountModel(EstAccountModels model)
		{
			string errorStr = "";
			bool result = false;

			List<string> queryList = new List<string>();
			queryList.Add("UPDATE est_settings SET SET_VALUE ='" + model.setting_SwiftCode + "' WHERE SET_KEY ='setting_SwiftCode' AND EST_CODE= '" + model.viewEstCode + "' ");
			queryList.Add("UPDATE est_settings SET SET_VALUE ='" + model.setting_BankAddr + "' WHERE SET_KEY ='setting_BankAddr'AND EST_CODE= '" + model.viewEstCode + "' ");
			queryList.Add("UPDATE est_settings SET SET_VALUE ='" + model.setting_AccountNum + "' WHERE SET_KEY ='setting_AccountNum' AND EST_CODE= '" + model.viewEstCode + "' ");
			queryList.Add("UPDATE est_settings SET SET_VALUE ='" + model.setting_ReceiverName_en + "' WHERE SET_KEY ='setting_ReceiverName_en'AND EST_CODE= '" + model.viewEstCode + "' ");
			queryList.Add("UPDATE est_settings SET SET_VALUE ='" + model.setting_Memo + "' WHERE SET_KEY ='setting_Memo'AND EST_CODE= '" + model.viewEstCode + "' ");

			result = exeQuery(queryList, out errorStr);

			return result;
		}


		//EST STATION EST 정보 관리 -> EST 통관 비용




		//EST STAION EST 정보 관리 -> 국가별 요율표
		string[] selectColumn_EstIframeCost = {
				"EST_CODE" ,
				"ESE_CODE" ,
				"NATION_CODE" ,
				"RELEASE_CODE" ,
				"WEIGHT" ,
				"SHIPPING_FEE_NOR" ,
				"SHIPPING_FEE_STC"
			};
		/*
		public EstShippingFee GetEstIframeCostList(EstIframeCostModels getModel, string estCode)
		{

			string errorStr = "";

			EstShippingFee model = new EstShippingFee();
			string listQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_EstIframeCost) + " FROM est_shipping_fee WHERE EST_CODE = '" + estCode + "' ";
			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{ 
				model.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
				model.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
				model.ESE_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
			    model.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();
				model.RELEASE_CODE = listDt.Rows[i]["RELEASE_CODE"].ToString().Trim();
				model.WEIGHT = double.Parse(listDt.Rows[i]["WEIGHT"].ToString().Trim());
				model.SHIPPING_FEE_NOR = double.Parse(listDt.Rows[i]["SHIPPING_FEE_NOR"].ToString().Trim());
				model.SHIPPING_FEE_STC = double.Parse(listDt.Rows[i]["SHIPPING_FEE_STC"].ToString().Trim());

				//model.Item.NATION_CODE = temp.NATION_CODE;
				}
			   
			}

			return model;
		}
		*/

		public EstIframeCostModels GetEstIframeCostList(EstIframeCostModels model, string estCode)
		{

			estCode = model.station;
			model.Item.NATION_CODE = model.nation;
			model.Item.RELEASE_CODE = model.type;
			if (model.type == null || model.station == null || model.nation == null)
			{
				return model;
			}

			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_EstIframeCost) + " FROM est_shipping_fee WHERE EST_CODE = '" + estCode + "' ";
			listQuery += " AND NATION_CODE = '" + model.Item.NATION_CODE + "' ";
			listQuery += " AND RELEASE_CODE = '" + model.Item.RELEASE_CODE + "' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{

					EstShippingFee temp = new EstShippingFee();

					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.ESE_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();
					temp.RELEASE_CODE = listDt.Rows[i]["RELEASE_CODE"].ToString().Trim();
					temp.WEIGHT = double.Parse(listDt.Rows[i]["WEIGHT"].ToString().Trim());
					temp.SHIPPING_FEE_NOR = double.Parse(listDt.Rows[i]["SHIPPING_FEE_NOR"].ToString().Trim());
					temp.SHIPPING_FEE_STC = double.Parse(listDt.Rows[i]["SHIPPING_FEE_STC"].ToString().Trim());

					model.Items.Add(temp);
				}

			}

			return model;
		}



		//EST STATION EST 정보 관리 -> 계정 등급 관리
		public EstGradeModels GetEstGradeList(EstGradeModels model, string estCode)
		{
			string errorStr = "";
			string listQuery = " SELECT GROUP_ID , GROUP_NAME FROM est_group WHERE EST_CODE= '"+estCode+"' order by GROUP_ID "; 

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EstGroup temp = new EstGroup();

					temp.GROUP_ID = int.Parse(listDt.Rows[i]["GROUP_ID"].ToString().Trim());
					temp.GROUP_NAME = listDt.Rows[i]["GROUP_NAME"].ToString().Trim();
					model.Items.Add(temp);
				}
			}

			return model;
		}

		public EstGradeViewModel GetEstGradeView(string groupId, string estCode)
		{

			EstGradeViewModel model = new EstGradeViewModel();

			string errorStr = "";

			//그룹 가져오기
			string listQuery = " SELECT GROUP_ID , GROUP_NAME FROM est_group WHERE GROUP_ID= '" + groupId + "' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.Item.GROUP_ID = int.Parse(listDt.Rows[0]["GROUP_ID"].ToString().Trim());
				model.Item.GROUP_NAME = listDt.Rows[0]["GROUP_NAME"].ToString().Trim();
			}

			//권한 가져오기
			listQuery = " SELECT GROUP_ID , EST_CODE, MENU_ID, PER_SELECT, PER_INSERT, PER_UPDATE, PER_DELETE FROM est_group_permission WHERE GROUP_ID = '" + groupId + "' AND EST_CODE = '" + estCode + "'";

			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EstGroupPermisson temp = new EstGroupPermisson();
					temp.GROUP_ID = int.Parse(listDt.Rows[i]["GROUP_ID"].ToString().Trim());
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
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
					EstGroupPermisson temp = new EstGroupPermisson();

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

		public string SetEstGrade(EstGradeViewModel model,string estCode)
		{
			

			string errorStr = "";
			string result = "";
			string exeQueryStr = "";
			List<string> queryList = new List<string>();
			

			if (model.act_type != null && model.act_type == "ins")
			{
				model.Item.EST_CODE = model.viewEstCode;

				//ese_group 그룹 
				exeQueryStr = " INSERT INTO est_group ( GROUP_ID ,EST_CODE, GROUP_NAME )VALUES(  ";
				exeQueryStr += " " + model.Item.GROUP_ID + "";
				exeQueryStr += ", '" + model.Item.EST_CODE + "' ";
				exeQueryStr += ", '" + model.Item.GROUP_NAME + "' ";
				exeQueryStr += " ) ";

				queryList.Add(exeQueryStr);

				//est_group_permission 권한
				foreach (EstGroupPermisson temp in model.Items)
				{
					temp.EST_CODE = model.viewEstCode;

					exeQueryStr = " INSERT INTO est_group_permission ( GROUP_ID , EST_CODE, MENU_ID, PER_SELECT, PER_INSERT, PER_UPDATE, PER_DELETE )VALUES(  ";
					exeQueryStr += " ( SELECT MAX(GROUP_ID) FROM est_group ) ";// + model.Item.GROUP_ID;
					exeQueryStr += " , '" + temp.EST_CODE + "'";
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
				exeQueryStr = " UPDATE est_group SET ";
				exeQueryStr += "GROUP_NAME = '" + model.Item.GROUP_NAME + "'";
				exeQueryStr += " WHERE GROUP_ID = " + model.Item.GROUP_ID;

				queryList.Add(exeQueryStr);

				//ese_group_permission 권한
				foreach (EstGroupPermisson temp in model.Items)
				{
					exeQueryStr = " UPDATE est_group_permission SET  ";
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

		public string DelEstGrade(int SEQNO)
		{
			string errorStr = "";
			string result = "";
			List<string> queryList = new List<string>();

			string exeQueryStr = " DELETE FROM est_group WHERE GROUP_ID = " + SEQNO;
			queryList.Add(exeQueryStr);

			exeQueryStr = " DELETE FROM est_group_permission WHERE GROUP_ID = " + SEQNO;
			queryList.Add(exeQueryStr);

			result = "실패.";
			if (exeQuery(queryList, out errorStr))
				result = "성공.";

			return result;
		}



		//EST STATION EST 정보 관리 -> 계정 관리
		public EstUserModels GetEstUserList(EstUserModels model, string estCode)
		{
			string errorStr = "";

			//string listQuery = " SELECT SEQNO, EST_CODE, EMAIL, PASSWD, USERNAME, TELNO, GROUP_ID, CREATETIME, DEPARTMENT, POSITION, MEMO, STATUS FROM est_user order by SEQNO ";
			string listQuery = "  SELECT SEQNO, EMAIL,PASSWD,USERNAME,TELNO,eu.GROUP_ID,eg.GROUP_NAME,CREATETIME,DEPARTMENT,POSITION,MEMO,STATUS FROM est_user eu left outer join est_group eg on eu.GROUP_ID = eg.GROUP_ID ORDER BY SEQNO  ";
			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EstUser temp = new EstUser();
					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					//temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
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

		public EstUser GetEstUserView(EstUserModels getModel)
		{
			string errorStr = "";

			EstUser model = new EstUser();

			string listQuery = " SELECT SEQNO, EST_CODE, EMAIL, PASSWD, USERNAME, TELNO, GROUP_ID, CREATETIME, DEPARTMENT, POSITION, MEMO, STATUS  FROM est_user WHERE SEQNO = " + getModel.act_key;

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
				model.EMAIL = listDt.Rows[0]["EMAIL"].ToString().Trim();
				model.PASSWD = listDt.Rows[0]["PASSWD"].ToString().Trim();
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

		public string SetEstUser(EstUserModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";

			if (model.act_type != null && model.act_type == "ins")
			{
				exeQueryStr = " INSERT INTO est_user ( EMAIL, EST_CODE, PASSWD, USERNAME, TELNO, GROUP_ID, DEPARTMENT, POSITION, MEMO, STATUS )VALUES(  ";
				exeQueryStr += " '" + model.Item.EMAIL + "'";
				exeQueryStr += " , '" + model.Item.EST_CODE + "'";
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
				exeQueryStr = " UPDATE est_user SET ";
				exeQueryStr += " EMAIL = '" + model.Item.EMAIL + "'";
				exeQueryStr += " ,EST_CODE = '" + model.Item.EST_CODE + "'";
				//exeQueryStr += " ,PASSWD = '" + model.Item.PASSWD + "'";
				exeQueryStr += " ,USERNAME = '" + model.Item.USERNAME + "'";
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

		public string DelEstUser(int SEQNO)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM est_user WHERE SEQNO = " + SEQNO;

			result = "실패.";
			if (exeQuery(exeQueryStr, out errorStr))
				result = "성공.";

			return result;
		}


	


		


		



		



	

	}
}