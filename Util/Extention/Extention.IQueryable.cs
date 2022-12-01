namespace Util;

/// <summary>
/// IQueryable<T>的拓展操作
/// 作者：Coldairarrow
/// </summary>
public static partial class Extention
{
    /// <summary>
    /// 获取分页后的数据
    /// </summary>
    /// <typeparam name="T">实体参数</typeparam>
    /// <param name="source">IQueryable数据源</param>
    /// <param name="pageIndex">当前页</param>
    /// <param name="pageRows">每页行数</param>
    /// <returns></returns>
    public static IQueryable<T> GetPagination<T>(this IQueryable<T> source, int pageIndex, int pageRows)
    {
        Pagination pagination = new Pagination
        {
            PageNumber = pageIndex,
            PageSize = pageRows,
        };

        return source.GetPagination(pagination);
    }

    /// <summary>
    /// 获取分页后的数据
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="source">数据源IQueryable</param>
    /// <param name="pagination">分页参数</param>
    /// <returns></returns>
    public static IQueryable<T> GetPagination<T>(this IQueryable<T> source, Pagination pagination)
    {
        pagination.Total = source.Count();
        return source.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
    }
}