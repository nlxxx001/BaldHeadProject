using ApiServer.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [Route("Api/Health")]
    [ApiExplorerSettings(GroupName = "Health")]
    [CustomAuthorize(EnumCustomAuthorize.None)]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public WebApiPackage<string> Get()
        {
            return new WebApiPackage<string>() { result = "success", resultCode = 200, resultDesc = "", success = true };
        }
    }
}
