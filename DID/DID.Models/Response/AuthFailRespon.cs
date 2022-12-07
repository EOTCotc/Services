using DID.Entitys;

namespace DID.Models.Response
{
    /// <summary>
    /// 审核失败信息
    /// </summary>
    public class AuthFailRespon
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
        /// 备注
        /// </summary>
        public string? Remark
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
        /// 审批记录
        /// </summary>
        public List<AuthInfo>? Auths
        {
            get; set;
        }
    }
}
