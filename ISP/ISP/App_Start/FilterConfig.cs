using System.Web.Mvc;
using ISP.Filters;

namespace ISP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoggingFilter());
            filters.Add(new ExceptionFilter());
        }
    }
}
