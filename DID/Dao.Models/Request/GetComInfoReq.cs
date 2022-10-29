using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class GetComInfoReq : DaoBaseReq
    {
        /// <summary>
        /// 推荐人编号
        /// </summary>
        public string RefUserId
        {
            get; set;
        }
    }
}
