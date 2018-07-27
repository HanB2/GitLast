using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_ese_old.Controllers
{
    public partial class StcProdController : Controller
    {
        // GET: StcProd 상품조회
        public ActionResult StcProdList()
        {
            return View();
        }
    }
}