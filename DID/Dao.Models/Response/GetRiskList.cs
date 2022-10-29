using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetRiskList
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone
        {
            get; set;
        }
        /// <summary>
        /// 是否解除风控
        /// </summary>
        public IsEnum Status
        {
            get; set;
        }
    }
}
