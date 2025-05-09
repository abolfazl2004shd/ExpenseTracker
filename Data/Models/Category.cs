using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Icon { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;    
    }
}
