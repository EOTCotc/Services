using Dao.Models.Base;

namespace Dao.Models.Request
{
    public class GetTeamAuthListReq : DaoBaseReq
    {
        /// <summary>
        /// 0 待处理 1 已处理
        /// </summary>
        public int Type
        {
            get; set;
        }
    }
}
