

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
    public interface ITeacherService
    {
        /// <summary>
        /// 获取老师信息
        /// </summary>
        /// <returns></returns>
        Task<Response<List<Teacher>>> GetTeacher();
        /// <summary>
        /// 获取老师信息
        /// </summary>
        /// <returns></returns>
        Task<Response<Teacher>> GetTeacher(string id);
        /// <summary>
        /// 添加老师信息
        /// </summary>
        /// <returns></returns>
        Task<Response> AddTeacher(AddTeacherReq req);
        /// <summary>
        /// 更新老师信息
        /// </summary>
        /// <returns></returns>
        Task<Response> UpdateTeacher(Teacher req);
        /// <summary>
        /// 删除老师信息
        /// </summary>
        /// <returns></returns>
        Task<Response> DeleteTeacher(string id);
    }

    /// <summary>
    /// 禅论系统服务
    /// </summary>
    public class TeacherService : ITeacherService
    {
        private readonly ILogger<TeacherService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public TeacherService(ILogger<TeacherService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取老师信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<Teacher>>> GetTeacher()
        {
            using var db = new NDatabase();
            var list = await db.FetchAsync<Teacher>("select * from App_Teacher where IsDelete = 0");

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取老师信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<Teacher>> GetTeacher(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<Teacher>(id);

            return InvokeResult.Success(model);
        }

        /// <summary>
        /// 添加老师信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> AddTeacher(AddTeacherReq req)
        {
            using var db = new NDatabase();
            var model = new Teacher {
                TeacherId = Guid.NewGuid().ToString(),
                Name = req.Name,
                Blurb = req.Blurb,
                HeadImage = req.HeadImage
            };
            await db.InsertAsync(model);

            return InvokeResult.Success("添加成功!");
        }

        /// <summary>
        /// 更新老师信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> UpdateTeacher(Teacher req)
        {
            using var db = new NDatabase();
            await db.UpdateAsync(req);

            return InvokeResult.Success("更新成功!");
        }
        /// <summary>
        /// 删除老师信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> DeleteTeacher(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<Teacher>(id);
            model.IsDelete = DID.Entitys.IsEnum.是;
            await db.UpdateAsync(model);

            return InvokeResult.Success("删除成功!");
        }

    }
}