using DID.Common;
using DID.Entity;
using DID.Entitys;
using DID.Models.Base;
using DID.Models.Request;
using DID.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NPoco;
using RestSharp;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DID.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<UserInfoRespon>> GetUserInfo(string userId);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<Response<UserInfoRespon>> GetUserInfoByUid(int uid);

        /// <summary>
        /// 更新用户信息（邀请人 电报群 国家地区）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Response> SetUserInfo(UserInfoRespon user);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<Response<string>> Login(LoginReq login);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<Response> Register(LoginReq login);

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Response> GetCode(string mail, int type);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassWord"></param>
        /// <returns></returns>
        Task<Response> ChangePassword(string userId, string newPassWord);
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="newPassWord"></param>
        /// <returns></returns>
        Task<Response> RetrievePassword(string mail, string newPassWord);

        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response> ChangeMail(string userId, ChangeMailReq req);

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <returns></returns>
        Task<Response> Logout(string userId, string? reasons);

        /// <summary>
        /// 取消注销
        /// </summary>
        /// <returns></returns>
        Task<Response> CancelLogout(string userId);

        /// <summary>
        /// 获取提交注销时间
        /// </summary>
        /// <returns></returns>
        Task<Response<DateTime>> GetLogoutDate(string userId);

        /// <summary>
        /// 获取团队信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isAuth"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        Task<Response<TeamInfoRespon>> GetUserTeam(string userId, bool? IsAuth, long page, long itemsPerPage);

        /// <summary>
        /// 提交团队申请
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response> TeamAuth(string userId);


        /// <summary>
        /// 获取用户质押数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<double>> GetUserEOTC(string userId);

        /// <summary>
        /// 获取认证图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IActionResult> GetAuthImage(string path, string userId);

        /// <summary>
        /// 设置App支付密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="payPassWord"></param>
        /// <returns></returns>
        Task<Response> SetPayPassWord(string userId, string payPassWord);

        /// <summary>
        /// 修改邀请人
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="newUid"></param>
        /// <returns></returns>
        Task<Response> UpdatePid(int uId, int newUid);

        /// <summary>
        /// 获取社区名称
        /// </summary>
        /// <returns></returns>
        Task<Response<GetInfoRespon>> GetInfo(int uId);
    }
    /// <summary>
    /// 审核认证服务
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly IMemoryCache _cache;


        private readonly IRewardService _reservice;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="cache"></param>
        public UserService(ILogger<UserService> logger, IMemoryCache cache, IRewardService reservice)
        {
            _logger = logger;
            _cache = cache;
            _reservice = reservice;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<Response<UserInfoRespon>> GetUserInfoByUid(int uid)
        {
            var userRespon = new UserInfoRespon();
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where Uid = @0", uid);
            if(null == user)
                return InvokeResult.Fail<UserInfoRespon>("用户信息未找到!");

            userRespon.Uid = user.Uid;
            userRespon.UserId = user.DIDUserId;
            if (!string.IsNullOrEmpty(user.RefUserId))
                userRespon.RefUid = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", user.RefUserId);
            userRespon.CreditScore = user.CreditScore;
            userRespon.Mail = user.Mail;
            userRespon.Country = user.Country;
            userRespon.Area = user.Area;
            userRespon.Telegram = user.Telegram;
            userRespon.AuthType = user.AuthType;
            if (user.AuthType == AuthTypeEnum.审核成功)
            {
                var authInfo = await db.SingleOrDefaultByIdAsync<UserAuthInfo>(user.UserAuthInfoId);
                userRespon.Name = authInfo.Name;
                userRespon.PhoneNum = authInfo.PhoneNum;
            }
            userRespon.UserNode = user.UserNode;

            //用户社区信息
            if (!string.IsNullOrEmpty(user.RefUserId))
                userRespon.CommunityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", user.DIDUserId);
            if (!string.IsNullOrEmpty(user.ApplyCommunityId))
            {
                userRespon.ApplyCommunityId = user.ApplyCommunityId;
                var community = await db.SingleOrDefaultByIdAsync<Community>(user.ApplyCommunityId);
                userRespon.IsImprove = !string.IsNullOrEmpty(community.Telegram);
            }

            if (!string.IsNullOrEmpty(user.UserLogoutId))
                userRespon.HasLogout = true;

            //支付密码
            userRespon.HasPassWord = !string.IsNullOrEmpty(user.PayPassWord);

            //空投
            var model = CurrentUser.GetEUModel(user);
            userRespon.AirdropEotc = model?.Airdrop??0;

            userRespon.EOTC = model?.EOTC ?? 0;

            userRespon.USDT = model?.USDT ?? 0;

            userRespon.StakeEotc = model?.StakeEotc ?? 0;

            return InvokeResult.Success(userRespon);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<UserInfoRespon>> GetUserInfo(string userId)
        {
            var userRespon = new UserInfoRespon();
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            if (null == user)
                return InvokeResult.Fail<UserInfoRespon>("用户信息未找到!");

            userRespon.Uid = user.Uid;
            userRespon.UserId = userId;
            userRespon.RefUserId = user.RefUserId;
            if (!string.IsNullOrEmpty(user.RefUserId))
                userRespon.RefUid = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", user.RefUserId);
            userRespon.CreditScore = user.CreditScore;
            userRespon.Mail = user.Mail;
            var country = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = 'COUNTRIES'", user.Country);

            var province = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = @1", user.Province, user.Country);

            var city = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = @1", user.City, user.Province);

            var area = await db.SingleOrDefaultAsync<string>("select name from area where code = @0 and pcode = @1", user.Area, user.City);
            userRespon.Country = user.Country + "-" + country;
            userRespon.Province = user.Province + "-" + province;
            userRespon.City = user.City + "-" + city;
            userRespon.Area = user.Area + "-" + area;
            userRespon.Telegram = user.Telegram;
            userRespon.AuthType = user.AuthType;
            if (user.AuthType == AuthTypeEnum.审核成功)
            {
                var authInfo = await db.SingleOrDefaultByIdAsync<UserAuthInfo>(user.UserAuthInfoId);
                userRespon.Name = authInfo?.Name;
                userRespon.PhoneNum = authInfo?.PhoneNum;
            }
            userRespon.UserNode = user.UserNode;

            //用户社区信息
            if (!string.IsNullOrEmpty(user.RefUserId))
                userRespon.CommunityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0",userId);
            if (!string.IsNullOrEmpty(user.ApplyCommunityId))
            {
                userRespon.ApplyCommunityId = user.ApplyCommunityId;
                var community = await db.SingleOrDefaultByIdAsync<Community>(user.ApplyCommunityId);
                userRespon.IsImprove = !string.IsNullOrEmpty(community.Telegram);
                userRespon.ComAuditType = community.AuthType;
            }

            var auth = await db.SingleOrDefaultAsync<int>("select count(*) from Auth where AuditUserId = @0 and AuditType = 0 and IsDelete = 0 and IsDao = 0", userId);
            userRespon.HasAuth = auth > 0;

            var comauth = await db.SingleOrDefaultAsync<int>("select count(*) from ComAuth where AuditUserId = @0 and AuditType = 0 and IsDelete = 0 and IsDao = 0", userId);
            userRespon.HasComAuth = comauth > 0;

            if (!string.IsNullOrEmpty(user.UserLogoutId))
                userRespon.HasLogout = true;

            //支付密码
            userRespon.HasPassWord = !string.IsNullOrEmpty(user.PayPassWord);

            //空投
            var model = CurrentUser.GetEUModel(user);
            userRespon.AirdropEotc = model?.Airdrop ?? 0;

            userRespon.EOTC = model?.EOTC ?? 0;

            userRespon.USDT = model?.USDT ?? 0;

            userRespon.StakeEotc = model?.StakeEotc ?? 0;

            return InvokeResult.Success(userRespon);
        }

        /// <summary>
        /// 更新用户信息（邀请人 电报群 国家地区）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response> SetUserInfo(UserInfoRespon req)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", req.UserId);
            if (null == user)
                return InvokeResult.Fail<double>("用户信息未找到!");
            var sql = new Sql("update DIDUser set ");
            if (!string.IsNullOrEmpty(req.RefUserId) && string.IsNullOrEmpty(user.RefUserId))//邀请人不能修改
            {
                var refUserId = await db.SingleOrDefaultAsync<string>("select DIDUserId from DIDUser where DIDUserId = @0 and IsLogout = 0", req.RefUserId);
                var communityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", req.RefUserId);
                if (string.IsNullOrEmpty(refUserId) || req.UserId == refUserId || string.IsNullOrEmpty(communityId))//不能修改为自己 必须有社区
                    return InvokeResult.Fail("1"); //邀请码错误!
                sql.Append("RefUserId = @0, ", req.RefUserId);
            }
            if (!string.IsNullOrEmpty(req.Telegram))
                sql.Append("Telegram = @0, ", req.Telegram);
            if(!string.IsNullOrEmpty(req.Country))
                sql.Append("Country = @0, ", req.Country);
            if (!string.IsNullOrEmpty(req.Province))
                sql.Append("Province = @0, ", req.Province);
            if (!string.IsNullOrEmpty(req.City))
                sql.Append("City = @0, ", req.City);
            if (!string.IsNullOrEmpty(req.Area))
                sql.Append("Area = @0, ", req.Area);
            sql.Append("DIDUserId = @0 where DIDUserId = @0 and IsLogout = 0", req.UserId);

            
            db.BeginTransaction();
            //加入推荐人社区
            if (!string.IsNullOrEmpty(req.RefUserId) && string.IsNullOrEmpty(user.RefUserId))
            {
                if (req.RefUserId == req.UserId)
                    return InvokeResult.Fail("邀请码错误!");
                var communityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", req.RefUserId);
                var userCommunityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", req.UserId);
                if (!string.IsNullOrEmpty(communityId) && string.IsNullOrEmpty(userCommunityId))
                {
                    var userCom = new UserCommunity()
                    {
                        UserCommunityId = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.Now,
                        DIDUserId = req.UserId,
                        CommunityId = communityId
                    };
                    await db.InsertAsync(userCom);
                }

                //发放邀请人空投
                //if (!string.IsNullOrEmpty(req.RefUserId))
                //{
                //    var refUser = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0 and IsLogout = 0", req.RefUserId);
                //    var refeotc = _reservice.GetRewardValue("RefAirdrop").Result.Items;//奖励eotc数量
                //    refUser.AirdropEotc += refeotc;
                //    await db.UpdateAsync(refUser);
                //}

                //调用otc注册
                if (!string.IsNullOrEmpty(req.RefUserId))
                {
                    var uid = await db.SingleOrDefaultAsync<string>("select UId from DIDUser where DIDUserId = @0 and IsLogout = 0", user.DIDUserId);
                    var pid = await db.SingleOrDefaultAsync<string>("select UId from DIDUser where DIDUserId = @0 and IsLogout = 0", req.RefUserId);
                    var code = CurrentUser.RegisterEotc(user.Mail, "''","''","''", uid, pid, user.PassWord);
                    if(code <= 0)
                        return InvokeResult.Fail("otc用户注册失败!");
                }
            }

            await db.ExecuteAsync(sql);
            db.CompleteTransaction();
            return InvokeResult.Success("更新成功!");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<Response<string>> Login(LoginReq login)
        {
            //1.验证用户账号密码是否正确
            using var db = new NDatabase();
            var user = new DIDUser();
            if (!string.IsNullOrEmpty(login.Mail) && !string.IsNullOrEmpty(login.Password))//邮箱密码登录
            {
                user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where Mail = @0 and IsLogout = 0", login.Mail);

                if (null == user)
                    //return InvokeResult.Fail<string>("2");//邮箱未注册!
                    return InvokeResult.Fail<string>("邮箱未注册!");

                if (user.PassWord != login.Password)
                    //return InvokeResult.Fail<string>("3");//密码错误!
                    return InvokeResult.Fail<string>("密码错误!");
            }

            if (!string.IsNullOrEmpty(login.WalletAddress) && !string.IsNullOrEmpty(login.Otype) && !string.IsNullOrEmpty(login.Sign))//钱包登录
            {
                var wallet = await db.SingleOrDefaultAsync<Wallet>("select * from Wallet where WalletAddress = @0 and Otype = @1 and Sign = @2 and IsLogout = 0 and IsDelete = 0",
                                                            login.WalletAddress, login.Otype, login.Sign);
                if(null != wallet && string.IsNullOrEmpty(user.DIDUserId))
                    user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0 and IsLogout = 0", wallet.DIDUserId);

                if (!string.IsNullOrEmpty(user.DIDUserId))
                {
                    if (null == wallet)//绑定钱包到用户
                    {
                        var item = new Wallet
                        {
                            WalletId = Guid.NewGuid().ToString(),
                            Otype = login.Otype,
                            Sign = login.Sign,
                            WalletAddress = login.WalletAddress,
                            DIDUserId = user.DIDUserId,
                            CreateDate = DateTime.Now,
                            IsLogout = IsEnum.否
                        };
                        await db.InsertAsync(item);
                    }
                    else if (wallet.DIDUserId != user.DIDUserId)
                        //return InvokeResult.Fail<string>("4");//钱包地址错误!
                        return InvokeResult.Fail<string>("钱包地址错误!");

                }
            }
            if(string.IsNullOrEmpty(user.DIDUserId))
                //return InvokeResult.Fail<string>("5");//登录错误!
                return InvokeResult.Fail<string>("登录错误!");//登录错误!
            if(user.IsDisable == IsEnum.是)
                return InvokeResult.Fail<string>("用户已禁用!");//登录错误!

            //更新登录时间
            user.LoginDate = DateTime.Now;

            await db.UpdateAsync(user);

            //2.生成JWT
            //Header,选择签名算法
            var signingAlogorithm = SecurityAlgorithms.HmacSha256;
            //Payload,存放用户信息，下面我们放了一个用户id
            var claims = new[]
            {
                new Claim("UserId",user.DIDUserId)
            };
            //Signature
            //取出私钥并以utf8编码字节输出
            var secretByte = Encoding.UTF8.GetBytes(AppSettings.GetValue("Authentication","SecretKey"));
            //使用非对称算法对私钥进行加密
            var signingKey = new SymmetricSecurityKey(secretByte);
            //使用HmacSha256来验证加密后的私钥生成数字签名
            var signingCredentials = new SigningCredentials(signingKey, signingAlogorithm);
            //生成Token
            var Token = new JwtSecurityToken(
                    issuer: AppSettings.GetValue("Authentication","Issuer"),        //发布者
                    audience: AppSettings.GetValue("Authentication:Audience"),    //接收者
                    claims: claims,                                         //存放的用户信息
                    notBefore: DateTime.Now,                             //发布时间
                    expires: DateTime.Now.AddDays(30),                      //有效期设置为30天
                    signingCredentials                                      //数字签名 
                );
            //生成字符串token
            var TokenStr = new JwtSecurityTokenHandler().WriteToken(Token);

            return InvokeResult.Success<string>(TokenStr);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<Response> Register(LoginReq login)
        {
            using var db = new NDatabase();

            //var Uid = await db.SingleOrDefaultAsync<int>("select IDENT_CURRENT('DIDUser') + 1");//用户自增id
            var userId = await db.SingleOrDefaultAsync<string>("select DIDUserId from DIDUser where Mail = @0 and IsLogout = 0", login.Mail);
            var walletId = await db.SingleOrDefaultAsync<string>("select WalletId from Wallet where WalletAddress = @0 and Otype = @1 and Sign = @2 and IsLogout = 0 and IsDelete = 0",
                                                        login.WalletAddress, login.Otype, login.Sign);
            if (!string.IsNullOrEmpty(userId) || !string.IsNullOrEmpty(walletId))
                //return InvokeResult.Fail("3");//请勿重复注册!
                return InvokeResult.Fail("请勿重复注册!");//请勿重复注册!

            if (!string.IsNullOrEmpty(login.RefUserId))
            {
                var refUserId = await db.SingleOrDefaultAsync<string>("select DIDUserId from DIDUser where DIDUserId = @0 and IsLogout = 0", login.RefUserId);
                var communityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", login.RefUserId);
                if (string.IsNullOrEmpty(refUserId) || string.IsNullOrEmpty(communityId))//邀请码必须加入社区
                    //return InvokeResult.Fail("4"); //邀请码错误!
                     return InvokeResult.Fail("邀请码错误!"); //邀请码错误!
            }

            db.BeginTransaction();

            //var eotc = _reservice.GetRewardValue("UserAirdrop").Result.Items;//奖励eotc数量

            userId = Guid.NewGuid().ToString();
            var user = new DIDUser
            {
                DIDUserId = userId,
                PassWord = login.Password,
                AuthType = AuthTypeEnum.未审核,
                CreditScore = 0,
                Mail = login.Mail,
                RefUserId = login.RefUserId ?? login.RefUserId,
                UserNode = 0,//啥也不是
                RegDate = DateTime.Now,
                IsLogout = IsEnum.否
            };
            await db.InsertAsync(user);

            //加入推荐人社区
            if (!string.IsNullOrEmpty(login.RefUserId))
            {
                var communityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", login.RefUserId);
                if (!string.IsNullOrEmpty(communityId))
                {
                    var userCom = new UserCommunity()
                    {
                        UserCommunityId = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.Now,
                        DIDUserId = user.DIDUserId,
                        CommunityId = communityId
                    };
                    await db.InsertAsync(userCom);
                }
            }

            //uId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where Mail = @0", login.Mail);//获取主键
            if (!string.IsNullOrEmpty(login.WalletAddress) && !string.IsNullOrEmpty(login.Otype) && !string.IsNullOrEmpty(login.Sign))//有钱包时
            {
                var wallet = new Wallet
                {
                    WalletId = Guid.NewGuid().ToString(),
                    Otype = login.Otype,
                    Sign = login.Sign,
                    WalletAddress = login.WalletAddress,
                    DIDUserId = userId,
                    CreateDate = DateTime.Now,
                    IsLogout = IsEnum.否
                };
                await db.InsertAsync(wallet);
            }

            //添加现金支付
            var pay = new Payment() { 
                Type = PayType.现金支付,
                CreateDate = DateTime.Now,
                IsDelete = IsEnum.否,
                IsEnable = IsEnum.否,
                DIDUserId = userId,
                PaymentId = Guid.NewGuid().ToString()
            };
            await db.InsertAsync(pay);

            //发放邀请人空投
            //if (!string.IsNullOrEmpty(login.RefUserId))
            //{
            //    var refUser = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0 and IsLogout = 0", login.RefUserId);
            //    var refeotc = _reservice.GetRewardValue("RefAirdrop").Result.Items;//奖励eotc数量
            //    refUser.AirdropEotc += refeotc;
            //    await db.UpdateAsync(refUser);
            //}
            db.CompleteTransaction();

            //调用otc注册
            if (!string.IsNullOrEmpty(login.RefUserId))
            {
                var uid = await db.SingleOrDefaultAsync<string>("select UId from DIDUser where DIDUserId = @0 and IsLogout = 0", userId);
                var pid = await db.SingleOrDefaultAsync<string>("select UId from DIDUser where DIDUserId = @0 and IsLogout = 0", login.RefUserId);
                var code = CurrentUser.RegisterEotc(login.Mail!,
                                                    string.IsNullOrEmpty(login.WalletAddress) ? "''" : login.WalletAddress,
                                                    string.IsNullOrEmpty(login.Sign) ? "''" : login.Sign,
                                                    string.IsNullOrEmpty(login.Otype) ? "''" : login.Otype, uid, pid, login.Password!);
                if (code <= 0)
                    return InvokeResult.Fail("otc用户注册失败!");
            }
            

            return InvokeResult.Success("用户注册成功!");
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Response> GetCode(string mail,int type)
        {
            var sb = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < 6; i++)//6位验证码
            {
                sb.Append(random.Next(0, 9));
            }
            var code = sb.ToString();
            //var code = "123456";
            _cache.Set(mail, code, new TimeSpan(0, 10, 0));//十分钟过期

            //todo 发送邮件
            if (type == 0)//注册验证码
                EmailHelp.SendRegister(mail, code);
            else if (type == 1)
                EmailHelp.SendVerify(mail, code);
            return InvokeResult.Success("验证码发送成功!");
            //return InvokeResult.Success();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassWord"></param>
        /// <returns></returns>
        public async Task<Response> ChangePassword(string userId, string newPassWord)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            if (null == user)
                return InvokeResult.Fail("用户信息未找到!");
            if (!string.IsNullOrEmpty(user.RefUserId))
            {
                var code = CurrentUser.ChangePassword(user, newPassWord);
                if (code <= 0)
                    return InvokeResult.Fail("otc修改密码失败!");
            }
            await db.ExecuteAsync("update DIDUser set PassWord = @0 where DIDUserId = @1", newPassWord, userId);
            
            return InvokeResult.Success("修改成功!");
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="newPassWord"></param>
        /// <returns></returns>
        public async Task<Response> RetrievePassword(string mail, string newPassWord)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where Mail = @0", mail);
            if(null == user)
                return InvokeResult.Fail("用户信息未找到!");
            if (!string.IsNullOrEmpty(user.RefUserId))
            {
                var code = CurrentUser.ChangePassword(user, newPassWord);
                if (code <= 0)
                    return InvokeResult.Fail("otc修改密码失败!");
            }

            await db.ExecuteAsync("update DIDUser set PassWord = @0 where Mail = @1", newPassWord, mail);

            return InvokeResult.Success("修改成功!");
        }

        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<Response> ChangeMail(string userId, ChangeMailReq item)
        {
            using var db = new NDatabase();
            var wallet = await db.SingleOrDefaultAsync<Wallet>("select * from Wallet where WalletAddress = @0 and Otype = @1 and Sign = @2 and IsLogout = 0 and IsDelete = 0", item.WalletAddress, item.Otype, item.Sign);
            if(null == wallet)
                return InvokeResult.Fail("钱包验证错误!");//钱包验证错误!
            var user = await db.SingleOrDefaultAsync<string>("select DIDUserId from DIDUser where Mail = @0 and IsLogout = 0", item.Mail);
            if(!string.IsNullOrEmpty(user))
                return InvokeResult.Fail("邮箱已注册!");//邮箱已注册!
            await db.ExecuteAsync("update DIDUser set mail = @0 where DIDUserId = @1", item.Mail, userId);
            var user1 = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            var code = CurrentUser.ChangeMail(user1, item.Mail);
            if (code <= 0)
                return InvokeResult.Fail("otc修改邮箱失败!");
            return InvokeResult.Success("修改成功!");
        }

        //48小时后注销
        //private readonly System.Timers.Timer t = new(48 * 60 * 60 * 1000);//实例化Timer类，设置间隔时间为10000毫秒；
        //private readonly System.Timers.Timer t = new(5 * 60 * 1000);
        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task<Response> Logout(string userId, string reason)
        {
            //todo:判断注销条件
            using var db = new NDatabase();

            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            if (!string.IsNullOrEmpty(user.UserLogoutId))
            {
                var userLogout = await db.SingleOrDefaultByIdAsync<UserLogout>(user.UserLogoutId);
                if(userLogout.IsCancel == IsEnum.否)
                    return InvokeResult.Fail("请勿重复操作!");
            }

            db.BeginTransaction();

            var item = new UserLogout() { 
                DIDUserId = userId,
                UserLogoutId = Guid.NewGuid().ToString(),
                SubmitDate = DateTime.Now,
                IsCancel = IsEnum.否,
                Reason = reason
            };
            await db.InsertAsync(item);

            await db.ExecuteAsync("update DIDUser set UserLogoutId = @0 where DIDUserId = @1", item.UserLogoutId, userId);

            //注销时间默认48小时
            var hours = Convert.ToInt32(_reservice.GetRewardValue("LogoutHours").Result.Items);
            var timer = new Timers { 
                TimersId = Guid.NewGuid().ToString(), 
                Rid = item.UserLogoutId, 
                CreateDate = DateTime.Now, 
                StartTime = DateTime.Now, 
                EndTime = DateTime.Now.AddHours(hours) 
            };
            await db.InsertAsync(timer);
            TimersHelp.LogoutTimer(timer);

            //t.Elapsed += new System.Timers.ElapsedEventHandler(async (object? source, System.Timers.ElapsedEventArgs e) =>
            //{
            //    t.Stop(); //先关闭定时器

            //    var nowItem = await db.SingleOrDefaultByIdAsync<UserLogout>(item.UserLogoutId);
            //    if (nowItem.IsCancel == IsEnum.否)
            //    {
            //        nowItem.LogoutDate = DateTime.Now;
            //        await db.UpdateAsync(nowItem);
            //        await db.ExecuteAsync("update DIDUser set IsLogout = @0 where DIDUserId = @1", IsEnum.是, userId);//注销账号
            //        await db.ExecuteAsync("update Wallet set IsLogout = @0 where DIDUserId = @1", IsEnum.是, userId);//注销钱包
            //        var refUserId = await db.SingleOrDefaultAsync<string>("select RefUserId from DIDUser where DIDUserId = @0 and IsLogout = 0", userId);
            //        if (!string.IsNullOrEmpty(refUserId))
            //            await db.ExecuteAsync("update DIDUser set RefUserId = @0 where RefUserId = @1 and IsLogout = 0", refUserId, userId);//更新邀请人为当前用户的上级
            //    }
            //});//到达时间的时候执行事件；
            //t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            //t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            //t.Start(); //启动定时器

            //db.CompleteTransaction();
            return InvokeResult.Success("提交注销成功!");
        }

        /// <summary>
        /// 取消注销
        /// </summary>
        /// <returns></returns>
        public async Task<Response> CancelLogout(string userId)
        { 
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            db.BeginTransaction();
            if (!string.IsNullOrEmpty(user.UserLogoutId) && user.IsLogout == IsEnum.否)
            {
                var userLogout = await db.SingleOrDefaultByIdAsync<UserLogout>(user.UserLogoutId);
                userLogout.IsCancel = IsEnum.是;
                await db.UpdateAsync(userLogout);
                user.UserLogoutId = null;
                await db.UpdateAsync(user);
            }
            else
            {
                return InvokeResult.Fail("取消失败!");
            }
            db.CompleteTransaction();
            return InvokeResult.Success("取消成功!");
        }

        /// <summary>
        /// 获取提交注销时间
        /// </summary>
        /// <returns></returns>
        public async Task<Response<DateTime>> GetLogoutDate(string userId)
        {
            using var db = new NDatabase();
            var logoutDate = await db.SingleOrDefaultAsync<DateTime>("select b.SubmitDate from DIDUser a left join UserLogout b on a.UserLogoutId = b.UserLogoutId where a.DIDUserId = @0", userId);
            return InvokeResult.Success(logoutDate);
        }

        /// <summary>
        /// 获取团队信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isAuth"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public async Task<Response<TeamInfoRespon>> GetUserTeam(string userId,bool? isAuth, long page, long itemsPerPage)
        {
            using var db = new NDatabase();
            var model = new TeamInfoRespon();
            model.TeamNumber = await db.FirstOrDefaultAsync<int>(";with temp as \n" +
                        "(select DIDUserId from DIDUser where DIDUserId = @0 and IsLogout = 0\n" +
                        "union all \n" +
                        "select a.DIDUserId from DIDUser a inner join temp on a.RefUserId = temp.DIDUserId and a.IsLogout = 0) \n" +
                        "select Count(*) from temp where DIDUserId != @0", userId);

            model.PushNumber = await db.FirstOrDefaultAsync<int>("select Count(*) from DIDUser where RefUserId = @0 and IsLogout = 0", userId);

            //默认展示6级
            var list = await db.FetchAsync<DIDUser>(";with temp as \n" +
                                                    "(select *,0 Level from DIDUser where DIDUserId = @0 and IsLogout = 0\n" +
                                                    "union all \n" +
                                                    "select a.*,temp.Level+1 Level  from DIDUser a inner join temp on a.RefUserId = temp.DIDUserId WHERE temp.Level < 6 and a.IsLogout = 0) \n" +
                                                    "select * from temp where DIDUserId != @0 order by (select null)\n" +
                                                    "offset @1 rows fetch next @2 rows only", userId, (page - 1) * itemsPerPage, itemsPerPage);

            //todo:dao审核通过可以看所有数据

            //根据标签过滤数据
            if(null != isAuth && isAuth == true)
                list = list.Where(a => a.AuthType == AuthTypeEnum.审核成功).ToList();

            if (null != isAuth && isAuth == false)
                list = list.Where(a => a.AuthType != AuthTypeEnum.审核成功).ToList();

            var users = list.Select(a => new TeamUser()
                            {
                                UserNode = a.UserNode,//todo:获取用户等级  0 交易用户 1 信用节点 2 实时节点 3 中级节点 4 高级节点
                                UID = a.Uid,
                                Mail = a.Mail,
                                Phone = db.SingleOrDefault<string>("select b.PhoneNum from UserAuthInfo b left join DIDUser a  on a.UserAuthInfoId = b.UserAuthInfoId " +
                                        "where a.DIDUserId = @0 and a.AuthType = 2", a.DIDUserId),
                                RegDate = a.RegDate,
                                Name = CommonHelp.GetName(db.SingleOrDefault<string>("select b.Name from UserAuthInfo b left join DIDUser a  on a.UserAuthInfoId = b.UserAuthInfoId " +
                                        "where a.DIDUserId = @0 and a.AuthType = 2", a.DIDUserId))
                            }).ToList();
            model.Users = users;

            return InvokeResult.Success(model);
        }


        /// <summary>
        /// 提交团队申请
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response> TeamAuth(string userId)
        {
            using var db = new NDatabase();

            //随机Dao审核员审核
            var userIds = await db.FetchAsync<DIDUser>("select * from DIDUser where DIDUserId != @0 and IsExamine = 1 and IsLogout = 0 and IsEnable = 1", userId);
            if(userIds.Count == 0)
                return InvokeResult.Fail("没有审核员!");
            var random = new Random().Next(userIds.Count);
            var auditUserId = userIds[random].DIDUserId;
            

            var id = await db.SingleOrDefaultAsync<string>("select TeamAuthId from TeamAuth where AuditType = @0 and DIDUserId = @1", TeamAuditEnum.未审核, userId);
            if (!string.IsNullOrEmpty(id))
            {
                return InvokeResult.Fail("请勿重复提交!");
            }

            var item = new TeamAuth() { 
                TeamAuthId = Guid.NewGuid().ToString(),
                AuditType = TeamAuditEnum.未审核,
                CreateDate = DateTime.Now,
                DIDUserId = userId,
                AuditUserId = auditUserId
            };
            await db.InsertAsync(item);
            return InvokeResult.Success("提交成功!");
        }

        /// <summary>
        /// 获取用户质押数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<double>> GetUserEOTC(string userId)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            if (null == user)
                return InvokeResult.Fail<double>("用户信息未找到!");
            var eotc = CurrentUser.GetEUModel(user)?.StakeEotc ?? 0;
            return InvokeResult.Success(eotc);
        }

        /// <summary>
        /// 获取认证图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetAuthImage(string path, string userId)
        {
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            using var db = new NDatabase();

            var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);

            var bmp = WaterMarkHelp.AddWaterMark(path, user.Uid.ToString());
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
            }
            finally
            {
                //显式释放资源 
                bmp.Dispose();
            }
            return new FileContentResult(ms.ToArray(), "image/jpg");
        }

        /// <summary>
        /// 设置App支付密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="payPassWord"></param>
        /// <returns></returns>
        public async Task<Response> SetPayPassWord(string userId, string payPassWord)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            if (null == user)
                return InvokeResult.Fail<double>("用户信息未找到!");

            user.PayPassWord = payPassWord;
            await db.UpdateAsync(user);

            return InvokeResult.Success("设置成功!");
        }

        /// <summary>
        /// 修改邀请人
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="newUid"></param>
        /// <returns></returns>
        public async Task<Response> UpdatePid(int uId, int newUid)
        {
            using var db = new NDatabase();
            if(newUid >= uId)
                return InvokeResult.Fail<double>("邀请人错误!");

            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where Uid = @0", uId);
            var refUser = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where Uid = @0", newUid);
            if (null == user || null == refUser)
                return InvokeResult.Fail<double>("用户信息未找到!");
            if(user.RefUserId == refUser.DIDUserId)
                return InvokeResult.Success("修改成功!");

            db.BeginTransaction();
            user.RefUserId = refUser.DIDUserId;
            await db.UpdateAsync(user);

            //otc更新邀请人
            var code = CurrentUser.UpdatePid(user, newUid.ToString());
            if (code <= 0)
                return InvokeResult.Fail("otc更新邀请人失败!");
            //更新社区信息
            var userCom = await db.SingleOrDefaultAsync<UserCommunity>("select * from UserCommunity where DIDUserId = @0", user.DIDUserId);
            if (null != userCom)
            {
                var communityId = await db.SingleOrDefaultAsync<string>("select CommunityId from UserCommunity where DIDUserId = @0", refUser.DIDUserId);
                userCom.CommunityId = communityId;
                await db.UpdateAsync(userCom);
            }
            db.CompleteTransaction();

            return InvokeResult.Success("修改成功!");
        }


        /// <summary>
        /// 获取社区名称
        /// </summary>
        /// <returns></returns>
        public async Task<Response<GetInfoRespon>> GetInfo(int uId)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where Uid = @0", uId);
            if (null == user)
                return InvokeResult.Fail<GetInfoRespon>("用户信息未找到!");

            var comName = await db.SingleOrDefaultAsync<string>("select ComName from Community where CommunityId = (select CommunityId from UserCommunity where DIDUserId = @0)", user.DIDUserId);

            return InvokeResult.Success(new GetInfoRespon { ComName = comName });
        }

    }
}
