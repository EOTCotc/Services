
using Dao.Entity;
using DID.Entitys;
using Microsoft.Extensions.Logging;

namespace DID.Common
{
    public class TimersHelp
    {
        private static ILogger<TimersHelp> _logger = IocManager.Resolve<ILogger<TimersHelp>>();

        public TimersHelp(ILogger<TimersHelp> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 注销定时器
        /// </summary>
        public static void LogoutTimer(Timers timer)
        {
            if (timer.StartTime <= DateTime.Now && DateTime.Now <= timer.EndTime)
            {
                System.Timers.Timer t = new((timer.EndTime - DateTime.Now).TotalMilliseconds);

                t.Elapsed += new System.Timers.ElapsedEventHandler((object? source, System.Timers.ElapsedEventArgs e) =>
                {
                    t.Stop(); //先关闭定时器
                    using var db = new NDatabase();
                    var nowItem = db.SingleOrDefaultById<UserLogout>(timer.Rid);
                    if (nowItem.IsCancel == IsEnum.否)
                    {
                        nowItem.LogoutDate = DateTime.Now;
                        db.Update(nowItem);
                        db.Execute("update DIDUser set IsLogout = @0 where DIDUserId = @1", IsEnum.是, nowItem.DIDUserId);//注销账号
                        db.Execute("update Wallet set IsLogout = @0 where DIDUserId = @1", IsEnum.是, nowItem.DIDUserId);//注销钱包
                        var refUserId = db.SingleOrDefault<string>("select RefUserId from DIDUser where DIDUserId = @0 and IsLogout = 0", nowItem.DIDUserId);
                        if (!string.IsNullOrEmpty(refUserId))
                            db.Execute("update DIDUser set RefUserId = @0 where RefUserId = @1 and IsLogout = 0", refUserId, nowItem.DIDUserId);//更新邀请人为当前用户的上级
                    }
                });//到达时间的时候执行事件；
                t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                t.Start(); //启动定时器
            }
        }

        /// <summary>
        /// 身份审核定时器
        /// </summary>
        public static void AuthTimer(Timers timer)
        {
            if (timer.StartTime <= DateTime.Now && DateTime.Now <= timer.EndTime)
            {
                var t = new System.Timers.Timer((timer.EndTime - DateTime.Now).TotalMilliseconds);
                t.Elapsed += new System.Timers.ElapsedEventHandler((object? source, System.Timers.ElapsedEventArgs e) =>
                {

                    t.Stop(); //先关闭定时器
                              //todo: Dao审核
                    using var db = new NDatabase();
                    var item = db.SingleOrDefaultById<Auth>(timer.Rid);
                    if (null == item)
                        return;
                    var authinfo = db.SingleOrDefault<UserAuthInfo>("select * from UserAuthInfo where UserAuthInfoId = @0", item.UserAuthInfoId);
                    if (null == authinfo)
                        return;
                    var users = db.Fetch<string>("select AuditUserId from Auth where UserAuthInfoId = @0", item.UserAuthInfoId);


                    if (item.AuditType == AuditTypeEnum.未审核)
                    {
                        item.IsDelete = IsEnum.是;
                        db.Update(item);

                        //随机Dao审核员审核
                        var userIds = db.Fetch<DIDUser>("select * from DIDUser where DIDUserId != @0 and IsExamine = 1 and IsLogout = 0 and IsEnable = 1 and DIDUserId not in (@1)", authinfo.CreatorId!, users);

                        var random = new Random().Next(userIds.Count);
                        var auditUserId = userIds[random].DIDUserId;

                        item.AuthId = Guid.NewGuid().ToString();
                        item.IsDao = IsEnum.是;
                        item.AuditUserId = auditUserId;//Dao在线节点用户编号
                        item.CreateDate = DateTime.Now;
                        item.IsDelete = IsEnum.否;
                        db.Insert(item);
                    }
                });//到达时间的时候执行事件；
                t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                t.Start(); //启动定时器
            }
        }

        /// <summary>
        /// 团队审核定时器
        /// </summary>
        public static void ComAuthTimer(Timers timer)
        {
            if (timer.StartTime <= DateTime.Now && DateTime.Now <= timer.EndTime)
            {
                var t = new System.Timers.Timer((timer.EndTime - DateTime.Now).TotalMilliseconds);
                t.Elapsed += new System.Timers.ElapsedEventHandler((object? source, System.Timers.ElapsedEventArgs e) =>
                {
                    t.Stop(); //先关闭定时器
                              //todo: Dao审核
                    using var db = new NDatabase();
                    var item = db.SingleOrDefaultById<ComAuth>(timer.Rid);
                    if (null == item)
                        return;
                    var cominfo = db.SingleOrDefault<Community>("select * from Community where CommunityId = @0", item.CommunityId);
                    if (null == cominfo)
                        return;
                    var users = db.Fetch<string>("select AuditUserId from ComAuth where CommunityId = @0", item.CommunityId);

                    //随机Dao审核员审核
                    var userIds = db.Fetch<DIDUser>("select * from DIDUser where DIDUserId != @0 and IsExamine = 1 and IsLogout = 0 and IsEnable = 1 and DIDUserId not in (@1)", cominfo.DIDUserId, users);

                    var random = new Random().Next(userIds.Count);
                    var auditUserId = userIds[random].DIDUserId;

                    if (item.AuditType == AuditTypeEnum.未审核)
                    {
                        item.IsDelete = IsEnum.是;
                        db.Update(item);

                        item.ComAuthId = Guid.NewGuid().ToString();
                        item.IsDao = IsEnum.是;
                        item.AuditUserId = auditUserId;//Dao在线节点用户编号
                        item.CreateDate = DateTime.Now;
                        item.IsDelete = IsEnum.否;
                        db.Insert(item);
                    }
                });//到达时间的时候执行事件；
                t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                t.Start(); //启动定时器
            }
        }

        ///// <summary>
        ///// 仲裁定时器(举证)
        ///// </summary>
        //public void ArbitrateAdduceTimer(Timers timer)
        //{
        //    if (timer.StartTime <= DateTime.Now && DateTime.Now <= timer.EndTime)
        //    {
        //        var t = new System.Timers.Timer((timer.EndTime - DateTime.Now).TotalMilliseconds);

        //        t.Elapsed += new System.Timers.ElapsedEventHandler((object? source, System.Timers.ElapsedEventArgs e) =>
        //        {
        //            t.Stop(); //先关闭定时器      

        //            using var db = new NDatabase();
        //            var item = db.SingleOrDefaultById<ArbitrateInfo>(timer.Rid);

        //            if (DateTime.Now >= item.AdduceDate)
        //            {

        //                item.Status = ArbitrateStatusEnum.投票中;
        //                db.Update(item);
                        
        //            }
        //            else
        //            {
        //                var time = (int)(item.AdduceDate - DateTime.Now).TotalMilliseconds;
        //                ToDelay(arbitrateInfoId, time);
        //            }
        //        });//到达时间的时候执行事件；
        //        t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
        //        t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        //        t.Start(); //启动定时器
        //    }
        //}
        ///// <summary>
        ///// 仲裁定时器(投票)
        ///// </summary>
        //public void ArbitrateVoteTimer(Timers timer)
        //{
        //    if (timer.StartTime <= DateTime.Now && DateTime.Now <= timer.EndTime)
        //    {
        //        var t = new System.Timers.Timer((timer.EndTime - DateTime.Now).TotalMilliseconds);
        //        t.Elapsed += new System.Timers.ElapsedEventHandler((object? source, System.Timers.ElapsedEventArgs e) =>
        //        {
        //            t.Stop(); //先关闭定时器
        //            using var db = new NDatabase();
        //            var item = db.SingleOrDefaultById<ArbitrateInfo>(timer.Rid);
        //            var uservotes = db.Fetch<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and IsDelete = 0", timer.Rid);//全部投票信息
        //            //原告票数
        //            var ynum = uservotes.Where(a => a.VoteStatus == VoteStatusEnum.原告胜).Count();
        //            //被告票数
        //            var bnum = uservotes.Where(a => a.VoteStatus == VoteStatusEnum.被告胜).Count();

        //            //原告胜
        //            if (ynum > item.Number / 2)
        //            {
        //                item.Status = ArbitrateStatusEnum.原告胜;
        //            }

        //            //被告胜
        //            if (bnum > item.Number / 2)
        //            {
        //                item.Status = ArbitrateStatusEnum.被告胜;
        //            }

        //            var votes = db.Fetch<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", arbitrateInfoId, VoteStatusEnum.未投票);

        //            votes.ForEach(a =>
        //            {
        //                var userId = a.VoteUserId;
        //                //todo: 扣分 扣EOTC
        //                var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
        //                _csservice.CreditScore(new CreditScoreReq { Fraction = 3, Remarks = "仲裁到时未投票", Type = TypeEnum.减分, Uid = user.Uid });

        //                a.IsDelete = IsEnum.是;
        //                db.Update(a);

        //                //更新延期状态
        //                var delay = db.SingleOrDefault<ArbitrateDelay>("select * from ArbitrateDelay where IsArbitrate = 1 and ArbitrateInfoId = @0", arbitrateInfoId);
        //                if (null != delay)
        //                {
        //                    delay.Status = 2;
        //                    db.Update(delay);
        //                }

        //            });

        //            //发送结案通知
        //            if (item.Status == ArbitrateStatusEnum.原告胜 || item.Status == ArbitrateStatusEnum.被告胜)
        //            {
        //                SendMessage(new List<string> { item.Plaintiff, item.Defendant }, MessageTypeEnum.结案通知, arbitrateInfoId, IsEnum.否);
        //                SendMessage(uservotes.Where(a => a.IsDelete == IsEnum.否).Select(a => a.ArbitrateInfoId).ToList(), MessageTypeEnum.结案通知, arbitrateInfoId, IsEnum.是);

        //                //仲裁员仲裁胜利次数+1
        //                uservotes.ForEach(a =>
        //                {
        //                    if (a.VoteStatus == (item.Status == ArbitrateStatusEnum.原告胜 ? VoteStatusEnum.原告胜 : VoteStatusEnum.被告胜))
        //                    {
        //                        var model = db.SingleOrDefault<UserArbitrate>("select * from UserArbitrate where DIDUserId = @0 and IsDelete = 0", a.VoteUserId);
        //                        var eotc = _reservice.GetRewardValue("Arbitration").Result.Items;//奖励eotc数量
        //                        model.VictoryNum += 1;
        //                        model.EOTC += eotc;
        //                        db.Update(model);

        //                        //奖励EOTC
        //                        var detail = new IncomeDetails()
        //                        {
        //                            IncomeDetailsId = Guid.NewGuid().ToString(),
        //                            CreateDate = DateTime.Now,
        //                            EOTC = eotc,
        //                            Remarks = "仲裁处理",
        //                            Type = IDTypeEnum.处理仲裁,
        //                            DIDUserId = a.VoteUserId
        //                        };
        //                        var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", a.VoteUserId);

        //                        db.BeginTransaction();
        //                        db.Insert(detail);
        //                        user.DaoEOTC += eotc;
        //                        db.Update(user);
        //                        db.CompleteTransaction();
        //                    }
        //                });
        //            }

        //            //限时未出结果 未投票用户扣分 加入新人
        //            if (item.Status == ArbitrateStatusEnum.投票中)
        //            {
        //                var allvotes = db.Fetch<string>("select VoteUserId from ArbitrateVote where ArbitrateInfoId = @0", arbitrateInfoId);
        //                var list = new List<string>();
        //                var userIds = db.Fetch<DIDUser>("select * from DIDUser where DIDUserId != @0 and DIDUserId != @1 and IsArbitrate = 1 and IsLogout = 0 and IsEnable = 1 and DIDUserId not in (@2)",
        //                    item.Defendant, item.Plaintiff, allvotes);

        //                for (var i = 0; i < votes.Count; i++)
        //                {
        //                    var random = 0;
        //                    do
        //                    {
        //                        random = new Random().Next(userIds.Count);
        //                    } while (list.Exists(a => a == userIds[random].DIDUserId));
        //                    list.Add(userIds[random].DIDUserId);
        //                }
        //                list.ForEach(a =>
        //                {
        //                    //新的投票对象
        //                    var newvote = new ArbitrateVote
        //                    {
        //                        ArbitrateVoteId = Guid.NewGuid().ToString(),
        //                        ArbitrateInfoId = item.ArbitrateInfoId,
        //                        CreateDate = DateTime.Now,
        //                        VoteStatus = VoteStatusEnum.未投票,
        //                        VoteUserId = a
        //                    };

        //                    db.Insert(newvote);
        //                });

        //                //item.VoteDate = item.VoteDate.AddDays(3);//投票时间延长3天
        //                item.VoteDate = item.VoteDate.AddMinutes(5);//投票时间延长3天
        //                db.Update(item);
        //                //重新启动投票定时器
        //                t1.AutoReset = false;
        //                t1.Enabled = true;
        //                t1.Start();
        //            }

        //            db.Update(item);
        //        });
        //        t1.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
        //        t1.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        //        t1.Start(); //启动定时器
        //    }
        //}
    }
}
