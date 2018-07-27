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

using comm_global;

using web_est.Models;

#endregion

namespace web_est.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // TODO: This should be moved to the constructor of the controller in combination with a DependencyResolver setup
        // NOTE: You can use NuGet to find a strategy for the various IoC packages out there (i.e. StructureMap.MVC5)
        private readonly UserManager _manager = UserManager.Create();

        // GET: /account/forgotpassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            // 로그인이 이미 되어있는 상태인지 체크
            EnsureLoggedOut();

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

            return View(viewModel);
        }

        // POST: /account/login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountLoginModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(viewModel);

            //// Verify if a user exists with the provided identity information
            //var user = await _manager.FindByEmailAsync(viewModel.Email);

            //// If a user was found
            //if (user != null)
            //{
            //    // Then create an identity for it and sign it in
            //    await SignInAsync(user, viewModel.RememberMe);

            //    // If the user came from a specific page, redirect back to it
            //    return RedirectToLocal(viewModel.ReturnUrl);
            //}

            //// No existing user was found that matched the given criteria
            //ModelState.AddModelError("", "Invalid username or password.");

            //// If we got this far, something failed, redisplay form
            //return View(viewModel);



            // E-Mail 유효성 체크
            if (!GlobalFunction.EMailValidCheck(viewModel.Email))
            {
                ModelState.AddModelError("", "이메일 주소가 유효하지 않습니다.");
                return View(viewModel);
            }

            // 사용자 정보를 가져온다
            EstUserModels EstUser = EstUserDatabase.GetUserData(viewModel.Email, viewModel.Password);
            if (EstUser == null)
            {
                ModelState.AddModelError("", "DB 연결 에러입니다. 잠시후 다시 시도하십시요.");
                return View(viewModel);
            }
            if (EstUser.EMAIL.Length == 0)
            {
                ModelState.AddModelError("", "이메일 또는 비밀번호가 일치하지 않습니다.");
                return View(viewModel);
            }

            // 사용자 상태 체크
            if (EstUser.STATUS != 0)  // 상태(0=사용중, 1=중지됨)
            {
                ModelState.AddModelError("", "사용자 로그인이 거부되었습니다. 관리자에게 문의하시기 바랍니다.");
                return View(viewModel);
            }



            // STATION 정보를 가져온다
            EsmStationModels StationInfo = EsmStationDatabase.GetStationData(EstUser.EST_CODE);
            if (StationInfo == null)
            {
                ModelState.AddModelError("", "DB 연결 에러입니다. 잠시후 다시 시도하십시요.");
                return View(viewModel);
            }
            if (StationInfo.EST_CODE.Length == 0)
            {
                ModelState.AddModelError("", "STATION 정보를 가져올수 없습니다.");
                return View(viewModel);
            }

            // STATION 상태 체크
            if (StationInfo.STATUS != 0)  // 상태(0 = 사용중, 1 = 중지됨)
            {
                ModelState.AddModelError("", "STATION 로그인이 거부되었습니다. 관리자에게 문의하시기 바랍니다.");
                return View(viewModel);
            }



            // 로그인 처리
            FormsAuthentication.SetAuthCookie(viewModel.Email, false);

            // 로그인 정보 저장
            CommLoginLogModels LoginLog = new CommLoginLogModels();
            LoginLog.EST_CODE = EstUser.EST_CODE;        //	varchar(5)			STATION 코드	
            LoginLog.ESE_CODE = "";        //	varchar(8)			SENDER 코드	
            LoginLog.EMAIL = EstUser.EMAIL;       //	varchar(50)			사용자 이메일	
            LoginLog.LOGDATETIME = "";     //	datetime			로그인 시간	
            LoginLog.IPADDR = Request.ServerVariables["REMOTE_ADDR"].ToString().Trim();      //	varchar(20)			로그인 IP	
            LoginLog.TYPE = "est";        //	varchar(10)			입력구분
            CommLoginLogDatabase.SaveLoginLog(LoginLog);

            // 세션 변수 저장
            Session["ESTCODE"] = StationInfo.EST_CODE;  // STATION CODE
            Session["ESTNAME"] = StationInfo.EST_NAME;  // STATION 이름
            Session["USERID"] = EstUser.EMAIL;  // 사용자 ID (email 주소)
            Session["USERNAME"] = EstUser.USERNAME;  // 사용자 이름
            Session["USERPOWER"] = 1;  // 0=esm, 1=est

            // 시작페이지로 이동
            if (Url.IsLocalUrl(viewModel.ReturnUrl) && viewModel.ReturnUrl.Length > 1 && viewModel.ReturnUrl.StartsWith("/")
                            && !viewModel.ReturnUrl.StartsWith("//") && !viewModel.ReturnUrl.StartsWith("/\\"))
            {
                return Redirect(viewModel.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
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
        public async Task<ActionResult> Register(AccountRegistrationModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(viewModel);

            // Prepare the identity with the provided information
            var user = new IdentityUser
            {
                UserName = viewModel.Username ?? viewModel.Email,
                Email = viewModel.Email
            };

            // Try to create a user with the given identity
            try
            {
                var result = await _manager.CreateAsync(user, viewModel.Password);

                // If the user could not be created
                if (!result.Succeeded) {
                    // Add all errors to the page so they can be used to display what went wrong
                    AddErrors(result);

                    return View(viewModel);
                }

                // If the user was able to be created we can sign it in immediately
                // Note: Consider using the email verification proces
                await SignInAsync(user, false);

                return RedirectToLocal();
            }
            catch (DbEntityValidationException ex)
            {
                // Add all errors to the page so they can be used to display what went wrong
                AddErrors(ex);

                return View(viewModel);
            }
        }

        // POST: /account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // First we clean the authentication ticket like always
            FormsAuthentication.SignOut();
            Session.Clear();

            // Second we clear the principal to ensure the user does not retain any authentication
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
            // this clears the Request.IsAuthenticated flag since this triggers a new request
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

        // 로그인이 이미 되어있는 상태인지 체크한다
        private void EnsureLoggedOut()
        {
            if (Request.IsAuthenticated)  // 이미 로그인된 상태이면 ==> 로그아웃
                Logout();

            if (Session["ESTCODE"] == null)  // 세션이 종료된 경우 ==> 로그아웃
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