using DID.Entitys;

namespace DID.Models.Response
{
    /// <summary>
    /// 社区审核信息
    /// </summary>
    public class ComAuthRespon
    {
        /// <summary>
        /// 社区编号
        /// </summary>
        public string CommunityId
        {
            get; set;
        }
        /// <summary>
        /// 申请人
        /// </summary>
        public string DIDUser
        {
            get; set;
        }
        /// <summary>
        /// 社区名
        /// </summary>
        public string ComName
        {
            get; set;
        }
        /// <summary>
        /// 手机号码
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
        /// <summary>
        /// 推荐人社区
        /// </summary>
        public string RefCommunityName
        {
            get; set;
        }
        /// <summary>
        /// 推荐人编号 
        /// </summary>
        public int RefUId
        {
            get; set;
        }
        /// <summary>
        /// 推荐人编号 
        /// </summary>
        public string RefUserId
        {
            get; set;
        }
        /// <summary>
        /// 推荐人姓名
        /// </summary>
        public string RefName
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
        /// 详细地址
        /// </summary>
        public string Address
        {
            get; set;
        }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string AddressName
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
        public string Image
        {
            get; set;
        }
        /// <summary>
        /// 简介
        /// </summary>
        public string Describe
        {
            get; set;
        }
        /// <summary>
        /// 电报群
        /// </summary>
        public string Telegram
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
        /// 审批记录
        /// </summary>
        public List<AuthInfo>? Auths
        {
            get; set;
        }
        /// <summary>
        /// EOTC数量
        /// </summary>
        public double? Eotc
        {
            get; set;
        }
    }
}
