using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Entity
{
    /// <summary>
    /// /// <summary>
    /// 类型 0=bug反馈 1=功能建议 
    /// </summary>
    public enum WorkOrderTypeEnum { bug反馈 , 功能建议 }
    /// <summary>
    /// 状态 选项：0=待处理 1=处理中 2=已处理
    /// </summary>
    public enum WorkOrderStatusEnum {待处理, 处理中 ,已处理 }

    [PrimaryKey("WorkOrderId", AutoIncrement = false)]
    public class WorkOrder
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string WorkOrderId
        {
            get; set; 
        }
        /// <summary>
        /// 钱包编号
        /// </summary>
        public string WalletId
        {
            get; set;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string DIDUserId
        {
            get; set;
        }
        /// <summary>
        /// 类型 0=bug反馈 1=功能建议 
        /// </summary>
        public WorkOrderTypeEnum WorkOrderType
        {
            get; set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe
        {
            get; set;
        }
        /// <summary>
        /// 图片
        /// </summary>
        public string Images
        {
            get; set;
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone
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
        /// 状态 选项：0=待处理 1=处理中 2=已处理
        /// </summary>
        public WorkOrderStatusEnum WorkOrderStatus
        {
            get; set;
        }
        /// <summary>
        /// 处理人钱包编号
        /// </summary>
        public string HandleWalletId
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
