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
    /// 消息类型 0 申请延期 1 追加举证 2 仲裁取消 3 结案通知
    /// </summary>
    public enum MessageTypeEnum {  申请延期, 追加举证, 仲裁取消 , 结案通知, 被告消息}
    [PrimaryKey("ArbitrateMessageId", AutoIncrement = false)]
    public class ArbitrateMessage
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ArbitrateMessageId
        {
            get; set;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId
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
        /// 是否为仲裁员消息
        /// </summary>
        public IsEnum IsArbitrate
        {
            get; set;
        }
    }
}
