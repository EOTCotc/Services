using Dao.Common;
using Dao.Common.ActionFilter;
using Dao.Models.Base;
using Dao.Models.Request;
using Dao.Models.Response;
using Dao.Services;
using DID.Entitys;
using DID.Models.Base;
using DID.Models.Response;
using DID.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dao.Controllers
{
    /// <summary>
    /// Dao审核
    /// </summary>
    [ApiController]
    [Route("api/examine")]
    [AllowAnonymous]
    [DaoActionFilter]
    public class ExamineController : Controller
    {
        private readonly ILogger<ExamineController> _logger;

        private readonly IUserAuthService _authservice;

        private readonly ICommunityService _comservice;

        private readonly ITeamAuthService _teamservice;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="authservice"></param>
        /// <param name="comservice"></param>
        public ExamineController(ILogger<ExamineController> logger, IUserAuthService authservice, ICommunityService comservice, ITeamAuthService teamservice)
        {
            _logger = logger;
            _authservice = authservice;
            _comservice = comservice;
            _teamservice = teamservice;
        }

        /// <summary>
        /// 获取用户未审核信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getunauditedinfo")]
        public async Task<Response<List<UserAuthRespon>>> GetUnauditedInfo(DaoBasePageReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _authservice.GetUnauditedInfo(userId, IsEnum.是, req.Page, req.ItemsPerPage, req.Key);
        }

        /// <summary>
        /// 获取用户已审核审核信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getauditedinfo")]
        public async Task<Response<List<UserAuthRespon>>> GetAuditedInfo(DaoBasePageReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _authservice.GetAuditedInfo(userId, IsEnum.是, req.Page, req.ItemsPerPage, req.Key);
        }

        /// <summary>
        /// 获取用户打回信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getbackinfo")]
        public async Task<Response<List<UserAuthRespon>>> GetBackInfo(DaoBasePageReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _authservice.GetBackInfo(userId, IsEnum.是, req.Page, req.ItemsPerPage, req.Key);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("auditinfo")]
        public async Task<Response> AuditInfo(DaoAuditInfoReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _authservice.AuditInfo(req.UserAuthInfoId, userId, req.AuditType, req.Remark, true);
        }

        /// <summary>
        /// 获取社区打回信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getbackcom")]
        public async Task<Response<List<ComAuthRespon>>> GetBackCom(DaoBasePageReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _comservice.GetBackCom(userId, IsEnum.是, req.Page, req.ItemsPerPage);
        }

        /// <summary>
        /// 获取社区未审核信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getunauditedcom")]
        public async Task<Response<List<ComAuthRespon>>> GetUnauditedCom(DaoBasePageReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _comservice.GetUnauditedCom(userId, IsEnum.是, req.Page, req.ItemsPerPage);
        }

        /// <summary>
        /// 获取社区已审核审核信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getauditedcom")]
        public async Task<Response<List<ComAuthRespon>>> GetAuditedCom(DaoBasePageReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _comservice.GetAuditedCom(userId, IsEnum.是, req.Page, req.ItemsPerPage);
        }

        /// <summary>
        /// 社区申请审核
        /// </summary>
        /// <returns> </returns>
        [HttpPost]
        [Route("auditcommunity")]
        public async Task<Response> AuditCommunity(DaoAuditCommunityReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _comservice.AuditCommunity(req.CommunityId, userId, req.AuditType, req.Remark, true);
        }

        /// <summary>
        /// 获取团队认证列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getteamauthlist")]
        public async Task<Response<List<GetTeamAuthListRespon>>> GetTeamAuthList(GetTeamAuthListReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _teamservice.GetTeamAuthList(userId, req.Type);
        }

        /// <summary>
        /// 团队认证
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("teamauth")]
        public async Task<Response> TeamAuth(TeamAuthReq req)
        {
            var userId = WalletHelp.GetUserId(req);
            return await _teamservice.TeamAuth(req.TeamAuthId, userId, req.AuditType, req.Remark );
        }

        /// <summary>
        /// 获取社区信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getcominfo")]
        public async Task<Response<GetComInfoRespon>> GetComInfo(GetComInfoReq req)
        {
            return await _teamservice.GetComInfo(req.RefUserId);
        }
    }
}
