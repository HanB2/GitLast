using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_est.Controllers
{
    // 출고 관리
    [Authorize]
    public class OrderOutController : Controller
    {
        // Master B/L - Master B/L 관리
        public ActionResult MasterBL()
        {
            return View();
        }


        // Master B/L - Master 번호설정
        public ActionResult MasterSet()
        {
            return View();
        }


        // 출고관리
        public ActionResult Release()
        {
            return View();
        }


        // 출고데이터 엑셀 출력
        public ActionResult ExcelExport()
        {
            return View();
        }


        // 배송상태 업데이트 - YTO GLOBAL 상태 업데이트
        public ActionResult StatusUpdateYTOG()
        {
            return View();
        }


        // 배송상태 업데이트 - YTO 내륙 상태 업데이트
        public ActionResult StatusUpdateYTOL()
        {
            return View();
        }


        // 배송상태 업데이트 - DD Express 상태 업데이트
        public ActionResult StatusUpdateDD()
        {
            return View();
        }
    }
}