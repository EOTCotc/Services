using Dao.Common;
using Dao.Entity;
using Dao.Models.Base;
using Dao.Models.Request;
using Dao.Models.Response;
using DID.Common;
using DID.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Dao.Services
{
    /// <summary>
    /// 工单接口
    /// </summary>
    public interface IWorkOrderService
    {
        /// <summary>
        /// 添加工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response> AddWorkOrder(AddWorkOrderReq req);

        /// <summary>
        /// 社区图片上传 1 请上传文件! 2 文件类型错误!
        /// </summary>
        /// <returns></returns>
        Task<Response> UploadImage(IFormFile file, string type);

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response<List<GetWorkOrderListRespon>>> GetWorkOrderList(GetWorkOrderListReq req);

        /// <summary>
        /// 获取工单详情
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<Response<GetWorkOrderRespon>> GetWorkOrder(string workOrderId);

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response> WorkOrderStatus(WorkOrderStatusReq req);

    }

    /// <summary>
    /// 工单服务
    /// </summary>
    public class WorkOrderService : IWorkOrderService
    {
        private readonly ILogger<WorkOrderService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public WorkOrderService(ILogger<WorkOrderService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 添加工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response> AddWorkOrder(AddWorkOrderReq req)
        {
            var walletId = WalletHelp.GetWalletId(req);

            using var db = new NDatabase();
            var model = new WorkOrder()
            {
                WorkOrderId = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                WorkOrderType = req.WorkOrderType,
                Describe = req.Describe,
                Images = req.Images,
                Phone = req.Phone,
                WorkOrderStatus = WorkOrderStatusEnum.待处理,
                WalletId = walletId
            };
            await db.InsertAsync(model);
            return InvokeResult.Success("提交成功!");
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Response> UploadImage(IFormFile file,string type)
        {
            try
            {
                var dir = new DirectoryInfo(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, $"Images/{type}/"));

                //保存目录不存在就创建这个目录
                if (!dir.Exists)
                {
                    Directory.CreateDirectory(dir.FullName);
                }
                //var filename = upload.UserId + "_" + upload.Type + ".jpg";
                var filename = Guid.NewGuid().ToString() + ".jpg";
                using (var stream = new FileStream(dir.FullName + filename, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    //file.CopyTo(stream);
                }
                //return InvokeResult.Success("Images/AuthImges/" + upload.UId + "/" + filename);
                return InvokeResult.Success($"Images/{type}/{filename}");
            }
            catch (Exception e)
            {
                _logger.LogError("UploadImage", e);
                return InvokeResult.Fail("Fail");
            }
        }

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response<List<GetWorkOrderListRespon>>> GetWorkOrderList(GetWorkOrderListReq req)
        {
            var walletId = WalletHelp.GetWalletId(req);
            using var db = new NDatabase();
            var models = new List<WorkOrder>();
            if (null == req.WorkOrderType)
            {
                if (req.WorkOrderStatus == WorkOrderStatusEnum.待处理)
                    models = (await db.PageAsync<WorkOrder>(req.Page, req.ItemsPerPage, "select * from WorkOrder where WorkOrderStatus = @0 order by CreateDate desc", req.WorkOrderStatus)).Items;
                else//只能看自己处理的
                    models = (await db.PageAsync<WorkOrder>(req.Page, req.ItemsPerPage, "select * from WorkOrder where WorkOrderStatus = @0 and HandleWalletId = @1 order by CreateDate desc", req.WorkOrderStatus, walletId)).Items;
            }
            else
            {
                if (req.WorkOrderStatus == WorkOrderStatusEnum.待处理)
                    models = (await db.PageAsync<WorkOrder>(req.Page, req.ItemsPerPage, "select * from WorkOrder where WorkOrderStatus = @0 and WorkOrderType = @1 order by CreateDate desc", req.WorkOrderStatus, req.WorkOrderType)).Items;
                else//只能看自己处理的
                    models = (await db.PageAsync<WorkOrder>(req.Page, req.ItemsPerPage, "select * from WorkOrder where WorkOrderStatus = @0 and WorkOrderType = @1 and HandleWalletId = @2 order by CreateDate desc", req.WorkOrderStatus, req.WorkOrderType, walletId)).Items;
            }
            var list = models.Select(x => new GetWorkOrderListRespon()
            {
                WorkOrderId = x.WorkOrderId,
                CreateDate = x.CreateDate,
                Describe = x.Describe,
                Status = x.WorkOrderStatus,
                Submitter = WalletHelp.GetSubmitter(x.WalletId),
                Type = x.WorkOrderType
            }).ToList();

            return InvokeResult.Success(list);
        }

        

        /// <summary>
        /// 获取工单详情
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<Response<GetWorkOrderRespon>> GetWorkOrder(string workOrderId)
        {
            using var db = new NDatabase();

            var model = await db.SingleOrDefaultByIdAsync<WorkOrder>(workOrderId);

            var respon = new GetWorkOrderRespon()
            {
                CreateDate = model.CreateDate,
                Describe = model.Describe,
                Images = model.Images,
                Phone = model.Phone,
                Status = model.WorkOrderStatus,
                Handle = WalletHelp.GetSubmitter(model.HandleWalletId),
                Submitter = WalletHelp.GetSubmitter(model.WalletId),
                Record = model.Record
            };
            return InvokeResult.Success(respon);
        }


        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response> WorkOrderStatus(WorkOrderStatusReq req)
        {
            using var db = new NDatabase();
            if (req.WorkOrderStatus == WorkOrderStatusEnum.处理中)
            {
                var walletId = WalletHelp.GetWalletId(req);
                var model = await db.SingleOrDefaultByIdAsync<WorkOrder>(req.WorkOrderId);
                model.HandleWalletId = walletId;
                model.WorkOrderStatus = req.WorkOrderStatus;
                model.Record = req.Record;
                await db.UpdateAsync(model);
            }
            else if(req.WorkOrderStatus == WorkOrderStatusEnum.已处理)
            {
                var model = await db.SingleOrDefaultByIdAsync<WorkOrder>(req.WorkOrderId);
                model.WorkOrderStatus = req.WorkOrderStatus;
                model.Record = req.Record;
                await db.UpdateAsync(model);
            }
            else if (req.WorkOrderStatus == WorkOrderStatusEnum.待处理)
            {
                var model = await db.SingleOrDefaultByIdAsync<WorkOrder>(req.WorkOrderId);
                model.WorkOrderStatus = req.WorkOrderStatus;
                model.HandleWalletId = "";
                model.Record = req.Record;
                await db.UpdateAsync(model);
            }

            return InvokeResult.Success("修改成功!");
        }
    }
}