using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_est
{
    public class EstSettings
    {
        public static int ITEMS_PER_PAGE_INIT = 20;  // 한페이지에 보여줄 갯수 초기값

        // 한페이지에 보여줄 갯수 설정값을 가지고 있는 쿠키명
        public static string COOKIE_ITEMS_PER_PAGE_SETTINGS_CURRENCY = "COOKIE_EST_ITEMS_PER_PAGE_SETTINGS_CURRENCY";  // 환경설정 > 기본정보 > 기준통화 및 환율
        public static string COOKIE_ITEMS_PER_PAGE_SETTINGS_SHIPPING_COUNTRY = "COOKIE_EST_ITEMS_PER_PAGE_SETTINGS_SHIPPING_COUNTRY";  // 환경설정 > 기본정보 > 배송가능국가
        public static string COOKIE_ITEMS_PER_PAGE_SETTINGS_AIRPORT = "COOKIE_EST_ITEMS_PER_PAGE_SETTINGS_AIRPORT";  // 환경설정 > 기본정보 > 공항코드
        public static string COOKIE_ITEMS_PER_PAGE_SETTINGS_RELEASE_TYPE = "COOKIE_EST_ITEMS_PER_PAGE_SETTINGS_RELEASE_TYPE";  // 환경설정 > 기본정보 > 현지 배송업체
        public static string COOKIE_ITEMS_PER_PAGE_SETTINGS_LOCAL_DELIVERY = "COOKIE_EST_ITEMS_PER_PAGE_SETTINGS_LOCAL_DELIVERY";  // 환경설정 > 기본정보 > 현지 배송업체
    }
}