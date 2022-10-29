using Dao.Models.Base;
using DID.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class TeamAuthReq : DaoBaseReq
    {
        /// <summary>
        /// 团队审核编号
        /// </summary>
        public string TeamAuthId
        {
            get; set;
        }
        /// <summary>
        ///  审核类型  0 未审核 1 审核通过  2 未实名认证 3 其他
        /// </summary>
        public TeamAuditEnum AuditType
        {
            get; set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark
        {
            get; set;
        }
    }

    public class TeamUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string DIDUserId
        {
            get; set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail
        {
            get; set;
        }
        /// <summary>
        /// 认证编号
        /// </summary>
        public string? UserAuthInfoId
        {
            get; set;
        }
        /// <summary>
        /// 层级
        /// </summary>
        public string Level
        {
            get; set;
        }
    }
}
