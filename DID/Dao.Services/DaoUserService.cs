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
    public interface IDaoUserService
    {
        /// <summary>
        /// 成为仲裁员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response> ToArbitrator(string userId);

        /// <summary>
        /// 成为审核员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response> ToAuditor(string userId);

        /// <summary>
        /// 获取Dao用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<GetDaoInfoRespon>> GetDaoInfo(string userId);

        /// <summary>
        /// 获取审核员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<GetAuditorRespon>> GetAuditor(string userId);

        /// <summary>
        /// 解除审核员身份
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response> RelieveAuditor(string userId);

        /// <summary>
        /// 是否启用Dao审核仲裁权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        Task<Response> SetDaoEnable(string userId, IsEnum isEnable);
    }

    /// <summary>
    /// Dao用户信息服务
    /// </summary>
    public class DaoUserService : IDaoUserService
    {
        private readonly ILogger<DaoUserService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public DaoUserService(ILogger<DaoUserService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 成为仲裁员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response> ToArbitrator(string userId)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);

            if (null == user)
            {
                return InvokeResult.Fail("用户信息未找到!");
            }
            if(user.AuthType != AuthTypeEnum.审核成功)
                return InvokeResult.Fail("用户未认证!");
            var eotc = CurrentUser.GetEUModel(user.DIDUserId)?.StakeEotc ?? 0;
            if (eotc < 5000)
                return InvokeResult.Fail("质押EOTC数量不足!");
            if (user.IsArbitrate == IsEnum.是)
                return InvokeResult.Fail("请勿重复设置!");


            user.IsArbitrate = IsEnum.是;
                
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var nums = await db.FetchAsync<string>("select Number from UserArbitrate order by Number Desc");

            var number = "";
            if (nums.Count > 0 && nums[0]?.Substring(0, 14) == date)
                number = date + (Convert.ToInt32(nums[0].Substring(14, nums[0].Length - 14)) + 1);
            else
                number = date + 1;

            var model = new UserArbitrate() { 
                CreateDate = DateTime.Now,
                DIDUserId = userId,
                UserArbitrateId = Guid.NewGuid().ToString(),
                Number = number
            };

            db.BeginTransaction();
            await db.UpdateAsync(user);
            await db.InsertAsync(model);
            db.CompleteTransaction();

            return InvokeResult.Success("设置成功!");
        }

        /// <summary>
        /// 成为审核员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response> ToAuditor(string userId)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);

            if (null == user)
            {
                return InvokeResult.Fail("用户信息未找到!");
            }
            if (user.AuthType != AuthTypeEnum.审核成功)
                return InvokeResult.Fail("用户未认证!");
            var eotc = CurrentUser.GetEUModel(user.DIDUserId)?.StakeEotc ?? 0;
            if (eotc < 5000)
                return InvokeResult.Fail("质押EOTC数量不足!");
            if (user.IsExamine == IsEnum.是)
                return InvokeResult.Fail("请勿重复设置!");
            user.IsExamine = IsEnum.是;

            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var nums = await db.FetchAsync<string>("select Number from UserArbitrate order by Number Desc");

            var number = "";
            if (nums.Count > 0 && nums[0]?.Substring(0, 14) == date)
                number = date + (Convert.ToInt32(nums[0].Substring(14, nums[0].Length - 14)) + 1);
            else
                number = date + 1;

            var model = new UserExamine()
            {
                CreateDate = DateTime.Now,
                DIDUserId = userId,
                UserExamineId = Guid.NewGuid().ToString(),
                Number = number
            };

            db.BeginTransaction();
            await db.UpdateAsync(user);
            await db.InsertAsync(model);
            db.CompleteTransaction();

            await db.UpdateAsync(user);

            return InvokeResult.Success("设置成功!");
        }

        /// <summary>
        /// 获取审核员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<GetAuditorRespon>> GetAuditor(string userId)
        {
            var db = new NDatabase();

            var model = await db.SingleOrDefaultAsync<UserExamine>("select * from UserExamine where DIDUserId = @0 and IsDelete = 0", userId);

            return InvokeResult.Success(new GetAuditorRespon
            {
                ExamineNum = model.ExamineNum,
                CreateDate = model.CreateDate,
                EOTC = model.EOTC,
                Number = model.Number,
                Name = WalletHelp.GetName(userId)
            });
        }


        /// <summary>
        /// 解除审核员身份
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response> RelieveAuditor(string userId)
        {
            var db = new NDatabase();

            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            user.IsExamine = IsEnum.否;

            var model = await db.SingleOrDefaultAsync<UserExamine>("select * from UserExamine where DIDUserId = @0 and IsDelete = 0", userId);
            model.IsDelete = IsEnum.是;

            db.BeginTransaction();
            await db.UpdateAsync(user);
            await db.UpdateAsync(model);
            db.CompleteTransaction();

            return InvokeResult.Success("解除成功!");
        }


        /// <summary>
        /// 获取Dao用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<GetDaoInfoRespon>> GetDaoInfo(string userId)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            if (null == user)
            {
                return InvokeResult.Fail<GetDaoInfoRespon>("用户信息未找到!");
            }

            return InvokeResult.Success(new GetDaoInfoRespon() { 
                DaoEOTC = user.DaoEOTC,
                IsExamine = user.IsExamine,
                IsArbitrate = user.IsArbitrate,
                RiskLevel = user.RiskLevel,
                AuthType = user.AuthType,
                Mail = user.Mail,
                Uid = user.Uid,
                UserId = userId,
                IsEnable = user.IsEnable
            });
        }

        /// <summary>
        /// 是否启用Dao审核仲裁权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public async Task<Response> SetDaoEnable(string userId, IsEnum isEnable)
        {
            using var db = new NDatabase();
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            if (null == user)
            {
                return InvokeResult.Fail("用户信息未找到!");
            }
            user.IsEnable = isEnable;
            await db.UpdateAsync(user);
            return InvokeResult.Success("设置成功!");
        }


    }
}