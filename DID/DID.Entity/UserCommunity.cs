using NPoco;

namespace DID.Entitys
{
    /// <summary>
    /// 社区审核表
    /// </summary>
    [PrimaryKey("UserCommunityId", AutoIncrement = false)]
    public class UserCommunity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string UserCommunityId
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
        /// 社区编号
        /// </summary>
        public string CommunityId
        {
            get; set;
        }
        /// <summary>
        /// 加入日期
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
    }
}
