using Dao.Common;
using Dao.Entity;
using Dao.Models.Base;
using Dao.Models.Request;
using Dao.Models.Response;
using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using Microsoft.Extensions.Logging;
using NPoco;

namespace Dao.Services
{
    /// <summary>
    /// 风控服务接口
    /// </summary>
    public interface IRiskService
    {
        /// <summary>
        /// 设置用户风险等级
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response> UserRiskLevel(UserRiskLevelReq req);

        /// <summary>
        /// 获取风控列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response<List<UserRiskRespon>>> UserRisk(DaoBaseReq req);

        /// <summary>
        /// 修改用户风险状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response> UserRiskStatus(UserRiskStatusReq req);

        /// <summary>
        /// 解除风险
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response> RemoveRisk(RemoveRiskReq req);

        /// <summary>
        /// 获取风险用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Response<RiskUserInfo>> GetUserInfo(RemoveRiskReq req);

        /// <summary>
        /// 获取用户风险等级
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<RiskLevelEnum>> GetUserRiskLevel(string userId);

        /// <summary>
        /// 获取解除风控联系人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<List<GetRiskList>>> GetRiskList(string userId);
    }

    /// <summary>
    /// 风控服务
    /// </summary>
    public class RiskService : IRiskService
    {
        private readonly ILogger<RiskService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public RiskService(ILogger<RiskService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 设置用户风险等级
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response> UserRiskLevel(UserRiskLevelReq req)
        {
            using var db = new NDatabase();
            var userId = WalletHelp.GetUserId(req);
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);

            if (user.RiskLevel == RiskLevelEnum.高风险)
                return InvokeResult.Fail("请勿重复设置!");
            user.RiskLevel = req.Level;

            await db.UpdateAsync(user);
            if (req.Level == RiskLevelEnum.高风险)
            {
                //生成审核信息（5个人3个通过解除） 可配置
                var list = new List<string>();

                var userIds = await db.FetchAsync<DIDUser>("select * from DIDUser where DIDUserId != @0 and IsExamine = 1 and IsLogout = 0", userId);
                for (var i = 0; i < 5; i++)
                {
                    var random = 0;
                    do
                    {
                        random = new Random().Next(userIds.Count);
                    } while (list.Exists(a => a == userIds[random].DIDUserId));
                    list.Add(userIds[random].DIDUserId);
                }
                //list.Add("e8771b3c-3b05-4830-900d-df2be0a6e9f7");
                //list.Add("d389e5db-37d0-40cd-9d8b-0d31a0ef2c12");
                //list.Add("61d14a4f-c45f-4b13-a957-5bcaff9b3324");
                //list.Add("7e88d292-7454-4e26-821a-b4e6049a7a95");
                //list.Add("2a5bf1dd-e15b-40f4-94bb-b68cee2bbaf9");

                var risks = new List<UserRisk>();

                list.ForEach(x => risks.Add(
                    new UserRisk
                    {
                        UserRiskId = Guid.NewGuid().ToString(),
                        AuditUserId = x,
                        DIDUserId = userId,
                        AuthStatus = RiskStatusEnum.未核对,
                        IsRemoveRisk = IsEnum.否,
                        Reason = req.Reason,
                        CreateDate = DateTime.Now,
                        IsDelete = IsEnum.否
                    })
                );

                await db.InsertBatchAsync(risks);
            }


            return InvokeResult.Success("设置成功!");
        }

        /// <summary>
        /// 获取解除风控联系人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<List<GetRiskList>>> GetRiskList(string userId)
        {
            using var db = new NDatabase();
            var items = await db.FetchAsync<UserRisk>("select * from UserRisk where DIDUserId = @0 and IsDelete = 0", userId);

            var risks = new List<GetRiskList>();
            items.ForEach(x => risks.Add(
                new GetRiskList
                {
                    Name = WalletHelp.GetName(x.AuditUserId),
                    Phone = WalletHelp.GetPhone(x.AuditUserId),
                    Status = x.IsRemoveRisk
                })
            );

            return InvokeResult.Success(risks);
        }

        /// <summary>
        /// 获取用户风险等级
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<RiskLevelEnum>> GetUserRiskLevel(string userId)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);

            return InvokeResult.Success(user.RiskLevel);
        }

        /// <summary>
        /// 获取风控列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response<List<UserRiskRespon>>> UserRisk(DaoBaseReq req)
        {
            using var db = new NDatabase();
            var userId = WalletHelp.GetUserId(req);

            var models = await db.FetchAsync<UserRisk>("select * from UserRisk where AuditUserId = @0 and IsRemoveRisk = 0", userId);

            var list = new List<UserRiskRespon>();

            models.ForEach(a => list.Add(new UserRiskRespon()
            {
                UserRiskId = a.UserRiskId,
                Name = WalletHelp.GetUidName(a.DIDUserId),
                Reason = a.Reason,
                AuthStatus = a.AuthStatus,
                CreateDate = a.CreateDate
            })
            );

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 修改用户风险状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response> UserRiskStatus(UserRiskStatusReq req)
        {
            using var db = new NDatabase();
            var item = await db.SingleOrDefaultByIdAsync<UserRisk>(req.UserRiskId);
            item.AuthStatus = req.AuthStatus;
            if (!string.IsNullOrEmpty(req.Images))
                item.Images = req.Images;
            await db.UpdateAsync(item);

            return InvokeResult.Success("修改成功!");
        }

        /// <summary>
        /// 解除风险
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response> RemoveRisk(RemoveRiskReq req)
        {
            using var db = new NDatabase();
            var item = await db.SingleOrDefaultByIdAsync<UserRisk>(req.UserRiskId);
            var userId = WalletHelp.GetUserId(req);
            if (item.AuthStatus == RiskStatusEnum.核对成功 && item.AuditUserId == userId)
            {
                item.IsRemoveRisk = IsEnum.是;
                //item.Images = req.Images;
                await db.UpdateAsync(item);

                var list = await db.FetchAsync<UserRisk>("select * from UserRisk where DIDUserId = @0 and IsDelete = 0", item.DIDUserId);
                var num = list.Sum(a => a.IsRemoveRisk == IsEnum.是 ? 1 : 0);

                //5个人3个通过
                if (num >= 3)
                {
                    var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", item.DIDUserId);
                    user.RiskLevel = RiskLevelEnum.低风险;

                    await db.UpdateAsync(user);
                    list.ForEach(a =>
                    {
                        a.IsDelete = IsEnum.是;
                        db.Update(a);
                    });
                }
                return InvokeResult.Success("解除成功!");
            }
            else
                return InvokeResult.Fail("解除失败!");
        }


        /// <summary>
        /// 获取风险用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Response<RiskUserInfo>> GetUserInfo(RemoveRiskReq req)
        {
            using var db = new NDatabase();
            var item = await db.SingleOrDefaultByIdAsync<UserRisk>(req.UserRiskId);
            var authinfo = await db.SingleOrDefaultAsync<UserAuthInfo>("select b.* from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0 and a.AuthType = 2", item.DIDUserId);
            if (authinfo == null) return InvokeResult.Fail<RiskUserInfo>("1");//认证信息未找到!
            var model = new RiskUserInfo();
            model.Name = CommonHelp.GetName(authinfo.Name);

            //后四位变为*
            authinfo.PhoneNum = authinfo.PhoneNum.Remove(authinfo.PhoneNum.Length - 4, 4).Insert(authinfo.PhoneNum.Length - 4, "****");
            authinfo.IdCard = authinfo.IdCard.Remove(authinfo.IdCard.Length - 4, 4).Insert(authinfo.IdCard.Length - 4, "****");
            model.PhoneNum = authinfo.PhoneNum;
            model.IdCard = authinfo.IdCard;

            var auths = await db.FetchAsync<Auth>("select * from Auth where UserAuthInfoId = @0 order by AuditStep Desc", authinfo.UserAuthInfoId);
            if (auths.Count > 0)
            {
                model.PortraitImage = auths?[0].PortraitImage;
                model.NationalImage = auths?[0].NationalImage;
                model.HandHeldImage = auths?[0].HandHeldImage;
            }

            model.Image = item.Images;
            return InvokeResult.Success(model);
        }
    }
}