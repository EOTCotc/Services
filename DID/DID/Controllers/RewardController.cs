    using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using DID.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DID.Controllers
{
    /// <summary>
    /// 奖励设置相关接口
    /// </summary>
    [ApiController]
    [Route("api/reward")]
    public class RewardController : Controller
    {
        private readonly ILogger<RewardController> _logger;

        private readonly IRewardService _service;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        public RewardController(ILogger<RewardController> logger, IRewardService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 获取收益EOTC数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getrewardvalue")]
        public async Task<Response<string>> GetRewardValue(string key)
        {
            return await _service.GetRewardValue(key);
        }

        /// <summary>
        /// 获取收益设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getreward")]
        public async Task<Response<List<Reward>>> GetReward()
        {
            return await _service.GetReward();
        }

        /// <summary>
        /// 更新收益设置
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("updatereward")]
        public async Task<Response> UpdateReward(List<Reward> list)
        {
            return await _service.UpdateReward(list);
        }

        /// <summary>
        /// 获取仲裁扣费（0天1人）
        /// </summary>
        /// <param name="num"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getarbdayeotc")]
        [AllowAnonymous]
        public async Task<Response<double>> GetArbDayEotc(int num, int type)
        {
            return await _service.GetArbDayEotc(num, type);
        }
    }
}
