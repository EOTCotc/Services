using App.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Respon
{
    public class GetOrderByUserIdRespon : Order
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
    }
}
