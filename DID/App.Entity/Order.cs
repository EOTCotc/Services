using DID.Entitys;
using NPoco;

namespace App.Entity
{
    /// <summary>
    /// 状态 0 待支付 1 已支付 2 已取消 
    /// </summary>
    public enum StatusEnum { 待支付, 已支付, 已取消 }
    /// <summary>
    /// 类型 0 课程 1 系统
    /// </summary>
    public enum OrderTypeEnum { 课程, 系统 }
    /// <summary>
    /// 订单表
    /// </summary>
    [TableName("App_Order")]
    [PrimaryKey("OrderId", AutoIncrement = false)]
    public class Order
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string OrderId
        {
            get; set;
        }
        /// <summary>
        /// 关联编号
        /// </summary>
        public string Rid
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
        /// 类型 0 课程 1 系统
        /// </summary>
        public OrderTypeEnum OrderType
        {
            get; set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone
        {
            get; set;
        }
        /// <summary>
        /// 微信号
        /// </summary>
        public string Wechat
        {
            get; set;
        }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PaymentDate
        {
            get; set;
        }
        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime? CancelDate
        {
            get; set;
        }
        /// <summary>
        /// 状态 0 待支付 1 已支付 2 已取消 
        /// </summary>
        public StatusEnum Status
        {
            get; set;
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity
        {
            get; set;
        }
        /// <summary>
        /// 自愿者编号
        /// </summary>
        public string? VolunteerId
        {
            get; set;
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public IsEnum IsDelete
        {
            get; set;
        }
    }
}