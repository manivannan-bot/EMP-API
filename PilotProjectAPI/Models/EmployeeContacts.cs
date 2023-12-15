using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PilotProjectAPI.Models
{
    public class EmployeeContacts
    {
        [Key]
        public int EmployeeContactId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [ForeignKey("EmployeeMaster")]
        public int EmployeeUniqueId { get; set; }

        public virtual EmployeeMaster EmployeeMaster { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string Relation { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(10)]
        public string Zip { get; set; }

        [MaxLength(300)]
        public string Address { get; set; }
    }
}
