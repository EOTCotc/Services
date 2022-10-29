
using App.Entity;
using App.Models.Request;
using App.Models.Respon;
using App.Services;
using DID.Common;
using DID.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    /// <summary>
    /// APP订单接口
    /// </summary>
    [ApiController]
    [Route("api/order")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        private readonly IOrderService _service;

        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public OrderController(ILogger<OrderController> logger, IOrderService service, ICurrentUser currentUser)
        {
            _logger = logger;
            _service = service;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("order")]
        public async Task<Response<List<GetOrderRespon>>> GetOrder()
        {
            return await _service.GetOrder();
        }
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("orderbyid")]
        public async Task<Response<GetOrderRespon>> GetOrder(string id)
        {
            return await _service.GetOrder(id);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getorderbyuserid")]
        public async Task<Response<List<GetOrderByUserIdRespon>>> GetOrderByUserId(StatusEnum? status)
        {
            return await _service.GetOrderByUserId(_currentUser.UserId, status);
        }

        /// <summary>
        /// 支付订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("payorder")]
        public async Task<Response> PayOrder(string orderid)
        {
            return await _service.PayOrder(orderid, _currentUser.UserId);
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("cancelorder")]
        public async Task<Response> CancelOrder(string orderid)
        {
            return await _service.CancelOrder(orderid, _currentUser.UserId);
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("order")]
        public async Task<Response> AddOrder(AddOrderReq req)
        {
            return await _service.AddOrder(req, _currentUser.UserId);
        }
        /// <summary>
        /// 更新订单
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("order")]
        public async Task<Response> UpdateOrder(Order req)
        {
            return await _service.UpdateOrder(req);
        }
        /// <summary>
        /// 更新订单用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("orderuser")]
        public async Task<Response> UpdateOrderUser(UpdateOrderUserReq req)
        {
            return await _service.UpdateOrderUser(req);
        }
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("order")]
        public async Task<Response> DeleteOrder(string id)
        {
            return await _service.DeleteOrder(id);
        }
    }
}
