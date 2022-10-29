using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DID.Entity
{
    /// <summary>
    /// 审核类型  0 未审核 1 审核通过  2 未实名认证 3 其他
    /// </summary>
    public enum TeamAuditEnum { 未审核, 审核通过, 未实名, 其他} 
    /// <summary>
    /// 团队认证表
    /// </summary>
    [PrimaryKey("TeamAuthId", AutoIncrement = false)]
    public class TeamAuth
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string TeamAuthId
        {
            get; set;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string DIDUserId
        {
            get; set;
        }
        /// <summary>
        /// 审核用户
        /// </summary>
        public string AuditUserId
        {
            get; set;
        }
        /// <summary>
        /// 审核类型  0 未审核 1 审核通过  2 未实名认证 3 其他
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
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditDate
        {
            get; set;
        }
    }
}
