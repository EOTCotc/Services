namespace App.Models.Request
{
    public class AddVolunteerReq
    {
        /// <summary>
        /// 微信号
        /// </summary>
        public string Wechat
        {
            get; set;
        }
        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode
        {
            get; set;
        }
    }
}