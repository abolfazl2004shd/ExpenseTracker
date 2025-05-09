using Data.Repositories;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    public class CategoryService(ExpenseTrackerDbContext context) : ICategoryService
    {
        private readonly ExpenseTrackerDbContext _context = context;
        public List<Category> AllCategories()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }
    }
}
