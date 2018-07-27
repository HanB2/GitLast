using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_est.Controllers
{
    // 보관 관리
    [Authorize]
    public class StockController : Controller
    {
        // 상품 목록
        public ActionResult ProductList()
        {
            return View();
        }


        // 보관신청 조회
        public ActionResult KeepReq()
        {
            return View();
        }


        // 상품 입고
        public ActionResult ProductIn()
        {
            return View();
        }


        // 재고 현황
        public ActionResult Inventory()
        {
            return View();
        }


        // 입출고 현황
        public ActionResult InOut()
        {
            return View();
        }
    }
}