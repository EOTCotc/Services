using Dao.Entity;
using DID.Common;
using DID.Entitys;
using DID.Models;
using DID.Models.Base;
using DID.Models.Request;
using DID.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace DID.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserAuthService
    {
        /// <summary>
        /// 上传用户认证信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<Response> UploadUserInfo(UserAuthInfo info);
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response> UploadImage(IFormFile file, string userId);

        /// <summary>
        /// 获取未审核信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        Task<Response<List<UserAuthRespon>>> GetUnauditedInfo(string userId, IsEnum isDao, long page, long itemsPerPage);

        /// <summary>
        /// 获取已审核审核信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        Task<Response<List<UserAuthRespon>>> GetAuditedInfo(string userId, IsEnum isDao, long page, long itemsPerPage);

        /// <summary>
        /// 获取打回信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        Task<Response<List<UserAuthRespon>>> GetBackInfo(string userId, IsEnum isDao, long page, long itemsPerPage);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="userAuthInfoId"></param>
        /// <param name="userId"></param>
        /// <param name="auditType"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        Task<Response> AuditInfo(string userAuthInfoId, string userId, AuditTypeEnum auditType, string? remark, bool isDao = false);
        /// <summary>
        /// 获取用户审核成功信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<AuthSuccessRespon>> GetAuthSuccess(string userId);
        /// <summary>
        /// 获取用户审核失败信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<AuthFailRespon>> GetAuthFail(string userId);
    }
    /// <summary>
    /// 审核认证服务
    /// </summary>
    public class UserAuthService : IUserAuthService
    {
        private readonly ILogger<UserAuthService> _logger;

        private readonly ICreditScoreService _csservice;

        private readonly IRewardService _reservice;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public UserAuthService(ILogger<UserAuthService> logger, ICreditScoreService csservice, IRewardService reservice)
        {
            _logger = logger;
            _csservice = csservice;
            _reservice = reservice;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="userAuthInfoId"></param>
        /// <param name="userId"></param>
        /// <param name="auditType"></param>
        /// <param name="remark"></param>
        /// <param name="isDao"></param>
        /// <returns></returns>
        public async Task<Response> AuditInfo(string userAuthInfoId, string userId, AuditTypeEnum auditType, string? remark, bool isDao)
        {
            using var db = new NDatabase();
            var authinfo = await db.SingleByIdAsync<UserAuthInfo>(userAuthInfoId);
            var auth = await db.SingleOrDefaultAsync<Auth>("select * from Auth where UserAuthInfoId = @0 and AuditUserId = @1", userAuthInfoId, userId);

            //不会出现重复的记录 每个用户只审核一次
            if (auth.AuditType != AuditTypeEnum.未审核)
                return InvokeResult.Fail("已审核!");

            auth.AuditType = auditType;
            auth.Remark = remark;
            auth.AuditDate = DateTime.Now;

            db.BeginTransaction();
            await db.UpdateAsync(auth);

            //修改用户审核状态
            if (auth.AuditStep == AuditStepEnum.抽审 && auth.AuditType == AuditTypeEnum.审核通过)
            {
                await db.ExecuteAsync("update DIDUser set AuthType = @1 where DIDUserId = @0", authinfo.CreatorId, AuthTypeEnum.审核成功);
                //加信用分 用户+8 邀请人+1 节点+1
                var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", authinfo.CreatorId);
                _csservice.CreditScore(new CreditScoreReq { Fraction = 8, Remarks ="完成强关系链认证", Type= TypeEnum.加分, Uid =  user.Uid});

                var refuser = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", user.RefUserId);
                _csservice.CreditScore(new CreditScoreReq { Fraction = 1, Remarks = "强关系链认证（审核）", Type = TypeEnum.加分, Uid = refuser.Uid });

                //审核节点 +1
                var auths = await db.FetchAsync<Auth>("select * from Auth where UserAuthInfoId = @0 and IsDelete = 0 order by AuditStep", userAuthInfoId);

                foreach (var item in auths)
                {
                    if (item.IsDao == IsEnum.否 && item.AuditStep == AuditStepEnum.二审)
                    {
                        var authuser = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", item.AuditUserId);
                        _csservice.CreditScore(new CreditScoreReq { Fraction = 1, Remarks = "强关系链认证（审核）", Type = TypeEnum.加分, Uid = authuser.Uid });
                    }
                    if (item.IsDao == IsEnum.否 && item.AuditStep == AuditStepEnum.抽审)
                    {
                        var authuser = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", item.AuditUserId);
                        _csservice.CreditScore(new CreditScoreReq { Fraction = 1, Remarks = "强关系链认证（审核）", Type = TypeEnum.加分, Uid = authuser.Uid });
                    }
                }
            }
            else if (auth.AuditType != AuditTypeEnum.审核通过)
            {
                //修改用户审核状态
                await db.ExecuteAsync("update DIDUser set AuthType = @1 where DIDUserId = @0", authinfo.CreatorId, AuthTypeEnum.审核失败);
                //todo: 审核失败 扣分  
                if (auth.AuditType == AuditTypeEnum.恶意提交)
                {
                    var auths = await db.FetchAsync<Auth>("select * from Auth where UserAuthInfoId = @0 and IsDelete = 0 order by AuditStep", userAuthInfoId);

                    for (var i = 0; i < auths.Count - 1; i++)
                    {
                        var authuser = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", auths[i].AuditUserId);
                        _csservice.CreditScore(new CreditScoreReq { Fraction = 3, Remarks = "强关系链认证(恶意提交)", Type = TypeEnum.减分, Uid = authuser.Uid });
                    }

                    //恶意提交3次 禁用账号
                    var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", authinfo.CreatorId);
                    user.AuthFail += 1;
                    if (user.AuthFail >= 3)
                        user.IsDisable = IsEnum.是;
                    await db.UpdateAsync(user);
                }
            }

            //下一步审核
            if (auth.AuditStep == AuditStepEnum.初审 && auth.AuditType == AuditTypeEnum.审核通过)
            {
                //上级节点审核
                var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", authinfo.CreatorId);
                var authUserIds = await db.FetchAsync<string>("select AuditUserId from Auth where UserAuthInfoId = @0 and IsDelete = 0 order by AuditStep", userAuthInfoId);
                authUserIds.Add(authinfo.CreatorId!);
                //var auths = await db.FetchAsync<DIDUser>("select * from DIDUser where UserNode = @0 and DIDUserId not in (@1) and IsLogout = 0 ", user.UserNode == UserNodeEnum.高级节点 ? UserNodeEnum.高级节点 : ++user.UserNode, authUserIds);
                //var random = new Random().Next(auths.Count);
                var authUserId = await db.SingleOrDefaultAsync<string>(
                                                                        ";WITH temp AS (\n" +
                                                        "	SELECT\n" +
                                                        "		DIDUserId,RefUserId, UserNode\n" +
                                                        "	FROM\n" +
                                                        "		DIDUser \n" +
                                                        "	WHERE\n" +
                                                        "		DIDUserId = @0\n" +
                                                        "		AND IsLogout = 0 UNION ALL\n" +
                                                        "	SELECT\n" +
                                                        "		a.DIDUserId,a.RefUserId , a.UserNode\n" +
                                                        "	FROM\n" +
                                                        "		DIDUser a\n" +
                                                        "		INNER JOIN temp ON a.DIDUserId = temp.RefUserId\n" +
                                                        "		AND a.IsLogout = 0 \n" +
                                                        "	) SELECT top 1 * FROM temp where UserNode > 1 and DIDUserId not in (@1)\n" +
                                                        "	", authinfo.CreatorId, authUserIds);


                if (string.IsNullOrEmpty(authUserId))
                    return InvokeResult.Success("审核失败,未找到上级节点!");

                var nextAuth = new Auth()
                {
                    AuthId = Guid.NewGuid().ToString(),
                    UserAuthInfoId = userAuthInfoId,
                    AuditUserId = authUserId,                                                                             
                    CreateDate = DateTime.Now,
                    AuditType = AuditTypeEnum.未审核,
                    AuditStep = AuditStepEnum.二审
                };
                //人像照处理
                var img = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, authinfo.PortraitImage));
                img = CommonHelp.WhiteGraphics(img, new Rectangle((int)(img.Width * 0.6), 0, (int)(img.Width * 0.4), img.Height));//遮住右边40%
                nextAuth.PortraitImage = "Auth/AuthImges/" + authinfo.CreatorId + "/" + Guid.NewGuid().ToString() + ".jpg";
                img.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nextAuth.PortraitImage));
                //国徽面处理
                var img1 = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, authinfo.NationalImage));
                img1 = CommonHelp.WhiteGraphics(img1, new Rectangle((int)(img1.Width * 0.6), 0, (int)(img1.Width * 0.4), img1.Height));//遮住右边40%
                nextAuth.NationalImage = "Auth/AuthImges/" + authinfo.CreatorId + "/" + Guid.NewGuid().ToString() + ".jpg";
                img1.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nextAuth.NationalImage));
                
                await db.InsertAsync(nextAuth);

                //去Dao审核
                ToDaoAuth(nextAuth, authinfo.CreatorId!);
            }
            else if (auth.AuditStep == AuditStepEnum.二审 && auth.AuditType == AuditTypeEnum.审核通过)
            {
                //中高级节点审核
                var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", authinfo.CreatorId);
                var authUserIds = await db.FetchAsync<string>("select AuditUserId from Auth where UserAuthInfoId = @0 and IsDelete = 0 order by AuditStep", userAuthInfoId);
                authUserIds.Add(authinfo.CreatorId!);
                var auths = await db.FetchAsync<DIDUser>("select * from DIDUser where (UserNode = 4 or UserNode = 5) and IsLogout = 0 and DIDUserId not in (@0)", authUserIds);
                var random = new Random().Next(auths.Count);
                var authUserId = auths[random].DIDUserId;
                if (string.IsNullOrEmpty(authUserId))
                    return InvokeResult.Success("审核失败,未找到中高级节点!");

                var nextAuth = new Auth()
                {
                    AuthId = Guid.NewGuid().ToString(),
                    UserAuthInfoId = userAuthInfoId,
                    AuditUserId = authUserId,//推荐人审核                                                                              
                    CreateDate = DateTime.Now,
                    AuditType = AuditTypeEnum.未审核,
                    AuditStep = AuditStepEnum.抽审
                };
                //人像照处理
                var img = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, authinfo.PortraitImage));
                img = CommonHelp.MaSaiKeGraphics(img, 8);//随机30%马赛克
                nextAuth.PortraitImage = "Auth/AuthImges/" + authinfo.CreatorId + "/" + Guid.NewGuid().ToString() + ".jpg";
                img.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nextAuth.PortraitImage));
                //国徽面处理
                var img1 = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, authinfo.NationalImage));
                img1 = CommonHelp.MaSaiKeGraphics(img1, 8);//随机30%马赛克
                nextAuth.NationalImage = "Auth/AuthImges/" + authinfo.CreatorId + "/" + Guid.NewGuid().ToString() + ".jpg";
                img1.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nextAuth.NationalImage));

                await db.InsertAsync(nextAuth);

                //去Dao审核
                //ToDaoAuth(nextAuth.AuthId);
            }
            db.CompleteTransaction();

            //奖励EOTC 10
            if (isDao)
            {
                var eotc = _reservice.GetRewardValue("IdentityAudit").Result.Items;//奖励eotc数量
                var detail = new IncomeDetails()
                {
                    IncomeDetailsId = Guid.NewGuid().ToString(),
                    CreateDate = DateTime.Now,
                    EOTC = eotc,
                    Remarks = "处理身份认证审核",
                    Type = IDTypeEnum.处理审核,
                    DIDUserId = userId
                };
                var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);

                db.BeginTransaction();
                db.Insert(detail);
                user.DaoEOTC += eotc;
                db.Update(user);
                var userExamine = db.SingleOrDefault<UserExamine>("select * from UserExamine where DIDUserId = @0 and IsDelete = 0", userId);
                userExamine.EOTC += eotc;
                userExamine.ExamineNum += 1;
                db.Update(userExamine);
                db.CompleteTransaction();
            }

            return InvokeResult.Success("审核成功!");
        }

        /// <summary>
        /// 获取已审核审核信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        public async Task<Response<List<UserAuthRespon>>> GetAuditedInfo(string userId, IsEnum isDao,long page, long itemsPerPage)
        {
            var result = new List<UserAuthRespon>();
            using var db = new NDatabase();
            //var items = await db.FetchAsync<Auth>("select * from Auth where AuditUserId = @0 and AuditType != 0", userId);
            var items = (await db.PageAsync<Auth>(page, itemsPerPage, "select * from Auth where AuditUserId = @0 and AuditType != 0 and IsDelete = 0 and IsDao = @1 order by CreateDate Desc", userId, isDao)).Items;
            foreach (var item in items)
            {
                var authinfo = await db.SingleOrDefaultAsync<UserAuthRespon>("select * from UserAuthInfo where UserAuthInfoId = @0",item.UserAuthInfoId);
                authinfo.PortraitImage = item.PortraitImage;
                authinfo.NationalImage = item.NationalImage;
                authinfo.HandHeldImage = item.HandHeldImage;
                //基本信息处理
                if (item.AuditStep == AuditStepEnum.初审)
                {
                    if (authinfo.PhoneNum.Length > 4)
                        authinfo.PhoneNum = authinfo.PhoneNum.Remove(0, 4).Insert(0, "****");
                    if (authinfo.IdCard.Length > 4)
                        authinfo.IdCard = authinfo.IdCard.Remove(0, 4).Insert(0, "****");
                }
                else if (item.AuditStep == AuditStepEnum.二审)
                {
                    if (authinfo.PhoneNum.Length > 4)
                        authinfo.PhoneNum = authinfo.PhoneNum.Remove(authinfo.PhoneNum.Length - 4, 4).Insert(authinfo.PhoneNum.Length - 4, "****");
                    if (authinfo.IdCard.Length > 4)
                        authinfo.IdCard = authinfo.IdCard.Remove(authinfo.IdCard.Length - 4, 4).Insert(authinfo.IdCard.Length - 4, "****");
                }
                else if (item.AuditStep == AuditStepEnum.抽审)
                {
                    if (authinfo.PhoneNum.Length > 7)
                        authinfo.PhoneNum = authinfo.PhoneNum.Remove(3, 4).Insert(3, "****");
                    if (authinfo.IdCard.Length > 7)
                        authinfo.IdCard = authinfo.IdCard.Remove(authinfo.IdCard.Length - 4, 4).Insert(authinfo.IdCard.Length - 4, "****");
                }
                var auths = await db.FetchAsync<Auth>("select * from Auth where UserAuthInfoId = @0 and IsDelete = 0 order by AuditStep", item.UserAuthInfoId);
                var list = new List<AuthInfo>();
                foreach (var auth in auths)
                {
                    list.Add(new AuthInfo()
                    {
                        UId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", auth.AuditUserId),
                        AuditStep = auth.AuditStep,
                        AuthDate = auth.AuditDate,
                        Name = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", auth.AuditUserId),
                        AuditType = auth.AuditType,
                        Remark = auth.Remark,
                        IsDao = auth.IsDao
                    }) ;
                }
                authinfo.Auths = list;
                result.Add(authinfo);
            }
            return InvokeResult.Success(result);
        }

        /// <summary>
        /// 获取未审核信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        public async Task<Response<List<UserAuthRespon>>> GetUnauditedInfo(string userId, IsEnum isDao,long page, long itemsPerPage)
        {
            var result = new List<UserAuthRespon>();
            using var db = new NDatabase();
            //var items = await db.FetchAsync<Auth>("select * from Auth where AuditUserId = @0 and AuditType = 0", userId);
            var items = (await db.PageAsync<Auth>(page, itemsPerPage, "select * from Auth where AuditUserId = @0 and AuditType = 0 and IsDelete = 0 and IsDao = @1", userId, isDao)).Items;
            foreach (var item in items)
            {
                var authinfo = await db.SingleOrDefaultAsync<UserAuthRespon>("select * from UserAuthInfo where UserAuthInfoId = @0", item.UserAuthInfoId);
                authinfo.PortraitImage = item.PortraitImage;
                authinfo.NationalImage = item.NationalImage;
                authinfo.HandHeldImage = item.HandHeldImage;
                //基本信息处理
                if (item.AuditStep == AuditStepEnum.初审)
                {
                    if (authinfo.PhoneNum.Length > 4)
                        authinfo.PhoneNum = authinfo.PhoneNum.Remove(0, 4).Insert(0, "****");
                    if (authinfo.IdCard.Length > 4)
                        authinfo.IdCard = authinfo.IdCard.Remove(0, 4).Insert(0, "****");
                }
                else if (item.AuditStep == AuditStepEnum.二审)
                {
                    if (authinfo.PhoneNum.Length > 4)
                        authinfo.PhoneNum = authinfo.PhoneNum.Remove(authinfo.PhoneNum.Length - 4, 4).Insert(authinfo.PhoneNum.Length - 4, "****");
                    if (authinfo.IdCard.Length > 4)
                        authinfo.IdCard = authinfo.IdCard.Remove(authinfo.IdCard.Length - 4, 4).Insert(authinfo.IdCard.Length - 4, "****");
                }
                else if (item.AuditStep == AuditStepEnum.抽审)
                {
                    if (authinfo.PhoneNum.Length > 7)
                        authinfo.PhoneNum = authinfo.PhoneNum.Remove(3, 4).Insert(3, "****");
                    if (authinfo.IdCard.Length > 7)
                        authinfo.IdCard = authinfo.IdCard.Remove(authinfo.IdCard.Length - 4, 4).Insert(authinfo.IdCard.Length - 4, "****");
                }
                var auths = await db.FetchAsync<Auth>("select * from Auth where UserAuthInfoId = @0 and IsDelete = 0 order by AuditStep", item.UserAuthInfoId);
                var list = new List<AuthInfo>();
                foreach (var auth in auths)
                {
                    list.Add(new AuthInfo()
                    {
                        UId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", auth.AuditUserId),
                        AuditStep = auth.AuditStep,
                        AuthDate = auth.AuditDate,
                        Name = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", auth.AuditUserId),
                        AuditType = auth.AuditType,
                        Remark = auth.Remark,
                        IsDao = auth.IsDao
                    });

                }
                authinfo.Auths = list;
                result.Add(authinfo);
            }
            return InvokeResult.Success(result);
        }

        /// <summary>
        /// 获取打回信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDao"></param>
        /// <param name="page">页数</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <returns></returns>
        public async Task<Response<List<UserAuthRespon>>> GetBackInfo(string userId, IsEnum isDao,long page, long itemsPerPage)
        {
            var result = new List<UserAuthRespon>();
            using var db = new NDatabase();
            //var items = await db.FetchAsync<Auth>("select * from Auth where AuditUserId = @0", userId);
            var items = (await db.PageAsync<Auth>(page, itemsPerPage, "select * from Auth where AuditUserId = @0 and IsDelete = 0 and IsDao = @1", userId, isDao)).Items;
            foreach (var item in items)
            {
                var authinfo = await db.SingleOrDefaultAsync<UserAuthRespon>("select * from UserAuthInfo where UserAuthInfoId = @0", item.UserAuthInfoId);
                authinfo.PortraitImage = item.PortraitImage;
                authinfo.NationalImage = item.NationalImage;
                authinfo.HandHeldImage = item.HandHeldImage;
                //基本信息处理
                if (item.AuditStep == AuditStepEnum.初审)
                {
                    if (authinfo.PhoneNum.Length > 4)
                        authinfo.PhoneNum = authinfo.PhoneNum.Remove(0, 4).Insert(0, "****");
                    if (authinfo.IdCard.Length > 4)
                        authinfo.IdCard = authinfo.IdCard.Remove(0, 4).Insert(0, "****");
                }
                else if (item.AuditStep == AuditStepEnum.二审)
                {
                    if (authinfo.PhoneNum.Length > 4)
                        authinfo.PhoneNum = authinfo.PhoneNum.Remove(authinfo.PhoneNum.Length - 4, 4).Insert(authinfo.PhoneNum.Length - 4, "****");
                    if (authinfo.IdCard.Length > 4)
                        authinfo.IdCard = authinfo.IdCard.Remove(authinfo.IdCard.Length - 4, 4).Insert(authinfo.IdCard.Length - 4, "****");
                }
                else if (item.AuditStep == AuditStepEnum.抽审)
                {
                    if (authinfo.PhoneNum.Length > 7)
                        authinfo.PhoneNum = authinfo.PhoneNum.Remove(3, 4).Insert(3, "****");
                    if (authinfo.IdCard.Length > 7)
                        authinfo.IdCard = authinfo.IdCard.Remove(authinfo.IdCard.Length - 4, 4).Insert(authinfo.IdCard.Length - 4, "****");
                }
                var auths = await db.FetchAsync<Auth>("select * from Auth where UserAuthInfoId = @0 and IsDelete = 0 order by AuditStep", item.UserAuthInfoId);
                var list = new List<AuthInfo>();
                foreach (var auth in auths)
                {
                    list.Add(new AuthInfo()
                    {
                        UId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", auth.AuditUserId),
                        AuditStep = auth.AuditStep,
                        AuthDate = auth.AuditDate,
                        Name = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", auth.AuditUserId),
                        AuditType = auth.AuditType,
                        Remark = auth.Remark,
                        IsDao = auth.IsDao
                    });
                }
                authinfo.Auths = list;
                var next = auths.Where(a => a.AuditStep == item.AuditStep + 1).ToList();
                if (next.Count > 0 && (next[0].AuditType != AuditTypeEnum.未审核 && next[0].AuditType != AuditTypeEnum.审核通过))
                    result.Add(authinfo);
            }
            return InvokeResult.Success(result);
        }

        /// <summary>
        /// 获取审核详细信息
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        //public async Task<Response<List<UserAuthInfo>>> GetInfo(List<Auth> lists)
        //{
        //    using var db = new NDatabase();
        //    foreach (var item in lists)
        //    {
        //        var items = await db.FetchAsync<UserAuthInfo>("select * from UserAuthInfo where AuditUid = @0 and AuditType != 0 and AuditType != 1", userId);
        //    }
        //    return InvokeResult.Success(items);
        //}

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response> UploadImage(IFormFile file, string userId)
        {
            try
            {
                var dir = new DirectoryInfo(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "Auth/AuthImges/" + userId + "/"));
                
                //保存目录不存在就创建这个目录
                if (!dir.Exists)
                {
                    Directory.CreateDirectory(dir.FullName);
                }
                //var filename = upload.UserId + "_" + upload.Type + ".jpg";
                var filename = Guid.NewGuid().ToString() + ".jpg";
                using (var stream = new FileStream(dir.FullName + filename, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    //file.CopyTo(stream);
                }
                //return InvokeResult.Success("Images/AuthImges/" + upload.UId + "/" + filename);
                return InvokeResult.Success(filename);
            }
            catch (Exception e)
            {
                _logger.LogError("UploadImage", e);
                return InvokeResult.Fail("Fail");
            }
        }

        /// <summary>
        /// 上传用户认证信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response> UploadUserInfo(UserAuthInfo info)
        {
            info.UserAuthInfoId = Guid.NewGuid().ToString();
            info.CreateDate = DateTime.Now;
            //info.PortraitImage = "Images/AuthImges/" + info.CreatorId + "/" + info.PortraitImage;
            //info.NationalImage = "Images/AuthImges/" + info.CreatorId + "/" + info.NationalImage;
            //info.HandHeldImage = "Images/AuthImges/" + info.CreatorId + "/" + info.HandHeldImage;

            using var db = new NDatabase();
            //var list = await db.SingleOrDefaultAsync<string>("select UserAuthInfoId from DIDUSer where Uid =@0", info.CreatorId);
            //if(!string.IsNullOrEmpty(list))
            //    return InvokeResult.Fail("请勿重复提交!");
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", info.CreatorId);
            if(null == user)
                return InvokeResult.Fail("用户信息未找到!");//请勿重复提交!
            if (user.AuthType != AuthTypeEnum.未审核 && user.AuthType != AuthTypeEnum.审核失败)
                return InvokeResult.Fail("请勿重复提交!");//请勿重复提交!
            if(user.IsDisable == IsEnum.是)
                return InvokeResult.Fail("用户已被禁用!");//请勿重复提交!
            if(string.IsNullOrEmpty(user.RefUserId))
                return InvokeResult.Fail("请先添加推荐人!");//请勿重复提交!

            var authUserId = await db.SingleOrDefaultAsync<string>("select DIDUserId from DIDUser where DIDUserId = @0 and AuthType = 2", user.RefUserId);//审核人
            if (string.IsNullOrEmpty(authUserId))
            {
                authUserId = await db.SingleOrDefaultAsync<string>(
                                                                        ";WITH temp AS (\n" +
                                                        "	SELECT\n" +
                                                        "		DIDUserId,RefUserId, UserNode\n" +
                                                        "	FROM\n" +
                                                        "		DIDUser \n" +
                                                        "	WHERE\n" +
                                                        "		DIDUserId = @0\n" +
                                                        "		AND IsLogout = 0 UNION ALL\n" +
                                                        "	SELECT\n" +
                                                        "		a.DIDUserId,a.RefUserId , a.UserNode\n" +
                                                        "	FROM\n" +
                                                        "		DIDUser a\n" +
                                                        "		INNER JOIN temp ON a.DIDUserId = temp.RefUserId\n" +
                                                        "		AND a.IsLogout = 0 \n" +
                                                        "	) SELECT top 1 * FROM temp where UserNode > 1 \n" +
                                                        "	", info.CreatorId);
            }
            if(string.IsNullOrEmpty(authUserId))
                return InvokeResult.Fail("未找到审核人!");//请勿重复提交!

            var auth = new Auth
            {
                AuthId = Guid.NewGuid().ToString(),
                UserAuthInfoId = info.UserAuthInfoId,
                AuditUserId = authUserId,//推荐人审核
                //HandHeldImage = "Images/AuthImges/" + info.CreatorId + "/" + info.HandHeldImage,
                CreateDate = DateTime.Now,
                AuditType = AuditTypeEnum.未审核,
                AuditStep = AuditStepEnum.初审
            };
            //人像照处理
            var img = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, info.PortraitImage));
            img = CommonHelp.WhiteGraphics(img, new Rectangle(0, 0, (int)(img.Width * 0.4), img.Height));//遮住左边40%
            auth.PortraitImage = "Auth/AuthImges/" + info.CreatorId + "/" + Guid.NewGuid().ToString() + ".jpg";
            img.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, auth.PortraitImage));
            //国徽面处理
            var img1 = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, info.NationalImage));
            img1 = CommonHelp.WhiteGraphics(img1, new Rectangle(0, 0, (int)(img1.Width * 0.4), img1.Height));//遮住左边40%
            auth.NationalImage = "Auth/AuthImges/" + info.CreatorId + "/" + Guid.NewGuid().ToString() + ".jpg";
            img1.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, auth.NationalImage));

            db.BeginTransaction();
            await db.InsertAsync(info);
            await db.InsertAsync(auth);
            await db.ExecuteAsync("update DIDUser set UserAuthInfoId = @0,AuthType = @2 where DIDUserId = @1", info.UserAuthInfoId, info.CreatorId, AuthTypeEnum.审核中);//更新用户当前认证编号 审核中
            db.CompleteTransaction();

            //去Dao审核
            ToDaoAuth(auth, info.CreatorId!);

            return InvokeResult.Success("提交成功!");
        }

        /// <summary>
        /// 获取用户审核成功信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<AuthSuccessRespon>> GetAuthSuccess(string userId)
        {
            using var db = new NDatabase();
            var item = new AuthSuccessRespon();
            var authinfo = await db.SingleOrDefaultAsync<UserAuthInfo>("select b.* from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0 and a.AuthType = 2", userId);
            if (authinfo == null) return InvokeResult.Fail<AuthSuccessRespon>("1");//认证信息未找到!
            item.Name = authinfo!.Name;
            item.PhoneNum = authinfo.PhoneNum;
            item.IdCard = authinfo.IdCard;
            item.RefUid = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId" +
                " where a.DIDUserId = (select RefUserId from DIDUser where DIDUserId = @0)", userId);
            item.Mail = await db.SingleOrDefaultAsync<string>("select Mail from DIDUser where DIDUserId =@0", userId);
            var auths = await db.FetchAsync<Auth>("select * from Auth where UserAuthInfoId = @0 and IsDelete = 0 order by AuditStep", authinfo.UserAuthInfoId);
            var list = new List<AuthInfo>();
            foreach (var auth in auths)
            {
                list.Add(new AuthInfo()
                {
                    UId = await db.SingleOrDefaultAsync<int>("select Uid from DIDUser where DIDUserId = @0", auth.AuditUserId),
                    AuditStep = auth.AuditStep,
                    AuthDate = auth.AuditDate,
                    Name = await db.SingleOrDefaultAsync<string>("select b.Name from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", auth.AuditUserId),
                    AuditType = auth.AuditType,
                    Remark = auth.Remark
                });
            }
            item.Auths = list;
            return InvokeResult.Success(item);
        }

        /// <summary>
        /// 获取用户审核失败信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<AuthFailRespon>> GetAuthFail(string userId)
        {
            using var db = new NDatabase();
            var item = new AuthFailRespon();
            var authinfo = await db.SingleOrDefaultAsync<UserAuthInfo>("select b.* from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0", userId);
            if (authinfo == null) return InvokeResult.Fail<AuthFailRespon>("认证信息未找到");//认证信息未找到!
            item.Name = authinfo!.Name;
            item.PhoneNum = authinfo.PhoneNum;
            item.IdCard = authinfo.IdCard;
            item.NationalImage = authinfo.NationalImage;
            item.PortraitImage = authinfo.PortraitImage;
            item.HandHeldImage = authinfo.HandHeldImage;
            var auths = await db.FetchAsync<Auth>("select * from Auth where UserAuthInfoId = @0 and IsDelete = 0 order by AuditStep Desc", authinfo.UserAuthInfoId);
            //if (auths.Count == 0) return InvokeResult.Fail<AuthFailRespon>("认证信息未找到");//认证信息未找到!
            if(auths.Count > 0)
            {
                item.Remark = auths[0].Remark;
                item.AuditType = auths[0].AuditType;
            }
            return InvokeResult.Success(item);
        }

        /// <summary>
        /// 两小时未审核去Dao审核
        /// </summary>
        /// <param name="item">审核记录</param>
        /// <param name="userId">创建用户</param>
        public void ToDaoAuth(Auth item,string userId)
        {
            //两小时没人审核 自动到Dao审核
            //var t = new System.Timers.Timer(5 * 60 * 1000);//实例化Timer类，设置间隔时间为10000毫秒；
            var t = new System.Timers.Timer(2 * 60 * 60 * 1000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(async (object? source, System.Timers.ElapsedEventArgs e) =>
            {
                
                t.Stop(); //先关闭定时器
                          //todo: Dao审核
                using var db = new NDatabase();
                //var item = await db.SingleOrDefaultByIdAsync<Auth>(authId);

                if (item.AuditType == AuditTypeEnum.未审核)
                {
                    item.IsDelete = IsEnum.是;
                    await db.UpdateAsync(item);

                    //随机Dao审核员审核
                    var userIds = await db.FetchAsync<DIDUser>("select * from DIDUser where DIDUserId != @0 and IsExamine = 1 and IsLogout = 0 and IsEnable = 1", userId);
                    if (userIds.Count == 0)
                        _logger.LogError("身份认证审核(没有找到审核员)");
                    var random = new Random().Next(userIds.Count);
                    var auditUserId =  userIds[random].DIDUserId;

                    item.AuthId = Guid.NewGuid().ToString();
                    item.IsDao = IsEnum.是;
                    item.AuditUserId = auditUserId;//Dao在线节点用户编号
                    item.CreateDate = DateTime.Now;
                    item.IsDelete = IsEnum.否;
                    await db.InsertAsync(item);
                }
            });//到达时间的时候执行事件；
            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            t.Start(); //启动定时器

        }
    }
}
