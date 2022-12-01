using ApiServer.Comm;
using Cache;
using System.Diagnostics;

namespace ApiServer.Middlewares;

/// <summary>
/// 访问次数统计
/// </summary>
public class StatisticsAccessCountMiddleware
{
    private readonly RequestDelegate _next;

    public StatisticsAccessCountMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        string guid = Guid.NewGuid().ToString("N");
        string pathlow = httpContext.Request.Path.Value.ToLower();
        string address = httpContext.GetRemoteHost();
        string token = httpContext.Request.Headers["token"];
        string device = httpContext.Request.Headers["deviceid"];
        httpContext.Request.Headers["tempGuid"] = guid;
        long mem = Process.GetCurrentProcess().WorkingSet64;

        httpContext.Request.EnableBuffering();
        using StreamReader reader = new(httpContext.Request.Body);
        string body = reader.ReadToEndAsync().Result;
        httpContext.Request.Body.Position = 0;

        Stream stream = httpContext.Response.Body;
        using MemoryStream ms = new();
        httpContext.Response.Body = ms;

        await _next(httpContext);

        ms.Position = 0;
        using StreamReader reader1 = new(ms);
        string body1 = reader1.ReadToEndAsync().Result;
        ms.Position = 0;
        await ms.CopyToAsync(stream);
        httpContext.Response.Body = stream;

        stopwatch.Stop();

        ApiStatistics.StatisticsAccess(pathlow, stopwatch.Elapsed);
        LogHelper.WriteLog($" \t IP:{address} \t PATH:{pathlow} \t Guid:{guid} \t Mem:{mem} \t Time:{stopwatch.ElapsedMilliseconds} \t Token:{token} \t DeviceID:{device} \t Request:{body} \t Response:{body1}", "logs/AccessLog", DateTime.Now.ToDefaultDateString() + ".log");

        if (stopwatch.ElapsedMilliseconds > 1000)
        {
            LogHelper.WriteLog($" \t IP:{address} \t PATH:{pathlow} \t Guid:{guid} \t Mem:{mem} \t Time:{stopwatch.ElapsedMilliseconds} \t Token:{token} \t DeviceID:{device} \t Request:{body} \t Response:{body1}", "logs/AccessLog", DateTime.Now.ToDefaultDateString() + "_1000.log");
        }
    }
}