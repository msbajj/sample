using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class OfficeAssignment
    {
        [Key]//教练表外键，办公室表主键
        /*办公室分配仅与分配有办公室的讲师相关，因此其主键也是 Instructor 实体的外键。 
         * 但 Entity Framework 无法自动识别 InstructorID 作为此实体的主键，因为其名称不符合 ID 或 classnameID 命名约定。
         * 因此，Key 特性用于将其识别为主键：*/
        public int InstructorID { get; set; }

        [StringLength(50)]
        [Display(Name = "办公室")]
        public string Location { get; set; }

        [Required]//可以不用，因为InstructorID外键（办公室表的主键）不能为空
        public Instructor Instructor { get; set; }
    }
}