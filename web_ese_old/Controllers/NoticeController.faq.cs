using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_ese_old.Controllers
{
    public partial class NoticeController : Controller
    {
        // GET: Notice 자주묻는 질문
        public ActionResult NoticeFaq()
        {
            return View();
        }
    }
}