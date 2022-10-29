using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DID.Models.Request
{
    public class AuditInfoReq
    {
        /// <summary>
        /// 审核信息编号
        /// </summary>
        public string UserAuthInfoId
        {
            get; set;
        }
        
        /// <summary>
        /// 审核类型
        /// </summary>
        public AuditTypeEnum AuditType
        {
            get; set;
        }
        
        //备注
        public string? Remark
        {
            get; set;
        }
    }
}
