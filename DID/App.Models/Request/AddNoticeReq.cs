namespace App.Models.Request
{
    public class AddNoticeReq
    {
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
    }
}