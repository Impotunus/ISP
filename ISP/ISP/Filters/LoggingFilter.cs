using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ISP.Filters
{
    public class LoggingFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var fileName = "Logs.txt";
            var path = HttpContext.Current.Server.MapPath("~/Logs/") + fileName;
            var logFile = new FileInfo(path);
            if (!logFile.Exists)
            {
                File.Create(path);
            }
            using (var writer = new StreamWriter(path, true, Encoding.Unicode))
            {
                writer.Write($"------------------------------------------------------------- {writer.NewLine}");
                
                writer.Write($"Controller: {filterContext.Controller}, Action: {filterContext.ActionDescriptor.ActionName} {writer.NewLine}");
                writer.Write($"Result: {filterContext.Result}, Date: {DateTimeOffset.UtcNow.LocalDateTime.ToLongDateString()} {writer.NewLine}");
            }
        }
    }
}