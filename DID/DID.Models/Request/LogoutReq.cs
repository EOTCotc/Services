namespace DID.Models.Request
{
    public class LogoutReq
    {
        /// <summary>
        /// 原因
        /// </summary>
        public string? Reason
        {
            get; set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail
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
