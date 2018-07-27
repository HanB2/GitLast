using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_est
{
    public class EstFunction
    {
        // 한페이지에 보여줄 갯수를 HTML select list 로 리턴
        public static List<string> GetPages_SelectList()
        {
            List<string> pages_list = new List<string>();

            pages_list.Add("20");
            pages_list.Add("50");
            pages_list.Add("100");
            pages_list.Add("200");
            pages_list.Add("300");
            pages_list.Add("400");
            pages_list.Add("500");
            pages_list.Add("1000");

            return pages_list;
        }
    }
}