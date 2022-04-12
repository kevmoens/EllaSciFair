using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllaSciFair.Data
{
    [Table("SignUp")]
    public class SignUp
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? FileName { get; set; }
        public bool IsPublic { get; set; } = false;
    }
}
