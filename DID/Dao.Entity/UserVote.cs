using NPoco;

namespace Dao.Entity
{
    [PrimaryKey("UserVoteId", AutoIncrement = false)]
    public class UserVote
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string UserVoteId
        {
            get; set;
        }
        /// <summary>
        /// 钱包编号
        /// </summary>
        public string WalletId
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
        /// 提案编号
        /// </summary>
        public string ProposalId
        {
            get; set;
        }
        /// <summary>
        /// 票数
        /// </summary>
        public int VoteNum
        {
            get; set;
        }
        /// <summary>
        /// 类型 0 反对 1 赞成
        /// </summary>
        public VoteEnum Type
        {
            get; set;
        }
        /// <summary>
        /// 投票时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
    }
    /// <summary>
    /// 类型 0 反对 1 赞成
    /// </summary>
    public enum VoteEnum { 反对, 赞成 }
}
