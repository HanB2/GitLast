using System;
using System.Security.Principal;

namespace web_ese.Filter
{
	internal class UserPrincipal : IPrincipal
	{
		private IIdentity identity;
		private string[] v1;
		public string HairColor { get; set; }


		public UserPrincipal(IIdentity identity, string[] v1, string HairColor)
		{
			this.identity = identity;
			this.v1 = v1;
			this.HairColor = HairColor;
		}

		public IIdentity Identity
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsInRole(string role)
		{
			throw new NotImplementedException();
		}
	}
}