#region Using

using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using web_esm.Models;
using web_esm.Models_Db;
using comm_model;

#endregion

namespace web_esm.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private readonly UserManager _manager = UserManager.Create();
		
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			// 로그인이 이미 되어있는 상태인지 체크
			EnsureLoggedOut();

			return View();
		}
		
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			EnsureLoggedOut();
			
			var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };

			return View(viewModel);
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(AccountLoginModel viewModel)
		{
			if (!ModelState.IsValid)
				return View(viewModel);

			string resultStr = "";
			AccountDbModels act = new AccountDbModels();
			EsmUser model = new EsmUser();
			model = act.loginChk(viewModel);    //로그인 체크



			if (model == null)		//아이디 페스워드 체크
			{
				resultStr = "이메일 또는 비밀번호를 확인해 주시기 바랍니다.";
				ViewBag.PublicMsg = resultStr;
				return View(viewModel);
			}

			if (model.STATUS == 0)  //사용 여부 체크
			{
				resultStr = "사용이 정지된 계정입니다.";
				ViewBag.PublicMsg = resultStr;
				return View(viewModel);
			}
			

			FormsAuthentication.SetAuthCookie(viewModel.Email, false);

			Session["MANAGE_NO"] = model.SEQNO;
			Session["MANAGE_GRADE"] = model.GROUP_ID;
			Session["CURRENT_LOGIN_EMAIL"] = model.EMAIL;
			
			//로그인 기록 데이터 세팅
			CommLoginLog clh = new CommLoginLog();
			clh.EMAIL = viewModel.Email;
			clh.IPADDR = Request.UserHostAddress;

			act.loginHis(clh);    //로그인 로그 기록
			
			return RedirectToAction("Index", "Home");
		}
		
		[AllowAnonymous]
		public ActionResult Error()
		{
			EnsureLoggedOut();

			return View();
		}
		
		[AllowAnonymous]
		public ActionResult Register()
		{
			EnsureLoggedOut();
			ViewBag.PublicMsg = "관리자 신규 등록은 이투마스에 문의해 주시기 바랍니다.";
			return View(new AccountRegistrationModel());
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(AccountRegistrationModel viewModel)
		{
			ViewBag.PublicMsg = "관리자 신규 등록은 이투마스에 문의해 주시기 바랍니다.";
			return View(viewModel);
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			Session.Clear();
			
			HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
			
			return RedirectToLocal();
		}

		private ActionResult RedirectToLocal(string returnUrl = "")
		{
			if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
				return Redirect(returnUrl);
			
			return RedirectToAction("index", "home");
		}

		private void AddErrors(DbEntityValidationException exc)
		{
			foreach (var error in exc.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors.Select(validationError => validationError.ErrorMessage)))
			{
				ModelState.AddModelError("", error);
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		// 로그인이 이미 되어있는 상태인지 체크한다
		private void EnsureLoggedOut()
		{
			if (Request.IsAuthenticated)  // 이미 로그인된 상태이면 ==> 로그아웃
				Logout();

			if (Session["NAME"] == null)
				Logout();
		}

		private async Task SignInAsync(IdentityUser user, bool isPersistent)
		{
			FormsAuthentication.SignOut();
			
			var identity = await _manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
			
			FormsAuthentication.SetAuthCookie(identity.Name, isPersistent);
		}
		
		[AllowAnonymous]
		public ActionResult Lock()
		{
			return View();
		}
	}
}