using DID.Entitys;
using NPoco;

namespace DID.Models.Response
{
    /// <summary>
    /// 审核类型 0 未审核 1 审核通过  2 信息不全 3 信息有误 4 证件照片有误 5 证件照片不清晰
    /// </summary>
    //public enum AuditTypeEnum { 未审核, 审核通过, 信息不全, 信息有误, 证件照片有误, 证件照片不清晰 }

    ///// <summary>
    ///// 审核步骤 0 未审核 1 初审  2 二审 3 抽审
    ///// </summary>
    //public enum AuditStepEnum { 未审核, 初审, 二审, 抽审 }

    /// <summary>
    /// /* 用户认证信息 */
    /// </summary>
    public class UserAuthRespon
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string? UserAuthInfoId
        {
            get; set;
        }
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
        /// <summary>
        /// 人像面
        /// </summary>
        public string? PortraitImage
        {
            get; set;
        }
        /// <summary>
        /// 国徽面
        /// </summary>
        public string? NationalImage
        {
            get; set;
        }
        /// <summary>
        /// 手持证件照
        /// </summary>
        public string? HandHeldImage
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
        /// 创建用户编号
        /// </summary>
        public string CreatorId
        {
            get; set;
        }
        /// <summary>
        /// 初审类型 0 未审核 1 审核通过  2 信息不全 3 信息有误 4 证件照片有误 5 证件照片不清晰
        /// </summary>
        //public AuditTypeEnum AuditType
        //{
        //    get; set;
        //}
        /// <summary>
        /// 审批记录
        /// </summary>
        public List<AuthInfo>? Auths
        {
            get; set;
        }
        ///// <summary>
        ///// 初审类型 0 未审核 1 审核通过  2 信息不全 3 信息有误 4 证件照片有误 5 证件照片不清晰
        ///// </summary>
        //public AuditTypeEnum OnceAuditType
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 初审用户编号
        ///// </summary>
        //public string? OnceAuditId
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 初审时间
        ///// </summary>
        //public DateTime OnceAuditDate
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 二审类型 0 未审核 1 审核通过  2 信息不全 3 信息有误 4 证件照片有误 5 证件照片不清晰
        ///// </summary>
        //public AuditTypeEnum TwiceAuditType
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 二审用户编号
        ///// </summary>
        //public string? TwiceAuditId
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 二审时间
        ///// </summary>
        //public DateTime TwiceAuditDate
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 抽审类型 0 未审核 1 审核通过  2 信息不全 3 信息有误 4 证件照片有误 5 证件照片不清晰
        ///// </summary>
        //public AuditTypeEnum ThriceAuditType
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 抽审用户编号
        ///// </summary>
        //public string? ThriceAuditId
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 抽审时间
        ///// </summary>
        //public DateTime ThriceAuditDate
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 审核类型 0 未审核 1 审核通过  2 信息不全 3 信息有误 4 证件照片有误 5 证件照片不清晰
        ///// </summary>
        //public AuditTypeEnum AuditType
        //{
        //    get; set;
        //}

        ///// <summary>
        ///// 审核步骤 0 未审核 1 初审  2 二审 3 抽审
        ///// </summary>
        //public AuditStepEnum AuditStep
        //{
        //    get; set;
        //}
    }

}

