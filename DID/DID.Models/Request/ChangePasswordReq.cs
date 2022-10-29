using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DID.Models.Request
{
    public class ChangePasswordReq
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail
        {
            get; set;
        }
        /// <summary>
        /// 新密码（md5）
        /// </summary>
        public string NewPassWord
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
