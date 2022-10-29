using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DID.Models.Request
{
    public class AuditCommunityReq
    {
        /// <summary>
        /// 社区编号
        /// </summary>
        public string CommunityId
        {
            get; set;
        }
        /// <summary>
        ///  审核类型 0 未审核 1 审核通过  2 信息不全 3 信息有误 4 证件照片有误 5 证件照片不清晰
        /// </summary>
        public AuditTypeEnum AuditType
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
}
