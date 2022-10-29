

using App.Entity;
using App.Models.Request;
using App.Models.Respon;
using DID.Common;
using DID.Models.Base;
using Microsoft.Extensions.Logging;
using Snowflake.Core;

namespace App.Services
{
    /// <summary>
    /// 订单接口
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        Task<Response<List<GetOrderRespon>>> GetOrder();
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        Task<Response<GetOrderRespon>> GetOrder(string id);
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        Task<Response<List<GetOrderByUserIdRespon>>> GetOrderByUserId(string userId, StatusEnum? status);
        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <returns></returns>
        Task<Response> AddOrder(AddOrderReq req,string userId);
        /// <summary>
        /// 支付订单
        /// </summary>
        /// <returns></returns>
        Task<Response> PayOrder(string orderid, string userId);
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        Task<Response> CancelOrder(string orderid, string userId);
        /// <summary>
        /// 更新订单信息
        /// </summary>
        /// <returns></returns>
        Task<Response> UpdateOrder(Order req);
        /// <summary>
        /// 更新订单用户信息
        /// </summary>
        /// <returns></returns>
        Task<Response> UpdateOrderUser(UpdateOrderUserReq req);
        /// <summary>
        /// 删除订单信息
        /// </summary>
        /// <returns></returns>
        Task<Response> DeleteOrder(string id);
    }

    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public OrderService(ILogger<OrderService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<GetOrderRespon>>> GetOrder()
        {
            using var db = new NDatabase();
            var list = await db.FetchAsync<GetOrderRespon>("select * from App_Order where IsDelete = 0");
            list.ForEach(a => {
                if (a.OrderType == OrderTypeEnum.课程)
                    a.Course = db.SingleOrDefaultById<Course>(a.Rid);
                if (a.OrderType == OrderTypeEnum.系统)
                    a.CLSystem = db.SingleOrDefaultById<CLSystem>(a.Rid);
                a.Volunteer = db.SingleOrDefaultById<Volunteer>(a.VolunteerId);
            });
            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<GetOrderRespon>> GetOrder(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultAsync<GetOrderRespon>("select * from App_Order where OrderId = @0", id);
            if(null == model)
                return InvokeResult.Fail<GetOrderRespon>("订单信息未找到!");
            if (model.OrderType == OrderTypeEnum.课程)
                model.Course = db.SingleOrDefaultById<Course>(model.Rid);
            if (model.OrderType == OrderTypeEnum.系统)
                model.CLSystem = db.SingleOrDefaultById<CLSystem>(model.Rid);
            model.Volunteer = db.SingleOrDefaultById<Volunteer>(model.VolunteerId);

            return InvokeResult.Success(model);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<GetOrderByUserIdRespon>>> GetOrderByUserId(string userId, StatusEnum? status)
        {
            using var db = new NDatabase();
            var list = new List<GetOrderByUserIdRespon>();
            if (null == status)
                list = await db.FetchAsync<GetOrderByUserIdRespon>("select * from App_Order where DIDUserId = @0 and IsDelete = 0", userId);
            else
                list = await db.FetchAsync<GetOrderByUserIdRespon>("select * from App_Order where DIDUserId = @0 and IsDelete = 0 and Status = @1", userId, status);

            list.ForEach(a => {
                if (a.OrderType == OrderTypeEnum.课程)
                    a.Course = db.SingleOrDefaultById<Course>(a.Rid);
                if (a.OrderType == OrderTypeEnum.系统)
                    a.CLSystem = db.SingleOrDefaultById<CLSystem>(a.Rid);
            });

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> AddOrder(AddOrderReq req, string userId)
        {
            using var db = new NDatabase();
            var model = new Order {
                OrderId = new IdWorker(1, 1).NextId().ToString(),
                Rid = req.Rid,
                Name = req.Name,
                CreateDate = DateTime.Now,
                DIDUserId = userId,
                OrderType = req.OrderType,
                Phone = req.Phone,
                Wechat = req.Wechat,
                Quantity = req.Quantity,
                Status = StatusEnum.待支付
            };
            await db.InsertAsync(model);

            return InvokeResult.Success("添加成功!");
        }

        /// <summary>
        /// 支付订单
        /// </summary>
        /// <returns></returns>
        public async Task<Response> PayOrder(string orderid, string userId)
        {
            using var db = new NDatabase();
            var order = await db.SingleOrDefaultByIdAsync<Order>(orderid);
            if(userId != order.DIDUserId)
                return InvokeResult.Fail("支付失败!");
            if (order.Status != StatusEnum.待支付)
                return InvokeResult.Fail("支付失败!");
            order.Status = StatusEnum.已支付;
            order.PaymentDate = DateTime.Now;
            var list = await db.FetchAsync<Volunteer>("select * from App_Volunteer where IsDelete = 0");
            if (list.Count > 0)
            {
                var random = new Random().Next(list.Count);
                order.VolunteerId = list[random].VolunteerId;
            }
            await db.UpdateAsync(order);

            return InvokeResult.Success("支付成功!");
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        public async Task<Response> CancelOrder(string orderid, string userId)
        {
            using var db = new NDatabase();
            var order = await db.SingleOrDefaultByIdAsync<Order>(orderid);
            if (userId != order.DIDUserId)
                return InvokeResult.Fail("取消失败!");
            if(order.Status != StatusEnum.待支付)
                return InvokeResult.Fail("取消失败!");
            order.CancelDate = DateTime.Now;
            order.Status = StatusEnum.已取消;
            await db.UpdateAsync(order);

            return InvokeResult.Success("取消成功!");
        }

        /// <summary>
        /// 更新订单信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> UpdateOrder(Order req)
        {
            using var db = new NDatabase();
            await db.UpdateAsync(req);

            return InvokeResult.Success("更新成功!");
        }

        /// <summary>
        /// 更新订单用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> UpdateOrderUser(UpdateOrderUserReq req)
        {
            using var db = new NDatabase();
            var order = await db.SingleOrDefaultByIdAsync<Order>(req.OrderId);
            if (null == order)
                return InvokeResult.Fail("订单信息未找到!");
            if(order.Status != StatusEnum.待支付)
                return InvokeResult.Fail("订单不能修改!");
            order.Name = req.Name;
            order.Phone = req.Phone;
            order.Wechat = req.Wechat;
            await db.UpdateAsync(order);
            return InvokeResult.Success("更新成功!");
        }
        /// <summary>
        /// 删除订单信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response> DeleteOrder(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<Order>(id);
            model.IsDelete = DID.Entitys.IsEnum.是;
            await db.UpdateAsync(model);

            return InvokeResult.Success("删除成功!");
        }

    }
}