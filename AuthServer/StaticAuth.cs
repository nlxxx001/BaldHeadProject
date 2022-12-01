namespace AuthServer;

public static class StaticAuth
{
    /// <summary>
    /// 自带的静态变量
    /// </summary>
    public static Auth Auth { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="redis"></param>
    /// <param name="database"></param>
    public static void Init(string redis, int database)
    {
        Auth = new Auth(redis, database);
    }
}