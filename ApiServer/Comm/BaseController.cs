using AuthServer;
using AuthServer.Models;
using DTO.Comm;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Comm;

/// <summary>
/// 返回数据打包器
/// </summary>
[ApiController]
public class BaseController : Controller
{
    /// <summary>
    /// 获取用户基础信息
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public UserBasicModel GetUser(bool need = false)
    {
        string token = HttpContext.Request.Headers["Token"];

        if (string.IsNullOrWhiteSpace(token))
        {
            if (need) throw new ApiException("无法获取用户信息", 103);
            else return null;
        }
        UserBasicModel user = StaticAuth.Auth.GetUserByToken<UserBasicModel>(token);
        return user;
    }

    /// <summary>
    /// 获取组织信息
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public OrgModel GetOrgInfo(bool need = false)
    {
        string orgId = HttpContext.Request.Headers["OrgId"];

        if (string.IsNullOrWhiteSpace(orgId))
        {
            if (need) throw new ApiException("无法获取组织信息", 103);
            else return null;
        }
        return new OrgModel { OrgId = orgId };
    }

    /// <summary>
    /// 获取应用信息
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public AppModel GetAppInfo(bool need = false)
    {
        string appId = HttpContext.Request.Headers["AppId"];

        if (string.IsNullOrWhiteSpace(appId))
        {
            if (need) throw new ApiException("无法获取应用信息");
            else return null;
        }
        return new AppModel { AppID = appId };
    }

    /// <summary>
    /// 获取平台信息
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public PlatformModel GetPlatformInfo(bool need = false)
    {
        string platId = HttpContext.Request.Headers["PlatId"];

        if (string.IsNullOrWhiteSpace(platId))
        {
            if (need) throw new ApiException("无法获取平台信息");
            else return null;
        }
        return new PlatformModel { PlatId = platId };
    }

    /// <summary>
    /// 获取通用全局请求参数
    /// </summary>
    /// <param name="needUser"></param>
    /// <param name="needApp"></param>
    /// <param name="needPlatform"></param>
    /// <param name="needOrg"></param>
    /// <returns></returns>
    [NonAction]
    public CommonRequestData GetCommonRequestData(bool needUser = false, bool needApp = false, bool needPlatform = false, bool needOrg = false)
    {
        return new CommonRequestData
        {
            UserInfo = GetUser(needUser),
            AppInfo = GetAppInfo(needApp),
            PlatformInfo = GetPlatformInfo(needPlatform),
            OrgInfo = GetOrgInfo(needOrg),
        };
    }

    /// <summary>
    /// 返回空或对象
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public WebApiPackage OK(object obj = null)
    {
        return new WebApiPackage
        {
            success = true,
            resultCode = 200,
            resultDesc = "调用成功",
            result = obj,
        };
    }

    /// <summary>
    /// 返回数据
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public WebApiPackage<T> Data<T>(T result)
    {
        return new WebApiPackage<T>
        {
            success = true,
            resultCode = 200,
            resultDesc = "调用成功",
            result = result,
        };
    }

    /// <summary>
    /// 返回列表
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public WebApiPackageList<T> List<T>(IEnumerable<T> list, Pagination pagination)
    {
        return new WebApiPackageList<T>
        {
            success = true,
            resultCode = 200,
            resultDesc = "调用成功",
            result = new WebApiPackageList1<T>
            {
                list = list,
                pager = pagination,
            }
        };
    }
    /// <summary>
    /// 返回列表和其他数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="pagination"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public WebApiPackageList<T> List<T>(IEnumerable<T> list, Pagination pagination, object data)
    {
        return new WebApiPackageList<T>
        {
            success = true,
            resultCode = 200,
            resultDesc = "调用成功",
            result = new WebApiPackageList1<T>
            {
                list = list,
                pager = pagination,
                data = data
            }
        };
    }
}