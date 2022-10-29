using DID.Entitys;

namespace DID.Models.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class GetCreditScoreRespon
    {
        /// <summary>
        /// 信用分
        /// </summary>
        public int CreditScore
        {
            get; set;
        }

        /// <summary>
        /// 信用分记录
        /// </summary>
        public List<CreditScoreHistory>? Items
        {
            get; set;
        }
    }
}
