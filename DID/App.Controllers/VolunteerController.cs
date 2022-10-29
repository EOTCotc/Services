
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
    /// APP自愿者接口
    /// </summary>
    [ApiController]
    [Route("api/volunteer")]
    public class VolunteerController : Controller
    {
        private readonly ILogger<VolunteerController> _logger;

        private readonly IVolunteerService _service;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public VolunteerController(ILogger<VolunteerController> logger, IVolunteerService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 获取自愿者信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("volunteer")]
        public async Task<Response<List<Volunteer>>> GetVolunteer()
        {
            return await _service.GetVolunteer();
        }
        /// <summary>
        /// 获取自愿者
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("volunteerbyid")]
        public async Task<Response<Volunteer>> GetVolunteer(string id)
        {
            return await _service.GetVolunteer(id);
        }

        /// <summary>
        /// 添加自愿者
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("volunteer")]
        public async Task<Response> AddVolunteer(AddVolunteerReq req)
        {
            return await _service.AddVolunteer(req);
        }
        /// <summary>
        /// 更新自愿者
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("volunteer")]
        public async Task<Response> UpdateVolunteer(Volunteer req)
        {
            return await _service.UpdateVolunteer(req);
        }
        /// <summary>
        /// 删除自愿者
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("volunteer")]
        public async Task<Response> DeleteVolunteer(string id)
        {
            return await _service.DeleteVolunteer(id);
        }
    }
}
