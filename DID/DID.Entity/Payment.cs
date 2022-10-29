using NPoco;

namespace DID.Entitys
{
    /// <summary>
    /// 类型 0 现金支付 1 银行卡 2 支付宝 3 微信支付
    /// </summary>
    public enum PayType { 现金支付, 银行卡, 支付宝, 微信支付 }
    /// <summary>
    ///  0 否 1 是
    /// </summary>
    public enum IsEnum { 否, 是 }

    /* 收付款信息 */
    [PrimaryKey("PaymentId", AutoIncrement = false)]
    public class Payment
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string? PaymentId
        {
            get; set;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string? DIDUserId
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name
        {
            get; set;
        }
        /// <summary>
        /// 开户银行
        /// </summary>
        public string? Bank
        {
            get; set;
        }
        /// <summary>
        /// 卡号
        /// </summary>
        public string? CardNum
        {
            get; set;
        }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string? PassWord
        {
            get; set;
        }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// </summary>
        public DateTime? CreateDate
        {
            get; set;
        }
        /// <summary>
        /// 更新日期 默认为当前时间
        /// </summary>
        public DateTime? UpdateDate
        {
            get; set;
        }
        /// <summary>
        /// 类型 0 现金支付 1 银行卡 2 支付宝 3 微信支付
        /// </summary>
        public PayType Type
        {
            get; set;
        }
        /// <summary>
        /// 是否删除 0 否 1 是
        /// </summary>
        public IsEnum IsDelete
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
        /// 是否启用 0 否 1 是
        /// </summary>
        public IsEnum IsEnable
        {
            get; set;
        }
    }
}

