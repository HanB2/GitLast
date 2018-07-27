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
using web_ese.Models;
using web_ese.Models_Db;
using comm_model;
using System.Threading;

#endregion

namespace web_ese.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager _manager = UserManager.Create();
		AccountDbModels act = new AccountDbModels();

		// GET: /account/forgotpassword
		[AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View();
        }


		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ForgotPassword(string email)
		{
			//이메일을 검사하여 유효한 경우 패스워드를 초기화 한 후 이메일 전송
			string resultStr = act.resetPassword(email);
			ViewBag.PublicMsg = resultStr;
			return View();
		}




		// GET: /account/login
		[AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            // Store the originating URL so we can attach it to a form field
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };

			// 언어설정
			//CKBIZ_Comm.Language.resLanguage.Culture = System.Globalization.CultureInfo.CreateSpecificCulture(SELECT_LANG);
			Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ko-KR");
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

			return View(viewModel);
        }

        // POST: /account/login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLoginModel viewModel)
		{
			if (!ModelState.IsValid)
				return View(viewModel);

			string resultStr = "";
			EseUser model = new EseUser();
			model = act.loginChk(viewModel);    //로그인 체크



			if (model == null)      //아이디 페스워드 체크
			{
				resultStr = "이메일 또는 비밀번호를 확인해 주시기 바랍니다.";
				ViewBag.PublicMsg = resultStr;
				return View(viewModel);
			}

			if (model.chkSTATUS == 1)  //사용 여부 체크
			{
				resultStr = "사용이 정지된 계정입니다.";
				ViewBag.PublicMsg = resultStr;
				return View(viewModel);
			}

			if (model.STATUS == 1)  //사용 여부 체크
			{
				resultStr = "사용이 정지된 ESE 계정입니다.";
				ViewBag.PublicMsg = resultStr;
				return View(viewModel);
			}

			if (model.STATUS == 2)  //사용 여부 체크
			{
				//resultStr = "[문구 수정 필요]승인되지 않은 사용자 입니다. 기본정보를 입력해주시면 빠른시일에 승인 처리 해드리겠습니다.";
				//ViewBag.PublicMsg = resultStr;
			}

			FormsAuthentication.SetAuthCookie(viewModel.Email, false);

			Session["MANAGE_NO"] = model.SEQNO;
			Session["MANAGE_GRADE"] = model.GROUP_ID;
			Session["CURRENT_LOGIN_EMAIL"] = model.EMAIL;
			Session["EST_CODE"] = model.EST_CODE;
			Session["ESE_CODE"] = model.ESE_CODE;
			Session["STATUS"] = model.STATUS;

			//로그인 유지 체크 시 쿠키 설정
			if (viewModel.RememberMe)
			{
				Response.Cookies["CHK_LOGIN_REMEMBER"].Value = "CHK_LOGIN_REMEMBER";
				Response.Cookies["MANAGE_NO"].Value = model.SEQNO.ToString();
				Response.Cookies["MANAGE_GRADE"].Value = model.GROUP_ID.ToString();
				Response.Cookies["CURRENT_LOGIN_EMAIL"].Value = model.EMAIL;
				Response.Cookies["EST_CODE"].Value = model.EST_CODE;
				Response.Cookies["ESE_CODE"].Value = model.ESE_CODE;
				Response.Cookies["STATUS"].Value = model.STATUS.ToString();
			}
			else
			{
				//로그인 유지 체크 헤제 시 쿠키 삭제
				if (Request.Cookies["CHK_LOGIN_REMEMBER"] != null)
					Response.Cookies["CHK_LOGIN_REMEMBER"].Expires = DateTime.Now.AddDays(-1);
				if (Request.Cookies["MANAGE_NO"] != null)
					Response.Cookies["MANAGE_NO"].Expires = DateTime.Now.AddDays(-1);
				if (Request.Cookies["MANAGE_GRADE"] != null)
					Response.Cookies["MANAGE_GRADE"].Expires = DateTime.Now.AddDays(-1);
				if (Request.Cookies["CURRENT_LOGIN_EMAIL"] != null)
					Response.Cookies["CURRENT_LOGIN_EMAIL"].Expires = DateTime.Now.AddDays(-1);
				if (Request.Cookies["EST_CODE"] != null)
					Response.Cookies["EST_CODE"].Expires = DateTime.Now.AddDays(-1);
				if (Request.Cookies["ESE_CODE"] != null)
					Response.Cookies["ESE_CODE"].Expires = DateTime.Now.AddDays(-1);
				if (Request.Cookies["STATUS"] != null)
					Response.Cookies["STATUS"].Expires = DateTime.Now.AddDays(-1);
			}
			


			//로그인 기록 데이터 세팅
			CommLoginLog clh = new CommLoginLog();
			clh.ESE_CODE = model.ESE_CODE;
			clh.EMAIL = viewModel.Email;
			clh.IPADDR = Request.UserHostAddress;

			act.loginHis(clh);    //로그인 로그 기록

			return RedirectToLocal(viewModel.ReturnUrl);
			//return RedirectToAction("Index", "Home");
		}

        // GET: /account/error
        [AllowAnonymous]
        public ActionResult Error()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View();
        }

        // GET: /account/register
        [AllowAnonymous]
        public ActionResult Register()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View(new AccountRegistrationModel());
        }

        // POST: /account/register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountRegistrationModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);



			if (act.chkDuplEmail(viewModel))//아이디 중복 체크
			{
				TempData["PublicMsg"] = "중복된 이메일 계정입니다.";
				return View(viewModel);
			}

			if (act.chkEstCode(viewModel))//EST코드 유효성 체크 (빈값이면 패스 빈값이 아닐경우 유효한 EST코드 인지 확인)
			{
				TempData["PublicMsg"] = "해당 EST가 없습니다.";
				return View(viewModel);
			}


			if (!act.setRegister(viewModel)) //계정 등록 실패 시
			{
				TempData["PublicMsg"] = "등록에 실패 하였습니다. 관리자에게 문의해주세요.";
				return View(viewModel);
			}

			//가입이 성공적으로 완료 되었을 경우
			TempData["PublicMsg"] = "회원가입이 성공하였습니다.";
			return RedirectToLocal();
		}

        // POST: /account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // First we clean the authentication ticket like always
            FormsAuthentication.SignOut();

            // Second we clear the principal to ensure the user does not retain any authentication
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

			// Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
			// this clears the Request.IsAuthenticated flag since this triggers a new request


			if (Request.Cookies["CHK_LOGIN_REMEMBER"] != null)
				Response.Cookies["CHK_LOGIN_REMEMBER"].Expires = DateTime.Now.AddDays(-1);
			if (Request.Cookies["MANAGE_NO"] != null)
				Response.Cookies["MANAGE_NO"].Expires = DateTime.Now.AddDays(-1);
			if (Request.Cookies["MANAGE_GRADE"] != null)
				Response.Cookies["MANAGE_GRADE"].Expires = DateTime.Now.AddDays(-1);
			if (Request.Cookies["CURRENT_LOGIN_EMAIL"] != null)
				Response.Cookies["CURRENT_LOGIN_EMAIL"].Expires = DateTime.Now.AddDays(-1);
			if (Request.Cookies["EST_CODE"] != null)
				Response.Cookies["EST_CODE"].Expires = DateTime.Now.AddDays(-1);
			if (Request.Cookies["ESE_CODE"] != null)
				Response.Cookies["ESE_CODE"].Expires = DateTime.Now.AddDays(-1);
			if (Request.Cookies["STATUS"] != null)
				Response.Cookies["STATUS"].Expires = DateTime.Now.AddDays(-1);



			return RedirectToLocal();
        }

        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // If we cannot verify if the url is local to our host we redirect to a default location
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
            // Add all errors that were returned to the page error collection
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }

        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            // Create a claims based identity for the current user
            var identity = await _manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(identity.Name, isPersistent);
        }

        // GET: /account/lock
        [AllowAnonymous]
        public ActionResult Lock()
        {
            return View();
        }
    }
}