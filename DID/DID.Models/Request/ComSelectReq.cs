using DID.Entitys;
using NPoco;

namespace DID.Models.Request
{
    /// <summary>
    /// 用户社区选择（没邀请码）
    /// </summary>
    public class ComSelectReq
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string? UserId
        {
            get; set;
        }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country
        {
            get; set;
        }
        /// <summary>
        /// 省
        /// </summary>
        public string Province
        {
            get; set;
        }
        /// <summary>
        /// 市
        /// </summary>
        public string? City
        {
            get; set;
        }
        /// <summary>
        /// 区
        /// </summary>
        public string? Area
        {
            get; set;
        }
    }
}
