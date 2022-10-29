using Dao.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class UserRiskRespon
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string UserRiskId
        {
            get; set;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get; set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }

        /// <summary>
        /// 审核状态 0 未核对 1 核对成功  2 核对失败
        /// </summary>
        public RiskStatusEnum AuthStatus
        {
            get; set;
        }
    }
}
