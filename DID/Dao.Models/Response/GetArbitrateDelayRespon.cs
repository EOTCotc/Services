using Dao.Entity;
using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetArbitrateDelayRespon
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
        /// 被告编号
        /// </summary>
        public string DefendantId
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
        /// 仲裁事件 0 账户被冻结 1 卖家未确认收款 2 其他
        /// </summary>
        public ArbitrateInTypeEnum ArbitrateInType
        {
            get; set;
        }

        /// <summary>
        /// 申请人编号
        /// </summary>
        public string DelayUserId
        {
            get; set;
        }

        /// <summary>
        /// 原因 0  举证时间不足 1 核实信息还在审核中 （仲裁员）2 举证不足,无法进行判决 3 部分举证不全
        /// </summary>
        public string? Reason
        {
            get; set;
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string? Explain
        {
            get; set;
        }
        /// <summary>
        /// 延期天数
        /// </summary>
        public int Days
        {
            get; set;
        }

        /// <summary>
        /// 发起人姓名
        /// </summary>
        public string? Name
        {
            get; set;
        }
        /// <summary>
        /// 仲裁员编号
        /// </summary>
        public string? Number
        {
            get; set;
        }

        /// <summary>
        /// 状态 0 未投票 1 不同意 2 同意
        /// </summary>
        public DelayVoteStatus? Status
        {
            get; set;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string? DelayVoteId
        {
            get; set;
        }

        /// <summary>
        /// 是否延期成功
        /// </summary>
        public int DelayStatus
        {
            get; set;
        }

    }
}
