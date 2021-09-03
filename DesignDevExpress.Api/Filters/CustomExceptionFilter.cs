using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using DevExpress.DashboardCommon;

namespace DesignDevExpress.Api.Filters
{
    public class CustomExceptionFilter : IExceptionFilter {
        IWebHostEnvironment Env;
        public CustomExceptionFilter(IWebHostEnvironment hostingEnvironment) {
            Env = hostingEnvironment;
        }

        public void OnException(ExceptionContext context) {
            AddToLog(context.Exception, Path.Combine(Env.ContentRootPath, "log", "Error.log"));
        }

        public static void AddToLog(Exception exception, string path) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToLocalTime().ToString("F"));
            GetExceptionInfo(exception, sb);
            sb.AppendLine("------------------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(path, sb.ToString());
        }

        private static void GetExceptionInfo(Exception exception, StringBuilder sb) {
            sb.AppendLine(exception.GetType().ToString());
            sb.AppendLine(exception.Message);
            sb.AppendLine("Stack Trace: ");
            sb.AppendLine(exception.StackTrace);
            if (exception is DashboardDataLoadingException) {
                foreach (var dataLoadingError in ((DashboardDataLoadingException)exception).Errors) {
                    sb.AppendLine("InnerException: ");
                    GetExceptionInfo(dataLoadingError.InnerException, sb);
                }
            }
            if (exception.InnerException != null) {
                sb.AppendLine("InnerException: ");
                GetExceptionInfo(exception.InnerException, sb);
            }
        }
    }
}