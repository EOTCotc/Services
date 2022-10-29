using App.Entity;
using DID.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Respon
{
    public class GetOrderRespon : Order
    {
        /// <summary>
        /// 禅论系统信息
        /// </summary>
        public CLSystem? CLSystem
        {
            get; set;
        }
        /// <summary>
        /// 课程信息
        /// </summary>
        public Course? Course
        {
            get; set;
        }
        /// <summary>
        /// 自愿者信息
        /// </summary>
        public Volunteer? Volunteer
        {
            get; set;
        }
        /// <summary>
        /// 创建用户信息
        /// </summary>
        //public DIDUser? DIDUser
        //{
        //    get; set;
        //}
    }
}
