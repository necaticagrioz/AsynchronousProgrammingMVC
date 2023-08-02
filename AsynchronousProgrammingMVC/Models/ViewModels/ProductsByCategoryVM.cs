using AsynchronousProgrammingMVC.Models.Entities.Abstract;
using AsynchronousProgrammingMVC.Models.Entities.Concrete;

namespace AsynchronousProgrammingMVC.Models.ViewModels
{
    public class ProductsByCategoryVM
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? CategoryName { get; set; }
        public List<Category> Categories { get; set; }
    }
}
