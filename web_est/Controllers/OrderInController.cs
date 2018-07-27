using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_est.Controllers
{
    // 입고 관리
    [Authorize]
    public class OrderInController : Controller
    {
        // 일반배송 관리 - 접수목록 조회
        public ActionResult NorOrderList()
        {
            return View();
        }


        // 일반배송 관리 - 일반배송 입고
        public ActionResult NorOrderIn()
        {
            return View();
        }


        // 재고배송 관리 - 접수목록 조회
        public ActionResult StockOrderList()
        {
            return View();
        }


        // 재고배송 관리 - Packing 검수
        public ActionResult Packing()
        {
            return View();
        }


        // 무게변경 및 송장출력 ==> exe
        public ActionResult WeightModify()
        {
            return View();
        }


        // 운송장 출력 ==> exe
        public ActionResult LabelPrint()
        {
            return View();
        }


        // 상세 검색
        public ActionResult DetailSearch()
        {
            return View();
        }
    }
}