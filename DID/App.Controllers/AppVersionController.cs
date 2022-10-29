
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
    /// APP版本管理接口
    /// </summary>
    [ApiController]
    [Route("api/appversion")]
    public class AppVersionController : Controller
    {
        private readonly ILogger<AppVersionController> _logger;

        private readonly IAppVersionService _service;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public AppVersionController(ILogger<AppVersionController> logger, IAppVersionService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 获取AppVersion信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("appversion")]
        public async Task<Response<List<AppVersion>>> GetAppVersion()
        {
            return await _service.GetAppVersion();
        }
        /// <summary>
        /// 获取AppVersion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("appversionbyid")]
        public async Task<Response<AppVersion>> GetAppVersion(string id)
        {
            return await _service.GetAppVersion(id);
        }
        /// <summary>
        /// 获取最新App版本信息
        /// </summary>
        /// <param name="osType">0 android 1 ios</param>
        /// <returns></returns>
        [HttpGet]
        [Route("appversionbytype")]
        public async Task<Response<AppVersion>> GetAppVersion(int osType)
        {
            return await _service.GetAppVersion(osType);
        }

        /// <summary>
        /// 添加AppVersion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("appversion")]
        public async Task<Response> AddAppVersion(AddAppVersionReq req)
        {
            return await _service.AddAppVersion(req);
        }
        /// <summary>
        /// 更新AppVersion
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("appversion")]
        public async Task<Response> UpdateAppVersion(AppVersion req)
        {
            return await _service.UpdateAppVersion(req);
        }
        /// <summary>
        /// 删除AppVersion
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("appversion")]
        public async Task<Response> DeleteAppVersion(string id)
        {
            return await _service.DeleteAppVersion(id);
        }
    }
}
