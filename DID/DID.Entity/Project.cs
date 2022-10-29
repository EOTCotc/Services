/*
  ###CSharp Code Generate###
  Project
  Create by User(EMAIL) 2022/9/1 17:50:53
  项目表

Project(项目表)
---------------------------------------
ProjectId(编号)         PKString(50)
Name(名称)              Enum
CreditScore(信用分)     Enum
Mail(邮箱)              Enum
Telegram(电报群)        Enum
Country(国家)           Enum
Province(省)            Enum
City(市)                Enum
Area(区)                Enum
Uid(编号)               Enum
RegDate(注册时间)       Enum

*/

using NPoco;

namespace DID.Entitys
{
    /* 项目表 */
    [PrimaryKey("ProjectId", AutoIncrement = false)]
    public class Project
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ProjectId
        {
            get;set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public IsEnum Name
        {
            get; set;
        }
        /// <summary>
        /// 信用分
        /// </summary>
        public IsEnum CreditScore
        {
            get; set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public IsEnum Mail
        {
            get; set;
        }
        /// <summary>
        /// 电报群
        /// </summary>
        public IsEnum Telegram
        {
            get; set;
        }
        /// <summary>
        /// 国家
        /// </summary>
        public IsEnum Country
        {
            get; set;
        }
        /// <summary>
        /// 省
        /// </summary>
        public IsEnum Province
        {
            get; set;
        }
        /// <summary>
        /// 市
        /// </summary>
        public IsEnum City
        {
            get; set;
        }
        /// <summary>
        /// 区
        /// </summary>
        public IsEnum Area
        {
            get; set;
        }
        /// <summary>
        /// 编号
        /// </summary>
        public IsEnum Uid
        {
            get; set;
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public IsEnum RegDate
        {
            get; set;
        }
    }
}

