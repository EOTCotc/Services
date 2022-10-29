using DID.Common;
using DID.Models.Base;
using DID.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace DID.Controllers
{
    /// <summary>
    /// 项目相关接口
    /// </summary>
    [ApiController]
    [Route("api/project")]
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;

        private readonly IProjectService _service;

        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        /// <param name="currentUser"></param>
        public ProjectController(ILogger<ProjectController> logger, IProjectService service, ICurrentUser currentUser)
        {
            _logger = logger;
            _service = service;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 获取绑定项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getprojects")]
        public async Task<Response<List<UserProjectRespon>>> GetProjects()
        {
            return await _service.GetProjects(_currentUser.UserId);
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("unbind")]
        public async Task<Response> Unbind( string projectId)
        {
            return await _service.Unbind(_currentUser.UserId, projectId);
        }
    }
}
