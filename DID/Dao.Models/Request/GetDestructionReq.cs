using Dao.Models.Base;

namespace Dao.Models.Request
{
    /// <summary>
    /// 获取销毁记录
    /// </summary>
    public class GetDestructionReq 
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? KeyWord
        {
            get; set; 
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate
        {
            get; set; 
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get; set;
        }
        /// <summary>
        /// 页数
        /// </summary>
        //public long Page
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 每页数量
        ///// </summary>
        //public long ItemsPerPage
        //{
        //    get; set;
        //}
    }
}
