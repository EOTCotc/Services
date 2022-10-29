using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class WorkOrderStatusReq : DaoBaseReq
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string WorkOrderId
        {
            get; set;
        }
        /// <summary>
        /// 状态 选项：0=待处理 1=处理中 2=已处理
        /// </summary>
        public WorkOrderStatusEnum WorkOrderStatus
        {
            get; set;
        }
        /// <summary>
        /// 处理记录
        /// </summary>
        public string? Record
        {
            get; set;
        }
    }
}
