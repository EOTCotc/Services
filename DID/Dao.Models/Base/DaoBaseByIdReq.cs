using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Base
{
    public class DaoBaseByIdReq : DaoBaseReq
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            get; set; 
        }
    }
}
