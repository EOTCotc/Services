using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Request
{
    public class UpdateOrderUserReq
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string OrderId
        {
            get; set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone
        {
            get; set;
        }
        /// <summary>
        /// 微信号
        /// </summary>
        public string Wechat
        {
            get; set;
        }
    }
}
