using Dao.Models.Base;
using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class SetDaoEnableReq : DaoBaseReq
    {
        /// <summary>
        /// 是否启用Dao审核仲裁权限 0 否 1 是
        /// </summary>
        public IsEnum IsEnable
        {
            get; set;
        }
    }
}
