﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_ese_old.Controllers
{
    public partial class StcReqController : Controller
    {
        //보관상품선택 팝업
        public ActionResult StcReqSelect()
        {
            return View();
        }
    }
}