namespace App.Models.Request
{
    public class AddTeacherReq
    {
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
    }
}