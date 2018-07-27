using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using web_ese_old.Models;
using web_ese_old.Filter;

namespace web_ese_old.Controllers
{
	[Authorize]
	public class LoginController : Controller
    {

		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;

		public LoginController()
		{
		}

		public LoginController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}


		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}



		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginTempModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// 계정이 잠기는 로그인 실패로 간주되지 않습니다.
			// 암호 오류 시 계정 잠금을 트리거하도록 설정하려면 shouldLockout: true로 변경하십시오.
			var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
			switch (result)
			{
				case SignInStatus.Success:
					//return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "잘못된 로그인 시도입니다.");
					return View(model);
			}
		}




		// GET: Login
		public ActionResult FindPW()
		{
			return View();
		}


		// GET: Login
		public ActionResult Join()
		{
			return View();
		}
	}
}