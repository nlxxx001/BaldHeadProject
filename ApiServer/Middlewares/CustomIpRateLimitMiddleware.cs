using ApiServer.Comm;
using AspNetCoreRateLimit;
using Cache;
using Microsoft.Extensions.Options;

namespace ApiServer.Middlewares;

/// <summary>
/// 自定义限流中间件
/// </summary>
public class CustomIpRateLimitMiddleware : RateLimitMiddleware<IpRateLimitProcessor>
{
    public CustomIpRateLimitMiddleware(RequestDelegate next,
        IProcessingStrategy processingStrategy,
        IOptions<IpRateLimitOptions> options,
        IRateLimitCounterStore counterStore,
        IIpPolicyStore policyStore,
        IRateLimitConfiguration config
    ) : base(next, options?.Value, new IpRateLimitProcessor(options?.Value, counterStore, policyStore, config, processingStrategy), config) { }

    /// <summary>
    /// 记录日志    限制时返回的内容在AppSettings里面配置,目前配置的错误代码为105
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="identity"></param>
    /// <param name="counter"></param>
    /// <param name="rule"></param>
    protected override void LogBlockedRequest(HttpContext httpContext, ClientRequestIdentity identity, RateLimitCounter counter, RateLimitRule rule)
    {
        string address = httpContext.GetRemoteHost();
        string pathlow = httpContext.Request.Path.Value.ToLower();
        string guid = httpContext.Request.Headers["tempGuid"];

        ApiStatistics.Interception(pathlow);
        LogHelper.WriteLog($" \t IP:{address} \t PATH:{pathlow} \t Guid:{guid}", "logs/InterceptionLog", DateTime.Now.ToDefaultDateString() + ".log");
    }
}