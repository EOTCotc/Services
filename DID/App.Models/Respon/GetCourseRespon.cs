using App.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Respon
{
    public class GetCourseRespon : Course
    {
        /// <summary>
        /// 教师信息
        /// </summary>
        public List<Teacher>? Teachers
        {
            get; set;
        }
    }
}
