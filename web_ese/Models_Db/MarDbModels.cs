using comm_dbconn;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Mar;
using static web_ese.Models_Act.Mar.MarInOutModels;

namespace web_ese.Models_Db
{
	public class MarDbModels : DatabaseConnection
	{

		// MAR 충전(이체)===========================================================
		public MarReqModels GetMarReqBase(MarReqModels model)
		{
			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();
			string estCode = context.Session["EST_CODE"].ToString();
			
			string errorStr = "";

			string listQuery = " SELECT BANK_NAME ,BANK_ACCOUNT ,DEPOSIT_NAME ,DEPOSIT_CURRENCY, DEPOSIT_DATETIME FROM mar_charge_req WHERE ESE_CODE = '" + eseCode + "' AND EST_CODE = '"+ estCode + "'  ORDER BY SEQNO DESC LIMIT 0, 1";
			
			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.Item.BANK_NAME = listDt.Rows[0]["BANK_NAME"].ToString().Trim();
				model.Item.BANK_ACCOUNT = listDt.Rows[0]["BANK_ACCOUNT"].ToString().Trim();
				model.Item.DEPOSIT_NAME = listDt.Rows[0]["DEPOSIT_NAME"].ToString().Trim();
				model.Item.DEPOSIT_CURRENCY = listDt.Rows[0]["DEPOSIT_CURRENCY"].ToString().Trim();
				model.Item.DEPOSIT_DATETIME = listDt.Rows[0]["DEPOSIT_DATETIME"].ToString().Trim();
			}

			return model;
		}


		public string SetMarReq(MarReqModels model)
		{
			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();
			string estCode = context.Session["EST_CODE"].ToString();

			string resultStr = "";

			string exeQueryStr = "";
			string errorStr = "";
			
			exeQueryStr = " INSERT INTO mar_charge_req ( EST_CODE, ESE_CODE, BANK_NAME , BANK_ACCOUNT, DEPOSIT_NAME, DEPOSIT_AMOUNT , DEPOSIT_CURRENCY, DEPOSIT_DATETIME, MEMO ,STATUS )VALUES(  ";
			exeQueryStr += "'" + estCode + "'";
			exeQueryStr += ", '" + eseCode + "'";
			exeQueryStr += ",'" + model.Item.BANK_NAME + "' ";
			exeQueryStr += ", '" + model.Item.BANK_ACCOUNT + "' ";
			exeQueryStr += ", '" + model.Item.DEPOSIT_NAME + "' ";
			exeQueryStr += ",  " + model.Item.DEPOSIT_AMOUNT + " ";
			exeQueryStr += ",  '" + model.Item.DEPOSIT_CURRENCY + "' ";		
			exeQueryStr += " , DATE_FORMAT(NOW(), '%Y%m%e') ";
			exeQueryStr += ", '" + model.Item.MEMO + "' ";
			exeQueryStr += ",  10 ";
			exeQueryStr += " ) ";

			if (exeQuery(exeQueryStr, out errorStr))
			{
				resultStr = "성공.";
			}
			else
			{
				resultStr = "실패.";
			}

			return resultStr;
		}



		//MAR 충전/ 사용 이력========================================================


		public MarInOutModels GetMarInOut1(MarInOutModels model)
		{

			HttpContext context = HttpContext.Current;
			string ESE_CODE = context.Session["ESE_CODE"].ToString();

			string errorStr = "";
			
			string listQuery = " SELECT mcr.STATUS, IFNULL(mcr.DEPOSIT_AMOUNT,0) as DEPOSIT_AMOUNT, mcr.CREATETIME, mi.DATETIME_UPD, IFNULL(mi.MAR, 0) as MAR ";
			string cntQuery = " SELECT count(*) as cnt ";

			string baseQuery = " FROM mar_charge_req mcr left outer join mar_inout mi on mcr.SEQNO = mi.FK_SEQNO AND mi.INOUT_TYPE = 1 ";
			baseQuery +=" WHERE mcr.ESE_CODE = '"+ ESE_CODE+"' ";
			
			string endQuery = " ORDER BY mcr.SEQNO DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리
			
			int totCnt = getQueryCnt(cntQuery, out errorStr);   //전체 리스트 갯수 구하기

			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;  //총 페이징 갯수 구하기

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					MAR_REQ_LIST temp = new MAR_REQ_LIST();

					temp.STATUS = int.Parse(listDt.Rows[i]["STATUS"].ToString().Trim());
					temp.DEPOSIT_AMOUNT = double.Parse(listDt.Rows[i]["DEPOSIT_AMOUNT"].ToString().Trim());
					temp.MAR = double.Parse(listDt.Rows[i]["MAR"].ToString().Trim());
					temp.CREATETIME = listDt.Rows[i]["CREATETIME"].ToString().Trim();
					temp.DATETIME_UPD = listDt.Rows[i]["DATETIME_UPD"].ToString().Trim();

					switch (temp.STATUS)
					{
						case 0:		temp.STATUS_TEXT = "취소";		break;
						case 10:	temp.STATUS_TEXT = "출전신청";	break;
						case 20:	temp.STATUS_TEXT = "입금확인불가"; break;
						case 30:	temp.STATUS_TEXT = "충전완료";	break;
					}

					model.reqList.Add(temp);
				}
			}

			return model;
		}


		public MarInOutModels GetMarInOut3(MarInOutModels model)
		{
			HttpContext context = HttpContext.Current;
			string ESE_CODE = context.Session["ESE_CODE"].ToString();

			string errorStr = "";

			string listQuery = " SELECT mcr.STATUS, IFNULL(mcr.REQ_AMOUNT, 0) as REQ_AMOUNT , mcr.CREATETIME, mi.DATETIME_UPD, IFNULL(mi.AMOUNT, 0) as AMOUNT ";
			string cntQuery = " SELECT count(*) as cnt ";

			string baseQuery = " FROM mar_withdraw_req mcr left outer join mar_inout mi on mcr.SEQNO = mi.FK_SEQNO AND mi.INOUT_TYPE = 2 ";
			baseQuery += " WHERE mcr.ESE_CODE = '" + ESE_CODE + "' ";

			string endQuery = " ORDER BY mcr.SEQNO DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리

			int totCnt = getQueryCnt(cntQuery, out errorStr);   //전체 리스트 갯수 구하기

			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;  //총 페이징 갯수 구하기

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					MAR_OUT_REQ_LIST temp = new MAR_OUT_REQ_LIST();

					temp.STATUS = int.Parse(listDt.Rows[i]["STATUS"].ToString().Trim());
					temp.REQ_AMOUNT = double.Parse(listDt.Rows[i]["REQ_AMOUNT"].ToString().Trim());
					temp.AMOUNT = double.Parse(listDt.Rows[i]["AMOUNT"].ToString().Trim());
					temp.CREATETIME = listDt.Rows[i]["CREATETIME"].ToString().Trim();
					temp.DATETIME_UPD = listDt.Rows[i]["DATETIME_UPD"].ToString().Trim();

					switch (temp.STATUS)
					{
						case 0:		temp.STATUS_TEXT = "취소"; break;
						case 10:	temp.STATUS_TEXT = "환전신청"; break;
						case 20:	temp.STATUS_TEXT = "환전불가"; break;
						case 30:	temp.STATUS_TEXT = "환전완료"; break;
					}

					model.outReqList.Add(temp);
				}
			}

			return model;
		}

		public MarInOutModels GetMarInOut4(MarInOutModels model)
		{
			HttpContext context = HttpContext.Current;
			string ESE_CODE = context.Session["ESE_CODE"].ToString();

			string errorStr = "";

			string listQuery = " SELECT MAR, DATETIME_UPD, INOUT_TYPE ";
			string cntQuery = " SELECT count(*) as cnt ";

			string baseQuery = " FROM mar_inout ";
			baseQuery += " WHERE ESE_CODE = '" + ESE_CODE + "' ";
			
			string endQuery = " ORDER BY SEQNO DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리

			int totCnt = getQueryCnt(cntQuery, out errorStr);   //전체 리스트 갯수 구하기

			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;  //총 페이징 갯수 구하기

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					MAR_INOUT_LIST temp = new MAR_INOUT_LIST();

					temp.INOUT_TYPE = int.Parse(listDt.Rows[i]["INOUT_TYPE"].ToString().Trim());
					temp.MAR = double.Parse(listDt.Rows[i]["MAR"].ToString().Trim());
					temp.DATETIME_UPD = listDt.Rows[i]["DATETIME_UPD"].ToString().Trim();

					switch (temp.INOUT_TYPE)
					{
						case 1: temp.INOUT_TYPE_TEXT = "MAR 충전 (계좌이체)"; break;
						case 2:
							temp.INOUT_TYPE_TEXT = "MAR 환전";
							temp.MAR = temp.MAR * -1;
							break;
					}

					model.inOutList.Add(temp);
				}
			}

			return model;
		}

		// MAR 환불 신청 ========================================================
		public MarOutReqModels GetMarOutReqBase(MarOutReqModels model)
		{
			
			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();
			string estCode = context.Session["EST_CODE"].ToString();
			
			model.MY_MAR = 0.0;

			string errorStr = "";
			string listQuery = " SELECT SEQNO , MAR FROM est_sender WHERE ESE_CODE = '" + eseCode + "' AND EST_CODE = '" + estCode + "'";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.Item.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				if (!string.IsNullOrEmpty(listDt.Rows[0]["MAR"].ToString().Trim()))
				{ 
					model.Item.Mar = double.Parse(listDt.Rows[0]["MAR"].ToString().Trim());
				}
			}

			model.MY_MAR = model.Item.Mar;
			
			listQuery = " SELECT st.SET_KEY, st.SET_VALUE ";
			listQuery += " FROM est_sender es left outer join ese_settings st on es.ESE_CODE = st.ESE_CODE WHERE  es.ESE_CODE = '" + eseCode + "' ";
			listDt = getQueryResult(listQuery, out errorStr);

			model.CHK_DATA = true;

			if (listDt != null && listDt.Rows.Count != 0)
			{

				EseSettings temp = new EseSettings();
				for (int i = 0; i < listDt.Rows.Count; i++)
				{ 
					temp.SET_KEY = listDt.Rows[i]["SET_KEY"].ToString().Trim();

					if (string.IsNullOrEmpty(listDt.Rows[i]["SET_VALUE"].ToString().Trim()) && temp.SET_KEY != "setting_Memo")
					{	
							model.CHK_DATA = false;
							return model;		
					}
					
				}
			}
			
			return model;
		}


		public string SetMarOutReq(MarOutReqModels model)
		{
			string resultStr = "";

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();
			string estCode = context.Session["EST_CODE"].ToString();

			//환불 계좌정보 가져오기=============================
			string errorStr = "";
			string BaseQuery = "";

			BaseQuery = " SELECT SET_KEY , SET_VALUE ";
			BaseQuery += " FROM ese_settings WHERE ESE_CODE = '" + eseCode + "' AND SET_KEY in ('setting_SwiftCode' , 'setting_BankAddr' , 'setting_AccountNum' , 'setting_ReceiverName_en' )";

			string exeQueryStr = "";

			DataTable listDt = getQueryResult(BaseQuery, out errorStr);
			
			string setting_SwiftCode = "";
			string setting_BankAddr = "";
			string tmp_key = "";
			string tmp_value = "";

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					tmp_key = listDt.Rows[i]["SET_KEY"].ToString().Trim();
					tmp_value = listDt.Rows[i]["SET_VALUE"].ToString().Trim();
					
					switch (tmp_key)
					{
						case "setting_SwiftCode":
							setting_SwiftCode = tmp_value;
							break;
						case "setting_BankAddr":
							setting_BankAddr = tmp_value;
							break;
						case "setting_AccountNum":
							model.Item.BANK_ACCOUNT = tmp_value;
							break;
						case "setting_ReceiverName_en":
							model.Item.HOLDER_NAME = tmp_value;
							break;
					}
			   }
			}
			
			//입력 받은 값 세팅
			model.Item.REQ_AMOUNT = model.REQ_AMOUNT;  //소수점 둘째 자리 까지만 허용
			model.Item.MEMO = model.REQ_MEMO; // 환불 요청 메모
			
			model.Item.BANK_NAME = setting_SwiftCode + "|" + setting_BankAddr ;      

			model.Item.STATUS = 10;
			
			exeQueryStr = " INSERT INTO mar_withdraw_req ( ESE_CODE, EST_CODE, REQ_AMOUNT , MEMO, BANK_NAME, BANK_ACCOUNT , HOLDER_NAME, STATUS )VALUES(  ";
			exeQueryStr += " '" + eseCode + "' ";
			exeQueryStr += ", '" + estCode + "' ";
			exeQueryStr += ", " + model.Item.REQ_AMOUNT + "";
			exeQueryStr += ", '" + model.Item.MEMO + "' ";
			exeQueryStr += ", '" + model.Item.BANK_NAME + "' ";
			exeQueryStr += ", '" + model.Item.BANK_ACCOUNT + "' ";
			exeQueryStr += ", '" + model.Item.HOLDER_NAME + "' ";
			exeQueryStr += ", " + model.Item.STATUS + "";
			exeQueryStr += " ) ";

			if (exeQuery(exeQueryStr, out errorStr))
			{
				resultStr = "성공.";
			}
			else
			{
				resultStr = "실패.";
			}
			
			return resultStr;
		}

		
	}
}
 