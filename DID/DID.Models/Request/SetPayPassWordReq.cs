using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DID.Models.Request
{
    public class SetPayPassWordReq
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail
        {
            get; set;
        }
        /// <summary>
        /// 支付密码（md5）
        /// </summary>
        public string PayPassWord
        {
            get; set;
        }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code
        {
            get; set;
        }
    }
}
