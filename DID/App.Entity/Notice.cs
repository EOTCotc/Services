using DID.Entitys;
using NPoco;

namespace App.Entity
{
    /// <summary>
    /// 公告表
    /// </summary>\
    [TableName("App_Notice")]
    [PrimaryKey("NoticeId", AutoIncrement = false)]
    public class Notice
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string NoticeId
        {
            get; set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get; set;
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get; set;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorName
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
        /// 是否删除
        /// </summary>
        public IsEnum IsDelete
        {
            get; set;
        }
    }
}