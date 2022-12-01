using ApiServer.Comm;
using BasicLibrary.Comm;
using DTO.Comm;
using LeYiXue.Common.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Secure
{
    /// <summary>
    /// 系统间对接查询此接口
    /// </summary>
    [Route("Api/Secure/OpenUpApi")]
    [ApiExplorerSettings(GroupName = "Secure")]
    public class OpenUpApiController : BaseController
    {
        /// <summary>
        /// 获取学期列表
        /// </summary>
        /// <param name="safetyDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetClassSubjectTeacher")]
        public WebApiPackage<List<TermDataOutDto>> GetTermList([FromBody] BaseRequestSafety safetyDto)
        {
            return Data(BaseControllerService.InitResponseSafety(new CommonDemoSerivce().GetTermList, safetyDto, "GetTermList", "获取学期列表"));
        }
    }
}
