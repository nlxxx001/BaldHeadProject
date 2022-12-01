using DTO.API.Cache;

namespace Cache;

public static class Res
{
    /// <summary>
    /// 是否调试环境
    /// </summary>
    public static bool IsTest { get; set; } = false;

    /// <summary>
    /// Oss路径枚举
    /// </summary>
    public static OssPathes OssPathes { get; set; } = new OssPathes();

}