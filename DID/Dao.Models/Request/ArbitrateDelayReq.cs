using Dao.Entity;
using Dao.Models.Base;
using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Request
{
    public class ArbitrateDelayReq : DaoBaseReq
    {
        /// <summary>
        /// 仲裁编号
        /// </summary>
        public string ArbitrateInfoId
        {
            get; set;
        }

        /// <summary>
        /// 原因
        /// </summary>
        public ReasonEnum Reason
        {
            get; set;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Explain
        {
            get; set;
        }
        /// <summary>
        /// 延期天数
        /// </summary>
        public int Day
        {
            get; set;
        }
        /// <summary>
        /// 是否未仲裁员
        /// </summary>
        public IsEnum IsArbitrate
        {
            get; set;
        }
    }
}
