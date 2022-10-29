using DID.Entitys;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Entity
{
    /// <summary>
    ///  投票状态 0 未投票 1 原告胜 2 被告胜
    /// </summary>
    public enum VoteStatusEnum { 未投票, 原告胜, 被告胜 }
    /// <summary>
    /// 仲裁投票表
    /// </summary>
    [PrimaryKey("ArbitrateVoteId", AutoIncrement = false)]
    public class ArbitrateVote
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ArbitrateVoteId
        {
            get; set;
        }
        /// <summary>
        /// 仲裁编号
        /// </summary>
        public string ArbitrateInfoId
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
        /// 投票状态 0 未投票 1 原告胜 2 被告胜
        /// </summary>
        public VoteStatusEnum VoteStatus
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

        /// <summary>
        /// 原因
        /// </summary>
        public string? Reason
        {
            get; set;
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        public IsEnum IsDelete
        {
            get; set;
        }
    }
}
