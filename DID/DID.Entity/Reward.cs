
using System;
using System.Text;

namespace DID.Entitys
{
    /* 奖励设置表 创建提案 通过 100 提案投票 提案通过 身份认证审核   10 申请社区审核   10 仲裁（胜利、失败）仲裁员 20 空投（邀请人50 用户20） */
    public class Reward
    {
        /// <summary>
        /// 键
        /// </summary>
        public string RewardKey
        {
            get; set;
        }
        /// <summary>
        /// 值
        /// </summary>
        public double RewardValue
        {
            get; set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get; set;
        }

    }

}

