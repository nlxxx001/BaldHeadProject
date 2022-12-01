namespace AuthServer.Models;

/// <summary>
/// 过期类型
/// </summary>
public enum ExpireType
{
    /// <summary>
    /// 绝对过期
    /// 注：即自创建一段时间后就过期
    /// </summary>
    Absolute = 0,

    /// <summary>
    /// 相对过期
    /// 注：即该键未被访问后一段时间后过期，若此键一直被访问则过期时间自动延长
    /// </summary>
    Relative = 1,
}

/// <summary>
/// 平台类型
/// </summary>
public enum PlatformType
{
    /// <summary>
    /// 移动端APP
    /// </summary>
    App = 1,

    /// <summary>
    /// PC端
    /// </summary>
    PC = 2,

    /// <summary>
    /// Web端
    /// </summary>
    Web = 3,
}

/// <summary>
/// 用户类型
/// </summary>
public enum UserType
{
    /// <summary>
    /// 员工
    /// </summary>
    Staff = 1,

    /// <summary>
    /// 老师
    /// </summary>
    Teacher = 2,

    /// <summary>
    /// 学生
    /// </summary>
    Student = 3,
    /// <summary>
    /// 家长
    /// </summary>
    Patriarch = 4,

    /// <summary>
    /// 图书馆管理员
    /// </summary>
    LibraryAdmin = 5
}

/// <summary>
/// 动作类型
/// </summary>
public enum ActivityType
{


    /// <summary>
    /// 登出
    /// </summary>
    LogOut = 0,
}

/// <summary>
/// 验证类型
/// </summary>
public enum ValidationResult
{
    /// <summary>
    /// 成功
    /// </summary>
    Success = 0,

    /// <summary>
    /// Token过期
    /// </summary>
    TokenExpired = 1,

    /// <summary>
    /// Token无效
    /// </summary>
    TokenInvalid = 2,
}