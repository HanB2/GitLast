using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_ese_old.Controllers
{
    public partial class NoticeController : Controller
    {
        // GET: Notice 자료실
        public ActionResult NoticeFile()
        {
            return View();
        }
    }
}