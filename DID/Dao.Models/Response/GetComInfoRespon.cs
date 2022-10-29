using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetComInfoRespon
    {
        /// <summary>
        /// 社区名
        /// </summary>
        public string? ComName
        {
            get; set;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Mail
        {
            get; set;
        }

        /// <summary>
        /// 电话
        /// </summary>
        public string? Phone
        {
            get; set;
        }
    }
}
