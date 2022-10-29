using Dao.Common;
using Dao.Common.ActionFilter;
using Dao.Entity;
using Dao.Models.Base;
using Dao.Models.Request;
using Dao.Models.Response;
using Dao.Services;
using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dao.Controllers
{
    /// <summary>
    /// 仲裁接口
    /// </summary>
    [ApiController]
    [Route("api/arbitrate")]
    [AllowAnonymous]
    [DaoActionFilter]
    public class ArbitrateController : Controller
    {
        private readonly ILogger<ArbitrateController> _logger;

        private readonly IArbitrateService _service;

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public ArbitrateController(ILogger<ArbitrateController> logger, IArbitrateService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 获取审核员信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getarbitrator")]
        public async Task<Response<GetArbitratorRespon>> GetArbitrator(DaoBaseReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetArbitrator(userId);
        }

        /// <summary>
        /// 解除仲裁员身份
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("relievearbitrator")]
        public async Task<Response> RelieveArbitrator(DaoBaseReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.RelieveArbitrator(userId);
        }


        /// <summary>
        /// 获取仲裁员列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getarbitrators")]
        public async Task<Response<List<GetArbitratorsRespon>>> GetArbitrators(DaoBaseReq req)
        {
            return await _service.GetArbitrators();
        }

        /// <summary>
        /// 获取仲裁公示
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getarbitrateinfo")]
        public async Task<Response<List<GetArbitrateInfoRespon>>> GetArbitrateInfo(DaoBaseReq req)
        {
            return await _service.GetArbitrateInfo();
        }

        /// <summary>
        /// 获取仲裁详情
        /// </summary>
        /// param name="arbitrateInfoId"
        /// <returns></returns>
        [HttpPost]
        [Route("getarbitratedetails")]
        public async Task<Response<GetArbitrateDetailsRespon>> GetArbitrateDetails(GetArbitrateDetailsReq req) 
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetArbitrateDetails(req.ArbitrateInfoId, userId);
        }

        /// <summary>
        /// 提交仲裁
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addarbitrateinfo")]
        public async Task<Response> AddArbitrateInfo(GetArbitrateInfoReq req)
        {
            return await _service.AddArbitrateInfo(req.Plaintiff, req.Defendant, req.OrderId, req.Num, req.Memo, req.Images, req.ArbitrateInType);
        }


        /// <summary>
        /// 获取待处理 已仲裁（原告、被告）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getuserarbitrate")]
        public async Task<Response<List<GetArbitrateInfoRespon>>> GetUserArbitrate(GetUserArbitrateReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetUserArbitrate(userId, req.Type);
        }

        /// <summary>
        /// 获取待仲裁 已结案列表 0 待仲裁 1 已仲裁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getarbitratelist")]
        public async Task<Response<List<GetArbitrateInfoRespon>>> GetArbitrateList(GetUserArbitrateReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetArbitrateList(userId, req.Type);
        }


        /// <summary>
        /// 仲裁员投票
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("arbitratevote")]
        public async Task<Response> ArbitrateVote(ArbitrateVoteReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.ArbitrateVote(req.ArbitrateInfoId, userId, req.Reason, req.Status);
        }

        /// <summary>
        /// 申请延期
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("arbitratedelay")]
        public async Task<Response> ArbitrateDelay(ArbitrateDelayReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.ArbitrateDelay(req.ArbitrateInfoId, userId, req.Reason, req.Explain, req.Day,req.IsArbitrate);
        }

        /// <summary>
        /// 申请延期投票
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("arbitratedelayvote")]
        public async Task<Response> ArbitrateDelayVote(ArbitrateDelayVoteReq req)
        {
            return await _service.ArbitrateDelayVote(req.DelayVoteId, req.Status);
        }

        /// <summary>
        /// 追加举证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("addadducelist")]
        public async Task<Response> AddAdduceList(AddAdduceListReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.AddAdduceList(req.ArbitrateInfoId, userId, req.Memo, req.Images);
        }

        /// <summary>
        /// 取消仲裁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("cancelarbitrate")]
        public async Task<Response> CancelArbitrate(CancelArbitrateReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.CancelArbitrate(userId, req.ArbitrateInfoId, req.CancelReason);
        }

        /// <summary>
        /// 获取用户认证信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getuserinfo")]
        public async Task<Response<RiskUserInfo>> GetUserInfo(DaoBaseByIdReq req)
        {
            return await _service.GetUserInfo(req.Id);
        }

        /// <summary>
        /// 获取仲裁消息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getarbitratemessage")]
        public async Task<Response<List<GetArbitrateMessageRespon>>> GetArbitrateMessage(GetArbitrateMessageReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetArbitrateMessage(userId, req.IsArbitrate);
        }

        /// <summary>
        /// 获取延期消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getarbitratedelay")]
        public async Task<Response<GetArbitrateDelayRespon>> GetArbitrateDelay(GetArbitrateDelayReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetArbitrateDelay(userId, req.Id, req.IsArbitrate);
        }

        /// <summary>
        /// 获取取消仲裁消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getcancelarbitrate")]
        public async Task<Response<GetCancelArbitrateRespon>> GetCancelArbitrate(DaoBaseByIdReq req)
        {
            return await _service.GetCancelArbitrate(req.Id);
        }

        /// <summary>
        /// 获取追加举证消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getadducelist")]
        public async Task<Response<GetAdduceListRespon>> GetAdduceList(DaoBaseByIdReq req)
        {
            return await _service.GetAdduceList(req.Id);
        }

        /// <summary>
        /// 获取结案通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getclosure")]
        public async Task<Response<GetClosureRespon>> GetClosure(DaoBaseByIdReq req)
        {
            return await _service.GetClosure(req.Id);
        }

        /// <summary>
        /// 设置消息为已读
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("setmessageisopen")]
        public async Task<Response> SetMessageIsOpen(DaoBaseByIdReq req)
        {
            return await _service.SetMessageIsOpen(req.Id);
        }

        /// <summary>
        /// 获取是否有未读消息 0 原被告 1 仲裁员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getmessageisopen")]
        public async Task<Response<int>> GetMessageIsOpen(GetArbitrateMessageReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetMessageIsOpen(userId, req.IsArbitrate);
        }

        /// <summary>
        /// 获取被告待处理消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getwaitmessage")]
        public async Task<Response<int>> GetWaitMessage(DaoBaseReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetWaitMessage(userId);
        }

        /// <summary>
        /// 添加支付信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addarbitratepay")]
        public async Task<Response> AddArbitratePay(AddArbitratePayReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.AddArbitratePay(req, userId);
        }

    }
}
