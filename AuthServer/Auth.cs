using AuthServer.Models;
using Util;
using Util.Helper;

namespace AuthServer;

/// <summary>
/// 学生验证类
/// </summary>
public class Auth
{
    #region 私有字段&属性&构造函数

    internal RedisCache redisCache;
    internal SystemCache systemCache;
    internal string equipmentID;

    internal const string prefix = "Auth";
    internal const string prefixSubscribe = "Auth_Subscribe";
    internal const double outTime = 120;//超时时间(分钟)

    /// <summary>
    /// 初始化,构造函数
    /// </summary>
    /// <param name="redis">47.103.142.91:6379,password=xTOdvTvclzMzvx8KCkbyJK1TcjYTBVbq</param>
    /// <param name="database">2</param>
    public Auth(string redis, int database)
    {
        equipmentID = Guid.NewGuid().ToString("N");
        systemCache = new SystemCache();
        redisCache = new RedisCache(redis, database);
        redisCache.ReceiveMq<SubscribeModel>(prefixSubscribe, RecvRemove);
    }

    #endregion

    #region 对外接口

    /// <summary>
    /// 取用户
    /// </summary>
    /// <param name="token">用户Token</param>
    /// <returns></returns>
    public T GetUserByToken<T>(string token) where T : UserBasicModel, new()
    {
        UserCenterUserResponse response = HttpHelper.ApiSendDto<UserCenterUserResponse>(HttpHelperUrl.GetUserCenterApiPostUrl("login/getUserInfo"), new { }, token);
        var result = new UserBasicModel()
        {
            Token = token,
            UserID = response.Id,
            UserName = response.Name,
            UserAccount = response.Account,
        };

        T user = result as T;
        if (user == null) return null;

        return user.Token == token ? user : null;
    }

    /// <summary>
    /// 根据域名解析应用信息
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public AppModel GetAppInfo(string domain)
    {
        //默认从缓存中获取平台信息，避免反复从接口获取，影响性能
        AppModel appInfo = CacheHelper.SystemCache.GetCache<AppModel>(domain);
        if (appInfo == null)
        {
            //从接口获取平台信息
            UserCenterAppInfoReponse info = HttpHelper.ApiSendDto<UserCenterAppInfoReponse>(HttpHelperUrl.GetUserCenterApiPostUrl("application-manage/findByDomainUrl"), new { domainUrl = domain });
            if (info == null) throw new ApiException("当前域名下未配置平台信息");
            appInfo = new AppModel
            {
                AppID = info.ApplicationId,
                AppName = info.ApplicationName,
            };
            CacheHelper.SystemCache.SetCache(domain, appInfo, new TimeSpan(2, 0, 0));//缓存设置2小时过期
        }
        else return appInfo;
        return appInfo;
    }

    /// <summary>
    /// 被踢时触发的事件
    /// </summary>
    public Action<object> OnRemove { get; set; }

    #endregion

    #region 私有方法

    /// <summary>
    /// 接收到其他服务器的通知删除本地key
    /// </summary>
    /// <param name="key"></param>
    internal void RecvRemove(SubscribeModel subscribe)
    {
        if (subscribe.EquipmentID == equipmentID) return;

        if (subscribe.ActivityType == ActivityType.LogOut)
        {
            UserBasicModel user = systemCache.GetCache<UserBasicModel>(subscribe.Message);
            if (user != null)
            {
                systemCache.RemoveCache(subscribe.Message);  //移除本地缓存
                OnRemove?.Invoke(user);
            }
        }
    }

    #endregion
}