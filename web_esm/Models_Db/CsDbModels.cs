using comm_dbconn;
using comm_model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;
using web_esm.Models_Act.Cs;

namespace web_esm.Models_Db
{
	public class CsDbModels : DatabaseConnection
	{

		//CS NOTICE
		string[] selectColoum_CsNotice = {
			"EST_CODE" ,
			"WRITER_ID" ,
			"TITLE_CN" ,
			"TITLE_EN" ,
			"TITLE_KR" ,
			"CONTENTS_CN" ,
			"CONTENTS_EN" ,
			"CONTENTS_KR" ,
			"REGDATE" ,
			"UP_DATE" ,
			"READ_NUM" ,
			"POPUP_DISPLAY" ,
			"POPUP_START" ,
			"POPUP_END" ,
			"WEB_DISPLAY",
			"BD_TYPE"
		};

		string[] insertColoum_CsNotice = {
			//"EST_CODE" ,
			//"ESE_CODE" ,
			"WRITER_ID" ,
			"TITLE_CN" ,
			"TITLE_EN" ,
			"TITLE_KR" ,
			"CONTENTS_CN" ,
			"CONTENTS_EN" ,
			"CONTENTS_KR" ,
			"POPUP_DISPLAY" ,
			"POPUP_START" ,
			"POPUP_END" ,
			"WEB_DISPLAY",
			"BD_TYPE"
		};

		public CsNoticeModels GetCsNoticeList(CsNoticeModels model)
		{
			string errorStr = "";

			string listQuery = " SELECT bn.SEQNO , esmU.EMAIL , " + string.Join(",", selectColoum_CsNotice);
			string cntQuery = " SELECT count(*) as cnt ";

			string baseQuery = " FROM bd_notice bn left outer join esm_user esmU on bn.WRITER_ID = esmU.SEQNO WHERE 1=1 ";
			
			if (!String.IsNullOrEmpty(model.schType))  //공지유형
				baseQuery += " AND  bn.BD_TYPE = " + model.schType.Trim();
			
			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  bn.REGDATE >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  bn.REGDATE <= '" + model.schEdt.Trim() + " 23:59:59'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt) )  //검색조건 검색어
				baseQuery += " AND  bn." + model.schTypeTxt.Trim() + " like '%" + model.schTxt.Trim() + "%' ";

			string endQuery  = " ORDER BY " + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page -1) * model.Paging.pageNum) +" , " + model.Paging.pageNum ;    //정렬

			cntQuery += baseQuery;				//토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리
			
			int totCnt = getQueryCnt(cntQuery, out errorStr);	//전체 리스트 갯수 구하기
			
			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1 ;	//총 페이징 갯수 구하기

			DataTable listDt = getQueryResult(listQuery, out errorStr);
						
			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					BdNotice temp = new BdNotice();
					
					temp.SEQNO			= int.Parse(	listDt.Rows[i]["SEQNO"].ToString().Trim()	);  
					temp.EST_CODE		= listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.WRITER_ID      = int.Parse(listDt.Rows[i]["WRITER_ID"].ToString().Trim());	
					temp.TITLE_CN		= listDt.Rows[i]["TITLE_CN"].ToString().Trim();  
					temp.TITLE_EN		= listDt.Rows[i]["TITLE_EN"].ToString().Trim();  
					temp.TITLE_KR		= listDt.Rows[i]["TITLE_KR"].ToString().Trim();  
					temp.CONTENTS_CN	= listDt.Rows[i]["CONTENTS_CN"].ToString().Trim();  
					temp.CONTENTS_EN	= listDt.Rows[i]["CONTENTS_EN"].ToString().Trim();  
					temp.CONTENTS_KR	= listDt.Rows[i]["CONTENTS_KR"].ToString().Trim();  
					temp.REGDATE		= listDt.Rows[i]["REGDATE"].ToString().Trim();  
					temp.UP_DATE		= listDt.Rows[i]["UP_DATE"].ToString().Trim();  
					temp.READ_NUM		= int.Parse(	listDt.Rows[i]["READ_NUM"].ToString().Trim()	);  
					temp.POPUP_DISPLAY	= int.Parse(	listDt.Rows[i]["POPUP_DISPLAY"].ToString().Trim()	);  
					temp.POPUP_START	= listDt.Rows[i]["POPUP_START"].ToString().Trim();  
					temp.POPUP_END		= listDt.Rows[i]["POPUP_END"].ToString().Trim();  
					temp.WEB_DISPLAY	= int.Parse(	listDt.Rows[i]["WEB_DISPLAY"].ToString().Trim() );
					//(msh)추가
					temp.BD_TYPE = int.Parse(listDt.Rows[i]["BD_TYPE"].ToString().Trim());

					foreach (schTypeArray tempS in model.schTypeArray){
						if (tempS.opt_value == listDt.Rows[i]["BD_TYPE"].ToString().Trim())
						{
							temp.BD_TYPE = int.Parse(listDt.Rows[i]["BD_TYPE"].ToString().Trim());
							temp.BD_TYPE_txt = tempS.opt_key;
						}
					}
					temp.WRITER_NAME = listDt.Rows[i]["EMAIL"].ToString().Trim();

					model.Items.Add(temp);
				}
			}
			
			return model;
		}

		
		public BdNotice GetCsNoticeView(CsNoticeModels getModel)
		{
			string errorStr = "";
			BdNotice model = new BdNotice();

			if (getModel.act_type != null && getModel.act_type == "updt")
			{

				string ViewQuery = " SELECT bn.SEQNO , esmU.EMAIL , " + string.Join(",", selectColoum_CsNotice) + " FROM bd_notice bn left outer join esm_user esmU on bn.WRITER_ID = esmU.SEQNO  WHERE bn.SEQNO = " + getModel.act_key;
				DataTable listDt = getQueryResult(ViewQuery, out errorStr);

				if (listDt != null && listDt.Rows.Count != 0)
				{
					model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
					model.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.WRITER_ID = int.Parse(listDt.Rows[0]["WRITER_ID"].ToString().Trim());
					model.TITLE_CN = listDt.Rows[0]["TITLE_CN"].ToString().Trim();
					model.TITLE_EN = listDt.Rows[0]["TITLE_EN"].ToString().Trim();
					model.TITLE_KR = listDt.Rows[0]["TITLE_KR"].ToString().Trim();
					model.CONTENTS_CN = listDt.Rows[0]["CONTENTS_CN"].ToString().Trim();
					model.CONTENTS_EN = listDt.Rows[0]["CONTENTS_EN"].ToString().Trim();
					model.CONTENTS_KR = listDt.Rows[0]["CONTENTS_KR"].ToString().Trim();
					model.REGDATE = listDt.Rows[0]["REGDATE"].ToString().Trim();
					model.UP_DATE = listDt.Rows[0]["UP_DATE"].ToString().Trim();
					model.READ_NUM = int.Parse(listDt.Rows[0]["READ_NUM"].ToString().Trim());
					model.POPUP_DISPLAY = int.Parse(listDt.Rows[0]["POPUP_DISPLAY"].ToString().Trim());
					model.POPUP_START = listDt.Rows[0]["POPUP_START"].ToString().Trim();
					model.POPUP_END = listDt.Rows[0]["POPUP_END"].ToString().Trim();
					model.WEB_DISPLAY = int.Parse(listDt.Rows[0]["WEB_DISPLAY"].ToString().Trim());
					model.BD_TYPE = int.Parse(listDt.Rows[0]["BD_TYPE"].ToString().Trim());
					model.WRITER_NAME = listDt.Rows[0]["EMAIL"].ToString().Trim();
				}
				
			}
			return model;
		}
		
		
		public string setCsNotice(CsNoticeModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";

			model.Item.BD_TYPE = int.Parse(model.schType);
			if (model.act_type != null && model.act_type == "ins")
			{
				HttpContext context = HttpContext.Current;
				string writerId = context.Session["MANAGE_NO"].ToString();


				exeQueryStr = " INSERT INTO bd_notice(" + string.Join(",", insertColoum_CsNotice) + " )VALUES(  ";
				exeQueryStr += " " + writerId + "";
				exeQueryStr += ", '" + model.Item.TITLE_CN + "' ";
				exeQueryStr += ", '" + model.Item.TITLE_EN + "' ";
				exeQueryStr += ", '" + model.Item.TITLE_KR + "' ";
				exeQueryStr += ", '" + model.Item.CONTENTS_CN + "' ";
				exeQueryStr += ", '" + model.Item.CONTENTS_EN + "' ";
				exeQueryStr += ", '" + model.Item.CONTENTS_KR + "' ";
				exeQueryStr += ", " + model.Item.POPUP_DISPLAY + "";
				exeQueryStr += ", '" + model.Item.POPUP_START + "' ";
				exeQueryStr += ", '" + model.Item.POPUP_END + "' ";
				exeQueryStr += ", " + model.Item.WEB_DISPLAY + "";
				exeQueryStr += ", " + model.Item.BD_TYPE + "";
				exeQueryStr += " ) ";
			}
			else if (model.act_type != null && model.act_type == "updt")
			{

				exeQueryStr = " UPDATE bd_notice SET ";
				exeQueryStr += "TITLE_CN = '"		+ model.Item.TITLE_CN + "'";
				exeQueryStr += ",TITLE_EN = '" + model.Item.TITLE_EN + "'";
				exeQueryStr += ",TITLE_KR = '" + model.Item.TITLE_KR + "'";
				exeQueryStr += ",CONTENTS_CN =  '" + model.Item.CONTENTS_CN + "'";
				exeQueryStr += ",CONTENTS_EN = '" + model.Item.CONTENTS_EN + "'";
				exeQueryStr += ",CONTENTS_KR = '" + model.Item.CONTENTS_KR + "'";
				exeQueryStr += ",UP_DATE =  CURRENT_TIMESTAMP";
				exeQueryStr += ",POPUP_DISPLAY ="	+ model.Item.POPUP_DISPLAY + "";
				exeQueryStr += ",POPUP_START =  '" + model.Item.POPUP_START + "'";
				exeQueryStr += ",POPUP_END =  '" + model.Item.POPUP_END + "'";
				exeQueryStr += ",WEB_DISPLAY ="	+ model.Item.WEB_DISPLAY + "";
				exeQueryStr += ",BD_TYPE =" + model.Item.BD_TYPE + "";
				exeQueryStr += " WHERE SEQNO = " + model.Item.SEQNO ;


			

			}
			else
			{
				result = "잘못된 접근입니다.";
				return result;
			}

			if(exeQuery(exeQueryStr, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}

			return result;

		}


		public string delCsNotice(int seq)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM bd_notice WHERE SEQNO = " + seq;

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

		//CS QNA
		string[] selectColumn_CsQna = {
			"EST_CODE" ,
			"ESE_CODE" ,
			"WRITER_ID" ,
			"REGDATE" ,
			"QNA_TYPE" ,
			"TITLE" ,
			"QUESTION" ,
			"ANSWER" ,
			"ANSWER_ID" ,
			"ANSWER_DATE" 
		};

		public CsQnaModels GetCsQnaList(CsQnaModels model)
		{
			string errorStr = "";

			//string listQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_CsQna);
			string listQuery = " SELECT bq.SEQNO , esmU.EMAIL , " + string.Join(",", selectColumn_CsQna);

			string cntQuery = " SELECT count(*) as cnt ";

			//string baseQuery = " FROM bd_qna WHERE 1=1 ";
			string baseQuery = " FROM bd_qna bq left outer join esm_user esmU on bq.WRITER_ID = esmU.SEQNO WHERE 1=1 ";

			if (!String.IsNullOrEmpty(model.schType))  //문의유형
				baseQuery += " AND  QNA_TYPE = " + model.schType.Trim();

			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  REGDATE >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  REGDATE <= '" + model.schEdt.Trim() + " 23:59:59'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt.Trim() + "%' ";

			string endQuery = " ORDER BY " + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리

			int totCnt = getQueryCnt(cntQuery, out errorStr);   //전체 리스트 갯수 구하기

			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;    //총 페이징 갯수 구하기

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			//model.Items = new List<BdQna>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					BdQna temp = new BdQna();

					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.ESE_CODE = listDt.Rows[i]["ESE_CODE"].ToString().Trim();
					temp.WRITER_ID = int.Parse(listDt.Rows[i]["WRITER_ID"].ToString().Trim());
					temp.REGDATE = listDt.Rows[i]["REGDATE"].ToString().Trim();
					temp.QNA_TYPE = int.Parse(listDt.Rows[i]["QNA_TYPE"].ToString().Trim());
					temp.TITLE = listDt.Rows[i]["TITLE"].ToString().Trim();
					temp.QUESTION = listDt.Rows[i]["QUESTION"].ToString().Trim();		
					temp.ANSWER = listDt.Rows[i]["ANSWER"].ToString().Trim();
					temp.ANSWER_ID = int.Parse(listDt.Rows[i]["ANSWER_ID"].ToString().Trim());
					temp.ANSWER_DATE = listDt.Rows[i]["ANSWER_DATE"].ToString().Trim();
					

					foreach (schTypeArray tempS in model.schTypeArray)
					{
						if (tempS.opt_value == listDt.Rows[i]["QNA_TYPE"].ToString().Trim())
						{
							temp.QNA_TYPE = int.Parse(listDt.Rows[i]["QNA_TYPE"].ToString().Trim());
							temp.QNA_TYPE_txt = tempS.opt_key;
						}
					}
					temp.WRITER_NAME = listDt.Rows[i]["EMAIL"].ToString().Trim();


					model.Items.Add(temp);
				}
			}

			return model;
		}

		public BdQna GetCsQnaView(CsQnaModels getModel)
		{
			string errorStr = "";
			BdQna model = new BdQna();

			if (getModel.act_type != null && getModel.act_type == "updt")
			{

				//string ViewQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_CsQna) + " FROM bd_qna WHERE SEQNO = " + getModel.act_key;
				string ViewQuery = " SELECT bQ.SEQNO , esmU.EMAIL , " + string.Join(",", selectColumn_CsQna) + " FROM bd_qna bn left outer join esm_user esmU on bn.WRITER_ID = esmU.SEQNO  WHERE bn.SEQNO = " + getModel.act_key;

				DataTable listDt = getQueryResult(ViewQuery, out errorStr);

				if (listDt != null && listDt.Rows.Count != 0)
				{
					model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
					model.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.ESE_CODE = listDt.Rows[0]["ESE_CODE"].ToString().Trim();
					model.WRITER_ID = int.Parse(listDt.Rows[0]["WRITER_ID"].ToString().Trim());
					model.REGDATE = listDt.Rows[0]["REGDATE"].ToString().Trim();
					model.QNA_TYPE = int.Parse(listDt.Rows[0]["QNA_TYPE"].ToString().Trim());
					model.TITLE = listDt.Rows[0]["TITLE"].ToString().Trim();
					model.QUESTION = listDt.Rows[0]["QUESTION"].ToString().Trim();
					model.ANSWER = listDt.Rows[0]["ANSWER"].ToString().Trim();
					model.REGDATE = listDt.Rows[0]["REGDATE"].ToString().Trim();
					model.ANSWER_ID = int.Parse(listDt.Rows[0]["ANSWER_ID"].ToString().Trim());
					model.ANSWER_DATE = listDt.Rows[0]["ANSWER_DATE"].ToString().Trim();
					model.QNA_TYPE_txt = listDt.Rows[0]["ANSWER_DATE"].ToString().Trim();
				}
			}

			return model;
		}
		
		public string setCsQnaModels(CsQnaModels model)
		{
			string result = "";
			string errorStr = "";
			//(msh)추가 2018-06-15 오전10:53
			string exeQueryStr = " UPDATE bd_qna SET  ";
			exeQueryStr += " ANSWER  = '" + model.Item.ANSWER + "' ";
			exeQueryStr += ", ANSWER_ID = '" + model.Item.ANSWER_ID + "' ";
			exeQueryStr += ", ANSWER_DATE =  CURRENT_TIMESTAMP ";
			exeQueryStr += " WHERE SEQNO =  " + model.Item.SEQNO;

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

	}
}
 