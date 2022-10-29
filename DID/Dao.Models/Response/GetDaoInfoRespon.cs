using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Models.Response
{
    public class GetDaoInfoRespon
    {
        /// <summary>
        /// Dao总收益
        /// </summary>
        public double DaoEOTC
        {
            get; set;
        }

        /// <summary>
        /// 是否为仲裁员 0 否 1 是 
        /// </summary>
        public IsEnum IsArbitrate
        {
            get; set;
        }

        /// <summary>
        /// 是否为审核员 0 否 1 是
        /// </summary>
        public IsEnum IsExamine
        {
            get; set;
        }

        /// <summary>
        /// 风险等级 0 低风险 1 中风险 2 高风险
        /// </summary>
        public RiskLevelEnum RiskLevel
        {
            get; set;
        }

        /// <summary>
        /// 认证状态
        /// </summary>
        public AuthTypeEnum AuthType
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
        /// UId
        /// </summary>
        public int? Uid
        {
            get; set;
        }

        /// <summary>
        /// UserId
        /// </summary>
        public string? UserId
        {
            get; set;
        }

        /// <summary>
        /// 是否启用Dao审核仲裁权限 0 否 1 是
        /// </summary>
        public IsEnum IsEnable
        {
            get; set;
        }
    }
}
