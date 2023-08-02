using AsynchronousProgrammingMVC.Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace AsynchronousProgrammingMVC.Models.Entities.Concrete
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
