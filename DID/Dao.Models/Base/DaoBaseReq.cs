using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Base
{
    /// <summary>
    /// Dao接口请求
    /// </summary>
    public class DaoBaseReq
    {
        /// <summary>
        /// 钱包地址
        /// </summary>
        public string WalletAddress
        {
            get; set;
        }
        /// <summary>
        /// 网络类型
        /// </summary>
        public string Otype
        {
            get; set;
        }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign
        {
            get; set;
        }
    }
}
