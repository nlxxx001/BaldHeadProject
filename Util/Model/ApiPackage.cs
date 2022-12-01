

namespace Util;

/// <summary>
/// Web返回基础包
/// </summary>
public class WebApiPackage
{
    /// <summary>
    /// 是否成功  true 或false
    /// </summary>
    public bool success { get; set; }
    /// <summary>
    /// 返回代码 200 成功 101 错误 102 验证错误
    /// </summary>
    public int resultCode { get; set; }
    /// <summary>
    /// 返回结果描述
    /// </summary>
    public string resultDesc { get; set; }
    /// <summary>
    /// 返回结果
    /// </summary>
    public object result { get; set; }
}

/// <summary>
/// Web返回基础包
/// </summary>
/// <typeparam name="T"></typeparam>
public class WebApiPackage<T>
{
    /// <summary>
    /// 是否成功  true 或false
    /// </summary>
    public bool success { get; set; }
    /// <summary>
    /// 返回代码 200 成功 101 错误 102 验证错误
    /// </summary>
    public int resultCode { get; set; }
    /// <summary>
    /// 返回结果描述
    /// </summary>
    public string resultDesc { get; set; }
    /// <summary>
    /// 返回结果
    /// </summary>
    public T result { get; set; }
}

/// <summary>
/// Web返回列表包
/// </summary>
/// <typeparam name="T"></typeparam>
public class WebApiPackageList<T>
{
    public bool success { get; set; }

    public int resultCode { get; set; }

    public string resultDesc { get; set; }

    public WebApiPackageList1<T> result { get; set; }
}

/// <summary>
/// 列表结构
/// </summary>
/// <typeparam name="T"></typeparam>
public class WebApiPackageList1<T>
{
    public IEnumerable<T> list { get; set; }

    public Pagination pager { get; set; }

    public object data { get; set; } = null;
}

