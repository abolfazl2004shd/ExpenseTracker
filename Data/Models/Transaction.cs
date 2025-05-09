using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Note { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; } = DateTime.Now;


        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value")]
        public decimal Amount { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
