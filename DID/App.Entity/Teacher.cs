using DID.Entitys;
using NPoco;

namespace App.Entity
{
    /// <summary>
    /// 教师表
    /// </summary>
    [TableName("App_Teacher")]
    [PrimaryKey("TeacherId", AutoIncrement = false)]
    public class Teacher
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string TeacherId
        {
            get; set;
        }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImage
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get; set;
        }
        /// <summary>
        /// 简介
        /// </summary>
        public string Blurb
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