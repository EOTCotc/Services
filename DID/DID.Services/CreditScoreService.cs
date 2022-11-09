using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using DID.Models.Request;
using DID.Models.Response;
using Microsoft.Extensions.Logging;

namespace DID.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICreditScoreService
    {
        /// <summary>
        /// 信用分操作(加分、 减分)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Response> CreditScore(CreditScoreReq req);

        /// <summary>
        /// 获取信用分记录和当前信用分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Response<GetCreditScoreRespon>> GetCreditScore(string userId, long page, long itemsPerPage, TypeEnum type);

        /// <summary>
        /// 获取用户信用分
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        Task<int> GetCreditScoreByUid(string uId);
    }
    /// <summary>
    /// 信用分服务
    /// </summary>
    public class CreditScoreService : ICreditScoreService
    {
        private readonly ILogger<CreditScoreService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CreditScoreService(ILogger<CreditScoreService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///// 信用分操作(加分、 减分)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<Response> CreditScore(CreditScoreReq req)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where Uid = @0 and IsLogout = 0", req.Uid);
            if(null == user)
                return InvokeResult.Fail("用户未找到!");//用户未找到!

            var item = new CreditScoreHistory
            {
                CreditScoreHistoryId = Guid.NewGuid().ToString(),
                Fraction = req.Fraction,
                Type = req.Type,
                Remarks = req.Remarks,
                CreateDate = DateTime.Now,
                DIDUserId = user.DIDUserId
            };
            
            db.BeginTransaction();
            if (item.Type == TypeEnum.加分)
                await db.ExecuteAsync("update DIDUser set CreditScore = CreditScore + @1  where DIDUserId = @0", user.DIDUserId, item.Fraction);
            else
            {
                if(user.CreditScore < req.Fraction)
                    return InvokeResult.Fail("信用分不足!");//信用分不足!
                await db.ExecuteAsync("update DIDUser set CreditScore = CreditScore - @1  where DIDUserId = @0", user.DIDUserId, item.Fraction);
            }
            var insert = await db.InsertAsync(item);
            db.CompleteTransaction();
            return InvokeResult.Success("记录插入成功!");
        }

        /// <summary>
        /// 获取信用分记录和当前信用分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Response<GetCreditScoreRespon>> GetCreditScore(string userId, long page, long itemsPerPage, TypeEnum type)
        {
            using var db = new NDatabase();
            //var list = await db.FetchAsync<CreditScoreHistory>("select * from CreditScoreHistory where DIDUserId=@0", userId);
            var list = (await db.PageAsync<CreditScoreHistory>(page, itemsPerPage, "select * from CreditScoreHistory where DIDUserId=@0 and Type = @1 order by CreateDate", userId, type)).Items;
            var fraction = await db.SingleOrDefaultAsync<int>("select CreditScore from DIDUser where DIDUserId = @0", userId);

            return InvokeResult.Success(new GetCreditScoreRespon { CreditScore = fraction, Items = list });
        }

        /// <summary>
        /// 获取用户信用分
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public async Task<int> GetCreditScoreByUid(string uId)
        {
            using var db = new NDatabase();
            var fraction = await db.SingleOrDefaultAsync<int>("select CreditScore from DIDUser where Uid = @0", uId);
            return fraction;
        }

    }
}
