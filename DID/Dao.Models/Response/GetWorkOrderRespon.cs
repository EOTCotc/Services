using Dao.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetWorkOrderRespon
    {
        /// <summary>
        /// 创建日期 默认为当前时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
        /// <summary>
        /// 状态 选项：0=待处理 1=处理中 2=已处理
        /// </summary>
        public WorkOrderStatusEnum Status
        {
            get; set;
        }
        /// <summary>
        /// 提交人
        /// </summary>
        public string Submitter
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
        /// <summary>
        /// 处理人
        /// </summary>
        public string Handle
        {
            get; set;
        }
        /// <summary>
        /// 处理记录
        /// </summary>
        public string Record
        {
            get; set;
        }
    }
}
