using Dao.Entity;
using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetArbitrateDetailsRespon
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ArbitrateInfoId
        {
            get; set;
        }

        /// <summary>
        /// 原告
        /// </summary>
        public string Plaintiff
        {
            get; set;
        }

        /// <summary>
        /// 原告编号
        /// </summary>
        public string PlaintiffId
        {
            get; set;
        }

        /// <summary>
        /// 原告Uid
        /// </summary>
        public string PlaintiffUId
        {
            get; set;
        }

        /// <summary>
        /// 被告Uid
        /// </summary>
        public string DefendantUId
        {
            get; set;
        }

        /// <summary>
        /// 被告
        /// </summary>
        public string Defendant
        {
            get; set;
        }

        /// <summary>
        /// 被告编号
        /// </summary>
        public string DefendantId
        {
            get; set;
        }

        /// <summary>
        /// 原告票数
        /// </summary>
        public int PlaintiffNum
        {
            get; set;
        }

        /// <summary>
        /// 被告票数
        /// </summary>
        public int DefendantNum
        {
            get; set;
        }

        /// <summary>
        /// 结案时间
        /// </summary>
        public DateTime VoteDate
        {
            get; set;
        }

        /// <summary>
        /// 投票记录
        /// </summary>
        public List<Vote>? Votes
        {
            get; set;
        }

        /// <summary>
        /// 举证截至日期
        /// </summary>
        public DateTime AdduceDate
        {
            get; set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }

        /// <summary>
        /// 状态 0 举证中 1 投票中 2 取消 3 原告胜 4 被告胜
        /// </summary>
        public ArbitrateStatusEnum Status
        {
            get; set;
        }

        /// <summary>
        /// 举证记录
        /// </summary>
        public List<AdduceList> Adduce
        {
            get; set;
        }
        /// <summary>
        /// 判决原因
        /// </summary>
        public string Reason
        {
            get; set;
        }
        /// <summary>
        /// 当前仲裁员投票记录
        /// </summary>
        public Vote? UserVote
        {
            get; set;
        }

        /// <summary>
        /// 奖励或扣除eotc数量
        /// </summary>
        public double? EOTC
        {
            get; set;
        }

        /// <summary>
        /// 当前用户是否延期
        /// </summary>
        public IsEnum HasDelay
        {
            get; set;
        }

        /// <summary>
        /// 是否取消
        /// </summary>
        public IsEnum IsCancel
        {
            get; set;
        }


        /// <summary>
        /// 仲裁事件 0 账户被冻结 1 卖家未确认收款 2 其他
        /// </summary>
        public ArbitrateInTypeEnum ArbitrateInType
        {
            get; set;
        }

        /// <summary>
        /// 投票状态 0 未投票 1 原告胜 2 被告胜
        /// </summary>
        public VoteStatusEnum? VoteStatus
        {
            get; set;
        }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId
        {
            get; set;
        }
    }

    public class Vote
    { 
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 仲裁员编号
        /// </summary>
        public string Number
        {
            get; set;
        }
        /// <summary>
        /// 投票状态 0 未投票 1 原告胜 2 被告胜
        /// </summary>
        public VoteStatusEnum VoteStatus
        {
            get; set;
        }

        /// <summary>
        /// 判决原因
        /// </summary>
        public string? Reason
        {
            get; set;
        }
    }


}
