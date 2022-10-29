using Dao.Common;
using Dao.Common.ActionFilter;
using Dao.Entity;
using Dao.Models.Base;
using Dao.Models.Request;
using Dao.Models.Response;
using Dao.Services;
using DID.Common;
using DID.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dao.Controllers
{
    /// <summary>
    /// Dao 销毁记录
    /// </summary>
    [ApiController]
    [Route("api/destruction")]
    [AllowAnonymous]
    //[DaoActionFilter]
    public class DestructionController : Controller
    {
        private readonly ILogger<DestructionController> _logger;

        private readonly IDestructionService _service;

        private readonly IWorkOrderService _workservice;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public DestructionController(ILogger<DestructionController> logger, IDestructionService service, IWorkOrderService workservice)
        {
            _logger = logger;
            _service = service;
            _workservice = workservice;
        }
        /// <summary>
        /// 添加销毁记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("adddestruction")]
        public async Task<Response> AddDestruction(Destruction req)
        { 
            return await _service.AddDestruction(req);
        }

        /// <summary>
        /// 查询销毁记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getdestruction")]
        public async Task<Response<List<Destruction>>> GetDestruction(GetDestructionReq req)
        {
            return await _service.GetDestruction(req);
        }

        /// <summary>
        /// 删除销毁记录
        /// </summary>
        /// <param name="destructionId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("destruction")]
        public async Task<Response> DeleteDestruction(string destructionId)
        {
            return await _service.DeleteDestruction(destructionId);
        }

        /// <summary>
        /// 工单图片上传 1 请上传文件! 2 文件类型错误!
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadimage")]
        public async Task<Response> UploadImage(string type)
        {
            var files = Request.Form.Files;
            if (files.Count == 0) return InvokeResult.Fail("1");//请上传文件!
            if (!CommonHelp.IsPicture(files[0])) return InvokeResult.Fail("2");//文件类型错误!

            return await _workservice.UploadImage(files[0], type);
        }
    }
}
