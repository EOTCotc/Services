using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class AddWorkOrderReq : DaoBaseReq
    {
        /// <summary>
        /// 类型 0=bug反馈 1=功能建议 
        /// </summary>
        public WorkOrderTypeEnum WorkOrderType
        {
            get; set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe
        {
            get; set;
        }
        /// <summary>
        /// 图片
        /// </summary>
        public string Images
        {
            get; set;
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone
        {
            get; set;
        }
    }
}
