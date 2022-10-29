using Dao.Models.Base;
using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class GetArbitrateMessageReq : DaoBaseReq
    {
        /// <summary>
        /// 是否仲裁员
        /// </summary>
        public IsEnum IsArbitrate
        {
            get; set; 
        }
    }
}
