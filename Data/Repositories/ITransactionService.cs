using Data.Models;

namespace Data.Repositories
{
    public interface ITransactionService
    {
        List<Transaction> AllTransactions();
    }
}
