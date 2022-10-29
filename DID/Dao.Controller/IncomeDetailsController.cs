using Dao.Common;
using Dao.Common.ActionFilter;
using Dao.Entity;
using Dao.Models.Base;
using Dao.Models.Request;
using Dao.Models.Response;
using Dao.Services;
using DID.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dao.Controllers
{
    /// <summary>
    /// Dao 收益详情
    /// </summary>
    [ApiController]
    [Route("api/incomedetails")]
    [AllowAnonymous]
    [DaoActionFilter]
    public class IncomeDetailsController : Controller
    {
        private readonly ILogger<IncomeDetailsController> _logger;

        private readonly IIncomeDetailsService _service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public IncomeDetailsController(ILogger<IncomeDetailsController> logger, IIncomeDetailsService service)
        {
            _logger = logger;
            _service = service;
        }
        /// <summary>
        /// 添加收益详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("addincomedetails")]
        public async Task<Response> AddIncomeDetails(AddIncomeDetailsReq req)
        {
            return await _service.AddIncomeDetails(req);
        }

        /// <summary>
        /// 获取收益详情
        /// </summary>
        /// <param name="req"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        [HttpPost]
        [Route("getincomedetails")]
        public async Task<Response<List<IncomeDetailsRespon>>> GetIncomeDetails(DaoBasePageReq req)
        {
            return await _service.GetIncomeDetails(req, req.Page, req.ItemsPerPage);
        }

        /// <summary>
        /// 获取总收益
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("gettotalincome")]
        public async Task<Response<double>> GetTotalIncome(DaoBaseReq req)
        { 
            return await _service.GetTotalIncome(req);
        }
    }
}
