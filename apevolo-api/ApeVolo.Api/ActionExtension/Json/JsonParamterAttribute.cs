﻿using System.Threading.Tasks;
using ApeVolo.Common.Extention;
using ApeVolo.Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApeVolo.Api.ActionExtension.Json;

/// <summary>
/// Json参数支持
/// </summary>
public class JsonParamterAttribute : BaseActionFilter
{
    public override async Task OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (filterContext.ContainsFilter<NoJsonParamterAttribute>())
            return;
        HttpContext = filterContext.HttpContext;
        //参数映射：支持application/json
        string contentType = filterContext.HttpContext.Request.ContentType;
        if (!contentType.IsNullOrEmpty() && contentType.Contains("application/json"))
        {
            var actionParameters = filterContext.ActionDescriptor.Parameters;
            var allParamters = HttpHelper.GetAllRequestParams(filterContext.HttpContext);
            var actionArguments = filterContext.ActionArguments;
            actionParameters.ForEach(aParamter =>
            {
                string key = aParamter.Name;
                if (allParamters.ContainsKey(key))
                {
                    actionArguments[key] =
                        allParamters[key]?.ToString()?.ChangeType_ByConvert(aParamter.ParameterType);
                }
                else
                {
                    try
                    {
                        actionArguments[key] = allParamters.ToJson().ToObject(aParamter.ParameterType);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            });
        }

        await Task.CompletedTask;
    }
    // /// <summary>
    // /// Action执行之前执行
    // /// </summary>
    // /// <param name="context">过滤器上下文</param>
    // public void OnActionExecuting(ActionExecutingContext context)
    // {
    //     if (context.ContainsFilter<NoJsonParamterAttribute>())
    //         return;
    //
    //     //参数映射：支持application/json
    //     string contentType = context.HttpContext.Request.ContentType;
    //     if (!contentType.IsNullOrEmpty() && contentType.Contains("application/json"))
    //     {
    //         var actionParameters = context.ActionDescriptor.Parameters;
    //         var allParamters = HttpHelper.GetAllRequestParams(context.HttpContext);
    //         var actionArguments = context.ActionArguments;
    //         actionParameters.ForEach(aParamter =>
    //         {
    //             string key = aParamter.Name;
    //             if (allParamters.ContainsKey(key))
    //             {
    //                 actionArguments[key] = allParamters[key]?.ToString()?.ChangeType_ByConvert(aParamter.ParameterType);
    //             }
    //             else
    //             {
    //                 try
    //                 {
    //                     actionArguments[key] = allParamters.ToJson().ToObject(aParamter.ParameterType);
    //                 }
    //                 catch
    //                 {
    //                     // ignored
    //                 }
    //             }
    //         });
    //     }
    // }
    //
    // /// <summary>
    // /// Action执行完毕之后执行
    // /// </summary>
    // /// <param name="context"></param>
    // public void OnActionExecuted(ActionExecutedContext context)
    // {
    //
    // }
}