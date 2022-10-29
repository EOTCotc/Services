using Dao.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetClosureRespon
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
        /// 状态 0 举证中 1 投票中 2 原告胜 3 被告胜
        /// </summary>
        public ArbitrateStatusEnum Status
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
        /// 投票截止日期
        /// </summary>
        public DateTime VoteDate
        {
            get; set;
        }
    }
}
