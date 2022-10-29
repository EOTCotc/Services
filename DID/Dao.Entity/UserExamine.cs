using DID.Entitys;
using NPoco;

namespace Dao.Entity
{
    /// <summary>
    /// 审核员信息表
    /// </summary>
    [PrimaryKey("UserExamineId", AutoIncrement = false)]
    public class UserExamine
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string UserExamineId
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
        /// 审核员编号
        /// </summary>
        public string Number
        {
            get; set;
        }
        /// <summary>
        /// 创建日期 默认为当前时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
        /// <summary>
        /// 是否删除 0 否 1 是
        /// </summary>
        public IsEnum IsDelete
        {
            get; set;
        }
        /// <summary>
        /// 审核收益
        /// </summary>
        public double EOTC
        {
            get; set;
        }
        /// <summary>
        /// 审核次数
        /// </summary>
        public int ExamineNum
        {
            get; set;
        }
    }
}
