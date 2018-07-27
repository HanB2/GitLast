using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using comm_dbconn;
using comm_delvapi.Model;
using comm_global;
using comm_model;

using System.Text.RegularExpressions;
using comm_delvapi.DB;

namespace comm_delvapi.Class
{
	public class API_DELV
	{

		// 2018-02-22 jsy : 현지 배송번호 가져오기
		public static OrdMaster GetLocalDeliveryNoOnes( OrdMaster model, ref string error_str )
		{
			error_str = "";
			bool CHK_KR = false;
			string DELV_COM = "";
			string NATION_CODE = model.NATION_CODE;


			// 한국 배송번호 가져오기
			if (NATION_CODE == "KR")
			{
				CHK_KR = GlobalFunction.GetBool(SettingsDatabase.GetSettings(model.ESE_CODE, "_upload_delvno_autoset_kr"));
				DELV_COM = SettingsDatabase.GetSettings(model.ESE_CODE, "_upload_delvno_autoset_kr_id");

				if (CHK_KR && DELV_COM.Length > 0)
				{
					if (DELV_COM == "KR_CJ" || DELV_COM == "KR_POST")  // 우체국 또는 CJ대한통운
					{
						string error2;
						string DELVNO = ConfigDatabase.GetInvoiceNoOnes(model.EST_CODE, out error2, DELV_COM);
						if (DELVNO == null)
						{
							error_str += error2 + "\n";
						}
						else
						{
							model.DELVNO = DELVNO;
							model.DELV_COM = DELV_COM;
						}
					}
					else
					{
						error_str += DELV_COM + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
					}
				}

			}

			// 말레이시아 배송번호 가져오기
			if (NATION_CODE == "MY")
			{
				CHK_KR = GlobalFunction.GetBool(ConfigDatabase.GetSettings(model.ESE_CODE, "_upload_delvno_autoset_my"));
				DELV_COM = ConfigDatabase.GetSettings(model.ESE_CODE, "_upload_delvno_autoset_my_id");
				if (CHK_KR && DELV_COM.Length > 0)
				{
					if (DELV_COM == "MY_DDEX" || DELV_COM == "MY_ABX")  // DD Express 또는 ABX Express
					{
						string error2;
						string DELVNO = ConfigDatabase.GetInvoiceNoOnes(model.EST_CODE, out error2, DELV_COM);
						if (DELVNO == null )
						{
							error_str += error2 + "\n";
						}
						else
						{
							model.DELVNO = DELVNO;
							model.DELV_COM = DELV_COM;
						}
					}
					else
					{
						error_str += DELV_COM + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
					}
				}
			}

			/*
			// 중국 배송번호 가져오기
			if (NATION_CODE == "CN")
			{
				CHK_KR = GlobalFunction.GetBool(ConfigDatabase.GetSettings(model.ESE_CODE, "_upload_delvno_autoset_cn"));
				DELV_COM = ConfigDatabase.GetSettings(model.ESE_CODE, "_upload_delvno_autoset_cn_id");
				if (CHK_KR && DELV_COM.Length > 0)
				{
					if (DELV_COM == "CN_YTO_GLOBAL")  // 중국YTO Global 송장번호 가져오기
					{
						// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
						Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(model.EST_CODE, "_api_cn_yto_global_");
						if (SETTINGS_DIC == null)
							SETTINGS_DIC = new Dictionary<string, string>();

						string API_YTO_APP_TOKEN = (SETTINGS_DIC.ContainsKey("_api_cn_yto_global_app_token") ? SETTINGS_DIC["_api_cn_yto_global_app_token"] : "");
						string API_YTO_APP_KEY = (SETTINGS_DIC.ContainsKey("_api_cn_yto_global_app_key") ? SETTINGS_DIC["_api_cn_yto_global_app_key"] : "");

						string error2 = "";
						string DELVNO = GetYtoGlobalExpNo_One(API_YTO_APP_TOKEN, API_YTO_APP_KEY, ref model, ref error2);

						if (error2.Length > 0)
							error_str += error2 + "\n";

						model.DELVNO = DELVNO;
						model.DELV_COM = DELV_COM;
					}
					else if (DELV_COM == "CN_YTO_LOCAL")  // 중국YTO 내륙 송장번호 가져오기
					{
						string error2 = "";
						string get_delvno_cnt = CommApiYtoLocal.GetYTOLocalWaybillNo(ref model, ref error2, "asd");

						if (error2.Length > 0)
							error_str += error2 + "\n";

						model.DELVNO = DELVNO;
						model.DELV_COM = DELV_COM;
					}
					else if (DELV_COM == "CN_AIRPORTLOG")  // 중국 AirportLog 송장번호 가져오기
					{
						string error2 = "";
						string[] DELVNO_LIST = null;
						if (EST_CODE == "00017")  // ZHENHONG 인 경우 ==> AirportLog 송장번호를 미리 생성해놓고 랜덤하게 설정한다
							DELVNO_LIST = ConfigDatabase.GetInvoiceNo_AirportLogUsed(EST_CODE, CN_LIST.Count, out error2);
						else
							DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, CN_LIST.Count, out error2, _upload_delvno_autoset_cn_id);

						if (DELVNO_LIST == null || DELVNO_LIST.Length != CN_LIST.Count)
						{
							error_str += error2 + "\n";
						}
						else
						{
							int index = 0;
							for (int i = 0; i < CN_LIST.Count; i++)
							{
								int cn_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == CN_LIST[i].WAYBILLNO);
								if (cn_index >= 0)
								{
									INVOICE_LIST[cn_index].DELVNO = DELVNO_LIST[index++];
									INVOICE_LIST[cn_index].DELV_COM = _upload_delvno_autoset_cn_id;

									// 가져온 택배번호 업데이트
									string sql3 = "update INVOICE set "
												+ string.Format("DELVNO='{0}'", INVOICE_LIST[cn_index].DELVNO)
												+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[cn_index].DELV_COM)
												+ string.Format(" where EST_CODE='{0}'", EST_CODE)
												+ string.Format(" and WAYBILLNO='{0}'", CN_LIST[i].WAYBILLNO)
												+ "";
									sql_list.Add(sql3);
								}
							}
						}
					}
					else
					{
						error_str += DELV_COM + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
					}
				}
			}




			



			// 일본 배송번호 가져오기
			List<OrdMaster> JP_LIST = INVOICE_LIST.FindAll(m => m.NATION_CODE == "JP" && m.DELVNO.Length == 0 && m.DELV_COM.Length == 0);
			bool _upload_delvno_autoset_jp = GlobalFunction.GetBool(ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_jp"));
			string _upload_delvno_autoset_jp_id = ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_jp_id");
			if ((JP_LIST != null && JP_LIST.Count > 0) && _upload_delvno_autoset_jp && _upload_delvno_autoset_jp_id.Length > 0)
			{
				if (_upload_delvno_autoset_jp_id == "JP_QXP")  // 일본 Qxpress 송장번호 가져오기
				{
					// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
					Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(EST_CODE, "_api_qxp_");
					if (SETTINGS_DIC == null)
						SETTINGS_DIC = new Dictionary<string, string>();

					string API_QXP_ACCOUNT_ID = (SETTINGS_DIC.ContainsKey("_api_qxp_account_id") ? SETTINGS_DIC["_api_qxp_account_id"] : "");  // Account ID
					string API_QXP_API_KEY = (SETTINGS_DIC.ContainsKey("_api_qxp_api_key") ? SETTINGS_DIC["_api_qxp_api_key"] : "");  // API KEY

					string error2 = "";
					int get_delvno_cnt = GetQxpressExpNo_Multi(API_QXP_ACCOUNT_ID, API_QXP_API_KEY, ref JP_LIST, ref error2);

					if (error2.Length > 0)
						error_str += error2 + "\n";

					// 가져온 택배번호 업데이트
					for (int k = 0; k < JP_LIST.Count; k++)
					{
						if (JP_LIST[k].DELVNO.Length == 0)
							continue;

						string sql3 = "update INVOICE set "
									+ string.Format("DELVNO='{0}'", JP_LIST[k].DELVNO)
									+ string.Format(", DELV_COM='{0}'", _upload_delvno_autoset_jp_id)
									+ string.Format(" where EST_CODE='{0}'", EST_CODE)
									+ string.Format(" and WAYBILLNO='{0}'", JP_LIST[k].WAYBILLNO)
									+ "";
						sql_list.Add(sql3);
					}
				}
				else if (_upload_delvno_autoset_jp_id == "JP_POST")  // 일본 JapanPost 송장번호 가져오기
				{
					// 2018-06-27 jsy : 일반/다이비끼 구분하여 송장번호를 가져온다
					List<OrdMaster> JP_NORMAL_LIST = JP_LIST.FindAll(m => m.USER2.Length == 0);  // 일반
					if (JP_NORMAL_LIST != null && JP_NORMAL_LIST.Count > 0)
					{
						string error2;
						string[] DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, JP_NORMAL_LIST.Count, out error2, _upload_delvno_autoset_jp_id, "N");
						if (DELVNO_LIST == null || DELVNO_LIST.Length != JP_NORMAL_LIST.Count)
						{
							error_str += error2 + "\n";
						}
						else
						{
							int index = 0;
							for (int i = 0; i < JP_NORMAL_LIST.Count; i++)
							{
								int jp_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == JP_NORMAL_LIST[i].WAYBILLNO);
								if (jp_index >= 0)
								{
									INVOICE_LIST[jp_index].DELVNO = DELVNO_LIST[index++];
									INVOICE_LIST[jp_index].DELV_COM = _upload_delvno_autoset_jp_id;

									// 가져온 택배번호 업데이트
									string sql3 = "update INVOICE set "
												+ string.Format("DELVNO='{0}'", INVOICE_LIST[jp_index].DELVNO)
												+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[jp_index].DELV_COM)
												+ string.Format(" where EST_CODE='{0}'", EST_CODE)
												+ string.Format(" and WAYBILLNO='{0}'", JP_NORMAL_LIST[i].WAYBILLNO)
												+ "";
									sql_list.Add(sql3);
								}
							}
						}
					}

					List<OrdMaster> JP_DAIBIKI_LIST = JP_LIST.FindAll(m => m.USER2.Length > 0);  // 다이비끼
					if (JP_DAIBIKI_LIST != null && JP_DAIBIKI_LIST.Count > 0)
					{
						string error2;
						string[] DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, JP_DAIBIKI_LIST.Count, out error2, _upload_delvno_autoset_jp_id, "D");
						if (DELVNO_LIST == null || DELVNO_LIST.Length != JP_DAIBIKI_LIST.Count)
						{
							error_str += error2 + "\n";
						}
						else
						{
							int index = 0;
							for (int i = 0; i < JP_DAIBIKI_LIST.Count; i++)
							{
								int jp_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == JP_DAIBIKI_LIST[i].WAYBILLNO);
								if (jp_index >= 0)
								{
									INVOICE_LIST[jp_index].DELVNO = DELVNO_LIST[index++];
									INVOICE_LIST[jp_index].DELV_COM = _upload_delvno_autoset_jp_id;

									// 가져온 택배번호 업데이트
									string sql3 = "update INVOICE set "
												+ string.Format("DELVNO='{0}'", INVOICE_LIST[jp_index].DELVNO)
												+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[jp_index].DELV_COM)
												+ string.Format(" where EST_CODE='{0}'", EST_CODE)
												+ string.Format(" and WAYBILLNO='{0}'", JP_DAIBIKI_LIST[i].WAYBILLNO)
												+ "";
									sql_list.Add(sql3);
								}
							}
						}
					}
				}
				else if (_upload_delvno_autoset_jp_id == "JP_SAGAWA")  // 일본 SAGAWA 송장번호 가져오기
				{
					string error2;
					string[] DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, JP_LIST.Count, out error2, _upload_delvno_autoset_jp_id);
					if (DELVNO_LIST == null || DELVNO_LIST.Length != JP_LIST.Count)
					{
						error_str += error2 + "\n";
					}
					else
					{
						int index = 0;
						for (int i = 0; i < JP_LIST.Count; i++)
						{
							int jp_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == JP_LIST[i].WAYBILLNO);
							if (jp_index >= 0)
							{
								INVOICE_LIST[jp_index].DELVNO = DELVNO_LIST[index++];
								INVOICE_LIST[jp_index].DELV_COM = _upload_delvno_autoset_jp_id;

								// 가져온 택배번호 업데이트
								string sql3 = "update INVOICE set "
											+ string.Format("DELVNO='{0}'", INVOICE_LIST[jp_index].DELVNO)
											+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[jp_index].DELV_COM)
											+ string.Format(" where EST_CODE='{0}'", EST_CODE)
											+ string.Format(" and WAYBILLNO='{0}'", JP_LIST[i].WAYBILLNO)
											+ "";
								sql_list.Add(sql3);
							}
						}
					}
				}
				else
				{
					error_str += _upload_delvno_autoset_jp_id + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
				}
			}



			// 홍콩 배송번호 가져오기
			List<OrdMaster> HK_LIST = INVOICE_LIST.FindAll(m => m.NATION_CODE == "HK" && m.DELVNO.Length == 0 && m.DELV_COM.Length == 0);
			bool _upload_delvno_autoset_hk = GlobalFunction.GetBool(ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_hk"));
			string _upload_delvno_autoset_hk_id = ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_hk_id");
			if ((HK_LIST != null && HK_LIST.Count > 0) && _upload_delvno_autoset_hk && _upload_delvno_autoset_hk_id.Length > 0)
			{
				if (_upload_delvno_autoset_hk_id == "HK_QXP")  // 홍콩 Qxpress 송장번호 가져오기
				{
					// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
					Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(EST_CODE, "_api_qxp_");
					if (SETTINGS_DIC == null)
						SETTINGS_DIC = new Dictionary<string, string>();

					string API_QXP_ACCOUNT_ID = (SETTINGS_DIC.ContainsKey("_api_qxp_account_id") ? SETTINGS_DIC["_api_qxp_account_id"] : "");  // Account ID
					string API_QXP_API_KEY = (SETTINGS_DIC.ContainsKey("_api_qxp_api_key") ? SETTINGS_DIC["_api_qxp_api_key"] : "");  // API KEY

					string error2 = "";
					int get_delvno_cnt = GetQxpressExpNo_Multi(API_QXP_ACCOUNT_ID, API_QXP_API_KEY, ref HK_LIST, ref error2);

					if (error2.Length > 0)
						error_str += error2 + "\n";

					// 가져온 택배번호 업데이트
					for (int k = 0; k < HK_LIST.Count; k++)
					{
						if (HK_LIST[k].DELVNO.Length == 0)
							continue;

						string sql3 = "update INVOICE set "
									+ string.Format("DELVNO='{0}'", HK_LIST[k].DELVNO)
									+ string.Format(", DELV_COM='{0}'", _upload_delvno_autoset_hk_id)
									+ string.Format(" where EST_CODE='{0}'", EST_CODE)
									+ string.Format(" and WAYBILLNO='{0}'", HK_LIST[k].WAYBILLNO)
									+ "";
						sql_list.Add(sql3);
					}
				}
				else
				{
					error_str += _upload_delvno_autoset_hk_id + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
				}
			}



			// 싱가포르 배송번호 가져오기
			List<OrdMaster> SG_LIST = INVOICE_LIST.FindAll(m => m.NATION_CODE == "SG" && m.DELVNO.Length == 0 && m.DELV_COM.Length == 0);
			bool _upload_delvno_autoset_sg = GlobalFunction.GetBool(ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_sg"));
			string _upload_delvno_autoset_sg_id = ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_sg_id");
			if ((SG_LIST != null && SG_LIST.Count > 0) && _upload_delvno_autoset_sg && _upload_delvno_autoset_sg_id.Length > 0)
			{
				if (_upload_delvno_autoset_sg_id == "SG_QXP")  // 싱가포르 Qxpress 송장번호 가져오기
				{
					// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
					Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(EST_CODE, "_api_qxp_");
					if (SETTINGS_DIC == null)
						SETTINGS_DIC = new Dictionary<string, string>();

					string API_QXP_ACCOUNT_ID = (SETTINGS_DIC.ContainsKey("_api_qxp_account_id") ? SETTINGS_DIC["_api_qxp_account_id"] : "");  // Account ID
					string API_QXP_API_KEY = (SETTINGS_DIC.ContainsKey("_api_qxp_api_key") ? SETTINGS_DIC["_api_qxp_api_key"] : "");  // API KEY

					string error2 = "";
					int get_delvno_cnt = GetQxpressExpNo_Multi(API_QXP_ACCOUNT_ID, API_QXP_API_KEY, ref SG_LIST, ref error2);

					if (error2.Length > 0)
						error_str += error2 + "\n";

					// 가져온 택배번호 업데이트
					for (int k = 0; k < SG_LIST.Count; k++)
					{
						if (SG_LIST[k].DELVNO.Length == 0)
							continue;

						string sql3 = "update INVOICE set "
									+ string.Format("DELVNO='{0}'", SG_LIST[k].DELVNO)
									+ string.Format(", DELV_COM='{0}'", _upload_delvno_autoset_sg_id)
									+ string.Format(" where EST_CODE='{0}'", EST_CODE)
									+ string.Format(" and WAYBILLNO='{0}'", SG_LIST[k].WAYBILLNO)
									+ "";
						sql_list.Add(sql3);
					}
				}
				else
				{
					error_str += _upload_delvno_autoset_sg_id + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
				}
			}



			if (sql_list.Count > 0)
			{
				string error4 = "";
				if (!DatabaseConnection.ExcuteQueryMySQL(sql_list, out error4))
				{
					error_str += error4 + "\n";
				}
			}
			*/

			return model;
		}







		// 2018-02-22 jsy : 현지 배송번호 가져오기
		public static void GetLocalDeliveryNo(
					string EST_CODE
					, string ESE_CODE
					, ref List<OrdMaster> INVOICE_LIST
					, ref string error_str
					)
		{
			error_str = "";

			List<string> sql_list = new List<string>();



			// 한국 배송번호 가져오기
			List<OrdMaster> KR_LIST = INVOICE_LIST.FindAll(m => m.NATION_CODE == "KR");
			bool UPLOAD_DELVNO_AUTOSET_KR = GlobalFunction.GetBool(SettingsDatabase.GetSettings(EST_CODE, "_upload_delvno_autoset_kr"));
			string UPLOAD_DELVNO_AUTOSET_KR_COMID = SettingsDatabase.GetSettings(EST_CODE, "_upload_delvno_autoset_kr_id");
			if ((KR_LIST != null && KR_LIST.Count > 0) && UPLOAD_DELVNO_AUTOSET_KR && UPLOAD_DELVNO_AUTOSET_KR_COMID.Length > 0)
			{
				if (UPLOAD_DELVNO_AUTOSET_KR_COMID == "KR_CJ" || UPLOAD_DELVNO_AUTOSET_KR_COMID == "KR_POST")  // 우체국 또는 CJ대한통운
				{
					string error2;
					string[] DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, KR_LIST.Count, out error2, UPLOAD_DELVNO_AUTOSET_KR_COMID);
					if (DELVNO_LIST == null || DELVNO_LIST.Length != KR_LIST.Count)
					{
						error_str += error2 + "\n";
					}
					else
					{
						int index = 0;
						for (int i = 0; i < KR_LIST.Count; i++)
						{
							int kr_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == KR_LIST[i].WAYBILLNO);
							if (kr_index >= 0)
							{
								INVOICE_LIST[kr_index].DELVNO = DELVNO_LIST[index++];
								INVOICE_LIST[kr_index].DELV_COM = UPLOAD_DELVNO_AUTOSET_KR_COMID;

								// 가져온 택배번호 업데이트
								string sql3 = "update INVOICE set "
											+ string.Format("DELVNO='{0}'", INVOICE_LIST[kr_index].DELVNO)
											+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[kr_index].DELV_COM)
											+ string.Format(" where EST_CODE='{0}'", EST_CODE)
											+ string.Format(" and WAYBILLNO='{0}'", KR_LIST[i].WAYBILLNO)
											+ "";
								sql_list.Add(sql3);
							}
						}
					}
				}
				else
				{
					error_str += UPLOAD_DELVNO_AUTOSET_KR_COMID + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
				}
			}



			// 말레이시아 배송번호 가져오기
			List<OrdMaster> MY_LIST = INVOICE_LIST.FindAll(m => m.NATION_CODE == "MY");
			bool UPLOAD_DELVNO_AUTOSET_MY = GlobalFunction.GetBool(ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_my"));
			string UPLOAD_DELVNO_AUTOSET_MY_COMID = ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_my_id");
			if ((MY_LIST != null && MY_LIST.Count > 0) && UPLOAD_DELVNO_AUTOSET_MY && UPLOAD_DELVNO_AUTOSET_MY_COMID.Length > 0)
			{
				if (UPLOAD_DELVNO_AUTOSET_MY_COMID == "MY_DDEX" || UPLOAD_DELVNO_AUTOSET_MY_COMID == "MY_ABX")  // DD Express 또는 ABX Express
				{
					string error2;
					string[] DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, MY_LIST.Count, out error2, UPLOAD_DELVNO_AUTOSET_MY_COMID);
					if (DELVNO_LIST == null || DELVNO_LIST.Length != MY_LIST.Count)
					{
						error_str += error2 + "\n";
					}
					else
					{
						int index = 0;
						for (int i = 0; i < MY_LIST.Count; i++)
						{
							int my_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == MY_LIST[i].WAYBILLNO);
							if (my_index >= 0)
							{
								INVOICE_LIST[my_index].DELVNO = DELVNO_LIST[index++];
								INVOICE_LIST[my_index].DELV_COM = UPLOAD_DELVNO_AUTOSET_MY_COMID;

								// 가져온 택배번호 업데이트
								string sql3 = "update INVOICE set "
											+ string.Format("DELVNO='{0}'", INVOICE_LIST[my_index].DELVNO)
											+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[my_index].DELV_COM)
											+ string.Format(" where EST_CODE='{0}'", EST_CODE)
											+ string.Format(" and WAYBILLNO='{0}'", MY_LIST[i].WAYBILLNO)
											+ "";
								sql_list.Add(sql3);
							}
						}
					}
				}
				else
				{
					error_str += UPLOAD_DELVNO_AUTOSET_MY_COMID + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
				}
			}



			// 중국 배송번호 가져오기
			List<OrdMaster> CN_LIST = INVOICE_LIST.FindAll(m => m.NATION_CODE == "CN");
			bool _upload_delvno_autoset_cn = GlobalFunction.GetBool(ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_cn"));
			string _upload_delvno_autoset_cn_id = ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_cn_id");
			if ((CN_LIST != null && CN_LIST.Count > 0) && _upload_delvno_autoset_cn && _upload_delvno_autoset_cn_id.Length > 0)
			{
				if (_upload_delvno_autoset_cn_id == "CN_YTO_GLOBAL")  // 중국YTO Global 송장번호 가져오기
				{
					// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
					Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(EST_CODE, "_api_cn_yto_global_");
					if (SETTINGS_DIC == null)
						SETTINGS_DIC = new Dictionary<string, string>();

					string API_YTO_APP_TOKEN = (SETTINGS_DIC.ContainsKey("_api_cn_yto_global_app_token") ? SETTINGS_DIC["_api_cn_yto_global_app_token"] : "");
					string API_YTO_APP_KEY = (SETTINGS_DIC.ContainsKey("_api_cn_yto_global_app_key") ? SETTINGS_DIC["_api_cn_yto_global_app_key"] : "");

					string error2 = "";
					int get_delvno_cnt = GetYtoGlobalExpNo_Multi(API_YTO_APP_TOKEN, API_YTO_APP_KEY, ref CN_LIST, ref error2);

					if (error2.Length > 0)
						error_str += error2 + "\n";

					// 가져온 택배번호 업데이트
					for (int k = 0; k < CN_LIST.Count; k++)
					{
						if (CN_LIST[k].DELVNO.Length == 0)
							continue;

						string sql3 = "update INVOICE set "
									+ string.Format("DELVNO='{0}'", CN_LIST[k].DELVNO)
									+ string.Format(", DELV_COM='{0}'", _upload_delvno_autoset_cn_id)
									+ string.Format(", SDATA1='{0}'", CN_LIST[k].SDATA1)  // 지역코드
									+ string.Format(" where EST_CODE='{0}'", EST_CODE)
									+ string.Format(" and WAYBILLNO='{0}'", CN_LIST[k].WAYBILLNO)
									+ "";
						sql_list.Add(sql3);
					}
				}
				else if (_upload_delvno_autoset_cn_id == "CN_YTO_LOCAL")  // 중국YTO 내륙 송장번호 가져오기
				{
					string error2 = "";
					int get_delvno_cnt = CommApiYtoLocal.GetYTOLocalWaybillNo_Multi(ref CN_LIST, ref error2);

					if (error2.Length > 0)
						error_str += error2 + "\n";

					// 가져온 택배번호 업데이트
					for (int k = 0; k < CN_LIST.Count; k++)
					{
						if (CN_LIST[k].DELVNO.Length == 0)
							continue;

						string sql3 = "update invoice set "
									+ string.Format("DELVNO='{0}'", CN_LIST[k].DELVNO)
									+ string.Format(", DELV_COM='{0}'", _upload_delvno_autoset_cn_id)
									+ string.Format(", SDATA1='{0}'", CN_LIST[k].SDATA1)
									+ string.Format(", SDATA2='{0}'", CN_LIST[k].SDATA2)
									+ string.Format(" where EST_CODE='{0}'", EST_CODE)
									+ string.Format(" and WAYBILLNO='{0}'", CN_LIST[k].WAYBILLNO)
									+ "";
						sql_list.Add(sql3);
					}
				}
				else if (_upload_delvno_autoset_cn_id == "CN_AIRPORTLOG")  // 중국 AirportLog 송장번호 가져오기
				{
					string error2 = "";
					string[] DELVNO_LIST = null;
					if (EST_CODE == "00017")  // ZHENHONG 인 경우 ==> AirportLog 송장번호를 미리 생성해놓고 랜덤하게 설정한다
						DELVNO_LIST = ConfigDatabase.GetInvoiceNo_AirportLogUsed(EST_CODE, CN_LIST.Count, out error2);
					else
						DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, CN_LIST.Count, out error2, _upload_delvno_autoset_cn_id);

					if (DELVNO_LIST == null || DELVNO_LIST.Length != CN_LIST.Count)
					{
						error_str += error2 + "\n";
					}
					else
					{
						int index = 0;
						for (int i = 0; i < CN_LIST.Count; i++)
						{
							int cn_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == CN_LIST[i].WAYBILLNO);
							if (cn_index >= 0)
							{
								INVOICE_LIST[cn_index].DELVNO = DELVNO_LIST[index++];
								INVOICE_LIST[cn_index].DELV_COM = _upload_delvno_autoset_cn_id;

								// 가져온 택배번호 업데이트
								string sql3 = "update INVOICE set "
											+ string.Format("DELVNO='{0}'", INVOICE_LIST[cn_index].DELVNO)
											+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[cn_index].DELV_COM)
											+ string.Format(" where EST_CODE='{0}'", EST_CODE)
											+ string.Format(" and WAYBILLNO='{0}'", CN_LIST[i].WAYBILLNO)
											+ "";
								sql_list.Add(sql3);
							}
						}
					}
				}
				else
				{
					error_str += _upload_delvno_autoset_cn_id + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
				}
			}



			// 일본 배송번호 가져오기
			List<OrdMaster> JP_LIST = INVOICE_LIST.FindAll(m => m.NATION_CODE == "JP" && m.DELVNO.Length == 0 && m.DELV_COM.Length == 0);
			bool _upload_delvno_autoset_jp = GlobalFunction.GetBool(ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_jp"));
			string _upload_delvno_autoset_jp_id = ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_jp_id");
			if ((JP_LIST != null && JP_LIST.Count > 0) && _upload_delvno_autoset_jp && _upload_delvno_autoset_jp_id.Length > 0)
			{
				if (_upload_delvno_autoset_jp_id == "JP_QXP")  // 일본 Qxpress 송장번호 가져오기
				{
					// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
					Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(EST_CODE, "_api_qxp_");
					if (SETTINGS_DIC == null)
						SETTINGS_DIC = new Dictionary<string, string>();

					string API_QXP_ACCOUNT_ID = (SETTINGS_DIC.ContainsKey("_api_qxp_account_id") ? SETTINGS_DIC["_api_qxp_account_id"] : "");  // Account ID
					string API_QXP_API_KEY = (SETTINGS_DIC.ContainsKey("_api_qxp_api_key") ? SETTINGS_DIC["_api_qxp_api_key"] : "");  // API KEY

					string error2 = "";
					int get_delvno_cnt = GetQxpressExpNo_Multi(API_QXP_ACCOUNT_ID, API_QXP_API_KEY, ref JP_LIST, ref error2);

					if (error2.Length > 0)
						error_str += error2 + "\n";

					// 가져온 택배번호 업데이트
					for (int k = 0; k < JP_LIST.Count; k++)
					{
						if (JP_LIST[k].DELVNO.Length == 0)
							continue;

						string sql3 = "update INVOICE set "
									+ string.Format("DELVNO='{0}'", JP_LIST[k].DELVNO)
									+ string.Format(", DELV_COM='{0}'", _upload_delvno_autoset_jp_id)
									+ string.Format(" where EST_CODE='{0}'", EST_CODE)
									+ string.Format(" and WAYBILLNO='{0}'", JP_LIST[k].WAYBILLNO)
									+ "";
						sql_list.Add(sql3);
					}
				}
				else if (_upload_delvno_autoset_jp_id == "JP_POST")  // 일본 JapanPost 송장번호 가져오기
				{
					// 2018-06-27 jsy : 일반/다이비끼 구분하여 송장번호를 가져온다
					List<OrdMaster> JP_NORMAL_LIST = JP_LIST.FindAll(m => m.USER2.Length == 0);  // 일반
					if (JP_NORMAL_LIST != null && JP_NORMAL_LIST.Count > 0)
					{
						string error2;
						string[] DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, JP_NORMAL_LIST.Count, out error2, _upload_delvno_autoset_jp_id, "N");
						if (DELVNO_LIST == null || DELVNO_LIST.Length != JP_NORMAL_LIST.Count)
						{
							error_str += error2 + "\n";
						}
						else
						{
							int index = 0;
							for (int i = 0; i < JP_NORMAL_LIST.Count; i++)
							{
								int jp_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == JP_NORMAL_LIST[i].WAYBILLNO);
								if (jp_index >= 0)
								{
									INVOICE_LIST[jp_index].DELVNO = DELVNO_LIST[index++];
									INVOICE_LIST[jp_index].DELV_COM = _upload_delvno_autoset_jp_id;

									// 가져온 택배번호 업데이트
									string sql3 = "update INVOICE set "
												+ string.Format("DELVNO='{0}'", INVOICE_LIST[jp_index].DELVNO)
												+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[jp_index].DELV_COM)
												+ string.Format(" where EST_CODE='{0}'", EST_CODE)
												+ string.Format(" and WAYBILLNO='{0}'", JP_NORMAL_LIST[i].WAYBILLNO)
												+ "";
									sql_list.Add(sql3);
								}
							}
						}
					}

					List<OrdMaster> JP_DAIBIKI_LIST = JP_LIST.FindAll(m => m.USER2.Length > 0);  // 다이비끼
					if (JP_DAIBIKI_LIST != null && JP_DAIBIKI_LIST.Count > 0)
					{
						string error2;
						string[] DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, JP_DAIBIKI_LIST.Count, out error2, _upload_delvno_autoset_jp_id, "D");
						if (DELVNO_LIST == null || DELVNO_LIST.Length != JP_DAIBIKI_LIST.Count)
						{
							error_str += error2 + "\n";
						}
						else
						{
							int index = 0;
							for (int i = 0; i < JP_DAIBIKI_LIST.Count; i++)
							{
								int jp_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == JP_DAIBIKI_LIST[i].WAYBILLNO);
								if (jp_index >= 0)
								{
									INVOICE_LIST[jp_index].DELVNO = DELVNO_LIST[index++];
									INVOICE_LIST[jp_index].DELV_COM = _upload_delvno_autoset_jp_id;

									// 가져온 택배번호 업데이트
									string sql3 = "update INVOICE set "
												+ string.Format("DELVNO='{0}'", INVOICE_LIST[jp_index].DELVNO)
												+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[jp_index].DELV_COM)
												+ string.Format(" where EST_CODE='{0}'", EST_CODE)
												+ string.Format(" and WAYBILLNO='{0}'", JP_DAIBIKI_LIST[i].WAYBILLNO)
												+ "";
									sql_list.Add(sql3);
								}
							}
						}
					}
				}
				else if (_upload_delvno_autoset_jp_id == "JP_SAGAWA")  // 일본 SAGAWA 송장번호 가져오기
				{
					string error2;
					string[] DELVNO_LIST = ConfigDatabase.GetInvoiceNo(EST_CODE, JP_LIST.Count, out error2, _upload_delvno_autoset_jp_id);
					if (DELVNO_LIST == null || DELVNO_LIST.Length != JP_LIST.Count)
					{
						error_str += error2 + "\n";
					}
					else
					{
						int index = 0;
						for (int i = 0; i < JP_LIST.Count; i++)
						{
							int jp_index = INVOICE_LIST.FindIndex(m => m.WAYBILLNO == JP_LIST[i].WAYBILLNO);
							if (jp_index >= 0)
							{
								INVOICE_LIST[jp_index].DELVNO = DELVNO_LIST[index++];
								INVOICE_LIST[jp_index].DELV_COM = _upload_delvno_autoset_jp_id;

								// 가져온 택배번호 업데이트
								string sql3 = "update INVOICE set "
											+ string.Format("DELVNO='{0}'", INVOICE_LIST[jp_index].DELVNO)
											+ string.Format(", DELV_COM='{0}'", INVOICE_LIST[jp_index].DELV_COM)
											+ string.Format(" where EST_CODE='{0}'", EST_CODE)
											+ string.Format(" and WAYBILLNO='{0}'", JP_LIST[i].WAYBILLNO)
											+ "";
								sql_list.Add(sql3);
							}
						}
					}
				}
				else
				{
					error_str += _upload_delvno_autoset_jp_id + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
				}
			}



			// 홍콩 배송번호 가져오기
			List<OrdMaster> HK_LIST = INVOICE_LIST.FindAll(m => m.NATION_CODE == "HK" && m.DELVNO.Length == 0 && m.DELV_COM.Length == 0);
			bool _upload_delvno_autoset_hk = GlobalFunction.GetBool(ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_hk"));
			string _upload_delvno_autoset_hk_id = ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_hk_id");
			if ((HK_LIST != null && HK_LIST.Count > 0) && _upload_delvno_autoset_hk && _upload_delvno_autoset_hk_id.Length > 0)
			{
				if (_upload_delvno_autoset_hk_id == "HK_QXP")  // 홍콩 Qxpress 송장번호 가져오기
				{
					// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
					Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(EST_CODE, "_api_qxp_");
					if (SETTINGS_DIC == null)
						SETTINGS_DIC = new Dictionary<string, string>();

					string API_QXP_ACCOUNT_ID = (SETTINGS_DIC.ContainsKey("_api_qxp_account_id") ? SETTINGS_DIC["_api_qxp_account_id"] : "");  // Account ID
					string API_QXP_API_KEY = (SETTINGS_DIC.ContainsKey("_api_qxp_api_key") ? SETTINGS_DIC["_api_qxp_api_key"] : "");  // API KEY

					string error2 = "";
					int get_delvno_cnt = GetQxpressExpNo_Multi(API_QXP_ACCOUNT_ID, API_QXP_API_KEY, ref HK_LIST, ref error2);

					if (error2.Length > 0)
						error_str += error2 + "\n";

					// 가져온 택배번호 업데이트
					for (int k = 0; k < HK_LIST.Count; k++)
					{
						if (HK_LIST[k].DELVNO.Length == 0)
							continue;

						string sql3 = "update INVOICE set "
									+ string.Format("DELVNO='{0}'", HK_LIST[k].DELVNO)
									+ string.Format(", DELV_COM='{0}'", _upload_delvno_autoset_hk_id)
									+ string.Format(" where EST_CODE='{0}'", EST_CODE)
									+ string.Format(" and WAYBILLNO='{0}'", HK_LIST[k].WAYBILLNO)
									+ "";
						sql_list.Add(sql3);
					}
				}
				else
				{
					error_str += _upload_delvno_autoset_hk_id + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
				}
			}



			// 싱가포르 배송번호 가져오기
			List<OrdMaster> SG_LIST = INVOICE_LIST.FindAll(m => m.NATION_CODE == "SG" && m.DELVNO.Length == 0 && m.DELV_COM.Length == 0);
			bool _upload_delvno_autoset_sg = GlobalFunction.GetBool(ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_sg"));
			string _upload_delvno_autoset_sg_id = ConfigDatabase.GetSettings(ESE_CODE, "_upload_delvno_autoset_sg_id");
			if ((SG_LIST != null && SG_LIST.Count > 0) && _upload_delvno_autoset_sg && _upload_delvno_autoset_sg_id.Length > 0)
			{
				if (_upload_delvno_autoset_sg_id == "SG_QXP")  // 싱가포르 Qxpress 송장번호 가져오기
				{
					// SETTINGS 테이블에서 API 연동에 필요한 key값을 가져온다
					Dictionary<string, string> SETTINGS_DIC = SettingsDatabase.GetSettingsDic(EST_CODE, "_api_qxp_");
					if (SETTINGS_DIC == null)
						SETTINGS_DIC = new Dictionary<string, string>();

					string API_QXP_ACCOUNT_ID = (SETTINGS_DIC.ContainsKey("_api_qxp_account_id") ? SETTINGS_DIC["_api_qxp_account_id"] : "");  // Account ID
					string API_QXP_API_KEY = (SETTINGS_DIC.ContainsKey("_api_qxp_api_key") ? SETTINGS_DIC["_api_qxp_api_key"] : "");  // API KEY

					string error2 = "";
					int get_delvno_cnt = GetQxpressExpNo_Multi(API_QXP_ACCOUNT_ID, API_QXP_API_KEY, ref SG_LIST, ref error2);

					if (error2.Length > 0)
						error_str += error2 + "\n";

					// 가져온 택배번호 업데이트
					for (int k = 0; k < SG_LIST.Count; k++)
					{
						if (SG_LIST[k].DELVNO.Length == 0)
							continue;

						string sql3 = "update INVOICE set "
									+ string.Format("DELVNO='{0}'", SG_LIST[k].DELVNO)
									+ string.Format(", DELV_COM='{0}'", _upload_delvno_autoset_sg_id)
									+ string.Format(" where EST_CODE='{0}'", EST_CODE)
									+ string.Format(" and WAYBILLNO='{0}'", SG_LIST[k].WAYBILLNO)
									+ "";
						sql_list.Add(sql3);
					}
				}
				else
				{
					error_str += _upload_delvno_autoset_sg_id + " : " + comm_global.Language.Resources.API_UNKNOWN_SHIPPING_COMPANY + "\n";
				}
			}



			if (sql_list.Count > 0)
			{
				string error4 = "";
				if (!DatabaseConnection.ExcuteQueryMySQL(sql_list, out error4))
				{
					error_str += error4 + "\n";
				}
			}

			return;
		}

		



		// 2018-01-18 jsy : 중국YTO Global 송장번호를 API를 사용하여 가져온다 ==> 여러개
		public static int GetYtoGlobalExpNo_Multi(
					string API_YTO_APP_TOKEN
					, string API_YTO_APP_KEY
					, ref List<OrdMaster> INVOICE_LIST
					, ref string error_str
					)
		{
			error_str = "";

			int success = 0;

			// 중국YTO Global API 연동에 필요한 값을 체크한다
			if (API_YTO_APP_TOKEN.Length == 0 || API_YTO_APP_KEY.Length == 0)
			{
				error_str = "API 연동에 필요한 값이 설정되지 않았습니다.";
				return success;
			}

			// 송장데이터 체크
			if (INVOICE_LIST == null || INVOICE_LIST.Count == 0)
			{
				error_str = comm_global.Language.Resources.API_NO_DATA;
				return success;
			}



			for (int i = 0; i < INVOICE_LIST.Count; i++)
			{
				OrdMaster INVOICE = INVOICE_LIST[i];
				string err1 = "";
				string DELVNO = GetYtoGlobalExpNo_One(API_YTO_APP_TOKEN, API_YTO_APP_KEY, ref INVOICE, ref err1);
				INVOICE_LIST[i] = INVOICE;

				if (DELVNO.Length == 0)
					error_str += err1 + "\n";
				else
					success++;
			}

			return success;
		}


		// 2018-01-18 jsy : 중국YTO Global 송장번호를 API를 사용하여 가져온다 ==> 한개
		public static string GetYtoGlobalExpNo_One(
					string API_YTO_APP_TOKEN
					, string API_YTO_APP_KEY
					, ref OrdMaster INVOICE
					, ref string error_str
					)
		{
			error_str = "";

			// 중국YTO Global API 연동에 필요한 값을 체크한다
			if (API_YTO_APP_TOKEN.Length == 0 || API_YTO_APP_KEY.Length == 0)
			{
				error_str = "API 연동에 필요한 값이 설정되지 않았습니다.";
				return "";
			}

			// 송장데이터 체크
			if (INVOICE == null || INVOICE.WAYBILLNO.Length == 0)
			{
				error_str = comm_global.Language.Resources.API_NO_DATA;
				return "";
			}



			//=====================================================================================
			// 1. 배송번호가 설정되었는지 체크한다
			//=====================================================================================

			if (INVOICE.DELVNO.Length > 0)
			{
				error_str = string.Format("{0} : ", INVOICE.WAYBILLNO) + comm_global.Language.Resources.API_SHIPPING_NUMBER_ALREADY_SET;
				return "";
			}



			//=====================================================================================
			// 2. YTO API에 전달할 데이터를 생성한다
			//=====================================================================================

			if (INVOICE.GoodsList.Count == 0)
			{
				error_str = string.Format("{0} : ", INVOICE.WAYBILLNO) + comm_global.Language.Resources.API_WE_CANNOT_GET_PRODUCT_INFO;
				return "";
			}

			YTOModel model = new YTOModel();
			model.RAINBOWCODE = INVOICE.EST_CODE;
			model.AGENTCODE = INVOICE.ESE_CODE;
			model.INVOICENO = INVOICE.WAYBILLNO;

			model.reference_no = INVOICE.WAYBILLNO;

			model.shipper_countrycode = INVOICE.DEP_NATION_CODE;  // 발송인 국가코드
			model.consignee_countrycode = INVOICE.NATION_CODE;  // 수취인 국가코드

			model.shipper_name = INVOICE.SENDER_NAME;
			model.shipper_street = INVOICE.SENDER_ADDR;
			model.shipper_mobile = INVOICE.SENDER_TELNO;
			model.consignee_name = INVOICE.RECEIVER_NAME;



			string state = "";
			string city = "";
			string district = "";
			string addr2 = "";
			string zipcode = "";
			string RECEIVER_ADDRESS = INVOICE.RECEIVER_ADDR1 + INVOICE.RECEIVER_ADDR2;

			// 2017-07-07 jsy : 중국주소를 자동으로 성,시,구 로 나누지 못한 경우 ==> 전체주소를 전송한다
			if (SettingsDatabase.SplitAddress_CN(RECEIVER_ADDRESS, ref state, ref city, ref district, ref addr2, ref zipcode, true))
			{
				model.consignee_province = state;
				model.consignee_city = city;
				model.consignee_district = district;
				model.consignee_street = addr2;
				model.consignee_postcode = (INVOICE.RECEIVER_ZIPCODE.Length == 6) ? INVOICE.RECEIVER_ZIPCODE : zipcode;
			}
			else
			{
				model.consignee_province = "";
				model.consignee_city = "";
				model.consignee_district = "";
				model.consignee_street = RECEIVER_ADDRESS;
				model.consignee_postcode = INVOICE.RECEIVER_ZIPCODE;
			}

			model.consignee_telephone = INVOICE.RECEIVER_TELNO;

			for (int i = 0; i < INVOICE.GoodsList.Count; i++)
			{
				if (INVOICE.GoodsList[i].GOODS_NAME.Length == 0)
					continue;

				YTO_INVOICE_MODEL invoice_model = new YTO_INVOICE_MODEL();
				invoice_model.invoice_enname = ((INVOICE.GoodsList[i].PRODUCT_NAME_EN.Length > 0) ? INVOICE.GoodsList[i].PRODUCT_NAME_EN : INVOICE.GoodsList[i].GOODS_NAME);
				invoice_model.invoice_cnname = ((INVOICE.GoodsList[i].PRODUCT_NAME_CN.Length > 0) ? INVOICE.GoodsList[i].PRODUCT_NAME_CN : INVOICE.GoodsList[i].GOODS_NAME);
				invoice_model.invoice_quantity = INVOICE.GoodsList[i].QTY;

				// 상품금액을 USD로 변환한다
				double PRICE_USD = INVOICE.GoodsList[i].PRICE;
				if (INVOICE.CURRENCY_GOODS != "USD")
				{
					PRICE_USD = ConfigDatabase.ExchangePrice(
									INVOICE.EST_CODE
									, SettingsDatabase.GetCurrencyUnit(INVOICE.EST_CODE)
									, INVOICE.GoodsList[i].PRICE
									, INVOICE.CURRENCY_GOODS
									, "USD"
									);
				}
				invoice_model.invoice_unitcharge = PRICE_USD;

				model.invoiceList.Add(invoice_model);

			}

			if (model.invoiceList.Count == 0)
			{
				error_str = string.Format("{0} : ", INVOICE.WAYBILLNO) + comm_global.Language.Resources.API_CAN_NOT_EXECUTE_BECAUSE_THERE_IS_NO_PRODUCT_INFORMATION;
				return "";
			}

			model.order_weight = INVOICE.REALWEIGHT.ToString("0.000");  // 2017-04-18 jsy : 무게 자동저장후 송장출력을 할수도 있으므로 실제무게를 전송하도록 수정

			API_YTO_GLOBAL api = new API_YTO_GLOBAL(API_YTO_GLOBAL.YTO_APIType.REAL, API_YTO_APP_TOKEN, API_YTO_APP_KEY, "", INVOICE.EST_CODE);

			// 주문등록 API
			string delvno = "";
			if (!api.OrderAdd(model, ref delvno, ref error_str))
			{
				return "";
			}

			// 지역코드 가져오기
			string err2 = "";
			string distribute_code = api.getdistribute(model.RAINBOWCODE, model.AGENTCODE, model.reference_no, delvno, ref err2);

			INVOICE.DELVNO = delvno;
			INVOICE.DELV_COM = "CN_YTO_GLOBAL";
			INVOICE.SDATA1 = distribute_code;

			return delvno;
		}










		// 2018-02-19 jsy : 일본 Qxpress 송장번호를 API를 사용하여 가져온다 ==> 여러개
		public static int GetQxpressExpNo_Multi(
					string API_QXP_ACCOUNT_ID
					, string API_QXP_API_KEY
					, ref List<OrdMaster> INVOICE_LIST
					, ref string error_str
					)
		{
			error_str = "";

			int success = 0;

			// 일본 Qxpress API 연동에 필요한 값을 체크한다
			if (API_QXP_ACCOUNT_ID.Length == 0 || API_QXP_API_KEY.Length == 0)
			{
				error_str = "API 연동에 필요한 값이 설정되지 않았습니다.";
				return success;
			}

			// 송장데이터 체크
			if (INVOICE_LIST == null || INVOICE_LIST.Count == 0)
			{
				error_str = comm_global.Language.Resources.API_NO_DATA;
				return success;
			}



			for (int i = 0; i < INVOICE_LIST.Count; i++)
			{
				OrdMaster INVOICE = INVOICE_LIST[i];
				string err1 = "";
				string DELVNO = GetQxpressExpNo_One(API_QXP_ACCOUNT_ID, API_QXP_API_KEY, ref INVOICE, ref err1);
				INVOICE_LIST[i] = INVOICE;

				if (DELVNO.Length == 0)  // 배송번호 가져오기 실패
				{
					if (INVOICE_LIST[i].ORDERNO1.Length > 0)
						error_str += string.Format("주문번호 {0} : ", INVOICE_LIST[i].ORDERNO1) + err1 + "\n";
					else
						error_str += string.Format("송장번호 {0} : ", INVOICE_LIST[i].WAYBILLNO) + err1 + "\n";
				}
				else
				{
					success++;
				}
			}

			return success;
		}


		// 2018-02-19 jsy : 일본 Qxpress 송장번호를 API를 사용하여 가져온다 ==> 한개
		public static string GetQxpressExpNo_One(
					string API_QXP_ACCOUNT_ID
					, string API_QXP_API_KEY
					, ref OrdMaster INVOICE
					, ref string error_str
					)
		{
			error_str = "";

			// 일본 Qxpress API 연동에 필요한 값을 체크한다
			if (API_QXP_ACCOUNT_ID.Length == 0 || API_QXP_API_KEY.Length == 0)
			{
				error_str = "API 연동에 필요한 값이 설정되지 않았습니다.";
				return "";
			}

			// 송장데이터 체크
			if (INVOICE == null || INVOICE.WAYBILLNO.Length == 0)
			{
				error_str = comm_global.Language.Resources.API_NO_DATA;
				return "";
			}



			//=====================================================================================
			// 1. 배송번호가 설정되었는지 체크한다
			//=====================================================================================

			if (INVOICE.DELVNO.Length > 0)
			{
				error_str = string.Format("{0} : ", INVOICE.WAYBILLNO) + comm_global.Language.Resources.API_SHIPPING_NUMBER_ALREADY_SET;
				return "";
			}



			//=====================================================================================
			// 2. 일본 Qxpress API에 전달할 데이터를 생성한다
			//=====================================================================================

			if (INVOICE.GoodsList.Count == 0)
			{
				error_str = string.Format("{0} : ", INVOICE.WAYBILLNO) + comm_global.Language.Resources.API_WE_CANNOT_GET_PRODUCT_INFO;
				return "";
			}

			// 일본 수취인 주소는 숫자를 기준으로 둘로 나눈다
			string rcptAddr1 = INVOICE.RECEIVER_ADDR1;
			string rcptAddr2 = ".";
			if (Regex.IsMatch(INVOICE.RECEIVER_ADDR1, @"[0-9]"))
			{
				Match match1 = Regex.Match(INVOICE.RECEIVER_ADDR1, @"[0-9]");
				if (match1 != null && match1.Index > 0)
				{
					rcptAddr1 = INVOICE.RECEIVER_ADDR1.Substring(0, match1.Index).Trim();
					rcptAddr2 = INVOICE.RECEIVER_ADDR1.Substring(match1.Index).Trim();
				}
			}

			// 상품정보
			string contents = INVOICE.GetGoodsDescriptionWithoutQty(" ");
			int quantity = INVOICE.GetGoodsPieces();
			double value = INVOICE.GetGoodsTotalAmount();
			string currency = "JPY";  // 배송국가 : 일본
			if (INVOICE.NATION_CODE == "HK") currency = "HKD";  // 배송국가 : 홍콩
			else if (INVOICE.NATION_CODE == "SG") currency = "SGD";  // 배송국가 : 싱가포르

			// 상품금액을 도착국가의 화폐단위로 변환한다
			double value_jpy = ConfigDatabase.ExchangePrice(
									INVOICE.EST_CODE
									, SettingsDatabase.GetCurrencyUnit(INVOICE.EST_CODE)
									, value
									, INVOICE.CURRENCY_GOODS
									, currency
									);

			QxpressModels model = new QxpressModels();
			model.RAINBOWCODE = INVOICE.EST_CODE;
			model.AGENTCODE = INVOICE.ESE_CODE;
			model.INVOICENO = INVOICE.WAYBILLNO;

			// 고객이 직접 입력하는 ORDERNO 는 겹치지 않도록 최소 4글자 이상 입력하도록 한다
			string refOrderNo = ((INVOICE.ORDERNO1.Length >= 4 && INVOICE.ORDERNO1.Length <= 50) ? INVOICE.ORDERNO1 : "");

			// 출발국 KR 도착국 JP 의 부산선편 유무(Y OR N)
			string useFukuokaRoute = "N";
//if (INVOICE.DEP_NATION_CODE == "KR" && INVOICE.NATION_CODE == "JP" && INVOICE.DELV_CODE == "B")	//추후 출고 타입 세팅 시 변경 필요 ********************
//{
//	useFukuokaRoute = "Y";
//}

			model.refOrderNo = refOrderNo;  // Your reference delivery order number
			model.svcType = "RM";  // "RM" (It is fixed value)
			model.rcptName = INVOICE.RECEIVER_NAME;  // Recipient name
			model.rcptEmail = INVOICE.RECEIVER_EMAIL;  // Recipient email address
			model.rcptCountry = INVOICE.NATION_CODE;  // Recipient country ("SG")
			model.rcptAddr1 = rcptAddr1;  // Recipient address 1 (State, City, Street) * JP - 도도부현 (都道府県)
			model.rcptAddr2 = rcptAddr2;  // Recipient address 2 (Bldg, Ste#) * JP - 시구정촌(市区町村) + 번지(番地)
			model.rcptZipcode = INVOICE.RECEIVER_ZIPCODE;  // Recipient postal code of address
			model.rcptPhone = INVOICE.RECEIVER_TELNO;  // Recipient telephone
			model.rcptMobile = INVOICE.RECEIVER_CPHONENO;  // Recipient mobile
			model.rcptMemo = INVOICE.MESSAGE_DELV;  // Recipient request memo
			model.contents = contents;  // Item name
			model.quantity = quantity;  // Quantity of Items (is not box qty.)
			model.value = value_jpy;  // Parcel amount value
			model.currency = currency;  // Currency of value ("SGD")
			model.useFukuokaRoute = useFukuokaRoute;  // 출발국 KR 도착국 JP 의 부산선편 유무(Y OR N)
			model.rcptNameFurigana = "";  // 수취인 명 (후리가나) : 도착국 JP 일 경우 선택입력
			model.premiumService = "N";  // Premium delivery service (only for KR-US route) : Y OR N



			//API_QXPRESS QXP = new API_QXPRESS(API_QXPRESS.ApiType.TEST);
			API_QXPRESS QXP = new API_QXPRESS(API_QXPRESS.ApiType.REAL, API_QXP_ACCOUNT_ID, API_QXP_API_KEY);

			// 주문등록 API
			//string DELVNO = "";
			if (!QXP.CreateOrder(ref model, ref error_str))
			{
				return "";
			}

			//// refOrderNo 를 입력한 경우는 "QS"로 시작하는 번호를 리턴해주지 않는다...
			//// "QS" 번호를 가져오기 위해서 트랙킹 데이터를 가져온다
			//if (DELVNO.Substring(0, 2) != "QS")
			//{
			//    System.Threading.Thread.Sleep(100);

			//    // 트랙킹 가져오기 API
			//    QxpressTrackingModels TRACKING = new QxpressTrackingModels();
			//    TRACKING.EST_CODE = INVOICE.EST_CODE;
			//    TRACKING.ESE_CODE = INVOICE.ESE_CODE;
			//    TRACKING.WAYBILLNO = INVOICE.WAYBILLNO;
			//    TRACKING.DELVNO = DELVNO;
			//    if (!QXP.Tracking(ref TRACKING, ref error_str))
			//    {
			//        return "";
			//    }

			//    DELVNO = TRACKING.qs_no;
			//}

			if (model.ResultCode != 0)
			{
				return "";
			}

			string DELVNO = model.ShippingNo;

			if (DELVNO.Length == 0 || DELVNO.Substring(0, 2) != "QS")
			{
				error_str = "Qxpress 번호 가져오기 실패!!!";
				return "";
			}

			INVOICE.DELVNO = DELVNO;
			INVOICE.DELV_COM = "JP_QXP";

			return DELVNO;  // 일본 Qxpress 번호 리턴
		}

	}
}
