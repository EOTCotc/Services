using DID.Entitys;
using NPoco;

namespace DID.Models.Request
{
    /// <summary>
    /// 信用分
    /// </summary>
    public class CreditScoreReq
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int Uid
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
    }
}
