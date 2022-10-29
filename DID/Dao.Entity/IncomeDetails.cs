using NPoco;

namespace Dao.Entity
{
    /// <summary>
    ///  0=处理工单 1=处理仲裁 2=处理审核 
    /// </summary>
    public enum IDTypeEnum
    { 
        /// <summary>
        /// 
        /// </summary>
        处理工单, 
        /// <summary>
        /// 
        /// </summary>
        处理仲裁, 
        /// <summary>
        /// 
        /// </summary>
        处理审核 ,

        提案投票 ,
        创建提案
    }
    /// <summary>
    /// 收益明细
    /// </summary>
    [PrimaryKey("IncomeDetailsId", AutoIncrement = false)]
    public class IncomeDetails
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string IncomeDetailsId
        {
            get; set;
        }
        /// <summary>
        /// 钱包编号
        /// </summary>
        public string WalletId
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
        /// 收益数量
        /// </summary>
        public double EOTC
        {
            get; set;
        }
        /// <summary>
        /// 0=处理工单 1=处理仲裁 2=处理审核 
        /// </summary>
        public IDTypeEnum Type
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
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }

    }
}
