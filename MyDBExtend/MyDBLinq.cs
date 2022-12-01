using MyDbEntity;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using Util;
using Util.Model;

namespace MyDBExtend;

public static class MyDBLinq
{
    /// <summary>
    /// 根据指定条件检测数据库是否存在此数据,找不到的时候,直接抛API错误,基于FirstOrDefault
    /// </summary>
    /// <typeparam name="T">表类型</typeparam>
    /// <param name="source">源</param>
    /// <param name="predicate">条件</param>
    /// <param name="errMessage">报错信息</param>
    /// <returns></returns>
    public static T ExistCheck<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, string errMessage = "获取数据失败") where T : class => source.FirstOrDefault(predicate) ?? throw new ApiException(errMessage);

    /// <summary>
    /// 取表中非删除的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> Sets<T>(this MyDB myDB) where T : DbBase => myDB.Set<T>().Where(l => l.DeleteFlag == 0);

    /// <summary>
    /// 取表中删除的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> Setd<T>(this MyDB myDB) where T : DbBase => myDB.Set<T>().Where(l => l.DeleteFlag == 1);

    /// <summary>
    /// 取where中非删除的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ts"></param>
    /// <param name="pd"></param>
    /// <returns></returns>
    public static IQueryable<T> Wheres<T>(this IQueryable<T> ts, Expression<Func<T, bool>> pd = null) where T : DbBase
    {
        ts = ts.Where(l => l.DeleteFlag == 0);
        return pd != null ? ts.Where(pd) : ts;
    }

    /// <summary>
    /// 取where中删除的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ts"></param>
    /// <param name="pd"></param>
    /// <returns></returns>
    public static IQueryable<T> Whered<T>(this IQueryable<T> ts, Expression<Func<T, bool>> pd = null) where T : DbBase
    {
        ts = ts.Where(l => l.DeleteFlag == 1);
        return pd != null ? ts.Where(pd) : ts;
    }

    /// <summary>
    /// 尝试分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entitys"></param>
    /// <param name="pg"></param>
    /// <returns></returns>
    public static IQueryable<T> Pager<T>(this IQueryable<T> entitys, Pagination pg) => pg != null ? entitys.GetPagination(pg) : entitys;

    /// <summary>
    /// 取select中非删除的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="ts"></param>
    /// <param name="pd"></param>
    /// <returns></returns>
    public static IQueryable<R> Selects<T, R>(this IQueryable<T> ts, Expression<Func<T, R>> pd) where T : DbBase
    {
        return ts.Where(l => l.DeleteFlag == 0).Select(pd);
    }

    /// <summary>
    /// 取select中删除的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="ts"></param>
    /// <param name="pd"></param>
    /// <returns></returns>
    public static IQueryable<R> Selectd<T, R>(this IQueryable<T> ts, Expression<Func<T, R>> pd) where T : DbBase
    {
        return ts.Where(l => l.DeleteFlag == 1).Select(pd);
    }

    /// <summary>
    /// IQueryable方式 应用筛选先决条件判断数据是否存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="condition"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate) => condition ? query.Where(predicate) : query;

    /// <summary>
    /// IQueryable方式 应用筛选先决条件判断数据是否存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="condition"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate) => condition ? query.Where(predicate) : query;

    /// <summary>
    /// IEnumerable方式 应用筛选先决条件判断数据是否存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="condition"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Func<T, bool> predicate) => condition ? query.Where(predicate) : query;

    /// <summary>
    /// 取最大值,如果为空则为默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <param name="nullValue"></param>
    /// <returns></returns>
    public static R MaxOrDefault<T, R>(this IQueryable<T> source, Expression<Func<T, R?>> selector, R nullValue = default) where R : struct => source.Max(selector) ?? nullValue;
}