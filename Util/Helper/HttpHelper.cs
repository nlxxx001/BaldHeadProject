using LeYiXue.Common.Util;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Helper
{
    public class HttpHelperUrl
    {
        public static void Init(IConfiguration config) 
        {
            YunPingTaiPostUrl = config["ApiSetting:CloudPlatApi"] ?? "";
            UserCenterApiPostUrl = config["ApiSetting:UserCenterApi"] ?? "";
        }
        /// <summary>
        /// 云平台
        /// </summary>
        private static string YunPingTaiPostUrl;

        /// <summary>
        /// 获取云平台接口地址
        /// </summary>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public static string GetYunPingTaiPostUrl(string apiName)
        {
            return YunPingTaiPostUrl.TrimEnd('/') + "/" + apiName.Trim('/');
        }

        /// <summary>
        /// 用户中心接口地址
        /// </summary>
        private static string UserCenterApiPostUrl;

        /// <summary>
        /// 用户中心接口地址
        /// </summary>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public static string GetUserCenterApiPostUrl(string apiName)
        {
            return UserCenterApiPostUrl.TrimEnd('/') + "/" + apiName.Trim('/');
        }
    }

    public class HttpHelper
    {
        public static WebAPIHttps Api = new WebAPIHttps();

        /// <summary>
        /// 向服务器发送数据并取得数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="content">内容</param>
        /// <param name="token">token</param>
        /// <param name="additional">额外信息</param>
        /// <returns></returns>
        public static string ApiSend(string url, object content = null, string token = null, object additional = null, int type = 1)
        {
            string text = content.ToJson();
            string[] s = Api.PostResponse(url, text, token);
            if (s[1] == "OK")
            {
                WebApiPackage rp = s[0].ToObject<WebApiPackage>();
                if (rp.success && rp.resultCode == 200)
                {
                    return rp.result == null ? "" : rp.result.ToString();
                }
                else
                {
                    if (rp.resultCode == 107 || rp.resultCode == 103)
                    {
                        rp.resultDesc = "登录已经失效,请重新登录";
                    }
                    throw new ApiException(rp.resultDesc, rp.resultCode);
                }
            }
            else
            {
                throw new Exception("服务器连接失败");
            }
        }

        /// <summary>
        /// 向服务器发送数据并取得数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="content">内容</param>
        /// <param name="token">token</param>
        /// <param name="additional">额外信息</param>
        ///  <param name="type">1用户中心接口 2用户中心基础服务接口</param>
        /// <returns></returns>
        public static T ApiSendDto<T>(string url, object content = null, string token = null, object additional = null, int type = 1)
        {
            string text = content.ToJson();
            string[] s = Api.PostResponse(url, text, token);
            if (s[1] == "OK")
            {
                WebApiPackage rp = s[0].ToObject<WebApiPackage>();
                if (rp.success && rp.resultCode == 200)
                {
                    return rp.result == null ? default : rp.result.ToString().ToObject<T>();
                }
                else
                {
                    if (rp.resultCode == 107 || rp.resultCode == 103)
                    {
                        rp.resultDesc = "登录已经失效,请重新登录";
                    }
                    throw new ApiException(rp.resultDesc, rp.resultCode);
                }
            }
            else
            {
                throw new Exception("服务器连接失败");
            }
        }

        /// <summary>
        /// 向服务器发送数据并取得数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="content">内容</param>
        /// <param name="token">token</param>
        /// <param name="headers">自定义header参数</param>
        /// <returns></returns>
        public static WebApiPackage<T> ApiSendExtendDto<T>(string url, object content = null, string token = null, Dictionary<string, string> headers = null)
        {
            string text = content.ToJson();
            string[] s = Api.PostResponse(url, text, token, null, headers);
            if (s[1] == "OK")
            {
                WebApiPackage rp = s[0].ToObject<WebApiPackage>();
                if (rp.success && rp.resultCode == 200)
                {
                    return rp.result == null ? default : s[0].ToObject<WebApiPackage<T>>();
                }
                else
                {
                    return new WebApiPackage<T>() { resultCode = rp.resultCode, resultDesc = rp.resultDesc, success = rp.success };
                }
            }
            else
            {
                throw new Exception("服务器连接失败");
            }
        }

        /// <summary>
        /// 安全请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="content">内容</param>
        /// <param name="token">token</param>
        /// <param name="headers">自定义header参数</param>
        /// <returns></returns>
        public static T ApiSendSafetyDto<T, I>(string url, I content, string token = null, Dictionary<string, string> headers = null) where I : BaseRequestSafety
        {
            string text = content.ToJson();
            string[] s = Api.PostSafety(url, content, token, null, headers);
            if (s[1] == "OK")
            {
                WebApiPackage rp = s[0].ToObject<WebApiPackage>();
                if (rp.success && rp.resultCode == 200)
                {
                    return rp.result == null ? default : rp.result.ToString().ToObject<T>();
                }
                else
                {
                    throw new ApiException(rp.resultDesc, rp.resultCode);
                }
            }
            else
            {
                throw new Exception("服务器连接失败");
            }
        }
    }
}
