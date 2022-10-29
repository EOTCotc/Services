using DID.Entitys;
using NPoco;

namespace App.Entity
{
    /// <summary>
    /// app版本表
    /// </summary>
    [TableName("App_Version")]
    [PrimaryKey("VersionId", AutoIncrement = false)]
    public class AppVersion
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string VersionId
        {
            get; set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNo
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
        /// 系统版本 0=Android 1=ios
        /// </summary>
        public int OsType
        {
            get; set;
        }
        /// <summary>
        /// 下载连接1
        /// </summary>
        public string Link1
        {
            get; set;
        }
        /// <summary>
        /// 下载连接2
        /// </summary>
        public string Link2
        {
            get; set;
        }
        /// <summary>
        /// 下载连接3
        /// </summary>
        public string Link3
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