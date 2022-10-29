
using App.Entity;
using App.Models.Request;
using App.Models.Respon;
using App.Services;
using DID.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    /// <summary>
    /// APP课程接口
    /// </summary>
    [ApiController]
    [Route("api/course")]
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;

        private readonly ICourseService _service;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public CourseController(ILogger<CourseController> logger, ICourseService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 获取课程信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("course")]
        public async Task<Response<List<Course>>> GetCourse()
        {
            return await _service.GetCourse();
        }
        /// <summary>
        /// 获取课程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("coursebyid")]
        public async Task<Response<GetCourseRespon>> GetCourse(string id)
        {
            return await _service.GetCourse(id);
        }

        /// <summary>
        /// 添加课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("course")]
        public async Task<Response> AddCourse(AddCourseReq req)
        {
            return await _service.AddCourse(req);
        }
        /// <summary>
        /// 更新课程
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("course")]
        public async Task<Response> UpdateCourse(Course req)
        {
            return await _service.UpdateCourse(req);
        }
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("course")]
        public async Task<Response> DeleteCourse(string id)
        {
            return await _service.DeleteCourse(id);
        }
    }
}
