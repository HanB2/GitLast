using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using comm_global;

// EPPLUS
using OfficeOpenXml;
using OfficeOpenXml.Style;

using System.Web.Security;
using System.IO;
using System.Data;

namespace web_est.Controllers
{
    // 환경설정
    [Authorize]
    public class SettingsController : Controller
    {
        //=========================================================================================
        //=========================================================================================
        // 기본 정보 - 기준통화 및 환율
        //=========================================================================================
        //=========================================================================================

        // 기준통화 및 환율
        public ActionResult Currency()
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_CURRENCY", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수

            // 쿠키값 체크
            if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_CURRENCY] != null)
            {
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_CURRENCY].Value != null)
                {
                    ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_CURRENCY].Value));
                }
            }

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(CurrencyQueryResult(ITEMS_PER_PAGE, 1));
        }


        // 검색결과 리턴 : 기준통화 및 환율
        object CurrencyQueryResult(
                    int ITEMS_PER_PAGE  // 한 페이지당 보여줄 갯수
                    , int SEARCH_PAGE  // 검색할 페이지
                    )
        {
            // 전체 갯수를 가져온다
            int total_count = ConfCurrencyDatabase.GetCurrencyDataCount();
            if (total_count <= 0)
                return null;

            // 페이지를 계산한다
            PagenationModels pager = new PagenationModels();
            pager.Pagenation(total_count, ITEMS_PER_PAGE, SEARCH_PAGE);

            // 기준통화 및 환율 데이터를 가져온다
            int offset = ((total_count > ITEMS_PER_PAGE) ? (pager.CurrentPage - 1) * pager.PageSize : 0);
            int limit = ((total_count > ITEMS_PER_PAGE) ? pager.PageSize : 0);
            List<ConfCurrencyModels> list = ConfCurrencyDatabase.GetCurrencyDataList(offset, limit);
            if (list == null || list.Count == 0)
                return null;

            ConfCurrencyListModels model = new ConfCurrencyListModels();
            model.Pager = pager;
            model.Items = list;

            return model;
        }


        [HttpPost]
        public ActionResult Currency(
                    string items_per_page
                    , string search_page
                    )
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_CURRENCY", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수
            int SEARCH_PAGE = ((search_page != null) ? GlobalFunction.GetInt(search_page.Trim()) : 1);
            if (items_per_page != null)
            {
                ITEMS_PER_PAGE = GlobalFunction.GetInt(items_per_page.Trim());
            }
            else
            {
                // 쿠키값 체크
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_CURRENCY] != null)
                {
                    if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_CURRENCY].Value != null)
                    {
                        ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_CURRENCY].Value));
                    }
                }
            }

            if (ITEMS_PER_PAGE <= 0) ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;
            if (SEARCH_PAGE <= 0) SEARCH_PAGE = 1;

            // 쿠키 저장
            var Cookie1 = new HttpCookie(EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_CURRENCY);
            Cookie1.Value = ITEMS_PER_PAGE.ToString();
            Cookie1.Expires = DateTime.Now.AddDays(365);
            //Cookie1.HttpOnly = true;
            //Cookie1.Secure = true;
            //context.Response.SetCookie(Cookie1);
            Response.Cookies.Add(Cookie1);

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(CurrencyQueryResult(ITEMS_PER_PAGE, SEARCH_PAGE));
        }










        //=========================================================================================
        //=========================================================================================
        // 기본 정보 - 배송가능국가
        //=========================================================================================
        //=========================================================================================

        // 배송가능국가
        public ActionResult ShippingCountry()
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_SHIPPING_COUNTRY", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수

            // 쿠키값 체크
            if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_SHIPPING_COUNTRY] != null)
            {
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_SHIPPING_COUNTRY].Value != null)
                {
                    ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_SHIPPING_COUNTRY].Value));
                }
            }

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(ShippingCountryQueryResult(ITEMS_PER_PAGE, 1));
        }


        // 검색결과 리턴 : 배송가능국가
        object ShippingCountryQueryResult(
                    int ITEMS_PER_PAGE  // 한 페이지당 보여줄 갯수
                    , int SEARCH_PAGE  // 검색할 페이지
                    )
        {
            // 전체 갯수를 가져온다
            int total_count = ConfShippingCountryDatabase.GetShippingCountryDataCount();
            if (total_count <= 0)
                return null;

            // 페이지를 계산한다
            PagenationModels pager = new PagenationModels();
            pager.Pagenation(total_count, ITEMS_PER_PAGE, SEARCH_PAGE);

            // 기준통화 및 환율 데이터를 가져온다
            int offset = ((total_count > ITEMS_PER_PAGE) ? (pager.CurrentPage - 1) * pager.PageSize : 0);
            int limit = ((total_count > ITEMS_PER_PAGE) ? pager.PageSize : 0);
            List<ConfShippingCountryModels> list = ConfShippingCountryDatabase.GetShippingCountryDataList(offset, limit);
            if (list == null || list.Count == 0)
                return null;

            ConfShippingCountryListModels model = new ConfShippingCountryListModels();
            model.Pager = pager;
            model.Items = list;

            return model;
        }


        [HttpPost]
        public ActionResult ShippingCountry(
                    string items_per_page
                    , string search_page
                    )
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_SHIPPING_COUNTRY", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수
            int SEARCH_PAGE = ((search_page != null) ? GlobalFunction.GetInt(search_page.Trim()) : 1);
            if (items_per_page != null)
            {
                ITEMS_PER_PAGE = GlobalFunction.GetInt(items_per_page.Trim());
            }
            else
            {
                // 쿠키값 체크
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_SHIPPING_COUNTRY] != null)
                {
                    if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_SHIPPING_COUNTRY].Value != null)
                    {
                        ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_SHIPPING_COUNTRY].Value));
                    }
                }
            }

            if (ITEMS_PER_PAGE <= 0) ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;
            if (SEARCH_PAGE <= 0) SEARCH_PAGE = 1;

            // 쿠키 저장
            var Cookie1 = new HttpCookie(EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_SHIPPING_COUNTRY);
            Cookie1.Value = ITEMS_PER_PAGE.ToString();
            Cookie1.Expires = DateTime.Now.AddDays(365);
            //Cookie1.HttpOnly = true;
            //Cookie1.Secure = true;
            //context.Response.SetCookie(Cookie1);
            Response.Cookies.Add(Cookie1);

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(ShippingCountryQueryResult(ITEMS_PER_PAGE, SEARCH_PAGE));
        }










        //=========================================================================================
        //=========================================================================================
        // 기본 정보 - 공항코드
        //=========================================================================================
        //=========================================================================================

        // 공항코드
        public ActionResult Airport()
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_AIRPORT", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수

            // 쿠키값 체크
            if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_AIRPORT] != null)
            {
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_AIRPORT].Value != null)
                {
                    ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_AIRPORT].Value));
                }
            }

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(AirportQueryResult(ITEMS_PER_PAGE, 1));
        }


        // 검색결과 리턴 : 공항코드
        object AirportQueryResult(
                    int ITEMS_PER_PAGE  // 한 페이지당 보여줄 갯수
                    , int SEARCH_PAGE  // 검색할 페이지
                    )
        {
            // 전체 갯수를 가져온다
            int total_count = ConfAirportDatabase.GetAirportDataCount();
            if (total_count <= 0)
                return null;

            // 페이지를 계산한다
            PagenationModels pager = new PagenationModels();
            pager.Pagenation(total_count, ITEMS_PER_PAGE, SEARCH_PAGE);

            // 기준통화 및 환율 데이터를 가져온다
            int offset = ((total_count > ITEMS_PER_PAGE) ? (pager.CurrentPage - 1) * pager.PageSize : 0);
            int limit = ((total_count > ITEMS_PER_PAGE) ? pager.PageSize : 0);
            List<ConfAirportModels> list = ConfAirportDatabase.GetAirportDataList(offset, limit);
            if (list == null || list.Count == 0)
                return null;

            ConfAirportListModels model = new ConfAirportListModels();
            model.Pager = pager;
            model.Items = list;

            return model;
        }


        [HttpPost]
        public ActionResult Airport(
                    string items_per_page
                    , string search_page
                    )
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_AIRPORT", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수
            int SEARCH_PAGE = ((search_page != null) ? GlobalFunction.GetInt(search_page.Trim()) : 1);
            if (items_per_page != null)
            {
                ITEMS_PER_PAGE = GlobalFunction.GetInt(items_per_page.Trim());
            }
            else
            {
                // 쿠키값 체크
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_AIRPORT] != null)
                {
                    if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_AIRPORT].Value != null)
                    {
                        ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_AIRPORT].Value));
                    }
                }
            }

            if (ITEMS_PER_PAGE <= 0) ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;
            if (SEARCH_PAGE <= 0) SEARCH_PAGE = 1;

            // 쿠키 저장
            var Cookie1 = new HttpCookie(EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_AIRPORT);
            Cookie1.Value = ITEMS_PER_PAGE.ToString();
            Cookie1.Expires = DateTime.Now.AddDays(365);
            //Cookie1.HttpOnly = true;
            //Cookie1.Secure = true;
            //context.Response.SetCookie(Cookie1);
            Response.Cookies.Add(Cookie1);

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(AirportQueryResult(ITEMS_PER_PAGE, SEARCH_PAGE));
        }










        //=========================================================================================
        //=========================================================================================
        // 기본 정보 - 출고타입
        //=========================================================================================
        //=========================================================================================

        // 출고타입
        public ActionResult ReleaseType()
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_RELEASE_TYPE", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수

            // 쿠키값 체크
            if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_RELEASE_TYPE] != null)
            {
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_RELEASE_TYPE].Value != null)
                {
                    ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_RELEASE_TYPE].Value));
                }
            }

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(ReleaseTypeQueryResult(ITEMS_PER_PAGE, 1));
        }


        // 검색결과 리턴 : 출고타입
        object ReleaseTypeQueryResult(
                    int ITEMS_PER_PAGE  // 한 페이지당 보여줄 갯수
                    , int SEARCH_PAGE  // 검색할 페이지
                    )
        {
            // 전체 갯수를 가져온다
            int total_count = ConfReleaseTypeDatabase.GetReleaseTypeDataCount();
            if (total_count <= 0)
                return null;

            // 페이지를 계산한다
            PagenationModels pager = new PagenationModels();
            pager.Pagenation(total_count, ITEMS_PER_PAGE, SEARCH_PAGE);

            // 기준통화 및 환율 데이터를 가져온다
            int offset = ((total_count > ITEMS_PER_PAGE) ? (pager.CurrentPage - 1) * pager.PageSize : 0);
            int limit = ((total_count > ITEMS_PER_PAGE) ? pager.PageSize : 0);
            List<ConfReleaseTypeModels> list = ConfReleaseTypeDatabase.GetReleaseTypeDataList(offset, limit);
            if (list == null || list.Count == 0)
                return null;

            ConfReleaseTypeListModels model = new ConfReleaseTypeListModels();
            model.Pager = pager;
            model.Items = list;

            return model;
        }


        [HttpPost]
        public ActionResult ReleaseType(
                    string items_per_page
                    , string search_page
                    )
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_RELEASE_TYPE", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수
            int SEARCH_PAGE = ((search_page != null) ? GlobalFunction.GetInt(search_page.Trim()) : 1);
            if (items_per_page != null)
            {
                ITEMS_PER_PAGE = GlobalFunction.GetInt(items_per_page.Trim());
            }
            else
            {
                // 쿠키값 체크
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_RELEASE_TYPE] != null)
                {
                    if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_RELEASE_TYPE].Value != null)
                    {
                        ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_RELEASE_TYPE].Value));
                    }
                }
            }

            if (ITEMS_PER_PAGE <= 0) ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;
            if (SEARCH_PAGE <= 0) SEARCH_PAGE = 1;

            // 쿠키 저장
            var Cookie1 = new HttpCookie(EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_RELEASE_TYPE);
            Cookie1.Value = ITEMS_PER_PAGE.ToString();
            Cookie1.Expires = DateTime.Now.AddDays(365);
            //Cookie1.HttpOnly = true;
            //Cookie1.Secure = true;
            //context.Response.SetCookie(Cookie1);
            Response.Cookies.Add(Cookie1);

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(ReleaseTypeQueryResult(ITEMS_PER_PAGE, SEARCH_PAGE));
        }


        // AJAX : 출고타입별 기본요율표 리턴하기
        [HttpPost]
        public JsonResult Ajax_GetBasicCostTable(string nation_code, string release_code)
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                return Json(new { Success = false, ErrorMessage = "세션 에러입니다." }, JsonRequestBehavior.AllowGet);
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_RELEASE_TYPE", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return Json(new { Success = false, ErrorMessage = "접근권한이 없습니다." }, JsonRequestBehavior.AllowGet);
            }



            // parameter 체크
            string NATION_CODE = ((nation_code != null) ? nation_code.Trim().ToUpper() : "");
            string RELEASE_CODE = ((release_code != null) ? release_code.Trim().ToUpper() : "");
            if (NATION_CODE.Length == 0 || RELEASE_CODE.Length == 0)
            {
                return Json(new { Success = false, ErrorMessage = "매개변수 에러입니다." }, JsonRequestBehavior.AllowGet);
            }

            // 출고타입별 기본배송비를 가져온다
            List<EstShippingFeeModels> SHIPPING_FEE_LIST = EstShippingFeeDatabase.GetCostTableData(ESTCODE, "00000000", NATION_CODE, RELEASE_CODE);
            if (SHIPPING_FEE_LIST == null)
            {
                return Json(new { Success = false, ErrorMessage = "Database 에러입니다." }, JsonRequestBehavior.AllowGet);
            }

            // STATION 무게단위를 가져온다
            string WEIGHT_UNIT = EsmStationDatabase.GetWeightUnit(ESTCODE);

            // 기본배송비가 없는 경우 ==> 비어있는 상태를 리턴한다
            if (SHIPPING_FEE_LIST.Count == 0)
            {
                SHIPPING_FEE_LIST = EstShippingFeeDatabase.GetEmptyCostTable(WEIGHT_UNIT);
            }

// Station에 적용된 통관배송비를 가져온다

            var data = SHIPPING_FEE_LIST.Select(m => new {
                    WEIGHT = ((WEIGHT_UNIT == "KG") ? m.WEIGHT.ToString("0.000") : m.WEIGHT.ToString("0.0")),
                    CUSTOMS_FEE = 0.0,
                    SHIPPING_FEE_NOR = m.SHIPPING_FEE_NOR.ToString("#,##0.00"),
                    SHIPPING_FEE_STC = m.SHIPPING_FEE_STC.ToString("#,##0.00")
                }
            );

            return Json(new { Success = true, WeightUnit = WEIGHT_UNIT, Data = data }, JsonRequestBehavior.AllowGet);
        }


        // AJAX : 출고타입별 기본요율표 저장하기
        [HttpPost]
        public JsonResult Ajax_BasicCostTableUpdate(
                    string nation_code
                    , string release_code
                    , string[] weight_list
                    , string[] normal_fee_list
                    , string[] stock_fee_list
                    )
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                return Json(new { Success = false, ErrorMessage = "세션 에러입니다." }, JsonRequestBehavior.AllowGet);
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_RELEASE_TYPE", AUTH_MODE.AUTH_UPDATE, USERID))
            {
                return Json(new { Success = false, ErrorMessage = "수정권한이 없습니다." }, JsonRequestBehavior.AllowGet);
            }



            // parameter 체크
            string NATION_CODE = ((nation_code != null) ? nation_code.Trim().ToUpper() : "");
            string RELEASE_CODE = ((release_code != null) ? release_code.Trim().ToUpper() : "");
            if (NATION_CODE.Length == 0 || RELEASE_CODE.Length == 0
                || weight_list == null || weight_list.Length == 0
                || normal_fee_list == null || normal_fee_list.Length == 0
                || stock_fee_list == null || stock_fee_list.Length == 0
                || weight_list.Length != normal_fee_list.Length || normal_fee_list.Length != stock_fee_list.Length
                )
            {
                return Json(new { Success = false, ErrorMessage = "매개변수 에러입니다." }, JsonRequestBehavior.AllowGet);
            }

            // STATION 무게단위를 가져온다
            string WEIGHT_UNIT = EsmStationDatabase.GetWeightUnit(ESTCODE);



            List<EstShippingFeeModels> ShippingFeeList = new List<EstShippingFeeModels>();
            string error_str = "";

            for (int i = 0; i < weight_list.Length; i++)
            {
                double WEIGHT = GlobalFunction.GetDouble(weight_list[i].Trim(), 3);  //	무게(kg : 소수점 3자리)
                double SHIPPING_FEE_NOR = GlobalFunction.GetDouble(normal_fee_list[i].Trim(), 2);  // 일반신청 배송비(MAR : 소수점 2자리)
                double SHIPPING_FEE_STC = GlobalFunction.GetDouble(stock_fee_list[i].Trim(), 2);  // 보관신청 배송비(MAR : 소수점 2자리)

                if (WEIGHT <= 0.0)
                {
                    error_str = string.Format("{0}번줄의 무게를 정확히 입력하세요.", (i + 1));
                    break;
                }
                if (SHIPPING_FEE_NOR <= 0.0)
                {
                    error_str = string.Format("{0}번줄의 일반배송요금을 정확히 입력하세요.", (i + 1));
                    break;
                }
                if (SHIPPING_FEE_STC <= 0.0)
                {
                    error_str = string.Format("{0}번줄의 재고배송요금을 정확히 입력하세요.", (i + 1));
                    break;
                }

                List<EstShippingFeeModels> tmp_list = ShippingFeeList.FindAll(m => Math.Abs(m.WEIGHT - WEIGHT) < 0.001);
                if (tmp_list.Count > 0)
                {
                    error_str = string.Format("무게 {0} {1} : 중복 입력되었습니다.", ((WEIGHT_UNIT == "KG") ? WEIGHT.ToString("0.000") : WEIGHT.ToString("0.0")), WEIGHT_UNIT);
                    break;
                }



                EstShippingFeeModels ShippingFee = new EstShippingFeeModels();

                ShippingFee.EST_CODE = ESTCODE;  //	varchar(5)			STATION 코드	
                ShippingFee.ESE_CODE = "00000000";  //	varchar(8)  SENDER 코드("00000000" : 기본 요율표)
                ShippingFee.NATION_CODE = NATION_CODE;  //	char(2)			국가코드	
                ShippingFee.RELEASE_CODE = RELEASE_CODE;  //	varchar(20)			출고타입 코드	
                ShippingFee.WEIGHT = WEIGHT;  //	double			무게(kg : 소수점 3자리)	
                ShippingFee.SHIPPING_FEE_NOR = SHIPPING_FEE_NOR;  //	double			일반신청 배송비(MAR : 소수점 2자리)	
                ShippingFee.SHIPPING_FEE_STC = SHIPPING_FEE_STC;  //	double			보관신청 배송비(MAR : 소수점 2자리)	

                ShippingFeeList.Add(ShippingFee);
            }

            if (error_str.Length > 0)
            {
                return Json(new { Success = false, ErrorMessage = error_str }, JsonRequestBehavior.AllowGet);
            }
            if (ShippingFeeList.Count == 0)
            {
                return Json(new { Success = false, ErrorMessage = "저장할 내용이 없습니다." }, JsonRequestBehavior.AllowGet);
            }

            // DB에 저장
            if (!EstShippingFeeDatabase.BasicCostTableUpdate(ShippingFeeList))
            {
                return Json(new { Success = false, ErrorMessage = "Database 에러입니다." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = true, Message = "저장되었습니다." }, JsonRequestBehavior.AllowGet);
        }


        // AJAX : 기본요율표 Excel 다운로드
        [HttpPost]
        public JsonResult Ajax_BasicCostTableExcelDownload(
                    string nation_code
                    , string release_code
                    , string[] weight_list
                    , string[] customs_fee_list
                    , string[] normal_fee_list
                    , string[] stock_fee_list
                    )
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                return Json(new { Success = false, ErrorMessage = "세션 에러입니다." }, JsonRequestBehavior.AllowGet);
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_RELEASE_TYPE", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return Json(new { Success = false, ErrorMessage = "접근권한이 없습니다." }, JsonRequestBehavior.AllowGet);
            }



            // parameter 체크
            string NATION_CODE = ((nation_code != null) ? nation_code.Trim().ToUpper() : "");
            string RELEASE_CODE = ((release_code != null) ? release_code.Trim().ToUpper() : "");
            if (NATION_CODE.Length == 0 || RELEASE_CODE.Length == 0
                || weight_list == null || weight_list.Length == 0
                || customs_fee_list == null || customs_fee_list.Length == 0
                || normal_fee_list == null || normal_fee_list.Length == 0
                || stock_fee_list == null || stock_fee_list.Length == 0
                || weight_list.Length != customs_fee_list.Length || customs_fee_list.Length != normal_fee_list.Length || normal_fee_list.Length != stock_fee_list.Length
                )
            {
                return Json(new { Success = false, ErrorMessage = "매개변수 에러입니다." }, JsonRequestBehavior.AllowGet);
            }

            // STATION 무게단위를 가져온다
            string WEIGHT_UNIT = EsmStationDatabase.GetWeightUnit(ESTCODE);



            // Excel 파일을 저장할 폴더
            string folder_path = Server.MapPath("~/Content/_userfile/settings_basic_cost_table_download");
            if (!System.IO.Directory.Exists(folder_path))
            {
                System.IO.Directory.CreateDirectory(folder_path);
            }

            // 기존에 생성된 파일들을 삭제한다(어제, 오늘 생성된 파일을 제외하고 나머지는 삭제한다)
            DateTime CURRENT_TIME = DateTime.Now;
            string date1 = CURRENT_TIME.ToString("yyyyMMdd");
            string date2 = CURRENT_TIME.AddDays(-1).ToString("yyyyMMdd");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folder_path);
            foreach (System.IO.FileInfo f in di.GetFiles())
            {
                if (f.Extension.ToLower() == ".xlsx" && f.Name.Substring(0, 8) != date1 && f.Name.Substring(0, 8) != date2)
                {
                    f.Delete();
                }
            }

            // Excel 파일 생성 
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("BasicCostTable");

                int row = 1;
                int col = 1;

                workSheet.Cells[row, col++].Value = string.Format("무게 ({0})", WEIGHT_UNIT);
                workSheet.Cells[row, col++].Value = "통관배송비";
                workSheet.Cells[row, col++].Value = "일반배송요금";
                workSheet.Cells[row, col++].Value = "재고배송요금";

                for (int i = 0; i < weight_list.Length; i++)
                {
                    double WEIGHT = GlobalFunction.GetDouble(weight_list[i].Trim(), 3);  //	무게(kg : 소수점 3자리)
                    double CUSTOMS_FEE = GlobalFunction.GetDouble(customs_fee_list[i].Trim(), 2);  // ESM에서 설정한 무게별 통관배송비(MAR : 소수점 2자리)
                    double SHIPPING_FEE_NOR = GlobalFunction.GetDouble(normal_fee_list[i].Trim(), 2);  // 일반신청 배송비(MAR : 소수점 2자리)
                    double SHIPPING_FEE_STC = GlobalFunction.GetDouble(stock_fee_list[i].Trim(), 2);  // 보관신청 배송비(MAR : 소수점 2자리)

                    row++;
                    col = 1;

                    workSheet.Cells[row, col++].Value = WEIGHT;
                    workSheet.Cells[row, col++].Value = CUSTOMS_FEE;
                    workSheet.Cells[row, col++].Value = SHIPPING_FEE_NOR;
                    workSheet.Cells[row, col++].Value = SHIPPING_FEE_STC;
                }

                workSheet.Column(1).Style.Numberformat.Format = ((WEIGHT_UNIT == "KG") ? @"#,##0.000" : @"#,##0.0");  // 무게
                workSheet.Column(2).Style.Numberformat.Format = @"#,##0.00";  // 통관배송비
                workSheet.Column(3).Style.Numberformat.Format = @"#,##0.00";  // 일반배송요금
                workSheet.Column(4).Style.Numberformat.Format = @"#,##0.00";  // 재고배송요금

                workSheet.Cells[1, 1, row, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[1, 1, row, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[1, 1, row, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[1, 1, row, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                workSheet.Cells[1, 1, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                workSheet.Cells.AutoFitColumns();



                MemoryStream stream = new MemoryStream();
                excelPackage.SaveAs(stream);

                // 파일 저장
                string filename = System.IO.Path.Combine(folder_path, string.Format("{0}-BasicCostTable-{1}-{2}-{3}-{4}.xlsx", CURRENT_TIME.ToString("yyyyMMdd"), ESTCODE, NATION_CODE, RELEASE_CODE, Guid.NewGuid().ToString().Substring(0, 25)));
                System.IO.File.WriteAllBytes(filename, stream.ToArray());
                if (!System.IO.File.Exists(filename))
                {
                    return Json(new { Success = false, ErrorMessage = "파일 생성 실패" }, JsonRequestBehavior.AllowGet);
                }

                int index2 = filename.IndexOf("\\Content\\_userfile\\settings_basic_cost_table_download");
                string filename2 = filename.Substring(index2).Replace("\\", "/");

                return Json(new { Success = true, Data = filename2 }, JsonRequestBehavior.AllowGet);
            }
        }


        // AJAX : 기본요율표 Excel 업로드
        [HttpPost]
        public JsonResult Ajax_BasicCostTableExcelUpload(HttpPostedFileBase uploadfile)
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                return Json(new { Success = false, ErrorMessage = "세션 에러입니다." }, JsonRequestBehavior.AllowGet);
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_RELEASE_TYPE", AUTH_MODE.AUTH_UPDATE, USERID))
            {
                return Json(new { Success = false, ErrorMessage = "수정권한이 없습니다." }, JsonRequestBehavior.AllowGet);
            }



            // 업로드 된 파일 체크
            if (uploadfile == null || uploadfile.ContentLength == 0)
            {
                return Json(new { Success = false, ErrorMessage = "파일크기가 0 입니다." }, JsonRequestBehavior.AllowGet);
            }

            // 파일확장자 체크
            string file_ext = GlobalFunction.GetFileExt(uploadfile.FileName).ToLower();
            if (file_ext != "xlsx" && file_ext != "xls")
            {
                return Json(new { Success = false, ErrorMessage = "Excel 파일을 선택하세요." }, JsonRequestBehavior.AllowGet);
            }

            // 엑셀파일 여부 체크
            //// *.xls  : application/vnd.ms-excel
            //// *.xlsx : application/octet-stream
            //string ContentType = file.ContentType;
            //if (ContentType != "application/vnd.ms-excel"
            //    && ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            //    && ContentType != "application/octet-stream"
            //    )
            //{
            //    ViewBag.Message = "Please select Excel file.";
            //    return View();
            //}

            // 파일크기 체크
            int FILE_SIZE_LIMIT = 1024 * 1024 * 1;  // 1MB
            if (uploadfile.ContentLength > FILE_SIZE_LIMIT)
            {
                return Json(new { Success = false, ErrorMessage = "파일크기는 1MB를 초과할수 없습니다." }, JsonRequestBehavior.AllowGet);
            }

            // 저장폴더 체크
            string folder_path = Server.MapPath("~/Content/_userfile/settings_basic_cost_table_upload");
            if (!System.IO.Directory.Exists(folder_path))
            {
                System.IO.Directory.CreateDirectory(folder_path);
            }

            // 기존에 생성된 파일들을 삭제한다(어제, 오늘 생성된 파일을 제외하고 나머지는 삭제한다)
            DateTime CURRENT_TIME = DateTime.Now;
            string date1 = CURRENT_TIME.ToString("yyyyMMdd");
            string date2 = CURRENT_TIME.AddDays(-1).ToString("yyyyMMdd");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folder_path);
            foreach (System.IO.FileInfo f in di.GetFiles())
            {
                if (f.Name.Substring(0, 8) != date1 && f.Name.Substring(0, 8) != date2)
                {
                    f.Delete();
                }
            }

            // 파일 저장
            string file_name = GlobalFunction.GetFileName(uploadfile.FileName);
            string file_path = System.IO.Path.Combine(folder_path, string.Format("{0}_{1}", CURRENT_TIME.ToString("yyyyMMddHHmmss"), file_name));
            uploadfile.SaveAs(file_path);
            if (!System.IO.File.Exists(file_path))
            {
                return Json(new { Success = false, ErrorMessage = "파일 업로드 실패!!!" }, JsonRequestBehavior.AllowGet);
            }



            // Excel 파일 읽어오기
            string err1 = "";
            DataTable DT_EXCEL = GlobalFunction.ImportExcelXLS(file_path, true, out err1);
            if (err1.Length > 0)
            {
                return Json(new { Success = false, ErrorMessage = "error : " + err1 }, JsonRequestBehavior.AllowGet);
            }
            if (DT_EXCEL.Rows.Count == 0)
            {
                return Json(new { Success = false, ErrorMessage = "데이터가 없습니다." }, JsonRequestBehavior.AllowGet);
            }
            if (DT_EXCEL.Columns.Count < 4)
            {
                return Json(new { Success = false, ErrorMessage = "컬럼갯수가 일치하지 않습니다.\n\n정확한 컬럼갯수 : 4" }, JsonRequestBehavior.AllowGet);
            }



            List<EstShippingFeeModels> SHIPPING_FEE_LIST = new List<EstShippingFeeModels>();

            for (int i = 0; i < DT_EXCEL.Rows.Count; i++)
            {
                EstShippingFeeModels SHIPPING_FEE = new EstShippingFeeModels();

                SHIPPING_FEE.WEIGHT = GlobalFunction.GetDouble(DT_EXCEL.Rows[i][0].ToString().Trim(), 3);  // 무게
                SHIPPING_FEE.CUSTOMS_FEE = GlobalFunction.GetDouble(DT_EXCEL.Rows[i][1].ToString().Trim(), 2);  // 통관배송비
                SHIPPING_FEE.SHIPPING_FEE_NOR = GlobalFunction.GetDouble(DT_EXCEL.Rows[i][2].ToString().Trim(), 2);  // 일반배송요금
                SHIPPING_FEE.SHIPPING_FEE_STC = GlobalFunction.GetDouble(DT_EXCEL.Rows[i][3].ToString().Trim(), 2);  // 보관배송요금

                SHIPPING_FEE_LIST.Add(SHIPPING_FEE);
            }

            // STATION 무게단위를 가져온다
            string WEIGHT_UNIT = EsmStationDatabase.GetWeightUnit(ESTCODE);

            var data = SHIPPING_FEE_LIST.Select(m => new {
                    WEIGHT = ((WEIGHT_UNIT == "KG") ? m.WEIGHT.ToString("0.000") : m.WEIGHT.ToString("0.0")),
                    CUSTOMS_FEE = 0.0,
                    SHIPPING_FEE_NOR = m.SHIPPING_FEE_NOR.ToString("#,##0.00"),
                    SHIPPING_FEE_STC = m.SHIPPING_FEE_STC.ToString("#,##0.00")
                }
            );

            return Json(new { Success = true, Message = "Excel 파일 읽어오기 성공.", Data = data }, JsonRequestBehavior.AllowGet);
        }










        //=========================================================================================
        //=========================================================================================
        // 기본 정보 - 현지 배송업체
        //=========================================================================================
        //=========================================================================================

        // 현지 배송업체
        public ActionResult LocalDelivery()
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_LOCAL_DELIVERY", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수

            // 쿠키값 체크
            if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_LOCAL_DELIVERY] != null)
            {
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_LOCAL_DELIVERY].Value != null)
                {
                    ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_LOCAL_DELIVERY].Value));
                }
            }

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(LocalDeliveryQueryResult(ITEMS_PER_PAGE, 1));
        }


        // 검색결과 리턴 : 현지 배송업체
        object LocalDeliveryQueryResult(
                    int ITEMS_PER_PAGE  // 한 페이지당 보여줄 갯수
                    , int SEARCH_PAGE  // 검색할 페이지
                    )
        {
            // 전체 갯수를 가져온다
            int total_count = ConfLocalDeliveryDatabase.GetLocalDeliveryDataCount();
            if (total_count <= 0)
                return null;

            // 페이지를 계산한다
            PagenationModels pager = new PagenationModels();
            pager.Pagenation(total_count, ITEMS_PER_PAGE, SEARCH_PAGE);

            // 기준통화 및 환율 데이터를 가져온다
            int offset = ((total_count > ITEMS_PER_PAGE) ? (pager.CurrentPage - 1) * pager.PageSize : 0);
            int limit = ((total_count > ITEMS_PER_PAGE) ? pager.PageSize : 0);
            List<ConfLocalDeliveryModels> list = ConfLocalDeliveryDatabase.GetLocalDeliveryDataList(offset, limit);
            if (list == null || list.Count == 0)
                return null;

            ConfLocalDeliveryListModels model = new ConfLocalDeliveryListModels();
            model.Pager = pager;
            model.Items = list;

            return model;
        }


        [HttpPost]
        public ActionResult LocalDelivery(
                    string items_per_page
                    , string search_page
                    )
        {
            // 세션변수 체크
            string ESTCODE = (Session["ESTCODE"] != null) ? Session["ESTCODE"].ToString().Trim() : "";  // STATION CODE
            string USERID = (Session["USERID"] != null) ? Session["USERID"].ToString().Trim() : "";  // 사용자 ID (email 주소)
            int USERPOWER = (Session["USERPOWER"] != null) ? int.Parse(Session["USERPOWER"].ToString().Trim()) : -1;  // 0=esm, 1=est
            if ((USERPOWER != 0 && USERPOWER != 1) || ESTCODE.Length == 0 || USERID.Length == 0)
            {
                FormsAuthentication.SignOut();
                Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            // 권한 체크
            if (USERPOWER == 1 && !EstGroupPermissionDatabase.AuthCheck("EST_SETTINGS_LOCAL_DELIVERY", AUTH_MODE.AUTH_SEARCH, USERID))
            {
                return View("PermissionError");
            }



            int ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;  // 한페이지에 보여줄 갯수
            int SEARCH_PAGE = ((search_page != null) ? GlobalFunction.GetInt(search_page.Trim()) : 1);
            if (items_per_page != null)
            {
                ITEMS_PER_PAGE = GlobalFunction.GetInt(items_per_page.Trim());
            }
            else
            {
                // 쿠키값 체크
                if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_LOCAL_DELIVERY] != null)
                {
                    if (Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_LOCAL_DELIVERY].Value != null)
                    {
                        ITEMS_PER_PAGE = GlobalFunction.GetInt(Server.HtmlEncode(Request.Cookies[EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_LOCAL_DELIVERY].Value));
                    }
                }
            }

            if (ITEMS_PER_PAGE <= 0) ITEMS_PER_PAGE = EstSettings.ITEMS_PER_PAGE_INIT;
            if (SEARCH_PAGE <= 0) SEARCH_PAGE = 1;

            // 쿠키 저장
            var Cookie1 = new HttpCookie(EstSettings.COOKIE_ITEMS_PER_PAGE_SETTINGS_LOCAL_DELIVERY);
            Cookie1.Value = ITEMS_PER_PAGE.ToString();
            Cookie1.Expires = DateTime.Now.AddDays(365);
            //Cookie1.HttpOnly = true;
            //Cookie1.Secure = true;
            //context.Response.SetCookie(Cookie1);
            Response.Cookies.Add(Cookie1);

            ViewData["ITEMS_PER_PAGE"] = ITEMS_PER_PAGE;  // 한페이지에 보여줄 갯수
            ViewData["ITEMS_PER_PAGE_LIST"] = EstFunction.GetPages_SelectList();  // 페이지 갯수 목록

            return View(LocalDeliveryQueryResult(ITEMS_PER_PAGE, SEARCH_PAGE));
        }
    }
}