using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_ese_old.Controllers
{
    public partial class MarController : Controller
    {
        // GET: Mar MAR 충전 (계좌이체)
        public ActionResult MarChargereq()
        {
            return View();
        }
    }
}