using ApiServer.Comm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Secure
{
    /// <summary>
    /// 
    /// </summary>
    [Route("Api/Secure/Health")]
    [ApiExplorerSettings(GroupName = "Secure")]
    public class HealthController : BaseController
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpPost("checkrun")]
        public string CheckRun()
        {
            return "OK";
        }
    }
}
