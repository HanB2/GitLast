#region Using

using System.Web.Mvc;
using web_ese.Filters;

#endregion

namespace web_ese
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