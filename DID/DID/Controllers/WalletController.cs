    using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using DID.Services;
using Microsoft.AspNetCore.Mvc;

namespace DID.Controllers
{
    /// <summary>
    /// 钱包相关接口
    /// </summary>
    [ApiController]
    [Route("api/wallet")]
    public class WalletController : Controller
    {
        private readonly ILogger<WalletController> _logger;

        private readonly IWalletService _service;

        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        /// <param name="currentUser"></param>
        public WalletController(ILogger<WalletController> logger, IWalletService service, ICurrentUser currentUser)
        {
            _logger = logger;
            _service = service;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 绑定公链地址
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("setwallet")]
        public async Task<Response> SetWallet(Wallet req)
        {
            req.DIDUserId = _currentUser.UserId;
            return await _service.SetWallet(req);
        }

        /// <summary>
        /// 获取用户公链地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getwallets")]
        public async Task<Response<List<Wallet>>> GetWallets()
        {
            return await _service.GetWallets(_currentUser.UserId);
        }

        /// <summary>
        /// 取消授权
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deletewallet")]
        public async Task<Response> DeleteWallet(string walletId)
        {
            return await _service.DeleteWallet(walletId);
        }
    }
}
