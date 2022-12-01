using ApiServer.Comm;
using AuthServer;
using AuthServer.Models;
using Cache;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;
using System.Reflection;

namespace ApiServer.Filters;

/// <summary>
/// 权限过滤
/// </summary>
public class AuthenticationFilter : IActionFilter
{
    private readonly static ConcurrentDictionary<string, EnumCustomAuthorize> concurrentDictionary = new();

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        string pathlow = context.HttpContext.Request.Path.Value.ToLower();

        EnumCustomAuthorize enumCustomAuthorize = GetEnumCustomAuthorize(pathlow, context);

        switch (enumCustomAuthorize)
        {
            case EnumCustomAuthorize.None: break;
            case EnumCustomAuthorize.Normal: NormalAuthorize(pathlow, context); break;
        }
    }

    /// <summary>
    /// 判断该接口是否需要验证
    /// </summary>
    /// <param name="path"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    private static EnumCustomAuthorize GetEnumCustomAuthorize(string path, ActionExecutingContext context)
    {
        if (concurrentDictionary.TryGetValue(path, out EnumCustomAuthorize enumCustomAuthorize)) return enumCustomAuthorize;

        CustomAttributeData customAttributeData = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.CustomAttributes.FirstOrDefault(l => l.AttributeType == typeof(CustomAuthorizeAttribute))
            ?? (context.ActionDescriptor as ControllerActionDescriptor).ControllerTypeInfo.CustomAttributes.FirstOrDefault(l => l.AttributeType == typeof(CustomAuthorizeAttribute));

        enumCustomAuthorize = customAttributeData == null ? EnumCustomAuthorize.Normal : (EnumCustomAuthorize)(customAttributeData.ConstructorArguments[0].Value);

        concurrentDictionary.TryAdd(path, enumCustomAuthorize);
        return enumCustomAuthorize;
    }

    /// <summary>
    /// 正常验证, 只要有Token,且Token有效就可以
    /// </summary>
    /// <param name="path"></param>
    /// <param name="context"></param>
    private static void NormalAuthorize(string path, ActionExecutingContext context)
    {
        string msg = CheckToken(context);

        if (string.IsNullOrWhiteSpace(msg)) return;

        string address = context.HttpContext.GetRemoteHost();
        string guid = context.HttpContext.Request.Headers["tempGuid"];
        context.Result = HttpContextExtention.ApiError(msg, 103);

        ApiStatistics.Authentication(path);
        LogHelper.WriteLog($" \t IP:{address} \t PATH:{path} \t Guid:{guid}", "logs/Authentication", DateTime.Now.ToDefaultDateString() + ".log");
    }

    /// <summary>
    /// 检查Token
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static string CheckToken(ActionExecutingContext context)
    {
        string token = context.HttpContext.Request.Headers["token"];
        if (string.IsNullOrWhiteSpace(token)) return "token无效";

        UserBasicModel onlinePeople = StaticAuth.Auth.GetUserByToken<UserBasicModel>(token);
        if (onlinePeople == null) return "token已过期";

        return null;
    }
}

/// <summary>
/// 自定义权限验证
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : Attribute
{
    public EnumCustomAuthorize EnumAuthorizeAttribute { get; set; }

    /// <summary>
    /// 自定义权限验证
    /// </summary>
    /// <param name="enumAuthorizeAttribute">验证方式</param>
    public CustomAuthorizeAttribute(EnumCustomAuthorize enumAuthorizeAttribute = EnumCustomAuthorize.Normal)
    {
        EnumAuthorizeAttribute = enumAuthorizeAttribute;
    }
}

/// <summary>
/// 自定义权限验证枚举
/// </summary>
public enum EnumCustomAuthorize
{
    /// <summary>
    /// 免验证
    /// </summary>
    None = 0,

    /// <summary>
    /// 正常验证, 只要有Token,且Token有效就可以
    /// </summary>
    Normal = 1,
}