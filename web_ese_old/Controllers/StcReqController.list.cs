using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_ese_old.Controllers
{
    public partial class StcReqController : Controller
    {
        //보관신쳥현황 조회
        public ActionResult StcReqList()
        {
            return View();
        }


		//입출고 내역 조회
		public ActionResult StcInOut()
		{
			return View();
		}
	}
}