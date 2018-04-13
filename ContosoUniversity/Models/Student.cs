using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }

        //必填字段
        [Required(ErrorMessage= "必填字段")]
        [Display(Name = "用户ID")]
        [StringLength(20, ErrorMessage = "ID不能超过20字。")]
        public string user_id { get; set; }

        [Required(ErrorMessage = "必填字段")]
        [Display(Name = "姓名")]
        [StringLength(10,ErrorMessage = "姓名不能超过10字。")]
        //数据库中存储的列名
        [Column("user_name")]
        public string name { get; set; }

        [Display(Name = "招生日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
