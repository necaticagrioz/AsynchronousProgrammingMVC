using AsynchronousProgrammingMVC.Models.Entities.Concrete;

namespace AsynchronousProgrammingMVC.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        List<Category> GetCategories();
    }
}
