namespace DID.Models.Request
{
    public class ChangeMailReq
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail
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
        /// 验证码
        /// </summary>
        public string Code
        {
            get; set;
        }
    }
}
