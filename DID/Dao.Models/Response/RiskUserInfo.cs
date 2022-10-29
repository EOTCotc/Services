using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class RiskUserInfo
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNum
        {
            get; set;
        }
        /// <summary>
        /// 证件号
        /// </summary>
        public string IdCard
        {
            get; set;
        }
        /// <summary>
        /// 人像面
        /// </summary>
        public string? PortraitImage
        {
            get; set;
        }
        /// <summary>
        /// 国徽面
        /// </summary>
        public string? NationalImage
        {
            get; set;
        }
        /// <summary>
        /// 手持证件照
        /// </summary>
        public string? HandHeldImage
        {
            get; set;
        }

        /// <summary>
        /// 认证图片
        /// </summary>
        public string? Image
        {
            get; set;
        }
    }
}
