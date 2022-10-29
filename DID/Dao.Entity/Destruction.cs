using DID.Entitys;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Entity
{
    /// <summary>
    /// 销毁查询
    /// </summary>
    [PrimaryKey("DestructionId", AutoIncrement = false)]
    public class Destruction
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string? DestructionId
        {
            get; set;
        }
        ///// <summary>
        ///// 钱包编号
        ///// </summary>
        //public string WalletId
        //{
        //    get; set;
        //}
        /// <summary>
        /// EOTC数量
        /// </summary>
        public double EOTC
        {
            get; set;
        }
        /// <summary>
        /// hash值
        /// </summary>
        public string HashCode
        {
            get; set;
        }
        /// <summary>
        /// 注释
        /// </summary>
        public string Memo
        {
            get; set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark
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
        /// <summary>
        /// 销毁日期 默认为当前时间
        /// </summary>
        public DateTime DestructionDate
        {
            get; set;
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        public IsEnum IsDelete
        {
            get; set;
        }
    }
}
