using DID.Entitys;
using NPoco;

namespace App.Entity
{
    /// <summary>
    /// 等级 0 初级 1 中级 2 高级
    /// </summary>
    public enum GradeEnum {初级, 中级, 高级 }
    /// <summary>
    /// 课程表
    /// </summary>
    [TableName("App_Course")]
    [PrimaryKey("CourseId", AutoIncrement = false)]
    public class Course
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string CourseId
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
        /// 等级 0 初级 1 中级 2 高级
        /// </summary>
        public GradeEnum Grade
        {
            get; set;
        }
        /// <summary>
        /// 价格
        /// </summary>
        public double Price
        {
            get; set;
        }
        /// <summary>
        /// 购买人数
        /// </summary>
        public int BuyNum
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
        /// 简介标题
        /// </summary>
        public string BlurbTitle
        {
            get; set;
        }
        /// <summary>
        /// 主讲内容
        /// </summary>
        public string Content
        {
            get; set;
        }
        /// <summary>
        /// 主讲内容标题
        /// </summary>
        public string ContentTitle
        {
            get; set;
        }
        /// <summary>
        /// 图片
        /// </summary>
        public string Images
        {
            get; set;
        }
        /// <summary>
        /// 老师编号
        /// </summary>
        public string TeacherId
        {
            get; set;
        }
        /// <summary>
        /// 创建时间
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