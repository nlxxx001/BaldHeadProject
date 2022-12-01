using DTO.Comm;
using LeYiXue.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Helper;

namespace BasicLibrary.Comm
{
    public class CommonDemoSerivce
    {
        /// <summary>
        /// 获取全部学期
        /// </summary>
        /// <returns></returns>
        public List<TermDataOutDto> GetTermList(BaseRequestSafety safetyDto)
        {
            var result =  HttpHelper.ApiSendSafetyDto<List<TermDataOutDto>, BaseRequestSafety>(HttpHelperUrl.GetYunPingTaiPostUrl("Api/Secure/OpenUpApi/GetTermList"), safetyDto);
            return result;
        }
    }
}
