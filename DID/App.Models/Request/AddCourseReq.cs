using App.Entity;

namespace App.Models.Request
{
    public class AddCourseReq
    {
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
        //public int BuyNum
        //{
        //    get; set;
        //}
        /// <summary>
        /// 简介
        /// </summary>
        public string Blurb
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
        /// 简介标题
        /// </summary>
        public string BlurbTitle
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
    }
}