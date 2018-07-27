using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;  // EPPLUS : Excel 파일 읽어오기(*.xlsx)
using NPOI.SS.UserModel;  // NPOI : Excel 파일 읽어오기(*.xls)
using NPOI.HSSF.UserModel;  // NPOI : Excel 파일 읽어오기(*.xls)
using Gma.QrCodeNet;  // QR Code 생성

using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.Drawing;
using System.Web;

namespace comm_global
{
    // etomars 시스템 공통함수
    public class GlobalFunction
    {
        // AES_256 암호화
        static public string AESEncrypt_256(string InputText, string EncryptPassword = "")
        {
            string Password = (EncryptPassword.Length > 0) ? EncryptPassword : GlobalSettings.ENCRYPT_MASTER_KEY;

            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            // 입력받은 문자열을 바이트 배열로 변환  
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);

            // 딕셔너리 공격을 대비해서 키를 더 풀기 어렵게 만들기 위해서   
            // Salt를 사용한다.  
            byte[] Salt = Encoding.ASCII.GetBytes(Password);

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

            // Create a encryptor from the existing SecretKey bytes.  
            // encryptor 객체를 SecretKey로부터 만든다.  
            // Secret Key에는 32바이트  
            // Initialization Vector로 16바이트를 사용  
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();

            // CryptoStream객체를 암호화된 데이터를 쓰기 위한 용도로 선언  
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(PlainText, 0, PlainText.Length);

            cryptoStream.FlushFinalBlock();

            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string EncryptedData = Convert.ToBase64String(CipherBytes);

            return EncryptedData;
        }


        // AES_256 복호화
        static public string AESDecrypt_256(string InputText, string DecryptPassword)
        {
            string Password = (DecryptPassword.Length > 0) ? DecryptPassword : GlobalSettings.ENCRYPT_MASTER_KEY;

            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] EncryptedData = Convert.FromBase64String(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(Password);

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

            // Decryptor 객체를 만든다.  
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream(EncryptedData);

            // 데이터 읽기 용도의 cryptoStream객체  
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

            // 복호화된 데이터를 담을 바이트 배열을 선언한다.  
            byte[] PlainText = new byte[EncryptedData.Length];

            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

            return DecryptedData;
        }


        // 엑셀에서 읽어온 데이터를 스트링으로 변환한다
        // 숫자 형식인 경우 2.58312756042864E+15 같은 형식으로 읽어오기 때문에
        // 이런 경우는 일단 double로 type casting 한뒤 string으로 변환한다
        public static string GetString(object obj)
        {
            string str = "";
            if (obj == null)
                return str;

            Type type = obj.GetType();
            if (type.ToString() == "System.Double")
            {
                double d = (double)obj;
                str = d.ToString("0");
            }
            else
            {
                str = obj.ToString().Trim();
            }

            return str;
        }


        // string을 double로 변환하여 리턴한다
        public static double GetDouble(string str, int sosu)
        {
            double value = 0.0;
            if (str == null)
                return value;

            double.TryParse(str, out value);
            value = Math.Round(value, sosu);

            return value;
        }


        // string을 int로 변환하여 리턴한다
        public static int GetInt(string str)
        {
            int value = 0;
            if (str == null)
                return value;

            int.TryParse(str, out value);

            return value;
        }


        // string을 long으로 변환하여 리턴한다
        public static long GetLong(string str)
        {
            long value = 0;
            if (str == null)
                return value;

            long.TryParse(str, out value);

            return value;
        }

        // string => bool
        public static bool GetBool(string str)
        {
            bool value = false;

            bool.TryParse(str, out value);

            return value;
        }


        // 사용자 패스워드가 규칙에 맞게 입력되었는지 체크한다
        public static int PasswordValidationCheck(string password, ref string error_str)
        {
            error_str = "";

            string user_passwd = password.Trim();
            if (user_passwd.Length < 8)
            {
                error_str = "비밀번호는 영문자와 숫자를 포함해서 최소 8글자 이상 입력해야 합니다.(최대 30글자)";
                return -1;
            }

            if (user_passwd.Length > 30)
            {
                error_str = "비밀번호는 영문자와 숫자를 포함해서 최소 8글자 이상 입력해야 합니다.(최대 30글자)";
                return -2;
            }

            // 영문자, 숫자, 특수문자 포함해서 8~30글자의 패스워드를 입력받는다
            Regex rx1 = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,30}$");  // 소문자, 대문자, 숫자, 특수문자 각각 1개 이상씩 포함되어야 함
            Regex rx2 = new Regex(@"^(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^*()\-_=+\\\|\[\]{};:',.<>\/?])*.{8,30}$");  // 소문자, 숫자, 특수문자 각각 1개 이상씩 포함되어야 함
            Regex rx3 = new Regex(@"^(?=.*[a-z])(?=.*\d).{8,30}$");
            bool bValid = rx3.IsMatch(user_passwd);
            if (!bValid)
            {
                error_str = "비밀번호는 영문자와 숫자를 포함해야 합니다.";
                return -3;
            }

            return 0;
        }


        // 2018-06-18 jsy : E-Mail 유효성 체크
        public static bool EMailValidCheck(string email_addr)
        {
            return Regex.IsMatch(email_addr, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }










        // Excel 파일 가져오기
        // OLEDB로 Excel 파일을 읽어오려고 할때 오류발생 ==>  The 'Microsoft.ACE.OLEDB.12.0' provider is not registered on the local machine. 
        // 이런 경우는 https://www.microsoft.com/en-US/download/details.aspx?id=23734 웹사이트에서 파일을 다운받아 설치하면 된다
        // 그런데 사용자들에게 일일이 설명하기 귀찮으니까 Excel 파일을 읽어오는 dll을 사용한다
        // *.xlsx : EPPlus
        // *.xls  : NPOI
        public static DataTable ImportExcelXLS(string FileName, bool hasHeaders, out string error_str)
        {
            error_str = "";

            DataTable outputTable = null;
            string error = "";
            if (FileName.Substring(FileName.LastIndexOf('.')).ToLower() == ".xlsx")
                outputTable = GetDataTableFromExcel_EPPlus(FileName, hasHeaders, out error);  // EPPlus(*.xlsx)
            else
                outputTable = GetDataTableFromExcel_NPOI(FileName, hasHeaders, out error);  // NPOI(*.xls)

            error_str = error;

            return outputTable;
        }


		/**민성 테스트 위해 추가**/

		public DataTable ImportExcelXLS_Test(string FileName, bool hasHeaders, out string error_str)
		{
			error_str = "";

			DataTable outputTable = null;

			try
			{
				string error = "";
				if (FileName.Substring(FileName.LastIndexOf('.')).ToLower() == ".xlsx")
					outputTable = GetDataTableFromExcel_EPPlus(FileName, hasHeaders, out error);  // EPPlus(*.xlsx)
				else
					outputTable = GetDataTableFromExcel_NPOI(FileName, hasHeaders, out error);  // NPOI(*.xls)

				error_str = error;
			}
			catch
			{
				error_str = "잘못된 파일 형식입니다.";
			}

			return outputTable;
		}


		public static DataTable GetDataTableFromExcel_EPPlus(string path, bool hasHeaders, out string error_str)
        {
            error_str = "";

            try
            {
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }

                    //var ws = pck.Workbook.Worksheets.First();
                    var ws = pck.Workbook.Worksheets[1];  // 무조건 첫번째 탭을 읽어오도록 한다(index는 1부터 시작함)
                    DataTable tbl = new DataTable(ws.Name);

                    if (ws.Dimension == null)
                    {
                        return tbl;
                    }


                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        string col_name = (hasHeaders ? firstRowCell.Text : "");
                        if (col_name.Length == 0)
                            col_name = string.Format("Column {0}", firstRowCell.Start.Column);

                        tbl.Columns.Add(col_name);
                    }

                    var startRow = hasHeaders ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            //row[cell.Start.Column - 1] = cell.Text;

                            int col_index = cell.Start.Column - 1;
                            if (col_index >= tbl.Columns.Count)
                                break;

                            row[col_index] = cell.Value;
                        }
                    }

                    return tbl;
                }
            }
            catch (Exception ex1)
            {
                error_str = ex1.Message;
                return null;
            }
        }


        public static DataTable GetDataTableFromExcel_NPOI(string path, bool hasHeaders, out string error_str)
        {
            error_str = "";

            try
            {
                IWorkbook workbook;
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    workbook = new HSSFWorkbook(stream);
                }

                ISheet sheet = workbook.GetSheetAt(0);  // 무조건 첫번째 탭을 읽어오도록 한다
                DataTable dt = new DataTable(sheet.SheetName);

                // write header row
                IRow headerRow = sheet.GetRow(0);
                if (headerRow == null)
                {
                    return dt;
                }

                int total_cols = 0;
                foreach (ICell headerCell in headerRow)
                {
                    dt.Columns.Add(hasHeaders ? headerCell.ToString() : string.Format("Column {0}", (dt.Columns.Count + 1)));
                    total_cols++;
                }

                // 아래와 같은 방식으로 읽어오면 중간에 비어있는 셀을 읽어오지 못한다...
                //int rowIndex = 0;
                //foreach (IRow row in sheet)
                //{
                //    // skip header row
                //    if (hasHeaders && rowIndex++ == 0) continue;

                //    DataRow dataRow = dt.NewRow();
                //    dataRow.ItemArray = row.Cells.Select(c => c.ToString()).ToArray();
                //    dt.Rows.Add(dataRow);
                //}

                int startRow = (hasHeaders ? 1 : 0);
                for (int i = startRow; i < sheet.PhysicalNumberOfRows; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();

                    for (int k = 0; k < total_cols; k++)
                    {
                        ICell cell = row.GetCell(k, MissingCellPolicy.RETURN_NULL_AND_BLANK);
                        if (cell == null)
                            dataRow[k] = "";
                        else
                            dataRow[k] = GetCellValue_NPOI(cell, cell.CellType);
                    }

                    dt.Rows.Add(dataRow);
                }

                return dt;
            }
            catch (Exception ex1)
            {
                error_str = ex1.Message;
                return null;
            }
        }


        public static object GetCellValue_NPOI(ICell cell, CellType type)
        {
            object obj = null;

            switch (type)
            {
                case NPOI.SS.UserModel.CellType.Numeric:
                    obj = cell.NumericCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.String:
                    obj = cell.StringCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.Blank:
                    obj = "";
                    break;
                case NPOI.SS.UserModel.CellType.Boolean:
                    obj = cell.BooleanCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.Error:
                    obj = cell.ErrorCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.Formula:
                    obj = GetCellValue_NPOI(cell, cell.CachedFormulaResultType);
                    break;
                case NPOI.SS.UserModel.CellType.Unknown:
                    obj = "";
                    break;
                default:
                    obj = "";
                    break;
            }

            return obj;
        }










        // 따옴표 삭제
        static public void RemoveQuoteString(ref string str)
        {
            do
            {
                str = str.Replace("'", "").Trim();
            } while (str.Contains("'"));
        }


        // ORDERNO로 사용할 문자열에서 ASCII 문자만 가져온다
        public static string GetValidAsciiString(string str)
        {
            string valid_ascii_str = System.Text.RegularExpressions.Regex.Replace(str.Trim(), @"[^\x20-\x7E]", string.Empty);  // 문자열에서 사용할수 있는 ASCII 문자만 가져온다

            // 특수문자 제거
            valid_ascii_str = System.Text.RegularExpressions.Regex.Replace(valid_ascii_str, @"[\x22]", string.Empty);  // "
            valid_ascii_str = System.Text.RegularExpressions.Regex.Replace(valid_ascii_str, @"[\x27]", string.Empty);  // '
            valid_ascii_str = System.Text.RegularExpressions.Regex.Replace(valid_ascii_str, @"[\x2C]", string.Empty);  // ,
            valid_ascii_str = System.Text.RegularExpressions.Regex.Replace(valid_ascii_str, @"[\x60]", string.Empty);  // `

            return valid_ascii_str;
        }


        // 중국 신분증번호의 유효성을 체크한다
        public static bool SocialNo_ValidCheck_CN(string socialno)
        {
            bool socialno_verify = false;

            string SOCIALNO = Regex.Replace(socialno.ToUpper(), @"[^0-9X]", String.Empty);
            if (SOCIALNO.Length == 18)
            {
                string lastStr = SOCIALNO.Substring(17, 1);
                int lastNum = (lastStr == "X") ? 10 : int.Parse(lastStr);
                int CheckSum = CNID_GetCheckSum(SOCIALNO);
                if (CheckSum == lastNum)
                {
                    socialno_verify = true;
                }
            }

            return socialno_verify;
        }


        // 중국신분증번호의 마지막 18번째 숫자 체크
        public static int CNID_GetCheckSum(string idStr)
        {
            if (idStr.Length < 17)
                return -1;

            int[] idNum = new int[18];
            for (int i = 0; i < 17; i++)
            {
                int num = 0;
                if (!int.TryParse(idStr.Substring(i, 1), out num))
                    return -1;

                idNum[i] = num;
            }

            return CNID_GetCheckSum(idNum);
        }
        public static int CNID_GetCheckSum(int[] idNumber)
        {
            if (idNumber.Length < 17)
                return -1;

            int[] i = new int[17];
            int[] Ai = new int[17];
            int[] Wi = new int[17];
            int[] S = new int[17];
            int totalS = 0;
            for (int k = 0; k < 17; k++)
            {
                if (idNumber[k] < 0 || idNumber[k] > 9)
                    return -1;

                i[k] = 18 - k;
                Ai[k] = idNumber[k];
                Wi[k] = (int)Math.Pow(2, (i[k] - 1)) % 11;
                S[k] = Ai[k] * Wi[k];
                totalS += S[k];
            }
            int result = totalS % 11;

            int[] CheckSumNumber = new int[11];
            CheckSumNumber[0] = 1;
            CheckSumNumber[1] = 0;
            CheckSumNumber[2] = 10;
            CheckSumNumber[3] = 9;
            CheckSumNumber[4] = 8;
            CheckSumNumber[5] = 7;
            CheckSumNumber[6] = 6;
            CheckSumNumber[7] = 5;
            CheckSumNumber[8] = 4;
            CheckSumNumber[9] = 3;
            CheckSumNumber[10] = 2;

            return CheckSumNumber[result];
        }


        // 소숫점 뒷부분 0을 삭제한다
        public static string RemovePostfixZero(string str)
        {
            string str_new = str;

            int index = str_new.IndexOf('.');
            if (index < 0)
                return str_new;

            while (str_new.Length > 1)
            {
                string num = str_new.Substring(str_new.Length - 1, 1);
                if (num != "0" && num != ".")
                    break;

                str_new = str_new.Substring(0, str_new.Length - 1);
                if (num == ".")
                    break;
            }

            return str_new;
        }


        // 한국 우편번호 6자리를 5자리로 변환한다
        public static string GetKoreaZipcode5(string zipcode6, string address)
        {
            string zipcode5 = zipcode6;

            if (zipcode5.Length != 5)
            {
                string zipcode = GetKoreaZipcode5_DaumApi(address);
                if (zipcode.Length == 5) zipcode5 = zipcode;
            }

            return zipcode5;
        }


        // Daum 우편번호검색 웹페이지를 이용하여 우편번호를 검색한다
        public static string GetKoreaZipcode5_DaumApi(string address)
        {
            string zipcode5 = "";
            string addr = address.Trim();

            StringBuilder dataParams = new StringBuilder();
            dataParams.Append("region_name=" + System.Web.HttpUtility.UrlEncode(addr));
            dataParams.Append("&cq=" + System.Web.HttpUtility.UrlEncode(addr));
            dataParams.Append("&cpage=1");
            dataParams.Append("&origin=" + System.Web.HttpUtility.UrlEncode("http://postcode.map.daum.net"));
            dataParams.Append("&isp=N");
            dataParams.Append("&isgr=N");
            dataParams.Append("&isgj=N");
            dataParams.Append("&regionid=");
            dataParams.Append("&regionname=");
            dataParams.Append("&roadcode=");
            dataParams.Append("&roadname=");
            dataParams.Append("&banner=on");
            dataParams.Append("&indaum=");
            dataParams.Append("&vt=layer");
            dataParams.Append("&am=on");
            dataParams.Append("&animation=on");
            dataParams.Append("&mode=view");
            dataParams.Append("&shorthand=on");
            dataParams.Append("&CWinWidth=400");
            dataParams.Append("&theme=");
            dataParams.Append("&sit=");
            dataParams.Append("&sgit=");
            dataParams.Append("&sbit=");
            dataParams.Append("&pit=");
            dataParams.Append("&mit=");
            dataParams.Append("&lcit=");
            dataParams.Append("&plrg=");
            dataParams.Append("&plrgt=1.5");
            dataParams.Append("&sptype=");
            dataParams.Append("&sporgq=");
            dataParams.Append("&fullpath=" + System.Web.HttpUtility.UrlEncode("/guide"));

            string url = "http://postcode.map.daum.net/search?" + dataParams.ToString();
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";

            using (var stream = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                string ret = stream.ReadToEnd();

                string find_str = "<span class=\"txt_postcode\">";
                int index1 = ret.IndexOf(find_str, StringComparison.OrdinalIgnoreCase);
                int index2 = ret.IndexOf("</span>", (index1 + 1), StringComparison.OrdinalIgnoreCase);
                if (index1 > 0 && index2 > 0)
                {
                    zipcode5 = ret.Substring((index1 + find_str.Length), (index2 - index1 - find_str.Length));
                }
            }

            return zipcode5;
        }


        // 주소를 국가에 따라 다르게 표시한다
        public static string JoinAddress(string nation_code, string addr1, string addr2, string zipcode, bool masking = false)
        {
            string join_addr = "";

            if (nation_code == "KR")  // 한국인 경우 ==> 주소1 + 공백 + 주소2
            {
                if (!masking)
                {
                    join_addr = "(" + zipcode + ") " + addr1;
                    if (addr2.Length > 0) join_addr += " " + addr2;
                }
                else
                {
                    join_addr = "(" + zipcode + ") ";
                    if (addr2.Length > 0)
                    {
                        join_addr += addr1 + " ";
                        for (int i = 0; i < addr2.Length; i++)
                            join_addr += "*";
                    }
                    else
                    {
                        int cnt = (int)(addr1.Length * 0.6);
                        join_addr += addr1.Substring(0, (addr1.Length - cnt));
                        for (int i = 0; i < cnt; i++)
                            join_addr += "*";
                    }
                }
            }
            else if (nation_code == "CN")  // 중국은 그대로 적용한다
            {
                if (!masking)
                {
                    join_addr = "(" + zipcode + ") " + addr1 + addr2;
                }
                else
                {
                    join_addr = "(" + zipcode + ") ";
                    if (addr2.Length > 0)
                    {
                        join_addr += addr1;
                        for (int i = 0; i < addr2.Length; i++)
                            join_addr += "*";
                    }
                    else
                    {
                        int cnt = (int)(addr1.Length * 0.6);
                        join_addr += addr1.Substring(0, (addr1.Length - cnt));
                        for (int i = 0; i < cnt; i++)
                            join_addr += "*";
                    }
                }
            }
            else  // 나머지의 경우는 미국식으로 변경한다
            {
                if (!masking)
                {
                    join_addr = addr2;
                    if (join_addr.Length > 0) join_addr += ", ";
                    join_addr += addr1;
                    join_addr += " " + zipcode;
                    join_addr += " " + nation_code;
                }
                else
                {
                    if (addr2.Length > 0)
                    {
                        for (int i = 0; i < addr2.Length; i++)
                            join_addr += "*";

                        join_addr += ", " + addr1;
                    }
                    else
                    {
                        int cnt = (int)(addr1.Length * 0.6);
                        for (int i = 0; i < cnt; i++)
                            join_addr += "*";

                        join_addr += addr1.Substring(cnt);
                    }

                    join_addr += " " + zipcode;
                    join_addr += " " + nation_code;
                }
            }

            return join_addr;
        }


        // 자기자신의 외부IP를 가져온다
        public static string GetMyIP()
        {
            string ipaddress = "";

            try
            {
                string url = string.Format("https://whatismyipaddress.com/");
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.Host = "whatismyipaddress.com";
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36";

                string ret = "";
                using (var stream = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    ret = stream.ReadToEnd();

                    //string findStr = "<a href=\"//whatismyipaddress.com/ip/";
                    //int index1 = ret.IndexOf(findStr, 0, StringComparison.OrdinalIgnoreCase);
                    //int index2 = ret.IndexOf(">", index1 + 1, StringComparison.OrdinalIgnoreCase);
                    //int index3 = ret.IndexOf("</a>", index2 + 1, StringComparison.OrdinalIgnoreCase);
                    //if (index1 > 0 && index2 > 0 && index3 > 0)
                    //{
                    //    ipaddress = ret.Substring((index2 + 1), (index3 - index2 - 1));
                    //}

                    string findStr = "whatismyipaddress.com/ip/";
                    int index1 = ret.IndexOf(findStr, 0, StringComparison.OrdinalIgnoreCase);
                    int index2 = ret.IndexOf("\"", index1 + 1, StringComparison.OrdinalIgnoreCase);
                    if (index1 > 0 && index2 > 0)
                    {
                        ipaddress = ret.Substring((index1 + findStr.Length), (index2 - (index1 + findStr.Length)));
                    }
                }
            }
            catch
            {
                ipaddress = "";
            }

            return ipaddress;
        }


        // 작업내용을 알아볼수 있는 문자로 리턴한다(insert => 초기입력, modify => 수정, delete => 삭제)
        public static string GetActionString(string action)
        {
            string ACTION_STR = "";

            if (action.IndexOf("_insert", StringComparison.OrdinalIgnoreCase) >= 0) ACTION_STR = "최초입력";
            else if (action.IndexOf("_modify", StringComparison.OrdinalIgnoreCase) >= 0) ACTION_STR = "수정";
            else if (action.IndexOf("_delete", StringComparison.OrdinalIgnoreCase) >= 0) ACTION_STR = "삭제";

            return ACTION_STR;
        }


        // inch ==> mm
        public static double InchToMillimeter(double fInch)
        {
            return fInch * 25.4;
        }


        // mm ==> inch
        public static double MillimeterToInch(double fMillimeter)
        {
            return fMillimeter / 25.4;
        }


        // QR Code를 bitmap으로 생성하여 리턴한다
        public static Bitmap GenerateQRCode(string text, System.Drawing.Color DarkColor, System.Drawing.Color LightColor)
        {
            Gma.QrCodeNet.Encoding.QrEncoder Encoder = new Gma.QrCodeNet.Encoding.QrEncoder(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.L);
            Gma.QrCodeNet.Encoding.QrCode Code = Encoder.Encode(text);
            Bitmap TempBMP = new Bitmap(Code.Matrix.Width, Code.Matrix.Height);
            for (int X = 0; X <= Code.Matrix.Width - 1; X++)
            {
                for (int Y = 0; Y <= Code.Matrix.Height - 1; Y++)
                {
                    if (Code.Matrix.InternalArray[X, Y])
                        TempBMP.SetPixel(X, Y, DarkColor);
                    else
                        TempBMP.SetPixel(X, Y, LightColor);
                }
            }
            return TempBMP;
        }


        // 이미지를 다운로드하여 리턴한다
       /* public static Bitmap DownloadImage(string image_url)
        {
            Bitmap image = null;

            try
            {
                WebRequest request2 = WebRequest.Create(image_url);
                using (WebResponse response2 = request2.GetResponse())
                {
                    using (Stream responseStream2 = response2.GetResponseStream())
                    {
                        image = Image.FromStream(responseStream2) as Bitmap;
                    }
                }
            }
            catch (Exception ex1)
            {
            }

            return image;
        }
		*/

        // 파일경로에서 파일명만 리턴한다
        public static string GetFileName(string file_path)
        {
            string file_name = file_path;

            int index = file_path.LastIndexOf("\\");
            if (index >= 0)
                file_name = file_path.Substring((index + 1));

            return file_name;
        }


        // 파일경로에서 파일확장자만 리턴한다
        public static string GetFileExt(string file_path)
        {
            string file_ext = "";

            int index = file_path.LastIndexOf(".");
            if (index >= 0)
                file_ext = file_path.Substring((index + 1));

            return file_ext;
        }


        // 상태번호를 상태설명 string 으로 리턴한다
        public static string GetStatusStr(int status)
        {
            string status_str = "";

            if (status == 10) status_str = "입고대기";
            else if (status == 20) status_str = "입고";
            else if (status == 30) status_str = "출고";
            else if (status == 100) status_str = "항공기 출발";
            else if (status == 200) status_str = "항공기 도착";
            else if (status == 300) status_str = "통관 진행중";
            else if (status == 400) status_str = "통관 완료";
            else if (status == 500) status_str = "배송 시작";
            else if (status == 700) status_str = "배송 완료";

            return status_str;
        }



		//****민성 추가
		//엑셀 업로드 파일 DataTable 로 리턴
		public DataTable getUploadExcelData(HttpPostedFileBase file, out string error_str)
		{
			error_str = "";
			DataTable result = null;

			// 업로드 된 파일 체크
			if (file == null || file.ContentLength == 0)
			{
				//ViewBag.Message = CKBIZ_Comm.Language.resLanguage.FILE_SIZE_IS_ZERO;
				error_str = "파일크기가 0 입니다.";
				return result;
			}

			// 파일크기 체크
			int FILE_SIZE_LIMIT = 1024 * 1024 * 10;  // 10MB
			if (file.ContentLength > FILE_SIZE_LIMIT)
			{
				//ViewBag.Message = CKBIZ_Comm.Language.resLanguage.FILE_SIZE_CANNOT_EXCEED_10_MB;
				error_str = "파일크기가 10MB를 초과하였습니다.";
				return result;
			}

			// 저장폴더 체크
			string folder_path = HttpContext.Current.Server.MapPath("~/Content/temp");
			if (!System.IO.Directory.Exists(folder_path))
			{
				System.IO.Directory.CreateDirectory(folder_path);
			}

			// 파일 저장
			DateTime CURRENT_TIME = new DateTime(); //BranchDatabase.GetCurrentTime(branch_code);
			string file_name = file.FileName;
			string file_path = System.IO.Path.Combine(folder_path, string.Format("{0}_{1}", CURRENT_TIME.ToString("yyyyMMddHHmmss"), file_name));
			file.SaveAs(file_path);
			if (!System.IO.File.Exists(file_path))
			{
				//ViewBag.Message = CKBIZ_Comm.Language.resLanguage.FILE_UPLOAD_FAILED;
				error_str = "파일업로드에 실패 하였습니다.";
				return result;
			}

			GlobalFunction GobFunc = new GlobalFunction();
			result = GobFunc.ImportExcelXLS_Test(file_path, true, out error_str);

			if (result == null)
			{
				//ViewBag.Message = CKBIZ_Comm.Language.resLanguage.FILE_UPLOAD_FAILED;
				error_str = "데이터가 없습니다.";
			}

			return result;
		}
	}
}
