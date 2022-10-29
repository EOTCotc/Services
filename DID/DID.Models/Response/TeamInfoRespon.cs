using DID.Entitys;

namespace DID.Models.Response
{
    public class TeamInfoRespon
    {
        /// <summary>
        /// 团队人数
        /// </summary>
        public int TeamNumber
        {
            get; set;
        }

        /// <summary>
        /// 直推人数
        /// </summary>
        public int PushNumber
        {
            get; set;
        }

        ///// <summary>
        ///// 有效节点
        ///// </summary>
        //public int ValidNode
        //{
        //    get; set;
        //}

        /// <summary>
        /// 用户详情
        /// </summary>
        public List<TeamUser> Users
        {
            get; set;
        }
    }

    public class TeamUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UID
        {
            get; set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 用户节点
        /// </summary>
        public UserNodeEnum UserNode
        {
            get; set;
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegDate
        {
            get; set;
        }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone
        {
            get; set;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail
        {
            get; set;
        }

        ///// <summary>
        ///// 是否已认证
        ///// </summary>
        //public IsEnum IsAuth
        //{
        //    get; set;
        //}

    }
}
