using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;

namespace DID.Entitys
{
    public enum TimerTypeEnum {
        注销,
        身份审核,
        社区审核,
        仲裁举证,
        仲裁投票
    }
    [PrimaryKey("TimersId", AutoIncrement = false)]
    public class Timers
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string TimersId
        {
            get; set;
        }
        /// <summary>
        /// 类型 0  1 身份审核 2 社区审核 3 仲裁
        /// </summary>
        public TimerTypeEnum TimerType
        {
            get; set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get; set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get; set;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        //public string DIDUserId
        //{
        //    get; set;
        //}
        /// <summary>
        /// 关联编号
        /// </summary>
        public string Rid
        {
            get; set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
    }
}
