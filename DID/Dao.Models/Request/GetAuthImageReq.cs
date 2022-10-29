using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class GetAuthImageReq : DaoBaseReq
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path
        {
            get; set;
        }
    }
}
