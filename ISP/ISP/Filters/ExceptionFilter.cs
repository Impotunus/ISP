using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace ISP.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var fileName = "ExceptionLogs.txt";
            
            var path = filterContext.HttpContext.Server.MapPath("~/Logs/") + fileName;
            var logFile = new FileInfo(path);
            if (!logFile.Exists)
            {
                File.Create(path);
            }
            using (var writer = new StreamWriter(path, true, Encoding.Unicode))
            {
                writer.Write($"------------------------------------------------------------- {writer.NewLine}");

                writer.Write($"{DateTimeOffset.UtcNow.Date.ToLongDateString()} Exception: {filterContext.Exception.Message} {writer.NewLine}");
                writer.Write($"Handled: {filterContext.ExceptionHandled}.{writer.NewLine}");
                writer.Write($"Result: {filterContext.Result} {writer.NewLine}{writer.NewLine}");
            }

            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }
    }
}