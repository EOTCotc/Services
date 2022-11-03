using Dao.Common;
using Dao.Entity;
using Dao.Models.Base;
using Dao.Models.Request;
using Dao.Models.Response;
using DID.Common;
using DID.Entitys;
using DID.Models.Base;
using DID.Models.Request;
using DID.Services;
using Microsoft.Extensions.Logging;
using NPoco;
using TimersHelp = Dao.Common.TimersHelp;

namespace Dao.Services
{
    /// <summary>
    /// 仲裁服务接口
    /// </summary>
    public interface IArbitrateService
    {
        /// <summary>
        /// 获取审核员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<GetArbitratorRespon>> GetArbitrator(string userId);

        /// <summary>
        /// 解除仲裁员身份
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response> RelieveArbitrator(string userId);


        /// <summary>
        /// 获取仲裁员列表
        /// </summary>
        /// <returns></returns>
        Task<Response<List<GetArbitratorsRespon>>> GetArbitrators();

        /// <summary>
        /// 获取仲裁公示
        /// </summary>
        /// <returns></returns>
        Task<Response<List<GetArbitrateInfoRespon>>> GetArbitrateInfo();

        /// <summary>
        /// 获取仲裁详情
        /// </summary>
        /// param name="arbitrateInfoId"
        /// <returns></returns>
        Task<Response<GetArbitrateDetailsRespon>> GetArbitrateDetails(string arbitrateInfoId, string userId);

        /// <summary>
        /// 提交仲裁
        /// </summary>
        /// <param name="plaintiff"></param>
        /// <param name="defendant"></param>
        /// <param name="orderId"></param>
        /// <param name="num"></param>
        /// <param name="memo"></param>
        /// <param name="images"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Response> AddArbitrateInfo(string plaintiff, string defendant, string orderId, int num, string memo, string images, ArbitrateInTypeEnum type);


        /// <summary>
        /// 获取待处理 已仲裁（原告、被告）
        /// </summary>
        /// <returns></returns>
        Task<Response<List<GetArbitrateInfoRespon>>> GetUserArbitrate(string userId, int type);

        /// <summary>
        /// 获取待仲裁 已结案列表 0 待仲裁 1 已仲裁
        /// </summary>
        /// <returns></returns>
        Task<Response<List<GetArbitrateInfoRespon>>> GetArbitrateList(string userId, int type);


        /// <summary>
        /// 仲裁员投票
        /// </summary>
        /// <returns></returns>
        Task<Response> ArbitrateVote(string arbitrateInfoId, string userId, string reason, VoteStatusEnum status);

        /// <summary>
        /// 申请延期
        /// </summary>
        /// <returns></returns>
        Task<Response> ArbitrateDelay(string arbitrateInfoId, string userId, ReasonEnum reason, string explain, int day, IsEnum isArbitrate);

        /// <summary>
        /// 申请延期投票
        /// </summary>
        /// <returns></returns>
        Task<Response> ArbitrateDelayVote(string delayVoteId, DelayVoteStatus status);

        /// <summary>
        /// 追加举证
        /// </summary>
        /// <returns></returns>
        Task<Response> AddAdduceList(string arbitrateInfoId, string userId, string memo, string images);

        /// <summary>
        /// 取消仲裁
        /// </summary>
        /// <returns></returns>
        Task<Response> CancelArbitrate(string userId, string arbitrateInfoId, CancelReasonEnum reason);

        /// <summary>
        /// 获取风险用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<RiskUserInfo>> GetUserInfo(string userId);


        /// <summary>
        /// 获取仲裁消息列表
        /// </summary>
        /// <returns></returns>
        Task<Response<List<GetArbitrateMessageRespon>>> GetArbitrateMessage(string userId, IsEnum isArbitrate);

        /// <summary>
        /// 获取原被告延期消息
        /// </summary>
        /// <returns></returns>
        Task<Response<GetArbitrateDelayRespon>> GetArbitrateDelay(string userId, string id, IsEnum isArbitrate);

        /// <summary>
        /// 获取取消仲裁消息
        /// </summary>
        /// <returns></returns>
        Task<Response<GetCancelArbitrateRespon>> GetCancelArbitrate(string id);

        /// <summary>
        /// 获取追加举证消息
        /// </summary>
        /// <returns></returns>
        Task<Response<GetAdduceListRespon>> GetAdduceList(string id);

        /// <summary>
        /// 获取结案通知
        /// </summary>
        /// <returns></returns>
        Task<Response<GetClosureRespon>> GetClosure(string id);

        /// <summary>
        /// 设置消息为已读
        /// </summary>
        /// <returns></returns>
        Task<Response> SetMessageIsOpen(string messageId);

        /// <summary>
        /// 获取仲裁员是否有未读消息
        /// </summary>
        /// <returns></returns>
        Task<Response<int>> GetMessageIsOpen(string userId, IsEnum isArbitrate);

        /// <summary>
        /// 获取被告待处理消息
        /// </summary>
        /// <returns></returns>
        Task<Response<int>> GetWaitMessage(string userId);

        /// <summary>
        /// 添加支付信息
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response> AddArbitratePay(AddArbitratePayReq req, string userId);
    }

    /// <summary>
    /// 仲裁服务
    /// </summary>
    public class ArbitrateService : IArbitrateService
    {
        private readonly ILogger<ArbitrateService> _logger;

        private readonly ICreditScoreService _csservice;

        private readonly IRewardService _reservice;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="csservice"></param>
        public ArbitrateService(ILogger<ArbitrateService> logger, ICreditScoreService csservice, IRewardService reservice)
        {
            _logger = logger;
            _csservice = csservice;
            _reservice = reservice;
        }
        /// <summary>
        /// 添加支付信息
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response> AddArbitratePay(AddArbitratePayReq req, string userId)
        {
            using var db = new NDatabase();
            var item = new ArbitratePay {
                ArbitrateInfoId = req.ArbitrateInfoId,
                ArbitratePayId = Guid.NewGuid().ToString(),
                EOTC = req.EOTC,
                PayDate = DateTime.Now,
                PayUserId = userId,
                Remark = req.Remark
            };
            await db.InsertAsync(item);
            
            return InvokeResult.Success("添加成功!");
        }
        /// <summary>
        /// 获取仲裁员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<GetArbitratorRespon>> GetArbitrator(string userId)
        {
            var db = new NDatabase();

            var model = await db.SingleOrDefaultAsync<UserArbitrate>("select * from UserArbitrate where DIDUserId = @0 and IsDelete = 0", userId);
            if (null == model)
                return InvokeResult.Fail<GetArbitratorRespon>("未找到仲裁员信息!");

            return InvokeResult.Success(new GetArbitratorRespon
            {
                ArbitrateNum = model.ArbitrateNum,
                CreateDate = model.CreateDate,
                EOTC = model.EOTC,
                Number = model.Number,
                VictoryNum = model.VictoryNum,
                Name = WalletHelp.GetName(userId)
            });
        }


        /// <summary>
        /// 解除仲裁员身份
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response> RelieveArbitrator(string userId)
        {
            var db = new NDatabase();

            var user = await db.SingleOrDefaultAsync<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
            user.IsArbitrate = IsEnum.否;

            var model = await db.SingleOrDefaultAsync<UserArbitrate>("select * from UserArbitrate where DIDUserId = @0 and IsDelete = 0", userId);
            model.IsDelete = IsEnum.是;

            db.BeginTransaction();
            await db.UpdateAsync(user);
            await db.UpdateAsync(model);
            db.CompleteTransaction();

            return InvokeResult.Success("解除成功!");
        }

        /// <summary>
        /// 获取仲裁员列表
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<GetArbitratorsRespon>>> GetArbitrators()
        {
            var db = new NDatabase();

            var items = await db.FetchAsync<UserArbitrate>("select * from UserArbitrate where IsDelete = 0");

            var list = items.Select(a =>
            {
                return new GetArbitratorsRespon
                {
                    ArbitrateNum = a.ArbitrateNum,
                    CreateDate = a.CreateDate,
                    Name = CommonHelp.GetName(WalletHelp.GetName(a.DIDUserId)),
                    Number = a.Number
                };
            }).ToList();

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取仲裁公示
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<GetArbitrateInfoRespon>>> GetArbitrateInfo()
        {
            var db = new NDatabase();

            var items = await db.FetchAsync<ArbitrateInfo>("select * from ArbitrateInfo where Status > 1 and IsCancel = 0 order by VoteDate Desc");//获取已判决仲裁列表

            var list = items.Select(a =>
            {
                return new GetArbitrateInfoRespon
                {
                    ArbitrateInfoId = a.ArbitrateInfoId,
                    Status = a.Status,
                    Defendant = WalletHelp.GetName(a.Defendant),
                    Plaintiff = WalletHelp.GetName(a.Plaintiff),
                    DefendantNum = db.SingleOrDefault<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", a.ArbitrateInfoId, VoteStatusEnum.被告胜),
                    PlaintiffNum = db.SingleOrDefault<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", a.ArbitrateInfoId, VoteStatusEnum.原告胜),
                    AdduceDate = a.AdduceDate,
                    VoteDate = a.VoteDate,
                    PlaintiffId = a.Plaintiff,
                    DefendantId = a.Defendant,
                    OrderId = a.OrderId
                };
            }).ToList();

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取仲裁详情
        /// </summary>
        /// param name="arbitrateInfoId"
        /// <returns></returns>
        public async Task<Response<GetArbitrateDetailsRespon>> GetArbitrateDetails(string arbitrateInfoId, string userId)
        {
            var db = new NDatabase();
            var item = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(arbitrateInfoId);
            if(null == item)
                return InvokeResult.Fail<GetArbitrateDetailsRespon>("仲裁信息未找到!");

            var model = new GetArbitrateDetailsRespon
            {
                ArbitrateInfoId = item.ArbitrateInfoId,
                VoteDate = item.VoteDate,
                AdduceDate = item.AdduceDate,
                CreateDate = item.CreateDate,
                Status = item.Status,
                Defendant = WalletHelp.GetName(item.Defendant),
                Plaintiff = WalletHelp.GetName(item.Plaintiff),
                DefendantNum = await db.SingleOrDefaultAsync<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", item.ArbitrateInfoId, VoteStatusEnum.被告胜),
                PlaintiffNum = await db.SingleOrDefaultAsync<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", item.ArbitrateInfoId, VoteStatusEnum.原告胜),
                PlaintiffId = item.Plaintiff,
                DefendantId = item.Defendant,
                DefendantUId = db.SingleOrDefault<string>("select Uid from DIDUser where DIDUserId = @0", item.Defendant),
                PlaintiffUId = db.SingleOrDefault<string>("select Uid from DIDUser where DIDUserId = @0", item.Plaintiff),
                IsCancel = item.IsCancel,
                ArbitrateInType = item.ArbitrateInType,
                OrderId = item.OrderId
            };

            var users = await db.FetchAsync<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus > 0 and IsDelete = 0", item.ArbitrateInfoId);//已投票记录

            var votes = new List<Vote>();

            users.ForEach(a =>
            {
                votes.Add(new Vote
                {
                    Name = WalletHelp.GetName(a.VoteUserId),
                    Number = db.SingleOrDefault<string>("select Number from UserArbitrate where DIDUserId = @0 and IsDelete = 0", a.VoteUserId),
                    VoteStatus = a.VoteStatus,
                    Reason = a.Reason
                });
            });
            model.Votes = votes;
 
            model.AVotes = users;

            model.VoteStatus = await db.SingleOrDefaultAsync<VoteStatusEnum>("select VoteStatus from ArbitrateVote where ArbitrateInfoId = @0 and VoteUserId = @1", item.ArbitrateInfoId, userId);

            //举证记录
            var adduce = await db.FetchAsync<AdduceList>("select * from AdduceList where ArbitrateInfoId = @0 order by CreateDate Desc", arbitrateInfoId);
            model.Adduce = adduce;

            //当前仲裁员判决
            var useradduce = await db.SingleOrDefaultAsync<ArbitrateVote>("select * from ArbitrateVote where  ArbitrateInfoId = @0 and VoteUserId = @1 and IsDelete = 0", arbitrateInfoId, userId);
            if (null != useradduce)
            {
                model.UserVote = new Vote
                {
                    Name = WalletHelp.GetName(useradduce.VoteUserId),
                    Number = db.SingleOrDefault<string>("select Number from UserArbitrate where DIDUserId = @0 and IsDelete = 0", useradduce.VoteUserId),
                    VoteStatus = useradduce.VoteStatus,
                    Reason = useradduce.Reason
                };
            }
            model.EOTC = 100;

            var delay = await db.SingleOrDefaultAsync<ArbitrateDelay>("select * from ArbitrateDelay where ArbitrateInfoId = @0 and DelayUserId = @1", arbitrateInfoId, userId);
            model.HasDelay = null == delay ? IsEnum.否 : IsEnum.是;

            return InvokeResult.Success(model);
        }

        /// <summary>
        /// 提交仲裁
        /// </summary>
        /// <param name="plaintiff">uId</param>
        /// <param name="defendant">uId</param>
        /// <param name="orderId"></param>
        /// <param name="num"></param>
        /// <param name="memo"></param>
        /// <param name="images"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Response> AddArbitrateInfo(string plaintiff, string defendant, string orderId, int num, string memo, string images, ArbitrateInTypeEnum type)
        {
            using var db = new NDatabase();

            //通过UID获取用户编号
            var defendantUser = await db.SingleOrDefaultByIdAsync<DIDUser>(defendant);
            var plaintiffUser = await db.SingleOrDefaultByIdAsync<DIDUser>(plaintiff);
            if (null == plaintiffUser)
                return InvokeResult.Fail("原告信息未找到!");
            if (null == defendantUser)
                return InvokeResult.Fail("被告信息未找到!");
            var voteHours = Convert.ToInt32(_reservice.GetRewardValue("VoteHours").Result.Items);
            var adduceHours = Convert.ToInt32(_reservice.GetRewardValue("AdduceHours").Result.Items);
            var item = new ArbitrateInfo
            {
                ArbitrateInfoId = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                AdduceDate = DateTime.Now.AddHours(adduceHours),//举证3三天
                //AdduceDate = DateTime.Now.AddMinutes(5),
                Defendant = defendantUser.DIDUserId,
                OrderId = orderId,
                Plaintiff = plaintiffUser.DIDUserId,
                Number = num,
                Status = ArbitrateStatusEnum.举证中,
                VoteDate = DateTime.Now.AddHours(voteHours),//投票3天
                //VoteDate = DateTime.Now.AddMinutes(5),
                ArbitrateInType = type
            };

            //todo: 获取仲裁投票用户编号
            var list = new List<string>();

            var userIds = await db.FetchAsync<DIDUser>("select * from DIDUser where DIDUserId != @0 and DIDUserId != @1 and IsArbitrate = 1 and IsEnable = 1 and IsLogout = 0", defendantUser.DIDUserId, plaintiffUser.DIDUserId);
            if(userIds.Count < num)
                return InvokeResult.Fail("仲裁员人数不足!");
            for (var i = 0; i < num; i++)
            {
                var random = 0;
                do
                {
                    random = new Random().Next(userIds.Count);
                } while (list.Exists(a => a == userIds[random].DIDUserId));
                list.Add(userIds[random].DIDUserId);
            }

            //var userIds = new List<string>();
            //userIds.Add("e8771b3c-3b05-4830-900d-df2be0a6e9f7");
            //userIds.Add("d389e5db-37d0-40cd-9d8b-0d31a0ef2c12");
            //userIds.Add("61d14a4f-c45f-4b13-a957-5bcaff9b3324");
            //userIds.Add("7e88d292-7454-4e26-821a-b4e6049a7a95");
            //userIds.Add("2a5bf1dd-e15b-40f4-94bb-b68cee2bbaf9");

            var votes = new List<ArbitrateVote>();
            list.ForEach(a =>
            {
                votes.Add(new ArbitrateVote
                {
                    ArbitrateVoteId = Guid.NewGuid().ToString(),
                    ArbitrateInfoId = item.ArbitrateInfoId,
                    CreateDate = DateTime.Now,
                    VoteStatus = VoteStatusEnum.未投票,
                    VoteUserId = a
                });
            });

            //举证
            var adduce = new AdduceList
            {
                AdduceListId = Guid.NewGuid().ToString(),
                ArbitrateInfoId = item.ArbitrateInfoId,
                AdduceUserId = plaintiffUser.DIDUserId,
                CreateDate = DateTime.Now,
                Images = images,
                Memo = memo
            };

            //发送消息
            SendMessage(new List<string> { defendantUser.DIDUserId }, MessageTypeEnum.被告消息, item.ArbitrateInfoId, IsEnum.否);

            db.BeginTransaction();
            await db.InsertAsync(item);
            await db.InsertBatchAsync(votes);
            await db.InsertAsync(adduce);
            db.CompleteTransaction();

            //默认3天举证时间
            //ToDelay(item.ArbitrateInfoId, 3 * 24 * 3600 * 1000);
            //ToDelay(item.ArbitrateInfoId, 10 * 60 * 1000);
            //默认3天举证时间
            //var adduceHours = Convert.ToInt32(_reservice.GetRewardValue("AdduceHours").Result.Items);
            var adduceTimer = new Timers { 
                TimersId = Guid.NewGuid().ToString(), 
                Rid = item.ArbitrateInfoId, 
                CreateDate = DateTime.Now,
                StartTime = DateTime.Now, 
                EndTime = DateTime.Now.AddHours(adduceHours),
                //EndTime = DateTime.Now.AddMinutes(5),
                TimerType = TimerTypeEnum.仲裁举证
            };
            await db.InsertAsync(adduceTimer);
            TimersHelp.ArbitrateAdduceTimer(adduceTimer);

            return InvokeResult.Success("提交成功!");
        }

        /// <summary>
        /// 获取待处理 已仲裁（原告、被告）
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<GetArbitrateInfoRespon>>> GetUserArbitrate(string userId, int type)
        {
            using var db = new NDatabase();
            var items = new List<ArbitrateInfo>();
            if (type == 0)//待处理
                items = await db.FetchAsync<ArbitrateInfo>("select * from ArbitrateInfo where Status < 2 and (Plaintiff = @0 or Defendant = @0) and IsCancel = 0 order by VoteDate Desc", userId);
            else
                items = await db.FetchAsync<ArbitrateInfo>("select * from ArbitrateInfo where (Status > 1 or IsCancel = 1) and (Plaintiff = @0 or Defendant = @0) order by VoteDate Desc", userId);

            var list = items.Select(a =>
            {
                return new GetArbitrateInfoRespon
                {
                    ArbitrateInfoId = a.ArbitrateInfoId,
                    Status = a.Status,
                    AdduceDate = a.AdduceDate,
                    VoteDate = a.VoteDate,
                    Defendant = WalletHelp.GetName(a.Defendant),
                    Plaintiff = WalletHelp.GetName(a.Plaintiff),
                    ArbitrateInType = a.ArbitrateInType,
                    DefendantNum = db.SingleOrDefault<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", a.ArbitrateInfoId, VoteStatusEnum.被告胜),
                    PlaintiffNum = db.SingleOrDefault<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", a.ArbitrateInfoId, VoteStatusEnum.原告胜),
                    IsCancel = a.IsCancel,
                    PlaintiffId = a.Plaintiff,
                    DefendantId = a.Defendant,
                    DefendantUId = db.SingleOrDefault<string>("select Uid from DIDUser where DIDUserId = @0", a.Defendant),
                    PlaintiffUId = db.SingleOrDefault<string>("select Uid from DIDUser where DIDUserId = @0", a.Plaintiff),
                    HasDelay = db.SingleOrDefault<ArbitrateDelay>("select * from ArbitrateDelay where ArbitrateInfoId = @0 and DelayUserId = @1", a.ArbitrateInfoId, userId) == null ? IsEnum.否 : IsEnum.是,
                    OrderId = a.OrderId
                };
            }).ToList();

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取待仲裁 已结案列表 0 待仲裁 1 已仲裁
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<GetArbitrateInfoRespon>>> GetArbitrateList(string userId, int type)
        {
            using var db = new NDatabase();
            var items = new List<ArbitrateVote>();
            if (type == 0)//待仲裁
                items = await db.FetchAsync<ArbitrateVote>("select * from ArbitrateVote where VoteStatus = 0 and VoteUserId = @0 and IsDelete = 0 order by CreateDate Desc", userId);
            else
                items = await db.FetchAsync<ArbitrateVote>("select * from ArbitrateVote where (VoteStatus > 0 or IsDelete = 1) and VoteUserId = @0 order by CreateDate Desc", userId);//已仲裁

            var list = new List<GetArbitrateInfoRespon>();
            items.ForEach(a =>
            {
                var model = db.SingleOrDefaultById<ArbitrateInfo>(a.ArbitrateInfoId);
                if (model.IsCancel == IsEnum.否)
                {
                    var delay = db.SingleOrDefault<ArbitrateDelay>("select * from ArbitrateDelay where ArbitrateInfoId = @0 and DelayUserId = @1", a.ArbitrateInfoId, userId);

                    list.Add(new GetArbitrateInfoRespon
                    {
                        ArbitrateInfoId = a.ArbitrateInfoId,
                        Status = model.Status,
                        AdduceDate = model.AdduceDate,
                        VoteDate = model.VoteDate,
                        ArbitrateInType = model.ArbitrateInType,
                        Defendant = WalletHelp.GetName(model.Defendant),
                        Plaintiff = WalletHelp.GetName(model.Plaintiff),
                        DefendantNum = db.SingleOrDefault<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", a.ArbitrateInfoId, VoteStatusEnum.被告胜),
                        PlaintiffNum = db.SingleOrDefault<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", a.ArbitrateInfoId, VoteStatusEnum.原告胜),
                        IsCancel = model.IsCancel,
                        VoteStatus = a.VoteStatus,
                        PlaintiffId = model.Plaintiff,
                        DefendantId = model.Defendant,
                        EOTC = 100,//EOTC数量
                        HasDelay = delay == null ? IsEnum.否 : IsEnum.是,
                        OrderId = model.OrderId
                    });
                }
            });

            return InvokeResult.Success(list);
        }

        /// <summary>
        /// 获取风险用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Response<RiskUserInfo>> GetUserInfo(string userId)
        {
            using var db = new NDatabase();
            var authinfo = await db.SingleOrDefaultAsync<UserAuthInfo>("select b.* from DIDUser a left join UserAuthInfo b on  a.UserAuthInfoId = b.UserAuthInfoId where a.DIDUserId = @0 and a.AuthType = 2", userId);
            if (authinfo == null) return InvokeResult.Fail<RiskUserInfo>("认证信息未找到!");//认证信息未找到!
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

            return InvokeResult.Success(model);
        }


        /// <summary>
        /// 仲裁员投票
        /// </summary>
        /// <returns></returns>
        public async Task<Response> ArbitrateVote(string arbitrateInfoId, string userId, string reason, VoteStatusEnum status)
        {
            using var db = new NDatabase();
            var arbitrate = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(arbitrateInfoId);
            if (null == arbitrate)
                return InvokeResult.Fail("信息未找到!");
            if (DateTime.Now < arbitrate.AdduceDate)
                return InvokeResult.Fail("举证中!");
            if (DateTime.Now > arbitrate.VoteDate)
                return InvokeResult.Fail("投票已截止!");

            var vote = await db.SingleOrDefaultAsync<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and VoteUserId = @1 and IsDelete = 0", arbitrateInfoId, userId);
            if (null == vote)
                return InvokeResult.Fail("信息未找到!");

            vote.VoteStatus = status;
            vote.VoteDate = DateTime.Now;
            vote.Reason = reason;
            await db.UpdateAsync(vote);

            var model = await db.SingleOrDefaultAsync<UserArbitrate>("select * from UserArbitrate where DIDUserId = @0 and IsDelete = 0", userId);
            if(null == model)
                return InvokeResult.Fail("仲裁员信息未找到!");
            model.ArbitrateNum += 1;
            await db.UpdateAsync(model);

            //提前结束
            var votes = await db.FetchAsync<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", arbitrateInfoId, VoteStatusEnum.未投票);
            if (votes.Count == 0)
            {
                var uservotes = await db.FetchAsync<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and IsDelete = 0", arbitrateInfoId);//全部投票信息
                //原告票数
                var ynum = uservotes.Where(a => a.VoteStatus == VoteStatusEnum.原告胜).Count();
                //被告票数
                var bnum = uservotes.Where(a => a.VoteStatus == VoteStatusEnum.被告胜).Count();
                //原告胜
                if (ynum > arbitrate.Number / 2)
                {
                    arbitrate.Status = ArbitrateStatusEnum.原告胜;
                }

                //被告胜
                if (bnum > arbitrate.Number / 2)
                {
                    arbitrate.Status = ArbitrateStatusEnum.被告胜;
                }
                //发送结案通知
                if (arbitrate.Status == ArbitrateStatusEnum.原告胜 || arbitrate.Status == ArbitrateStatusEnum.被告胜)
                {
                    SendMessage(new List<string> { arbitrate.Plaintiff, arbitrate.Defendant }, MessageTypeEnum.结案通知, arbitrateInfoId, IsEnum.否);
                    SendMessage(uservotes.Select(a => a.VoteUserId).ToList(), MessageTypeEnum.结案通知, arbitrateInfoId, IsEnum.是);
                }
                arbitrate.VoteDate = DateTime.Now;
                await db.UpdateAsync(arbitrate);

                //仲裁员仲裁胜利次数+1
                uservotes.ForEach(a =>
                {
                    if (a.VoteStatus == (arbitrate.Status == ArbitrateStatusEnum.原告胜 ? VoteStatusEnum.原告胜 : VoteStatusEnum.被告胜) )
                    {
                        var model = db.SingleOrDefault<UserArbitrate>("select * from UserArbitrate where DIDUserId = @0 and IsDelete = 0", a.VoteUserId);
                        var eotc = Convert.ToDouble(_reservice.GetRewardValue("Arbitration").Result.Items);//奖励eotc数量
                        model.VictoryNum += 1;
                        model.EOTC += eotc;
                        db.Update(model);
                        //奖励EOTC
                        var detail = new IncomeDetails()
                        {
                            IncomeDetailsId = Guid.NewGuid().ToString(),
                            CreateDate = DateTime.Now,
                            EOTC = eotc,
                            Remarks = "仲裁处理",
                            Type = IDTypeEnum.处理仲裁,
                            DIDUserId = a.VoteUserId
                        };
                        var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", a.VoteUserId);

                        db.BeginTransaction();
                        db.Insert(detail);
                        user.DaoEOTC += eotc;
                        db.Update(user);
                        db.CompleteTransaction();
                    }    
                });

                //关闭定时器
                t1.Stop();
            }

            return InvokeResult.Success("判决成功!");
        }


        //3天默认时间
        private static System.Timers.Timer t = new();//实例化Timer类，设置间隔时间为10000毫秒；
        //3天仲裁时间
        private readonly System.Timers.Timer t1 = new(3 * 24 * 3600 * 1000);
        //private static System.Timers.Timer t1 = new(10 * 60 * 1000);

        /// <summary>
        /// 举证定时器 
        /// </summary>
        /// <param name="arbitrateInfoId"></param>
        /// <param name="time"></param>
        public void ToDelay(string arbitrateInfoId, int time)
        {
            t.Stop();
            t = new(time);
            t.Elapsed += new System.Timers.ElapsedEventHandler((object? source, System.Timers.ElapsedEventArgs e) =>
            {

                t.Stop(); //先关闭定时器
                t1.Stop(); //先关闭定时器        

                using var db = new NDatabase();
                var item = db.SingleOrDefaultById<ArbitrateInfo>(arbitrateInfoId);

                if (DateTime.Now >= item.AdduceDate)
                {

                    item.Status = ArbitrateStatusEnum.投票中;
                    db.Update(item);
                    t1.Elapsed += new System.Timers.ElapsedEventHandler((object? source, System.Timers.ElapsedEventArgs e) =>
                    {
                        t1.Stop(); //先关闭定时器
                        using var db = new NDatabase(); 
                        var item = db.SingleOrDefaultById<ArbitrateInfo>(arbitrateInfoId);
                        var uservotes = db.Fetch<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and IsDelete = 0", arbitrateInfoId);//全部投票信息
                        //原告票数
                        var ynum = uservotes.Where(a => a.VoteStatus == VoteStatusEnum.原告胜).Count();
                        //被告票数
                        var bnum = uservotes.Where(a => a.VoteStatus == VoteStatusEnum.被告胜).Count();

                        //原告胜
                        if (ynum > item.Number / 2)
                        {
                            item.Status = ArbitrateStatusEnum.原告胜;
                        }

                        //被告胜
                        if (bnum > item.Number / 2)
                        {
                            item.Status = ArbitrateStatusEnum.被告胜;
                        }

                        var votes = db.Fetch<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", arbitrateInfoId, VoteStatusEnum.未投票);

                        votes.ForEach(a =>
                        {
                            var userId = a.VoteUserId;
                            //todo: 扣分 扣EOTC
                            var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
                            _csservice.CreditScore(new CreditScoreReq { Fraction = 3, Remarks = "仲裁到时未投票", Type = TypeEnum.减分, Uid = user.Uid });

                            a.IsDelete = IsEnum.是;
                            db.Update(a);

                            //更新延期状态
                            var delay = db.SingleOrDefault<ArbitrateDelay>("select * from ArbitrateDelay where IsArbitrate = 1 and ArbitrateInfoId = @0", arbitrateInfoId);
                            if (null != delay)
                            {
                                delay.Status = 2;
                                db.Update(delay);
                            }
                                
                        });

                        //发送结案通知
                        if (item.Status == ArbitrateStatusEnum.原告胜 || item.Status == ArbitrateStatusEnum.被告胜)
                        {
                            SendMessage(new List<string> { item.Plaintiff, item.Defendant }, MessageTypeEnum.结案通知, arbitrateInfoId, IsEnum.否);
                            SendMessage(uservotes.Where(a => a.IsDelete == IsEnum.否).Select(a => a.ArbitrateInfoId).ToList(), MessageTypeEnum.结案通知, arbitrateInfoId, IsEnum.是);

                            //仲裁员仲裁胜利次数+1
                            uservotes.ForEach(a =>
                            {
                                if (a.VoteStatus == (item.Status == ArbitrateStatusEnum.原告胜 ? VoteStatusEnum.原告胜 : VoteStatusEnum.被告胜))
                                {
                                    var model = db.SingleOrDefault<UserArbitrate>("select * from UserArbitrate where DIDUserId = @0 and IsDelete = 0", a.VoteUserId);
                                    var eotc = Convert.ToDouble(_reservice.GetRewardValue("Arbitration").Result.Items);//奖励eotc数量
                                    model.VictoryNum += 1;
                                    model.EOTC += eotc;
                                    db.Update(model);

                                    //奖励EOTC
                                    var detail = new IncomeDetails()
                                    {
                                        IncomeDetailsId = Guid.NewGuid().ToString(),
                                        CreateDate = DateTime.Now,
                                        EOTC = eotc,
                                        Remarks = "仲裁处理",
                                        Type = IDTypeEnum.处理仲裁,
                                        DIDUserId = a.VoteUserId
                                    };
                                    var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", a.VoteUserId);

                                    db.BeginTransaction();
                                    db.Insert(detail);
                                    user.DaoEOTC += eotc;
                                    db.Update(user);
                                    db.CompleteTransaction();
                                }
                            });
                        }

                        //限时未出结果 未投票用户扣分 加入新人
                        if (item.Status == ArbitrateStatusEnum.投票中)
                        {
                            var allvotes = db.Fetch<string>("select VoteUserId from ArbitrateVote where ArbitrateInfoId = @0", arbitrateInfoId);
                            var list = new List<string>();
                            var userIds = db.Fetch<DIDUser>("select * from DIDUser where DIDUserId != @0 and DIDUserId != @1 and IsArbitrate = 1 and IsLogout = 0 and IsEnable = 1 and DIDUserId not in (@2)",
                                item.Defendant, item.Plaintiff, allvotes);

                            for (var i = 0; i < votes.Count; i++)
                            {
                                var random = 0;
                                do
                                {
                                    random = new Random().Next(userIds.Count);
                                } while (list.Exists(a => a == userIds[random].DIDUserId));
                                list.Add(userIds[random].DIDUserId);
                            }
                            list.ForEach(a =>
                            {
                                //新的投票对象
                                var newvote = new ArbitrateVote
                                {
                                    ArbitrateVoteId = Guid.NewGuid().ToString(),
                                    ArbitrateInfoId = item.ArbitrateInfoId,
                                    CreateDate = DateTime.Now,
                                    VoteStatus = VoteStatusEnum.未投票,
                                    VoteUserId = a
                                };

                                db.Insert(newvote);
                            });
                            var voteHours = Convert.ToInt32(_reservice.GetRewardValue("VoteHours").Result.Items);
                            item.VoteDate = item.VoteDate.AddHours(voteHours);//投票时间延长3天
                            //item.VoteDate = item.VoteDate.AddMinutes(5);//投票时间延长3天
                            db.Update(item);
                            //重新启动投票定时器
                            t1.AutoReset = false;
                            t1.Enabled = true;
                            t1.Start();
                        }

                        db.Update(item);
                    });
                    t1.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
                    t1.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                    t1.Start(); //启动定时器
                }
                else
                {
                    var time = (int)(item.AdduceDate - DateTime.Now).TotalMilliseconds;
                    ToDelay(arbitrateInfoId, time);
                }
            });//到达时间的时候执行事件；
            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            t.Start(); //启动定时器
        }

        /// <summary>
        /// 申请延期
        /// </summary>
        /// <returns></returns>
        public async Task<Response> ArbitrateDelay(string arbitrateInfoId, string userId, ReasonEnum reason, string explain, int day, IsEnum isArbitrate)
        {
            using var db = new NDatabase();
            db.BeginTransaction();
            var item = new ArbitrateDelay
            {
                ArbitrateDelayId = Guid.NewGuid().ToString(),
                ArbitrateInfoId = arbitrateInfoId,
                CreateDate = DateTime.Now,
                Days = day,
                DelayUserId = userId,
                Explain = explain,
                Reason = reason,
                IsArbitrate = isArbitrate
            };
            await db.InsertAsync(item);
            //原被告 支付成功直接延期
            if (isArbitrate == IsEnum.否)
            {
                //todo:验证是否支付成功
                var model = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(arbitrateInfoId);
                model.AdduceDate = model.AdduceDate.AddDays(day);
                model.VoteDate = model.VoteDate.AddDays(day);
                model.Status = ArbitrateStatusEnum.举证中;
                await db.UpdateAsync(model);
                item.Status = 1;
                await db.UpdateAsync(item);
                //var time = (int)(model.AdduceDate - DateTime.Now).TotalMilliseconds;
                //ToDelay(arbitrateInfoId, time);

                //发送延期成功消息(原告，被告)
                if(userId == model.Plaintiff)
                    SendMessage(new List<string> { model.Defendant }, MessageTypeEnum.申请延期, item.ArbitrateDelayId, IsEnum.否);
                else
                    SendMessage(new List<string> { model.Plaintiff }, MessageTypeEnum.申请延期, item.ArbitrateDelayId, IsEnum.否);
            }

            if (isArbitrate == IsEnum.是)
            {
                //todo: 获取仲裁投票用户编号 发起人默认同意
                var userIds = await db.FetchAsync<string>("select VoteUserId from ArbitrateVote where ArbitrateInfoId = @0 and IsDelete = 0 and VoteUserId != @1", arbitrateInfoId, userId);
                var votes = new List<DelayVote>();
                userIds.ForEach(a =>
                {
                    votes.Add(new DelayVote
                    {
                        DelayVoteId = Guid.NewGuid().ToString(),
                        ArbitrateDelayId = item.ArbitrateDelayId,
                        CreateDate = DateTime.Now,
                        Status = DelayVoteStatus.未投票,
                        VoteUserId = a
                    });
                });
                await db.InsertBatchAsync(votes);
                //发送延期投票消息
                SendMessage(userIds, MessageTypeEnum.申请延期, item.ArbitrateDelayId, IsEnum.是);
            }

            
            
            db.CompleteTransaction();

            return InvokeResult.Success("申请成功!");
        }


        /// <summary>
        /// 申请延期投票
        /// </summary>
        /// <returns></returns>
        public async Task<Response> ArbitrateDelayVote(string delayVoteId, DelayVoteStatus status)
        {
            using var db = new NDatabase();

            var delayVote = await db.SingleOrDefaultByIdAsync<DelayVote>(delayVoteId);
            if(null == delayVote)
                return InvokeResult.Fail("投票记录未找到!");
            delayVote.Status = status;
            await db.UpdateAsync(delayVote);

            var list = await db.FetchAsync<DelayVote>("select * from DelayVote where ArbitrateDelayId = @0", delayVote.ArbitrateDelayId);
            var num = 1;//同意票数（提交人默认同意）

            list.ForEach(a => { if (a.Status == DelayVoteStatus.同意) num++;});

            if (status == DelayVoteStatus.同意 && (num - 1) <= (list.Count + 1) / 2 && num > (list.Count + 1) / 2)
            {
                var delay = await db.SingleOrDefaultByIdAsync<ArbitrateDelay>(delayVote.ArbitrateDelayId);
                var model = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(delay.ArbitrateInfoId);
                model.AdduceDate = DateTime.Now.AddDays(delay.Days);
                model.VoteDate = DateTime.Now.AddDays(delay.Days);
                model.Status = ArbitrateStatusEnum.举证中;
                await db.UpdateAsync(model);
                delay.Status = 1;
                await db.UpdateAsync(delay);

                var votes = await db.FetchAsync<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and IsDelete = 0", delay.ArbitrateInfoId);
                foreach (var i in votes)
                {
                    i.VoteDate = null;
                    i.Reason = null;
                    i.VoteStatus = VoteStatusEnum.未投票;
                    await db.UpdateAsync(i);
                }

                //延期
                //var time = (model.AdduceDate - DateTime.Now).Milliseconds;
                //ToDelay(delay.ArbitrateInfoId, delay.Days * 24 * 60 * 3600 * 1000);

                //发送延期成功消息(原告，被告)
                SendMessage(new List<string> { model.Plaintiff, model.Defendant}, MessageTypeEnum.申请延期, delayVote.ArbitrateDelayId, IsEnum.否);
            }
            return InvokeResult.Success("投票成功!");
        }

        /// <summary>
        /// 追加举证
        /// </summary>
        /// <returns></returns>
        public async Task<Response> AddAdduceList(string arbitrateInfoId, string userId, string memo, string images)
        {
            using var db = new NDatabase();

            var arbitrate = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(arbitrateInfoId);

            if (DateTime.Now > arbitrate.AdduceDate)
                return InvokeResult.Fail("举证已截止!");

            //举证
            var adduce = new AdduceList
            {
                AdduceListId = Guid.NewGuid().ToString(),
                ArbitrateInfoId = arbitrateInfoId,
                AdduceUserId = userId,
                CreateDate = DateTime.Now,
                Images = images,
                Memo = memo
            };

            //发送消息原被告
            var id = userId == arbitrate.Defendant ? arbitrate.Plaintiff : arbitrate.Defendant;
            SendMessage(new List<string> { id }, MessageTypeEnum.追加举证, adduce.AdduceListId, IsEnum.否);
            //仲裁员
            var userIds = await db.FetchAsync<string>("select VoteUserId from ArbitrateVote where ArbitrateInfoId = @0 and IsDelete = 0", arbitrateInfoId);
            SendMessage(userIds, MessageTypeEnum.追加举证, adduce.AdduceListId, IsEnum.是);

            await db.InsertAsync(adduce);
            return InvokeResult.Success("添加成功!");
        }

        /// <summary>
        /// 取消仲裁
        /// </summary>
        /// <returns></returns>
        public async Task<Response> CancelArbitrate(string userId, string arbitrateInfoId, CancelReasonEnum reason) 
        {
            using var db = new NDatabase();

            var arbitrate = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(arbitrateInfoId);

            if (null == arbitrate)
                return InvokeResult.Fail("信息未找到!");
            if (userId != arbitrate.Plaintiff)
                return InvokeResult.Fail("没有权限!");

            arbitrate.IsCancel = IsEnum.是;
            arbitrate.CancelReason = reason;
            arbitrate.VoteDate = DateTime.Now;
            await db.UpdateAsync(arbitrate);

            //关闭定时器
            t.Stop();
            t1.Stop();
            //发送消息被告
            SendMessage(new List<string> { arbitrate.Defendant }, MessageTypeEnum.仲裁取消, arbitrateInfoId, IsEnum.否);
            //仲裁员
            var userIds = await db.FetchAsync<string>("select VoteUserId from ArbitrateVote where ArbitrateInfoId = @0 and IsDelete = 0", arbitrateInfoId);
            SendMessage(userIds, MessageTypeEnum.仲裁取消, arbitrateInfoId, IsEnum.是);

            return InvokeResult.Success("取消成功!");
        }


        /// <summary>
        /// 获取仲裁消息列表
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<GetArbitrateMessageRespon>>> GetArbitrateMessage(string userId, IsEnum isArbitrate)
        {
            using var db = new NDatabase();

            var list = await db.FetchAsync<ArbitrateMessage>("select * from ArbitrateMessage where UserId = @0 and IsArbitrate = @1 order by CreateDate Desc", userId, isArbitrate);

            var items = new List<GetArbitrateMessageRespon>();
            list.ForEach(a =>
            {
                var reason = "";//原因
                var isArbitrate = IsEnum.否;//延期是否为仲裁员发起
                if (a.MessageType == MessageTypeEnum.申请延期)
                {
                    var model = db.SingleOrDefaultById<ArbitrateDelay>(a.AssociatedId);
                    if (model != null)
                    {
                        reason = Enum.GetName(model.Reason);
                        isArbitrate = model.IsArbitrate;
                    }
                }
                else if (a.MessageType == MessageTypeEnum.追加举证)
                {
                    var model = db.SingleOrDefaultById<AdduceList>(a.AssociatedId);
                    var info = db.SingleOrDefaultById<ArbitrateInfo>(model.ArbitrateInfoId);
                    if (model != null && info != null)
                    {
                        if (info.Defendant == model.AdduceUserId)
                            reason = "原告追加举证";
                        else if (info.Defendant == model.AdduceUserId)
                            reason = "被告追加举证";
                    }
                }
                else if (a.MessageType == MessageTypeEnum.仲裁取消)
                {
                    var model = db.SingleOrDefaultById<ArbitrateInfo>(a.AssociatedId);
                    if (model != null)
                    {
                        reason = Enum.GetName(model.CancelReason);
                    }
                }
                
                items.Add(new GetArbitrateMessageRespon
                {
                    ArbitrateMessageId = a.ArbitrateMessageId,
                    AssociatedId = a.AssociatedId,
                    CreateDate = a.CreateDate,
                    IsOpen = a.IsOpen,
                    MessageType = a.MessageType,
                    Reason = reason
                });
            });

            return InvokeResult.Success(items);
        }

        /// <summary>
        /// 获取原被告延期消息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<GetArbitrateDelayRespon>> GetArbitrateDelay(string userId, string id, IsEnum isArbitrate)
        {
            using var db = new NDatabase();

            var model = await db.SingleOrDefaultByIdAsync<ArbitrateDelay>(id);
            var info = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(model.ArbitrateInfoId);
            var delayVote = await db.SingleOrDefaultAsync<DelayVote>("select * from DelayVote where ArbitrateDelayId = @0 and VoteUserId = @1", id, userId);
            var respon = new GetArbitrateDelayRespon
            {
                ArbitrateInfoId = model.ArbitrateInfoId,
                ArbitrateInType = info.ArbitrateInType,
                Days = model.Days,
                Defendant = WalletHelp.GetName(info.Defendant),
                DefendantId = info.Defendant,
                Plaintiff = WalletHelp.GetName(info.Plaintiff),
                PlaintiffId = info.Plaintiff,
                DelayUserId = model.DelayUserId,
                Explain = model.Explain,
                Reason = Enum.GetName(model.Reason),
                Status = delayVote?.Status,
                DelayVoteId = delayVote?.DelayVoteId,
                DelayStatus = model.Status
            };

            if (isArbitrate == IsEnum.是)
            {
                respon.Name = WalletHelp.GetName(model.DelayUserId);
                respon.Number = db.SingleOrDefault<string>("select Number from UserArbitrate where DIDUserId = @0 and IsDelete = 0", model.DelayUserId);
            }

            return InvokeResult.Success(respon);
        }

        /// <summary>
        /// 获取取消仲裁消息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<GetCancelArbitrateRespon>> GetCancelArbitrate(string id)
        {
            using var db = new NDatabase();
            var info = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(id);
            var respon = new GetCancelArbitrateRespon
            {
                ArbitrateInfoId = id,
                ArbitrateInType = info.ArbitrateInType,
                Defendant = WalletHelp.GetName(info.Defendant),
                DefendantId = info.Defendant,
                Plaintiff = WalletHelp.GetName(info.Plaintiff),
                PlaintiffId = info.Plaintiff,
                Reason = Enum.GetName(info.CancelReason)
            };

            return InvokeResult.Success(respon);
        }

        /// <summary>
        /// 获取追加举证消息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<GetAdduceListRespon>> GetAdduceList(string id)
        {
            using var db = new NDatabase();
            var model = await db.SingleOrDefaultByIdAsync<AdduceList>(id);
            var info = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(model.ArbitrateInfoId);
            var respon = new GetAdduceListRespon
            {
                ArbitrateInfoId = model.ArbitrateInfoId,
                ArbitrateInType = info.ArbitrateInType,
                Defendant = WalletHelp.GetName(info.Defendant),
                DefendantId = info.Defendant,
                Plaintiff = WalletHelp.GetName(info.Plaintiff),
                PlaintiffId = info.Plaintiff,
                AdduceUserId = model.AdduceUserId,
                Images = model.Images,
                Memo = model.Memo
            };

            return InvokeResult.Success(respon);
        }

        /// <summary>
        /// 获取结案通知
        /// </summary>
        /// <returns></returns>
        public async Task<Response<GetClosureRespon>> GetClosure(string id)
        {
            using var db = new NDatabase();
            var info = await db.SingleOrDefaultByIdAsync<ArbitrateInfo>(id);
            var respon = new GetClosureRespon
            {
                ArbitrateInfoId = info.ArbitrateInfoId,
                Status = info.Status,
                Defendant = WalletHelp.GetName(info.Defendant),
                Plaintiff = WalletHelp.GetName(info.Plaintiff),
                DefendantNum = await db.SingleOrDefaultAsync<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", id, VoteStatusEnum.被告胜),
                PlaintiffNum = await db.SingleOrDefaultAsync<int>("select count(*) from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", id, VoteStatusEnum.原告胜),
                PlaintiffId = info.Plaintiff,
                DefendantId = info.Defendant,
                EOTC = 100,
                VoteDate = info.VoteDate
            };

            return InvokeResult.Success(respon);
        }

        /// <summary>
        /// 设置消息为已读
        /// </summary>
        /// <returns></returns>
        public async Task<Response> SetMessageIsOpen(string messageId)
        {
            using var db = new NDatabase();
            var info = await db.SingleOrDefaultByIdAsync<ArbitrateMessage>(messageId);
            if(null == info)
                return InvokeResult.Fail("消息未找到!");
            info.IsOpen = IsEnum.是;
            await db.UpdateAsync(info);
            return InvokeResult.Success("设置成功!");
        }

        /// <summary>
        /// 获取是否有未读消息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<int>> GetMessageIsOpen(string userId, IsEnum isArbitrate)
        {
            using var db = new NDatabase();
            var info = await db.SingleOrDefaultAsync<int>("select count(*) from ArbitrateMessage where UserId = @0 and IsOpen = 0 and IsArbitrate = @1", userId, isArbitrate);

            //if(info.Count > 0)
            //    return InvokeResult.Success(true);
            return InvokeResult.Success(info);
        }

        /// <summary>
        /// 获取被告待处理消息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<int>> GetWaitMessage(string userId)
        {
            using var db = new NDatabase();
            var info = await db.SingleOrDefaultAsync<int>("select count(*) from ArbitrateInfo where Status = 2 and Defendant = @0 and IsCancel = 0", userId);

            return InvokeResult.Success(info);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="type"></param>
        /// <param name="associatedId"></param>
        /// <param name="isArbitrate"></param>
        public async void SendMessage(List<string> userIds, MessageTypeEnum type, string associatedId, IsEnum isArbitrate)
        {
            using var db = new NDatabase();
            var models = userIds.Select(a => new ArbitrateMessage
            {
                ArbitrateMessageId = Guid.NewGuid().ToString(),
                AssociatedId = associatedId,
                CreateDate = DateTime.Now,
                MessageType = type,
                UserId = a,
                IsOpen = IsEnum.否,
                IsArbitrate = isArbitrate
            }).ToList();
            if(models.Count > 0)
                await db.InsertBatchAsync(models);
        }
    }
}