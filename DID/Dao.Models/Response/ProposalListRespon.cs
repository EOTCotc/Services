using Dao.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class ProposalListRespon
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ProposalId
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
        /// 总票数
        /// </summary>
        public int Total
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
    }
}
