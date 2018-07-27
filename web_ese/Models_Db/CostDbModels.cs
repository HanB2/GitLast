using comm_dbconn;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Cost;

namespace web_ese.Models_Db
{
	public class CostDbModels : DatabaseConnection
	{
		
		//EST_CODE 콤보박스 
		public List<schTypeArray> GetEstCodeSelectBox()
		{

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();

			List<schTypeArray> model = new List<schTypeArray>();

			string errorStr = "";
			

			//ESE가 속해있는 스테이션 정보 가져오기
			string listQuery = " SELECT eu.EST_CODE, es.EST_NAME FROM ese_user eu left outer join esm_station es on eu.EST_CODE = es.EST_CODE WHERE eu.GROUP_ID = 0 AND eu.ESE_CODE =  '" + eseCode + "'  ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["EST_NAME"].ToString().Trim(), opt_key = listDt.Rows[i]["EST_CODE"].ToString().Trim() });
				}
			}


			//ESE가 추가로 사용 가능한 스테이션 정보 가져오기
			listQuery = " SELECT eue.USE_EST, es.EST_NAME FROM ese_use_est eue left outer join esm_station es on eue.USE_EST = es.EST_CODE WHERE eue.ESE_CODE = '" + eseCode + "' ";

			listDt = getQueryResult(listQuery, out errorStr);
			
			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["EST_NAME"].ToString().Trim(), opt_key = listDt.Rows[i]["USE_EST"].ToString().Trim() });
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

		

		
	}
}