using NPoco;

namespace DID.Entitys
{
    /// <summary>
    /// 审核类型 0 未审核 1 审核通过  2 恶意提交 3 信息有误 
    public enum AuditTypeEnum { 未审核, 审核通过, 恶意提交, 信息有误 }

    /// <summary>
    /// 审核步骤 0 初审  1 二审 2 抽审 3 Dao
    /// </summary>
    public enum AuditStepEnum { 初审, 二审, 抽审 }

    /// <summary>
    /// /* 用户认证信息 */
    /// </summary>
    [PrimaryKey("AuthId", AutoIncrement = false)]
    public class Auth
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string AuthId
        {
            get; set;
        }
        /// <summary>
        /// 认证信息编号
        /// </summary>
        public string UserAuthInfoId
        {
            get; set;
        }
        /// <summary>
        /// 审核用户编号
        /// </summary>
        public string AuditUserId
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
        /// 创建时间
        /// </summary>
        public DateTime CreateDate 
        {
            get; set;
        }

        /// <summary>
        /// 审核类型 0 未审核 1 审核通过  2 恶意提交 3 信息有误 
        /// </summary>
        public AuditTypeEnum AuditType
        {
            get; set;
        }
        /// <summary>
        /// 初审时间
        /// </summary>
        public DateTime? AuditDate
        {
            get; set;
        }

        /// <summary>
        /// 审核步骤  1 初审  2 二审 3 抽审
        /// </summary>
        public AuditStepEnum AuditStep
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
        /// <summary>
        /// 是否删除
        /// </summary>
        public IsEnum IsDelete
        {
            get; set;
        }

    }

}

