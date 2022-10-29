using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class AddIncomeDetailsReq : DaoBaseReq
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
        /// 备注
        /// </summary>
        public string? Remarks
        {
            get; set;
        }
    }
}
