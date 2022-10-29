using NPoco;

namespace DID.Entitys
{
    /// <summary>
    /// 钱包
    /// </summary>
    [PrimaryKey("WalletId", AutoIncrement = false)]
    public class Wallet
    {

        /// <summary>
        /// 编号
        /// </summary>
        public string WalletId
        {
            get; set;
        }
        /// <summary>
        /// 钱包地址
        /// </summary>
        public string WalletAddress
        {
            get; set;
        }
        /// <summary>
        /// 网络类型
        /// </summary>
        public string Otype
        {
            get; set;
        }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign
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
        /// 绑定时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }

        /// <summary>
        /// 是否注销
        /// </summary>
        public IsEnum IsLogout
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
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteDate
        {
            get; set;
        }
    }
}

