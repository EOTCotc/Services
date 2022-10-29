using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApeVolo.Api.Controllers.Base;
using ApeVolo.Common.AttributeExt;
using ApeVolo.Common.Caches.Redis.Service;
using ApeVolo.Common.Extention;
using ApeVolo.Common.Global;
using ApeVolo.Common.Helper.Excel;
using ApeVolo.Common.Model;
using ApeVolo.Common.Resources;
using ApeVolo.Common.WebApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ApeVolo.Api.Controllers;

/// <summary>
/// 在线用户
/// </summary>
[Area("OnlineUser")]
[Route("/api/online")]
public class OnlineController : BaseApiController
{
    private readonly IRedisCacheService _redisCacheService;

    public OnlineController(IRedisCacheService redisCacheService)
    {
        _redisCacheService = redisCacheService;
    }

    #region 对内接口

    /// <summary>
    /// 在线用户列表
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("{0}List")]
    public async Task<ActionResult<object>> QueryPageList(Pagination pagination)
    {
        List<OnlineUser> onlineUsers = new List<OnlineUser>();
        var arrayList = await _redisCacheService.ScriptEvaluateKeys(RedisKey.OnlineKey);
        if (arrayList.Length > 0)
        {
            foreach (var item in arrayList)
            {
                OnlineUser onlineUser = await _redisCacheService.GetCacheAsync<OnlineUser>(item);
                if (onlineUser != null)
                {
                    onlineUser.CurrentPermission = null;
                    onlineUsers.Add(onlineUser);
                }
            }
        }

        List<OnlineUser> newOnlineUsers = new List<OnlineUser>();
        if (onlineUsers.Count > 0)
        {
            newOnlineUsers = onlineUsers.Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
        }

        return new ActionResultVm<OnlineUser>
        {
            Content = newOnlineUsers,
            TotalElements = onlineUsers.Count
        }.ToJson();
    }

    /// <summary>
    /// 强制登出用户
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("out")]
    [Description("OutOnlineUser")]
    [ApeVoloAuthorize(new[] { "admin" })]
    public async Task<ActionResult<object>> DropOut([FromBody] HashSet<string> keys)
    {
        if (keys == null || keys.Count < 1)
        {
            return Error("keys is null");
        }

        foreach (var item in keys)
        {
            await _redisCacheService.RemoveAsync(RedisKey.OnlineKey + item);
        }

        return Success();
    }

    /// <summary>
    /// 导出在线用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Description("{0}Export")]
    [Route("download")]
    [ApeVoloAuthorize(new[] { "admin" })]
    public async Task<ActionResult<object>> Download()
    {
        List<OnlineUser> onlineUsers = new List<OnlineUser>();
        var arrayList = await _redisCacheService.ScriptEvaluateKeys(RedisKey.OnlineKey);
        if (arrayList.Length > 0)
        {
            foreach (var item in arrayList)
            {
                OnlineUser onlineUser = await _redisCacheService.GetCacheAsync<OnlineUser>(item);
                if (onlineUser != null)
                {
                    onlineUser.CurrentPermission = null;
                    onlineUsers.Add(onlineUser);
                }
            }
        }

        List<ExportRowModel> exportRowModels = new List<ExportRowModel>();
        List<ExportColumnModel> exportColumnModels;
        int point;
        onlineUsers.ForEach(onlineUser =>
        {
            point = 0;
            exportColumnModels = new List<ExportColumnModel>();
            exportColumnModels.Add(new ExportColumnModel
                { Key = "登录账号", Value = onlineUser.UserName, Point = point++ });
            exportColumnModels.Add(new ExportColumnModel
                { Key = "用户昵称", Value = onlineUser.NickName, Point = point++ });
            exportColumnModels.Add(new ExportColumnModel { Key = "所属部门", Value = onlineUser.Dept, Point = point++ });
            exportColumnModels.Add(new ExportColumnModel { Key = "登录IP", Value = onlineUser.Ip, Point = point++ });
            exportColumnModels.Add(
                new ExportColumnModel { Key = "IP地址", Value = onlineUser.Address, Point = point++ });
            exportColumnModels.Add(new ExportColumnModel
                { Key = "操作系统", Value = onlineUser.OperatingSystem, Point = point++ });
            exportColumnModels.Add(new ExportColumnModel
                { Key = "设备类型", Value = onlineUser.DeviceType, Point = point++ });
            exportColumnModels.Add(new ExportColumnModel
                { Key = "浏览器", Value = onlineUser.BrowserName, Point = point++ });
            exportColumnModels.Add(new ExportColumnModel
                { Key = "版本号", Value = onlineUser.Version, Point = point++ });
            exportColumnModels.Add(new ExportColumnModel
            {
                Key = "登录时间", Value = onlineUser.LoginTime.ToString(CultureInfo.InvariantCulture), Point = point++
            });
            exportColumnModels.Add(new ExportColumnModel { Key = "在线Key", Value = onlineUser.Key, Point = point++ });
            exportRowModels.Add(new ExportRowModel { exportColumnModels = exportColumnModels });
        });

        var filepath = ExcelHelper.ExportData(exportRowModels, Localized.Get("OnlineUser"));

        FileInfo fileInfo = new FileInfo(filepath);
        var ext = fileInfo.Extension;
        new FileExtensionContentTypeProvider().Mappings.TryGetValue(ext, out var contently);
        return File(await System.IO.File.ReadAllBytesAsync(filepath), contently ?? "application/octet-stream",
            fileInfo.Name);
    }

    #endregion
}