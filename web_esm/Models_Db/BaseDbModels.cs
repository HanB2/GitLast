using comm_dbconn;
using comm_global;
using comm_model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using web_esm.Models;
using web_esm.Models_Act.Base;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Db
{
	public class BaseDbModels : DatabaseConnection
	{
		//BaseCurrency 통화 관리 =============================================================================
		string[] selectColumn_BaseCurrency = {
			"CURRENCY_UNIT" ,
			"BASIC_UNIT" ,
			"MEMO" 
		};
		
		public BaseCurrencyModels GetBaseCurrencyList()
		{
			string errorStr = "";
			
			string listQuery = " SELECT cc.SEQNO ,cc.CURRENCY_UNIT ,BASIC_UNIT ,MEMO ,AMNT ,DATETIME_UPD FROM conf_currency cc left outer join ";
			listQuery += " (SELECT CURRENCY_UNIT, AMNT, DATETIME_UPD FROM conf_exchange_rate ";
			listQuery += " WHERE SEQNO in (SELECT max(SEQNO) FROM conf_exchange_rate GROUP BY CURRENCY_UNIT )) cer";
			listQuery += " on cc.CURRENCY_UNIT = cer.CURRENCY_UNIT order by cc.SEQNO";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			BaseCurrencyModels model = new BaseCurrencyModels();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					ConfCurrency temp = new ConfCurrency();
					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.CURRENCY_UNIT = listDt.Rows[i]["CURRENCY_UNIT"].ToString().Trim();
					if (listDt.Rows[i]["BASIC_UNIT"].ToString().Trim() != "")
					{
						temp.BASIC_UNIT = Convert.ToDouble(listDt.Rows[i]["BASIC_UNIT"].ToString().Trim());
					}
						
					temp.MEMO = listDt.Rows[i]["MEMO"].ToString().Trim();
					
					if (listDt.Rows[i]["AMNT"].ToString().Trim() != "")
					{
						temp.AMNT = Convert.ToDouble(listDt.Rows[i]["AMNT"].ToString().Trim());
					}
					temp.DATETIME_UPD = listDt.Rows[i]["DATETIME_UPD"].ToString().Trim();
					model.Items.Add(temp);
				}
			}

			return model;
		}


		public ConfCurrency GetBaseCurrencyView(int SEQNO)
		{
			string errorStr = "";
			
			string listQuery = " SELECT cc.SEQNO ,cc.CURRENCY_UNIT ,BASIC_UNIT ,MEMO ,AMNT ,DATETIME_UPD FROM conf_currency cc left outer join ";
			listQuery += " (SELECT CURRENCY_UNIT, AMNT, DATETIME_UPD FROM conf_exchange_rate ";
			listQuery += " WHERE SEQNO in (SELECT max(SEQNO) FROM conf_exchange_rate GROUP BY CURRENCY_UNIT )) cer";
			listQuery += " on cc.CURRENCY_UNIT = cer.CURRENCY_UNIT  WHERE SEQNO = " + SEQNO;

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			ConfCurrency model = new ConfCurrency();
			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.CURRENCY_UNIT = listDt.Rows[0]["CURRENCY_UNIT"].ToString().Trim();
				if (listDt.Rows[0]["BASIC_UNIT"].ToString().Trim() != "")
				{
					model.BASIC_UNIT = Convert.ToDouble(listDt.Rows[0]["BASIC_UNIT"].ToString().Trim());
				}

				if (listDt.Rows[0]["AMNT"].ToString().Trim() != "")
				{
					model.AMNT = Convert.ToDouble(listDt.Rows[0]["AMNT"].ToString().Trim());
				}
				model.DATETIME_UPD = listDt.Rows[0]["DATETIME_UPD"].ToString().Trim();
				model.MEMO = listDt.Rows[0]["MEMO"].ToString().Trim();
			}
			return model;
		}


		public string setBaseCurrency(BaseCurrencyModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";
			List<string> exeQueryList = new List<string>();

			if (model.act_type != null && model.act_type == "ins")
			{
				
				exeQueryStr = " INSERT INTO conf_currency ( CURRENCY_UNIT, BASIC_UNIT, MEMO )VALUES(  ";
				exeQueryStr += "'" + model.Item.CURRENCY_UNIT + "'";
				exeQueryStr += ", " + model.Item.BASIC_UNIT + "";
				exeQueryStr += ", '" + model.Item.MEMO + "'";
				exeQueryStr += " ) ";

				exeQueryList.Add(exeQueryStr);
				
				exeQueryStr = " INSERT INTO conf_exchange_rate ( CURRENCY_UNIT, AMNT)VALUES(  ";
				exeQueryStr += "'" + model.Item.CURRENCY_UNIT + "'";
				exeQueryStr += ", " + model.Item.AMNT + "";
				exeQueryStr += " ) ";

				exeQueryList.Add(exeQueryStr);
			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				exeQueryStr = " UPDATE conf_currency SET ";
				exeQueryStr += " CURRENCY_UNIT = '" + model.Item.CURRENCY_UNIT + "'";
				exeQueryStr += ", BASIC_UNIT =  '" + model.Item.BASIC_UNIT + "'";
				exeQueryStr += ", MEMO =  '" + model.Item.MEMO + "'";
				exeQueryStr += " WHERE SEQNO = " + model.Item.SEQNO;
				
				exeQueryList.Add(exeQueryStr);
				
				exeQueryStr = " INSERT INTO conf_exchange_rate ( CURRENCY_UNIT, AMNT)VALUES(  ";
				exeQueryStr += "'" + model.Item.CURRENCY_UNIT + "'";
				exeQueryStr += ", " + model.Item.AMNT + "";
				exeQueryStr += " ) ";

				exeQueryList.Add(exeQueryStr);
			}
			else
			{
				result = "잘못된 접근입니다.";
				return result;
			}



			if (exeQuery(exeQueryList, out errorStr))
			{
				result = "성공.";

			}//(msh)else 쿼리 추가
			else
			{
				result = "실패.";
			}
			

			return result;

		}


		public string delBaseCurrency(int seq)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM conf_currency WHERE SEQNO = " + seq;

			result = "실패.";
			if (exeQuery(exeQueryStr, out errorStr))
			{
				result = "성공.";
			}

			return result;
		}

		public string setAMNTList(BaseCurrencyModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";
			List<string> exeQueryList = new List<string>();

			foreach(var item in model.Items)
			{
				exeQueryStr = " INSERT INTO conf_exchange_rate ( CURRENCY_UNIT, AMNT)VALUES(  ";
				exeQueryStr += "'" + item.CURRENCY_UNIT + "'";
				exeQueryStr += ", " + item.AMNT + "";
				exeQueryStr += " ) ";

				exeQueryList.Add(exeQueryStr);
			}
			
			result = "실패.";
			if (exeQuery(exeQueryList, out errorStr))
			{
				result = "성공.";
			}

			return result;
		}


		//BaseNation 배송가능 국가 관리 =============================================================================
		string[] column_BaseNation = {
			"NATION_CODE" ,
			"NATION_NAME" ,
			"WEIGHT_UNIT" ,
			"CURRENCY_UNIT",
			"USE_YN"
		};

		public BaseNationModels GetBaseNationList()
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", column_BaseNation) + " FROM conf_shipping_country  ORDER BY SEQNO ";
	
			DataTable listDt = getQueryResult(listQuery, out errorStr);

			BaseNationModels model = new BaseNationModels();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					ConfShippingCountry temp = new ConfShippingCountry();
					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();
					temp.NATION_NAME = listDt.Rows[i]["NATION_NAME"].ToString().Trim();
					temp.WEIGHT_UNIT = listDt.Rows[i]["WEIGHT_UNIT"].ToString().Trim();
					temp.CURRENCY_UNIT = listDt.Rows[i]["CURRENCY_UNIT"].ToString().Trim();
					temp.USE_YN = int.Parse(listDt.Rows[i]["USE_YN"].ToString().Trim());
					model.Items.Add(temp);
				}
			}

			return model;
		}


		public ConfShippingCountry GetBaseNationView(int SEQNO)
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", column_BaseNation) + " FROM conf_shipping_country  WHERE SEQNO = " + SEQNO;

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			ConfShippingCountry model = new ConfShippingCountry();
			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.SEQNO			= int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.NATION_CODE	= listDt.Rows[0]["NATION_CODE"].ToString().Trim();
				model.NATION_NAME	= listDt.Rows[0]["NATION_NAME"].ToString().Trim();
				model.WEIGHT_UNIT	= listDt.Rows[0]["WEIGHT_UNIT"].ToString().Trim();
				model.CURRENCY_UNIT = listDt.Rows[0]["CURRENCY_UNIT"].ToString().Trim();
				model.USE_YN		= int.Parse(listDt.Rows[0]["USE_YN"].ToString().Trim());
			}
			
			return model;
		}


		public string setBaseNation(BaseNationModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";

			if (model.act_type != null && model.act_type == "ins")
			{
				exeQueryStr = " INSERT INTO conf_shipping_country (" + string.Join(",", column_BaseNation) + " )VALUES(  ";
				exeQueryStr += " '" + model.Item.NATION_CODE + "'";
				exeQueryStr += ", '" + model.Item.NATION_NAME + "'";
				exeQueryStr += ", '" + model.Item.WEIGHT_UNIT + "'";
				exeQueryStr += ", '" + model.Item.CURRENCY_UNIT + "'";
				exeQueryStr += ", " + model.Item.USE_YN + "";
				exeQueryStr += " ) ";
			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				exeQueryStr = " UPDATE conf_shipping_country SET ";
				exeQueryStr += " NATION_CODE = '" + model.Item.NATION_CODE + "'";
				exeQueryStr += ", NATION_NAME =  '" + model.Item.NATION_NAME + "'";
				exeQueryStr += ", WEIGHT_UNIT =  '" + model.Item.WEIGHT_UNIT + "'";
				exeQueryStr += ", CURRENCY_UNIT =  '" + model.Item.CURRENCY_UNIT + "'";
				exeQueryStr += ", USE_YN =   " + model.Item.USE_YN + "";
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


		public string delBaseNation(int seq)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM conf_shipping_country WHERE SEQNO = " + seq;
			//(msh)주석처리 string exeQueryStr = " UPDATE conf_shipping_country SET USE_YN = 0 WHERE SEQNO = " + seq;

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


		//BaseNation 공항 관리 =============================================================================
		string[] column_BaseAirport = {
			"NATION_CODE" ,
			"AIRPORT_CODE" ,
			"AIRPORT_NAME" ,
			"AIRPORT_LOCATION"
		};
		
		public BaseAirportModels GetBaseAirportList(BaseAirportModels model)
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", column_BaseAirport) + " FROM conf_airport WHERE 1=1 ";

			if (!String.IsNullOrEmpty(model.schType))
			{
				listQuery += " AND NATION_CODE = '" + model.schType + "' "; 
			}

			listQuery += " ORDER BY SEQNO ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					ConfAirpot temp = new ConfAirpot();
					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();
					temp.AIRPORT_CODE = listDt.Rows[i]["AIRPORT_CODE"].ToString().Trim();
					temp.AIRPORT_NAME = listDt.Rows[i]["AIRPORT_NAME"].ToString().Trim();
					temp.AIRPORT_LOCATION = listDt.Rows[i]["AIRPORT_LOCATION"].ToString().Trim();
					model.Items.Add(temp);
				}
			}

			return model;
		}


		public ConfAirpot GetBaseAirportView(int SEQNO)
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", column_BaseAirport) + " FROM conf_airport  WHERE SEQNO = " + SEQNO;

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			ConfAirpot model = new ConfAirpot();
			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.NATION_CODE = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
				model.AIRPORT_CODE = listDt.Rows[0]["AIRPORT_CODE"].ToString().Trim();
				model.AIRPORT_NAME = listDt.Rows[0]["AIRPORT_NAME"].ToString().Trim();
				model.AIRPORT_LOCATION = listDt.Rows[0]["AIRPORT_LOCATION"].ToString().Trim();
			}

			return model;
		}


		public string setBaseAirport(BaseAirportModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";

			if (model.act_type != null && model.act_type == "ins")
			{
				exeQueryStr = " INSERT INTO conf_airport (" + string.Join(",", column_BaseAirport) + " )VALUES(  ";
				exeQueryStr += " '" + model.Item.NATION_CODE + "'";
				exeQueryStr += ", '" + model.Item.AIRPORT_CODE + "'";
				exeQueryStr += ", '" + model.Item.AIRPORT_NAME + "'";
				exeQueryStr += ", '" + model.Item.AIRPORT_LOCATION + "'";
				exeQueryStr += " ) ";
			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				exeQueryStr = " UPDATE conf_airport SET ";
				exeQueryStr += " NATION_CODE = '" + model.Item.NATION_CODE + "'";
				exeQueryStr += ", AIRPORT_CODE =  '" + model.Item.AIRPORT_CODE + "'";
				exeQueryStr += ", AIRPORT_NAME =  '" + model.Item.AIRPORT_NAME + "'";
				exeQueryStr += ", AIRPORT_LOCATION =  '" + model.Item.AIRPORT_LOCATION + "'";
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


		public string delBaseAirport(int seq)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM conf_airport WHERE SEQNO = " + seq;

			
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




		//BaseLocal 현지 배송업체 설정 =============================================================================
		string[] column_BaseLocal = {
			"NATION_CODE" ,
			"NAME" ,
			"HOMEPAGE" ,
			"COM_ID" ,
			"MEMO"
		};

		public BaseLocalModels GetBaseLocalList(BaseLocalModels model)
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", column_BaseLocal) + " FROM conf_local_delivery  WHERE 1=1 ";

			if (!String.IsNullOrEmpty(model.schType))
			{
				listQuery += "AND NATION_CODE = '" + model.schType + "'";
			}

			listQuery += " ORDER BY SEQNO ";


			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					ConfLocalDelivery temp = new ConfLocalDelivery();
					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();
					temp.NAME = listDt.Rows[i]["NAME"].ToString().Trim();
					temp.HOMEPAGE = listDt.Rows[i]["HOMEPAGE"].ToString().Trim();
					temp.COM_ID = listDt.Rows[i]["COM_ID"].ToString().Trim();
					temp.MEMO = listDt.Rows[i]["MEMO"].ToString().Trim();
					model.Items.Add(temp);
				}
			}

			return model;
		}


		public ConfLocalDelivery GetBaseLocalView(int SEQNO)
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", column_BaseLocal) + " FROM conf_local_delivery  WHERE SEQNO = " + SEQNO;

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			ConfLocalDelivery model = new ConfLocalDelivery();
			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.NATION_CODE = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
				model.NAME = listDt.Rows[0]["NAME"].ToString().Trim();
				model.HOMEPAGE = listDt.Rows[0]["HOMEPAGE"].ToString().Trim();
				model.COM_ID = listDt.Rows[0]["COM_ID"].ToString().Trim();
				model.MEMO = listDt.Rows[0]["MEMO"].ToString().Trim();
			}

			return model;
		}


		public string setBaseLocal(BaseLocalModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";

			if (model.act_type != null && model.act_type == "ins")
			{
				exeQueryStr = " INSERT INTO conf_local_delivery (" + string.Join(",", column_BaseLocal) + " )VALUES(  ";
				exeQueryStr += " '" + model.Item.NATION_CODE + "'";
				exeQueryStr += ", '" + model.Item.NAME + "'";
				exeQueryStr += ", '" + model.Item.HOMEPAGE + "'";
				exeQueryStr += ", '" + model.Item.COM_ID + "'";
				exeQueryStr += ", '" + model.Item.MEMO + "'";
				exeQueryStr += " ) ";
			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				exeQueryStr = " UPDATE conf_local_delivery SET ";
				exeQueryStr += " NATION_CODE = '" + model.Item.NATION_CODE + "'";
				exeQueryStr += ", NAME =  '" + model.Item.NAME + "'";
				exeQueryStr += ", HOMEPAGE =  '" + model.Item.HOMEPAGE + "'";
				exeQueryStr += ", COM_ID =  '" + model.Item.COM_ID + "'";
				exeQueryStr += ", MEMO =  '" + model.Item.MEMO + "'";
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


		public string delBaseLocal(int seq)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM conf_local_delivery WHERE SEQNO = " + seq;


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



		//BaseOutPutType 출고 타입 설정 =============================================================================
		string[] column_BaseOutPutType = {
			"NATION_CODE" ,
			"RELEASE_NAME" ,
			"RELEASE_CODE" ,
			"MEMO" ,
			"DELV_CODE"
		};

		public BaseOutPutTypeModels GetBaseOutPutTypeList(BaseOutPutTypeModels model)
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", column_BaseOutPutType) + " FROM conf_release_type  WHERE 1=1 ";

			if (!String.IsNullOrEmpty(model.schType))
			{
				listQuery += " AND NATION_CODE = '" + model.schType + "' ";
			}
			listQuery += " ORDER BY SEQNO ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);		

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					ConfReleaseType temp = new ConfReleaseType();
					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();
					temp.RELEASE_NAME = listDt.Rows[i]["RELEASE_NAME"].ToString().Trim();
					temp.RELEASE_CODE = listDt.Rows[i]["RELEASE_CODE"].ToString().Trim();
					temp.MEMO = listDt.Rows[i]["MEMO"].ToString().Trim();
					temp.DELV_CODE = listDt.Rows[i]["DELV_CODE"].ToString().Trim();
					model.Items.Add(temp);
				}
			}

			return model;
		}


		public ConfReleaseType GetBaseOutPutTypeView(int SEQNO)
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", column_BaseOutPutType) + " FROM conf_release_type  WHERE SEQNO = " + SEQNO;

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			ConfReleaseType model = new ConfReleaseType();
			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.NATION_CODE = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
				model.RELEASE_NAME = listDt.Rows[0]["RELEASE_NAME"].ToString().Trim();
				model.RELEASE_CODE = listDt.Rows[0]["RELEASE_CODE"].ToString().Trim();
				model.MEMO = listDt.Rows[0]["MEMO"].ToString().Trim();
				model.DELV_CODE = listDt.Rows[0]["DELV_CODE"].ToString().Trim();
			}

			return model;
		}


		public string setBaseOutPutType(BaseOutPutTypeModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";

			if (model.act_type != null && model.act_type == "ins")
			{
				exeQueryStr = " INSERT INTO conf_release_type (" + string.Join(",", column_BaseOutPutType) + " )VALUES(  ";
				exeQueryStr += " '" + model.Item.NATION_CODE + "'";
				exeQueryStr += ", '" + model.Item.RELEASE_NAME + "'";
				exeQueryStr += ", '" + model.Item.RELEASE_CODE + "'";
				exeQueryStr += ", '" + model.Item.MEMO + "'";
				exeQueryStr += ", '" + model.Item.DELV_CODE + "'";
				exeQueryStr += " ) ";
			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				exeQueryStr = " UPDATE conf_release_type SET ";
				exeQueryStr += " NATION_CODE = '" + model.Item.NATION_CODE + "'";
				exeQueryStr += ", RELEASE_NAME =  '" + model.Item.RELEASE_NAME + "'";
				exeQueryStr += ", RELEASE_CODE =  '" + model.Item.RELEASE_CODE + "'";
				exeQueryStr += ", MEMO =  '" + model.Item.MEMO + "'";
				exeQueryStr += ", DELV_CODE =  '" + model.Item.DELV_CODE + "'";
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


		public string delBaseOutPutType(int seq)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM conf_release_type WHERE SEQNO = " + seq;


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



		public BaseOutPutRateModels GetBaseOutPutRateView(BaseOutPutRateModels model)
		{
			string errorStr = "";

			string listQuery = " SELECT SEQNO , " + string.Join(",", column_BaseOutPutType) + " FROM conf_release_type  WHERE SEQNO = " + model.act_key;

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.Item.SEQNO = int.Parse(listDt.Rows[0]["SEQNO"].ToString().Trim());
				model.Item.NATION_CODE = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
				model.Item.RELEASE_NAME = listDt.Rows[0]["RELEASE_NAME"].ToString().Trim();
				model.Item.RELEASE_CODE = listDt.Rows[0]["RELEASE_CODE"].ToString().Trim();
				model.Item.MEMO = listDt.Rows[0]["MEMO"].ToString().Trim();
				model.Item.DELV_CODE = listDt.Rows[0]["DELV_CODE"].ToString().Trim();
			}
			
			listQuery = " SELECT WEIGHT , CUSTOMS_FEE FROM conf_customs_fee  WHERE EST_CODE = '00000' AND NATION_CODE = '" + model.Item.NATION_CODE + "' AND RELEASE_CODE = '" + model.Item.RELEASE_CODE + "' order by WEIGHT ";

			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					ConfCustomsFee temp = new ConfCustomsFee();
					
					if (listDt.Rows[i]["WEIGHT"].ToString().Trim() != "")
					{
						temp.WEIGHT = Convert.ToDouble(listDt.Rows[i]["WEIGHT"].ToString().Trim());
					}

					if (listDt.Rows[i]["CUSTOMS_FEE"].ToString().Trim() != "")
					{
						temp.CUSTOMS_FEE = Convert.ToDouble(listDt.Rows[i]["CUSTOMS_FEE"].ToString().Trim());
					}
					model.Items.Add(temp);
				}
			}


			return model;
		}


		//요율표 저장
		public string setBaseOutPutRate(BaseOutPutRateModels model, out string error_str)
		{
			error_str = "";
			GlobalFunction comModel = new GlobalFunction();
			DataTable data = comModel.getUploadExcelData(model.File.FILE, out error_str);
			
			if(error_str != "")
				return error_str;


			//유효성 검사 
			double test = 0;
			if (data != null && data.Rows.Count != 0)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{
					try
					{
						test = Convert.ToDouble(data.Rows[i][0].ToString().Trim());
						test = Convert.ToDouble(data.Rows[i][1].ToString().Trim());
					}
					catch
					{
						error_str = "[" + i + "] 행에 문제가 있습니다.";
						return error_str;
					}
					
				}
			}


			List<string> exeQueryList = new List<string>();
			List<string> linqList = new List<string>();
			
			string exeQueryStr = " INSERT INTO conf_customs_fee (EST_CODE, NATION_CODE, RELEASE_CODE, WEIGHT, CUSTOMS_FEE ) VALUES ";

			if (data != null && data.Rows.Count != 0)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{
					linqList.Add("( '00000', '" + model.Item.NATION_CODE + "', '" + model.Item.RELEASE_CODE + "',  " + data.Rows[i][0].ToString().Trim() + " ,  " + data.Rows[i][1].ToString().Trim() + "  )");
				}
			}
			
			exeQueryStr += string.Join(",", linqList);
			exeQueryList.Add("DELETE FROM conf_customs_fee WHERE EST_CODE = '00000' AND NATION_CODE = '" + model.Item.NATION_CODE + "' AND RELEASE_CODE = '" + model.Item.RELEASE_CODE + "'");
			exeQueryList.Add(exeQueryStr);
			exeQuery(exeQueryList, out error_str);

			return error_str;
		}



	}
}
