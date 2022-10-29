using DID.Entitys;
using NPoco;

namespace Dao.Entity
{
    /// <summary>
    /// 审核状态 0 未核对 1 核对成功  2 核对失败
    /// </summary>
    public enum RiskStatusEnum
    {
        未核对,
        核对成功,
        核对失败
    }

    /// <summary>
    /// 0 否 1 是
    /// </summary>
    //public enum IsEnum
    //{
    //    否,
    //    是
    //}
    [PrimaryKey("UserRiskId", AutoIncrement = false)]
    public class UserRisk
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string UserRiskId
        {
            get; set;
        }

        /// <summary>
        /// 风险用户编号
        /// </summary>
        public string DIDUserId
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
        /// 审核状态 0 未核对 1 核对成功  2 核对失败
        /// </summary>
        public RiskStatusEnum AuthStatus
        {
            get; set;
        }
        /// <summary>
        /// 是否解除风控 0 否 1 是
        /// </summary>
        public IsEnum IsRemoveRisk
        {
            get; set;
        }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
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
        /// 是否删除 0 否 1 是
        /// </summary>
        public IsEnum IsDelete
        {
            get; set;
        }

        /// <summary>
        /// 审核图片
        /// </summary>
        public string? Images
        {
            get; set;
        }
    }
}
