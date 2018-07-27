
using comm_dbconn;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_ese.Models;
using web_ese.Models_Act.Comm;
using static web_ese.Models_Act.Comm.HomeModels;

namespace web_ese.Models_Db
{
	public class HomeDbModels : DatabaseConnection
	{
		

		public HomeModels getBase(HomeModels model)
		{
			string lan = "";

			string getTitle = "";

			switch (lan)    //lan <== 다국어 지원 관련 언어선택 부분이므로 일단 신경쓰지 말고 진행 할것  그냥 돌리면 알아서 NATIONNAME_ko_KR 을 설정함
			{
				case "CN":
					getTitle = "TITLE_CN";
					break;
				case "EN":
					getTitle = "TITLE_EN";
					break;
				default:
					getTitle = "TITLE_KR";
					break;
			}



			string errStr = "";
			string exeQuery = "";
			
			HttpContext context = HttpContext.Current;
			string ESE_CODE = context.Session["ESE_CODE"].ToString();
			
			exeQuery = " SELECT SEQNO, " + getTitle + ", REGDATE FROM bd_notice WHERE EST_CODE is Null ORDER BY REGDATE DESC LIMIT 0,5 ";
			
			DataTable listDt = getQueryResult(exeQuery, out errStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					ETM_LST temp = new ETM_LST();

					temp.SEQNO = listDt.Rows[i]["SEQNO"].ToString().Trim();
					temp.TITLE = listDt.Rows[i][getTitle].ToString().Trim();
					temp.GET_DATE = listDt.Rows[i]["REGDATE"].ToString().Trim();

					model.ETM_NOTICE.Add(temp);
				}
			}

			//날짜기준 DESC 최신 5개  LIMT 0,5
			//model.EST_NOTICE = null;
			exeQuery = " SELECT bn.SEQNO, esmU.USERNAME, " + getTitle + ", bn.REGDATE FROM bd_notice bn left outer join esm_user esmU on bn.WRITER_ID = esmU.SEQNO WHERE EST_CODE is Not Null ORDER BY REGDATE DESC LIMIT 0,5 ";

			DataTable listDt2 = getQueryResult(exeQuery, out errStr);

			if (listDt2 != null && listDt2.Rows.Count != 0)
			{
				for (int i = 0; i < listDt2.Rows.Count; i++)
				{
					EST_LST temp = new EST_LST();

					temp.SEQNO = listDt2.Rows[i]["SEQNO"].ToString().Trim();
					temp.TITLE = listDt2.Rows[i][getTitle].ToString().Trim();
					temp.WRITER = listDt2.Rows[i]["USERNAME"].ToString().Trim();
					temp.GET_DATE = listDt2.Rows[i]["REGDATE"].ToString().Trim();

					model.EST_NOTICE.Add(temp);
				}
			}


			//등록 상품 갯수 카운트
			exeQuery = "SELECT COUNT(*) as cnt FROM stc_goods WHERE ESE_CODE = '" + ESE_CODE + "' ";
			model.GOODS_CNT = getQueryCnt(exeQuery, out errStr);

			//보관상품 카운트
			exeQuery = "SELECT( ";
			exeQuery += " IFNULL((SELECT SUM(GOODS_CNT) FROM stc_inout WHERE INOUT_TYPE = 0 AND ESE_CODE = '" + ESE_CODE + "'), 0) ";
			exeQuery += " - IFNULL((SELECT SUM(GOODS_CNT) FROM stc_inout WHERE INOUT_TYPE = 1 AND ESE_CODE = '" + ESE_CODE + "'),0) ";
			exeQuery += " - IFNULL((SELECT SUM(GOODS_CNT) FROM stc_inout WHERE INOUT_TYPE = 2 AND ESE_CODE = '" + ESE_CODE + "'),0) ";
			exeQuery += " + IFNULL((SELECT SUM(GOODS_CNT) FROM stc_inout WHERE INOUT_TYPE = 3 AND ESE_CODE = '" + ESE_CODE + "'),0) ";
			exeQuery += " ) as cnt ";
			model.GODS_STOCK = getQueryCnt(exeQuery, out errStr);
			

			//배송 작업 이후 다시 작업
			//임시 쿼리
			exeQuery = "SELECT 10 as cnt";
			model.ETS_STAT_30 = getQueryCnt(exeQuery, out errStr);
			model.ETS_STAT_100 = getQueryCnt(exeQuery, out errStr);
			model.ETS_STAT_200 = getQueryCnt(exeQuery, out errStr);
			model.ETS_STAT_500 = getQueryCnt(exeQuery, out errStr);
			model.ETS_STAT_700 = getQueryCnt(exeQuery, out errStr);
			//보류 항목 들
			//model.ETS_STAT_10 = getQueryCnt(exeQuery, out errStr);
			//model.ETS_STAT_20 = getQueryCnt(exeQuery, out errStr);
			//model.ETS_STAT_300 = getQueryCnt(exeQuery, out errStr);
			//model.ETS_STAT_400 = getQueryCnt(exeQuery, out errStr);


			exeQuery = "SELECT COUNT(*) as cnt FROM bd_qna WHERE ANSWER_DATE is null AND ESE_CODE = '" + ESE_CODE + "' ";
			model.CS_CNT = getQueryCnt(exeQuery, out errStr);
			
			return model;
		}



	}
}