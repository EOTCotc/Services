using Dao.Models.Response;
using Dao.Services;
using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using DID.Models.Request;
using DID.Models.Response;
using DID.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;

namespace DID.Controllers
{
    /// <summary>
    /// 用户相关接口
    /// </summary>
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly IUserService _service;

        private readonly IMemoryCache _cache;

        private readonly ICurrentUser _currentUser;

        private readonly IRiskService _riskservice;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="service"></param>
        /// <param name="cache"></param>
        /// <param name="currentUser"></param>
        /// <param name="riskservice"></param>
        public UserController(ILogger<UserController> logger, IUserService service, IMemoryCache cache, ICurrentUser currentUser, IRiskService riskservice)
        {
            _logger = logger;
            _service = service;
            _cache = cache;
            _currentUser = currentUser;
            _riskservice = riskservice;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getuserinfo")]
        public async Task<Response<UserInfoRespon>> GetUserInfo(/*int uid*/)
        {
            return await _service.GetUserInfo(_currentUser.UserId);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getuserinfobyuid")]
        public async Task<Response<UserInfoRespon>> GetUserInfoByUid(int uid)
        {
            return await _service.GetUserInfoByUid(uid);
        }

        /// <summary>
        /// 更新用户信息（邀请人 电报群 国家地区） 1 邀请码错误!
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("setuserinfo")]
        public async Task<Response> SetUserInfo(UserInfoRespon user)
        {
            user.UserId = _currentUser.UserId;
            return await _service.SetUserInfo(user);
        }

        /// <summary>
        /// 登录 1 邮箱格式错误! 2 邮箱未注册! 3 密码错误! 4 钱包地址错误! 5 登录错误!
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<Response<string>> Login(LoginReq login)
        {
           if (!string.IsNullOrEmpty(login.Mail) && !CommonHelp.IsMail(login.Mail))
                //return InvokeResult.Fail<string>("1");
                return InvokeResult.Fail<string>("邮箱格式错误!");
            //var code = _cache.Get(login.Mail)?.ToString();
            //if (code != login.Code)
            //    return InvokeResult.Fail<string>("验证码错误!");
            return await _service.Login(login);
        }

        /// <summary>
        /// 注册 1 邮箱格式错误! 2 验证码错误! 3 请勿重复注册! 4 邀请码错误! 5 钱包地址为空!
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<Response> Register(LoginReq login)
        {
            if (!CommonHelp.IsMail(login.Mail))
                //return InvokeResult.Fail<string>("1");//邮箱格式错误!
                return InvokeResult.Fail<string>("邮箱格式错误!");
            var code = _cache.Get(login.Mail)?.ToString();
            _cache.Remove(login.Mail);
            if (code != login.Code)
                //return InvokeResult.Fail<string>("2");//验证码错误!
                return InvokeResult.Fail<string>("验证码错误!");
            //if(string.IsNullOrEmpty(login.WalletAddress)||string.IsNullOrEmpty(login.Otype)|| string.IsNullOrEmpty(login.Sign))
            //    return InvokeResult.Fail<string>("5");//钱包地址为空!
            return await _service.Register(login);
        }


        /// <summary>
        /// 获取验证码 1 邮箱格式错误!
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="type">0 注册验证码 1 验证码</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getcode")]
        [AllowAnonymous]
        public async Task<Response> GetCode(string mail,int type)
        {
            if (!CommonHelp.IsMail(mail))
                //return InvokeResult.Fail<string>("1");//邮箱格式错误!
                return InvokeResult.Fail<string>("邮箱格式错误!");
            return await _service.GetCode(mail, type);
        }

        /// <summary>
        /// 修改密码 1 验证码错误!
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("changepwd")]
        public async Task<Response> ChangePassword(ChangePasswordReq req)
        {
            var usercode = _cache.Get(req.Mail)?.ToString();
            _cache.Remove(req.Mail);
            if (usercode != req.Code)
                //return InvokeResult.Fail<string>("1"); //验证码错误!
                return InvokeResult.Fail<string>("验证码错误!");
            return await _service.ChangePassword(_currentUser.UserId, req.NewPassWord);
        }

        /// <summary>
        /// 找回密码 1 验证码错误!
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("retrievepassword")]
        [AllowAnonymous]
        public async Task<Response> RetrievePassword(ChangePasswordReq req)
        {
            var usercode = _cache.Get(req.Mail)?.ToString();
            _cache.Remove(req.Mail);
            if (usercode != req.Code)
                //return InvokeResult.Fail<string>("1"); //验证码错误!
                return InvokeResult.Fail<string>("验证码错误!");
            return await _service.RetrievePassword(req.Mail, req.NewPassWord);
        }

        /// <summary>
        /// 修改邮箱 1 钱包验证错误! 2 邮箱已注册! 3 验证码错误!
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("changemail")]
        public async Task<Response> ChangeMail(ChangeMailReq req)
        {
            var usercode = _cache.Get(req.Mail)?.ToString();
            _cache.Remove(req.Mail);
            if (usercode != req.Code)
                //return InvokeResult.Fail<string>("3"); //验证码错误!
                return InvokeResult.Fail<string>("验证码错误!");
            return await _service.ChangeMail(_currentUser.UserId, req);
        }


        /// <summary>
        /// 获取邀请码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getrefuserid")]
        public async Task<Response<string>> GetRefUserId()
        {
            return InvokeResult.Success<string>(_currentUser.UserId);
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("logout")]
        public async Task<Response> Logout(LogoutReq req)
        {
            var usercode = _cache.Get(req.Mail)?.ToString();
            _cache.Remove(req.Mail);
            if (usercode != req.Code)
                //return InvokeResult.Fail<string>("1"); //验证码错误!
                return InvokeResult.Fail<string>("验证码错误!");
            return await _service.Logout(_currentUser.UserId, req.Reason);
        }

        /// <summary>
        /// 取消注销
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("cancellogout")]
        public async Task<Response> CancelLogout()
        {
            return await _service.CancelLogout(_currentUser.UserId);
        }

        /// <summary>
        /// 获取提交注销时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getlogoutdate")]
        public async Task<Response<DateTime>> GetLogoutDate()
        {
            return await _service.GetLogoutDate(_currentUser.UserId);
        }

        /// <summary>
        /// 获取团队信息
        /// </summary>
        /// <param name="isAuth">是否认证 null 查看所有</param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getuserteam")]
        public async Task<Response<TeamInfoRespon>> GetUserTeam(bool? isAuth, long page, long itemsPerPage)
        {
            return await _service.GetUserTeam(_currentUser.UserId, isAuth, page, itemsPerPage);
        }

        /// <summary>
        /// 提交团队申请
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("teamauth")]
        public async Task<Response> TeamAuth()
        {
            return await _service.TeamAuth(_currentUser.UserId);
        }

        /// <summary>
        /// 获取用户风险等级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getuserrisklevel")]
        public async Task<Response<RiskLevelEnum>> GetUserRiskLevel()
        {
            return await _riskservice.GetUserRiskLevel(_currentUser.UserId);
        }

        /// <summary>
        /// 获取解除风控联系人
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getrisklist")]
        public async Task<Response<List<GetRiskList>>> GetRiskList()
        {
            return await _riskservice.GetRiskList(_currentUser.UserId);
        }

        /// <summary>
        /// 获取认证图片
        /// </summary>
        [HttpGet]
        [Route("getauthimage")]
        public IActionResult GetAuthImage(string path)
        {
            return _service.GetAuthImage(path, _currentUser.UserId);
        }

        /// <summary>
        /// 获取用户质押数量
        /// </summary>
        [HttpGet]
        [Route("getusereotc")]
        public async Task<Response<double>> GetUserEOTC()
        {
            return await _service.GetUserEOTC(_currentUser.UserId);
        }

        /// <summary>
        /// 设置App支付密码
        /// </summary>
        /// <param name="payPassWord"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("setpaypassword")]
        public async Task<Response> SetPayPassWord(SetPayPassWordReq req)
        {
            var usercode = _cache.Get(req.Mail)?.ToString();
            _cache.Remove(req.Mail);
            if (usercode != req.Code)
                //return InvokeResult.Fail<string>("1"); //验证码错误!
                return InvokeResult.Fail<string>("验证码错误!");
            return await _service.SetPayPassWord(_currentUser.UserId, req.PayPassWord);
        }
    }
}
