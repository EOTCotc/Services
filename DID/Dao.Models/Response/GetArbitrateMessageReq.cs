using Dao.Entity;
using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetArbitrateMessageRespon
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ArbitrateMessageId
        {
            get; set;
        }

        /// <summary>
        /// 消息类型 0 申请延期 1 追加举证 2 仲裁取消
        /// </summary>
        public MessageTypeEnum MessageType
        {
            get; set;
        }

        /// <summary>
        /// 关联Id
        /// </summary>
        public string AssociatedId
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
        /// 是否打开 0 否 1 是
        /// </summary>
        public IsEnum IsOpen
        {
            get; set;
        }

        /// <summary>
        /// 原因
        /// </summary>
        public string? Reason
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
    }
}
