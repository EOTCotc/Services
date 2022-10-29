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
    /// Dao 工单
    /// </summary>
    [ApiController]
    [Route("api/workorder")]
    [AllowAnonymous]
    [DaoActionFilter]
    public class WorkOrderController : Controller
    {
        private readonly ILogger<WorkOrderController> _logger;

        private readonly IWorkOrderService _service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public WorkOrderController(ILogger<WorkOrderController> logger, IWorkOrderService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 添加工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("addworkorder")]
        public async Task<Response> AddWorkOrder(AddWorkOrderReq req)
        {
            return await _service.AddWorkOrder(req);
        }


        /// <summary>
        /// 工单图片上传 1 请上传文件! 2 文件类型错误!
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("uploadimage")]
        //public async Task<Response> UploadImage(string type)
        //{
        //    var files = Request.Form.Files;
        //    if (files.Count == 0) return InvokeResult.Fail("1");//请上传文件!
        //    if (!CommonHelp.IsPicture(files[0])) return InvokeResult.Fail("2");//文件类型错误!

        //    return await _service.UploadImage(files[0],type);
        //}

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getworkorderlist")]
        public async Task<Response> GetWorkOrderList(GetWorkOrderListReq req)
        {
            return await _service.GetWorkOrderList(req);
        }

        /// <summary>
        /// 获取工单详情
        /// </summary>
        /// <param name="req"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getworkorder")]
        public async Task<Response<GetWorkOrderRespon>> GetWorkOrder(GetWorkOrderReq req)
        { 
            return await _service.GetWorkOrder(req.WorkOrderId);
        }

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("workorderstatus")]
        public async Task<Response> WorkOrderStatus(WorkOrderStatusReq req)
        {
            return await _service.WorkOrderStatus(req);
        }
    }
}
