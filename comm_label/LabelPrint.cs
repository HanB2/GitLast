using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;
using BarcodeLib;
using System.IO;

namespace comm_label
{
	class LabelPrint
	{
		/*

		// 운송장을 PDF 파일로 생성한다
		static public bool CreatePDF(ref MemoryStream ms, string font_base_path
					, string image_base_path
					, string[] houseNoList
					, string RAINBOWCODE
					, string AGENTCODE
					, out string errorStr
					)
		{
			string m_errorStr = errorStr = "";

			iTextSharp.text.io.StreamUtil.AddToResourceSearch("/iTextAsian.dll");  // 한국어, 중국어 등을 표시하기 위해서
																				   //iTextSharp.text.io.StreamUtil.AddToResourceSearch("/iTextAsianCmaps.dll");

			Document document = new Document();
			PdfWriter writer = PdfWriter.GetInstance(document, ms);
			writer.CloseStream = false;  // 웹브라우저에서 바로 열리게

			document.Open();



			// 송장 데이터를 가져온다
			List<InvoiceModels> INVOICE_LIST = new List<InvoiceModels>();
			for (int i = 0; i < houseNoList.Length; i++)
			{
				InvoiceModels INVOICE = InvoiceDatabase.GetInvoiceData(RAINBOWCODE, AGENTCODE, houseNoList[i]);
				if (INVOICE == null || INVOICE.INVOICENO.Length == 0)
				{
					errorStr += houseNoList[i] + " : " + CKBIZ_Comm.Language.resLanguage.NO_DATA + "\n";
					continue;
				}

				INVOICE_LIST.Add(INVOICE);
			}
			if (errorStr.Length > 0)
				return false;



			// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
			Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(RAINBOWCODE, "_api_cn_yto_global_");
			if (SETTINGS_DIC == null)
				SETTINGS_DIC = new Dictionary<string, string>();

			string API_YTO_APP_TOKEN = (SETTINGS_DIC.ContainsKey("_api_cn_yto_global_app_token") ? SETTINGS_DIC["_api_cn_yto_global_app_token"] : "");
			string API_YTO_APP_KEY = (SETTINGS_DIC.ContainsKey("_api_cn_yto_global_app_key") ? SETTINGS_DIC["_api_cn_yto_global_app_key"] : "");

			// 라벨타입 설정값을 가져온다
			List<AgentMetaModels> META_LIST = AgentDatabase.GetSettingsList(AGENTCODE, "_label_type_");
			if (META_LIST == null)
				META_LIST = new List<AgentMetaModels>();

			string packing_label_type = "PACKING_TYPE1";  // 팩킹리스트
			string std_label_type = "STD_TYPE1";  // 시스템 자체 송장
			string krpost_label_type = "KRPOST_TYPE1";  // 한국 우체국 송장
			string krcj_label_type = "KRCJ_TYPE1";  // 한국 CJ대한통운 송장
			string cnytog_label_type = "CNYTOG_TYPE1";  // 중국 YTO Global 송장
														//string jpyamato_label_type = "JPYMT_TYPE1";  // 일본 YAMATO 송장
			string jpqxp_label_type = "JPQXP_TYPE1";  // 일본 QXPRESS 송장
			string jppost_label_type = "JPPOST_TYPE1";  // 일본우편(Japan Post) 송장
			string cnapl_label_type = "CNAPL_TYPE1";  // 중국 AIRPORTLOG 송장
			for (int i = 0; i < META_LIST.Count; i++)
			{
				if (META_LIST[i].META_KEY == "_label_type_packing")  // 팩킹리스트
					packing_label_type = META_LIST[i].META_VALUE;
				else if (META_LIST[i].META_KEY == "_label_type_std")  // 시스템 자체 송장
					std_label_type = META_LIST[i].META_VALUE;
				else if (META_LIST[i].META_KEY == "_label_type_krpost")  // 한국 우체국 송장
					krpost_label_type = META_LIST[i].META_VALUE;
				else if (META_LIST[i].META_KEY == "_label_type_krcj")  // 한국 CJ대한통운 송장
					krcj_label_type = META_LIST[i].META_VALUE;
				else if (META_LIST[i].META_KEY == "_label_type_cnytog")  // 중국 YTO Global 송장
					cnytog_label_type = META_LIST[i].META_VALUE;
				//else if (META_LIST[i].META_KEY == "_label_type_jpyamato")  // 일본 YAMATO 송장
				//    jpyamato_label_type = META_LIST[i].META_VALUE;
				else if (META_LIST[i].META_KEY == "_label_type_jpqxp")  // 일본 QXPRESS 송장
					jpqxp_label_type = META_LIST[i].META_VALUE;
				else if (META_LIST[i].META_KEY == "_label_type_jppost")  // 일본우편(Japan Post) 송장
					jppost_label_type = META_LIST[i].META_VALUE;
				else if (META_LIST[i].META_KEY == "_label_type_cnapl")  // 중국 AIRPORTLOG 송장
					cnapl_label_type = META_LIST[i].META_VALUE;
			}



			// 한국 우체국 송장이면서 A4 용지에 4장씩 출력하는 경우
			List<InvoiceModels> KRPOST_LIST = INVOICE_LIST.Where(m => m.NATION_CODE == "KR" && m.DELV_COM == "KR_POST").ToList();
			if (KRPOST_LIST != null && KRPOST_LIST.Count > 0 && krpost_label_type == "KRPOST_TYPE2")
			{
				if (KRPOST_LIST.Count != INVOICE_LIST.Count)
				{
					errorStr = "한국 우체국 송장 라벨타입이 A4(297mm x 210mm, 4장)인 경우는 한국 우체국 송장만 선택해야 합니다.";
					return false;
				}

				string fontfile = System.IO.Path.Combine(font_base_path, "H2GTRM.TTF");
				string logofile = System.IO.Path.Combine(image_base_path, "kr_ems.jpg");

				// PDF 파일 생성하기
				int div_num = 1;
				for (int i = 0; i < KRPOST_LIST.Count; i++)
				{
					InvoiceModels INVOICE = KRPOST_LIST[i];

					if (div_num == 1)
					{
						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(297), Utilities.MillimetersToPoints(210)));
						document.NewPage();
					}

					PdfContentByte over = writer.DirectContent;
					PrintLabel_KR.CreatePdfPage_iTextSharp_Post(over, INVOICE, fontfile, logofile, div_num);

					div_num++;
					if (div_num > 4)
						div_num = 1;
				}

				document.Close();
				//writer.Close();

				return true;
			}






			// 일본 QXPRESS 송장이면서 A4 용지에 24장씩 출력하는 경우
			List<InvoiceModels> JPQXP_LIST = INVOICE_LIST.Where(m => m.NATION_CODE == "JP" && m.DELV_COM == "JP_QXP").ToList();
			if (JPQXP_LIST != null && JPQXP_LIST.Count > 0 && jpqxp_label_type == "JPQXP_TYPE1")
			{
				if (JPQXP_LIST.Count != INVOICE_LIST.Count)
				{
					errorStr = "일본 QXPRESS 송장 라벨타입이 A4(210mm x 297mm, 24장)인 경우는 일본 QXPRESS 송장만 선택해야 합니다.";
					return false;
				}

				string fontfile = System.IO.Path.Combine(font_base_path, "NotoSerifCJKjp-Regular.otf");

				// PDF 파일 생성하기
				int div_num = 1;
				for (int i = 0; i < JPQXP_LIST.Count; i++)
				{
					InvoiceModels INVOICE = JPQXP_LIST[i];

					if (div_num == 1)
					{
						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(210), Utilities.MillimetersToPoints(297)));
						document.NewPage();
					}

					PdfContentByte over = writer.DirectContent;
					PrintLabel_JP.CreatePdfPage_iTextSharp_Qxpress(over, INVOICE, fontfile, div_num, (i + 1));

					div_num++;
					if (div_num > 24)
						div_num = 1;
				}

				document.Close();
				//writer.Close();

				return true;
			}



			// 중국 AIRPORTLOG 송장이면서 A4 용징 2장씩 출력하는 경우
			List<InvoiceModels> CNAPL_LIST = INVOICE_LIST.Where(m => m.NATION_CODE == "CN" && m.DELV_COM == "CN_AIRPORTLOG").ToList();
			if (CNAPL_LIST != null && CNAPL_LIST.Count > 0 && cnapl_label_type == "CNAPL_TYPE2")
			{
				if (CNAPL_LIST.Count != INVOICE_LIST.Count)
				{
					errorStr = "중국 AIRPORTLOG 송장 라벨타입이 A4(210mm x 297mm, 2장)인 경우는 중국 AIRPORTLOG 송장만 선택해야 합니다.";
					return false;
				}

				string fontfile = System.IO.Path.Combine(font_base_path, "simhei.ttf");
				//string fontfile = System.IO.Path.Combine(font_base_path, "msyh.ttc");

				// PDF 파일 생성하기
				int div_num = 1;
				for (int i = 0; i < CNAPL_LIST.Count; i++)
				{
					InvoiceModels INVOICE = CNAPL_LIST[i];

					if (div_num == 1)
					{
						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(210), Utilities.MillimetersToPoints(297)));
						document.NewPage();
					}

					PdfContentByte over = writer.DirectContent;
					//PrintLabel_CN.CreatePdfPage_iTextSharp_AIRPORTLOG_Img(over, INVOICE, fontfile, cnapl_label_type, div_num);
					PrintLabel_CN.CreatePdfPage_iTextSharp_AIRPORTLOG(over, INVOICE, fontfile, cnapl_label_type, div_num);

					div_num++;
					if (div_num > 2)
						div_num = 1;
				}

				document.Close();
				//writer.Close();

				return true;
			}



			// PDF 파일 생성하기
			for (int i = 0; i < INVOICE_LIST.Count; i++)
			{
				InvoiceModels INVOICE = INVOICE_LIST[i];



				if (INVOICE.NATION_CODE == "KR")  // 배송국가 : 한국
				{
					string fontfile = System.IO.Path.Combine(font_base_path, "H2GTRM.TTF");
					string logofile = System.IO.Path.Combine(image_base_path, "kr_ems.jpg");

					if (INVOICE.DELV_COM == "KR_POST" && krpost_label_type == "KRPOST_TYPE1")  // 한국 우체국 Label : 150mm x 100mm
					{
						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(CommSettings.LABEL_HEIGHT_SHORT), Utilities.MillimetersToPoints(CommSettings.LABEL_WIDTH)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						PrintLabel_KR.CreatePdfPage_iTextSharp_Post(over, INVOICE, fontfile, logofile);
						//PrintLabel_KR.CreatePdfPage_iTextSharp_Post(over, INVOICE);
					}
					else if (INVOICE.DELV_COM == "KR_CJ" && krcj_label_type == "KRCJ_TYPE1")  // 한국 CJ대한통운 Label : 200mm x 100mm
					{
						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(CommSettings.LABEL_HEIGHT_KRCJ), Utilities.MillimetersToPoints(CommSettings.LABEL_WIDTH_KRCJ)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						PrintLabel_KR.CreatePdfPage_iTextSharp_CJ(over, INVOICE, fontfile);
						//PrintLabel_KR.CreatePdfPage_iTextSharp_CJ(over, INVOICE);
					}
					else if (INVOICE.DELV_COM == "KR_CJ" && krcj_label_type == "KRCJ_TYPE2")  // 한국 CJ대한통운 Label : 180mm x 100mm
					{
						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(180), Utilities.MillimetersToPoints(100)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						PrintLabel_KR.CreatePdfPage_iTextSharp_CJ_180(over, INVOICE, fontfile);
					}
					else  // 시스템 기본 송장
					{
						fontfile = System.IO.Path.Combine(font_base_path, "NotoSans-Regular.ttf");
						logofile = "";

						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(CommSettings.LABEL_WIDTH), Utilities.MillimetersToPoints(CommSettings.LABEL_HEIGHT_SHORT)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						CreatePdfPage_iTextSharp_Comm(over, INVOICE, fontfile, logofile);
					}
				}
				else if (INVOICE.NATION_CODE == "CN")  // 배송국가 : 중국
				{
					if (INVOICE.DELV_COM == "CN_YTO_GLOBAL" && cnytog_label_type == "CNYTOG_TYPE1")  // 중국YTO Global Label : 100mm x 180mm
					{
						string fontfile = System.IO.Path.Combine(font_base_path, "simhei.ttf");
						//string fontfile = System.IO.Path.Combine(font_base_path, "msyh.ttc");

						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(100), Utilities.MillimetersToPoints(180)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						PrintLabel_CN.CreatePdfPage_iTextSharp_YtoGlobal(over, INVOICE, fontfile);



						//document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(100), Utilities.MillimetersToPoints(180)));
						//document.NewPage();

						//PdfContentByte over = writer.DirectContent;
						//if (!PrintLabel_CN.CreatePdfPage_iTextSharp_YtoGlobal_Img(over, INVOICE.RAINBOWCODE, INVOICE.INVOICENO, API_YTO_APP_TOKEN, API_YTO_APP_KEY))
						//{
						//    string fontfile = System.IO.Path.Combine(font_base_path, "simhei.ttf");
						//    //string fontfile = System.IO.Path.Combine(font_base_path, "msyh.ttc");

						//    PrintLabel_CN.CreatePdfPage_iTextSharp_YtoGlobal(over, INVOICE, fontfile);
						//}
					}
					else if (INVOICE.DELV_COM == "CN_AIRPORTLOG" && cnapl_label_type == "CNAPL_TYPE1")  // 중국 AIRPORTLOG Label : 150mm x 100mm
					{
						string fontfile = System.IO.Path.Combine(font_base_path, "simhei.ttf");
						//string fontfile = System.IO.Path.Combine(font_base_path, "msyh.ttc");

						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(150), Utilities.MillimetersToPoints(100)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						//PrintLabel_CN.CreatePdfPage_iTextSharp_AIRPORTLOG_Img(over, INVOICE, fontfile, cnapl_label_type, 0);
						PrintLabel_CN.CreatePdfPage_iTextSharp_AIRPORTLOG(over, INVOICE, fontfile, cnapl_label_type, 0);
					}
					else
					{
						string fontfile = System.IO.Path.Combine(font_base_path, "simhei.ttf");
						string logofile = System.IO.Path.Combine(image_base_path, "AirportLog.jpg");

						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(CommSettings.LABEL_WIDTH), Utilities.MillimetersToPoints(CommSettings.LABEL_HEIGHT_SHORT)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						PrintLabel_CN.CreatePdfPage_iTextSharp_Comm(over, INVOICE, fontfile, logofile);
					}
				}
				else if (INVOICE.NATION_CODE == "JP")  // 배송국가 : 일본
				{
					if (INVOICE.DELV_COM == "JP_QXP" && jpqxp_label_type == "JPQXP_TYPE2")  // QXPRESS Label : 64mm x 34mm
					{
						string fontfile = System.IO.Path.Combine(font_base_path, "NotoSerifCJKjp-Regular.otf");

						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(64), Utilities.MillimetersToPoints(34)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						PrintLabel_JP.CreatePdfPage_iTextSharp_Qxpress(over, INVOICE, fontfile, 0, (i + 1));
					}
					else if (INVOICE.DELV_COM == "JP_POST" && jppost_label_type == "JPPOST_TYPE1")  // 일본우편(Japan Post) Label : 100mm x 190mm
					{
						string fontfile = System.IO.Path.Combine(font_base_path, "NotoSerifCJKjp-Regular.otf");

						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(100), Utilities.MillimetersToPoints(190)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						PrintLabel_JP.CreatePdfPage_iTextSharp_JapanPost(over, INVOICE, fontfile);
					}
					else if (INVOICE.DELV_COM == "JP_POST" && jppost_label_type == "JPPOST_TYPE2")  // 일본우편(Japan Post) Label : 100mm x 150mm
					{
						string fontfile = System.IO.Path.Combine(font_base_path, "NotoSerifCJKjp-Regular.otf");

						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(100), Utilities.MillimetersToPoints(150)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						//PrintLabel_JP.CreatePdfPage_iTextSharp_JapanPost_150(over, INVOICE, fontfile);
						PrintLabel_JP.CreatePdfPage_iTextSharp_JapanPost_150_Type2(over, INVOICE, fontfile);
					}
					else
					{
						string fontfile = System.IO.Path.Combine(font_base_path, "NotoSerifCJKjp-Regular.otf");
						string logofile = "";

						document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(CommSettings.LABEL_WIDTH), Utilities.MillimetersToPoints(CommSettings.LABEL_HEIGHT_SHORT)));
						document.NewPage();

						PdfContentByte over = writer.DirectContent;
						CreatePdfPage_iTextSharp_Comm(over, INVOICE, fontfile, logofile);
					}
				}
				else  // 기타 배송국가
				{
					string fontfile = System.IO.Path.Combine(font_base_path, "NotoSans-Regular.ttf");
					string logofile = "";

					document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Utilities.MillimetersToPoints(CommSettings.LABEL_WIDTH), Utilities.MillimetersToPoints(CommSettings.LABEL_HEIGHT_SHORT)));
					document.NewPage();

					PdfContentByte over = writer.DirectContent;
					CreatePdfPage_iTextSharp_Comm(over, INVOICE, fontfile, logofile);
				}
			}

			document.Close();
			//writer.Close();

			return true;
		}


	*/
	}
}
