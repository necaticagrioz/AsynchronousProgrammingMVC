using AsynchronousProgrammingMVC.Infrastructure.Context;
using AsynchronousProgrammingMVC.Infrastructure.Repositories.Interfaces;
using AsynchronousProgrammingMVC.Models.Entities.Concrete;

namespace AsynchronousProgrammingMVC.Infrastructure.Repositories.Concretes
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Category> GetCategories()
        {
            var categories = _context.Categories.Where(x => x.Status != Models.Entities.Abstract.Status.Passive).ToList();
            return categories;
        }
    }
}
