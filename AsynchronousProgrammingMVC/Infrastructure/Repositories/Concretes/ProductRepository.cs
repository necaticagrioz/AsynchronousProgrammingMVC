using AsynchronousProgrammingMVC.Infrastructure.Context;
using AsynchronousProgrammingMVC.Infrastructure.Repositories.Interfaces;
using AsynchronousProgrammingMVC.Models.Entities.Concrete;

namespace AsynchronousProgrammingMVC.Infrastructure.Repositories.Concretes
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
