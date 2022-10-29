﻿using System;
using System.Text;
using ApeVolo.Api.ActionExtension.Json;
using ApeVolo.Common.Extention;
using ApeVolo.Common.Model;
using ApeVolo.Common.Resources;
using ApeVolo.Common.WebApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApeVolo.Api.Controllers.Base;

/// <summary>
/// 基控制器
/// </summary>
[JsonParamter]
public class BaseController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vm"></param>
    /// <returns></returns>
    private ContentResult JsonContent(ActionResultVm vm)
    {
        return new ContentResult
        {
            Content = new ActionResultVm
            {
                Status = vm.Status,
                Error = vm.Error,
                Message = vm.Message,
                Timestamp = DateTime.Now.ToUnixTimeStampMillisecond().ToString(),
                Path = Request.Path.Value?.ToLower() //HttpContext.Request.Path.Value?.ToLower()
            }.ToJson(),
            ContentType = "application/json; charset=utf-8",
            StatusCode = vm.Status
        };
    }


    /// <summary>
    /// 返回成功
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    protected ContentResult Success(string msg = "")
    {
        msg = msg.IsNullOrEmpty() ? Localized.Get("HttpOK") : msg;
        var vm = new ActionResultVm
        {
            Status = StatusCodes.Status200OK,
            Message = msg
        };

        return JsonContent(vm);
    }

    /// <summary>
    /// 创建成功
    /// </summary>
    /// <returns></returns>
    protected ContentResult Create(string msg = "")
    {
        msg = msg.IsNullOrEmpty() ? Localized.Get("HttpCreated") : msg;
        var vm = new ActionResultVm
        {
            Status = StatusCodes.Status201Created,
            Message = msg
        };

        return JsonContent(vm);
    }

    /// <summary>
    /// 更新成功 无需刷新
    /// </summary>
    /// <returns></returns>
    protected ContentResult NoContent(string msg = "")
    {
        msg = msg.IsNullOrEmpty() ? Localized.Get("HttpNoContent") : msg;
        var vm = new ActionResultVm
        {
            Status = StatusCodes.Status204NoContent,
            Message = msg
        };

        return JsonContent(vm);
    }

    /// <summary>
    /// 返回错误
    /// </summary>
    /// <param name="msg">错误提示</param>
    /// <returns></returns>
    protected ContentResult Error(string msg = "")
    {
        msg = msg.IsNullOrEmpty() ? Localized.Get("HttpBadRequest") : msg;
        var vm = new ActionResultVm
        {
            Status = StatusCodes.Status400BadRequest,
            Error = "BadRequest",
            Message = msg
        };

        return JsonContent(vm);
    }
}