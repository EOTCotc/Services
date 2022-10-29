
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
    /// APP禅论系统接口
    /// </summary>
    [ApiController]
    [Route("api/clsystem")]
    public class CLSystemController : Controller
    {
        private readonly ILogger<CLSystemController> _logger;

        private readonly ICLSystemService _service;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public CLSystemController(ILogger<CLSystemController> logger, ICLSystemService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 获取禅论系统信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("clsystem")]
        public async Task<Response<List<CLSystem>>> GetCLSystem()
        {
            return await _service.GetCLSystem();
        }
        /// <summary>
        /// 获取禅论系统
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("clsystembyid")]
        public async Task<Response<CLSystem>> GetCLSystem(string id)
        {
            return await _service.GetCLSystem(id);
        }

        /// <summary>
        /// 添加禅论系统
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("clsystem")]
        public async Task<Response> AddCLSystem(AddCLSystemReq req)
        {
            return await _service.AddCLSystem(req);
        }
        /// <summary>
        /// 更新禅论系统
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("clsystem")]
        public async Task<Response> UpdateCLSystem(CLSystem req)
        {
            return await _service.UpdateCLSystem(req);
        }
        /// <summary>
        /// 删除禅论系统
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("clsystem")]
        public async Task<Response> DeleteCLSystem(string id)
        {
            return await _service.DeleteCLSystem(id);
        }
    }
}
