using NPoco;

namespace DID.Entitys
{

    /// <summary>
    /// /* 标签*/
    /// </summary>
    [PrimaryKey("TagId", AutoIncrement = false)]
    public class Tag
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string TagId
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
    }
}

