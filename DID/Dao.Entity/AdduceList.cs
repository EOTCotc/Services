using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Entity
{
    /// <summary>
    /// 举证记录表
    /// </summary>
    [PrimaryKey("AdduceListId", AutoIncrement = false)]
    public class AdduceList
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string AdduceListId
        {
            get; set;
        }
        /// <summary>
        /// 仲裁信息编号
        /// </summary>
        public string ArbitrateInfoId
        {
            get; set;
        }
        /// <summary>
        /// 举证人编号
        /// </summary>
        public string AdduceUserId
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
        /// <summary>
        /// 文字举证
        /// </summary>
        public string Memo
        {
            get; set;
        }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
    }
}
