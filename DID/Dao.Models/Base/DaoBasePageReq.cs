using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Base
{
    /// <summary>
    /// Dao接口请求
    /// </summary>
    public class DaoBasePageReq : DaoBaseReq
    {
        /// <summary>
        /// 页数
        /// </summary>
        public long Page
        {
            get; set;
        }
        /// <summary>
        /// 每页数量
        /// </summary>
        public long ItemsPerPage
        {
            get; set;
        }
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string? Key
        {
            get; set;
        }
    }
}
