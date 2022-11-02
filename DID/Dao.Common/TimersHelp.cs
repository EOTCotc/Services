using Dao.Entity;
using DID.Common;
using DID.Entitys;
using DID.Models.Request;
using DID.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Common
{
    public class TimersHelp
    {
        private static ILogger<TimersHelp> _logger = IocManager.Resolve<ILogger<TimersHelp>>();

        private static ICreditScoreService _csservice = IocManager.Resolve<ICreditScoreService>();

        private static IRewardService _reservice = IocManager.Resolve<IRewardService>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="csservice"></param>
        //public TimersHelp(ILogger<TimersHelp> logger, ICreditScoreService csservice, IRewardService reservice)
        //{
        //    _logger = logger;
        //    _csservice = csservice;
        //    _reservice = reservice;
        //}

        /// <summary>
        /// 仲裁定时器(举证)
        /// </summary>
        public static void ArbitrateAdduceTimer(Timers timer)
        {
            if (timer.StartTime <= DateTime.Now && DateTime.Now <= timer.EndTime)
            {
                var t = new System.Timers.Timer((timer.EndTime - DateTime.Now).TotalMilliseconds);

                t.Elapsed += new System.Timers.ElapsedEventHandler((object? source, System.Timers.ElapsedEventArgs e) =>
                {
                    t.Stop(); //先关闭定时器      

                    using var db = new NDatabase();
                    var item = db.SingleOrDefaultById<ArbitrateInfo>(timer.Rid);

                    if (DateTime.Now >= item.AdduceDate)
                    {

                        item.Status = ArbitrateStatusEnum.投票中;
                        db.Update(item);
                        //投票三天
                        var voteHours = Convert.ToInt32(_reservice.GetRewardValue("VoteHours").Result.Items);
                        var voteTimer = new Timers { 
                            TimersId = Guid.NewGuid().ToString(), 
                            Rid = timer.Rid, CreateDate = DateTime.Now, 
                            StartTime = DateTime.Now, 
                            EndTime = DateTime.Now.AddHours(voteHours),
                            //EndTime = DateTime.Now.AddMinutes(5),
                            TimerType = TimerTypeEnum.仲裁投票 
                        };
                        db.Insert(voteTimer);
                        ArbitrateVoteTimer(voteTimer);
                    }
                    else
                    {
                        //var time = (int)(item.AdduceDate - DateTime.Now).TotalMilliseconds;
                        //ToDelay(arbitrateInfoId, time);
                        //举证时间延长
                        var adduceTimer = new Timers { 
                            TimersId = Guid.NewGuid().ToString(),
                            Rid = timer.Rid,
                            CreateDate = DateTime.Now, 
                            StartTime = DateTime.Now,
                            EndTime = item.AdduceDate,
                            TimerType = TimerTypeEnum.仲裁举证
                        };
                        db.Insert(adduceTimer);
                        ArbitrateAdduceTimer(adduceTimer);
                    }
                });//到达时间的时候执行事件；
                t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                t.Start(); //启动定时器
            }
        }
        /// <summary>
        /// 仲裁定时器(投票)
        /// </summary>
        public static void ArbitrateVoteTimer(Timers timer)
        {
            if (timer.StartTime <= DateTime.Now && DateTime.Now <= timer.EndTime)
            {
                var t = new System.Timers.Timer((timer.EndTime - DateTime.Now).TotalMilliseconds);
                t.Elapsed += new System.Timers.ElapsedEventHandler((object? source, System.Timers.ElapsedEventArgs e) =>
                {
                    t.Stop(); //先关闭定时器
                    using var db = new NDatabase();
                    var item = db.SingleOrDefaultById<ArbitrateInfo>(timer.Rid);
                    var uservotes = db.Fetch<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and IsDelete = 0", timer.Rid);//全部投票信息
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

                    var votes = db.Fetch<ArbitrateVote>("select * from ArbitrateVote where ArbitrateInfoId = @0 and VoteStatus = @1 and IsDelete = 0", timer.Rid, VoteStatusEnum.未投票);

                    votes.ForEach(a =>
                    {
                        var userId = a.VoteUserId;
                        //todo: 扣分 扣EOTC
                        var user = db.SingleOrDefault<DIDUser>("select * from DIDUser where DIDUserId = @0", userId);
                        _csservice.CreditScore(new CreditScoreReq { Fraction = 3, Remarks = "仲裁到时未投票", Type = TypeEnum.减分, Uid = user.Uid });

                        a.IsDelete = IsEnum.是;
                        db.Update(a);

                        //更新延期状态
                        var delay = db.SingleOrDefault<ArbitrateDelay>("select * from ArbitrateDelay where IsArbitrate = 1 and ArbitrateInfoId = @0", timer.Rid);
                        if (null != delay)
                        {
                            delay.Status = 2;
                            db.Update(delay);
                        }
                    });

                    //发送结案通知
                    if (item.Status == ArbitrateStatusEnum.原告胜 || item.Status == ArbitrateStatusEnum.被告胜)
                    {
                        SendMessage(new List<string> { item.Plaintiff, item.Defendant }, MessageTypeEnum.结案通知, timer.Rid, IsEnum.否);
                        SendMessage(uservotes.Where(a => a.IsDelete == IsEnum.否).Select(a => a.ArbitrateInfoId).ToList(), MessageTypeEnum.结案通知, timer.Rid, IsEnum.是);

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
                        var allvotes = db.Fetch<string>("select VoteUserId from ArbitrateVote where ArbitrateInfoId = @0", timer.Rid);
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
                        //投票三天
                        var voteHours = Convert.ToInt32(_reservice.GetRewardValue("VoteHours").Result.Items);
                        item.VoteDate = item.VoteDate.AddHours(voteHours);//投票时间延长默认3天
                        //item.VoteDate = item.VoteDate.AddMinutes(5);//投票时间延长3天
                        db.Update(item);
                        //重新启动投票定时器
                        
                        var voteTimer = new Timers
                        {
                            TimersId = Guid.NewGuid().ToString(),
                            Rid = timer.Rid,
                            CreateDate = DateTime.Now,
                            StartTime = DateTime.Now,
                            EndTime = DateTime.Now.AddHours(voteHours),
                            //EndTime = DateTime.Now.AddMinutes(5),
                            TimerType = TimerTypeEnum.仲裁投票
                        };
                        db.Insert(voteTimer);   
                        t.AutoReset = false;
                        t.Enabled = true;
                        t.Start();
                    }

                    db.Update(item);
                });
                t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；
                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                t.Start(); //启动定时器
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="type"></param>
        /// <param name="associatedId"></param>
        /// <param name="isArbitrate"></param>
        public static void SendMessage(List<string> userIds, MessageTypeEnum type, string associatedId, IsEnum isArbitrate)
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
            if (models.Count > 0)
                db.InsertBatch(models);
        }
    }
}
