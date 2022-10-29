using DID.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetTeamAuthListRespon
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string TeamAuthId
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
        /// 团队人数
        /// </summary>
        public int TeanNum
        {
            get; set;
        }
        /// <summary>
        /// 推荐人信息
        /// </summary>
        public string RefUser
        {
            get; set;
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        public string User
        {
            get; set;
        }
        /// <summary>
        /// 推荐人编号
        /// </summary>
        public string RefUserId
        {
            get; set;
        }
    }
}
