using comm_dbconn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models
{
	public class CommFunction : DatabaseConnection
	{
		//카테고리 가져오기
		//int level : 카테고리 단계
		//int seqNo : 상위 카테고리 
		//string lan : 선택 언어 
		public List<schTypeArray> GetCategorySelectBox(int level, int seqNo, string lan)
		{
			string errorStr = "";

			string getCol = "";
			switch (lan)
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

			string listQuery = " SELECT SEQNO, " + getCol + " FROM stc_category WHERE CATE_LEVEL = " + level + " AND SEQNO = " + seqNo;

			if (level == 1)	//최상위 카테고리인 경우 
				listQuery = " SELECT SEQNO, " + getCol + " FROM stc_category WHERE CATE_LEVEL = " + level ;

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_key = listDt.Rows[i]["SEQNO"].ToString().Trim(), opt_value = listDt.Rows[i][getCol].ToString().Trim() });
				}
			}

			return model;
		}


		//액션 리스트 (권한 관리에 사용)
		public List<schTypeArray> GetGradeList()
		{
			List<schTypeArray> model = new List<schTypeArray> {
				new schTypeArray {      opt_key = "ProdAdd",     opt_value = "상품추가"              },		
				new schTypeArray {      opt_key = "ProdList",    opt_value = "상품조회"              },
				new schTypeArray {      opt_key = "StocReq",      opt_value = "보관신청"              },
				new schTypeArray {      opt_key = "StocList",     opt_value = "재고조회"     },// 재고조회 StocList
				new schTypeArray {      opt_key = "StocReqList",     opt_value = "보관신쳥현황 조회"     },
				new schTypeArray {      opt_key = "StocInOut",  opt_value = "입출고 내역 조회"           },// 입출고 내역 조회 StocInOut
				new schTypeArray {      opt_key = "PickReq",      opt_value = "픽업 신청 보류"              },
				new schTypeArray {      opt_key = "PickList",     opt_value = "픽업 신청 조회 보류"              },
				new schTypeArray {      opt_key = "EtsReq",   opt_value = "일반 배송 신청"     },
				new schTypeArray {      opt_key = "EtsReqExcel",   opt_value = "대량 배송 신청"           },
				new schTypeArray {      opt_key = "EtsLabel",   opt_value = "배송 라벨 출력"           },
				new schTypeArray {      opt_key = "EtsList",  opt_value = "배송 상태 조회"  },
				//new schTypeArray {      opt_key = "EtsCost",        opt_value = "배송 요율표 조회"     },//지금(18/6/22) 컨트롤러에 없는 것
				new schTypeArray {      opt_key = "EseGrade",        opt_value = "계정 등급 관리"                },
				new schTypeArray {      opt_key = "EseAccount",      opt_value = "ESE 계정 관리"          },
				new schTypeArray {      opt_key = "EseInfo",  opt_value = "계정 정보"          },
				new schTypeArray {      opt_key = "EseInfoMy",  opt_value = "내 계정 정보"          },
				new schTypeArray {      opt_key = "EseManager",  opt_value = "계정관리"          },
				new schTypeArray {      opt_key = "MarReqPg",    opt_value = "MAR 충전(PG) 보류 "    },
				new schTypeArray {      opt_key = "MarReq",   opt_value = "MAR 충전(이체)"      },
				new schTypeArray {      opt_key = "MarInOut",   opt_value = "Mar 충전/사용 이력"         },
				new schTypeArray {      opt_key = "MarOutReq",      opt_value = "MAR 환불 신청"         },
				new schTypeArray {      opt_key = "CostIndex",      opt_value = "배송요금"         },
				new schTypeArray {      opt_key = "CsEsmNotice",      opt_value = "ETOMARS 공지"      },
				new schTypeArray {      opt_key = "CsEstNotice",      opt_value = "STATION 공지"                },
				new schTypeArray {      opt_key = "CsQna",     opt_value = "1:1 문의"                   }

			};
			return model;
		}


		//그룹ID(MSH)
		public List<schTypeArray> GroupIdSelectBox()
		{
			string errorStr = "";

			string listQuery = " SELECT GROUP_ID,GROUP_NAME FROM ese_group ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["GROUP_NAME"].ToString().Trim(), opt_key = listDt.Rows[i]["GROUP_ID"].ToString().Trim() });
				}
			}

			return model;
		}


		public bool SendMail(SendMailModel SendMail)
		{
			string LAST_ERROR = "";

			SmtpClient client = new SmtpClient(SendMail.SERVER, SendMail.PORT);

			//client.UseDefaultCredentials = false;
			client.EnableSsl = SendMail.ENABLE_SSL;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.Credentials = new System.Net.NetworkCredential(SendMail.USERID, SendMail.PASSWD);

			MailAddress from = new MailAddress(SendMail.SENDER_EMAIL, SendMail.SENDER_NAME, Encoding.UTF8);

			MailMessage mail = new MailMessage();
			mail.Subject = SendMail.TITLE;
			mail.SubjectEncoding = Encoding.UTF8;
			mail.Body = SendMail.BODY;
			//string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
			//mail.Body += Environment.NewLine + someArrows;
			mail.BodyEncoding = Encoding.UTF8;
			mail.IsBodyHtml = true;
			mail.From = from;

			//mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
			//mail.Priority = MailPriority.Normal;

			for (int i = 0; i < SendMail.MAIL_TO.Count; i++)  // 받는사람 목록
			{
				MailAddress to = new MailAddress(SendMail.MAIL_TO[i]);
				mail.To.Add(to);
			}
			for (int i = 0; i < SendMail.MAIL_CC.Count; i++)  // 참조 목록
			{
				MailAddress cc = new MailAddress(SendMail.MAIL_CC[i]);
				mail.CC.Add(cc);
			}

			try
			{
				client.Send(mail);
			}
			catch (Exception ex)
			{
				LAST_ERROR = ex.Message;
				return false;
			}

			return true;
		}


		//화폐 단위
		public List<schTypeArray> GetConfCurrencySelectBox()
		{
			string errorStr = "";
			string listQuery = " SELECT SEQNO , CURRENCY_UNIT FROM conf_currency  ORDER BY SEQNO ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);

			List<schTypeArray> model = new List<schTypeArray>();

			if (listDt != null && listDt.Rows.Count != 0)
			{
				for (int i = 0; i < listDt.Rows.Count; i++)
				{
					model.Add(new schTypeArray { opt_value = listDt.Rows[i]["CURRENCY_UNIT"].ToString().Trim(), opt_key = listDt.Rows[i]["CURRENCY_UNIT"].ToString().Trim() });
				}
			}

			return model;
		}



		//화폐 단위
		public double GetMyMAR()
		{
			string errorStr = "";
			double mar = 0;
			HttpContext context = HttpContext.Current;

			string ESE_CODE = context.Session["ESE_CODE"].ToString();

			string listQuery = " SELECT MAR FROM est_sender  WHERE ESE_CODE = '" + ESE_CODE + "' ";

			DataTable listDt = getQueryResult(listQuery, out errorStr);
			
			if (listDt != null && listDt.Rows.Count != 0)
			{
				mar = double.Parse(listDt.Rows[0]["MAR"].ToString().Trim());
			}

			return mar;
		}


		public string GetApiKey(string ESTCODE, string ESCODE)
		{
			string API_KEY = "";

			string sql1 = "select "
						+ "API_KEY"
						+ " from est_sender"
						+ string.Format(" where ESTCODE='{0}'", ESTCODE)
						+ string.Format(" and ESCODE='{0}'", ESCODE)
						+ "";
			string err1 = "";
			DataTable dt1 = getQueryResult(sql1, out err1);
			if (dt1 != null && dt1.Rows.Count > 0)
			{
				API_KEY = dt1.Rows[0]["API_KEY"].ToString().Trim();
			}

			return API_KEY;
		}


	}
}