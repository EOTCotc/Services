using Dao.Models.Base;
using Dao.Entity;
using DID.Entitys;

namespace Dao.Models.Request
{
    public class UserRiskLevelReq : DaoBaseReq
    {
        public RiskLevelEnum Level
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
    }
}
