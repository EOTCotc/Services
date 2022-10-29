using Dao.Common;
using Dao.Entity;
using Dao.Models.Base;
using Dao.Models.Request;
using Dao.Models.Response;
using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using Microsoft.Extensions.Logging;

namespace Dao.Services
{
    /// <summary>
    /// 收益详情服务接口
    /// </summary>
    public interface IIncomeDetailsService
    {
        /// <summary>
        /// 添加收益详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response> AddIncomeDetails(AddIncomeDetailsReq req);

        /// <summary>
        /// 获取收益详情
        /// </summary>
        /// <param name="req"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        Task<Response<List<IncomeDetailsRespon>>> GetIncomeDetails(DaoBaseReq req, long page, long itemsPerPage);

        /// <summary>
        /// 获取总收益
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response<double>> GetTotalIncome(DaoBaseReq req);

    }

    /// <summary>
    /// 收益详情服务
    /// </summary>
    public class IncomeDetailsService : IIncomeDetailsService
    {
        private readonly ILogger<IncomeDetailsService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public IncomeDetailsService(ILogger<IncomeDetailsService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 添加收益详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response> AddIncomeDetails(AddIncomeDetailsReq req)
        {
            using var db = new NDatabase();
            var userId = WalletHelp.GetUserId(req);
            var item = new IncomeDetails() { 
                IncomeDetailsId = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                EOTC = req.EOTC,
                Remarks = req.Remarks,
                Type = req.Type,
                WalletId = WalletHelp.GetWalletId(req),
                DIDUserId = userId
            };
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);

            db.BeginTransaction();
            await db.InsertAsync(item);
            user.DaoEOTC += req.EOTC;
            await db.UpdateAsync(user);
            db.CompleteTransaction();

            return InvokeResult.Success("添加成功");
        }

        /// <summary>
        /// 获取总收益
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response<double>> GetTotalIncome(DaoBaseReq req)
        {
            using var db = new NDatabase();
            var walletIds = WalletHelp.GetWalletIds(req);
            var list = await db.FetchAsync<double>("select EOTC from IncomeDetails where WalletId in (@0)", walletIds);
            var total = list.Sum();
            return InvokeResult.Success(total);
        }

        /// <summary>
        /// 获取收益详情
        /// </summary>
        /// <param name="req"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        public async Task<Response<List<IncomeDetailsRespon>>> GetIncomeDetails(DaoBaseReq req, long page, long itemsPerPage)
        {
            //var walletIds = WalletHelp.GetWalletIds(req);
            var userId = WalletHelp.GetUserId(req);
            using var db = new NDatabase();
            var items = (await db.PageAsync<IncomeDetails>(page, itemsPerPage, "select * from IncomeDetails where DIDUserId = @0", userId)).Items;
            var list = items.Select(a => new IncomeDetailsRespon() {
                EOTC = a.EOTC,
                Type = a.Type,
                CreateDate = a.CreateDate
            }).ToList();
            return InvokeResult.Success(list);
        }
    }

}