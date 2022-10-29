using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetAuditorRespon
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name
        {
            get; set;
        }
        /// <summary>
        /// 审核员编号
        /// </summary>
        public string? Number
        {
            get; set;
        }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// </summary>
        public DateTime? CreateDate
        {
            get; set;
        }
        /// <summary>
        /// 审核收益
        /// </summary>
        public double EOTC
        {
            get; set;
        }
        /// <summary>
        /// 审核次数
        /// </summary>
        public int ExamineNum
        {
            get; set;
        }
    }
}
