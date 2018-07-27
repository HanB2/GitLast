#region Using

using System.Web.Mvc;
using web_esm.Filters;

#endregion

namespace web_esm
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
			filters.Add(new CustomerFilter());
			

		}
    }
}