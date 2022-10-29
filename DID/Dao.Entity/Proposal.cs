using NPoco;

namespace Dao.Entity
{
    /// <summary>
    /// 状态 0=进行中 1=未通过 2=已通过 3=已终止
    /// </summary>
    public enum StateEnum { 进行中, 未通过, 已通过, 已终止 }

    //public enum IsCancelEnum { 未取消, 已取消 }

    [PrimaryKey("ProposalId", AutoIncrement = false)]
    public class Proposal
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ProposalId
        {
            get; set;
        }
        /// <summary>
        /// 钱包地址
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
        /// 标题
        /// </summary>
        public string Title
        {
            get; set;
        }
        /// <summary>
        /// 概述
        /// </summary>
        public string Summary
        {
            get; set;
        }
        /// <summary>
        /// 状态 0=进行中 1=未通过 2=已通过 3=已终止
        /// </summary>
        public StateEnum State
        {
            get; set;
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
        /// <summary>
        /// 赞成票数
        /// </summary>
        public int FavorVotes
        {
            get; set;
        }
        /// <summary>
        /// 反对票数
        /// </summary>
        public int OpposeVotes
        {
            get; set;
        }
        /// <summary>
        /// 是否取消 0 未取消 1 已取消
        /// </summary>
        //public IsCancelEnum IsCancel
        //{
        //    get; set;
        //}
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum
        {
            get; set;
        }
        /// <summary>
        /// 票数
        /// </summary>
        //public int VotesNum
        //{
        //    get; set;
        //}
    }
}
