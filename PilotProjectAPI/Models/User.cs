using System.ComponentModel.DataAnnotations;

namespace PilotProjectAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(100)]

        public string LoginId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        
    }
}
