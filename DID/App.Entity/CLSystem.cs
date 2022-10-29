using DID.Entitys;
using NPoco;

namespace App.Entity
{
    /// <summary>
    /// 类型 0 1个月 1 12个月
    /// </summary>
    public enum CLTypeEnum { M1, M12 }
    /// <summary>
    /// 禅论系统表
    /// </summary>
    [TableName("App_CLSystem")]
    [PrimaryKey("CLSystemId", AutoIncrement = false)]
    public class CLSystem
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string CLSystemId
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
        /// <summary>
        /// 是否删除
        /// </summary>
        public IsEnum IsDelete
        {
            get; set;
        }
    }
}