using Data.Repositories;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    public class TransactionService(ExpenseTrackerDbContext context) : ITransactionService
    {
        private readonly ExpenseTrackerDbContext _context = context;

        public List<Transaction> AllTransactions()
        {
            var transactions = _context.Transactions
                                 .Include(t => t.Category)
                                 .ToList();
            return transactions;
        }
    }
}
