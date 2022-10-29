using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class ArbitrateDelayVoteReq : DaoBaseReq
    {
        /// <summary>
        /// 延期投票编号
        /// </summary>
        public string DelayVoteId
        {
            get; set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public DelayVoteStatus Status
        {
            get; set;
        }

    }
}
