using Dao.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class AddAdduceListReq : DaoBaseReq
    {
        /// <summary>
        /// 仲裁编号
        /// </summary>
        public string ArbitrateInfoId
        {
            get; set;
        }
        /// <summary>
        /// 文字举证
        /// </summary>
        public string Memo
        {
            get; set;
        }
        /// <summary>
        /// 图片举证
        /// </summary>
        public string Images
        {
            get; set;
        }
    }
}
