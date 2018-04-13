using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A,B,C,D,F
    }
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }

        //?表示 Grade 属性可以为 null。 评级为 null 和评级为零是有区别的 
        //--null 意味着评级未知或者尚未分配。
        [Display(Name = "级别")]
        [DisplayFormat(NullDisplayText ="未评级")]
        public Grade? Grade { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}