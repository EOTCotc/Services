using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Entity
{
    /// <summary>
    /// 仲裁支付表
    /// </summary>
    [PrimaryKey("ArbitratePayId", AutoIncrement = false)]
    public class ArbitratePay
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string? ArbitratePayId
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
        /// 用户编号
        /// </summary>
        public string PayUserId
        {
            get; set;
        }
        /// <summary>
        /// eotc数量
        /// </summary>
        public double EOTC
        {
            get; set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark
        {
            get; set;
        }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayDate
        {
            get; set;
        }
    }
}
