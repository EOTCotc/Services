using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class UserRiskReq : DaoBaseReq
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Key
        {
            get; set;
        }
    }
}
