using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DID.Services
{
    /// <summary>
    /// 钱包接口
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// 绑定公链地址
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response> SetWallet(Wallet req);

        /// <summary>
        /// 获取用户公链地址
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<List<Wallet>>> GetWallets(string userId);

        /// <summary>
        /// 取消授权
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        Task<Response> DeleteWallet(string walletId);
    }
    /// <summary>
    /// 钱包服务
    /// </summary>
    public class WalletService : IWalletService
    {
        private readonly ILogger<WalletService> _logger;

        private readonly IMemoryCache _cache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="cache"></param>
        public WalletService(ILogger<WalletService> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        /// <summary>
        /// 获取用户公链地址
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<List<Wallet>>> GetWallets(string userId)
        {
            using var db = new NDatabase();
            var list = await db.FetchAsync<Wallet>("select * from Wallet where DIDUserId = @0 and IsLogout = 0 and IsDelete = 0", userId);
            return InvokeResult.Success(list);
        }
        /// <summary>
        /// 绑定公链地址
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response> SetWallet(Wallet req)
        {
            using var db = new NDatabase();
            req.CreateDate = DateTime.Now;
            await db.InsertAsync(req);
            return InvokeResult.Success("插入成功!");
        }
        /// <summary>
        /// 取消授权
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public async Task<Response> DeleteWallet(string walletId)
        {
            using var db = new NDatabase();
            await db.ExecuteAsync("update Wallet set IsDelete = @0,DeleteDate = @1 where WalletId = @2", IsEnum.是,DateTime.Now, walletId);
            return InvokeResult.Success("取消成功!");
        }
    }
}
