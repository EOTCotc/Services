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
    /// 原因 0  举证时间不足 1 核实信息还在审核中（仲裁员）2 举证不足,无法进行判决 3 部分举证不全
    /// </summary>
    public enum ReasonEnum
    {
        举证时间不足,

        核实信息还在审核中,

        举证不足, 无法进行判决,

        部分举证不全

    }
    /// <summary>
    /// 仲裁延期
    /// </summary>
    [PrimaryKey("ArbitrateDelayId", AutoIncrement = false)]
    public class ArbitrateDelay
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ArbitrateDelayId
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
        /// 用户编号
        /// </summary>
        public string DelayUserId
        {
            get; set;
        }
        /// <summary>
        /// 延期天数
        /// </summary>
        public int Days
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
        /// 原因 0  举证时间不足 1 核实信息还在审核中 （仲裁员）2 举证不足,无法进行判决 3 部分举证不全
        /// </summary>
        public ReasonEnum Reason
        {
            get; set;
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string? Explain
        {
            get; set;
        }

        /// <summary>
        /// 是否仲裁员发起 0 否 1 是
        /// </summary>
        public IsEnum IsArbitrate
        {
            get; set;
        }

        /// <summary>
        /// 是否延期成功 
        /// </summary>
        public int Status
        {
            get; set;
        }
    }
}
