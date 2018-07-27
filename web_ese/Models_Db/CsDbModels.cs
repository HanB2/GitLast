using comm_dbconn;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Cs;

namespace web_ese.Models_Db
{
	public class CsDbModels : DatabaseConnection
	{

		//CS NOTICE
		string[] selectColoum_CsNotice = {
			"WRITER_ID" ,
			"TITLE_CN" ,
			"TITLE_EN" ,
			"TITLE_KR" ,
			"CONTENTS_CN" ,
			"CONTENTS_EN" ,
			"CONTENTS_KR" ,
			"REGDATE" ,
			"BD_TYPE"
		};

		public CsEsmNoticeModels GetCsEsmNoticeList(CsEsmNoticeModels model)
		{
			string errorStr = "";


			string listQuery = " SELECT bn.SEQNO , esmU.EMAIL , " + string.Join(",", selectColoum_CsNotice);
			string cntQuery = " SELECT count(*) as cnt ";


			string baseQuery = " FROM bd_notice bn left outer join esm_user esmU on bn.WRITER_ID = esmU.SEQNO WHERE EST_CODE is Null ";

			if (!String.IsNullOrEmpty(model.schType))  //공지유형
				baseQuery += " AND  bn.BD_TYPE = " + model.schType.Trim();

			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  bn.REGDATE >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  bn.REGDATE <= '" + model.schEdt.Trim() + " 23:59:59'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt))  //검색조건 검색어
				baseQuery += " AND  bn." + model.schTypeTxt.Trim() + " like '%" + model.schTxt.Trim() + "%' ";

			string endQuery = " ORDER BY " + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리

			int totCnt = getQueryCnt(cntQuery, out errorStr);   //전체 리스트 갯수 구하기
			//model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;  //총 페이징 갯수 구하기

			model.Paging.totCnt = totCnt;   //전체 갯수 세팅
			model.Paging.startCnt = totCnt - (model.Paging.pageNum * (model.Paging.page - 1));  //리스트 첫번째 시작 번호 
			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;  //총 페이징 갯수 구하기
			if ((totCnt % model.Paging.pageNum) == 0) model.Paging.pageTotNum -= 1;  //총 페이징 갯수가 0 일경우 + 1

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					BdNotice temp = new BdNotice();


					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.WRITER_ID = int.Parse(listDt.Rows[i]["WRITER_ID"].ToString().Trim());
					temp.TITLE_CN = listDt.Rows[i]["TITLE_CN"].ToString().Trim();
					temp.TITLE_EN = listDt.Rows[i]["TITLE_EN"].ToString().Trim();
					temp.TITLE_KR = listDt.Rows[i]["TITLE_KR"].ToString().Trim();
					temp.CONTENTS_CN = listDt.Rows[i]["CONTENTS_CN"].ToString().Trim();
					temp.CONTENTS_EN = listDt.Rows[i]["CONTENTS_EN"].ToString().Trim();
					temp.CONTENTS_KR = listDt.Rows[i]["CONTENTS_KR"].ToString().Trim();
					temp.REGDATE = listDt.Rows[i]["REGDATE"].ToString().Trim();
					temp.BD_TYPE = int.Parse(listDt.Rows[i]["BD_TYPE"].ToString().Trim());

					foreach (schTypeArray tempS in model.schTypeArray)
					{
						if (tempS.opt_value == listDt.Rows[i]["BD_TYPE"].ToString().Trim())
						{
							temp.BD_TYPE = int.Parse(listDt.Rows[i]["BD_TYPE"].ToString().Trim());
							temp.BD_TYPE_txt = tempS.opt_key;
						}
					}

					model.Items.Add(temp);
				}
			}

			return model;
		}

		public CsEstNoticeModels GetCsEstNoticeList(CsEstNoticeModels model)
		{
			string errorStr = "";


			string listQuery = " SELECT bn.SEQNO , esmU.EMAIL , " + string.Join(",", selectColoum_CsNotice);
			string cntQuery = " SELECT count(*) as cnt ";

			string baseQuery = " FROM bd_notice bn left outer join esm_user esmU on bn.WRITER_ID = esmU.SEQNO WHERE EST_CODE is Not Null ";

			if (!String.IsNullOrEmpty(model.schType))  //공지유형
				baseQuery += " AND  bn.BD_TYPE = " + model.schType.Trim();

			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  bn.REGDATE >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  bn.REGDATE <= '" + model.schEdt.Trim() + " 23:59:59'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt))  //검색조건 검색어
				baseQuery += " AND  bn." + model.schTypeTxt.Trim() + " like '%" + model.schTxt.Trim() + "%' ";

			string endQuery = " ORDER BY " + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리

			int totCnt = getQueryCnt(cntQuery, out errorStr);   //전체 리스트 갯수 구하기

			model.Paging.totCnt = totCnt;   //전체 갯수 세팅
			model.Paging.startCnt = totCnt - (model.Paging.pageNum * (model.Paging.page - 1));  //리스트 첫번째 시작 번호 
			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;  //총 페이징 갯수 구하기
			if ((totCnt % model.Paging.pageNum) == 0) model.Paging.pageTotNum -= 1;

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					BdNotice temp = new BdNotice();


					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.WRITER_ID = int.Parse(listDt.Rows[i]["WRITER_ID"].ToString().Trim());
					temp.TITLE_CN = listDt.Rows[i]["TITLE_CN"].ToString().Trim();
					temp.TITLE_EN = listDt.Rows[i]["TITLE_EN"].ToString().Trim();
					temp.TITLE_KR = listDt.Rows[i]["TITLE_KR"].ToString().Trim();
					temp.CONTENTS_CN = listDt.Rows[i]["CONTENTS_CN"].ToString().Trim();
					temp.CONTENTS_EN = listDt.Rows[i]["CONTENTS_EN"].ToString().Trim();
					temp.CONTENTS_KR = listDt.Rows[i]["CONTENTS_KR"].ToString().Trim();
					temp.REGDATE = listDt.Rows[i]["REGDATE"].ToString().Trim();
					temp.BD_TYPE = int.Parse(listDt.Rows[i]["BD_TYPE"].ToString().Trim());

					foreach (schTypeArray tempS in model.schTypeArray)
					{
						if (tempS.opt_value == listDt.Rows[i]["BD_TYPE"].ToString().Trim())
						{
							temp.BD_TYPE = int.Parse(listDt.Rows[i]["BD_TYPE"].ToString().Trim());
							temp.BD_TYPE_txt = tempS.opt_key;
						}
					}

					model.Items.Add(temp);
				}
			}

			return model;
		}

		public BdNotice GetCsEsmNoticeView(CsEsmNoticeModels getModel)
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
					//model.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.WRITER_ID = int.Parse(listDt.Rows[0]["WRITER_ID"].ToString().Trim());
					model.TITLE_CN = listDt.Rows[0]["TITLE_CN"].ToString().Trim();
					model.TITLE_EN = listDt.Rows[0]["TITLE_EN"].ToString().Trim();
					model.TITLE_KR = listDt.Rows[0]["TITLE_KR"].ToString().Trim();
					model.CONTENTS_CN = listDt.Rows[0]["CONTENTS_CN"].ToString().Trim();
					model.CONTENTS_EN = listDt.Rows[0]["CONTENTS_EN"].ToString().Trim();
					model.CONTENTS_KR = listDt.Rows[0]["CONTENTS_KR"].ToString().Trim();
					model.REGDATE = listDt.Rows[0]["REGDATE"].ToString().Trim();
					//model.UP_DATE = listDt.Rows[0]["UP_DATE"].ToString().Trim();
					//model.READ_NUM = int.Parse(listDt.Rows[0]["READ_NUM"].ToString().Trim());
					//model.POPUP_DISPLAY = int.Parse(listDt.Rows[0]["POPUP_DISPLAY"].ToString().Trim());
					//model.POPUP_START = listDt.Rows[0]["POPUP_START"].ToString().Trim();
					//model.POPUP_END = listDt.Rows[0]["POPUP_END"].ToString().Trim();
					//model.WEB_DISPLAY = int.Parse(listDt.Rows[0]["WEB_DISPLAY"].ToString().Trim());
					model.BD_TYPE = int.Parse(listDt.Rows[0]["BD_TYPE"].ToString().Trim());
					model.WRITER_NAME = listDt.Rows[0]["EMAIL"].ToString().Trim();
				}
			}

			return model;
		}

		public BdNotice GetCsEstNoticeView(CsEstNoticeModels getModel)
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
					//model.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.WRITER_ID = int.Parse(listDt.Rows[0]["WRITER_ID"].ToString().Trim());
					model.TITLE_CN = listDt.Rows[0]["TITLE_CN"].ToString().Trim();
					model.TITLE_EN = listDt.Rows[0]["TITLE_EN"].ToString().Trim();
					model.TITLE_KR = listDt.Rows[0]["TITLE_KR"].ToString().Trim();
					model.CONTENTS_CN = listDt.Rows[0]["CONTENTS_CN"].ToString().Trim();
					model.CONTENTS_EN = listDt.Rows[0]["CONTENTS_EN"].ToString().Trim();
					model.CONTENTS_KR = listDt.Rows[0]["CONTENTS_KR"].ToString().Trim();
					model.REGDATE = listDt.Rows[0]["REGDATE"].ToString().Trim();
					//model.UP_DATE = listDt.Rows[0]["UP_DATE"].ToString().Trim();
					//model.READ_NUM = int.Parse(listDt.Rows[0]["READ_NUM"].ToString().Trim());
					//model.POPUP_DISPLAY = int.Parse(listDt.Rows[0]["POPUP_DISPLAY"].ToString().Trim());
					//model.POPUP_START = listDt.Rows[0]["POPUP_START"].ToString().Trim();
					//model.POPUP_END = listDt.Rows[0]["POPUP_END"].ToString().Trim();
					//model.WEB_DISPLAY = int.Parse(listDt.Rows[0]["WEB_DISPLAY"].ToString().Trim());
					model.BD_TYPE = int.Parse(listDt.Rows[0]["BD_TYPE"].ToString().Trim());
					model.WRITER_NAME = listDt.Rows[0]["EMAIL"].ToString().Trim();
				}
			}

			return model;
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
				baseQuery += " AND  bq.QNA_TYPE = " + model.schType.Trim();
			
			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  bq.REGDATE >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  bq.REGDATE <= '" + model.schEdt.Trim() + " 23:59:59'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt.Trim() + "%' ";

			string endQuery = " ORDER BY " + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리

			int totCnt = getQueryCnt(cntQuery, out errorStr);   //전체 리스트 갯수 구하기

			model.Paging.totCnt = totCnt;   //전체 갯수 세팅
			model.Paging.startCnt = totCnt - (model.Paging.pageNum * (model.Paging.page - 1));  //리스트 첫번째 시작 번호 
			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;  //총 페이징 갯수 구하기
			if ((totCnt % model.Paging.pageNum) == 0) model.Paging.pageTotNum -= 1;

			DataTable listDt = getQueryResult(listQuery, out errorStr);

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

				string ViewQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_CsQna) + " FROM bd_qna WHERE SEQNO = " + getModel.act_key;
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
					

					foreach (schTypeArray tempS in getModel.schTypeArray)
					{
						if (tempS.opt_value == listDt.Rows[0]["QNA_TYPE"].ToString().Trim())
						{
							model.QNA_TYPE = int.Parse(listDt.Rows[0]["QNA_TYPE"].ToString().Trim());
							model.QNA_TYPE_txt = tempS.opt_key;
						}
					}
				}
			}

			if (getModel.act_type != null && getModel.act_type == "view")
			{
				string ViewQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_CsQna) + " FROM bd_qna WHERE SEQNO = " + getModel.act_key;
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


					foreach (schTypeArray tempS in getModel.schTypeArray)
					{
						if (tempS.opt_value == listDt.Rows[0]["QNA_TYPE"].ToString().Trim())
						{
							model.QNA_TYPE = int.Parse(listDt.Rows[0]["QNA_TYPE"].ToString().Trim());
							model.QNA_TYPE_txt = tempS.opt_key;
						}
					}
				}

			}

				return model;
		}


		
		public string setCsQnaModels(CsQnaModels model)
		{
			string result = "";
			string errorStr = "";
			string exeQueryStr = "";


			if (model.act_type != null && model.act_type == "ins")
			{
				exeQueryStr = " INSERT INTO bd_qna (ESE_CODE , TITLE, QUESTION, QNA_TYPE, WRITER_ID) VALUES ( ";
				exeQueryStr += " '" + model.Item.ESE_CODE + "' ";
				exeQueryStr += " ,'" + model.Item.TITLE + "' ";
				exeQueryStr += " ,'" + model.Item.QUESTION + "' ";
				exeQueryStr += " , " + model.schType + " ";
				exeQueryStr += " , " + model.Item.WRITER_ID + " ";
				exeQueryStr += " ) ";

			}

			else if (model.act_type != null && model.act_type == "updt")
			{

				exeQueryStr = " UPDATE bd_qna SET ";
				exeQueryStr += "ESE_CODE = '" + model.Item.ESE_CODE + "'";
				exeQueryStr += ",TITLE = '" + model.Item.TITLE + "'";
				exeQueryStr += ",QUESTION =  '" + model.Item.QUESTION + "'";
				exeQueryStr += " ,QNA_TYPE= " + model.schType + " ";
				exeQueryStr += " , WRITER_ID=" + model.Item.WRITER_ID + " ";
				exeQueryStr += " WHERE SEQNO = " + model.Item.SEQNO;




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

	}
}
