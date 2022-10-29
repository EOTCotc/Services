using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Request
{
    public class AddAppVersionReq
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNo
        {
            get; set;
        }
        /// <summary>
        /// 系统版本 0=Android 1=ios
        /// </summary>
        public int OsType
        {
            get; set;
        }
        /// <summary>
        /// 下载连接1
        /// </summary>
        public string Link1
        {
            get; set;
        }
        /// <summary>
        /// 下载连接2
        /// </summary>
        public string Link2
        {
            get; set;
        }
        /// <summary>
        /// 下载连接3
        /// </summary>
        public string Link3
        {
            get; set;
        }
    }
}
