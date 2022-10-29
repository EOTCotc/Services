using DID.Entitys;
using NPoco;

namespace DID.Models.Response
{
    /// <summary>
    /// 绑定项目
    /// </summary>
    public class UserProjectRespon
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ProjectId
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
