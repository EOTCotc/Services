using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class GetUserArbitrateReq : DaoBaseReq
    {

        /// <summary>
        /// 0 待处理 1 已仲裁
        /// </summary>
        public int Type
        {
            get; set;
        }
    }
}
