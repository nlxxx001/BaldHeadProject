using AuthServer;

namespace ApiServer.Config;

/// <summary>
/// 基础配置
/// </summary>
public static class BaseConfig
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder BaseInit(this WebApplicationBuilder builder)
    {
        Util.Helper.HttpHelperUrl.Init(builder.Configuration);
        CacheInit(builder);
        return builder;
    }

    /// <summary>
    /// 缓存框架初始化
    /// </summary>
    public static void CacheInit(WebApplicationBuilder builder)
    {
        CacheHelper.RedisIndex = builder.Configuration["Environment:RedisSetting:Index"].ToInt();
        CacheHelper.RedisConfig = builder.Configuration["Environment:RedisSetting:ConnectStr"];

        CacheHelper.Init();
        StaticAuth.Init(CacheHelper.RedisConfig, CacheHelper.RedisIndex);
    }
}