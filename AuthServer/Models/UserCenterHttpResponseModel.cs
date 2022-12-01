using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class UserCenterUserResponse
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Phone { get; set; }
    }

    public class UserCenterAppInfoReponse
    {
        /// <summary>
        /// 应用主键ID
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// 平台信息
        /// </summary>
        public List<UserCenterPlatformInfoReponse> PlatformManages { get; set; }
    }

    public class UserCenterPlatformInfoReponse
    {
        /// <summary>
        /// 平台id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 平台类型 0-乐易学 1-岳阳 2-龙岩 3-霍尔果斯
        /// </summary>
        public int PlatformType { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
