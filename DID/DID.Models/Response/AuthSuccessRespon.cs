using DID.Entitys;

namespace DID.Models.Response
{
    /// <summary>
    /// 审核详情
    /// </summary>
    public class AuthSuccessRespon
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNum
        {
            get; set;
        }
        /// <summary>
        /// 证件号
        /// </summary>
        public string IdCard
        {
            get; set;
        }
        public string Mail { get; set; }
        /// <summary>
        /// 邀请人姓名
        /// </summary>
        public string RefUid
        {
            get; set;
        }
        /// <summary>
        /// 审批记录
        /// </summary>
        public List<AuthInfo>? Auths
        {
            get; set;
        }
    }
    /// <summary>
    /// 审核详情
    /// </summary>
    public class AuthInfo
    {
        /// <summary>
        /// 审核人用户编号
        /// </summary>
        public int UId { get; set; }
        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuthDate { get; set; }
        /// <summary>
        /// 审核步骤  1 初审  2 二审 3 抽审
        /// </summary>
        public AuditStepEnum AuditStep
        {
            get; set;
        }
        /// <summary>
        /// 初审类型 0 未审核 1 审核通过  2 信息不全 3 信息有误 4 证件照片有误 5 证件照片不清晰
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
        /// <summary>
        /// 是否为Dao审核 0 否 1 是
        /// </summary>
        public IsEnum IsDao
        {
            get; set;
        }
    }
}
