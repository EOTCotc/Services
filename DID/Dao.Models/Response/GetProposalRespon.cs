using Dao.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetProposalRespon
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
        public string WalletAddress
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
        /// 是否已投票 0 未投票 1 已投票
        /// </summary>
        public int IsVote
        {
            get; set;
        }
    }
}
