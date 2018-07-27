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
using web_ese.Models_Act.Prod;
using static web_ese.Models_Act.Prod.ProdListModels;

namespace web_ese.Models_Db
{
	public class ProdDbModels : DatabaseConnection
	{


		string[] selectColoum_ProdAdd = {
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
			"RACKNO",
			"COMMENT",
			"REG_DT",
			"UPDT_DT"
		};


		public StcGoods GetProdAddView(ProdListModels getModel)
		{
			string errorStr = "";
			StcGoods model = new StcGoods();

			if (getModel.act_type != null && getModel.act_type == "updt")
			{

				string ViewQuery = " SELECT SEQNO, " + string.Join(",", selectColoum_ProdAdd) + " FROM stc_goods   WHERE SEQNO = " + getModel.act_key;
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
					model.UNIT_WEIGHT = double.Parse(listDt.Rows[0]["UNIT_WEIGHT"].ToString().Trim());
					model.WEIGHT_UNIT = listDt.Rows[0]["WEIGHT_UNIT"].ToString().Trim();
					model.STANDARD = listDt.Rows[0]["STANDARD"].ToString().Trim();
					model.EXPIRATION = int.Parse(listDt.Rows[0]["EXPIRATION"].ToString().Trim());
					model.EXPIRATION_DT = listDt.Rows[0]["EXPIRATION_DT"].ToString().Trim();
					model.ORIGIN = listDt.Rows[0]["ORIGIN"].ToString().Trim();
					model.INGREDIENT = listDt.Rows[0]["INGREDIENT"].ToString().Trim();
					model.SPEC = listDt.Rows[0]["SPEC"].ToString().Trim();
					model.SALE_SITE_URL = listDt.Rows[0]["SALE_SITE_URL"].ToString().Trim();
					model.PRODUCT_IMAGE = listDt.Rows[0]["PRODUCT_IMAGE"].ToString().Trim();
					model.RACKNO = listDt.Rows[0]["RACKNO"].ToString().Trim();
					model.COMMENT = listDt.Rows[0]["COMMENT"].ToString().Trim();
					model.REG_DT = listDt.Rows[0]["REG_DT"].ToString().Trim();
					model.UPDT_DT = listDt.Rows[0]["UPDT_DT"].ToString().Trim();
				}
			}

			return model;
		}

		public string SetProdAdd(ProdListModels model)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = "";
			
			if (model.act_type != null && model.act_type == "ins")
			{

				exeQueryStr = " INSERT INTO stc_goods(" + string.Join(",", selectColoum_ProdAdd) + " )VALUES(  ";
				exeQueryStr += "  '" + model.Item.EST_CODE + "' ";
				exeQueryStr += " , '" + model.Item.ESE_CODE + "' ";
				exeQueryStr += " , " + model.Item.CATEGORY1;
				exeQueryStr += " , " + model.Item.CATEGORY2;
				exeQueryStr += " , " + model.Item.CATEGORY3;
				exeQueryStr += " , " + model.Item.CATEGORY4;
				exeQueryStr += " , '" + model.Item.BARCODE + "' ";
				exeQueryStr += " , '" + model.Item.SKU + "' ";
				exeQueryStr += " , '" + model.Item.PRODUCT_NAME + "' ";
				exeQueryStr += " , '" + model.Item.PRODUCT_NAME_KR + "' ";
				exeQueryStr += " , '" + model.Item.PRODUCT_NAME_CN + "' ";
				exeQueryStr += " , '" + model.Item.PRODUCT_NAME_EN + "' ";
				exeQueryStr += " , '" + model.Item.BRAND + "' ";
				exeQueryStr += " , " + model.Item.PRICE;
				exeQueryStr += " , " + model.Item.UNIT_WEIGHT;
				exeQueryStr += " , '" + model.Item.WEIGHT_UNIT + "' ";
				exeQueryStr += " , '" + model.Item.STANDARD + "' ";
				exeQueryStr += " , " + model.Item.EXPIRATION;
				exeQueryStr += " , CURRENT_TIMESTAMP ";
				exeQueryStr += " , '" + model.Item.ORIGIN + "' ";
				exeQueryStr += " , '" + model.Item.INGREDIENT + "' ";
				exeQueryStr += " , '" + model.Item.SPEC + "' ";
				exeQueryStr += " , '" + model.Item.SALE_SITE_URL + "' ";
				exeQueryStr += " , '" + model.Item.PRODUCT_IMAGE + "' ";
				exeQueryStr += " , '" + model.Item.RACKNO + "' ";
				exeQueryStr += " , '" + model.Item.COMMENT + "' ";
				exeQueryStr += " , CURRENT_TIMESTAMP ";
				exeQueryStr += " , CURRENT_TIMESTAMP ";
				exeQueryStr += " ) ";
			}
			else if (model.act_type != null && model.act_type == "updt")
			{
				exeQueryStr = " UPDATE stc_goods SET ";
				exeQueryStr += " CATEGORY1 = '" + model.Item.CATEGORY1 + "' ";
				exeQueryStr += " ,CATEGORY2 = '" + model.Item.CATEGORY2 + "' ";
				exeQueryStr += " ,CATEGORY3 = '" + model.Item.CATEGORY3 + "' ";
				exeQueryStr += " ,CATEGORY4 = '" + model.Item.CATEGORY4 + "' ";
				exeQueryStr += " ,BARCODE = '" + model.Item.BARCODE + "' ";
				exeQueryStr += " ,SKU = '" + model.Item.SKU + "' ";
				exeQueryStr += " ,PRODUCT_NAME = '" + model.Item.PRODUCT_NAME + "' ";
				exeQueryStr += " ,PRODUCT_NAME_KR = '" + model.Item.PRODUCT_NAME_KR + "' ";
				exeQueryStr += " ,PRODUCT_NAME_CN = '" + model.Item.PRODUCT_NAME_CN + "' ";
				exeQueryStr += " ,PRODUCT_NAME_EN = '" + model.Item.PRODUCT_NAME_EN + "' ";
				exeQueryStr += " ,BRAND = '" + model.Item.BRAND + "' ";
				exeQueryStr += " ,PRICE = '" + model.Item.PRICE + "' ";
				exeQueryStr += " ,UNIT_WEIGHT = '" + model.Item.UNIT_WEIGHT + "' ";
				exeQueryStr += " ,WEIGHT_UNIT = '" + model.Item.WEIGHT_UNIT + "' ";
				exeQueryStr += " ,STANDARD = '" + model.Item.STANDARD + "' ";
				exeQueryStr += " ,EXPIRATION = '" + model.Item.EXPIRATION + "' ";
				//exeQueryStr += " ,EXPIRATION_DT = '" + model.Item.EXPIRATION_DT + "' ";
				exeQueryStr += " ,ORIGIN = '" + model.Item.ORIGIN + "' ";
				exeQueryStr += " ,INGREDIENT = '" + model.Item.INGREDIENT + "' ";
				exeQueryStr += " ,SPEC = '" + model.Item.SPEC + "' ";
				exeQueryStr += " ,SALE_SITE_URL = '" + model.Item.SALE_SITE_URL + "' ";
				exeQueryStr += " ,PRODUCT_IMAGE = '" + model.Item.PRODUCT_IMAGE + "' ";
				exeQueryStr += " ,RACKNO = '" + model.Item.RACKNO + "' ";
				exeQueryStr += " ,COMMENT = '" + model.Item.COMMENT + "' ";
				exeQueryStr += " ,UPDT_DT = CURRENT_TIMESTAMP ";
				exeQueryStr += " WHERE SEQNO = " + model.act_key;
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

		public string ChkUpdtProdAdd(ProdListModels model)
		{
			//esecode
			HttpContext context = HttpContext.Current;
			string ESE_CODE = context.Session["ESE_CODE"].ToString();

			string errorStr = "";
			string result = "";

			string listQuery = " SELECT count(*) as cnt FROM stc_goods WHERE BARCODE = '" + model.Item.BARCODE + "'  AND ESE_CODE = '" + ESE_CODE + "' ";


			int reCnt = getQueryCnt(listQuery, out errorStr);

			if (model.act_type != null && model.schTxt2 == "ins")
			{

				if (reCnt > 0 && model.Item.BARCODE != null)
				{
					result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.Script_Barcode + "]";

				}

			}
			else if (model.act_type != null && model.schTxt2 == "updt")
			{

				if (reCnt > 1 && model.Item.BARCODE != null)
				{
					result = @comm_global.Language.Resources.ESE_RETURN_DuplicatedInfo + "[" + @comm_global.Language.Resources.Script_Barcode + "]";

				}
			}


				return result;
		}


		public ProdListModels GetProdListList(ProdListModels model)
		{

			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();
			string estCode = context.Session["EST_CODE"].ToString();


			string errorStr = "";

			string listQuery = " SELECT SEQNO  , " + string.Join(",", selectColoum_ProdAdd);
			string cntQuery = " SELECT count(*) as cnt ";

			string baseQuery = " FROM stc_goods WHERE 1=1 ";

			baseQuery += " AND  ESE_CODE = '" + eseCode + "' ";
			baseQuery += " AND  EST_CODE = '" + estCode + "' ";


			if (model.cate1 != 0 )  //카테고리1
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
				baseQuery += " AND  UPDT_DT <= '" + model.schEdt.Trim() + " 23:59:59'";

			if (!String.IsNullOrEmpty(model.schTxt) && !String.IsNullOrEmpty(model.schTxt)) //브랜드
				baseQuery += " AND BRAND like '%" + model.schTxt.Trim() + "%' ";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt2))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt2.Trim() + "%' ";
			

			string endQuery = " ORDER BY " + model.sortKey.ToString().Trim() + " DESC limit " + ((model.Paging.page - 1) * model.Paging.pageNum) + " , " + model.Paging.pageNum;    //정렬

			cntQuery += baseQuery;              //토탈 카운트 쿼리 
			listQuery += baseQuery + endQuery;  //리스트 쿼리

			int totCnt = getQueryCnt(cntQuery, out errorStr);   //전체 리스트 갯수 구하기

			model.Paging.totCnt = totCnt;   //전체 갯수 세팅
			model.Paging.startCnt = totCnt - (model.Paging.pageNum * (model.Paging.page - 1));  //리스트 첫번째 시작 번호 
			model.Paging.pageTotNum = (totCnt / model.Paging.pageNum) + 1;  //총 페이징 갯수 구하기
			if ((totCnt % model.Paging.pageNum) == 0) model.Paging.pageTotNum -= 1;  //총 페이징 갯수가 0 일경우 + 1

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			int tmpCate = 0;

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					StcGoods temp = new StcGoods();

					temp.SEQNO = int.Parse(listDt.Rows[i]["SEQNO"].ToString().Trim());
					temp.EST_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.ESE_CODE = listDt.Rows[i]["EST_CODE"].ToString().Trim();
					temp.CATEGORY1 = int.Parse(listDt.Rows[i]["CATEGORY1"].ToString().Trim());
					temp.CATEGORY2 = int.Parse(listDt.Rows[i]["CATEGORY2"].ToString().Trim());
					temp.CATEGORY3 = int.Parse(listDt.Rows[i]["CATEGORY3"].ToString().Trim());
					temp.CATEGORY4 = int.Parse(listDt.Rows[i]["CATEGORY4"].ToString().Trim());
					temp.BARCODE = listDt.Rows[i]["BARCODE"].ToString().Trim();
					temp.SKU = listDt.Rows[i]["SKU"].ToString().Trim();
					temp.PRODUCT_NAME = listDt.Rows[i]["PRODUCT_NAME"].ToString().Trim();
					temp.PRODUCT_NAME_KR = listDt.Rows[i]["PRODUCT_NAME_KR"].ToString().Trim();
					temp.PRODUCT_NAME_CN = listDt.Rows[i]["PRODUCT_NAME_CN"].ToString().Trim();
					temp.PRODUCT_NAME_EN = listDt.Rows[i]["PRODUCT_NAME_EN"].ToString().Trim();
					temp.BRAND = listDt.Rows[i]["BRAND"].ToString().Trim();
					temp.PRICE = double.Parse(listDt.Rows[i]["PRICE"].ToString().Trim());
					temp.WEIGHT_UNIT = listDt.Rows[i]["WEIGHT_UNIT"].ToString().Trim();
					temp.STANDARD = listDt.Rows[i]["STANDARD"].ToString().Trim();
					temp.EXPIRATION = int.Parse(listDt.Rows[i]["EXPIRATION"].ToString().Trim());
					temp.EXPIRATION_DT = listDt.Rows[i]["EXPIRATION_DT"].ToString().Trim();
					temp.ORIGIN = listDt.Rows[i]["ORIGIN"].ToString().Trim();
					temp.INGREDIENT = listDt.Rows[i]["INGREDIENT"].ToString().Trim();
					temp.SPEC = listDt.Rows[i]["SPEC"].ToString().Trim();
					temp.SALE_SITE_URL = listDt.Rows[i]["SALE_SITE_URL"].ToString().Trim();
					temp.PRODUCT_IMAGE = listDt.Rows[i]["PRODUCT_IMAGE"].ToString().Trim();
					temp.RACKNO = listDt.Rows[i]["RACKNO"].ToString().Trim();
					temp.COMMENT = listDt.Rows[i]["COMMENT"].ToString().Trim();
					temp.REG_DT = listDt.Rows[i]["REG_DT"].ToString().Trim();
					temp.UPDT_DT = listDt.Rows[i]["UPDT_DT"].ToString().Trim();

					tmpCate = 0;
					if (temp.CATEGORY1 != 0)
						tmpCate = temp.CATEGORY1;
					if (temp.CATEGORY2 != 0)
						tmpCate = temp.CATEGORY2;
					if (temp.CATEGORY3 != 0)
						tmpCate = temp.CATEGORY3;
					if (temp.CATEGORY4 != 0)
						tmpCate = temp.CATEGORY4;

					if(tmpCate !=0)
						temp.CATEGORY1_txt = GetCategoryName(tmpCate);

					model.Items.Add(temp);
				}
			}

			return model;
		}

		public string delProdList(int seq)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM stc_goods WHERE SEQNO = " + seq;

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

		public string delProdList(string seq)
		{
			string errorStr = "";
			string result = "";
			string exeQueryStr = " DELETE FROM stc_goods WHERE SEQNO in (" + seq + ") ";

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


		//카테고리 콤보박스
		public List<schTypeArray> GetCategorySelectBox(int depth, int parent)
		{
			
			string getCol = "";
			string lan = "";
			switch (lan)    //lan <== 다국어 지원 관련 언어선택 부분이므로 일단 신경쓰지 말고 진행 할것  그냥 돌리면 알아서 NATIONNAME_ko_KR 을 설정함
			{
				case "CN":
					getCol = "CATE_NAME_CN";
					break;
				case "EN":
					getCol = "CATE_NAME_EN";
					break;
				default:
					getCol = "CATE_NAME_KR";
					break;
			}
			
			string errorStr = "";
			string listQuery = " SELECT SEQNO, " + getCol + " FROM stc_category WHERE CATE_LEVEL = " + depth + " AND CATE_PARENT = " + parent ;

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i][getCol].ToString().Trim(), opt_key = listDt.Rows[i]["SEQNO"].ToString().Trim() });
				}
			}

			return model;
		}


		//카테고리 이름 가져오기
		public string GetCategoryName(int SEQNO)
		{
			string resultStr = "";

			string getCol = "";
			string lan = "";
			switch (lan)    //lan <== 다국어 지원 관련 언어선택 부분이므로 일단 신경쓰지 말고 진행 할것  그냥 돌리면 알아서 NATIONNAME_ko_KR 을 설정함
			{
				case "CN":
					getCol = "CATE_NAME_CN";
					break;
				case "EN":
					getCol = "CATE_NAME_EN";
					break;
				default:
					getCol = "CATE_NAME_KR";
					break;
			}

			string errorStr = "";
			string listQuery = " SELECT " + getCol + " FROM stc_category WHERE SEQNO = " + SEQNO;

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			if (listDt != null && listDt.Rows.Count != 0)
			{
				resultStr = listDt.Rows[0][getCol].ToString().Trim();
			}

			return resultStr;
		}















		public MemoryStream GetProdListExcel(ProdListModels model)
		{

			string errorStr = "";

			string listQuery = " SELECT SEQNO  , " + string.Join(",", selectColoum_ProdAdd);

			string baseQuery = " FROM stc_goods WHERE 1=1 ";

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

			if (!String.IsNullOrEmpty(model.schTxt) && !String.IsNullOrEmpty(model.schTxt)) //브랜드
				baseQuery += " AND BRAND like '%" + model.schTxt.Trim() + "%' ";

			if (!String.IsNullOrEmpty(model.schTypeTxt) && !String.IsNullOrEmpty(model.schTxt2))  //검색조건 검색어
				baseQuery += " AND  " + model.schTypeTxt.Trim() + " like '%" + model.schTxt2.Trim() + "%' ";


			string endQuery = " ORDER BY " + model.sortKey.ToString().Trim() + " DESC ";    //정렬
			
			listQuery += baseQuery + endQuery;  //리스트 쿼리
			
			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			MemoryStream stream = new MemoryStream();

			using (ExcelPackage excelPackage = new ExcelPackage())
			{
				ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("SearchResult");

				int row = 1;
				int col = 1;

				// 첫줄
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_Num;  // 번호
				workSheet.Cells[row, col++].Value = "CATEGORY1";
				workSheet.Cells[row, col++].Value = "CATEGORY2";
				workSheet.Cells[row, col++].Value = "CATEGORY3";
				workSheet.Cells[row, col++].Value = "CATEGORY4";
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_Barcord;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_SKU;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_PRODUCT_NAME;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_PRODUCT_NAME_KR;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_PRODUCT_NAME_CN;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_PRODUCT_NAME_EN;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_BRAND;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_viewWEIGHT_UNIT;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_STANDARD;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_EXPIRATION;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_ORIGIN;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_INGREDIENT;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_SPEC;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_SALE_SITE_URL;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_PRODUCT_IMAGE;
				workSheet.Cells[row, col++].Value = @comm_global.Language.Resources.ESE_Comm_AddDate;
				
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
						workSheet.Cells[row, col++].Value = GetCategoryName(int.Parse(listDt.Rows[i]["CATEGORY1"].ToString().Trim()));
						workSheet.Cells[row, col++].Value = GetCategoryName(int.Parse(listDt.Rows[i]["CATEGORY2"].ToString().Trim()));
						workSheet.Cells[row, col++].Value = GetCategoryName(int.Parse(listDt.Rows[i]["CATEGORY3"].ToString().Trim()));
						workSheet.Cells[row, col++].Value = GetCategoryName(int.Parse(listDt.Rows[i]["CATEGORY4"].ToString().Trim()));
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["BARCODE"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["SKU"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["PRODUCT_NAME"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["PRODUCT_NAME_KR"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["PRODUCT_NAME_CN"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["PRODUCT_NAME_EN"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["BRAND"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["WEIGHT_UNIT"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["STANDARD"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["EXPIRATION"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["ORIGIN"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["INGREDIENT"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["SPEC"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["SALE_SITE_URL"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["PRODUCT_IMAGE"].ToString().Trim();
						workSheet.Cells[row, col++].Value = listDt.Rows[i]["REG_DT"].ToString().Trim();
						
					}
					
				}

				workSheet.Cells.AutoFitColumns();
				excelPackage.SaveAs(stream);
			}
			
			stream.Flush();
			stream.Position = 0;

			return stream;
				
		}
		


		public MemoryStream DownExcelProd(PROD_EXCEL model)
		{


			MemoryStream stream = new MemoryStream();

			using (ExcelPackage excelPackage = new ExcelPackage())
			{
				ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("SearchResult");

				int row = 1;
				int col = 1;
				

				// 첫줄
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_Barcord;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_Comm_PRODUCT_NAME;

				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_PRODUCT_NAME_EN;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_PRODUCT_NAME_CN;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_PRODUCT_NAME_KR;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_SKU;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_BRAND;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_UNIT_WEIGHT; //더블
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_viewWEIGHT_UNIT; //더블
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_STANDARD;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_EXPIRATION;		//int
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_ORIGIN;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_INGREDIENT;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_SPEC;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_SALE_SITE_URL;
				workSheet.Cells[row, col++].Value = comm_global.Language.Resources.ESE_ProdAdd_PRODUCT_IMAGE;

				workSheet.Cells[row, 1, row, (col - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				workSheet.Cells[row, 1, row, (col - 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
				workSheet.Cells[row, 1, row, (col - 1)].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
				
				workSheet.Cells.AutoFitColumns();
				
				excelPackage.SaveAs(stream);
			}

			stream.Flush();
			stream.Position = 0;

			return stream;

		}


		
		public PROD_EXCEL UploadExcelProd(PROD_EXCEL model)
		{
			HttpContext context = HttpContext.Current;
			string eseCode = context.Session["ESE_CODE"].ToString();
			string estCode = context.Session["EST_CODE"].ToString();

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
				


			//유효성 검사 
			if (data != null && data.Rows.Count != 0)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{
					//  0,1 필수 :: 7, 8 더블 :: 10 인트


					if (data.Rows[i][0].ToString().Trim() == "")	//필수값 체크
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_Barcord + "]");
						error_str = "error";
					}
					if (data.Rows[i][1].ToString().Trim() == "")    //필수값 체크
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_RequiredInput + "[" + comm_global.Language.Resources.ESE_Comm_PRODUCT_NAME + "]");
						error_str = "error";
					}

					try
					{
						Convert.ToDouble(data.Rows[i][7].ToString().Trim());    //더블형 체크
					}
					catch
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_DoubleInput + "[" + comm_global.Language.Resources.ESE_ProdAdd_UNIT_WEIGHT + "]");
						error_str = "error";
					}
					/*
					try
					{
						Convert.ToDouble(data.Rows[i][8].ToString().Trim());	//더블형 체크
					}
					catch
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_DoubleInput + "[" + comm_global.Language.Resources.ESE_ProdAdd_viewWEIGHT_UNIT + "]");
						error_str = "error";
					}
					*/
					try
					{
						int.Parse(data.Rows[i][10].ToString().Trim());    //인트 체크
					}
					catch
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_IntInput + "[" + comm_global.Language.Resources.ESE_ProdAdd_EXPIRATION + "]");
						error_str = "error";
					}

					string str_queryErr = "";
					//바코드 중복 체크
					string chkQuery = " SELECT COUNT(*) as cnt FROM stc_goods WHERE ESE_CODE = '" + eseCode + "' AND BARCODE = '" + data.Rows[i][0].ToString().Trim() + "' ";
					if(getQueryCnt(chkQuery,out str_queryErr) > 0 )
					{
						model.errList.Add("[" + i + "] - " + comm_global.Language.Resources.Script_OverLappedProd);
						error_str = "error";
					}

				}
			}

			if (error_str == "error")
			{
				model.result = false;
				return model;
			}


			string exeQueryStr = "";
			List<string> linqList = new List<string>();
			string tmpQuery = "";

			exeQueryStr = " INSERT INTO stc_goods (EST_CODE, ESE_CODE, BARCODE, PRODUCT_NAME, PRODUCT_NAME_KR, PRODUCT_NAME_CN, PRODUCT_NAME_EN ,SKU, BRAND, UNIT_WEIGHT, WEIGHT_UNIT, STANDARD, EXPIRATION, ORIGIN, INGREDIENT, SPEC, SALE_SITE_URL, PRODUCT_IMAGE ) VALUES ";

			if (data != null && data.Rows.Count != 0)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{

					//  0,1 필수 :: 7, 8 더블 :: 10 인트
					tmpQuery = "( '"+ estCode + "', " + "'" + eseCode + "'";
					tmpQuery += ", '" + data.Rows[i][0].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][1].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][2].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][3].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][4].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][5].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][6].ToString().Trim() + "'";
					tmpQuery += ", " + data.Rows[i][7].ToString().Trim();
					tmpQuery += ", '" + data.Rows[i][8].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][9].ToString().Trim() + "'";
					tmpQuery += ", " + data.Rows[i][10].ToString().Trim();
					tmpQuery += ", '" + data.Rows[i][11].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][12].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][13].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][14].ToString().Trim() + "'";
					tmpQuery += ", '" + data.Rows[i][15].ToString().Trim() + "' )";
					
					linqList.Add(tmpQuery);
				}
			}

			exeQueryStr += string.Join(",", linqList);
			model.result = exeQuery(exeQueryStr, out error_str);

			return model;
		}







	}
}
 