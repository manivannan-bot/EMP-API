using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PilotProjectAPI.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        
        
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        [Column("department_name")]
        public string DepartmentName { get; set; }
    }
}
