using NPoco;

namespace DID.Entitys
{
    /// <summary>
    /// 社区审核表
    /// </summary>
    [PrimaryKey("ComAuthId", AutoIncrement = false)]
    public class ComAuth
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ComAuthId
        {
            get; set;
        }
        /// <summary>
        /// 社区编号
        /// </summary>
        public string CommunityId
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
        /// 审核类型  0 未审核 1 审核通过  2 信息不全 3 信息有误 4 证件照片有误 5 证件照片不清晰
        /// </summary>
        public AuditTypeEnum AuditType
        {
            get; set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
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
        /// <summary>
        /// 审核步骤 0 初审  1 二审 2 抽审 3 Dao
        /// </summary>
        public AuditStepEnum AuditStep
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
