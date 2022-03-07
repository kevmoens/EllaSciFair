using System.ComponentModel.DataAnnotations;

namespace EllaSciFair.Data
{
    public class TakeANumber
    {
        [Key]
        public Guid? Id { get; set; }
        [Required]
        public int? CurrentNumber { get; set; }
    }
}
