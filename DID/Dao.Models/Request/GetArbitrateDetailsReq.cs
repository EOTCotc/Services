using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class GetArbitrateDetailsReq : DaoBaseReq
    {
        /// <summary>
        /// 仲裁编号
        /// </summary>
        public string ArbitrateInfoId
        {
            get; set;
        }
    }
}
