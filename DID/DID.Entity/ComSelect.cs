using NPoco;

namespace DID.Entitys
{
    /// <summary>
    /// 社区选择表 
    /// </summary>
    [PrimaryKey("ComSelectId", AutoIncrement = false)]
    public class ComSelect
    {

        /// <summary>
        /// 编号
        /// </summary>
        public string ComSelectId
        {
            get; set;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string DIDUserId
        {
            get; set;
        }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country
        {
            get; set;
        }
        /// <summary>
        /// 省
        /// </summary>
        public string Province
        {
            get; set;
        }
        /// <summary>
        /// 市
        /// </summary>
        public string City
        {
            get; set;
        }
        /// <summary>
        /// 区
        /// </summary>
        public string Area
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
    }
}

