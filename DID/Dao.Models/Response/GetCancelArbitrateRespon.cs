using Dao.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetCancelArbitrateRespon
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
        /// 取消原因 0 与被告方达成和解 1 单方面撤诉
        /// </summary>
        public string? Reason
        {
            get; set;
        }
        
    }
}
