using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Entity
{
    /// <summary>
    /// 状态 0 未投票 1 不同意 2 同意
    /// </summary>
    public enum DelayVoteStatus { 未投票 , 不同意, 同意 }
    [PrimaryKey("DelayVoteId", AutoIncrement = false)]
    public class DelayVote
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string DelayVoteId
        {
            get; set;
        }

        /// <summary>
        /// 仲裁延期编号
        /// </summary>
        public string ArbitrateDelayId
        {
            get; set;
        }

        /// <summary>
        /// 投票人编号
        /// </summary>
        public string VoteUserId
        {
            get; set;
        }

        /// <summary>
        /// 状态 0 未投票 1 不同意 2 同意
        /// </summary>
        public DelayVoteStatus Status
        {
            get; set;
        }

        /// <summary>
        /// 创建日期 默认为当前时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }

        /// <summary>
        /// 投票日期
        /// </summary>
        public DateTime? VoteDate
        {
            get; set;
        }
    }
}
