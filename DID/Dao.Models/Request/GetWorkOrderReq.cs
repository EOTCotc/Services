using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class GetWorkOrderReq : DaoBaseReq
    {
        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderId
        {
            get; set;
        }
    }
}
