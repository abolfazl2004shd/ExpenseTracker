using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Data.Models;
using ExpenseTracker.ViewModels;
using Data.Repositories;

namespace ExpenseTracker.Controllers
{
    public class TransactionController(ExpenseTrackerDbContext context, ICategoryService categoryService) : Controller // Fixed class declaration
    {
        private readonly ExpenseTrackerDbContext _context = context;
        private readonly ICategoryService _categoryService = categoryService;


        [Route("/")]
        public async Task<IActionResult> Index()
        {
            ViewData["Categories"] = _categoryService.AllCategories();
            var expenseFilter = new TransactionFilterViewModel
            {
                Transactions = await _context.Transactions.Include(transaction => transaction.Category).ToListAsync()
            };

            return View(viewName: nameof(Index), model: expenseFilter);
        }

        // GET: Transaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(viewName: nameof(Details), model: transaction);
        }

        // GET: Transaction/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Title");
            return View();
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,CategoryId,Note,Amount,Date")] Transaction transaction)
        {
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Icon", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,CategoryId,Note,Amount,Date")] Transaction transaction)
        {
            _context.Update(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }

        [HttpPost]
        public IActionResult Filter(TransactionFilterViewModel model)
        {
            var transactions = _context.Transactions.Include(t => t.Category).AsQueryable();

            if (model.StartDate.HasValue)
                transactions = transactions.Where(t => t.Date >= model.StartDate.Value);

            if (model.EndDate.HasValue)
                transactions = transactions.Where(t => t.Date <= model.EndDate.Value);

            if (!string.IsNullOrEmpty(model.CategoryTitle))
            {
                model.CategoryTitle = model.CategoryTitle.Trim();
                transactions = transactions.Where(t => t.Category.Title == model.CategoryTitle);
            }

            if (!string.IsNullOrEmpty(model.Type))
                transactions = transactions.Where(t => t.Category.Type == model.Type);

            model.Transactions = transactions.ToList();

            // Re-populate categories so they show up in the view dropdown
            ViewData["Categories"] = _categoryService.AllCategories();

            return View(nameof(Index), model);
        }


    }
}
