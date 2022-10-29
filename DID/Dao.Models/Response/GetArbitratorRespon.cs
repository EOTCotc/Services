using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetArbitratorRespon 
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name
        {
            get; set;
        }
        /// <summary>
        /// 仲裁员编号
        /// </summary>
        public string? Number
        {
            get; set;
        }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// </summary>
        public DateTime? CreateDate
        {
            get; set;
        }
        /// <summary>
        /// 仲裁收益
        /// </summary>
        public double EOTC
        {
            get; set;
        }
        /// <summary>
        /// 仲裁次数
        /// </summary>
        public int ArbitrateNum
        {
            get; set;
        }
        /// <summary>
        /// 仲裁胜利次数
        /// </summary>
        public int VictoryNum
        {
            get; set;
        }
    }
}
