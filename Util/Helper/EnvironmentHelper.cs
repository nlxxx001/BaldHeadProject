using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Helper
{
    /// <summary>
    /// 获取环境变量
    /// </summary>
    public class EnvironmentHelper
    {
        /// <summary>
        ///  获取环境变量值
        /// </summary>
        /// <param name="envName"></param>
        /// <returns></returns>
        public static string GetEnvironmentValue(string envName)
        {
            var environmentVariables = System.Environment.GetEnvironmentVariables();
            return (string)environmentVariables[envName];
        }
        /// <summary>
        /// 获取环境变量
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetEnvironment(IConfiguration configuration)
        {
            var environment = (string)GetEnvironmentValue("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(environment))
            {
                environment = configuration?["Environment:Type"] ?? "Development";
            }
            return environment;
        }
    }
}
