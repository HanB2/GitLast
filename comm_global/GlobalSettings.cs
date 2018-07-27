using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_global
{
    // etomars 시스템에서 공통으로 적용되는 변수(상수) 들을 여기에 정의해 놓는다
    public class GlobalSettings
    {
        // AES_256 암호화 Master Key
        public static string ENCRYPT_MASTER_KEY = "etomars1234!#@";

        // bulk insert로 한번에 작성할수 있는 query 최대갯수
        public static int BULK_QUERY_LIMIT = 1000;

        // 한국 도착물건인 경우 일반건으로 전환되는 물품금액의 한도
        public static double CUSTOMS_KR_PRICE_LIMIT_FROM_US = 200.0;  // 미국인 경우 : $200
        public static double CUSTOMS_KR_PRICE_LIMIT_FROM_NOT_US = 150.0;  // 미국이 아닌 경우 : $150


		public static string INVOICE_RANGE_TABLE_COMM = "comm_waybillno_range";  // 송장번호 권역대 테이블명


		public static int MAX_UPLOAD_EXCEL_COUNT = 5000;  // 엑셀로 한번에 업로드 가능한 갯수
		public static int MAX_UPLOAD_DAILY_COUNT = 10000;  // 하루 최대 업로드 가능한 갯수
		public static int MAX_API_BULK_REG_COUNT = 100;  // API Bulk Insert 한번에 업로드 가능한 갯수


		public static string ZipcodeTable_CN = "zipcode_cn";  // 중국 우편번호 테이블명
		public static string ZipcodeTable_US = "zipcode_us";  // 미국 우편번호 테이블명


		public static string ORDERNO_PREFIX = "ETM";  // 주문번호 접두어
	}
}
