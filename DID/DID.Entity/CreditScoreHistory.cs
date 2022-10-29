using NPoco;

namespace DID.Entitys
{
    /// <summary>
    /// 类型
    /// </summary>
    public enum TypeEnum { 加分, 减分 }
    /// <summary>
    /// 信用分历史
    /// </summary>
    [PrimaryKey("CreditScoreHistoryId", AutoIncrement = false)]
    public class CreditScoreHistory
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string? CreditScoreHistoryId
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
        /// 类型 0 加分 1 减分
        /// </summary>
        public TypeEnum Type
        {
            get; set;
        }
        /// <summary>
        /// 分数
        /// </summary>
        public int Fraction
        {
            get; set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks
        {
            get; set;
        }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }

    }
}
