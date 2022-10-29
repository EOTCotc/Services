using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    /// <summary>
    /// 提案
    /// </summary>
    public class ProposalReq : DaoBaseReq
    {
        /// <summary>
        /// 钱包地址
        /// </summary>
        //public string WalletAddress
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 网络类型
        ///// </summary>
        //public string Otype
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 签名
        ///// </summary>
        //public string Sign
        //{
        //    get; set;
        //}
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
    }
    /// <summary>
    /// 投票
    /// </summary>
    public class ProposalVoteReq : DaoBaseReq
    {
        /// <summary>
        /// 提案编号
        /// </summary>
        public string ProposalId
        {
            get; set;
        }
        ///// <summary>
        ///// 钱包地址
        ///// </summary>
        //public string WalletAddress
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 网络类型
        ///// </summary>
        //public string Otype
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 签名
        ///// </summary>
        //public string Sign
        //{
        //    get; set;
        //}
        /// <summary>
        /// 投票类型
        /// </summary>
        public VoteEnum Vote
        {
            get; set;
        }
    }
    
}
