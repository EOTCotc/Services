using Dao.Common;
using Dao.Models.Request;
using Dao.Models.Response;
using DID.Common;
using DID.Entity;
using DID.Entitys;
using DID.Models.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Services
{
    /// <summary>
    /// 团队认证服务接口
    /// </summary>
    public interface ITeamAuthService
    {
        /// <summary>
        /// 获取团队认证列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type">0 待处理 1 已处理</param>
        /// <returns></returns>
        Task<Response<List<GetTeamAuthListRespon>>> GetTeamAuthList(string userId, int type);

        /// <summary>
        /// 团队认证列表
        /// </summary>
        /// <param name="teamAuthId"></param>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        Task<Response> TeamAuth(string teamAuthId, string userId, TeamAuditEnum type, string? remark);

        /// <summary>
        /// 获取社区信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<GetComInfoRespon>> GetComInfo(string userId);
    }

    /// <summary>
    /// 团队认证服务
    /// </summary>
    public class TeamAuthService : ITeamAuthService
    {
        private readonly ILogger<TeamAuthService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public TeamAuthService(ILogger<TeamAuthService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取团队认证列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type">0 待处理 1 已处理</param>
        /// <returns></returns>
        public async Task<Response<List<GetTeamAuthListRespon>>> GetTeamAuthList(string userId, int type)
        {
            using var db = new NDatabase();
            var items = new List<TeamAuth>();
            if(type == 0)
                items = await db.FetchAsync<TeamAuth>("select * from TeamAuth where AuditUserId = @0 and AuditType = 0",userId);
            else
                items = await db.FetchAsync<TeamAuth>("select * from TeamAuth where AuditUserId = @0 and AuditType > 0", userId, type);

            var list = new List<GetTeamAuthListRespon>();

            items.ForEach( a => {
                var user =  db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", a.DIDUserId);
                //团队人数
                var teamNumber =  db.FirstOrDefault<int>(";with temp as \n" +
                        "(select DIDUserId from DIDUser where DIDUserId = @0 and IsLogout = 0\n" +
                        "union all \n" +
                        "select a.DIDUserId from DIDUser a inner join temp on a.RefUserId = temp.DIDUserId and a.IsLogout = 0) \n" +
                        "select Count(*) from temp", a.DIDUserId);
                list.Add(new GetTeamAuthListRespon()
                {
                    TeamAuthId = a.TeamAuthId,
                    AuditType = a.AuditType,
                    CreateDate = a.CreateDate,
                    Remark = a.Remark,
                    RefUserId = user.RefUserId!,
                    RefUser = WalletHelp.GetUidName(user.RefUserId!),
                    User = WalletHelp.GetUidName(a.DIDUserId),
                    TeanNum = teamNumber
                });
            });

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 团队认证列表
        /// </summary>
        /// <param name="teamAuthId"></param>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task<Response> TeamAuth(string teamAuthId, string userId, TeamAuditEnum type, string? remark)
        {
            using var db = new NDatabase();
            var item = await db.SingleOrDefaultByIdAsync<TeamAuth>(teamAuthId);
            if (null == item)
                return InvokeResult.Fail("认证信息未找到!");

            item.AuditDate = DateTime.Now;
            item.AuditType = type;
            item.AuditUserId = userId;
            item.Remark = remark;

            await db.UpdateAsync(item);

            if (type == TeamAuditEnum.审核通过)
            {
                //todo:发送邮件
                var users = await db.FetchAsync<TeamUser>(";with temp as \n" +
                            "(select *,0 Level from DIDUser where DIDUserId = @0 and IsLogout = 0\n" +
                            "union all \n" +
                            "select a.*,temp.Level+1 Level from DIDUser a inner join temp on a.RefUserId = temp.DIDUserId WHERE temp.Level < 6 and a.IsLogout = 0)\n" +
                            "select * from temp", item.DIDUserId);
                var str = "";
                users.ForEach(a =>
                {
                    var Name = CommonHelp.GetName(db.SingleOrDefault<string>("select b.Name from UserAuthInfo b left join DIDUser a  on a.UserAuthInfoId = b.UserAuthInfoId " +
                "where a.DIDUserId = @0 and a.AuthType = 2", a.DIDUserId))??"未认证";
                    str += "<tr>";
                    str += "<td>" + Name + "</td>";
                    str += "<td>" + a.Mail + "</td>";
                    str += "<td>" + a.Level + "</td>";
                    str += "</tr>";
                });
                if(users.Count > 0)
                    //EmailHelp.SendTeamInfo("625022186@qq.com", str);
                    EmailHelp.SendTeamInfo(users[0].Mail, str);
            }

            return InvokeResult.Success("审核成功!");
        }

        /// <summary>
        /// 获取社区信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<GetComInfoRespon>> GetComInfo(string userId)
        {
            using var db = new NDatabase();
            var com = await db.SingleOrDefaultAsync<Community>("select * from Community a left join UserCommunity b on a.CommunityId = b.CommunityId where b.DIDUserId = @0", userId);
            
            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            string? phone = null;

            if (user.AuthType == AuthTypeEnum.审核成功)
            {
                var authInfo = await db.SingleOrDefaultByIdAsync<UserAuthInfo>(user.UserAuthInfoId);
                phone = authInfo.PhoneNum;
            }

            var model = new GetComInfoRespon()
            {
                ComName = com?.ComName,
                Mail = user?.Mail,
                Phone = phone
            };

            return InvokeResult.Success(model);
        }
    }
}
