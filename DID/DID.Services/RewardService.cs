
using Dao.Entity;
using Dao.Models.Base;
using Dao.Models.Request;
using Dao.Models.Response;
using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using DID.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DID.Services
{
    /// <summary>
    /// 收益设置接口
    /// </summary>
    public interface IRewardService
    {
        /// <summary>
        /// 获取收益EOTC数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Response<double>> GetRewardValue(string key);

        /// <summary>
        /// 获取收益设置
        /// </summary>
        /// <returns></returns>
        Task<Response<List<Reward>>> GetReward();

        /// <summary>
        /// 更新收益设置
        /// </summary>
        /// <returns></returns>
        Task<Response> UpdateReward(List<Reward> list);


        /// <summary>
        /// 获取仲裁扣费（0天1人）
        /// </summary>
        /// <param name="num"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Response<double>> GetArbDayEotc(int num, int type);
    }

    /// <summary>
    /// 收益设置服务
    /// </summary>
    public class RewardService : IRewardService
    {
        private readonly ILogger<RewardService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public RewardService(ILogger<RewardService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取仲裁扣费（天）
        /// </summary>
        /// <param name="num"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Response<double>> GetArbDayEotc(int num, int type)
        {
            using var db = new NDatabase();
            double value = 0;
            if(type == 0)
                value = await db.SingleOrDefaultAsync<double>("select RewardValue from Reward where RewardKey = 'ArbDay'");
            else if(type == 1)
                value = await db.SingleOrDefaultAsync<double>("select RewardValue from Reward where RewardKey = 'ArbPeople'");

            return InvokeResult.Success(value * num);
        }

        /// <summary>
        /// 获取收益EOTC数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Response<double>> GetRewardValue(string key)
        {
            using var db = new NDatabase();

            var value = await db.SingleOrDefaultAsync<double>("select RewardValue from Reward where RewardKey = @0", key);

            return InvokeResult.Success(value);
        }

        /// <summary>
        /// 获取收益设置
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<Reward>>> GetReward()
        {
            using var db = new NDatabase();

            var list = await db.FetchAsync<Reward> ("select * from Reward");

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 更新收益设置
        /// </summary>
        /// <returns></returns>
        public async Task<Response> UpdateReward(List<Reward> list)
        {
            using var db = new NDatabase();
            db.BeginTransaction();
            foreach (var a in list)
            {
                await db.ExecuteAsync("update Reward set RewardValue = @0 where RewardKey = @1", a.RewardValue, a.RewardKey);
            }
            db.CompleteTransaction();
            return InvokeResult.Success("更新成功!");
        }
    }
}