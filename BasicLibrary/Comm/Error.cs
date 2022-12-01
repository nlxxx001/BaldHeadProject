using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLibrary.Comm
{
    public static class Error
    {
        public static void Api(string msg = null, int level = 1)
        {
            throw new ApiException(msg, level);
        }

        /// <summary>
        /// 校验为true则抛出指定异常
        /// </summary>
        /// <param name="checkParameter"></param>
        /// <param name="errMsg"></param>
        /// <param name="level"></param>
        public static void Check(bool checkParameter, string errMsg)
        {
            if (checkParameter)
            {
                throw new Exception(errMsg);
            }
        }
    }
}
