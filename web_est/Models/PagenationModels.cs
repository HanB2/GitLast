using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_est
{
    // 2018-06-18 jsy : 검색결과 페이지 번호를 보여주기 위한 class
    public class PagenationModels
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        int PAGE_RANGE = 10;

        public PagenationModels()
        {
            TotalItems = 0;
            CurrentPage = 0;
            PageSize = 0;
            TotalPages = 0;
            StartPage = 0;
            EndPage = 0;
        }

        public void Pagenation(int total_items, int page_size, int current_page)
        {
            TotalItems = total_items;
            PageSize = page_size;
            CurrentPage = current_page;

            // 페이지를 계산한다
            TotalPages = TotalItems / PageSize;  // 전체 페이지 갯수
            if (TotalPages * PageSize < TotalItems)
                TotalPages++;

            if (CurrentPage < 1 || CurrentPage > TotalPages)
                CurrentPage = 1;

            StartPage = CurrentPage - 5;  // pagenation 에서 가장 앞부분 페이지
            EndPage = CurrentPage + 4;  // pagenation 에서 가장 뒷부분 페이지

            if (StartPage <= 0)
            {
                EndPage -= (StartPage - 1);
                StartPage = 1;
            }
            if (EndPage > TotalPages)
            {
                EndPage = TotalPages;
                StartPage = EndPage - PAGE_RANGE + 1;
                if (StartPage <= 0)
                    StartPage = 1;
            }
        }
    }
}
