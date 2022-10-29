using Dao.Entity;
using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetArbitrateInfoRespon
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
        /// 被告
        /// </summary>
        public string Defendant
        {
            get; set;
        }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId
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
        /// 被告Uid
        /// </summary>
        public string DefendantUId
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
        /// 状态 0 举证中 1 投票中 2 原告胜 3 被告胜
        /// </summary>
        public ArbitrateStatusEnum Status
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
        /// 投票截止日期
        /// </summary>
        public DateTime VoteDate
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

        ///// <summary>
        ///// false 败诉 true 胜诉
        ///// </summary>
        //public bool IsVictory
        //{
        //    get; set;
        //}
        /// <summary>
        /// 投票状态 0 未投票 1 原告胜 2 被告胜
        /// </summary>
        public VoteStatusEnum VoteStatus
        {
            get; set;
        }

        /// <summary>
        /// 奖励或扣除EOTC数量
        /// </summary>
        public double EOTC
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
        /// 是否提交延期申请
        /// </summary>
        public IsEnum HasDelay
        {
            get; set;
        }
    }
}
