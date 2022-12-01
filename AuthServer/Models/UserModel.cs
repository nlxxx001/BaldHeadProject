namespace AuthServer.Models;

/// <summary>
/// 基础请求数据
/// </summary>
public class CommonRequestData
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public UserBasicModel UserInfo { get; set; }
    /// <summary>
    /// 应用信息
    /// </summary>
    public AppModel AppInfo { get; set; }
    /// <summary>
    /// 平台信息
    /// </summary>
    public PlatformModel PlatformInfo { get; set; }
    /// <summary>
    /// 组织信息
    /// </summary>
    public OrgModel OrgInfo { get; set; }
}

/// <summary>
/// 用户基础信息
/// </summary>
public class UserBasicModel
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserID { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string UserAccount { get; set; }

    /// <summary>
    /// 秘钥,Key
    /// </summary>
    public string Token { get; set; }
}

/// <summary>
/// 应用信息
/// </summary>
public class AppModel
{
    /// <summary>
    /// 应用id
    /// </summary>
    public string AppID { get; set; }
    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }
}

/// <summary>
/// 平台信息
/// </summary>
public class PlatformModel
{
    /// <summary>
    /// 平台id
    /// </summary>
    public string PlatId { get; set; }
    /// <summary>
    /// 平台名称
    /// </summary>
    public string PlatName { get; set; }
    /// <summary>
    /// 平台类型 0-乐易学 1-岳阳 2-龙岩 3-霍尔果斯
    /// </summary>
    public int PlatType { get; set; }
}

/// <summary>
/// 组织信息
/// </summary>
public class OrgModel
{
    /// <summary>
    /// 组织id
    /// </summary>
    public string OrgId { get; set; }
}