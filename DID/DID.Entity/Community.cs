using NPoco;

namespace DID.Entitys
{
    /// <summary>
    /// 社区申请表
    /// </summary>
    [PrimaryKey("CommunityId", AutoIncrement = false)]
    public class Community
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string? CommunityId
        {
            get; set; 
        }
        /// <summary>
        /// 申请人
        /// </summary>
        public string? DIDUserId
        {
            get; set;
        }
        /// <summary>
        /// 社区名
        /// </summary>
        public string? ComName
        {
            get; set;
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string? Phone
        {
            get; set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Mail
        {
            get; set;
        }
        /// <summary>
        /// 推荐人社区
        /// </summary>
        public string? RefCommunityId
        {
            get; set;
        }
        /// <summary>
        /// 推荐人编号 
        /// </summary>
        public string? RefDIDUserId
        {
            get; set;
        }
        /// <summary>
        /// 国家
        /// </summary>
        public string? Country
        {
            get; set;
        }
        /// <summary>
        /// 省
        /// </summary>
        public string? Province
        {
            get; set;
        }
        /// <summary>
        /// 市
        /// </summary>
        public string? City
        {
            get; set;
        }
        /// <summary>
        /// 区
        /// </summary>
        public string? Area
        {
            get; set;
        }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string? Address
        {
            get; set;
        }
        /// <summary>
        /// 地址名称
        /// </summary>
        public string? AddressName
        {
            get; set;
        }
        /// <summary>
        /// 审核状态 0 未审核 1 审核中 2 审核成功 3 审核失败
        /// </summary>
        public AuthTypeEnum AuthType
        {
            get; set;
        }
        /// <summary>
        /// 有无办公室 0 无  1 有
        /// </summary>
        public IsEnum HasOffice
        {
            get; set;
        }
        /// <summary>
        /// 是否建群 0 无  1 有
        /// </summary>
        public IsEnum HasGroup
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
        /// <summary>
        /// 图片
        /// </summary>
        public string? Image
        {
            get; set;
        }
        /// <summary>
        /// 简介
        /// </summary>
        public string? Describe
        {
            get; set;
        }
        /// <summary>
        /// 电报群
        /// </summary>
        public string? Telegram
        {
            get; set;
        }
        /// <summary>
        /// qq群
        /// </summary>
        public string? QQ
        {
            get; set;
        }
        /// <summary>
        /// Discord
        /// </summary>
        public string? Discord
        {
            get; set;
        }

        /// <summary>
        /// 修改次数
        /// </summary>
        public int UpdateNum
        {
            get; set;
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateDate
        {
            get; set;
        }
    }
}
