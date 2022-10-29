

using App.Entity;
using App.Models.Request;
using DID.Common;
using DID.Models.Base;
using Microsoft.Extensions.Logging;

namespace App.Services
{
    /// <summary>
    /// 系统服务接口
    /// </summary>
    public interface ICLSystemService
    {
        /// <summary>
        /// 获取禅论系统信息
        /// </summary>
        /// <returns></returns>
        Task<Response<List<CLSystem>>> GetCLSystem();
        /// <summary>
        /// 获取禅论系统信息
        /// </summary>
        /// <returns></returns>
        Task<Response<CLSystem>> GetCLSystem(string id);
        /// <summary>
        /// 添加禅论系统信息
        /// </summary>
        /// <returns></returns>
        Task<Response> AddCLSystem(AddCLSystemReq req);
        /// <summary>
        /// 更新禅论系统信息
        /// </summary>
        /// <returns></returns>
        Task<Response> UpdateCLSystem(CLSystem req);
        /// <summary>
        /// 删除禅论系统信息
        /// </summary>
        /// <returns></returns>
        Task<Response> DeleteCLSystem(string id);
    }

    /// <summary>
    /// 禅论系统服务
    /// </summary>
    public class CLSystemService : ICLSystemService
    {
        private readonly ILogger<CLSystemService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CLSystemService(ILogger<CLSystemService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取禅论系统信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<CLSystem>>> GetCLSystem()
        {
            using var db = new NDatabase();
            var list = await db.FetchAsync<CLSystem>("select * from App_CLSystem where IsDelete = 0 order by Type");

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取禅论系统信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<CLSystem>> GetCLSystem(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<CLSystem>(id);

            return InvokeResult.Success(model);
        }

        /// <summary>
        /// 添加禅论系统信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> AddCLSystem(AddCLSystemReq req)
        {
            using var db = new NDatabase();
            var model = new CLSystem {
                CLSystemId = Guid.NewGuid().ToString(),
                Name = req.Name,
                Price = req.Price,
                Type = req.Type
            };
            await db.InsertAsync(model);

            return InvokeResult.Success("添加成功!");
        }

        /// <summary>
        /// 更新禅论系统信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> UpdateCLSystem(CLSystem req)
        {
            using var db = new NDatabase();
            await db.UpdateAsync(req);

            return InvokeResult.Success("更新成功!");
        }
        /// <summary>
        /// 删除禅论系统信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> DeleteCLSystem(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<CLSystem>(id);
            model.IsDelete = DID.Entitys.IsEnum.是;
            await db.UpdateAsync(model);

            return InvokeResult.Success("删除成功!");
        }

    }
}