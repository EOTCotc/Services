/*
  ###CSharp Code Generate###
  UserProject
  Create by User(EMAIL) 2022/9/1 17:50:27
  用户项目关系表

UserProject(用户项目关系表)
--------------------------------------------
UserProject(编号)          PKString(50)
DIDUserId(用户编号)        FKString(50) //<<关联:DIDUser.DIDUserId>>
ProjectId(项目编号)        FKString(50) //<<关联:Project.ProjectId>>
CreateDate(创建时间)       Date
BindingDate(绑定时间)      Date
UnboundDate(解绑时间)      Date
IsBind(是否绑定)           Enum
Name(名称)                 String(50)
Remarks(备注)              String(255)
IsDelete(是否删除)         Enum

*/

using NPoco;

namespace DID.Entitys
{
    /* 用户项目关系表 */
    [PrimaryKey("UserProjectId", AutoIncrement = false)]
    public class UserProject
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string UserProjectId
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
        /// 项目编号
        /// </summary>
        public string ProjectId
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
        ///// <summary>
        ///// 绑定时间
        ///// </summary>
        //public DateTime BindingDate
        //{
        //    get; set;
        //}
        ///// <summary>
        ///// 解绑时间
        ///// </summary>
        //public DateTime UnboundDate
        //{
        //    get; set;
        //}
        /// <summary>
        /// 是否绑定
        /// </summary>
        public IsEnum IsBind
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
        /// 备注
        /// </summary>
        public string Remarks
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

