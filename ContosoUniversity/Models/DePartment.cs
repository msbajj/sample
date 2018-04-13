using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class DePartment
    {
        public  int DepartmentID { get; set; }

        [Display(Name ="部门名称")]
        [StringLength(50,MinimumLength =3)]
        public string Name { get; set; }

        [Display(Name ="预算")]
        [DataType(DataType.Currency)]
        [Column(TypeName ="money")]//映射成数据库的货币类型
        public decimal Budget { get; set; }

        [Display(Name ="开始时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Instructor Administrator { get; set; }
        public ICollection<Course> Courses { get; set; }

    }
}