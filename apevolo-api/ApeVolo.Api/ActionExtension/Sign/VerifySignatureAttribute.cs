using System;
using System.Threading.Tasks;
using ApeVolo.Common.Caches.Redis.Service;
using ApeVolo.Common.DI;
using ApeVolo.Common.Extention;
using ApeVolo.IBusiness.Interface.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ApeVolo.Api.ActionExtension.Sign;

/// <summary>
/// 接口对外签名验证
/// </summary>
public class VerifySignatureAttribute : BaseActionFilter
{
    /// <summary>
    /// Action执行之前执行
    /// </summary>
    /// <param name="filterContext"></param>
    public override async Task OnActionExecuting(ActionExecutingContext filterContext)
    {
        //判断是否需要签名
        if (filterContext.ContainsFilter<IgnoreVerifySignatureAttribute>())
            return;
        HttpContext = filterContext.HttpContext;
        var request = filterContext.HttpContext.Request;

        var appId = request.Headers["appId"].ToString();
        if (appId.IsNullOrEmpty())
        {
            filterContext.Result = Error("缺少header:appId");
            return;
        }

        var time = request.Headers["time"].ToString();
        if (time.IsNullOrEmpty())
        {
            filterContext.Result = Error("缺少header:time");
            return;
        }

        if (time.ToDateTime() < DateTime.Now.AddMinutes(-5) || time.ToDateTime() > DateTime.Now.AddMinutes(5))
        {
            filterContext.Result = Error("time过期");
            return;
        }

        var guid = request.Headers["guid"].ToString();
        if (guid.IsNullOrEmpty())
        {
            filterContext.Result = Error("缺少header:guid");
            return;
        }

        var guidKey = $"ApiGuid_{guid}";
        var redisCacheService = filterContext.HttpContext.RequestServices.GetRequiredService<IRedisCacheService>();
        if (!redisCacheService.GetCacheStrAsync(guidKey).IsNullOrEmpty())
        {
            await redisCacheService.SetCacheAsync(guidKey, "1");
        }
        else
        {
            filterContext.Result = Error("禁止重复调用!");
            return;
        }

        request.EnableBuffering();
        var body = request.Body.ReadToString();

        var sign = request.Headers["sign"].ToString();
        if (sign.IsNullOrEmpty())
        {
            filterContext.Result = Error("缺少header:sign");
            return;
        }

        var appSecretModel = await filterContext.HttpContext.RequestServices.GetRequiredService<IAppSecretService>()
            .QueryFirstAsync(x => x.AppId == appId);
        if (appSecretModel.IsNull())
        {
            filterContext.Result = Error("header:appId无效");
            return;
        }

        var newSign = BuildApiSign(appId, appSecretModel.AppSecretKey, guid, time.ToDateTime(), body);
        if (sign != newSign)
        {
            var msg =
                $@"sign签名错误!
                        headers:{request.Headers.ToJson()}
                        body:{body}
                        传入sign{sign}
                        正确sign:{newSign}";
            filterContext.Result = Error(msg);
        }
    }

    /// <summary>
    /// 生成接口签名sign
    /// 注：md5(appId+time+guid+body+appSecret)
    /// </summary>
    /// <param name="appId">应用Id</param>
    /// <param name="appSecret">应用密钥</param>
    /// <param name="guid">唯一GUID</param>
    /// <param name="time">时间</param>
    /// <param name="body">请求体</param>
    /// <returns></returns>
    private string BuildApiSign(string appId, string appSecret, string guid, DateTime time, string body)
    {
        return $"{appId}{time:yyyy-MM-dd HH:mm:ss}{guid}{body}{appSecret}".ToMd5String();
    }
}