using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class UploadImageReq : DaoBaseReq
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get; set;
        }
    }
}
