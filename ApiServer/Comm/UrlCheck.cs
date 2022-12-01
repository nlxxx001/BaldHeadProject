using Cache;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using Util;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ApiServer.Comm
{
    public class UrlCheck : IActionFilter
    {
        string param;
        Stopwatch stopwatch = new Stopwatch();
        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopwatch.Stop();

            string response = context?.Result is ObjectResult result ? result?.Value?.ToJson() : null;
            string address = GetRemoteHost(context);
            string pathlow = context.HttpContext.Request.Path.Value.ToLower();
            string token = context.HttpContext.Request.Headers["token"];
            string device = context.HttpContext.Request.Headers["deviceid"];
            long mem = Process.GetCurrentProcess().WorkingSet64;

            //请求日志和统计
            LogHelper.WriteLog($" \t IP:{address} \t Time:{stopwatch.ElapsedMilliseconds} \t PATH:{pathlow} \t Mem:{mem} \t Token:{token} \t DeviceID:{device} \t Request:{param} \t Response:{response}", "logs/AccessLog", DateTime.Now.ToDefaultDateString() + ".log");
            if (stopwatch.ElapsedMilliseconds > 1000)
            {
                LogHelper.WriteLog($" \t IP:{address} \t Duration:{stopwatch.ElapsedMilliseconds} \t PATH:{pathlow} \t Token:{token} \t DeviceID:{device} \t Request:{param} \t Response:{response}", "logs/AccessLog", DateTime.Now.ToDefaultDateString() + "_1000.log");
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetRemoteHost(FilterContext context)
        {
            string ip = context.HttpContext.Request.Headers["x-forwarded-for"];
            if (ip == null || ip.Length == 0 || "unknown".Equals(ip))
            {
                ip = context.HttpContext.Request.Headers["Proxy-Client-IP"];
            }
            if (ip == null || ip.Length == 0 || "unknown".Equals(ip))
            {
                ip = context.HttpContext.Request.Headers["WL-Proxy-Client-IP"];
            }
            if (ip == null || ip.Length == 0 || "unknown".Equals(ip))
            {
                ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip.Equals("0:0:0:0:0:0:0:1") ? "127.0.0.1" : ip;
        }
    }

    /// <summary>
    /// 访问限制规则特性
    /// </summary>
    public class UrlAllowAttribute : Attribute
    {
        /// <summary>
        /// 最短间隔时间,毫秒
        /// </summary>
        public int IntervalTime { get; set; }

        /// <summary>
        /// 访问限制规则特性
        /// </summary>
        /// <param name="intervalTime">最短间隔时间,毫秒</param>
        public UrlAllowAttribute(int intervalTime = 0)
        {
            IntervalTime = intervalTime;
        }
    }
}
