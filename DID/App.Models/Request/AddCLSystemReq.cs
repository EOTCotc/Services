using App.Entity;

namespace App.Models.Request
{
    public class AddCLSystemReq
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
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
        /// 类型 0 1个月 1 12个月
        /// </summary>
        public CLTypeEnum Type
        {
            get; set;
        }
    }
}