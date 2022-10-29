using DID.Entitys;
using NPoco;

namespace App.Entity
{
    /// <summary>
    /// 自愿者表
    /// </summary>
    [TableName("App_Volunteer")]
    [PrimaryKey("VolunteerId", AutoIncrement = false)]
    public class Volunteer
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string VolunteerId
        {
            get; set;
        }
        /// <summary>
        /// 微信号
        /// </summary>
        public string Wechat
        {
            get; set;
        }
        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode
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