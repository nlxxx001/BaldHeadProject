using ApiServer.Comm;
using ApiServer.Filters;
using Cache;
using DTO.API.Cache;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers.Common
{
    [Route("Api/Common/Basic")]
    [ApiExplorerSettings(GroupName = "Common")]
    [CustomAuthorize(EnumCustomAuthorize.None)]
    public class CommonController : BaseController
    {
        /// <summary>
        /// 获取Oss上传路径枚举
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetOssPathes")]
        public WebApiPackage<OssPathes> GetOssPathes() => Data<OssPathes>(Res.OssPathes);
    }
}
