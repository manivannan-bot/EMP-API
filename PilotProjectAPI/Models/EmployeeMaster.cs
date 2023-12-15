using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PilotProjectAPI.Models
{
    public class EmployeeMaster
    {
        [Key]
        [Column("employee_unique_id")]
        public int EmployeeUniqueId { get; set; }

        
        public DateTime createdon { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(20)]

        [Column("employee_id")]
        public string EmployeeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(1)]
        [RegularExpression("^[MFmf]$")]
        public string Gender { get; set; }

        public DateTime? Dob { get; set; }

        [ForeignKey("Department")]
        [Column("departmentid")]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        [Column("hired_date")]
        public DateTime? HiredDate { get; set; }

        [Column("department_name")]
        public string DepartmentName { get;  set; }
    }
}
