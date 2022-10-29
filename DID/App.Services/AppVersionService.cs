

using App.Entity;
using App.Models.Request;
using DID.Common;
using DID.Models.Base;
using Microsoft.Extensions.Logging;

namespace App.Services
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IAppVersionService
    {
        /// <summary>
        /// 获取App版本信息
        /// </summary>
        /// <returns></returns>
        Task<Response<List<AppVersion>>> GetAppVersion();
        /// <summary>
        /// 获取App版本信息
        /// </summary>
        /// <returns></returns>
        Task<Response<AppVersion>> GetAppVersion(string id);
        /// <summary>
        /// 获取最新App版本信息
        /// </summary>
        /// <returns></returns>
        Task<Response<AppVersion>> GetAppVersion(int osType);
        /// <summary>
        /// 添加App版本信息
        /// </summary>
        /// <returns></returns>
        Task<Response> AddAppVersion(AddAppVersionReq req);
        /// <summary>
        /// 更新App版本信息
        /// </summary>
        /// <returns></returns>
        Task<Response> UpdateAppVersion(AppVersion req);
        /// <summary>
        /// 删除App版本信息
        /// </summary>
        /// <returns></returns>
        Task<Response> DeleteAppVersion(string id);
    }

    /// <summary>
    /// 服务
    /// </summary>
    public class AppVersionService : IAppVersionService
    {
        private readonly ILogger<AppVersionService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AppVersionService(ILogger<AppVersionService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取App版本信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<AppVersion>>> GetAppVersion()
        {
            using var db = new NDatabase();
            var list = await db.FetchAsync<AppVersion>("select * from App_Version where IsDelete = 0 order by CreateDate Desc,OsType");

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取App版本信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<AppVersion>> GetAppVersion(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<AppVersion>(id);

            return InvokeResult.Success(model);
        }


        /// <summary>
        /// 获取最新App版本信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<AppVersion>> GetAppVersion(int osType)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultAsync<AppVersion>("select * from App_Version where IsDelete = 0 and OsType = @0 order by CreateDate Desc", osType);

            return InvokeResult.Success(model);
        }

        /// <summary>
        /// 添加App版本信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> AddAppVersion(AddAppVersionReq req)
        {
            using var db = new NDatabase();
            var model = new AppVersion {
                VersionId = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                Link1 = req.Link1,
                Link2 = req.Link2,
                Link3 = req.Link3,
                VersionNo = req.VersionNo,
                OsType = req.OsType
            };
            await db.InsertAsync(model);

            return InvokeResult.Success("添加成功!");
        }

        /// <summary>
        /// 更新App版本信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> UpdateAppVersion(AppVersion req)
        {
            using var db = new NDatabase();
            await db.UpdateAsync(req);

            return InvokeResult.Success("更新成功!");
        }
        /// <summary>
        /// 删除App版本信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> DeleteAppVersion(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<AppVersion>(id);
            model.IsDelete = DID.Entitys.IsEnum.是;
            await db.UpdateAsync(model);

            return InvokeResult.Success("删除成功!");
        }

    }
}