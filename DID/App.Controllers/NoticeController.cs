
using App.Entity;
using App.Models.Request;
using App.Services;
using DID.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    /// <summary>
    /// APP公告接口
    /// </summary>
    [ApiController]
    [Route("api/notice")]
    public class NoticeController : Controller
    {
        private readonly ILogger<NoticeController> _logger;

        private readonly INoticeService _service;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public NoticeController(ILogger<NoticeController> logger, INoticeService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("notice")]
        [AllowAnonymous]
        public async Task<Response<List<Notice>>> GetNotice()
        {
            return await _service.GetNotice();
        }
        /// <summary>
        /// 获取公告
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("noticebyid")]
        [AllowAnonymous]
        public async Task<Response<Notice>> GetNotice(string id)
        {
            return await _service.GetNotice(id);
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("notice")]
        public async Task<Response> AddNotice(AddNoticeReq req)
        {
            return await _service.AddNotice(req);
        }
        /// <summary>
        /// 更新公告
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("notice")]
        public async Task<Response> UpdateNotice(Notice req)
        {
            return await _service.UpdateNotice(req);
        }
        /// <summary>
        /// 删除公告
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("notice")]
        public async Task<Response> DeleteNotice(string id)
        {
            return await _service.DeleteNotice(id);
        }
    }
}
