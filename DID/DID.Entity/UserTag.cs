using NPoco;

namespace DID.Entitys
{


    /// <summary>
    /// /* 用户标签 */
    /// </summary>
    [PrimaryKey("UserTagId", AutoIncrement = false)]
    public class UserTag
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string UserTagId
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
        /// 标签编号
        /// </summary>
        public string TagId
        {
            get; set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
    }
}

