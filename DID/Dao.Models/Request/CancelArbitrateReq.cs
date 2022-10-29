using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class CancelArbitrateReq : DaoBaseReq
    {
        /// <summary>
        /// 仲裁编号
        /// </summary>
        public string ArbitrateInfoId
        {
            get; set;
        }
        /// <summary>
        /// 取消原因 0 与被告方达成和解 1 单方面撤诉
        /// </summary>
        public CancelReasonEnum CancelReason
        {
            get; set;
        }
    }
}
