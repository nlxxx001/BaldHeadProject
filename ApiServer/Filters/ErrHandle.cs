using ApiServer.Comm;
using Cache;
using Microsoft.AspNetCore.Mvc.Filters;
using Util.Model;

namespace ApiServer.Filters;

/// <summary>
/// 接口错误统一处理
/// </summary>
public class ErrorHandle : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        string address = context.HttpContext.GetRemoteHost();
        string path = context.HttpContext.Request.Path.Value.ToLower();
        string guid = context.HttpContext.Request.Headers["tempGuid"];

        string message = context.Exception.Message;
        string detail = context.Exception.ToString();

        //异常分类
        switch (context.Exception)
        {
            case ApiException a:
                context.Result = HttpContextExtention.ApiError(a.Message, a.Level, a.Tag);

                ApiStatistics.Error(path);
                LogHelper.WriteLog($" \t IP:{address} \t PATH:{path} \t Guid:{guid}\r\nMessage:{message}\r\nDetail:{detail}", "logs/AccessErrorLog", DateTime.Now.ToDefaultDateString() + ".log");
                break;

            default:
                context.Result = HttpContextExtention.ApiError("系统繁忙", 9, context.Exception.Message);

                ApiStatistics.FatalError(path);
                LogHelper.WriteLog($" \t IP:{address} \t PATH:{path} \t Guid:{guid}\r\nMessage:{message}\r\nDetail:{detail}", "logs/AccessErrorLog", DateTime.Now.ToDefaultDateString() + "_Fatal.log");
                break;
        }
    }
}