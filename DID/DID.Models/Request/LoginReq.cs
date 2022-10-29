namespace DID.Models.Request
{
    /// <summary>
    /// 登录参数
    /// </summary>
    public class LoginReq
    {
        /// <summary>
        /// 钱包地址
        /// </summary>
        public string? WalletAddress
        {
            get; set;
        }
        /// <summary>
        /// 网络类型
        /// </summary>
        public string? Otype
        {
            get; set;
        }
        /// <summary>
        /// 签名
        /// </summary>
        public string? Sign
        {
            get; set;
        }
        /// <summary>
        /// 推荐人
        /// </summary>
        public string? RefUserId
        {
            get; set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Mail
        {
            get; set;
        }
        /// <summary>
        /// 验证码
        /// </summary>
        public string? Code
        {
            get; set;
        }

        /// <summary>
        /// 密码（英文、数字 至少6位）
        /// </summary>
        public string? Password
        {
            get; set;
        }
    }
}
