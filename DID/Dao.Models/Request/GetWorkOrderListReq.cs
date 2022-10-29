using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class GetWorkOrderListReq : DaoBaseReq
    {
        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderStatusEnum WorkOrderStatus
        {
            get; set;
        }
        /// <summary>
        /// 类型 0=bug反馈 1=功能建议 不传全部
        /// </summary>
        public WorkOrderTypeEnum? WorkOrderType
        {
            get; set;
        }
        /// <summary>
        /// 页数
        /// </summary>
        public long Page
        {
            get; set;
        }
        /// <summary>
        /// 每页数量
        /// </summary>
        public long ItemsPerPage
        {
            get; set;
        }
    }
}
