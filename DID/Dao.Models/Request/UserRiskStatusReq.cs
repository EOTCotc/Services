using Dao.Entity;
using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class UserRiskStatusReq : DaoBaseReq
    {
        /// <summary>
        /// 用户风控编号
        /// </summary>
        public string UserRiskId
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
        /// <summary>
        /// 认证图片
        /// </summary>
        public string? Images
        {
            get; set;
        }
    }
}
