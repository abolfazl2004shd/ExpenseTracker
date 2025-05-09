using Data.Models;

namespace ExpenseTracker.ViewModels
{
    public class TransactionFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CategoryTitle { get; set; }
        public string Type { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
