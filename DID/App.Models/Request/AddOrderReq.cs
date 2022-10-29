using App.Entity;

namespace App.Models.Request
{
    public class AddOrderReq
    {
        /// <summary>
        /// 关联编号
        /// </summary>
        public string Rid
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
        /// 数量
        /// </summary>
        public int Quantity
        {
            get; set;
        }
    }
}