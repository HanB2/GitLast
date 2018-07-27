using comm_dbconn;
using comm_global;
using comm_model;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Stoc;
using static web_ese.Models_Act.Stoc.StocInOutModels;
using static web_ese.Models_Act.Stoc.StocListModels;
using static web_ese.Models_Act.Stoc.StocReqModels;

namespace web_ese.Models_Db
{
	public class StocDbModels : DatabaseConnection
	{


		//AJAX 바코드 조회
		public StocReqModels StocReqChkBarcode(StocReqModels model)
		{
			string errorStr = "";
			string listQuery = " ";

			listQuery = " SELECT PRODUCT_NAME, BRAND, SALE_SITE_URL, SKU FROM stc_goods ";
			listQuery += " WHERE EST_CODE = '" + model.AjaxEstCode + "' AND ESE_CODE = '" + model.AjaxEseCode + "' AND BARCODE = '" + model.AjaxBarCode + "' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.InItem.EST_CODE = model.AjaxEstCode;
					model.InItem.ESE_CODE = model.AjaxEseCode;
					model.InItem.BARCODE = model.AjaxBarCode;
					model.InItem.GOODS_NAME = listDt.Rows[0]["PRODUCT_NAME"].ToString().Trim();
					model.InItem.SKU = listDt.Rows[0]["SKU"].ToString().Trim();
					model.InItem.BRAND = listDt.Rows[0]["BRAND"].ToString().Trim();
					model.InItem.PURCHASE_URL = listDt.Rows[0]["SALE_SITE_URL"].ToString().Trim();
				}
			}

			return model;
		}



		//기본정보 세팅
		public StocReqModels StocReqSetBase(StocReqModels model)
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

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();

			string errorStr = "";
			string listQuery = " ";


			//ESE 정보 가져오기 
			listQuery = " SELECT ESE_NAME, TELNO_REP, ZIPCODE, ADDR FROM est_sender WHERE ESE_CODE = '" + eseCode + "' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.BaseEseName = listDt.Rows[0]["ESE_NAME"].ToString().Trim();
					model.BaseEseTel = listDt.Rows[0]["TELNO_REP"].ToString().Trim();
					model.BaseAddr = listDt.Rows[0]["ZIPCODE"].ToString().Trim() + " " + listDt.Rows[0]["ADDR"].ToString().Trim();

				}
			}


			//소속 스테이션 정보 가져오기 
			listQuery = " SELECT ";
			listQuery += " est.NATION_CODE ";
			listQuery += " ,est.EST_CODE ";
			listQuery += " ,ese.ESE_CODE ";
			listQuery += " ,est.EST_NAME ";
			listQuery += " ,est.WEIGHT_UNIT ";
			listQuery += " ,cn." + getCol;
			listQuery += " FROM ";
			listQuery += " esm_station est ";
			listQuery += " inner join	";
			listQuery += " (SELECT EST_CODE, ESE_CODE FROM ese_user WHERE ESE_CODE = '" + eseCode + "' ) ese ";
			listQuery += " on est.EST_CODE = ese.EST_CODE ";
			listQuery += " inner join ";
			listQuery += " comm_nation cn	";
			listQuery += " on est.NATION_CODE = cn.NATIONNO	";

			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.BaseEstCode = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.BaseEstName = listDt.Rows[0]["EST_NAME"].ToString().Trim();
					model.BaseEseCode = eseCode;
					model.BaseNationCode = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
					model.BaseNationName = listDt.Rows[0][getCol].ToString().Trim();
					
				}
			}
			
			//기본정보 세팅
			model.arraySNation.Add(new schTypeArray { opt_key = model.BaseNationCode, opt_value = model.BaseNationName });
			model.arrayStation.Add(new schTypeArray { opt_key = model.BaseEstCode, opt_value = model.BaseEstName });

			//사용 나라 목록 가져오기
			listQuery = "	SELECT		";
			listQuery += " 		cn.NATIONNO		";
			listQuery += " 		,cn." + getCol + "		";
			listQuery += " 	FROM		";
			listQuery += " 		(SELECT distinct NATION_CODE FROM ese_use_est		";
			listQuery += " 		WHERE EST_CODE = '" + model.BaseEstCode + "' AND ESE_CODE = '" + model.BaseEseCode + "'  AND NATION_CODE != '" + model.BaseNationCode + "' ) eue		";
			listQuery += " 		inner join		";
			listQuery += " 		comm_nation cn		";
			listQuery += " 		on eue.NATION_CODE = cn.NATIONNO		";


			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.arraySNation.Add(new schTypeArray { opt_key = listDt.Rows[i]["NATIONNO"].ToString().Trim(), opt_value = listDt.Rows[i][getCol].ToString().Trim() });
				}
			}

			listQuery = " SELECT ";
			listQuery += " est.EST_CODE ";
			listQuery += " ,est.EST_NAME ";
			listQuery += " FROM ";
			listQuery += " (SELECT USE_EST FROM ese_use_est ";
			listQuery += " WHERE EST_CODE = '" + model.BaseEstCode + "' AND ESE_CODE = '" + model.BaseEseCode + "' AND NATION_CODE = '" + model.BaseNationCode + "') eue ";
			listQuery += " inner join ";
			listQuery += " esm_station est ";
			listQuery += " on eue.USE_EST = est.EST_CODE ";


			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.arrayStation.Add(new schTypeArray { opt_key = listDt.Rows[i]["EST_CODE"].ToString().Trim(), opt_value = listDt.Rows[i]["EST_NAME"].ToString().Trim() });
				}
			}
			
			return model;
		}

		





		string[] selectColoum_StocReq = {
			"EST_CODE" ,
			"ESE_CODE" ,
			"INPUT_TYPE" ,
			"INPUT_DELVNO" ,
			"SENDER_NAME" ,
			"SENDER_TEL" ,
			"SENDER_ADDR" ,
			"MEMO_ESE" ,
			"MEMO_EST" ,
			"INPUT_STAT" ,
			"REG_DT",
			"UPDT_DT" 
		};
		
		public StocReqModels GetStocReqView(StocReqModels model)
		{
			string errorStr = "";


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


			if (model.act_type != null && model.act_type == "updt")
			{
				string ViewQuery = " SELECT  sir.SEQNO,  sir.EST_CODE,sir.ESE_CODE,sir.INPUT_TYPE,sir.INPUT_DELVNO,sir.SENDER_NAME,sir.SENDER_TEL,sir.SENDER_ADDR,sir.MEMO_ESE,sir.MEMO_EST,sir.INPUT_STAT,sir.REG_DT,sir.UPDT_DT ,est.EST_NAME , cn.NATIONNAME";
				ViewQuery += " FROM ( SELECT * FROM stc_in_req WHERE SEQNO = " + model.act_key + ") sir ";
				ViewQuery += " left outer join esm_station est on sir.EST_CODE = est.EST_CODE left outer join comm_nation cn on cn.NATIONNO = est."+ getCol + " ";

				DataTable listDt = getQueryResult(ViewQuery, out errorStr);
				
				if (listDt != null && listDt.Rows.Count != 0)
				{
					model.Item.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
					model.Item.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.Item.ESE_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.Item.INPUT_TYPE = int.Parse(listDt.Rows[0]["INPUT_TYPE"].ToString().Trim());
					model.Item.INPUT_DELVNO = listDt.Rows[0]["INPUT_DELVNO"].ToString().Trim();
					model.Item.SENDER_NAME = listDt.Rows[0]["SENDER_NAME"].ToString().Trim();
					model.Item.SENDER_TEL = listDt.Rows[0]["SENDER_TEL"].ToString().Trim();
					model.Item.SENDER_ADDR = listDt.Rows[0]["SENDER_ADDR"].ToString().Trim();
					model.Item.MEMO_ESE = listDt.Rows[0]["MEMO_ESE"].ToString().Trim();
					model.Item.MEMO_EST = listDt.Rows[0]["MEMO_EST"].ToString().Trim();
					model.Item.INPUT_STAT = int.Parse(listDt.Rows[0]["INPUT_STAT"].ToString().Trim());
					model.Item.EST_NAME = listDt.Rows[0]["EST_NAME"].ToString().Trim();
					model.Item.NATIONNAME = listDt.Rows[0]["NATIONNAME"].ToString().Trim();
					
					foreach (schTypeArray tempS in model.schTypeArray)
					{
						if (tempS.opt_key == listDt.Rows[0]["INPUT_STAT"].ToString().Trim())
						{
							model.Item.INPUT_STAT_TEXT = tempS.opt_value;
						}
					}
					foreach (schTypeArray tempS in model.inputTypeArray)
					{
						if (tempS.opt_key == listDt.Rows[0]["INPUT_TYPE"].ToString().Trim())
						{
							model.Item.INPUT_TYPE_TEXT = tempS.opt_value;
						}
					}
					
				}
				
				ViewQuery = " SELECT sirg.BARCODE, sirg.BOXNUM, sirg.CNT, sirg.REAL_CNT, sirg.BAD_CNT, sg.PRODUCT_NAME, sg.SKU, sg.BRAND ";
				ViewQuery += " FROM stc_in_req_goods sirg left outer join stc_goods sg on sirg.BARCODE = sg.BARCODE ";
				ViewQuery += " WHERE REQ_SEQNO = " + model.act_key;
				
				listDt = getQueryResult(ViewQuery, out errorStr);


				if (listDt != null && listDt.Rows.Count != 0)
				{
					for (int i = 0; i < listDt.Rows.Count; i++)
					{
						OrdGoods temp = new OrdGoods();

						temp.BARCODE = listDt.Rows[i]["BARCODE"].ToString().Trim();
						temp.SKU = listDt.Rows[i]["SKU"].ToString().Trim();
						temp.GOODS_NAME = listDt.Rows[i]["PRODUCT_NAME"].ToString().Trim();
						temp.BRAND = listDt.Rows[i]["BRAND"].ToString().Trim();
						temp.BOXNUM = listDt.Rows[i]["BOXNUM"].ToString().Trim();
						temp.QTY = int.Parse(listDt.Rows[i]["CNT"].ToString().Trim());

						temp.REAL_CNT = 0;
						if (!string.IsNullOrEmpty(listDt.Rows[i]["REAL_CNT"].ToString().Trim()))
							temp.REAL_CNT = int.Parse(listDt.Rows[i]["REAL_CNT"].ToString().Trim());

						temp.BAD_CNT = 0;
						if (!string.IsNullOrEmpty(listDt.Rows[i]["BAD_CNT"].ToString().Trim()))
							temp.BAD_CNT = int.Parse(listDt.Rows[i]["BAD_CNT"].ToString().Trim());

						model.InItems.Add(temp);
					}
				}
			}

			return (model);
		}



		public string SetStocReq(StocReqModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";
			List<string> exeQueryArray = new List<string>();

			if (model.act_type != null && model.act_type == "ins")
			{
				exeQueryStr = " INSERT INTO stc_in_req ( EST_CODE, ESE_CODE, INPUT_TYPE, INPUT_DELVNO, SENDER_NAME, SENDER_TEL, SENDER_ADDR, MEMO_ESE, INPUT_STAT, INPUT_DELVNAME, INPUT_DELVTELL ) VALUES (";
				exeQueryStr += "  '" + model.GET_EST_CODE + "'";
				exeQueryStr += " , '" + model.BaseEseCode + "'";
				exeQueryStr += " , " + model.GET_INPUT_TYPE + "";
				exeQueryStr += " , '" + model.Item.INPUT_DELVNO + "'";
				exeQueryStr += " , '" + model.Item.SENDER_NAME + "'";
				exeQueryStr += " , '" + model.Item.SENDER_TEL + "'";
				exeQueryStr += " , '" + model.Item.SENDER_ADDR + "'";
				exeQueryStr += " , '" + model.Item.MEMO_ESE + "'";
				exeQueryStr += " , 10 ";
				exeQueryStr += " , '" + model.Item.INPUT_DELVNAME + "'";
				exeQueryStr += " , '" + model.Item.INPUT_DELVTELL + "'";
				exeQueryStr += " ) ";

				exeQueryArray.Add(exeQueryStr);
				foreach (StcInReqGoods item in model.Items)
				{
					exeQueryStr = " INSERT INTO stc_in_req_goods ( REQ_SEQNO, BARCODE, BOXNUM, CNT ) VALUES (";
					exeQueryStr += "  ( SELECT max(SEQNO) FROM stc_in_req )";
					exeQueryStr += " , '" + item.BARCODE + "'";
					exeQueryStr += " , '" + item.BOXNUM + "'";
					exeQueryStr += " , " + item.CNT + "";
					exeQueryStr += " ) ";

					exeQueryArray.Add(exeQueryStr);
				}

			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				exeQueryStr = " UPDATE stc_in_req SET ";
				exeQueryStr += "  EST_CODE = '" + model.Item.EST_CODE + "'";
				exeQueryStr += " , INPUT_TYPE = '" + model.Item.INPUT_TYPE + "'";
				exeQueryStr += " , INPUT_DELVNO = '" + model.Item.INPUT_DELVNO + "'";
				exeQueryStr += " , INPUT_DELVNAME = '" + model.Item.INPUT_DELVNAME + "'";
				exeQueryStr += " , INPUT_DELVTELL = '" + model.Item.INPUT_DELVTELL + "'";
				exeQueryStr += " , SENDER_NAME = '" + model.Item.SENDER_NAME + "'";
				exeQueryStr += " , SENDER_TEL = '" + model.Item.SENDER_TEL + "'";
				exeQueryStr += " , SENDER_ADDR = '" + model.Item.SENDER_ADDR + "'";
				exeQueryStr += " , MEMO_ESE = '" + model.Item.MEMO_ESE + "'";
				exeQueryStr += " , UPDT_DT = CURRENT_TIMESTAMP ";
				exeQueryStr += " WHERE SEQNO = " + model.act_key;

				exeQueryArray.Add(exeQueryStr);

				exeQueryArray.Add("DELETE FROM stc_in_req_goods WHERE REQ_SEQNO = " + model.act_key) ;

				foreach (StcInReqGoods item in model.Items)
				{
					exeQueryStr = " INSERT INTO stc_in_req_goods ( REQ_SEQNO, BARCODE, BOXNUM, CNT ) VALUES (";
					exeQueryStr += "  '" + model.act_key + "'";
					exeQueryStr += " , '" + item.BARCODE + "'";
					exeQueryStr += " , '" + item.BOXNUM + "'";
					exeQueryStr += " , " + item.CNT + "";
					exeQueryStr += " ) ";

					exeQueryArray.Add(exeQueryStr);
				}
			}
			else
			{
				result = "잘못된 접근입니다.";
				return result;
			}

			if (exeQuery(exeQueryArray, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}

			return result;
			
		}



		public string delStocReqList(int seqNo)
		{
			string errorStr = "";
			string result = "";
			List<string> exeQueryArray = new List<string>();
			
			exeQueryArray.Add(" UPDATE stc_in_req SET INPUT_STAT = 5 WHERE SEQNO = " + seqNo);
			
			if (exeQuery(exeQueryArray, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}

			return result;

		}


		public string delStocReqList(string seq)
		{
			string errorStr = "";
			string result = "";
			List<string> exeQueryArray = new List<string>();

			exeQueryArray.Add(" UPDATE stc_in_req SET INPUT_STAT = 5 WHERE SEQNO in (" + seq + ") ");

			if (exeQuery(exeQueryArray, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}

			return result;
		}

		public string chkStocReq(int seqNo)
		{
			string errorStr = "";
			string result = "";
			List<string> exeQueryArray = new List<string>();

			exeQueryArray.Add(" UPDATE stc_in_req SET INPUT_STAT = 25 , CHK_DT = CURRENT_TIMESTAMP   WHERE SEQNO = " + seqNo);

			if (exeQuery(exeQueryArray, out errorStr))
			{
				result = "성공.";
			}
			else
			{
				result = "실패.";
			}

			return result;

		}


		public StocReqModels GetStocReqList(StocReqModels model) 
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


			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();

			string errorStr = "";

			string listQuery = " SELECT sir.SEQNO, sir.EST_CODE , sir.INPUT_TYPE, sir.MEMO_EST, sir.INPUT_STAT, sir.REG_DT, sir.UPDT_DT, sir.CHK_DT ,est.EST_NAME, est."+getCol+", sirg.BOXNUM ,sirg.cnt ";
			string cntQuery = " SELECT count(*) as cnt ";

			string baseQuery = "FROM (SELECT SEQNO, EST_CODE, INPUT_TYPE, MEMO_EST, INPUT_STAT, REG_DT, UPDT_DT, CHK_DT FROM stc_in_req WHERE ESE_CODE = '" + eseCode + "' ";

			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  EST_CODE = '" + model.schStation.Trim() + "' ";
			
			if (!String.IsNullOrEmpty(model.schType))  
				baseQuery += " AND  INPUT_STAT = " + model.schType.Trim();

			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  REG_DT >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  REG_DT <= '" + model.schEdt.Trim() + " 23:59:59'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt.Trim() + "%' ";

			
			
			
			string lastQuery = " ) sir ";
			lastQuery += " inner join ";
			lastQuery += " ( SELECT in_est.EST_NAME, in_est.EST_CODE, cn." + getCol + " FROM esm_station in_est left outer join comm_nation cn on in_est.NATION_CODE = cn.NATIONNO ";

			if (!String.IsNullOrEmpty(model.schNation))
				lastQuery += " WHERE in_est.NATION_CODE = '" + model.schNation.Trim() + "' ";


			lastQuery += " ) est on sir.EST_CODE = est.EST_CODE ";
			lastQuery += " left outer join ";
			lastQuery += " ( SELECT REQ_SEQNO, Count(Distinct BOXNUM) As BOXNUM, SUM(CNT)As cnt, group_concat(in_sg.BRAND) as BRAND FROM stc_in_req_goods in_sirg left outer join stc_goods in_sg on  in_sirg.BARCODE = in_sg.BARCODE group by REQ_SEQNO  ) sirg on sir.SEQNO = sirg.REQ_SEQNO ";
			
			if (!String.IsNullOrEmpty(model.schTxt2))  //검색조건 검색어
				lastQuery += " WHERE sirg.BRAND like '%"+ model.schTxt2 + "%' ";

			string endQuery = " ORDER BY sir." + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬


			cntQuery += baseQuery + lastQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + lastQuery + endQuery;  //리스트 쿼리

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
					StcInReq temp = new StcInReq();
					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.INPUT_TYPE = int.Parse(listDt.Rows[i]["INPUT_TYPE"].ToString().Trim());
					temp.MEMO_EST = listDt.Rows[i]["MEMO_EST"].ToString().Trim();
					temp.INPUT_STAT = int.Parse(listDt.Rows[i]["INPUT_STAT"].ToString().Trim());
					temp.REG_DT = listDt.Rows[i]["REG_DT"].ToString().Trim();
					temp.UPDT_DT = listDt.Rows[i]["UPDT_DT"].ToString().Trim();
					temp.CHK_DT = listDt.Rows[i]["CHK_DT"].ToString().Trim();
					temp.EST_NAME = listDt.Rows[i]["EST_NAME"].ToString().Trim();
					temp.BOXNUM = listDt.Rows[i]["BOXNUM"].ToString().Trim();
					temp.CNT = listDt.Rows[i]["cnt"].ToString().Trim();
					temp.NATIONNAME = listDt.Rows[i][getCol].ToString().Trim();

					temp.CHK_TEXT = "-";
					if (temp.INPUT_STAT == 1)
						temp.CHK_TEXT = "OK"; 

					foreach (schTypeArray tempS in model.schTypeArray)
					{
						if (tempS.opt_key == listDt.Rows[i]["INPUT_STAT"].ToString().Trim())
						{
							temp.INPUT_STAT_TEXT = tempS.opt_value;
						}
					}

					model.ListItems.Add(temp);
				}
			}

			return model;
		}








		public StocListModels StocListBase(StocListModels model)
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

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();

			string errorStr = "";
			string listQuery = " ";
			
			DataTable listDt = new DataTable();
			

			//소속 스테이션 정보 가져오기 
			listQuery = " SELECT ";
			listQuery += " est.NATION_CODE ";
			listQuery += " ,est.EST_CODE ";
			listQuery += " ,ese.ESE_CODE ";
			listQuery += " ,est.EST_NAME ";
			listQuery += " ,est.WEIGHT_UNIT ";
			listQuery += " ,cn." + getCol;
			listQuery += " FROM ";
			listQuery += " esm_station est ";
			listQuery += " inner join	";
			listQuery += " (SELECT EST_CODE, ESE_CODE FROM ese_user WHERE ESE_CODE = '" + eseCode + "' ) ese ";
			listQuery += " on est.EST_CODE = ese.EST_CODE ";
			listQuery += " inner join ";
			listQuery += " comm_nation cn	";
			listQuery += " on est.NATION_CODE = cn.NATIONNO	";

			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.BaseEstCode = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.BaseEstName = listDt.Rows[0]["EST_NAME"].ToString().Trim();
					model.BaseEseCode = eseCode;
					model.BaseNationCode = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
					model.BaseNationName = listDt.Rows[0][getCol].ToString().Trim();

				}
			}

			//기본정보 세팅
			model.arraySNation.Add(new schTypeArray { opt_key = model.BaseNationCode, opt_value = model.BaseNationName });

			//사용 나라 목록 가져오기
			listQuery = "	SELECT		";
			listQuery += " 		cn.NATIONNO		";
			listQuery += " 		,cn." + getCol + "		";
			listQuery += " 	FROM		";
			listQuery += " 		(SELECT distinct NATION_CODE FROM ese_use_est		";
			listQuery += " 		WHERE EST_CODE = '" + model.BaseEstCode + "' AND ESE_CODE = '" + model.BaseEseCode + "'  AND NATION_CODE != '" + model.BaseNationCode + "' ) eue		";
			listQuery += " 		inner join		";
			listQuery += " 		comm_nation cn		";
			listQuery += " 		on eue.NATION_CODE = cn.NATIONNO		";


			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.arraySNation.Add(new schTypeArray { opt_key = listDt.Rows[i]["NATIONNO"].ToString().Trim(), opt_value = listDt.Rows[i][getCol].ToString().Trim() });
				}
			}
			
			ProdDbModels act = new ProdDbModels();      //DB커넥션 클래스 선언

			model.cateList1 = act.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기

			return model;
		}

		public StocListModels GetStocList(StocListModels model)
		{

			string lan = "";

			string getNationCol = "";
			string getCateCol = "";
			switch (lan)    //lan <== 다국어 지원 관련 언어선택 부분이므로 일단 신경쓰지 말고 진행 할것  그냥 돌리면 알아서 NATIONNAME_ko_KR 을 설정함
			{
				case "CN":
					getCateCol = "CATE_NAME_CN";
					getNationCol = "NATIONNAME_zh_CN";
					break;
				case "EN":
					getCateCol = "CATE_NAME_EN";
					getNationCol = "NATIONNAME";
					break;
				default:
					getCateCol = "CATE_NAME_KR";
					getNationCol = "NATIONNAME_ko_KR";
					break;
			}

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();


			string errorStr = "";
			string baseQuery = "";

			string listQuery = "SELECT si.EST_CODE, si.BARCODE, IN_CNT, OUT_CNT, BAD_CNT, CHANGE_CNT, IFNULL(EXP_OUT_CNT,0) as EXP_OUT_CNT , est.EST_NAME, est."+ getNationCol+ ",  sg.PRODUCT_NAME, sg.BRAND, sg.CATEGORY1, sg.CATEGORY2, sg.CATEGORY3, sg.CATEGORY4  ";
			string cntQuery = " SELECT count(*) as cnt ";

			baseQuery += " FROM ( SELECT EST_CODE, BARCODE, sum( IF(INOUT_TYPE=0 , GOODS_CNT , 0)) IN_CNT, sum( IF(INOUT_TYPE=1 , GOODS_CNT , 0)) OUT_CNT, sum( IF(INOUT_TYPE=2 , GOODS_CNT , 0)) BAD_CNT, sum( IF(INOUT_TYPE=3 , GOODS_CNT , 0)) CHANGE_CNT FROM stc_inout ";
			baseQuery += " WHERE ESE_CODE = '" + eseCode + "' ";
			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  EST_CODE = '" + model.schStation.Trim() + "' ";
			baseQuery += " GROUP BY EST_CODE, BARCODE ) si  ";
			baseQuery += " inner join ";
			baseQuery += " ( SELECT in_est.EST_CODE, in_est.EST_NAME, in_cn."+ getNationCol + " FROM esm_station in_est left outer join comm_nation in_cn on in_cn.NATIONNO = in_est.NATION_CODE WHERE 1=1";
			
			if (!String.IsNullOrEmpty(model.schNation))
				baseQuery += " AND in_est.NATION_CODE = '"+ model.schNation.Trim() + "' ";

			//baseQuery += " ) est on si.EST_CODE = est.EST_CODE inner join ( SELECT PRODUCT_NAME, BRAND, BARCODE, CATE, " + getCateCol + " FROM ( SELECT PRODUCT_NAME, BRAND, BARCODE, IF( CATEGORY4 = 0 , IF( CATEGORY3 = 0 , IF( CATEGORY2 = 0 , CATEGORY1, CATEGORY2), CATEGORY3), CATEGORY4 )as CATE    FROM stc_goods ";
			baseQuery += " ) est on si.EST_CODE = est.EST_CODE inner join ( SELECT PRODUCT_NAME, BRAND, BARCODE, CATE, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4 FROM ( SELECT PRODUCT_NAME, BRAND, BARCODE, IF( CATEGORY4 = 0 , IF( CATEGORY3 = 0 , IF( CATEGORY2 = 0 , CATEGORY1, CATEGORY2), CATEGORY3), CATEGORY4 )as CATE, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4 FROM stc_goods ";

			baseQuery += " WHERE  ESE_CODE = '" + eseCode + "' ";

			if (model.cate1 != 0)  //카테고리1
				baseQuery += " AND  CATEGORY1 = " + model.cate1;
			if (model.cate2 != 0)  //카테고리2
				baseQuery += " AND  CATEGORY2 = " + model.cate2;
			if (model.cate3 != 0)  //카테고리3
				baseQuery += " AND  CATEGORY3 = " + model.cate3;
			if (model.cate4 != 0)  //카테고리4
				baseQuery += " AND  CATEGORY4 = " + model.cate4;

			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  REG_DT >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  REG_DT <= '" + model.schEdt.Trim() + " 23:59:59'";

			baseQuery += " ) in_sg left outer join stc_category in_sc on in_sg.CATE = in_sc.SEQNO ) sg on si.BARCODE = sg.BARCODE  ";
			baseQuery += " left outer join ( SELECT BARCODE, SUM(QTY) as EXP_OUT_CNT FROM ord_goods in_ordGoods inner join(SELECT WAYBILLNO FROM ( SELECT EST_CODE, WAYBILLNO, MASTERNO FROM ord_master WHERE ORDERTYPE = 1 AND ESE_CODE = '" + eseCode + "' ) in_om ";
			baseQuery += " left outer join est_masterbl in_em on in_om.MASTERNO = in_em.MASTERNO WHERE OUTREG_STATUS = 0 OR in_om.MASTERNO is null  ";
			baseQuery += " ) in_ord on in_ordGoods.WAYBILLNO = in_ord.WAYBILLNO GROUP BY in_ordGoods.BARCODE, in_ordGoods.WAYBILLNO ) og on si.BARCODE = og.BARCODE ";
			baseQuery += " WHERE 1=1 ";
			
			if (!String.IsNullOrEmpty(model.schTxt))    //브랜드
				baseQuery += " AND sg.BRAND like '%"+ model.schTxt.Trim() + "%'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt2))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt2.Trim() + "%' ";




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
					StcListItem temp = new StcListItem();
					
					temp.SEQNO = i;
					temp.BARCODE = listDt.Rows[i]["BARCODE"].ToString().Trim();
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.NATION_NAME = listDt.Rows[i][getNationCol].ToString().Trim();
					temp.EST_NAME = listDt.Rows[i]["EST_NAME"].ToString().Trim();
					//temp.CATE_NAME = listDt.Rows[i][getCateCol].ToString().Trim();
					temp.CATEGORY1 = int.Parse(listDt.Rows[i]["CATEGORY1"].ToString().Trim());
					temp.CATEGORY2 = int.Parse(listDt.Rows[i]["CATEGORY2"].ToString().Trim());
					temp.CATEGORY3 = int.Parse(listDt.Rows[i]["CATEGORY3"].ToString().Trim());
					temp.CATEGORY4 = int.Parse(listDt.Rows[i]["CATEGORY4"].ToString().Trim());
					
					temp.GOODS_NAME = listDt.Rows[i]["PRODUCT_NAME"].ToString().Trim();
					temp.BRAND = listDt.Rows[i]["BRAND"].ToString().Trim();
					temp.IN_CNT = int.Parse(listDt.Rows[i]["IN_CNT"].ToString().Trim());
					temp.OUT_CNT = int.Parse(listDt.Rows[i]["OUT_CNT"].ToString().Trim());
					temp.BAD_CNT = int.Parse(listDt.Rows[i]["BAD_CNT"].ToString().Trim());
					temp.CHANGE_CNT = int.Parse(listDt.Rows[i]["CHANGE_CNT"].ToString().Trim());
					temp.STOC_CNT = temp.IN_CNT - temp.OUT_CNT - temp.BAD_CNT + temp.CHANGE_CNT;
					temp.EXP_OUT_CNT = int.Parse(listDt.Rows[i]["EXP_OUT_CNT"].ToString().Trim());
					temp.EXP_STOC_CNT = temp.STOC_CNT - temp.EXP_OUT_CNT;

					temp.CATE_NAME = "";
					ProdDbModels prodDb = new ProdDbModels();
					if (temp.CATEGORY1 > 0)
						temp.CATE_NAME += prodDb.GetCategoryName(temp.CATEGORY1);
					
					if (temp.CATEGORY2 > 0)
						temp.CATE_NAME += " > " + prodDb.GetCategoryName(temp.CATEGORY2);

					if (temp.CATEGORY3 > 0)
						temp.CATE_NAME += " > " + prodDb.GetCategoryName(temp.CATEGORY3);

					if (temp.CATEGORY4 > 0)
						temp.CATE_NAME += " > " + prodDb.GetCategoryName(temp.CATEGORY4);

					model.Items.Add(temp);
				}
			}

			return model;
		}


		public MemoryStream GetStocListExcel(StocListModels model)
		{

			string lan = "";

			string getNationCol = "";
			string getCateCol = "";
			switch (lan)    //lan <== 다국어 지원 관련 언어선택 부분이므로 일단 신경쓰지 말고 진행 할것  그냥 돌리면 알아서 NATIONNAME_ko_KR 을 설정함
			{
				case "CN":
					getCateCol = "CATE_NAME_CN";
					getNationCol = "NATIONNAME_zh_CN";
					break;
				case "EN":
					getCateCol = "CATE_NAME_EN";
					getNationCol = "NATIONNAME";
					break;
				default:
					getCateCol = "CATE_NAME_KR";
					getNationCol = "NATIONNAME_ko_KR";
					break;
			}

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();


			string errorStr = "";
			string baseQuery = "";

			string listQuery = "SELECT si.EST_CODE, si.BARCODE, IN_CNT, OUT_CNT, BAD_CNT, CHANGE_CNT, IFNULL(EXP_OUT_CNT,0) as EXP_OUT_CNT , est.EST_NAME, est." + getNationCol + ", sg.PRODUCT_NAME, sg.BRAND, sg.CATE, sg." + getCateCol + " ";

			baseQuery += " FROM ( SELECT EST_CODE, BARCODE, sum( IF(INOUT_TYPE=0 , GOODS_CNT , 0)) IN_CNT, sum( IF(INOUT_TYPE=1 , GOODS_CNT , 0)) OUT_CNT, sum( IF(INOUT_TYPE=2 , GOODS_CNT , 0)) BAD_CNT, sum( IF(INOUT_TYPE=3 , GOODS_CNT , 0)) CHANGE_CNT FROM stc_inout ";
			baseQuery += " WHERE ESE_CODE = '" + eseCode + "' ";
			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  EST_CODE = '" + model.schStation.Trim() + "' ";
			baseQuery += " GROUP BY EST_CODE, BARCODE ) si  ";
			baseQuery += " inner join ";
			baseQuery += " ( SELECT in_est.EST_CODE, in_est.EST_NAME, in_cn." + getNationCol + " FROM esm_station in_est left outer join comm_nation in_cn on in_cn.NATIONNO = in_est.NATION_CODE WHERE 1=1";

			if (!String.IsNullOrEmpty(model.schNation))
				baseQuery += " AND in_est.NATION_CODE = '" + model.schNation.Trim() + "' ";

			baseQuery += " ) est on si.EST_CODE = est.EST_CODE inner join ( SELECT PRODUCT_NAME, BRAND, BARCODE, CATE, " + getCateCol + " FROM ( SELECT PRODUCT_NAME, BRAND, BARCODE, IF( CATEGORY4 = 0 , IF( CATEGORY3 = 0 , IF( CATEGORY2 = 0 , CATEGORY1, CATEGORY2), CATEGORY3), CATEGORY4 )as CATE    FROM stc_goods ";
			baseQuery += " WHERE  ESE_CODE = '" + eseCode + "' ";

			if (model.cate1 != 0)  //카테고리1
				baseQuery += " AND  CATEGORY1 = " + model.cate1;
			if (model.cate2 != 0)  //카테고리2
				baseQuery += " AND  CATEGORY2 = " + model.cate2;
			if (model.cate3 != 0)  //카테고리3
				baseQuery += " AND  CATEGORY3 = " + model.cate3;
			if (model.cate4 != 0)  //카테고리4
				baseQuery += " AND  CATEGORY4 = " + model.cate4;

			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  REG_DT >= '" + model.schSdt.Trim() + "'";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  UPDT_DT <= '" + model.schEdt.Trim() + "'";

			baseQuery += " ) in_sg left outer join stc_category in_sc on in_sg.CATE = in_sc.SEQNO ) sg on si.BARCODE = sg.BARCODE  ";
			baseQuery += " left outer join ( SELECT BARCODE, SUM(QTY) as EXP_OUT_CNT FROM ord_goods in_ordGoods inner join(SELECT WAYBILLNO FROM ( SELECT EST_CODE, WAYBILLNO, MASTERNO FROM ord_master WHERE ORDERTYPE = 1 AND ESE_CODE = '" + eseCode + "' ) in_om ";
			baseQuery += " left outer join est_masterbl in_em on in_om.MASTERNO = in_em.MASTERNO WHERE OUTREG_STATUS = 0 OR in_om.MASTERNO is null  ";
			baseQuery += " ) in_ord on in_ordGoods.WAYBILLNO = in_ord.WAYBILLNO GROUP BY in_ordGoods.BARCODE, in_ordGoods.WAYBILLNO ) og on si.BARCODE = og.BARCODE ";
			baseQuery += " WHERE 1=1 ";

			if (!String.IsNullOrEmpty(model.schTxt))    //브랜드
				baseQuery += " AND sg.BRAND like '%" + model.schTxt.Trim() + "%'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt2))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt2.Trim() + "%' ";

			
			string endQuery = " ORDER BY " + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬
			
			listQuery += baseQuery + endQuery;  //리스트 쿼리
			

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			MemoryStream stream = new MemoryStream();

			using (ExcelPackage excelPackage = new ExcelPackage())
			{
				ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("SearchResult");

				int row = 1;
				int col = 1;
				// 첫줄
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_Num;  
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_NATION;   
				//workSheet.Cells[row, col++].Value = "EST_NAME";
				//workSheet.Cells[row, col++].Value = "EST_CODE";	//삭제?
				//workSheet.Cells[row, col++].Value = "CATE";     //삭제?
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_Category;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_Barcord;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_PRODUCT_NAME;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_Brand;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_StocList_Warehousing;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_StocList_Release;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_Poor;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdList_StockAdjustment;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdList_CurrentStock;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdList_ReleaseExpected;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdList_StockExpected;

				
				workSheet.Cells[row, 1, row, (col - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				workSheet.Cells[row, 1, row, (col - 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
				workSheet.Cells[row, 1, row, (col - 1)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

				// 두번째부터
				if (listDt != null && listDt.Rows.Count > 0)
				{
					for (int i = 0; i < listDt.Rows.Count; i++)
					{
						row++;
						col = 1;

						workSheet.Cells[row, col++].Value = (i + 1);  // 번호
						workSheet.Cells[row, col++].Value = listDt.Rows[i][getNationCol].ToString().Trim(); 
						//workSheet.Cells[row, col++].Value = listDt.Rows[i]["EST_NAME"].ToString().Trim();  
						//workSheet.Cells[row, col++].Value = listDt.Rows[i]["EST_CODE"].ToString().Trim();  
						//workSheet.Cells[row, col++].Value = listDt.Rows[i]["CATE"].ToString().Trim();  
						workSheet.Cells[row, col++].Value = listDt.Rows[i][getCateCol].ToString().Trim(); 
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["BARCODE"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["PRODUCT_NAME"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["BRAND"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["IN_CNT"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["OUT_CNT"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["BAD_CNT"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["CHANGE_CNT"].ToString().Trim();
						
						int IN_CNT = int.Parse(listDt.Rows[i]["IN_CNT"].ToString().Trim());
						int OUT_CNT = int.Parse(listDt.Rows[i]["OUT_CNT"].ToString().Trim());
						int BAD_CNT = int.Parse(listDt.Rows[i]["BAD_CNT"].ToString().Trim());
						int CHANGE_CNT = int.Parse(listDt.Rows[i]["CHANGE_CNT"].ToString().Trim());
						int STOC_CNT = IN_CNT - OUT_CNT - BAD_CNT + CHANGE_CNT;
						int EXP_OUT_CNT = int.Parse(listDt.Rows[i]["EXP_OUT_CNT"].ToString().Trim());
						int EXP_STOC_CNT = STOC_CNT - EXP_OUT_CNT;
						
						workSheet.Cells[row, col++].Value = STOC_CNT.ToString();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["EXP_OUT_CNT"].ToString().Trim();
						workSheet.Cells[row, col++].Value = EXP_STOC_CNT.ToString();

					}

				}

				workSheet.Cells.AutoFitColumns();

				//excelPackage.Workbook.Calculate();  //calculate all the values of the formulas in the Excel file

				excelPackage.SaveAs(stream);
			}

			stream.Flush(); //Always catches me out
			stream.Position = 0; //Not sure if this is required

			return stream;

		}






		public StocInOutModels StocInOutBase(StocInOutModels model)
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

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();

			string errorStr = "";
			string listQuery = " ";

			DataTable listDt = new DataTable();


			//소속 스테이션 정보 가져오기 
			listQuery = " SELECT ";
			listQuery += " est.NATION_CODE ";
			listQuery += " ,est.EST_CODE ";
			listQuery += " ,ese.ESE_CODE ";
			listQuery += " ,est.EST_NAME ";
			listQuery += " ,est.WEIGHT_UNIT ";
			listQuery += " ,cn." + getCol;
			listQuery += " FROM ";
			listQuery += " esm_station est ";
			listQuery += " inner join	";
			listQuery += " (SELECT EST_CODE, ESE_CODE FROM ese_user WHERE ESE_CODE = '" + eseCode + "' ) ese ";
			listQuery += " on est.EST_CODE = ese.EST_CODE ";
			listQuery += " inner join ";
			listQuery += " comm_nation cn	";
			listQuery += " on est.NATION_CODE = cn.NATIONNO	";

			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.BaseEstCode = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.BaseEstName = listDt.Rows[0]["EST_NAME"].ToString().Trim();
					model.BaseEseCode = eseCode;
					model.BaseNationCode = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
					model.BaseNationName = listDt.Rows[0][getCol].ToString().Trim();

				}
			}

			//기본정보 세팅
			model.arraySNation.Add(new schTypeArray { opt_key = model.BaseNationCode, opt_value = model.BaseNationName });

			//사용 나라 목록 가져오기
			listQuery = "	SELECT		";
			listQuery += " 		cn.NATIONNO		";
			listQuery += " 		,cn." + getCol + "		";
			listQuery += " 	FROM		";
			listQuery += " 		(SELECT distinct NATION_CODE FROM ese_use_est		";
			listQuery += " 		WHERE EST_CODE = '" + model.BaseEstCode + "' AND ESE_CODE = '" + model.BaseEseCode + "'  AND NATION_CODE != '" + model.BaseNationCode + "' ) eue		";
			listQuery += " 		inner join		";
			listQuery += " 		comm_nation cn		";
			listQuery += " 		on eue.NATION_CODE = cn.NATIONNO		";


			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.arraySNation.Add(new schTypeArray { opt_key = listDt.Rows[i]["NATIONNO"].ToString().Trim(), opt_value = listDt.Rows[i][getCol].ToString().Trim() });
				}
			}

			ProdDbModels act = new ProdDbModels();      //DB커넥션 클래스 선언

			model.cateList1 = act.GetCategorySelectBox(1, 0);    //1차 카테고리 가져오기

			return model;
		}


		public StocInOutModels GetStocInOut(StocInOutModels model)
		{

			string lan = "";

			string getNationCol = "";
			switch (lan)    //lan <== 다국어 지원 관련 언어선택 부분이므로 일단 신경쓰지 말고 진행 할것  그냥 돌리면 알아서 NATIONNAME_ko_KR 을 설정함
			{
				case "CN":
					getNationCol = "NATIONNAME_zh_CN";
					break;
				case "EN":
					getNationCol = "NATIONNAME";
					break;
				default:
					getNationCol = "NATIONNAME_ko_KR";
					break;
			}

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();


			string errorStr = "";
			string baseQuery = "";
			
			string listQuery = "SELECT si.EST_CODE, si.BARCODE, si.INOUT_TYPE, si.GOODS_CNT, si.UPDTIME, est.EST_NAME, est." + getNationCol + ", sg.PRODUCT_NAME, sg.BRAND, sg.SKU, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4 ";
			string cntQuery = " SELECT count(*) as cnt ";

			baseQuery += " FROM ( SELECT EST_CODE, BARCODE, INOUT_TYPE, GOODS_CNT, UPDTIME FROM stc_inout ";
			baseQuery += " WHERE ESE_CODE = '" + eseCode + "' ";
			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  EST_CODE = '" + model.schStation.Trim() + "' ";

			if (!String.IsNullOrEmpty(model.schSdt))      //등록일자 (시작일)
				baseQuery += " AND  UPDTIME >= '" + model.schSdt.Trim() + "' ";

			if (!String.IsNullOrEmpty(model.schEdt))      //등록일자 (종료일)
				baseQuery += " AND  UPDTIME <= '" + model.schEdt.Trim() + " 23:59:59' ";



			if (!String.IsNullOrEmpty(model.schStat))      //등록일자 (종료일)
				baseQuery += " AND  INOUT_TYPE = " + model.schStat.Trim() + " ";

			baseQuery += " ) si  ";
			baseQuery += " inner join ";
			baseQuery += " ( SELECT in_est.EST_CODE, in_est.EST_NAME, in_cn." + getNationCol + " FROM esm_station in_est left outer join comm_nation in_cn on in_cn.NATIONNO = in_est.NATION_CODE WHERE 1=1";

			if (!String.IsNullOrEmpty(model.schNation))
				baseQuery += " AND in_est.NATION_CODE = '" + model.schNation.Trim() + "' ";

			baseQuery += " ) est on si.EST_CODE = est.EST_CODE inner join ( SELECT PRODUCT_NAME, BRAND, BARCODE, SKU, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4 FROM stc_goods ";
			baseQuery += " WHERE  ESE_CODE = '" + eseCode + "' ";

			if (model.cate1 != 0)  //카테고리1
				baseQuery += " AND  CATEGORY1 = " + model.cate1;
			if (model.cate2 != 0)  //카테고리2
				baseQuery += " AND  CATEGORY2 = " + model.cate2;
			if (model.cate3 != 0)  //카테고리3
				baseQuery += " AND  CATEGORY3 = " + model.cate3;
			if (model.cate4 != 0)  //카테고리4
				baseQuery += " AND  CATEGORY4 = " + model.cate4;


			baseQuery += " ) sg on si.BARCODE = sg.BARCODE  ";
	
			baseQuery += " WHERE 1=1 ";

			if (!String.IsNullOrEmpty(model.schTxt))    //브랜드
				baseQuery += " AND sg.BRAND like '%" + model.schTxt.Trim() + "%'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt2))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt2.Trim() + "%' ";




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
					StocInOutItem temp = new StocInOutItem();

					temp.SEQNO = i;
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.NATION_NAME = listDt.Rows[i][getNationCol].ToString().Trim();
					temp.EST_NAME = listDt.Rows[i]["EST_NAME"].ToString().Trim();

					temp.CATEGORY1 = int.Parse(listDt.Rows[0]["CATEGORY1"].ToString().Trim());
					temp.CATEGORY2 = int.Parse(listDt.Rows[0]["CATEGORY2"].ToString().Trim());
					temp.CATEGORY3 = int.Parse(listDt.Rows[0]["CATEGORY3"].ToString().Trim());
					temp.CATEGORY4 = int.Parse(listDt.Rows[0]["CATEGORY4"].ToString().Trim());

					temp.BARCODE = listDt.Rows[i]["BARCODE"].ToString().Trim();
					temp.SKU = listDt.Rows[i]["SKU"].ToString().Trim();
					temp.GOODS_NAME = listDt.Rows[i]["PRODUCT_NAME"].ToString().Trim();
					temp.BRAND = listDt.Rows[i]["BRAND"].ToString().Trim();
					temp.STAT = listDt.Rows[i]["INOUT_TYPE"].ToString().Trim();
					temp.CNT = listDt.Rows[i]["GOODS_CNT"].ToString().Trim();
					temp.REG_DT = listDt.Rows[i]["UPDTIME"].ToString().Trim();

					foreach (schTypeArray tempS in model.schTypeArray)
					{
						if (tempS.opt_key == temp.STAT)
						{
							temp.STAT_TXT = tempS.opt_value;
						}
					}

					temp.CATE_NAME = "";
					ProdDbModels prodDb = new ProdDbModels();
					if (temp.CATEGORY1 > 0)
						temp.CATE_NAME += prodDb.GetCategoryName(temp.CATEGORY1);


					if (temp.CATEGORY2 > 0)
						temp.CATE_NAME += " > " + prodDb.GetCategoryName(temp.CATEGORY2);

					if (temp.CATEGORY3 > 0)
						temp.CATE_NAME += " > " + prodDb.GetCategoryName(temp.CATEGORY3);

					if (temp.CATEGORY4 > 0)
						temp.CATE_NAME += " > " + prodDb.GetCategoryName(temp.CATEGORY4);


					model.Items.Add(temp);
				}
			}

			return model;
		}



		// 재고조회 StocProdView


		string[] selectColoum_StcProdView = {
			"EST_CODE",
			"ESE_CODE",
			"CATEGORY1",
			"CATEGORY2",
			"CATEGORY3",
			"CATEGORY4",
			"BARCODE",
			"SKU",
			"PRODUCT_NAME",
			"PRODUCT_NAME_KR",
			"PRODUCT_NAME_CN",
			"PRODUCT_NAME_EN",
			"BRAND",
			"PRICE",
			"UNIT_WEIGHT",
			"WEIGHT_UNIT",
			"STANDARD",
			"EXPIRATION",
			"EXPIRATION_DT",
			"ORIGIN",
			"INGREDIENT",
			"SPEC",
			"SALE_SITE_URL",
			"PRODUCT_IMAGE",
			"COMMENT"
		};


		public StcGoods getStocProdView(StcGoods model)
		{
			string errorStr = "";
			
			string ViewQuery = " SELECT SEQNO, " + string.Join(",", selectColoum_StcProdView) + " FROM stc_goods " ;
			ViewQuery += " WHERE BARCODE = '" + model.BARCODE +"' AND ESE_CODE = '" + model.ESE_CODE + "' ";

			DataTable listDt = getQueryResult(ViewQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
				model.ESE_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
				model.CATEGORY1 = int.Parse(listDt.Rows[0]["CATEGORY1"].ToString().Trim());
				model.CATEGORY2 = int.Parse(listDt.Rows[0]["CATEGORY2"].ToString().Trim());
				model.CATEGORY3 = int.Parse(listDt.Rows[0]["CATEGORY3"].ToString().Trim());
				model.CATEGORY4 = int.Parse(listDt.Rows[0]["CATEGORY4"].ToString().Trim());
				model.BARCODE = listDt.Rows[0]["BARCODE"].ToString().Trim();
				model.SKU = listDt.Rows[0]["SKU"].ToString().Trim();
				model.PRODUCT_NAME = listDt.Rows[0]["PRODUCT_NAME"].ToString().Trim();
				model.PRODUCT_NAME_KR = listDt.Rows[0]["PRODUCT_NAME_KR"].ToString().Trim();
				model.PRODUCT_NAME_CN = listDt.Rows[0]["PRODUCT_NAME_CN"].ToString().Trim();
				model.PRODUCT_NAME_EN = listDt.Rows[0]["PRODUCT_NAME_EN"].ToString().Trim();
				model.BRAND = listDt.Rows[0]["BRAND"].ToString().Trim();
				model.PRICE = double.Parse(listDt.Rows[0]["PRICE"].ToString().Trim());
				model.UNIT_WEIGHT = int.Parse(listDt.Rows[0]["UNIT_WEIGHT"].ToString().Trim());
				model.WEIGHT_UNIT = listDt.Rows[0]["WEIGHT_UNIT"].ToString().Trim();
				model.STANDARD = listDt.Rows[0]["STANDARD"].ToString().Trim();
				model.EXPIRATION = int.Parse(listDt.Rows[0]["EXPIRATION"].ToString().Trim());
				model.EXPIRATION_DT = listDt.Rows[0]["EXPIRATION_DT"].ToString().Trim();
				model.ORIGIN = listDt.Rows[0]["ORIGIN"].ToString().Trim();
				model.INGREDIENT = listDt.Rows[0]["INGREDIENT"].ToString().Trim();
				model.SPEC = listDt.Rows[0]["SPEC"].ToString().Trim();
				model.SALE_SITE_URL = listDt.Rows[0]["SALE_SITE_URL"].ToString().Trim();
				model.PRODUCT_IMAGE = listDt.Rows[0]["PRODUCT_IMAGE"].ToString().Trim();
				model.COMMENT = listDt.Rows[0]["COMMENT"].ToString().Trim();
			}

			model.CATEGORY1_txt = "";
			ProdDbModels prodDb = new ProdDbModels();
			if (model.CATEGORY1 > 0)
				model.CATEGORY1_txt += prodDb.GetCategoryName(model.CATEGORY1);


			if (model.CATEGORY2 > 0)
				model.CATEGORY1_txt += " > " + prodDb.GetCategoryName(model.CATEGORY2);

			if (model.CATEGORY3 > 0)
				model.CATEGORY1_txt += " > " + prodDb.GetCategoryName(model.CATEGORY3);

			if (model.CATEGORY4 > 0)
				model.CATEGORY1_txt += " > " + prodDb.GetCategoryName(model.CATEGORY4);

			return model;
		}

		// 재고조회 StocInOutView

		string[] selectColoum_StcInOutView = {

			"SEQNO",
			"EST_CODE",
			"NATION_NAME",
			"EST_NAME",
			"CATE_NAME",
			"BARCODE",
			"SKU",
			"GOODS_NAME",
			"BRAND",
			"STAT_TXT",
			"STAT",
			"CNT",
			"REG_DT"
		};

		public StocInOutModels StocInOutView(StocInOutModels model)
		{

			string lan = "";

			string getNationCol = "";
			string getCateCol = "";
			switch (lan)    //lan <== 다국어 지원 관련 언어선택 부분이므로 일단 신경쓰지 말고 진행 할것  그냥 돌리면 알아서 NATIONNAME_ko_KR 을 설정함
			{
				case "CN":
					getCateCol = "CATE_NAME_CN";
					getNationCol = "NATIONNAME_zh_CN";
					break;
				case "EN":
					getCateCol = "CATE_NAME_EN";
					getNationCol = "NATIONNAME";
					break;
				default:
					getCateCol = "CATE_NAME_KR";
					getNationCol = "NATIONNAME_ko_KR";
					break;
			}

			//HttpContext context = HttpContext.Current;
			//string eseCode = context.Session["ESE_CODE"].ToString();


			string errorStr = "";
			string baseQuery = "";

			string listQuery = "SELECT si.EST_CODE, si.BARCODE, si.INOUT_TYPE, si.GOODS_CNT, si.UPDTIME, est.EST_NAME, est." + getNationCol + ", sg.PRODUCT_NAME, sg.BRAND, sg.SKU, sg.CATE, sg." + getCateCol + " ";
			string cntQuery = " SELECT count(*) as cnt ";

			baseQuery += " FROM ( SELECT EST_CODE, BARCODE, INOUT_TYPE, GOODS_CNT, UPDTIME FROM stc_inout ";
			baseQuery += " WHERE ESE_CODE = '" + model.BaseEseCode + "' ";

		

			baseQuery += " ) si  ";
			baseQuery += " inner join ";
			baseQuery += " ( SELECT in_est.EST_CODE, in_est.EST_NAME, in_cn." + getNationCol + " FROM esm_station in_est left outer join comm_nation in_cn on in_cn.NATIONNO = in_est.NATION_CODE WHERE 1=1";

			

			baseQuery += " ) est on si.EST_CODE = est.EST_CODE inner join ( SELECT PRODUCT_NAME, BRAND, BARCODE, SKU, CATE, " + getCateCol + " FROM ( SELECT PRODUCT_NAME, BRAND, BARCODE, SKU, IF( CATEGORY4 = 0 , IF( CATEGORY3 = 0 , IF( CATEGORY2 = 0 , CATEGORY1, CATEGORY2), CATEGORY3), CATEGORY4 )as CATE    FROM stc_goods ";
			baseQuery += " WHERE  ESE_CODE = '" + model.BaseEseCode + "' ";
			/*
			if (model.cate1 != 0)  //카테고리1
				baseQuery += " AND  CATEGORY1 = " + model.cate1;
			if (model.cate2 != 0)  //카테고리2
				baseQuery += " AND  CATEGORY2 = " + model.cate2;
			if (model.cate3 != 0)  //카테고리3
				baseQuery += " AND  CATEGORY3 = " + model.cate3;
			if (model.cate4 != 0)  //카테고리4
				baseQuery += " AND  CATEGORY4 = " + model.cate4;
			*/

			baseQuery += " ) in_sg left outer join stc_category in_sc on in_sg.CATE = in_sc.SEQNO ) sg on si.BARCODE = sg.BARCODE  ";

			baseQuery += " WHERE 1=1 ";

			if (!String.IsNullOrEmpty(model.schTxt))    //브랜드
				baseQuery += " AND sg.BRAND like '%" + model.schTxt.Trim() + "%'";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt2))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt2.Trim() + "%' ";




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
					StocInOutItem temp = new StocInOutItem();

					temp.SEQNO = i;
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.NATION_NAME = listDt.Rows[i][getNationCol].ToString().Trim();
					temp.EST_NAME = listDt.Rows[i]["EST_NAME"].ToString().Trim();
					temp.CATE_NAME = listDt.Rows[i][getCateCol].ToString().Trim();
					temp.BARCODE = listDt.Rows[i]["BARCODE"].ToString().Trim();
					temp.SKU = listDt.Rows[i]["SKU"].ToString().Trim();
					temp.GOODS_NAME = listDt.Rows[i]["PRODUCT_NAME"].ToString().Trim();
					temp.BRAND = listDt.Rows[i]["BRAND"].ToString().Trim();
					temp.STAT = listDt.Rows[i]["INOUT_TYPE"].ToString().Trim();
					temp.CNT = listDt.Rows[i]["GOODS_CNT"].ToString().Trim();
					temp.REG_DT = listDt.Rows[i]["UPDTIME"].ToString().Trim();

					foreach (schTypeArray tempS in model.schTypeArray)
					{
						if (tempS.opt_key == temp.STAT)
						{
							temp.STAT_TXT = tempS.opt_value;
						}
					}
					model.Items.Add(temp);
				}
			}

			return model;
		}



		public STOC_EXCEL UploadExcelProd(STOC_EXCEL model)
		{
			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();

			model.result = true;

			string error_str = "";
			GlobalFunction comModel = new GlobalFunction();
			DataTable data = comModel.getUploadExcelData(model.File.FILE, out error_str);

			model.errList = new List<string>();

			if (error_str != "")
			{
				model.errList.Add(error_str);
				model.result = false;
				return model;
			}

			int chkInt = 0;

			//유효성 검사 
			if (data != null && data.Rows.Count != 0)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{
					if (data.Rows[i][0].ToString().Trim() == "")    //	필수	EST CODE
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}

					if (data.Rows[i][1].ToString().Trim() == "" )    //	필수 INT	입고방법 ( 0 : 택배 / 1 : 퀵 / 2 : 셀프 )
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}

					if(int.TryParse(data.Rows[i][1].ToString().Trim(), out chkInt))
					{
						if (chkInt == 0 && data.Rows[i][2].ToString().Trim() == "")    //	0 일때 필수	입고배송번호 ( 택배일 경우 작성 )
						{
							model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
							error_str = "error";
						}

						if (chkInt != 0 && data.Rows[i][3].ToString().Trim() == "")    //	0 아닐때 필수	배송자명 ( 택배가 아닐 경우 작성 )
						{
							model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
							error_str = "error";
						}

						if (chkInt != 0 && data.Rows[i][4].ToString().Trim() == "")    //	0 아닐때 필수	배송자연락처 ( 택배가 아닐 경우 작성 )
						{
							model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
							error_str = "error";
						}
					}
					else // int 형식이 아닐때
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}

					if (data.Rows[i][5].ToString().Trim() == "")    //	필수	발송인 명
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}

					if (data.Rows[i][6].ToString().Trim() == "")    //	필수	발송인 연락처
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}

					if (data.Rows[i][7].ToString().Trim() == "")    //	필수	발송인 주소
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}

					//	data.Rows[i][8].ToString().Trim() 메모 널허용	메모 체크 안함

					if (data.Rows[i][9].ToString().Trim() == "")    //	필수	상품 바코드
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}

					if (data.Rows[i][10].ToString().Trim() == "")    //	필수	박스 번호
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}

					if (!int.TryParse(data.Rows[i][11].ToString().Trim(), out chkInt))    //	필수 INT	수량
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}
					
				}
			}

			if (error_str == "error")
			{
				model.result = false;
				return model;
			}
			
			List<string> exeQueryStr = new List<string>();
			string tmpQuery = "";
			
			if (data != null && data.Rows.Count != 0)
			{
				//실제 EST CODE 구하기
				string estCode = data.Rows[1][0].ToString().Trim(); //이걸로 실제 EST CODE 구해야됨 없는 경우 에러 추가

				tmpQuery = "SELECT EST_CODE FROM esm_station WHERE USERINPUTCODE = '" + estCode + "' ";

				DataTable listDt = getQueryResult(tmpQuery, out error_str);

				if (error_str != "")
				{
					model.result = false;
					model.errList.Add(error_str);
					return model;
				}
				else
				{
					if (listDt != null && listDt.Rows.Count != 0)
					{
						estCode = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					}
					else
					{
						model.result = false;
						model.errList.Add("EST CODE 가 유효하지 않습니다.");
						return model;
					}
						
				}
				
				tmpQuery = "INSERT INTO stc_in_req (EST_CODE, ESE_CODE, INPUT_TYPE, INPUT_DELVNO, INPUT_DELVNAME, INPUT_DELVTELL, SENDER_NAME, SENDER_TEL, SENDER_ADDR, MEMO_ESE ) VALUES ";

				tmpQuery = "( '" + estCode + "', " + "'" + eseCode + "'";
				tmpQuery += ", '" + data.Rows[1][1].ToString().Trim() + "'";    //	입고방법 ( 0 : 택배 / 1 : 퀵 / 2 : 셀프 )	
				tmpQuery += ", '" + data.Rows[1][2].ToString().Trim() + "'";    //	입고배송번호 ( 택배일 경우 작성 )	
				tmpQuery += ", '" + data.Rows[1][3].ToString().Trim() + "'";    //	배송자명 ( 택배가 아닐 경우 작성 )	
				tmpQuery += ", '" + data.Rows[1][4].ToString().Trim() + "'";    //	배송자연락처 ( 택배가 아닐 경우 작성 )	
				tmpQuery += ", '" + data.Rows[1][5].ToString().Trim() + "'";    //	발송인 명	
				tmpQuery += ", '" + data.Rows[1][6].ToString().Trim() + "'";    //	발송인 연락처	
				tmpQuery += ", '" + data.Rows[1][7].ToString().Trim() + "'";    //	발송인 주소	
				tmpQuery += ", '" + data.Rows[1][8].ToString().Trim() + "' )";    //	메모	

				exeQueryStr.Add(tmpQuery);


				for (int i = 0; i < data.Rows.Count; i++)
				{
					tmpQuery = "INSERT INTO stc_in_req_goods (REQ_SEQNO, BARCODE, BOXNUM, CNT) VALUES ";
					tmpQuery = "( (SELECT MAX(SEQNO) FROM stc_in_req) "; 
					tmpQuery += ", '" + data.Rows[i][9].ToString().Trim() + "'";    //	상품 바코드	
					tmpQuery += ", '" + data.Rows[i][10].ToString().Trim() + "'";    //	박스 번호	
					tmpQuery += ", '" + data.Rows[i][11].ToString().Trim() + "'";    //	수량	
					tmpQuery += ")";

					exeQueryStr.Add(tmpQuery);
				}
			}

			
			model.result = exeQuery(exeQueryStr, out error_str);

			return model;
		}

	}
}