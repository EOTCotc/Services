using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class ArbitrateVoteReq : DaoBaseReq
    {
        /// <summary>
        /// 仲裁编号
        /// </summary>
        public string ArbitrateInfoId
        {
            get; set;
        }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get; set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public VoteStatusEnum Status
        {
            get; set;
        }
    }
}
