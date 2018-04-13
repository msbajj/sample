using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Course
    {
        //你能自行指定主键，而不是让数据库自动指定主键。
        //主键值由用户提供，而不是由数据库生成
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "课程编号")]
        public int CourseID { get; set; }

        [Display(Name = "名称")]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "学分")]
        [Range(0,5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }

        [Display(Name = "院系")]
        public DePartment DePartment { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }

    }
}