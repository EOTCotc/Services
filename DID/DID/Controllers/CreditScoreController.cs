using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using DID.Models.Request;
using DID.Models.Response;
using DID.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DID.Controllers
{
    /// <summary>
    /// 信用分
    /// </summary>
    [ApiController]
    [Route("api/creditscore")]
    public class CreditScoreController : Controller
    {
        private readonly ILogger<CreditScoreController> _logger;

        private readonly ICreditScoreService _service;

        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        /// <param name="currentUser"></param>
        public CreditScoreController(ILogger<CreditScoreController> logger, ICreditScoreService service, ICurrentUser currentUser)
        {
            _logger = logger;
            _service = service;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 信用分操作(加分、 减分) 1 参数不合法! 2 用户未找到! 3 信用分不足!
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("creditscore")]
        public async Task<Response> CreditScore(CreditScoreReq req)
        {
            if (req.Fraction <= 0)
                return InvokeResult.Fail("1"); //参数不合法!
            return await _service.CreditScore(req);
        }

        /// <summary>
        /// 获取信用分记录和当前信用分
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <param name="type">类型 0 加分 1 减分</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getcreditscore")]
        public async Task<Response<GetCreditScoreRespon>> GetCreditScore(long page, long itemsPerPage, TypeEnum type)
        {
            return await _service.GetCreditScore(_currentUser.UserId, page, itemsPerPage, type);
        }

        /// <summary>
        /// 获取用户信用分
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getcreditscorebyuid")]
        [AllowAnonymous]
        public async Task<int> GetCreditScoreByUid(string uId)
        {
            return await _service.GetCreditScoreByUid(uId);
        }

    }
}
