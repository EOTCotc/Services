using DID.Entitys;
using NPoco;

namespace DID.Models.Response
{
    /// <summary>
    /// 用户社区列表
    /// </summary>
    public class ComRespon
    {
        /// <summary>
        /// 社区名称
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 照片
        /// </summary>
        public string? Image
        {
            get; set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Describe
        {
            get; set;
        }
        /// <summary>
        /// 电报群
        /// </summary>
        public string? Telegram
        {
            get; set;
        }
    }
}
