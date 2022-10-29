using Dao.Models.Base;
using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class GetArbitrateDelayReq : DaoBaseReq
    {
        public string Id
        {
            get; set;
        }
        public IsEnum IsArbitrate
        {
            get; set;
        }
    }
}
