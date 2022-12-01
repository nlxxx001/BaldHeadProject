using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Enums
{
    /// <summary>
    /// 返回状态枚举
    /// </summary>
    public enum ResultCodeEnum
    {
        /// <summary>
        /// 未传Token
        /// </summary>
        [Description("未传Token")]
        NoToken = 101,

        /// <summary>
        /// 无效Token
        /// </summary>
        [Description("无效Token")]
        InvalidToken = 102,

        /// <summary>
        /// Token过期
        /// </summary>
        [Description("Token过期")]
        TokenExpire = 103,

        /// <summary>
        /// 请求正常
        /// </summary>
        [Description("请求正常")]
        Success = 200,

        /// <summary>
        /// 必选参数为空
        /// </summary>
        [Description("必选参数为空")]
        ParameterEmpty = 201,

        /// <summary>
        /// 参数有误
        /// </summary>
        [Description("参数有误")]
        ParameterError = 202,

        /// <summary>
        /// 认证失败
        /// </summary>
        [Description("认证失败")]
        AuthFail = 301,

        /// <summary>
        /// 系统异常
        /// </summary>
        [Description("系统异常")]
        SystemException = 401,
    }
}
