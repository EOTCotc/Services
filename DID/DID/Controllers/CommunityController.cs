using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using DID.Models.Request;
using DID.Models.Response;
using DID.Services;
using Microsoft.AspNetCore.Mvc;

namespace DID.Controllers
{
    /// <summary>
    /// 社区相关接口
    /// </summary>
    [ApiController]
    [Route("api/community")]
    public class CommunityController : Controller
    {
        private readonly ILogger<CommunityController> _logger;

        private readonly ICommunityService _service;

        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        /// <param name="currentUser"></param>
        public CommunityController(ILogger<CommunityController> logger, ICommunityService service, ICurrentUser currentUser)
        {
            _logger = logger;
            _service = service;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 设置用户社区选择（未填邀请码） 1 请勿重复设置! 2 位置信息错误!
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("setcomselect")]
        public async Task<Response> SetComSelect(ComSelectReq req)
        {
            req.UserId = _currentUser.UserId;
            return await _service.SetComSelect(req);
        }

        /// <summary>
        /// 获取选择社区位置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getcomselect")]
        public async Task<Response<ComSelectRespon>> GetComSelect()
        {
            return await _service.GetComSelect(_currentUser.UserId);
        }

        /// <summary>
        /// 获取已有社区位置
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        [Route("getcomaddr")]
        public async Task<Response<ComAddrRespon>> GetComAddr()
        {
            return await _service.GetComAddr();
        }

        /// <summary>
        /// 获取当前位置社区数量
        /// </summary>
        /// <param name="country"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getcomnum")]
        public async Task<Response<int>> GetComNum(string country, string province, string city, string area)
        {
            return await _service.GetComNum(country, province, city, area);
        }

        /// <summary>
        /// 获取当前位置社区信息
        /// </summary>
        /// <param name="country"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="area"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getcomlist")]
        public async Task<Response<List<ComRespon>>> GetComList(string country, string province, string city, string area, long page, long itemsPerPage)
        {
            return await _service.GetComList(country, province, city, area, page, itemsPerPage);
        }

        /// <summary>
        /// 获取打回信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getbackcom")]
        public async Task<Response<List<ComAuthRespon>>> GetBackCom(long page, long itemsPerPage)
        {
            return await _service.GetBackCom(_currentUser.UserId, IsEnum.否, page, itemsPerPage);
        }

        /// <summary>
        /// 获取未审核信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getunauditedcom")]
        public async Task<Response<List<ComAuthRespon>>> GetUnauditedCom(long page, long itemsPerPage)
        {
            return await _service.GetUnauditedCom(_currentUser.UserId, IsEnum.否, page, itemsPerPage);
        }

        /// <summary>
        /// 获取已审核审核信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getauditedcom")]
        public async Task<Response<List<ComAuthRespon>>> GetAuditedCom(long page, long itemsPerPage)
        {
            return await _service.GetAuditedCom(_currentUser.UserId, IsEnum.否, page, itemsPerPage);
        }

        /// <summary>
        /// 社区申请审核
        /// </summary>
        /// <returns> </returns>
        [HttpPost]
        [Route("auditcommunity")]
        public async Task<Response> AuditCommunity(AuditCommunityReq req)
        {
            return await _service.AuditCommunity(req.CommunityId, _currentUser.UserId, req.AuditType, req.Remark);
        }

        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        [Route("getcommunityinfo")]
        public async Task<Response<GetCommunityInfoRespon>> GetCommunityInfo()
        {
            return await _service.GetCommunityInfo(_currentUser.UserId);
        }

        /// <summary>
        /// 查询社区信息
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        [Route("getcommunityinfobyid")]
        public async Task<Response<GetCommunityInfoRespon>> GetCommunityInfoById(string communityId)
        {
            return await _service.GetCommunityInfoById(communityId);
        }

        /// <summary>
        /// 添加社区信息
        /// </summary>
        /// <returns> </returns>
        [HttpPut]
        [Route("addcommunityinfo")]
        public async Task<Response> AddCommunityInfo(Community item)
        {
            item.DIDUserId = _currentUser.UserId;
            return await _service.AddCommunityInfo(item);
        }

        /// <summary>
        /// 社区申请
        /// </summary>
        /// <returns> </returns>
        [HttpPost]
        [Route("applycommunity")]
        public async Task<Response<string>> ApplyCommunity(Community item)
        {
            item.DIDUserId = _currentUser.UserId;
            return await _service.ApplyCommunity(item);
        }

        /// <summary>
        /// 社区图片上传 1 请上传文件! 2 文件类型错误!
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadimage")]
        public async Task<Response> UploadImage()
        {
            var files = Request.Form.Files;
            if (files.Count == 0) return InvokeResult.Fail("1");//请上传文件!
            if (!CommonHelp.IsPicture(files[0])) return InvokeResult.Fail("2");//文件类型错误!

            return await _service.UploadImage(files[0]);
        }

        /// <summary>
        /// 获取社区审核失败信息
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getcomauthfail")]
        public async Task<Response<List<AuthInfo>>> GetComAuthFail(string communityId)
        {
            return await _service.GetComAuthFail(communityId);
        }

        /// <summary>
        /// 社区名是否重复
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        [Route("hascomname")]
        public async Task<Response<bool>> HasComName(string comName)
        {
            return await _service.HasComName(comName);
        }
        /// <summary>
        /// 添加社区
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="comName"></param>
        [HttpPut]
        [Route("addcom")]
        public async Task<Response> AddCom(int uid, string comName)
        {
            return await _service.AddCom(_currentUser.UserId, uid, comName);
        }

    }
}
