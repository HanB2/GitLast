using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_ese_old.Controllers
{
    public partial class PicReqController : Controller
    {
        // GET: PicReq 픽업정보 팝업
        public ActionResult PicReqInfo()
        {
            return View();
        }
    }
}