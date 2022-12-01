using ApiServer.Comm;
using Cache;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace ApiServer.Filters;

/// <summary>
/// 参数验证过滤器
/// </summary>
public class ParameterValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //参数验证
        if (context.ModelState.IsValid == false)
        {
            context.Result = HttpContextExtention.ApiError(ValidErr(context.ModelState), 106);

            string address = context.HttpContext.GetRemoteHost();
            string pathlow = context.HttpContext.Request.Path.Value.ToLower();
            string guid = context.HttpContext.Request.Headers["tempGuid"];

            ApiStatistics.Parameter(pathlow);
            LogHelper.WriteLog($" \t IP:{address} \t PATH:{pathlow} \t Guid:{guid}", "logs/ParameterLog", DateTime.Now.ToDefaultDateString() + ".log");
            return;
        }
    }

    /// <summary>
    /// 返回验证报错
    /// </summary>
    /// <param name="modelstate"></param>
    /// <returns></returns>
    public static string ValidErr(ModelStateDictionary modelstate)
    {
        StringBuilder sb = new();
        foreach (string propName in modelstate.Keys)//遍历每个属性
        {
            //如果这个属性的错误消息<=0 那么表示数据没有错误
            if (modelstate[propName].Errors.Count <= 0)
            {
                continue;
            }
            //一个属性可能有多个错误，那么就对这个属性的错误进行遍历
            foreach (ModelError error in modelstate[propName].Errors)
            {
                sb.Append(propName + "属性验证错误：" + error.ErrorMessage);//将错误信息添加到sb中
                sb.AppendLine();
            }
        }
        return sb.ToString();
    }
}