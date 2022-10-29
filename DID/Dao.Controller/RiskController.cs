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
    /// 风控
    /// </summary>
    [ApiController]
    [Route("api/risk")]
    [AllowAnonymous]
    [DaoActionFilter]
    public class RiskController : Controller
    {
        private readonly ILogger<RiskController> _logger;

        private readonly IRiskService _service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public RiskController(ILogger<RiskController> logger, IRiskService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 设置用户风险等级
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("userrisklevel")]
        public async Task<Response> UserRiskLevel(UserRiskLevelReq req)
        {
            return await _service.UserRiskLevel(req);
        }

        /// <summary>
        /// 获取风控列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("userrisk")]
        public async Task<Response<List<UserRiskRespon>>> UserRisk(DaoBaseReq req)
        {
            return await _service.UserRisk(req);
        }

        /// <summary>
        /// 修改用户风险状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("userriskstatus")]
        public async Task<Response> UserRiskStatus(UserRiskStatusReq req)
        {
            return await _service.UserRiskStatus(req);
        }

        /// <summary>
        /// 解除风险
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("removerisk")]
        public async Task<Response> RemoveRisk(RemoveRiskReq req)
        { 
            return await _service.RemoveRisk(req);
        }

        /// <summary>
        /// 获取风险用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getuserinfo")]
        public async Task<Response<RiskUserInfo>> GetUserInfo(RemoveRiskReq req)
        {
            return await _service.GetUserInfo(req);
        }

        /// <summary>
        /// 获取用户风险等级
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getuserrisklevel")]
        public async Task<Response<RiskLevelEnum>> GetUserRiskLevel(DaoBaseReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetUserRiskLevel(userId);
        }

        /// <summary>
        /// 获取解除风控联系人
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getrisklist")]
        public async Task<Response<List<GetRiskList>>> GetRiskList(DaoBaseReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _service.GetRiskList(userId);
        }

    }
}
