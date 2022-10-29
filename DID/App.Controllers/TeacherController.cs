
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
    /// APP老师接口
    /// </summary>
    [ApiController]
    [Route("api/teacher")]
    public class TeacherController : Controller
    {
        private readonly ILogger<TeacherController> _logger;

        private readonly ITeacherService _service;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public TeacherController(ILogger<TeacherController> logger, ITeacherService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 获取老师信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("teacher")]
        public async Task<Response<List<Teacher>>> GetTeacher()
        {
            return await _service.GetTeacher();
        }
        /// <summary>
        /// 获取老师
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("teacherbyid")]
        public async Task<Response<Teacher>> GetTeacher(string id)
        {
            return await _service.GetTeacher(id);
        }

        /// <summary>
        /// 添加老师
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("teacher")]
        public async Task<Response> AddTeacher(AddTeacherReq req)
        {
            return await _service.AddTeacher(req);
        }
        /// <summary>
        /// 更新老师
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("teacher")]
        public async Task<Response> UpdateTeacher(Teacher req)
        {
            return await _service.UpdateTeacher(req);
        }
        /// <summary>
        /// 删除老师
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("teacher")]
        public async Task<Response> DeleteTeacher(string id)
        {
            return await _service.DeleteTeacher(id);
        }
    }
}
