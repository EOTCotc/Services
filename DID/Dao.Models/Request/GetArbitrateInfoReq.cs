using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class GetArbitrateInfoReq : DaoBaseReq
    {
        /// <summary>
        /// 原告
        /// </summary>
        public string Plaintiff
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
        /// 订单编号
        /// </summary>
        public string OrderId
        {
            get; set;
        }
        /// <summary>
        /// 仲裁人数
        /// </summary>
        public int Num
        {
            get; set;
        }
        /// <summary>
        /// 文字举证
        /// </summary>
        public string Memo
        {
            get; set;
        }
        /// <summary>
        /// 图片举证
        /// </summary>
        public string Images
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


    }
}
