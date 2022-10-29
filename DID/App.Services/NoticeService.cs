

using App.Entity;
using App.Models.Request;
using DID.Common;
using DID.Models.Base;
using Microsoft.Extensions.Logging;

namespace App.Services
{
    /// <summary>
    /// 公告服务接口
    /// </summary>
    public interface INoticeService
    {
        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <returns></returns>
        Task<Response<List<Notice>>> GetNotice();
        /// <summary>
        /// 获取公告
        /// </summary>
        /// <returns></returns>
        Task<Response<Notice>> GetNotice(string id);
        /// <summary>
        /// 添加公告
        /// </summary>
        /// <returns></returns>
        Task<Response> AddNotice(AddNoticeReq req);
        /// <summary>
        /// 更新公告
        /// </summary>
        /// <returns></returns>
        Task<Response> UpdateNotice(Notice req);
        /// <summary>
        /// 删除公告
        /// </summary>
        /// <returns></returns>
        Task<Response> DeleteNotice(string id);
    }

    /// <summary>
    /// 公告服务
    /// </summary>
    public class NoticeService : INoticeService
    {
        private readonly ILogger<NoticeService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>s
        public NoticeService(ILogger<NoticeService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<Notice>>> GetNotice()
        {
            using var db = new NDatabase();
            var list = await db.FetchAsync<Notice>("select * from App_Notice where IsDelete = 0 order by CreateDate Desc");

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        /// <returns></returns>
        public async Task<Response<Notice>> GetNotice(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<Notice>(id);

            return InvokeResult.Success(model);
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <returns></returns>
        public async Task<Response> AddNotice(AddNoticeReq req)
        {
            using var db = new NDatabase();
            var model = new Notice {
                Content = req.Content,
                CreateDate = DateTime.Now,
                CreatorName = req.CreatorName,
                Title = req.Title,
                NoticeId = Guid.NewGuid().ToString()
            };
            await db.InsertAsync(model);

            return InvokeResult.Success("添加成功!");
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <returns></returns>
        public async Task<Response> UpdateNotice(Notice req)
        {
            using var db = new NDatabase();
            await db.UpdateAsync(req);

            return InvokeResult.Success("更新成功!");
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <returns></returns>
        public async Task<Response> DeleteNotice(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<Notice>(id);
            model.IsDelete = DID.Entitys.IsEnum.是;
            await db.UpdateAsync(model);

            return InvokeResult.Success("删除成功!");
        }

    }
}