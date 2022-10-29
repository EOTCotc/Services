

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
    public interface IVolunteerService
    {
        /// <summary>
        /// 获取自愿者信息
        /// </summary>
        /// <returns></returns>
        Task<Response<List<Volunteer>>> GetVolunteer();
        /// <summary>
        /// 获取自愿者信息
        /// </summary>
        /// <returns></returns>
        Task<Response<Volunteer>> GetVolunteer(string id);
        /// <summary>
        /// 添加自愿者信息
        /// </summary>
        /// <returns></returns>
        Task<Response> AddVolunteer(AddVolunteerReq req);
        /// <summary>
        /// 更新自愿者信息
        /// </summary>
        /// <returns></returns>
        Task<Response> UpdateVolunteer(Volunteer req);
        /// <summary>
        /// 删除自愿者信息
        /// </summary>
        /// <returns></returns>
        Task<Response> DeleteVolunteer(string id);
    }

    /// <summary>
    /// 禅论系统服务
    /// </summary>
    public class VolunteerService : IVolunteerService
    {
        private readonly ILogger<VolunteerService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public VolunteerService(ILogger<VolunteerService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取自愿者信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<Volunteer>>> GetVolunteer()
        {
            using var db = new NDatabase();
            var list = await db.FetchAsync<Volunteer>("select * from App_Volunteer where IsDelete = 0");

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取自愿者信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<Volunteer>> GetVolunteer(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<Volunteer>(id);

            return InvokeResult.Success(model);
        }

        /// <summary>
        /// 添加自愿者信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> AddVolunteer(AddVolunteerReq req)
        {
            using var db = new NDatabase();
            var model = new Volunteer {
                VolunteerId = Guid.NewGuid().ToString(),
                Wechat = req.Wechat,
                QRCode = req.QRCode
            };
            await db.InsertAsync(model);

            return InvokeResult.Success("添加成功!");
        }

        /// <summary>
        /// 更新自愿者信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> UpdateVolunteer(Volunteer req)
        {
            using var db = new NDatabase();
            await db.UpdateAsync(req);

            return InvokeResult.Success("更新成功!");
        }
        /// <summary>
        /// 删除自愿者信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> DeleteVolunteer(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<Volunteer>(id);
            model.IsDelete = DID.Entitys.IsEnum.是;
            await db.UpdateAsync(model);

            return InvokeResult.Success("删除成功!");
        }

    }
}