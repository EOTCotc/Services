using Dao.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class IncomeDetailsRespon
    {
        /// <summary>
        /// 收益数量
        /// </summary>
        public double EOTC
        {
            get; set;
        }
        /// <summary>
        /// 0=处理工单 1=处理仲裁 2=处理审核 
        /// </summary>
        public IDTypeEnum Type
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
    }
}
