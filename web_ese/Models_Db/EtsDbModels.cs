using comm_dbconn;
using comm_model;
using comm_delvapi;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;
using web_ese.Models_Act.Ets;
using static web_ese.Models_Act.Ets.EtsLabelModels;
using static web_ese.Models_Act.Ets.EtsListModels;
using comm_delvapi.Class;

namespace web_ese.Models_Db
{
	public class EtsDbModels : DatabaseConnection
	{

		//배송 신청 수정 시 정보 가져오기
		public EtsReqModels EtsReqGetInfo(EtsReqModels model)
		{
			string exeQueryStr = "";
			string errorStr = "";

			exeQueryStr = " SELECT ";
			exeQueryStr += " ESE_CODE, WAYBILLNO, DELVNO, DELV_COM, ORDERTYPE, ";   //기본 정보
			exeQueryStr += " DEP_NATION_CODE, EST_CODE, ORDERNO1, SENDER_NAME, SENDER_TELNO, SENDER_ADDR, ";    //발송인 정보
			exeQueryStr += " RECEIVER_NAME, RECEIVER_TELNO, RECEIVER_CPHONENO, SOCIALNO_BIZNO, RECEIVER_EMAIL, NATION_CODE, RECEIVERTYPE, CLEAR_PDT_CODE, ";    //수취인 정보1
			exeQueryStr += " RECEIVER_STATE, RECEIVER_CITY, RECEIVER_DISTRICT, RECEIVER_ZIPCODE, RECEIVER_ADDR1, RECEIVER_ADDR2, "; //수취인 정보2
			exeQueryStr += " DIM_WIDTH, DIM_LENGTH, DIM_HEIGHT, REALVOLUME, WEIGHT_UNIT, REALWEIGHT, CHARGEABLE_WEIGHT, DELV_CODE, QTY_BOX, USER1, USER2, USER3,MESSAGE_DELV "; //기타 정보
			exeQueryStr += " FROM ord_master";
			exeQueryStr += " WHERE 1=1";
			exeQueryStr += " AND WAYBILLNO = '" + model.Item.WAYBILLNO + "' ";		//액트키를 이용하던 Item 에 받아와서 쓰던 마음대로
			exeQueryStr += " AND ESE_CODE = '" + model.Item.ESE_CODE + "' ";		//세션값으로 바꿀것
			
			DataTable listDt = getQueryResult(exeQueryStr, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.Item.ESE_CODE = listDt.Rows[0]["ESE_CODE"].ToString().Trim();
				model.Item.WAYBILLNO = listDt.Rows[0]["WAYBILLNO"].ToString().Trim();
				model.Item.DELVNO = listDt.Rows[0]["DELVNO"].ToString().Trim();
				model.Item.DELV_COM = listDt.Rows[0]["DELV_COM"].ToString().Trim();
				model.Item.ORDERTYPE = int.Parse(listDt.Rows[0]["ORDERTYPE"].ToString().Trim());

				model.Item.DEP_NATION_CODE = listDt.Rows[0]["DEP_NATION_CODE"].ToString().Trim();
				model.Item.EST_CODE = listDt.Rows[0]["EST_CODE"].ToString().Trim();
				model.Item.ORDERNO1 = listDt.Rows[0]["ORDERNO1"].ToString().Trim();
				model.Item.SENDER_NAME = listDt.Rows[0]["SENDER_NAME"].ToString().Trim();
				model.Item.SENDER_TELNO = listDt.Rows[0]["SENDER_TELNO"].ToString().Trim();
				model.Item.SENDER_ADDR = listDt.Rows[0]["SENDER_ADDR"].ToString().Trim();
				
				model.Item.RECEIVER_NAME = listDt.Rows[0]["RECEIVER_NAME"].ToString().Trim();
				model.Item.RECEIVER_TELNO = listDt.Rows[0]["RECEIVER_TELNO"].ToString().Trim();
				model.Item.RECEIVER_CPHONENO = listDt.Rows[0]["RECEIVER_CPHONENO"].ToString().Trim();
				model.Item.SOCIALNO_BIZNO = listDt.Rows[0]["SOCIALNO_BIZNO"].ToString().Trim();
				model.Item.RECEIVER_EMAIL = listDt.Rows[0]["RECEIVER_EMAIL"].ToString().Trim();
				model.Item.NATION_CODE = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
				model.Item.RECEIVERTYPE = listDt.Rows[0]["RECEIVERTYPE"].ToString().Trim();
				model.Item.CLEAR_PDT_CODE = int.Parse(listDt.Rows[0]["CLEAR_PDT_CODE"].ToString().Trim());
				
				model.Item.RECEIVER_STATE = listDt.Rows[0]["RECEIVER_STATE"].ToString().Trim();
				model.Item.RECEIVER_CITY = listDt.Rows[0]["RECEIVER_CITY"].ToString().Trim();
				model.Item.RECEIVER_DISTRICT = listDt.Rows[0]["RECEIVER_DISTRICT"].ToString().Trim();
				model.Item.RECEIVER_ZIPCODE = listDt.Rows[0]["RECEIVER_ZIPCODE"].ToString().Trim();
				model.Item.RECEIVER_ADDR1 = listDt.Rows[0]["RECEIVER_ADDR1"].ToString().Trim();
				model.Item.RECEIVER_ADDR2 = listDt.Rows[0]["RECEIVER_ADDR2"].ToString().Trim();
				
				model.Item.DIM_WIDTH = double.Parse(listDt.Rows[0]["DIM_WIDTH"].ToString().Trim());
				model.Item.DIM_LENGTH = double.Parse(listDt.Rows[0]["DIM_LENGTH"].ToString().Trim());
				model.Item.REALVOLUME = double.Parse(listDt.Rows[0]["REALVOLUME"].ToString().Trim());
				model.Item.DIM_HEIGHT = double.Parse(listDt.Rows[0]["DIM_HEIGHT"].ToString().Trim());
				model.Item.WEIGHT_UNIT = listDt.Rows[0]["WEIGHT_UNIT"].ToString().Trim();
				model.Item.REALWEIGHT = double.Parse(listDt.Rows[0]["REALWEIGHT"].ToString().Trim());
				model.Item.CHARGEABLE_WEIGHT = double.Parse(listDt.Rows[0]["CHARGEABLE_WEIGHT"].ToString().Trim());
				model.Item.DELV_CODE = listDt.Rows[0]["DELV_CODE"].ToString().Trim();
				model.Item.QTY_BOX = int.Parse(listDt.Rows[0]["QTY_BOX"].ToString().Trim());
				model.Item.USER1 = listDt.Rows[0]["USER1"].ToString().Trim();
				model.Item.USER2 = listDt.Rows[0]["USER2"].ToString().Trim();
				model.Item.USER3 = listDt.Rows[0]["USER3"].ToString().Trim();
				model.Item.MESSAGE_DELV = listDt.Rows[0]["MESSAGE_DELV"].ToString().Trim();

			}
			
			exeQueryStr = " SELECT GOODS_NAME, BRAND, PRICE, QTY, HSCODE, PURCHASE_URL, BARCODE ";
			exeQueryStr += " FROM ord_goods ";
			exeQueryStr += " WHERE 1=1";
			exeQueryStr += " AND WAYBILLNO = '" + model.Item.WAYBILLNO + "' ";
			exeQueryStr += " AND EST_CODE = '" + model.Item.EST_CODE + "' ";
			exeQueryStr += " AND ESE_CODE = '" + model.Item.ESE_CODE + "' ";
			exeQueryStr += " OREDER BY ITEMSN ";
			
			listDt = getQueryResult(exeQueryStr, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					OrdGoods temp = new OrdGoods();

					temp.GOODS_NAME = listDt.Rows[i]["GOODS_NAME"].ToString().Trim();
					temp.BRAND = listDt.Rows[i]["BRAND"].ToString().Trim();
					temp.PRICE = double.Parse(listDt.Rows[i]["PRICE"].ToString().Trim());
					temp.QTY = int.Parse(listDt.Rows[i]["QTY"].ToString().Trim());
					temp.HSCODE = listDt.Rows[i]["HSCODE"].ToString().Trim();
					temp.PURCHASE_URL = listDt.Rows[i]["PURCHASE_URL"].ToString().Trim();
					temp.BARCODE = listDt.Rows[i]["BARCODE"].ToString().Trim();

					
					model.Items.Add(temp);
				}
			}

			return model;
		}


		//배송 신청 등록 시 기본정보 세팅
		public EtsReqModels EtsReqSetBase(EtsReqModels model)
		{
			if (model.act_type != "updt")
				model = new EtsReqModels();

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

			//소속 스테이션 정보 가져오기 
			listQuery = " SELECT ";
			listQuery += " est.NATION_CODE ";
			listQuery += " ,est.EST_CODE ";
			listQuery += " ,ese.ESE_CODE ";
			listQuery += " ,est.EST_NAME ";
			listQuery += " ,est.WEIGHT_UNIT ";
			listQuery += " ,cn."+ getCol;
			listQuery += " FROM ";
			listQuery += " esm_station est ";
			listQuery += " inner join	";
			listQuery += " (SELECT EST_CODE, ESE_CODE FROM ese_user WHERE ESE_CODE = '" + eseCode + "' ) ese ";
			listQuery += " on est.EST_CODE = ese.EST_CODE ";
			listQuery += " inner join ";
			listQuery += " comm_nation cn	";
			listQuery += " on est.NATION_CODE = cn.NATIONNO	";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.BaseEstCode = listDt.Rows[0]["EST_CODE"].ToString().Trim();
					model.BaseEstName = listDt.Rows[0]["EST_NAME"].ToString().Trim();
					model.BaseEseCode = eseCode;
					model.BaseNationCode = listDt.Rows[0]["NATION_CODE"].ToString().Trim();
					model.BaseNationName = listDt.Rows[0][getCol].ToString().Trim();
					model.BaseReleaseWeightUnit = listDt.Rows[0]["WEIGHT_UNIT"].ToString().Trim();
				}
			}

			model.BaseWeightDivide_KG = "6000";
			model.BaseWeightDivide_LB = "166";

			/* 추후 스테이션별 부피 계산값 변경 기능 사용 시 수정 하여 적용
			listQuery = " SELECT SET_VALUE FROM est_settings WHERE SET_KEY = 'vol_weight_calc_divide' AND EST_CODE = '" + model.BaseEstCode + "'  ";
			
			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.BaseWeightDivide_KG = listDt.Rows[0]["SET_VALUE"].ToString().Trim();
					model.BaseWeightDivide_LB = listDt.Rows[0]["SET_VALUE"].ToString().Trim();
				}
			}
			*/



			//기본 인치 세팅 무게가 KG 일 경우 cm 
			model.BaseReleaseDimUnit = "inch";
			if(model.BaseReleaseWeightUnit == "KG")
				model.BaseReleaseDimUnit = "cm";



			//기본정보 세팅
			model.arraySNation.Add(new schTypeArray {  opt_key = model.BaseNationCode , opt_value = model.BaseNationName });
			model.arrayStation.Add(new schTypeArray { opt_key = model.BaseEstCode, opt_value = model.BaseEstName });

			//사용 나라 목록 가져오기
			listQuery = "	SELECT		";
			listQuery += " 		cn.NATIONNO		";
			listQuery += " 		,cn."+getCol+"		";
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

			//배송가능 나라 목록 가져오기
			listQuery = "	SELECT		";
			listQuery += " 		cn.NATIONNO		";
			listQuery += " 		,cn." + getCol + "		";
			listQuery += " 	FROM		";
			listQuery += " 		conf_shipping_country csc		";
			listQuery += " 		inner join		";
			listQuery += " 		comm_nation cn		";
			listQuery += " 		on csc.NATION_CODE = cn.NATIONNO		";


			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.arrayENation.Add(new schTypeArray { opt_key = listDt.Rows[i]["NATIONNO"].ToString().Trim(), opt_value = listDt.Rows[i][getCol].ToString().Trim() });
				}
			}

			//통화 목록 가져오기
			listQuery = " SELECT CURRENCY_UNIT FROM conf_currency";
			
			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.arrayCurrencyGoods.Add(new schTypeArray { opt_key = listDt.Rows[i]["CURRENCY_UNIT"].ToString().Trim(), opt_value = listDt.Rows[i]["CURRENCY_UNIT"].ToString().Trim() });
				}
			}
			
			model.BaseReleaseCode = "";
			model.BaseReleaseCost = "";

			//EstSender model = new EstSender();
			listQuery = " SELECT ESE_NAME, ZIPCODE, ADDR, TELNO_TASK   FROM est_sender WHERE ESE_CODE = '" + eseCode + "' ";

			listDt = getQueryResult(listQuery, out errorStr);
			
			if (listDt != null && listDt.Rows.Count != 0)
			{
				
				model.BaseEseName = listDt.Rows[0]["ESE_NAME"].ToString().Trim();
				model.BaseEseTel = listDt.Rows[0]["TELNO_TASK"].ToString().Trim();
				model.BaseEseAddr = "(" + listDt.Rows[0]["ZIPCODE"].ToString().Trim() + ") " + listDt.Rows[0]["ADDR"].ToString().Trim();
			}


			model.arrayCnAddr1 = CnAddrSearch(0, 0);
			model.arrayUsAddr1 = UsAddrSearch(0, "");


			model.Item.QTY_BOX = 1;


			return model;
		}



		//배송 등록
		public string EtsReqIns(EtsReqModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";
			List<string> exeQueryArray = new List<string>();

			//ESE_CODE 값.
			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();

			model.Item.WAYBILLNO = "000000003"; //중복 인서트가 안될 것이므로 테스트할때 + 1 씩 해서 작업할것
			model.Item.DELVNO = "111";			//API연동으로 가져와야 되는 값 일단 이것으로 고정해 놓고 작업
			model.Item.DELV_COM = "222";        //API연동으로 가져와야 되는 값 일단 이것으로 고정해 놓고 작업


			if (model.act_type != null && model.act_type == "ins")
			{
				model = CreatEtsBase(model);    //송장 번호 / 현지 배송 회사 / APIKEY 연동 현지 배송 번호 

				exeQueryStr = " INSERT INTO ord_master ( ";
				exeQueryStr += " ESE_CODE, WAYBILLNO, DELVNO, DELV_COM, ORDERTYPE, ";	//기본 정보
				exeQueryStr += " DEP_NATION_CODE, EST_CODE, ORDERNO1, SENDER_NAME, SENDER_TELNO, SENDER_ADDR, ";	//발송인 정보
				exeQueryStr += " RECEIVER_NAME, RECEIVER_TELNO, RECEIVER_CPHONENO, SOCIALNO_BIZNO, RECEIVER_EMAIL, NATION_CODE, RECEIVERTYPE, CLEAR_PDT_CODE, ";	//수취인 정보1
				exeQueryStr += " RECEIVER_STATE, RECEIVER_CITY, RECEIVER_DISTRICT, RECEIVER_ZIPCODE, RECEIVER_ADDR1, RECEIVER_ADDR2, ";	//수취인 정보2
				exeQueryStr += " DIM_WIDTH, DIM_LENGTH, DIM_HEIGHT, REALVOLUME, WEIGHT_UNIT, REALWEIGHT, CHARGEABLE_WEIGHT, DELV_CODE, QTY_BOX, USER1, USER2, USER3,MESSAGE_DELV "; //기타 정보
				exeQueryStr += " ,UPLOADYMD ,CURRENCY_GOODS"; //기본값 세팅
				exeQueryStr += " ) VALUES (";
				
				//기본 정보
				exeQueryStr += "  '" + eseCode + "'";   //센더코드
				exeQueryStr += " , '" + model.Item.WAYBILLNO + "'";   //송장번호 (생성 로직 작성 필요)
				exeQueryStr += " , '" + model.Item.DELVNO + "'";   //현지 배송 번호	(생성 로직 작성 필요)
				exeQueryStr += " , '" + model.Item.DELV_COM + "'";   //배송회사 코드	(생성 로직 작성 필요)
				exeQueryStr += " , " + model.Item.ORDERTYPE + "";   //배송 타입 (일반 / 보관) int

				//발송인 정보
				exeQueryStr += " , '" + model.Item.DEP_NATION_CODE + "'";   //출발국가
				exeQueryStr += " , '" + model.Item.EST_CODE + "'";   //출발 스테이션
				exeQueryStr += " , '" + model.Item.ORDERNO1 + "'";   //주문 번호 1
				exeQueryStr += " , '" + model.Item.SENDER_NAME + "'";   //보내는 사람 이름
				exeQueryStr += " , '" + model.Item.SENDER_TELNO + "'";   //보내는 사람 전화번호
				exeQueryStr += " , '" + model.Item.SENDER_ADDR + "'";   //보내는 사람 주소

				//수취인 정보
				exeQueryStr += " , '" + model.Item.RECEIVER_NAME + "'";   //수취인 명
				exeQueryStr += " , '" + model.Item.RECEIVER_TELNO + "'";   //전화번호1
				exeQueryStr += " , '" + model.Item.RECEIVER_CPHONENO + "'";   //전화번호2
				exeQueryStr += " , '" + model.Item.SOCIALNO_BIZNO + "'";   //주민/통관번호
				exeQueryStr += " , '" + model.Item.RECEIVER_EMAIL + "'";   //이메일
				exeQueryStr += " , '" + model.Item.NATION_CODE + "'";   //도착국가
				exeQueryStr += " , '" + model.Item.RECEIVERTYPE + "'";   //받는이 구분(1=개인, 2=사업자)
				exeQueryStr += " , " + model.Item.CLEAR_PDT_CODE + "";   //신청타입(0:목록,1:일반) => 배송국가 한국 INT
				exeQueryStr += " , '" + model.Item.RECEIVER_STATE + "'";   //성
				exeQueryStr += " , '" + model.Item.RECEIVER_CITY + "'";   //시
				exeQueryStr += " , '" + model.Item.RECEIVER_DISTRICT + "'";   //구
				exeQueryStr += " , '" + model.Item.RECEIVER_ZIPCODE + "'";   //우편번호
				exeQueryStr += " , '" + model.Item.RECEIVER_ADDR1 + "'";   //기본주소
				exeQueryStr += " , '" + model.Item.RECEIVER_ADDR2 + "'";   //상세주소

				//기타정보
				exeQueryStr += " , " + model.Item.DIM_WIDTH + "";   //가로 DOUBLE
				exeQueryStr += " , " + model.Item.DIM_LENGTH + "";   //세로 DOUBLE
				exeQueryStr += " , " + model.Item.DIM_HEIGHT + "";   //높이 DOUBLE
				exeQueryStr += " , " + model.Item.REALVOLUME + "";   //부피무게 DOOUBLE
				exeQueryStr += " , '" + model.Item.WEIGHT_UNIT + "'";   //무게 단위
				exeQueryStr += " , " + model.Item.REALWEIGHT + "";   //실제무게 DOUBLE
				exeQueryStr += " , " + model.Item.CHARGEABLE_WEIGHT + "";   //적용무게 double
				exeQueryStr += " , '" + model.Item.DELV_CODE + "'";   //배송타입 -	출고타입 기호(A~Z)
				exeQueryStr += " , " + model.Item.QTY_BOX + "";   //박스수량 int
				exeQueryStr += " , '" + model.Item.USER1 + "'";   //ETC1	변경 필요
				exeQueryStr += " , '" + model.Item.USER2 + "'";   //ETC2
				exeQueryStr += " , '" + model.Item.USER3 + "'";   //ETC3
				exeQueryStr += " , '" + model.Item.MESSAGE_DELV + "'";   //메모	지정 안되어있음

				//기본값
				exeQueryStr += " , DATE_FORMAT(NOW(), '%Y%m%e') ";
				exeQueryStr += " , '" + model.Item.CURRENCY_GOODS + "'";   //화폐단위
				exeQueryStr += " ) ";

				exeQueryArray.Add(exeQueryStr);
				foreach (OrdGoods item in model.Items)
				{
					exeQueryArray.Add(CreatOrdGoods(model.Item.EST_CODE, model.Item.ESE_CODE, model.Item.WAYBILLNO, model.Item.ORDERTYPE, item));
				}

				//exeQueryArray.Add(CreatStat(model.Item.EST_CODE, model.Item.ESE_CODE, model.Item.WAYBILLNO));
			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				exeQueryStr = " UPDATE ord_master SET ";

				exeQueryStr += " DELVNO = '" + model.Item.DELVNO + "' , ";
				exeQueryStr += " DELV_COM = '" + model.Item.DELV_COM + "' , ";
				exeQueryStr += " ORDERTYPE = '" + model.Item.ORDERTYPE + "' , ";
				exeQueryStr += " DEP_NATION_CODE = '" + model.Item.DEP_NATION_CODE + "' , ";
				exeQueryStr += " EST_CODE = '" + model.Item.EST_CODE + "' , ";
				exeQueryStr += " ORDERNO1 = '" + model.Item.ORDERNO1 + "' , ";
				exeQueryStr += " SENDER_NAME = '" + model.Item.SENDER_NAME + "' , ";
				exeQueryStr += " SENDER_TELNO = '" + model.Item.SENDER_TELNO + "' , ";
				exeQueryStr += " SENDER_ADDR = '" + model.Item.SENDER_ADDR + "' , ";
				exeQueryStr += " RECEIVER_NAME = '" + model.Item.RECEIVER_NAME + "' , ";
				exeQueryStr += " RECEIVER_TELNO = '" + model.Item.RECEIVER_TELNO + "' , ";
				exeQueryStr += " RECEIVER_CPHONENO = '" + model.Item.RECEIVER_CPHONENO + "' , ";
				exeQueryStr += " SOCIALNO_BIZNO = '" + model.Item.SOCIALNO_BIZNO + "' , ";
				exeQueryStr += " RECEIVER_EMAIL = '" + model.Item.RECEIVER_EMAIL + "' , ";
				exeQueryStr += " NATION_CODE = '" + model.Item.NATION_CODE + "' , ";
				exeQueryStr += " RECEIVERTYPE = '" + model.Item.RECEIVERTYPE + "' , ";
				exeQueryStr += " CLEAR_PDT_CODE = " + model.Item.CLEAR_PDT_CODE + " , "; //INT
				exeQueryStr += " RECEIVER_STATE = '" + model.Item.RECEIVER_STATE + "' , ";
				exeQueryStr += " RECEIVER_CITY = '" + model.Item.RECEIVER_CITY + "' , ";
				exeQueryStr += " RECEIVER_DISTRICT = '" + model.Item.RECEIVER_DISTRICT + "' , ";
				exeQueryStr += " RECEIVER_ZIPCODE = '" + model.Item.RECEIVER_ZIPCODE + "' , ";
				exeQueryStr += " RECEIVER_ADDR1 = '" + model.Item.RECEIVER_ADDR1 + "' , ";
				exeQueryStr += " RECEIVER_ADDR2 = '" + model.Item.RECEIVER_ADDR2 + "' , ";
				exeQueryStr += " DIM_WIDTH = " + model.Item.DIM_WIDTH + " , "; //DOUBLE
				exeQueryStr += " DIM_LENGTH = " + model.Item.DIM_LENGTH + " , "; //DOUBLE
				exeQueryStr += " DIM_HEIGHT = " + model.Item.DIM_HEIGHT + " , "; //DOUBLE
				exeQueryStr += " REALVOLUME = " + model.Item.REALVOLUME + " , "; //DOUBLE
				exeQueryStr += " WEIGHT_UNIT = '" + model.Item.WEIGHT_UNIT + "' , ";
				exeQueryStr += " REALWEIGHT = '" + model.Item.REALWEIGHT + "' , ";
				exeQueryStr += " CHARGEABLE_WEIGHT = " + model.Item.CHARGEABLE_WEIGHT + " , "; //DOUBLE
				exeQueryStr += " DELV_CODE = '" + model.Item.DELV_CODE + "' , ";
				exeQueryStr += " QTY_BOX = " + model.Item.QTY_BOX + " , ";  //INT
				exeQueryStr += " USER1 = '" + model.Item.USER1 + "' , ";
				exeQueryStr += " USER2 = '" + model.Item.USER2 + "' , ";
				exeQueryStr += " USER3 = '" + model.Item.USER3 + "' , ";
				exeQueryStr += " MESSAGE_DELV = '" + model.Item.MESSAGE_DELV + "'  ";

				exeQueryStr += " WHERE 1=1 " ;

				exeQueryStr += " AND ESE_CODE   	= '" + model.Item.ESE_CODE + "' , ";
				exeQueryStr += " AND WAYBILLNO   	= '" + model.Item.WAYBILLNO + "' , ";

				exeQueryArray.Add(exeQueryStr);

				exeQueryArray.Add("DELETE FROM ord_goods WHERE REQ_SEQNO = " + model.Item.WAYBILLNO);

				foreach (OrdGoods item in model.Items)
				{
					exeQueryArray.Add(CreatOrdGoods(model.Item.EST_CODE, model.Item.ESE_CODE, model.Item.WAYBILLNO, model.Item.ORDERTYPE, item));
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


		//송장 번호 / 현지 배송 회사 / APIKEY 연동 현지 배송 번호 
		public EtsReqModels CreatEtsBase(EtsReqModels model)
		{
			string error_str = "";
			model.Item.GoodsList = model.Items;

			API_DELV api = new API_DELV();
			model.Item = API_DELV.GetLocalDeliveryNoOnes(model.Item, ref error_str);

			model.err_str = error_str;

			return model;
		}



		public string CreatStat(string EST_CODE, string ESE_CODE, string WAYBILLNO)
		{
			//ESE_CODE 값.
			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();

			string resultStr = " INSERT INTO ord_stats (EST_CODE, ESE_CODE, UPLOADYMD, WAYBILLNO, STATUS, ) VALUES ( ";
			resultStr += "  '" + EST_CODE + "'";
			resultStr += " , '" + ESE_CODE + "'";
			resultStr += " , DATE_FORMAT(NOW(), '%Y%m%e') ";
			resultStr += " , '" + WAYBILLNO + "'";
			resultStr += " ) ";
			//return resultStr;
			return "";

		}

		//배송 상품 등록 쿼리 생성
		public string CreatOrdGoods(string EST_CODE, string ESE_CODE, string WAYBILLNO, int type, OrdGoods item)
		{
			//ESE_CODE 값.
			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();


			string exeQueryStr = "";
			exeQueryStr = " INSERT INTO ord_goods ( EST_CODE, ESE_CODE, UPLOADYMD, WAYBILLNO, ITEMSN, GOODS_NAME, BRAND, PRICE, QTY, HSCODE, PURCHASE_URL, BARCODE ) VALUES (";
			exeQueryStr += "  '" + EST_CODE + "'";
			exeQueryStr += " , '" + eseCode + "'";
			exeQueryStr += " , DATE_FORMAT(NOW(), '%Y%m%e') ";
			exeQueryStr += " , '" + WAYBILLNO + "'";
			exeQueryStr += " , " + item.ITEMSN + ""; //INT
			exeQueryStr += " , '" + item.GOODS_NAME + "'";
			exeQueryStr += " , '" + item.BRAND + "'";
			exeQueryStr += " , " + item.PRICE + ""; //DOUBLE
			exeQueryStr += " , " + item.QTY + "";//INT
			exeQueryStr += " , '" + item.HSCODE + "'";
			exeQueryStr += " , '" + item.PURCHASE_URL + "'";
			exeQueryStr += " , '" + item.BARCODE + "'";
			exeQueryStr += " ) ";

			if (type == 1)
			{
				string selectQueryStr = "SELECT SKU, UNIT_WEIGHT, WEIGHT_UNIT FROM stc_goods WHERE ESE_CODE = '" + EST_CODE + "' AND '" + item.BARCODE + "'";
				string errorStr = "";

				DataTable listDt = getQueryResult(selectQueryStr, out errorStr);

				if (listDt != null && listDt.Rows.Count != 0)
				{
					item.VOLUME = double.Parse(listDt.Rows[0]["UNIT_WEIGHT"].ToString().Trim()); 
					item.UNIT = listDt.Rows[0]["WEIGHT_UNIT"].ToString().Trim();
					item.SKU = listDt.Rows[0]["SKU"].ToString().Trim();
				}

				exeQueryStr = " INSERT INTO ord_goods ( EST_CODE, ESE_CODE, UPLOADYMD, WAYBILLNO, ITEMSN, GOODS_NAME, BRAND, PRICE, QTY, HSCODE, VOLUME, UNIT, PURCHASE_URL, BARCODE, SKU ) VALUES (";
				exeQueryStr += "  '" + EST_CODE + "'";
				exeQueryStr += " , '" + ESE_CODE + "'";
				exeQueryStr += " , DATE_FORMAT(NOW(), \"%Y%m%e\") ";
				exeQueryStr += " , '" + WAYBILLNO + "'";
				exeQueryStr += " , " + item.ITEMSN + ""; //INT
				exeQueryStr += " , '" + item.GOODS_NAME + "'";
				exeQueryStr += " , '" + item.BRAND + "'";
				exeQueryStr += " , " + item.PRICE + ""; //DOUBLE
				exeQueryStr += " , " + item.QTY + ""; //INT
				exeQueryStr += " , '" + item.HSCODE + "'";
				exeQueryStr += " , '" + item.VOLUME + "'";
				exeQueryStr += " , '" + item.UNIT + "'";
				exeQueryStr += " , '" + item.PURCHASE_URL + "'";
				exeQueryStr += " , '" + item.BARCODE + "'";
				exeQueryStr += " , '" + item.SKU + "'";
				exeQueryStr += " ) ";

			}

			


			return exeQueryStr;
		}


		//AJAX 바코드 조회
		public EtsReqModels EtsReqChkBarcode(EtsReqModels model)
		{
			List<schTypeArray> result = new List<schTypeArray>();

			string errorStr = "";
			string listQuery = " ";

			listQuery = " SELECT PRODUCT_NAME, BRAND, SALE_SITE_URL FROM stc_goods ";
			listQuery += " WHERE EST_CODE = '" + model.AjaxEstCode + "' AND ESE_CODE = '" + model.AjaxEseCode + "' AND BARCODE = '" + model.AjaxBarCode+"' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.InItem.EST_CODE = model.AjaxEstCode;
					model.InItem.ESE_CODE = model.AjaxEseCode;
					model.InItem.BARCODE = model.AjaxBarCode;
					model.InItem.GOODS_NAME = listDt.Rows[0]["PRODUCT_NAME"].ToString().Trim();
					model.InItem.BRAND = listDt.Rows[0]["BRAND"].ToString().Trim();
					model.InItem.PURCHASE_URL = listDt.Rows[0]["SALE_SITE_URL"].ToString().Trim();
				}

				//재고 수량 조회
				listQuery = " SELECT INOUT_TYPE, sum(GOODS_CNT) as cnt ";
				listQuery += " FROM stc_inout ";
				listQuery += " WHERE EST_CODE = ''" + model.AjaxEstCode + "'' AND ESE_CODE = '" + model.AjaxEseCode + "' AND BARCODE = '" + model.AjaxBarCode + "' ";
				listQuery += " GROUP BY INOUT_TYPE  ORDER BY INOUT_TYPE DESC";

				listDt = getQueryResult(listQuery, out errorStr);

				List<schTypeArray> tmp = new List<schTypeArray>();

				if (listDt != null && listDt.Rows.Count != 0)
				{
					for (int i = 0; i < listDt.Rows.Count; i++)
					{
						tmp.Add(new schTypeArray { opt_value = listDt.Rows[i]["cnt"].ToString().Trim(), opt_key = listDt.Rows[i]["INOUT_TYPE"].ToString().Trim() });
					}
				}

				model.InItem.stoc_cnt = 0;
				foreach(schTypeArray calc    in tmp)
				{
					if(calc.opt_key == "0")
					{
						model.InItem.stoc_cnt += int.Parse(calc.opt_value);
					}
					else
					{
						model.InItem.stoc_cnt -= int.Parse(calc.opt_value);
					}
				}
			}
			
			return model;
		}

		
		//AJAX 국가별 STATION 조회 / View 사용
		public List<schTypeArray> EtsReqChkStation(EtsReqModels getModel)
		{
			List<schTypeArray> model = new List<schTypeArray>();

			string errorStr = "";
			string listQuery = "";

			listQuery = " SELECT eu.EST_CODE, es.EST_NAME ";
			listQuery += " FROM ";
			listQuery += " (SELECT EST_CODE FROM ese_user WHERE GROUP_ID = 0 AND ESE_CODE = '" + getModel.AjaxEseCode + "' ) eu ";
			listQuery += " inner join  ";
			listQuery += " ( SELECT EST_CODE, EST_NAME FROM esm_station WHERE NATION_CODE = '" + getModel.AjaxNationCode + "' ) es  ";
			listQuery += " on eu.EST_CODE = es.EST_CODE ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);


			if (listDt != null && listDt.Rows.Count != 0)
			{
				model.Add(new schTypeArray { opt_key = listDt.Rows[0]["EST_CODE"].ToString().Trim(), opt_value = listDt.Rows[0]["EST_NAME"].ToString().Trim() });
			}


			//ESE가 추가로 사용 가능항 스테이션 정보 가져오기
			listQuery = " SELECT eue.USE_EST, est.EST_NAME ";
			listQuery += " FROM ";
			listQuery += " ( SELECT EST_CODE, ESE_CODE FROM ese_user WHERE GROUP_ID = 0 AND ESE_CODE = '" + getModel.AjaxEseCode + "' ) ese ";
			listQuery += " inner join ";
			listQuery += " ( SELECT EST_CODE, ESE_CODE, USE_EST FROM ese_use_est eue WHERE NATION_CODE = '" + getModel.AjaxNationCode + "' ) eue ";
			listQuery += " on ese.EST_CODE = eue.EST_CODE AND ese.ESE_CODE = eue.ESE_CODE ";
			listQuery += " inner join ";
			listQuery += " ( SELECT EST_CODE, EST_NAME FROM esm_station WHERE NATION_CODE = '" + getModel.AjaxNationCode + "' ) est ";
			listQuery += " on eue.USE_EST = est.EST_CODE ";


			listDt = getQueryResult(listQuery, out errorStr);

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_key = listDt.Rows[i]["USE_EST"].ToString().Trim(), opt_value = listDt.Rows[i]["EST_NAME"].ToString().Trim() });
				}
			}

			return model;
		}


		//AJAX RELEASE_CODE 콤보박스 
		public List<schTypeArray> EtsReqChkReleaseCode(EtsReqModels getModel)
		{

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();

			string errorStr = "";
			string listQuery = " SELECT DELV_CODE, RELEASE_CODE, RELEASE_NAME FROM conf_release_type WHERE  NATION_CODE = '" + getModel.AjaxNationCode + "' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();
			string set_value = "";

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					set_value = "[" + listDt.Rows[i]["DELV_CODE"].ToString().Trim() + "] " + listDt.Rows[i]["RELEASE_NAME"].ToString().Trim();
					model.Add(new schTypeArray { opt_key = listDt.Rows[i]["DELV_CODE"].ToString().Trim() , opt_value = set_value  });
				}
			}

			return model;
		}


		//AJAX 요금표 가져오기
		public List<EstShippingFee> EtsReqChkCost(EtsReqModels getModel)
		{
			List<EstShippingFee> model = new List<EstShippingFee>();

			string[] selectColumn_CostIndex = {
				"EST_CODE" ,
				"NATION_CODE" ,
				"RELEASE_CODE" ,
				"WEIGHT" ,
				"SHIPPING_FEE_NOR" ,
				"SHIPPING_FEE_STC"
			};

			string errorStr = "";
			string listQuery = " SELECT SEQNO , " + string.Join(",", selectColumn_CostIndex);
			listQuery += " FROM est_shipping_fee WHERE 1=1 ";
			listQuery += " AND  NATION_CODE = '" + getModel.AjaxNationCode.Trim() + "' ";
			listQuery += " AND  EST_CODE = '" + getModel.AjaxEstCode.Trim() + "' ";
			listQuery += " AND  RELEASE_CODE = ( SELECT RELEASE_CODE FROM conf_release_type WHERE NATION_CODE = '" + getModel.AjaxNationCode.Trim() + "' AND DELV_CODE = '" + getModel.AjaxReleaseCode.Trim() + "')   ";
			string eseQuery = " AND ESE_CODE = '" + getModel.AjaxEseCode.Trim() + "' ";
			string defQuery = " AND ESE_CODE = '00000000' ";

			//ESE에 배정된 별도 가격표 조회
			DataTable listDt = getQueryResult(listQuery + eseQuery, out errorStr);

			if (listDt == null || listDt.Rows.Count == 0)
			{
				//별도의 가격표가 없을 경우 기본 가격표 조회
				listDt = getQueryResult(listQuery + defQuery, out errorStr);
			}

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					EstShippingFee temp = new EstShippingFee();

					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();
					temp.RELEASE_CODE = listDt.Rows[i]["RELEASE_CODE"].ToString().Trim();
					temp.WEIGHT = double.Parse(listDt.Rows[i]["WEIGHT"].ToString().Trim());
					temp.SHIPPING_FEE_NOR = double.Parse(listDt.Rows[i]["SHIPPING_FEE_NOR"].ToString().Trim());
					temp.SHIPPING_FEE_STC = double.Parse(listDt.Rows[i]["SHIPPING_FEE_STC"].ToString().Trim());

					model.Add(temp);
				}
			}

			return model;
		}




		//배송 라벨 출력 리스트 기본정보 (국가 스테이션 콤보박스용 데이터 등)
		public EtsLabelModels EtsLabelSetBase(EtsLabelModels model)
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

			//소속 스테이션 정보 가져오기 
			listQuery = " SELECT ";
			listQuery += " est.NATION_CODE ";
			listQuery += " ,est.EST_CODE ";
			listQuery += " ,ese.ESE_CODE ";
			listQuery += " ,est.EST_NAME ";
			listQuery += " ,cn." + getCol;
			listQuery += " FROM ";
			listQuery += " esm_station est ";
			listQuery += " inner join";
			listQuery += " (SELECT EST_CODE, ESE_CODE FROM ese_user WHERE ESE_CODE = '" + eseCode + "' ) ese ";
			listQuery += " on est.EST_CODE = ese.EST_CODE ";
			listQuery += " inner join ";
			listQuery += " comm_nation cn";
			listQuery += " on est.NATION_CODE = cn.NATIONNO	";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

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
			listQuery = "SELECT ";
			listQuery += "cn.NATIONNO";
			listQuery += ",cn." + getCol + "";
			listQuery += " FROM";
			listQuery += "(SELECT distinct NATION_CODE FROM ese_use_est";
			listQuery += " WHERE EST_CODE = '" + model.BaseEstCode + "' AND ESE_CODE = '" + model.BaseEseCode + "'  AND NATION_CODE != '" + model.BaseNationCode + "' ) eue";
			listQuery += " inner join";
			listQuery += " comm_nation cn";
			listQuery += " on eue.NATION_CODE = cn.NATIONNO";


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


		//배송 라벨 출력 리스트 가져오기
		public EtsLabelModels GetEtsLabelList(EtsLabelModels model)
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

			string listQuery = "SELECT WAYBILLNO, DEP_NATION_CODE, NATION_CODE, ORDERNO1, DELVNO, DELVFEE, UPLOADYMD ";
			string cntQuery = " SELECT count(*) as cnt ";

			baseQuery += " FROM  ord_master ";
			baseQuery += " WHERE ESE_CODE = '" + eseCode + "' ";

			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  EST_CODE = '" + model.schStation.Trim() + "' "; // STATION

			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  DEP_NATION_CODE = '" + model.schSNation.Trim() + "' "; // 출발 국가

			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  NATION_CODE = '" + model.schENation.Trim() + "' "; // 도착 국가

			if (!String.IsNullOrEmpty(model.schSdt))      // 업로드일자 (시작일)
				baseQuery += " AND  UPLOADYMD >= '" + model.schSdt.Trim() + "' ";

			if (!String.IsNullOrEmpty(model.schEdt))      // 업로드 일자 (종료일)
				baseQuery += " AND  UPLOADYMD <= '" + model.schEdt.Trim() + " 23:59:59' ";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt.Trim() + "%' ";

			string endQuery = "ORDER BY" + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery ;  //리스트 쿼리

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
					ETS_LABEL_ITEM temp = new ETS_LABEL_ITEM();

					temp.WAYBILLNO = listDt.Rows[i]["WAYBILLNO"].ToString().Trim();
					temp.DEP_NATION_CODE = listDt.Rows[i]["DEP_NATION_CODE"].ToString().Trim();		
					temp.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();			
					temp.ORDERNO1 = listDt.Rows[i]["ORDERNO1"].ToString().Trim();
					temp.DELVNO = listDt.Rows[i]["DELVNO"].ToString().Trim();
					temp.DELVFEE = double.Parse(listDt.Rows[i]["DELVFEE"].ToString().Trim());
					temp.STATUS = 0;
					temp.UPLOADYMD = listDt.Rows[i]["UPLOADYMD"].ToString().Trim();
					temp.MEMO = "";
					
					/* ord_master 테이블에 status가 없음
					foreach (schTypeArray tempS in model.arrayStat)
					{
						if (tempS.opt_key == listDt.Rows[i]["STATUS"].ToString().Trim())
						{
							temp.STATUS_TEST = tempS.opt_value;
						}
					}
					*/

					model.Items.Add(temp);
				}
			}


			return model;
		}

		//배송 라벨 출력 리스트에서 선택삭제 아직 미완성
		public string delEtsLabelList(string seq)
		{
			string errorStr = "";
			string result = "";
			List<string> exeQueryArray = new List<string>();

			//exeQueryArray.Add(" UPDATE ord_master SET INPUT_STAT = 5 WHERE SEQNO in (" + seq + ") ");

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





		// 중국주소를 리턴한다
		public List<schTypeArray> CnAddrSearch(int level, int selectItem)
		{
			string errorStr = "";
			string listQuery = "";
			List<schTypeArray> model = new List<schTypeArray>();
			DataTable listDt = new DataTable();
			if (level == 0)
			{
				listQuery = "select distinct PROV_CODE, PROV_NAME from zipcode_cn order by PROV_CODE asc";
				listDt = getQueryResult(listQuery, out errorStr);

				if (listDt != null && listDt.Rows.Count != 0)
				{
					for (int i = 0; i < listDt.Rows.Count; i++)
					{
						model.Add(new schTypeArray { opt_key = listDt.Rows[i]["PROV_CODE"].ToString().Trim(), opt_value = listDt.Rows[i]["PROV_NAME"].ToString().Trim() });
					}
				}
			}
			else if (level == 1)
			{
				// 1단계 주소를 선택하면 2단계 주소목록을 리턴한다
				listQuery = "select distinct CITY_CODE, CITY_NAME from zipcode_cn " + string.Format(" where PROV_CODE={0}", selectItem) + " order by CITY_CODE asc";
				listDt = getQueryResult(listQuery, out errorStr);

				if (listDt != null && listDt.Rows.Count != 0)
				{
					for (int i = 0; i < listDt.Rows.Count; i++)
					{
						model.Add(new schTypeArray { opt_key = listDt.Rows[i]["CITY_CODE"].ToString().Trim(), opt_value = listDt.Rows[i]["CITY_NAME"].ToString().Trim() });
					}
				}
			}
			else if (level == 2)
			{
				// 2단계 주소를 선택하면 3단계 주소목록을 리턴한다
				listQuery = "select distinct COUNTY_CODE, COUNTY_NAME, ZIPCODE from zipcode_cn " + string.Format(" where CITY_CODE={0}", selectItem) + " order by COUNTY_CODE asc";
				listDt = getQueryResult(listQuery, out errorStr);

				if (listDt != null && listDt.Rows.Count != 0)
				{
					for (int i = 0; i < listDt.Rows.Count; i++)
					{
						model.Add(new schTypeArray { opt_key = listDt.Rows[i]["ZIPCODE"].ToString().Trim() , opt_value = listDt.Rows[i]["COUNTY_NAME"].ToString().Trim() });
					}
				}
			}

			return model;
		}


		// 미국주소를 리턴한다
		public List<schTypeArray> UsAddrSearch(int level, string selectItem)
		{
			string errorStr = "";
			string listQuery = "";
			List<schTypeArray> model = new List<schTypeArray>();
			DataTable listDt = new DataTable();

			if (level == 0)
			{
				// 1단계 주소목록을 리턴한다
				listQuery = "select distinct STATE_MIN, STATE from zipcode_us order by STATE_MIN asc";
				listDt = getQueryResult(listQuery, out errorStr);

				if (listDt != null && listDt.Rows.Count != 0)
				{
					for (int i = 0; i < listDt.Rows.Count; i++)
					{
						model.Add(new schTypeArray { opt_key = listDt.Rows[i]["STATE_MIN"].ToString().Trim(), opt_value = listDt.Rows[i]["STATE"].ToString().Trim() });
					}
				}
			}
			else if (level == 1)
			{
				// 1단계 주소를 선택하면 2단계 주소목록을 리턴한다
				listQuery = "select distinct CITY, ZIPCODE from zipcode_us" + string.Format(" where STATE_MIN='{0}'", selectItem) + " order by CITY asc";
				listDt = getQueryResult(listQuery, out errorStr);

				if (listDt != null && listDt.Rows.Count != 0)
				{
					for (int i = 0; i < listDt.Rows.Count; i++)
					{
						model.Add(new schTypeArray { opt_key = listDt.Rows[i]["ZIPCODE"].ToString().Trim(), opt_value = listDt.Rows[i]["CITY"].ToString().Trim() });
					}
				}
			}
			
			return model;
		}



		//배송 상태 조회 리스트 기본정보 (국가 스테이션 콤보박스용 데이터 등)
		public EtsListModels EtsListSetBase(EtsListModels model)
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

			//소속 스테이션 정보 가져오기 
			listQuery = " SELECT ";
			listQuery += " est.NATION_CODE ";
			listQuery += " ,est.EST_CODE ";
			listQuery += " ,ese.ESE_CODE ";
			listQuery += " ,est.EST_NAME ";
			listQuery += " ,cn." + getCol;
			listQuery += " FROM ";
			listQuery += " esm_station est ";
			listQuery += " inner join";
			listQuery += " (SELECT EST_CODE, ESE_CODE FROM ese_user WHERE ESE_CODE = '" + eseCode + "' ) ese ";
			listQuery += " on est.EST_CODE = ese.EST_CODE ";
			listQuery += " inner join ";
			listQuery += " comm_nation cn";
			listQuery += " on est.NATION_CODE = cn.NATIONNO	";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

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
			listQuery = "SELECT ";
			listQuery += "cn.NATIONNO";
			listQuery += ",cn." + getCol + "";
			listQuery += " FROM";
			listQuery += "(SELECT distinct NATION_CODE FROM ese_use_est";
			listQuery += " WHERE EST_CODE = '" + model.BaseEstCode + "' AND ESE_CODE = '" + model.BaseEseCode + "'  AND NATION_CODE != '" + model.BaseNationCode + "' ) eue";
			listQuery += " inner join";
			listQuery += " comm_nation cn";
			listQuery += " on eue.NATION_CODE = cn.NATIONNO";


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



		//배송 상태 조회 리스트 가져오기
		public EtsListModels GetEtsList(EtsListModels model)
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

			string listQuery = "SELECT WAYBILLNO, DEP_NATION_CODE, NATION_CODE, ORDERNO1, DELVNO, DELVFEE, UPLOADYMD ";
			string cntQuery = " SELECT count(*) as cnt ";

			baseQuery += " FROM  ord_master ";
			baseQuery += " WHERE ESE_CODE = '" + eseCode + "' ";

			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  EST_CODE = '" + model.schStation.Trim() + "' "; // STATION

			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  DEP_NATION_CODE = '" + model.schSNation.Trim() + "' "; // 출발 국가

			if (!String.IsNullOrEmpty(model.schStation))
				baseQuery += " AND  NATION_CODE = '" + model.schENation.Trim() + "' "; // 도착 국가

			if (!String.IsNullOrEmpty(model.schSdt))      // 업로드일자 (시작일)
				baseQuery += " AND  UPLOADYMD >= '" + model.schSdt.Trim() + "' ";

			if (!String.IsNullOrEmpty(model.schEdt))      // 업로드 일자 (종료일)
				baseQuery += " AND  UPLOADYMD <= '" + model.schEdt.Trim() + " 23:59:59' ";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt.Trim() + "%' ";

			string endQuery = "ORDER BY" + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery;  //리스트 쿼리

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
					ETS_LIST_ITEM temp = new ETS_LIST_ITEM();

					temp.WAYBILLNO = listDt.Rows[i]["WAYBILLNO"].ToString().Trim();
					temp.DEP_NATION_CODE = listDt.Rows[i]["DEP_NATION_CODE"].ToString().Trim();
					temp.NATION_CODE = listDt.Rows[i]["NATION_CODE"].ToString().Trim();
					temp.ORDERNO1 = listDt.Rows[i]["ORDERNO1"].ToString().Trim();
					temp.DELVNO = listDt.Rows[i]["DELVNO"].ToString().Trim();
					temp.DELVFEE = double.Parse(listDt.Rows[i]["DELVFEE"].ToString().Trim());
					temp.STATUS = 0;
					temp.UPLOADYMD = listDt.Rows[i]["UPLOADYMD"].ToString().Trim();
					temp.MEMO = "";

					/* ord_master 테이블에 status가 없음
					foreach (schTypeArray tempS in model.arrayStat)
					{
						if (tempS.opt_key == listDt.Rows[i]["STATUS"].ToString().Trim())
						{
							temp.STATUS_TEST = tempS.opt_value;
						}
					}
					*/

					model.Items.Add(temp);
				}
			}


			return model;
		}



	}
}