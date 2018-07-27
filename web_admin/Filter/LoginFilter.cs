
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace web_ese.Filter
{


	public class LoginFilter : ActionFilterAttribute, IAuthenticationFilter
	{
		public void OnAuthentication(AuthenticationContext filterContext)
		{

			//For demo purpose only. In real life your custom principal might be retrieved via different source. i.e context/request etc.
			//filterContext.Principal = new UserPrincipal(filterContext.HttpContext.User.Identity, new[] { "Admin" }, "Red");
			if (!filterContext.Principal.Identity.IsAuthenticated)
				filterContext.Result = new HttpUnauthorizedResult();
		}

		public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
		{
			//var color = ((UserPrincipal)filterContext.HttpContext.User).HairColor;
			var user = filterContext.HttpContext.User;
			/*
			if (!user.Identity.IsAuthenticated)
			{
				//실패시 로그인 페이지 이동
				//filterContext.Result = new RedirectResult("/Login/Login"); 
			}
			*/
		}
	}
}